using Luckyu.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.DataAccess
{
    public class Repository
    {
        public SqlSugarClient db
        {
            get
            {
                return new BaseConnection().Instance;
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

        #region 执行 SQL 语句
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <returns>h返回受影响行数，select语句无效</returns>
        public T ExecuteScalar<T>(string strSql, object param = null)
        {
            var obj = db.Ado.GetScalar(strSql, param);
            var result = (T)obj;
            return result;
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="dbParameter">参数</param>
        /// <returns></returns>
        public int ExecuteBySql(string strSql, object dbParameter = null)
        {
            return db.Ado.ExecuteCommand(strSql, dbParameter);
        }
        public int ExecuteBySql(string strSql, JObject dbParameter)
        {
            List<SugarParameter> parameters = new List<SugarParameter>();
            foreach (var item in dbParameter)
            {
                parameters.Add(new SugarParameter(item.Key, item.Value));
            }
            return db.Ado.ExecuteCommand(strSql, parameters);
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="dbParameter">参数</param>
        /// <returns></returns>
        public int ExecuteByProc(string procName, object dbParameter = null)
        {
            return db.Ado.ExecuteCommand(procName, dbParameter);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="dbParameter">参数</param>
        /// <returns></returns>
        public T ExecuteByProc<T>(string procName, object dbParameter = null) where T : class
        {
            var entity = db.Ado.SqlQuerySingle<T>(procName, dbParameter);
            return entity;
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        public List<T> QueryByProc<T>(string procName, object dbParameter = null) where T : class
        {
            var list = db.Ado.SqlQuery<T>(procName, dbParameter);
            return list;
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

        #region 写操作日志
        private void WriteLog<T>(T entity, string type, string name) where T : class, new()
        {
            //var entityInfo = db.EntityMaintenance.GetEntityInfo<T>();
            //if (entityInfo == null || entityInfo.Columns.IsEmpty())
            //{
            //    return;
            //}
            //var log = new Base_LogEntity();
            //log.Create();
            //log.F_SourceContentJson = JsonConvert.SerializeObject(entity);
            //log.F_OPERATETYPEID = type;
            //log.F_OPERATETYPE = name;
            //log.F_EXECUTERESULT = 1;
            //log.F_MODULE = entityInfo.DbTableName;
            //if (entityInfo.DbTableName == "BASE_BILLLOG" || entityInfo.DbTableName == "BASE_LOG")
            //{
            //    return;
            //}
            //var mainKey = entityInfo.Columns.Where(r => r.IsPrimarykey).Select(r => r.PropertyInfo).FirstOrDefault();
            //if (mainKey != null)
            //{
            //    var keyValue = mainKey.GetValue(entity).ToString();
            //    log.F_SourceObjectId = keyValue;
            //}
            //db.Insertable(log).ExecuteCommand();
        }
        private void WriteLog<T>(List<T> list, string type, string name) where T : class, new()
        {
            //var entityInfo = db.EntityMaintenance.GetEntityInfo<T>();
            //if (entityInfo == null || entityInfo.Columns.IsEmpty())
            //{
            //    return;
            //}
            //var log = new Base_LogEntity();
            //log.Create();
            //log.F_SourceContentJson = JsonConvert.SerializeObject(list);
            //log.F_OPERATETYPEID = type;
            //log.F_OPERATETYPE = name;
            //log.F_EXECUTERESULT = 1;
            //log.F_MODULE = entityInfo.DbTableName;
            //if (entityInfo.DbTableName == "BASE_BILLLOG" || entityInfo.DbTableName == "BASE_LOG")
            //{
            //    return;
            //}
            //db.Insertable(log).ExecuteCommand();
        }
        #endregion

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
                WriteLog(entity, "5", "新增");
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

                WriteLog(list, "5", "新增");
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
                result = db.Deleteable(entity).ExecuteCommand();
                DeleteExtensionTable<T>(entity);

                WriteLog(entity, "6", "删除");
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
                result = db.Deleteable(list).ExecuteCommand();
                DeleteExtensionTableList<T>(list);

                WriteLog(list, "6", "删除");
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
                result = db.Deleteable(list).ExecuteCommand();
                DeleteExtensionTableList<T>(list);

                WriteLog(list, "6", "删除");
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
                var query = db.Updateable(entity).UpdateColumns(onlyUpdateColumns);
                result = query.ExecuteCommand();

                WriteLog(entity, "7", "修改");
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
                var query = db.Updateable(entity);
                if (ignoreColumns != null)
                {
                    query = query.IgnoreColumns(ignoreColumns);
                }
                result = query.ExecuteCommand();
                UpdateExtensionTable<T>(entity);

                WriteLog(entity, "7", "修改");
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
                var query = db.Updateable(entity);
                var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
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
                VerifyDbColumn(entity, null, igCols);

                result = query.ExecuteCommand();
                UpdateExtensionTable<T>(entity);

                WriteLog(entity, "7", "修改");
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

                WriteLog(entity, "7", "修改");
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
                VerifyDbColumn(entity, null, igCols);

                result = query.ExecuteCommand();
                UpdateExtensionTable<T>(entity);

                WriteLog(entity, "7", "修改");
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
                var query = db.Updateable(entity);
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

                WriteLog(entity, "7", "修改");
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
            var strSql = new StringBuilder("INSERT INTO " + tableName + " ( ");
            var sqlValue = new StringBuilder(" ( ");

            var args = new List<SugarParameter>();
            foreach (var field in formDataJson)
            {
                var upper = field.Key.ToUpper();
                var col = cols.Where(r => r.DbColumnName.ToUpper() == upper).FirstOrDefault();
                if (col != null && !field.Value.IsEmpty())
                {
                    strSql.Append(upper + ",");
                    sqlValue.Append(" @" + upper + ",");

                    args.Add(new SugarParameter(upper, field.Value));
                }
            }
            strSql = strSql.Remove(strSql.Length - 1, 1);
            sqlValue = sqlValue.Remove(sqlValue.Length - 1, 1);

            strSql.Append(" ) VALUES " + sqlValue + ")");

            return db.Ado.ExecuteCommand(strSql.ToString(), args);
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

            var sqlCount = $"SELECT COUNT(1) FROM {tableNameEx} WHERE {pkName} = '{keyValue}' ";
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
            strSql.Append($" WHERE {strPkName} = @{strPkName} ");
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

            var sql = $"DELETE FROM {tableNameEx} WHERE {pkName} = @{pkName} ";
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

                var sql = $"DELETE FROM {tableNameEx} WHERE {pkName} = @{pkName} ";
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

        #region 对象实体 查询
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
            return count;
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
            var sql = $"SELECT * FROM {entityInfo.DbTableName} WHERE {pk.DbColumnName} = @{pk.DbColumnName}";
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
        public T GetEntity<T>(Expression<Func<T, bool>> condition, Expression<Func<T, object>> orderExp = null, OrderByType orderType = OrderByType.Asc) where T : class, new()
        {
            var query = db.Queryable<T>().Where(condition);
            if (orderExp != null)
            {
                query = query.OrderBy(orderExp, orderType);
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
            var entity = db.Ado.SqlQuerySingle<T>(strSql, dbParameter);
            if (entity != null)
            {
                AttachExtObject<T>(ref entity);
            }
            return entity;
        }
        public T GetEntity<T>(string strSql, List<SugarParameter> dbParameter) where T : class, new()
        {
            var entity = db.Ado.SqlQuerySingle<T>(strSql, dbParameter);
            if (entity != null)
            {
                AttachExtObject<T>(ref entity);
            }
            return entity;
        }

        #endregion

        #region DataTable
        public DataTable GetDataTable(string sql, List<SugarParameter> dbParameter = null)
        {
            return db.Ado.GetDataTable(sql, dbParameter);
        }

        public JqgridDatatablePageResponse GetDataTable(JqgridPageRequest jqPage, string strSql, List<SugarParameter> dbParameter = null)
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
            //var list = new List<T>();
            DataTable list = null;
            if (total > 0)
            {
                list = db.Ado.GetDataTable(pageSql, newParams);
            }
            var page = new JqgridDatatablePageResponse
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = total,
                rows = list,
            };
            return page;
            //return db.Ado.GetDataTable(sql, dbParameter);
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
        public List<T> GetList<T>(string strSql, List<SugarParameter> dbParameter) where T : class
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
            var conditionalModels = new List<IConditionalModel>();
            if (jqPage.isSearch)
            {
                foreach (var rule in jqPage.fitersObj.rules)
                {
                    conditionalModels.AddStringLikeCondition(rule.field, rule.data);
                }
            }
            if (!conditionalModels.IsEmpty())
            {
                query = query.Where(conditionalModels);
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
        public List<T> GetList<T>(JqgridPageRequest jqPage, Expression<Func<T, bool>> condition, List<IConditionalModel> conditionalModels) where T : class, new()
        {
            var query = db.Queryable<T>().Where(condition);
            if (!conditionalModels.IsEmpty())
            {
                query = query.Where(conditionalModels);
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

        public List<T> GetList<T>(JqgridPageRequest jqPage, Expression<Func<T, bool>> condition, Dictionary<string, Func<List<IConditionalModel>>> dicCondition) where T : class, new()
        {
            var query = db.Queryable<T>().Where(condition);
            var conditionalModels = new List<IConditionalModel>();
            if (jqPage.isSearch)
            {
                foreach (var rule in jqPage.fitersObj.rules)
                {
                    if (dicCondition.ContainsKey(rule.field))
                    {
                        var conditions = dicCondition[rule.field]();
                        conditionalModels.AddRange(conditions);
                    }
                    else
                    {
                        conditionalModels.AddStringLikeCondition(rule.field, rule.data);
                    }
                }
            }
            if (!conditionalModels.IsEmpty())
            {
                query = query.Where(conditionalModels);
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
        public List<T> GetList<T>(Expression<Func<T, bool>> condition, Expression<Func<T, object>> orderExp, OrderByType orderType = OrderByType.Asc) where T : class, new()
        {
            var query = db.Queryable<T>().Where(condition);
            if (orderExp != null)
            {
                query = query.OrderBy(orderExp, orderType);
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
        public List<T> GetList<T>(ISugarQueryable<T> query, List<IConditionalModel> conditionalModels) where T : class, new()
        {
            if (!conditionalModels.IsEmpty())
            {
                query = query.Where(conditionalModels);
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
        public List<T> GetListTop<T>(int top, Expression<Func<T, bool>> condition, Expression<Func<T, object>> orderExp, OrderByType orderType = OrderByType.Asc) where T : class, new()
        {
            var query = db.Queryable<T>().Where(condition);
            if (orderExp != null)
            {
                query = query.OrderBy(orderExp, orderType);
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
            var conditionalModels = new List<IConditionalModel>();
            if (jqPage.isSearch)
            {
                foreach (var rule in jqPage.fitersObj.rules)
                {
                    conditionalModels.AddStringLikeCondition(rule.field, rule.data);
                }
            }
            if (!conditionalModels.IsEmpty())
            {
                query = query.Where(conditionalModels);
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
            var conditionalModels = new List<IConditionalModel>();
            if (jqPage.isSearch)
            {
                foreach (var rule in jqPage.fitersObj.rules)
                {
                    conditionalModels.AddStringLikeCondition(rule.field, rule.data);
                }
            }
            if (!conditionalModels.IsEmpty())
            {
                query = query.Where(conditionalModels);
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
        public JqgridPageResponse<T> GetPage<T>(JqgridPageRequest jqPage, ISugarQueryable<T> query, List<IConditionalModel> conditionalModels) where T : class, new()
        {
            if (!conditionalModels.IsEmpty())
            {
                query = query.Where(conditionalModels);
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
        public JqgridPageResponse<T> GetPage<T>(JqgridPageRequest jqPage, List<IConditionalModel> conditionalModels) where T : class, new()
        {
            var query = db.Queryable<T>().Where(conditionalModels);
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
        public JqgridPageResponse<T> GetPage<T>(JqgridPageRequest jqPage, List<Expression<Func<T, bool>>> condition, List<IConditionalModel> conditionalModels) where T : class, new()
        {
            var query = db.Queryable<T>();
            if (condition.Count > 0)
            {
                foreach (var item in condition)
                {
                    query = query.Where(item);
                }
            }
            if (conditionalModels != null && conditionalModels.Count > 0)
            {
                query = query.Where(conditionalModels);
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
        public JqgridPageResponse<T> GetPage<T>(JqgridPageRequest jqPage, Expression<Func<T, bool>> condition, List<IConditionalModel> conditionalModels) where T : class, new()
        {
            var query = db.Queryable<T>();
            if (condition != null)
            {
                query = query.Where(condition);
            }
            if (conditionalModels != null && conditionalModels.Count > 0)
            {
                query = query.Where(conditionalModels);
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
            var modelCondition = new List<IConditionalModel>();
            if (jqPage.isSearch)
            {
                foreach (var rule in jqPage.fitersObj.rules)
                {
                    if (dicModels.ContainsKey(rule.field))
                    {
                        var comdition = dicModels[rule.field](rule.field, rule.data);
                        if (comdition != null)
                        {
                            modelCondition.AddRange(comdition);
                        }
                    }
                    else
                    {
                        modelCondition.AddStringLikeCondition(rule.field, rule.data);
                    }
                }
            }
            if (modelCondition != null && modelCondition.Count > 0)
            {
                query = query.Where(modelCondition);
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
        public JqgridPageResponse<T> GetPage<T>(JqgridPageRequest jqPage, string strSql, object dbParameter = null) where T : class, new()
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
            var total = db.Ado.GetInt(countSql, dbParameter);
            var list = new List<T>();
            if (total > 0)
            {
                list = db.Ado.SqlQuery<T>(pageSql, dbParameter).ToList();
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
        public JqgridPageResponse<T> GetPage<T>(JqgridPageRequest jqPage, string strSql, List<SugarParameter> dbParameter) where T : class, new()
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

        #region 数据源 查询
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <returns></returns>
        public DataTable GetTable(string strSql)
        {
            return db.Ado.GetDataTable(strSql);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="dbParameter">参数</param>
        /// <returns></returns>
        public DataTable GetTable(string strSql, object dbParameter)
        {
            return db.Ado.GetDataTable(strSql, dbParameter);
        }
        /// <summary>
        /// 查询列表(分页)
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="jqPage">分页数据</param>
        /// <returns></returns>
        public JqgridDatatablePageResponse GetTable(string strSql, JqgridPageRequest jqPage)
        {
            return this.GetDataTable(jqPage, strSql, null);

        }
        #endregion
    }
}
