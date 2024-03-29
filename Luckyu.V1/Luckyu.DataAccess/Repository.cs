﻿using Luckyu.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Luckyu.DataAccess
{
    public class Repository
    {
        public SqlSugarClient db
        {
            get
            {
                return BaseConnection.db;
            }
        }

        #region 事务
        public Repository BeginTrans()
        {
            db.BeginTran();
            return this;
        }
        public void Commit()
        {
            db.CommitTran();
        }
        public void Rollback()
        {
            db.RollbackTran();
        }
        #endregion

        #region 增删改
        /// <summary>
        /// 验证数据库字段长度是否够, 防止 截断字符串异常,
        /// 验证实体 时一并处理  字段string NULL => 空字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        private T VerifyDbColumn<T>(T entity, Expression<Func<T, object>> ignoreColumns1 = null, List<string> ignoreColumns2 = null) where T : class, new()
        {
            // 验证字段长度是否超长
            var entityInfo = db.EntityMaintenance.GetEntityInfo<T>();
            if (entityInfo == null)
            {
                throw new Exception("实体没有对应的数据表");
            }
            var tableName = entityInfo.DbTableName;
            var dbColumns = db.DbMaintenance.GetColumnInfosByTableName(tableName);
            var ignoreColumnName = new List<string>();
            if (ignoreColumns1 != null)
            {
                var properties = ReferencedPropertyFinder.GetExpProperties(ignoreColumns1);
                if (properties != null && properties.Count > 0)
                {
                    ignoreColumnName.AddRange(properties.Select(r => r.Name.ToUpper()).ToList());
                }
            }
            if (ignoreColumns2 != null)
            {
                ignoreColumnName.AddRange(ignoreColumns2.Select(r => r.ToUpper()).ToList());
            }
            foreach (var col in entityInfo.Columns)
            {
                if (col.DbColumnName.IsEmpty())
                {
                    continue;
                }
                if (col.PropertyInfo.PropertyType != typeof(string))
                {
                    continue;
                }

                var value = col.PropertyInfo.GetValue(entity);  // string  null 转为 空字符串
                if (value == null)
                {
                    col.PropertyInfo.SetValue(entity, "");
                }
                if (ignoreColumnName.Contains(col.DbColumnName.ToUpper()))
                {
                    continue;
                }
                var dbCol = dbColumns.Where(r => r.DbColumnName.ToUpper() == col.DbColumnName).FirstOrDefault();
                // 目前类型仅为varchar nvarchar,不能简单判断 col.PropertyInfo.PropertyType == typeod(string) 
                // 经测试数据库类型为text时,dbCol.Length有一个迷之长度16,实际数据库并没有长度限制,导致异常,不确定其他类型有没有,故先不做判断
                if (dbCol != null)
                {
                    var strValue = (value ?? "").ToString();
                    var valueLength = 0;
                    switch (dbCol.DataType)
                    {
                        case "varchar": valueLength = strValue.GetASCIILength(); break;
                        case "nvarchar": valueLength = strValue.Length; break;
                    }
                    if (dbCol.Length > 0 && valueLength > 0 && valueLength > dbCol.Length)
                    {
                        throw new Exception($"【{(col.DbTableName + " " + col.PropertyName + " " + col.ColumnDescription)}】字段长度过长，数据库长度为{ dbCol.Length}，实际字段长度为{valueLength}，请验证输入，或联系管理员增加数据库字段长度");
                    }
                }
            }
            return entity;
        }

        /// <summary>
        /// 插入实体数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public int Insert<T>(T entity) where T : class, new()
        {
            VerifyDbColumn(entity);
            var result = 0;
            try
            {
                result = db.Insertable(entity).ExecuteCommand();
                InsertExtensionTable<T>(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int Insert<T>(List<T> list) where T : class, new()
        {
            foreach (var entity in list)
            {
                VerifyDbColumn(entity);
            }
            var result = 0;
            try
            {
                result = db.Insertable(list).ExecuteCommand();
                InsertExtensionTableList<T>(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 主键存在修改 不存在新增
        /// </summary>
        public int InsertOrUpdate<T>(List<T> list) where T : class, new()
        {
            foreach (var entity in list)
            {
                VerifyDbColumn(entity);
            }
            var result = 0;
            try
            {
                foreach (var item in list)
                {
                    if (Exist(item))
                    {
                        Update(item);
                    }
                    else
                    {
                        Insert(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;

        }
        public int Delete<T>(T entity) where T : class, new()
        {
            var result = 0;
            try
            {
                result = db.Deleteable<T>().Where(entity).ExecuteCommand();
                DeleteExtensionTable<T>(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        public int Delete<T>(List<T> list) where T : class, new()
        {
            var result = 0;
            try
            {
                result = db.Deleteable<T>().Where(list).ExecuteCommand();
                DeleteExtensionTableList<T>(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public int Delete<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            var result = 0;
            try
            {
                var list = db.Queryable<T>().Where(condition).ToList();
                result = db.Deleteable<T>().Where(list).ExecuteCommand();
                DeleteExtensionTableList<T>(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 仅更新表中某些字段, 并且不会操作扩展表 一般用于逻辑删除等 没有做字段长度验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="onlyUpdateColumns"></param>
        /// <returns></returns>
        public int UpdateOnlyColumns<T>(T entity, Expression<Func<T, object>> onlyUpdateColumns) where T : class, new()
        {
            var result = 0;
            try
            {
                var query = db.Updateable<T>(entity).UpdateColumns(onlyUpdateColumns);
                result = query.ExecuteCommand();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 更新表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体</param>
        /// <param name="ignoreColumns">忽略的字段</param>
        /// <returns></returns>
        public int Update<T>(T entity, Expression<Func<T, object>> ignoreColumns = null) where T : class, new()
        {
            VerifyDbColumn(entity, ignoreColumns, null);
            var result = 0;
            try
            {
                var query = db.Updateable<T>(entity);
                if (ignoreColumns != null)
                {
                    query = query.IgnoreColumns(ignoreColumns);
                }
                result = query.ExecuteCommand();
                UpdateExtensionTable<T>(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 更新表 自动比对json json中没有的字段自动忽略
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="json"></param>
        public int Update<T>(T entity, string json) where T : class, new()
        {
            var result = 0;
            try
            {
                var query = db.Updateable<T>(entity);
                var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                var igCols = new List<string>();
                var flag = true; // 是否更新，如果忽略字段为除主键外全部，则不更新，否则报 update table set where id = @Id 无更新列sql语法错误
                if (dict != null && dict.Count > 0)
                {
                    var entityInfo = db.EntityMaintenance.GetEntityInfo<T>();
                    igCols = entityInfo.Columns.Where(r => !r.IsPrimarykey && !dict.Keys.Contains(r.PropertyName)).Select(r => r.PropertyName).ToList();
                    if (!igCols.IsEmpty())
                    {
                        query = query.IgnoreColumns(igCols.ToArray());
                    }
                    if (igCols.Count >= entityInfo.Columns.Count - 1)
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    VerifyDbColumn(entity, null, igCols);

                    result = query.ExecuteCommand();
                    UpdateExtensionTable<T>(entity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// 更新表 自动比对json json中没有的字段自动忽略
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="json"></param>
        /// <param name="ignoreColumns">更多要排除字段</param>
        /// <returns></returns>
        public int UpdateIgnoreColumns<T>(T entity, string json, Expression<Func<T, object>> ignoreColumns) where T : class, new()
        {
            var result = 0;
            try
            {
                var query = db.Updateable(entity);
                var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                var igCols = new List<string>();
                if (dict != null && dict.Count > 0)
                {
                    var entityInfo = db.EntityMaintenance.GetEntityInfo<T>();
                    igCols = entityInfo.Columns.Where(r => !r.IsPrimarykey && !dict.Keys.Contains(r.PropertyName)).Select(r => r.PropertyName).ToList();
                    if (!igCols.IsEmpty())
                    {
                        query = query.IgnoreColumns(igCols.ToArray());
                    }
                }
                if (ignoreColumns != null)
                {
                    query = query.IgnoreColumns(ignoreColumns);
                }
                VerifyDbColumn(entity, ignoreColumns, igCols);

                result = query.ExecuteCommand();
                UpdateExtensionTable<T>(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 更新表 自动比对json json中没有的字段自动忽略
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="json"></param>
        /// <param name="appendColumns">需要更新的字段(json中没有的,但依旧需要更新的)</param>
        /// <returns></returns>
        public int UpdateAppendColumns<T>(T entity, string json, Expression<Func<T, object>> appendColumns) where T : class, new()
        {
            var result = 0;
            try
            {
                var query = db.Updateable(entity);
                var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                var igCols = new List<string>();
                if (dict != null && dict.Count > 0)
                {
                    var entityInfo = db.EntityMaintenance.GetEntityInfo<T>();
                    igCols = entityInfo.Columns.Where(r => !r.IsPrimarykey && !dict.Keys.Contains(r.PropertyName)).Select(r => r.PropertyName).ToList();
                    if (appendColumns != null)
                    {
                        var properties = ReferencedPropertyFinder.GetExpProperties(appendColumns);
                        if (properties != null && properties.Count > 0)
                        {
                            var listField = properties.Select(r => r.Name).ToList();
                            igCols = igCols.Where(r => !listField.Contains(r)).ToList();
                        }
                    }
                    if (!igCols.IsEmpty())
                    {
                        query = query.IgnoreColumns(igCols.ToArray());
                    }
                }
                VerifyDbColumn(entity, null, igCols);

                result = query.ExecuteCommand();
                UpdateExtensionTable<T>(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        public int UpdateAppendColumns<T>(T entity, string json, List<string> appendColumns) where T : class, new()
        {
            var result = 0;
            try
            {
                var query = db.Updateable(entity);
                var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                var igCols = new List<string>();
                if (dict != null && dict.Count > 0)
                {
                    var entityInfo = db.EntityMaintenance.GetEntityInfo<T>();
                    igCols = entityInfo.Columns.Where(r => !r.IsPrimarykey && !dict.Keys.Contains(r.PropertyName)).Select(r => r.PropertyName).ToList();
                    if (!appendColumns.IsEmpty())
                    {
                        igCols = igCols.Where(r => !appendColumns.Contains(r)).ToList();
                    }
                    if (!igCols.IsEmpty())
                    {
                        query = query.IgnoreColumns(igCols.ToArray());
                    }
                }
                VerifyDbColumn(entity, null, igCols);

                result = query.ExecuteCommand();
                UpdateExtensionTable<T>(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 更新表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="json"></param>
        /// <param name="appendColumns">需要更新的字段(json中没有的,但依旧需要更新的)</param>
        /// <param name="ignoreColumns">更多要排除字段</param>
        /// <returns></returns>
        public int Update<T>(T entity, string json, Expression<Func<T, object>> appendColumns, Expression<Func<T, object>> ignoreColumns) where T : class, new()
        {
            var result = 0;
            try
            {
                var query = db.Updateable<T>(entity);
                var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                var igCols = new List<string>();
                if (dict != null && dict.Count > 0)
                {
                    var entityInfo = db.EntityMaintenance.GetEntityInfo<T>();
                    igCols = entityInfo.Columns.Where(r => !r.IsPrimarykey && !dict.Keys.Contains(r.PropertyName)).Select(r => r.PropertyName).ToList();
                    if (appendColumns != null)
                    {
                        var properties = ReferencedPropertyFinder.GetExpProperties(appendColumns);
                        if (properties != null && properties.Count > 0)
                        {
                            var listField = properties.Select(r => r.Name).ToList();
                            igCols = igCols.Where(r => !listField.Contains(r)).ToList();
                        }
                    }

                    if (!igCols.IsEmpty())
                    {
                        query = query.IgnoreColumns(igCols.ToArray());
                    }
                }
                if (ignoreColumns != null)
                {
                    query = query.IgnoreColumns(ignoreColumns);
                }
                VerifyDbColumn(entity, null, igCols);

                result = query.ExecuteCommand();
                UpdateExtensionTable<T>(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion

        #region 扩展表辅助方法
        /// <summary>
        /// 扩展表插入
        /// </summary>
        private int InsertExtensionTable<T>(T entity)
        {
            var type = typeof(T);
            if (!type.IsSubclassOf(typeof(ExtensionEntityBase))) // 没有扩展表
            {
                return 0;
            }
            var entityInfo = db.EntityMaintenance.GetEntityInfo<T>();
            if (entityInfo == null)
            {
                return 0;
            }
            var tableName = entityInfo.DbTableName;
            var tableNameEx = tableName + "_extension";
            // 获取扩展表的字段
            var colsEx = db.DbMaintenance.GetColumnInfosByTableName(tableNameEx);
            if (colsEx == null || colsEx.Count < 1)
            {
                return 0;
            }
            var pk = entityInfo.Columns.Where(r => r.IsPrimarykey).FirstOrDefault();
            var pkName = pk.DbColumnName.ToUpper();
            var keyValue = type.GetProperties().First(n => n.Name.ToUpper() == pkName).GetValue(entity);
            JObject extJObject = (JObject)entity.GetType().GetProperties().First(n => n.Name == "ExtObject").GetValue(entity);
            if (extJObject == null)
            {
                return 0;
            }
            extJObject.Add(pkName, keyValue.ToString());
            InsertExtSql(tableNameEx, colsEx, extJObject);

            return 1;
        }
        private int InsertExtensionTableList<T>(List<T> list)
        {
            var type = typeof(T);
            if (!type.IsSubclassOf(typeof(ExtensionEntityBase))) // 没有扩展表
            {
                return 0;
            }
            var entityInfo = db.EntityMaintenance.GetEntityInfo<T>();
            if (entityInfo == null)
            {
                return 0;
            }
            var tableName = entityInfo.DbTableName;
            var tableNameEx = tableName + "_extension";
            // 获取当前表的字段
            var colsEx = db.DbMaintenance.GetColumnInfosByTableName(tableNameEx);
            if (colsEx == null || colsEx.Count < 1)
            {
                return 0;
            }
            var pk = entityInfo.Columns.Where(r => r.IsPrimarykey).FirstOrDefault();
            var pkName = pk.DbColumnName.ToUpper();

            var result = 0;
            foreach (var entity in list)
            {
                var keyValue = type.GetProperties().First(n => n.Name.ToUpper() == pkName).GetValue(entity);

                JObject extJObject = (JObject)entity.GetType().GetProperties().First(n => n.Name == "ExtObject").GetValue(entity);

                if (extJObject == null)
                {
                    continue;
                }
                extJObject.Add(pkName, keyValue.ToString());
                result += InsertExtSql(tableNameEx, colsEx, extJObject);
            }
            return result;
        }
        private int InsertExtSql(string tableName, List<DbColumnInfo> cols, JObject formDataJson)
        {
            var strName = new StringBuilder();
            var sqlValue = new StringBuilder();

            var args = new List<SugarParameter>();
            foreach (var field in formDataJson)
            {
                var upper = field.Key.ToUpper();
                var col = cols.Where(r => r.DbColumnName.ToUpper() == upper).FirstOrDefault();
                if (col != null && !field.Value.IsEmpty())
                {
                    strName.Append($"{upper},");
                    sqlValue.Append($" {BaseConnection.ParaPre}{upper},");

                    args.Add(new SugarParameter(upper, field.Value));
                }
            }
            var sql = $"INSERT INTO {tableName} ( {strName.ToString().TrimEnd(',')} ) VALUES ( {sqlValue.ToString().TrimEnd(',')} )";
            return db.Ado.ExecuteCommand(sql, args);
        }

        /// <summary>
        /// 扩展表更新
        /// </summary>
        private int UpdateExtensionTable<T>(T entity)
        {
            var type = typeof(T);
            if (!type.IsSubclassOf(typeof(ExtensionEntityBase))) // 没有扩展表
            {
                return 0;
            }
            var entityInfo = db.EntityMaintenance.GetEntityInfo<T>();
            if (entityInfo == null)
            {
                return 0;
            }
            var tableName = entityInfo.DbTableName;
            var tableNameEx = tableName + "_extension";
            // 获取扩展表的字段
            var colsEx = db.DbMaintenance.GetColumnInfosByTableName(tableNameEx);
            if (colsEx == null || colsEx.Count < 1)
            {
                return 0;
            }
            // 主键 字段
            var pk = entityInfo.Columns.Where(r => r.IsPrimarykey).FirstOrDefault();
            var pkName = pk.DbColumnName.ToUpper();
            var keyValue = type.GetProperties().First(n => n.Name.ToUpper() == pkName).GetValue(entity);

            JObject extJObject = (JObject)entity.GetType().GetProperties().First(n => n.Name == "ExtObject").GetValue(entity);
            if (extJObject == null)
            {
                return 0;
            }

            extJObject.Add(pkName, keyValue.ToString());

            var sqlCount = $"SELECT COUNT(1) FROM {tableNameEx} WHERE {pkName} = {BaseConnection.ParaPre}{pkName} ";
            var countExt = db.Ado.GetInt(sqlCount);
            if (countExt > 0)
            {
                UpdateExtSql(tableNameEx, colsEx, extJObject);
            }
            else
            {
                InsertExtSql(tableNameEx, colsEx, extJObject);
            }
            return 1;
        }
        private int UpdateExtSql(string tableName, List<DbColumnInfo> cols, JObject formDataJson)
        {
            var strSql = new StringBuilder($"UPDATE {tableName} SET ");
            var listValue = new List<string>();
            var args = new List<SugarParameter>();
            foreach (var field in formDataJson)
            {
                var upper = field.Key.ToUpper();
                var col = cols.Where(r => r.DbColumnName.ToUpper() == upper).FirstOrDefault();
                if (!col.IsPrimarykey)
                {
                    listValue.Add($"{upper} = @{upper}");
                    args.Add(new SugarParameter(upper, field.Value.ToString()));
                }
            }
            strSql.Append(string.Join(",", listValue));
            var pk = cols.Where(r => r.IsPrimarykey).FirstOrDefault();
            var strPkName = pk.DbColumnName.ToUpper();
            strSql.Append($" WHERE {strPkName} = {BaseConnection.ParaPre}{strPkName} ");
            args.Add(new SugarParameter(strPkName, formDataJson[strPkName].ToString()));
            return db.Ado.ExecuteCommand(strSql.ToString(), args);
        }

        /// <summary>
        /// 扩展表删除
        /// </summary>
        private int DeleteExtensionTable<T>(T entity)
        {
            var type = typeof(T);
            if (!type.IsSubclassOf(typeof(ExtensionEntityBase))) // 没有扩展表
            {
                return 0;
            }
            var entityInfo = db.EntityMaintenance.GetEntityInfo<T>();
            if (entityInfo == null)
            {
                return 0;
            }
            var tableName = entityInfo.DbTableName;
            var tableNameEx = tableName + "_extension";
            var colsEx = db.DbMaintenance.GetColumnInfosByTableName(tableNameEx);
            if (colsEx == null || colsEx.Count < 1)
            {
                return 0;
            }
            var pk = entityInfo.Columns.Where(r => r.IsPrimarykey).FirstOrDefault();
            var pkName = pk.DbColumnName.ToUpper();
            var keyValue = type.GetProperties().First(n => n.Name.ToUpper() == pkName).GetValue(entity);

            var sql = $"DELETE FROM {tableNameEx} WHERE {pkName} = {BaseConnection.ParaPre}{pkName} ";
            var parms = new SugarParameter(pkName, keyValue);
            return db.Ado.ExecuteCommand(sql, parms);
        }
        private int DeleteExtensionTableList<T>(List<T> list)
        {
            var type = typeof(T);
            if (!type.IsSubclassOf(typeof(ExtensionEntityBase))) // 没有扩展表
            {
                return 0;
            }
            var entityInfo = db.EntityMaintenance.GetEntityInfo<T>();
            if (entityInfo == null)
            {
                return 0;
            }
            var tableName = entityInfo.DbTableName;
            var tableNameEx = tableName + "_extension";
            var colsEx = db.DbMaintenance.GetColumnInfosByTableName(tableNameEx);
            if (colsEx == null || colsEx.Count < 1)
            {
                return 0;
            }
            var pk = entityInfo.Columns.Where(r => r.IsPrimarykey).FirstOrDefault();
            var pkName = pk.DbColumnName.ToUpper();

            var result = 0;
            foreach (var entity in list)
            {
                var keyValue = type.GetProperties().First(n => n.Name.ToUpper() == pkName).GetValue(entity);

                var sql = $"DELETE FROM {tableNameEx} WHERE {pkName} = {BaseConnection.ParaPre}{pkName} ";
                var parms = new SugarParameter(pkName, keyValue);
                result += db.Ado.ExecuteCommand(sql, parms);
            }
            return result;
        }

        /// <summary>
        /// 扩展表查询
        /// </summary>
        private void AttachExtObject<T>(ref T entity)
        {
            var type = typeof(T);
            if (!type.IsSubclassOf(typeof(ExtensionEntityBase))) // 没有扩展表
            {
                return;
            }
            var entityInfo = db.EntityMaintenance.GetEntityInfo<T>();
            if (entityInfo == null)
            {
                return;
            }
            var tableName = entityInfo.DbTableName;
            var tableNameEx = tableName + "_extension";
            var colsEx = db.DbMaintenance.GetColumnInfosByTableName(tableNameEx);
            if (colsEx == null || colsEx.Count < 1)
            {
                return;
            }
            var pk = entityInfo.Columns.Where(r => r.IsPrimarykey).FirstOrDefault();
            var pkName = pk.DbColumnName.ToUpper();
            var keyValue = type.GetProperties().First(n => n.Name.ToUpper() == pkName).GetValue(entity);

            var sql = $"SELECT * FROM {tableNameEx} WHERE {pkName} = {BaseConnection.ParaPre}{pkName} ";
            var parm = new SugarParameter(pkName, keyValue);
            var dt = db.Ado.GetDataTable(sql, parm);
            JObject extObject = new JObject();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                foreach (var column in colsEx)
                {
                    if (column.IsPrimarykey)
                    {
                        continue;
                    }
                    extObject.Add(column.DbColumnName, dr[column.DbColumnName].ToString());
                }
            }
            entity.GetType().GetProperties().First(n => n.Name == "ExtObject").SetValue(entity, extObject);
        }
        private void AttachExtList<T>(ref List<T> list)
        {
            var type = typeof(T);
            if (!type.IsSubclassOf(typeof(ExtensionEntityBase))) // 没有扩展表
            {
                return;
            }
            var entityInfo = db.EntityMaintenance.GetEntityInfo<T>();
            if (entityInfo == null)
            {
                return;
            }
            var tableName = entityInfo.DbTableName;
            var tableNameEx = tableName + "_extension";
            var colsEx = db.DbMaintenance.GetColumnInfosByTableName(tableNameEx);
            if (colsEx == null || colsEx.Count < 1)
            {
                return;
            }
            var pk = entityInfo.Columns.Where(r => r.IsPrimarykey).FirstOrDefault();
            var pkName = pk.DbColumnName.ToUpper();
            foreach (var entity in list)
            {
                var keyValue = type.GetProperties().First(n => n.Name.ToUpper() == pkName).GetValue(entity);

                var sql = $"SELECT * FROM {tableNameEx} WHERE {pkName} = @{pkName} ";
                var parm = new SugarParameter(pkName, keyValue);
                var dt = db.Ado.GetDataTable(sql, parm);
                JObject extObject = new JObject();
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    foreach (var column in colsEx)
                    {
                        if (column.IsPrimarykey)
                        {
                            continue;
                        }
                        extObject.Add(column.DbColumnName, dr[column.DbColumnName].ToString());
                    }
                }
                entity.GetType().GetProperties().First(n => n.Name == "ExtObject").SetValue(entity, extObject);
            }
        }

        #endregion

        #region   查询
        public bool Exist<T>(T entity) where T : class, new()
        {
            var entityInfo = db.EntityMaintenance.GetEntityInfo<T>();
            if (entityInfo == null)
            {
                return false;
            }
            var pk = entityInfo.Columns.Where(r => r.IsPrimarykey).First();
            var keyValue = pk.PropertyInfo.GetValue(entity).ToString();
            var pkName = pk.DbColumnName;
            var sql = $"SELECT COUNT(1) FROM {entityInfo.DbTableName} WHERE {pk.DbColumnName} = @{pk.DbColumnName}";
            var parm = new SugarParameter(pk.DbColumnName, keyValue);
            var result = db.Ado.GetInt(sql, parm);
            return result > 0;
        }

        public int Count<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            var count = db.Queryable<T>().Where(condition).Count();
            return (int)count;
        }

        #region GetEntity
        /// <summary>
        /// 查找一个实体根据主键
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="KeyValue">主键</param>
        /// <returns></returns>
        public T GetEntity<T>(string keyValue) where T : class, new()
        {
            var entityInfo = db.EntityMaintenance.GetEntityInfo<T>();
            if (entityInfo == null)
            {
                return null;
            }
            var pk = entityInfo.Columns.Where(r => r.IsPrimarykey).First();
            var sql = $"SELECT * FROM {entityInfo.DbTableName} WHERE {pk.DbColumnName} = {BaseConnection.ParaPre}{pk.DbColumnName}";
            var parm = new SugarParameter(pk.DbColumnName, keyValue);
            var entity = db.Ado.SqlQuerySingle<T>(sql, parm);
            if (entity != null)
            {
                AttachExtObject<T>(ref entity);
            }
            return entity;
        }

        /// <summary>
        /// 查找一个实体（根据表达式）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="condition">表达式</param>
        /// <returns></returns>
        public T GetEntity<T>(Expression<Func<T, bool>> condition, string orderby = "") where T : class, new()
        {
            var query = db.Queryable<T>().Where(condition);
            if (!string.IsNullOrEmpty(orderby))
            {
                query = query.OrderBy(orderby);
            }
            var entity = query.First();
            if (entity != null)
            {
                AttachExtObject<T>(ref entity);
            }
            return entity;
        }
        public T GetEntity<T>(Expression<Func<T, bool>> condition, Expression<Func<T, object>> orderby = null, bool isDesc = false) where T : class, new()
        {
            var query = db.Queryable<T>().Where(condition);
            if (orderby != null)
            {
                if (isDesc)
                {
                    query = query.OrderBy(orderby, OrderByType.Desc);
                }
                else
                {
                    query = query.OrderBy(orderby, OrderByType.Asc);
                }
            }
            var entity = query.First();
            if (entity != null)
            {
                AttachExtObject<T>(ref entity);
            }
            return entity;
        }
        /// <summary>
        /// 查找一个实体（根据sql）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="strSql">sql语句</param>
        /// <param name="dbParameter">参数</param>
        /// <returns></returns>
        public T GetEntity<T>(string strSql, object dbParameter = null) where T : class, new()
        {
            var entity = db.Queryable<T>().Where(strSql, dbParameter).First();
            if (entity != null)
            {
                AttachExtObject<T>(ref entity);
            }
            return entity;
        }
        #endregion

        #region DataTable
        public DataTable GetDataTable(string sql, List<SugarParameter> dbParameter)
        {
            return db.Ado.GetDataTable(sql, dbParameter);
        }

        public DataTable GetDataTable(string sql, object paras = null)
        {
            return db.Ado.GetDataTable(sql, paras);
        }

        public JqgridDatatablePageResponse GetDataTablePage(JqgridPageRequest pagination, string strSql, List<SugarParameter> dbParameter = null)
        {
            var dbType = db.CurrentConnectionConfig.DbType;
            strSql = strSql.Trim().TrimEnd(';');
            if (!string.IsNullOrEmpty(pagination.sidx))
            {
                strSql += $" ORDER BY {pagination.sidx} {pagination.sord}";
            }
            var pageIndex = pagination.page < 1 ? 1 : pagination.page;
            var pageSize = pagination.rows < 1 ? 30 : pagination.rows;

            var sqlParts = SQLPartsHelper.SplitSQL(strSql);
            var countSql = SQLPartsHelper.GetCountSql(sqlParts);
            var pageSql = SQLPartsHelper.GetPageSql(sqlParts, pageIndex, pageSize, dbType);
            var newParams = new List<SugarParameter>();
            if (dbParameter != null)
            {
                foreach (var param in dbParameter)
                {
                    newParams.Add(new SugarParameter(param.ParameterName, param.Value, param.DbType));
                }
            }
            var total = db.Ado.GetInt(countSql, dbParameter);  // 执行之后dbParameter的value会被清空，所以前面复制出来一份，感觉是orm的bug，先这样吧
            //var list = new List<T>();
            DataTable list = null;
            if (total > 0)
            {
                list = db.Ado.GetDataTable(pageSql, newParams);
            }
            var page = new JqgridDatatablePageResponse
            {
                count = pagination.rows,
                page = pagination.page,
                records = total,
                rows = list,
            };
            return page;
        }

        #endregion

        #region GetList
        /// <summary>
        /// 查询列表根据sql语句(带参数)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="strSql">sql语句</param>
        /// <param name="dbParameter">参数</param>
        /// <returns></returns>
        public List<T> GetList<T>(string strSql, object dbParameter = null) where T : class
        {
            var list = db.Ado.SqlQuery<T>(strSql, dbParameter);
            if (list != null && list.Count > 0)
            {
                AttachExtList<T>(ref list);
            }
            return list;
        }
        /// <summary>
        /// 查询数据
        /// </summary
        /// <typeparam name="T">类型</typeparam>
        /// <param name="condition">表达式</param>
        /// <returns></returns>
        public List<T> GetList<T>(Expression<Func<T, bool>> condition, string orderby = "") where T : class, new()
        {
            var query = db.Queryable<T>().Where(condition);
            if (!string.IsNullOrEmpty(orderby))
            {
                query = query.OrderBy(orderby);
            }
            var list = query.ToList();
            if (list != null && list.Count > 0)
            {
                AttachExtList<T>(ref list);
            }
            return list;
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="condition">表达式</param>
        /// <returns></returns>
        public List<T> GetList<T>(JqgridPageRequest jqPage, Expression<Func<T, bool>> condition) where T : class, new()
        {
            var query = db.Queryable<T>().Where(condition);
            var filters = SearchConditionHelper.ContructJQCondition(jqPage);
            if (!filters.IsEmpty())
            {
                query = query.Where(filters);
            }
            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord}");
            }
            var list = query.ToList();
            if (list != null && list.Count > 0)
            {
                AttachExtList<T>(ref list);
            }
            return list;
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="condition">表达式</param>
        /// <returns></returns>
        public List<T> GetList<T>(JqgridPageRequest jqPage, Expression<Func<T, bool>> condition, List<IConditionalModel> filters) where T : class, new()
        {
            var query = db.Queryable<T>().Where(condition);
            if (!filters.IsEmpty())
            {
                query = query.Where(filters);
            }
            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord}");
            }
            var list = query.ToList();
            if (list != null && list.Count > 0)
            {
                AttachExtList<T>(ref list);
            }
            return list;
        }

        public List<T> GetList<T>(JqgridPageRequest jqPage, Expression<Func<T, bool>> condition, Dictionary<string, Func<List<ConditionalModel>>> dicCondition) where T : class, new()
        {
            var query = db.Queryable<T>().Where(condition);
            var filters = SearchConditionHelper.ContructJQCondition(jqPage);
            if (dicCondition.Count > 0)
            {
                foreach (var item in dicCondition)
                {
                    var conditions = dicCondition[item.Key]();
                    filters.AddRange(conditions);
                }
            }
            if (!filters.IsEmpty())
            {
                query = query.Where(filters);
            }
            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord}");
            }
            var list = query.ToList();
            if (list != null && list.Count > 0)
            {
                AttachExtList<T>(ref list);
            }
            return list;
        }

        public List<T> GetList<T>(Expression<Func<T, bool>> condition, Expression<Func<T, object>> orderby = null, bool isDesc = false) where T : class, new()
        {
            var query = db.Queryable<T>().Where(condition);
            if (orderby != null)
            {
                if (isDesc)
                {
                    query = query.OrderBy(orderby, OrderByType.Desc);
                }
                else
                {
                    query = query.OrderBy(orderby, OrderByType.Asc);
                }
            }
            var list = query.ToList();
            if (list != null && list.Count > 0)
            {
                AttachExtList<T>(ref list);
            }
            return list;
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="condition">表达式</param>
        /// <returns></returns>
        public List<T> GetList<T>(ISugarQueryable<T> query) where T : class, new()
        {
            var list = query.ToList();
            if (list != null && list.Count > 0)
            {
                AttachExtList<T>(ref list);
            }
            return list;
        }
        public List<T> GetList<T>(ISugarQueryable<T> query, List<IConditionalModel> filters) where T : class, new()
        {
            if (!filters.IsEmpty())
            {
                query = query.Where(filters);
            }
            var list = query.ToList();
            if (list != null && list.Count > 0)
            {
                AttachExtList<T>(ref list);
            }
            return list;
        }

        #endregion

        #region GetListTop
        /// <summary>
        /// 查询数据 取前几条
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="condition">表达式</param>
        /// <returns></returns>
        public List<T> GetListTop<T>(int top, Expression<Func<T, bool>> condition, Expression<Func<T, object>> orderby, bool isDesc = false) where T : class, new()
        {
            var query = db.Queryable<T>().Where(condition);
            if (orderby != null)
            {
                if (isDesc)
                {
                    query = query.OrderBy(orderby, OrderByType.Desc);
                }
                else
                {
                    query = query.OrderBy(orderby, OrderByType.Asc);
                }
            }
            var list = query.Take(top).ToList();
            if (list != null && list.Count > 0)
            {
                AttachExtList<T>(ref list);
            }
            return list;
        }
        /// <summary>
        /// 查询数据 取前几条
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="condition">表达式</param>
        /// <returns></returns>
        public List<T> GetListTop<T>(int top, Expression<Func<T, bool>> condition, string orderby = "") where T : class, new()
        {
            var query = db.Queryable<T>().Where(condition);
            if (!string.IsNullOrEmpty(orderby))
            {
                query = query.OrderBy(orderby);
            }
            var list = query.Take(top).ToList();
            if (list != null && list.Count > 0)
            {
                AttachExtList<T>(ref list);
            }
            return list;
        }

        #endregion

        #region GetEntityTop
        /// <summary>
        /// 根据排序取第几条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="top"></param>
        /// <param name="condition"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public T GetEntityTop<T>(int top, Expression<Func<T, bool>> condition, string orderby = "") where T : class, new()
        {
            var query = db.Queryable<T>().Where(condition);
            if (!string.IsNullOrEmpty(orderby))
            {
                query = query.OrderBy(orderby);
            }
            var entity = query.Skip(top - 1).Take(1).First();
            if (entity != null)
            {
                AttachExtObject<T>(ref entity);
            }
            return entity;
        }
        #endregion

        #region GetPage
        /// <summary>
        /// 分页
        /// </summary>
        public JqgridPageResponse<T> GetPage<T>(int pageIndex, int pageSize, Expression<Func<T, bool>> condition, string orderby) where T : class, new()
        {
            var query = db.Queryable<T>().Where(condition);
            if (!string.IsNullOrEmpty(orderby))
            {
                query = query.OrderBy(orderby);
            }
            var total = 0;
            var list = query.ToPageList(pageIndex, pageSize, ref total);
            var page = new JqgridPageResponse<T>
            {
                count = pageSize,
                page = pageIndex,
                records = total,
                rows = list,
            };
            return page;

        }
        public JqgridPageResponse<T> GetPage<T>(JqgridPageRequest jqPage, Expression<Func<T, bool>> condition) where T : class, new()
        {
            var query = db.Queryable<T>().Where(condition);
            var filters = SearchConditionHelper.ContructJQCondition(jqPage);
            if (!filters.IsEmpty())
            {
                query = query.Where(filters);
            }
            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord} ");
            }
            var total = 0;
            var list = query.ToPageList(jqPage.page, jqPage.rows, ref total);
            var page = new JqgridPageResponse<T>
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = total,
                rows = list,
            };
            return page;
        }
        public JqgridPageResponse<T> GetPage<T>(JqgridPageRequest jqPage, Expression<Func<T, bool>> condition, Expression<Func<T, T>> select) where T : class, new()
        {
            var query = db.Queryable<T>().Where(condition);
            var filters = SearchConditionHelper.ContructJQCondition(jqPage);
            if (!filters.IsEmpty())
            {
                query = query.Where(filters);
            }
            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord} ");
            }
            var total = 0;
            var list = query.ToPageList(jqPage.page, jqPage.rows, ref total);
            var page = new JqgridPageResponse<T>
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = total,
                rows = list,
            };
            return page;
        }
        public JqgridPageResponse<T> GetPage<T>(JqgridPageRequest jqPage, ISugarQueryable<T> query) where T : class, new()
        {
            var filters = SearchConditionHelper.ContructJQCondition(jqPage);
            if (!filters.IsEmpty())
            {
                query = query.Where(filters);
            }
            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord} ");
            }
            var total = 0;
            var list = query.ToPageList(jqPage.page, jqPage.rows, ref total);
            var page = new JqgridPageResponse<T>
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = total,
                rows = list,
            };
            return page;
        }
        public JqgridPageResponse<T> GetPage<T>(JqgridPageRequest jqPage, ISugarQueryable<T> query, List<IConditionalModel> filters) where T : class, new()
        {
            if (!filters.IsEmpty())
            {
                query = query.Where(filters);
            }
            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord} ");
            }
            var total = 0;
            var list = query.ToPageList(jqPage.page, jqPage.rows, ref total);
            var page = new JqgridPageResponse<T>
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = total,
                rows = list,
            };
            return page;
        }
        public JqgridPageResponse<T> GetPage<T>(JqgridPageRequest jqPage, List<IConditionalModel> filters) where T : class, new()
        {
            var query = db.Queryable<T>();
            if (!filters.IsEmpty())
            {
                query = query.Where(filters);
            }
            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord} ");
            }
            var total = 0;
            var list = query.ToPageList(jqPage.page, jqPage.rows, ref total);
            var page = new JqgridPageResponse<T>
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = total,
                rows = list,
            };
            return page;
        }
        public JqgridPageResponse<T> GetPage<T>(JqgridPageRequest jqPage, List<Expression<Func<T, bool>>> condition, List<IConditionalModel> filters) where T : class, new()
        {
            var query = db.Queryable<T>();
            if (condition.Count > 0)
            {
                foreach (var item in condition)
                {
                    query = query.Where(item);
                }
            }
            if (!filters.IsEmpty())
            {
                query = query.Where(filters);
            }
            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord} ");
            }
            var total = 0;
            var list = query.ToPageList(jqPage.page, jqPage.rows, ref total);
            var page = new JqgridPageResponse<T>
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = total,
                rows = list,
            };
            return page;
        }
        public JqgridPageResponse<T> GetPage<T>(JqgridPageRequest jqPage, Expression<Func<T, bool>> condition, List<IConditionalModel> filters) where T : class, new()
        {
            var query = db.Queryable<T>();
            if (condition != null)
            {
                query = query.Where(condition);
            }
            if (!filters.IsEmpty())
            {
                query = query.Where(filters);
            }
            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord} ");
            }
            var total = 0;
            var list = query.ToPageList(jqPage.page, jqPage.rows, ref total);
            var page = new JqgridPageResponse<T>
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = total,
                rows = list,
            };
            return page;
        }
        public JqgridPageResponse<T> GetPage<T>(JqgridPageRequest jqPage, Expression<Func<T, bool>> condition, Dictionary<string, Func<string, string, List<IConditionalModel>>> dicModels) where T : class, new()
        {
            var query = db.Queryable<T>();
            if (condition != null)
            {
                query = query.Where(condition);
            }
            var filters = SearchConditionHelper.ContructJQCondition(jqPage);
            if (jqPage.isSearch && dicModels.Count > 0)
            {
                var rules = jqPage.fitersObj.rules;
                foreach (var item in dicModels)
                {
                    var rule = rules.Where(r => r.field == item.Key).FirstOrDefault();
                    if (rule != null)
                    {
                        var comditions = dicModels[rule.field](rule.field, rule.data);
                        if (comditions != null)
                        {
                            filters.AddRange(comditions);
                        }
                    }
                }
            }
            if (!filters.IsEmpty())
            {
                query = query.Where(filters);
            }
            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord} ");
            }
            var total = 0;
            var list = query.ToPageList(jqPage.page, jqPage.rows, ref total);
            var page = new JqgridPageResponse<T>
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = total,
                rows = list,
            };

            return page;
        }
        /// <summary>
        /// 查询列表(分页)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="strSql">SQL语句</param>
        /// <param name="dbParameter">参数</param>
        /// <param name="jqPage">分页数据</param>
        /// <returns></returns>
        public JqgridPageResponse<T> GetPage<T>(JqgridPageRequest jqPage, string strSql, List<SugarParameter> dbParameter = null) where T : class, new()
        {
            var dbType = db.CurrentConnectionConfig.DbType;
            strSql = strSql.Trim().TrimEnd(';');
            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                strSql += $" ORDER BY {jqPage.sidx} {jqPage.sord}";
            }
            var pageIndex = jqPage.page < 1 ? 1 : jqPage.page;
            var pageSize = jqPage.rows < 1 ? 30 : jqPage.rows;

            var sqlParts = SQLPartsHelper.SplitSQL(strSql);
            var countSql = SQLPartsHelper.GetCountSql(sqlParts);
            var pageSql = SQLPartsHelper.GetPageSql(sqlParts, pageIndex, pageSize, dbType);
            var newParams = new List<SugarParameter>();
            if (dbParameter != null)
            {
                foreach (var param in dbParameter)
                {
                    newParams.Add(new SugarParameter(param.ParameterName, param.Value, param.DbType));
                }
            }
            var total = db.Ado.GetInt(countSql, dbParameter);  // 执行之后dbParameter的value会被清空，所以前面复制出来一份，感觉是orm的bug，先这样吧
            var list = new List<T>();
            if (total > 0)
            {
                list = db.Ado.SqlQuery<T>(pageSql, newParams).ToList();
            }
            var page = new JqgridPageResponse<T>
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = total,
                rows = list,
            };
            return page;
        }

        #endregion


        #endregion
    }
}
