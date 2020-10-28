using FreeSql;
using FreeSql.DatabaseModel;
using FreeSql.Internal.Model;
using Luckyu.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.DataAccess
{
    public class Repository
    {
        public IFreeSql db
        {
            get
            {
                return BaseConnection.InitDatabase();
            }
        }

        private static DbTransaction trans;

        #region 事务
        public Repository BeginTrans()
        {
            trans = db.Ado.MasterPool.Get().Value.BeginTransaction();
            return this;
        }
        public void Commit()
        {
            if (trans != null)
            {
                trans.Commit();
            }
        }
        public void Rollback()
        {
            if (trans != null)
            {
                trans.Rollback();
            }
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
            var obj = db.Ado.ExecuteScalar(strSql, param);
            var result = (T)obj;
            return result;
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public int ExecuteBySql(string strSql, object param = null)
        {
            return db.Ado.ExecuteNonQuery(strSql, param);
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
            var entityInfo = db.CodeFirst.GetTableByEntity<T>();
            if (entityInfo == null)
            {
                throw new Exception("实体没有对应的数据表");
            }
            var tableName = entityInfo.DbName;
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
                if (col.Key.IsEmpty())
                {
                    continue;
                }
                if (col.Value.CsType != typeof(string))
                {
                    continue;
                }

                var value = col.Value.GetValue(entity);  // string  null 转为 空字符串
                if (value == null)
                {
                    col.Value.SetValue(entity, "");
                }
                if (ignoreColumnName.Contains(col.Key.ToUpper()))
                {
                    continue;
                }
                // 目前类型仅为varchar nvarchar,不能简单判断 col.PropertyInfo.PropertyType == typeod(string) 
                // 经测试数据库类型为text时,dbCol.Length有一个迷之长度16,实际数据库并没有长度限制,导致异常,不确定其他类型有没有,故先不做判断
                var strValue = (value ?? "").ToString();
                var valueLength = 0;
                switch (col.Value.DbTypeText.ToLower())
                {
                    case "varchar": valueLength = strValue.GetASCIILength(); break;
                    case "nvarchar": valueLength = strValue.Length; break;
                }
                if (col.Value.DbSize > 0 && valueLength > 0 && valueLength > col.Value.DbSize)
                {
                    throw new Exception($"【{(col.Value.Table.DbName + " " + col.Value.CsName + " " + col.Value.Comment)}】字段长度过长，数据库长度为{ col.Value.DbSize}，实际字段长度为{valueLength}，请验证输入，或联系管理员增加数据库字段长度");
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
                result = db.Insert(entity).ExecuteAffrows();
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
                result = db.Insert(list).ExecuteAffrows();
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
                result = db.Delete<T>(entity).ExecuteAffrows();
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
                result = db.Delete<T>(list).ExecuteAffrows();
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
                var list = db.Select<T>().Where(condition).ToList();
                result = db.Delete<T>(list).ExecuteAffrows();
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
                var query = db.Update<T>(entity).UpdateColumns(onlyUpdateColumns);
                result = query.ExecuteAffrows();
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
                var query = db.Update<T>(entity);
                if (ignoreColumns != null)
                {
                    query = query.IgnoreColumns(ignoreColumns);
                }
                result = query.ExecuteAffrows();
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
                var query = db.Update<T>(entity);
                var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                var igCols = new List<string>();
                if (dict != null && dict.Count > 0)
                {
                    var entityInfo = db.CodeFirst.GetTableByEntity<T>();
                    var allColumns = entityInfo.Columns;
                    igCols = allColumns.Where(r => !r.Value.Attribute.IsPrimary && !dict.Keys.Contains(r.Key)).Select(r => r.Key).ToList();
                    if (!igCols.IsEmpty())
                    {
                        query = query.IgnoreColumns(igCols.ToArray());
                    }
                }
                VerifyDbColumn(entity, null, igCols);

                result = query.ExecuteAffrows();
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
        /// <param name="ignoreColumns">更多要排除字段</param>
        /// <returns></returns>
        public int UpdateIgnoreColumns<T>(T entity, string json, Expression<Func<T, object>> ignoreColumns) where T : class, new()
        {
            var result = 0;
            try
            {
                var query = db.Update<T>(entity);
                var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                var igCols = new List<string>();
                if (dict != null && dict.Count > 0)
                {
                    var entityInfo = db.CodeFirst.GetTableByEntity<T>();
                    var allColumns = entityInfo.Columns;
                    igCols = allColumns.Where(r => !r.Value.Attribute.IsPrimary && !dict.Keys.Contains(r.Key)).Select(r => r.Key).ToList();
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

                result = query.ExecuteAffrows();
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
                var query = db.Update<T>(entity);
                var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                var igCols = new List<string>();
                if (dict != null && dict.Count > 0)
                {
                    var entityInfo = db.CodeFirst.GetTableByEntity<T>();
                    var allColumns = entityInfo.Columns;
                    igCols = allColumns.Where(r => !r.Value.Attribute.IsPrimary && !dict.Keys.Contains(r.Key)).Select(r => r.Key).ToList();
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

                result = query.ExecuteAffrows();
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
                var query = db.Update<T>(entity);
                var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                var igCols = new List<string>();
                if (dict != null && dict.Count > 0)
                {
                    var entityInfo = db.CodeFirst.GetTableByEntity<T>();
                    var allColumns = entityInfo.Columns;
                    igCols = allColumns.Where(r => !r.Value.Attribute.IsPrimary && !dict.Keys.Contains(r.Key)).Select(r => r.Key).ToList();
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

                result = query.ExecuteAffrows();
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
            var entityInfo = db.CodeFirst.GetTableByEntity<T>();
            if (entityInfo == null)
            {
                return 0;
            }
            var tableName = entityInfo.DbName;
            var tableNameEx = tableName + "_extension";
            // 获取扩展表的字段
            var tableEx = db.DbFirst.GetTableByName(tableNameEx);
            if (tableEx == null)
            {
                return 0;
            }
            var colsEx = tableEx.Columns;
            if (colsEx == null || colsEx.Count < 1)
            {
                return 0;
            }
            var pk = entityInfo.Primarys.FirstOrDefault();
            var pkName = pk.Attribute.Name.ToUpper();
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
            var entityInfo = db.CodeFirst.GetTableByEntity<T>();
            if (entityInfo == null)
            {
                return 0;
            }
            var tableName = entityInfo.DbName;
            var tableNameEx = tableName + "_extension";
            // 获取当前表的字段
            var tableEx = db.DbFirst.GetTableByName(tableNameEx);
            if (tableEx == null)
            {
                return 0;
            }
            var colsEx = tableEx.Columns;
            if (colsEx == null || colsEx.Count < 1)
            {
                return 0;
            }
            var pk = entityInfo.Primarys.FirstOrDefault();
            var pkName = pk.Attribute.Name.ToUpper();

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

            var dicParam = new Dictionary<string, object>();
            foreach (var field in formDataJson)
            {
                var upper = field.Key.ToUpper();
                var col = cols.Where(r => r.Name.ToUpper() == upper).FirstOrDefault();
                if (col != null && !field.Value.IsEmpty())
                {
                    strName.Append($"{upper},");
                    sqlValue.Append($" {BaseConnection.ParaPre}{upper},");

                    dicParam.Add(upper, field.Value);
                }
            }
            var sql = $"INSERT INTO {tableName} ( {strName.ToString().TrimEnd(',')} ) VALUES ( {sqlValue.ToString().TrimEnd(',')} )";
            return db.Ado.ExecuteNonQuery(sql, dicParam);
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
            var entityInfo = db.CodeFirst.GetTableByEntity<T>();
            if (entityInfo == null)
            {
                return 0;
            }
            var tableName = entityInfo.DbName;
            var tableNameEx = tableName + "_extension";
            // 获取扩展表的字段
            var tableEx = db.DbFirst.GetTableByName(tableNameEx);
            if (tableEx == null)
            {
                return 0;
            }
            var colsEx = tableEx.Columns;
            if (colsEx == null || colsEx.Count < 1)
            {
                return 0;
            }
            // 主键 字段
            var pk = entityInfo.Primarys.FirstOrDefault();
            var pkName = pk.Attribute.Name.ToUpper();
            var keyValue = type.GetProperties().First(n => n.Name.ToUpper() == pkName).GetValue(entity);

            JObject extJObject = (JObject)entity.GetType().GetProperties().First(n => n.Name == "ExtObject").GetValue(entity);
            if (extJObject == null)
            {
                return 0;
            }

            extJObject.Add(pkName, keyValue.ToString());

            var sqlCount = $"SELECT COUNT(1) FROM {tableNameEx} WHERE {pkName} = {BaseConnection.ParaPre}{pkName} ";
            var countExt = (int)db.Ado.ExecuteScalar(sqlCount, new Dictionary<string, object> { { pkName, keyValue } });
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
            var dicParas = new Dictionary<string, object>();
            foreach (var field in formDataJson)
            {
                var upper = field.Key.ToUpper();
                var col = cols.Where(r => r.Name.ToUpper() == upper).FirstOrDefault();
                if (!col.IsPrimary)
                {
                    listValue.Add($"{upper} = @{upper}");
                    dicParas.Add(upper, field.Value.ToString());
                }
            }
            strSql.Append(string.Join(",", listValue));
            var pk = cols.Where(r => r.IsPrimary).FirstOrDefault();
            var strPkName = pk.Name.ToUpper();
            strSql.Append($" WHERE {strPkName} = {BaseConnection.ParaPre}{strPkName} ");
            dicParas.Add(strPkName, formDataJson[strPkName].ToString());
            return db.Ado.ExecuteNonQuery(strSql.ToString(), dicParas);
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
            var entityInfo = db.CodeFirst.GetTableByEntity<T>();
            if (entityInfo == null)
            {
                return 0;
            }
            var tableName = entityInfo.DbName;
            var tableNameEx = tableName + "_extension";
            var tableEx = db.DbFirst.GetTableByName(tableNameEx);
            if (tableEx == null)
            {
                return 0;
            }
            var colsEx = tableEx.Columns;
            if (colsEx == null || colsEx.Count < 1)
            {
                return 0;
            }
            var pk = entityInfo.Primarys.FirstOrDefault();
            var pkName = pk.Attribute.Name.ToUpper();
            var keyValue = type.GetProperties().First(n => n.Name.ToUpper() == pkName).GetValue(entity);

            var sql = $"DELETE FROM {tableNameEx} WHERE {pkName} = {BaseConnection.ParaPre}{pkName} ";
            var dicParas = new Dictionary<string, object> { { pkName, keyValue } };
            return db.Ado.ExecuteNonQuery(sql, dicParas);
        }
        private int DeleteExtensionTableList<T>(List<T> list)
        {
            var type = typeof(T);
            if (!type.IsSubclassOf(typeof(ExtensionEntityBase))) // 没有扩展表
            {
                return 0;
            }
            var entityInfo = db.CodeFirst.GetTableByEntity<T>();
            if (entityInfo == null)
            {
                return 0;
            }
            var tableName = entityInfo.DbName;
            var tableNameEx = tableName + "_extension";
            var tableEx = db.DbFirst.GetTableByName(tableNameEx);
            if (tableEx == null)
            {
                return 0;
            }
            var colsEx = tableEx.Columns;
            if (colsEx == null || colsEx.Count < 1)
            {
                return 0;
            }
            var pk = entityInfo.Primarys.FirstOrDefault();
            var pkName = pk.Attribute.Name.ToUpper();

            var result = 0;
            foreach (var entity in list)
            {
                var keyValue = type.GetProperties().First(n => n.Name.ToUpper() == pkName).GetValue(entity);

                var sql = $"DELETE FROM {tableNameEx} WHERE {pkName} = {BaseConnection.ParaPre}{pkName} ";
                var dicParas = new Dictionary<string, object> { { pkName, keyValue } };
                result += db.Ado.ExecuteNonQuery(sql, dicParas);
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
            var entityInfo = db.CodeFirst.GetTableByEntity<T>();
            if (entityInfo == null)
            {
                return;
            }
            var tableName = entityInfo.DbName;
            var tableNameEx = tableName + "_extension";
            var tableEx = db.DbFirst.GetTableByName(tableNameEx);
            if (tableEx == null)
            {
                return;
            }
            var colsEx = tableEx.Columns;
            if (colsEx == null || colsEx.Count < 1)
            {
                return;
            }
            var pk = entityInfo.Primarys.FirstOrDefault();
            var pkName = pk.Attribute.Name.ToUpper();
            var keyValue = type.GetProperties().First(n => n.Name.ToUpper() == pkName).GetValue(entity);

            var sql = $"SELECT * FROM {tableNameEx} WHERE {pkName} = {BaseConnection.ParaPre}{pkName} ";
            var dicParas = new Dictionary<string, object> { { pkName, keyValue } };
            var dt = db.Ado.ExecuteDataTable(sql, dicParas);
            JObject extObject = new JObject();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                foreach (var column in colsEx)
                {
                    if (column.IsPrimary)
                    {
                        continue;
                    }
                    extObject.Add(column.Name, dr[column.Name].ToString());
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
            var entityInfo = db.CodeFirst.GetTableByEntity<T>();
            if (entityInfo == null)
            {
                return;
            }
            var tableName = entityInfo.DbName;
            var tableNameEx = tableName + "_extension";
            var tableEx = db.DbFirst.GetTableByName(tableNameEx);
            if (tableEx == null)
            {
                return;
            }
            var colsEx = tableEx.Columns;
            if (colsEx == null || colsEx.Count < 1)
            {
                return;
            }
            var pk = entityInfo.Primarys.FirstOrDefault();
            var pkName = pk.Attribute.Name.ToUpper();
            foreach (var entity in list)
            {
                var keyValue = type.GetProperties().First(n => n.Name.ToUpper() == pkName).GetValue(entity);

                var sql = $"SELECT * FROM {tableNameEx} WHERE {pkName} = @{pkName} ";
                var dicParas = new Dictionary<string, object> { { pkName, keyValue } };
                var dt = db.Ado.ExecuteDataTable(sql, dicParas);
                JObject extObject = new JObject();
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    foreach (var column in colsEx)
                    {
                        if (column.IsPrimary)
                        {
                            continue;
                        }
                        extObject.Add(column.Name, dr[column.Name].ToString());
                    }
                }
                entity.GetType().GetProperties().First(n => n.Name == "ExtObject").SetValue(entity, extObject);
            }
        }

        #endregion

        #region Entity 查询
        public bool Exist<T>(T entity) where T : class, new()
        {
            var result = db.Select<T>(entity).Any();
            return result;
        }

        public int Count<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            var count = db.Select<T>().Where(condition).Count();
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
            var entityInfo = db.CodeFirst.GetTableByEntity<T>();
            if (entityInfo == null)
            {
                return null;
            }
            var pk = entityInfo.Primarys.FirstOrDefault();
            var pkName = pk.Attribute.Name.ToUpper();
            var sql = $"SELECT * FROM {entityInfo.DbName} WHERE {pkName} = {BaseConnection.ParaPre}{pkName}";
            var dicParas = new Dictionary<string, object> { { pkName, keyValue } };
            var entity = db.Select<T>().WithSql(sql, dicParas).First();
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
            var query = db.Select<T>().Where(condition);
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
            var query = db.Select<T>().Where(condition);
            if (orderby != null)
            {
                if (isDesc)
                {
                    query = query.OrderByDescending(orderby);
                }
                else
                {
                    query = query.OrderBy(orderby);
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
            var entity = db.Select<T>().WithSql(strSql, dbParameter).First();
            if (entity != null)
            {
                AttachExtObject<T>(ref entity);
            }
            return entity;
        }
        #endregion

        #region DataTable
        public DataTable GetDataTable(string sql, object paras = null)
        {
            return db.Ado.ExecuteDataTable(sql, paras);
        }

        public JqgridDatatablePageResponse GetDataTable(JqgridPageRequest jqPage, string strSql, object paras = null)
        {
            var dbType = db.Ado.DataType;
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
            var total = (int)db.Ado.ExecuteScalar(countSql, paras);  // 执行之后dbParameter的value会被清空，所以前面复制出来一份，感觉是orm的bug，先这样吧
            DataTable dt = null;
            if (total > 0)
            {
                dt = db.Ado.ExecuteDataTable(pageSql, paras);
            }
            var page = new JqgridDatatablePageResponse
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = total,
                rows = dt,
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
            var list = db.Ado.Query<T>(strSql, dbParameter);
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
            var query = db.Select<T>().Where(condition);
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
            var query = db.Select<T>().Where(condition);
            var filters = new List<DynamicFilterInfo>();
            if (jqPage.isSearch)
            {
                foreach (var rule in jqPage.fitersObj.rules)
                {
                    filters.Add(SearchConditionHelper.GetStringLikeCondition(rule.field, rule.data));
                }
            }
            if (!filters.IsEmpty())
            {
                foreach (var filter in filters)
                {
                    query = query.WhereDynamicFilter(filter);
                }
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
        public List<T> GetList<T>(JqgridPageRequest jqPage, Expression<Func<T, bool>> condition, List<DynamicFilterInfo> filters) where T : class, new()
        {
            var query = db.Select<T>().Where(condition);
            if (!filters.IsEmpty())
            {
                foreach (var filter in filters)
                {
                    query = query.WhereDynamicFilter(filter);
                }
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

        public List<T> GetList<T>(JqgridPageRequest jqPage, Expression<Func<T, bool>> condition, Dictionary<string, Func<DynamicFilterInfo>> dicCondition) where T : class, new()
        {
            var query = db.Select<T>().Where(condition);
            var filters = new List<DynamicFilterInfo>();
            if (jqPage.isSearch)
            {
                foreach (var rule in jqPage.fitersObj.rules)
                {
                    if (dicCondition.ContainsKey(rule.field))
                    {
                        var conditions = dicCondition[rule.field]();
                        filters.Add(conditions);
                    }
                    else
                    {
                        filters.Add(SearchConditionHelper.GetStringLikeCondition(rule.field, rule.data));
                    }
                }
            }
            if (!filters.IsEmpty())
            {
                foreach (var filter in filters)
                {
                    query = query.WhereDynamicFilter(filter);
                }
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
            var query = db.Select<T>().Where(condition);
            if (orderby != null)
            {
                if (isDesc)
                {
                    query = query.OrderByDescending(orderby);
                }
                else
                {
                    query = query.OrderBy(orderby);
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
        public List<T> GetList<T>(ISelect<T> query) where T : class, new()
        {
            var list = query.ToList();
            if (list != null && list.Count > 0)
            {
                AttachExtList<T>(ref list);
            }
            return list;
        }
        public List<T> GetList<T>(ISelect<T> query, List<DynamicFilterInfo> filters) where T : class, new()
        {
            if (!filters.IsEmpty())
            {
                foreach (var filter in filters)
                {
                    query = query.WhereDynamicFilter(filter);
                }
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
            var query = db.Select<T>().Where(condition);
            if (orderby != null)
            {
                if (isDesc)
                {
                    query = query.OrderByDescending(orderby);
                }
                else
                {
                    query = query.OrderBy(orderby);
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
            var query = db.Select<T>().Where(condition);
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
            var query = db.Select<T>().Where(condition);
            if (!string.IsNullOrEmpty(orderby))
            {
                query = query.OrderBy(orderby);
            }
            //var total = 0;
            var list = query.Count(out var total).Page(pageIndex, pageSize).ToList();
            var page = new JqgridPageResponse<T>
            {
                count = pageSize,
                page = pageIndex,
                records = (int)total,
                rows = list,
            };
            return page;

        }
        public JqgridPageResponse<T> GetPage<T>(JqgridPageRequest jqPage, Expression<Func<T, bool>> condition) where T : class, new()
        {
            var query = db.Select<T>().Where(condition);
            var filters = new List<DynamicFilterInfo>();
            if (jqPage.isSearch)
            {
                foreach (var rule in jqPage.fitersObj.rules)
                {
                    filters.Add(SearchConditionHelper.GetStringLikeCondition(rule.field, rule.data));
                }
            }
            if (!filters.IsEmpty())
            {
                foreach (var filter in filters)
                {
                    query = query.WhereDynamicFilter(filter);
                }
            }
            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord} ");
            }
            var list = query.Count(out var total).Page(jqPage.page, jqPage.rows).ToList();
            var page = new JqgridPageResponse<T>
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = (int)total,
                rows = list,
            };
            return page;
        }
        public JqgridPageResponse<T> GetPage<T>(JqgridPageRequest jqPage, ISelect<T> query) where T : class, new()
        {
            var filters = new List<DynamicFilterInfo>();
            if (jqPage.isSearch)
            {
                foreach (var rule in jqPage.fitersObj.rules)
                {
                    filters.Add(SearchConditionHelper.GetStringLikeCondition(rule.field, rule.data));
                }
            }
            if (!filters.IsEmpty())
            {
                foreach (var filter in filters)
                {
                    query = query.WhereDynamicFilter(filter);
                }
            }
            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord} ");
            }
            var list = query.Count(out var total).Page(jqPage.page, jqPage.rows).ToList();
            var page = new JqgridPageResponse<T>
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = (int)total,
                rows = list,
            };
            return page;
        }
        public JqgridPageResponse<T> GetPage<T>(JqgridPageRequest jqPage, ISelect<T> query, List<DynamicFilterInfo> filters) where T : class, new()
        {
            if (!filters.IsEmpty())
            {
                foreach (var filter in filters)
                {
                    query = query.WhereDynamicFilter(filter);
                }
            }
            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord} ");
            }
            var list = query.Count(out var total).Page(jqPage.page, jqPage.rows).ToList();
            var page = new JqgridPageResponse<T>
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = (int)total,
                rows = list,
            };
            return page;
        }
        public JqgridPageResponse<T> GetPage<T>(JqgridPageRequest jqPage, List<DynamicFilterInfo> filters) where T : class, new()
        {
            var query = db.Select<T>();
            if (!filters.IsEmpty())
            {
                foreach (var filter in filters)
                {
                    query = query.WhereDynamicFilter(filter);
                }
            }
            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord} ");
            }
            var list = query.Count(out var total).Page(jqPage.page, jqPage.rows).ToList();
            var page = new JqgridPageResponse<T>
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = (int)total,
                rows = list,
            };
            return page;
        }
        public JqgridPageResponse<T> GetPage<T>(JqgridPageRequest jqPage, List<Expression<Func<T, bool>>> condition, List<DynamicFilterInfo> filters) where T : class, new()
        {
            var query = db.Select<T>();
            if (condition.Count > 0)
            {
                foreach (var item in condition)
                {
                    query = query.Where(item);
                }
            }
            if (!filters.IsEmpty())
            {
                foreach (var filter in filters)
                {
                    query = query.WhereDynamicFilter(filter);
                }
            }
            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord} ");
            }
            var list = query.Count(out var total).Page(jqPage.page, jqPage.rows).ToList();
            var page = new JqgridPageResponse<T>
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = (int)total,
                rows = list,
            };
            return page;
        }
        public JqgridPageResponse<T> GetPage<T>(JqgridPageRequest jqPage, Expression<Func<T, bool>> condition, List<DynamicFilterInfo> filters) where T : class, new()
        {
            var query = db.Select<T>();
            if (condition != null)
            {
                query = query.Where(condition);
            }
            if (!filters.IsEmpty())
            {
                foreach (var filter in filters)
                {
                    query = query.WhereDynamicFilter(filter);
                }
            }
            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord} ");
            }
            var list = query.Count(out var total).Page(jqPage.page, jqPage.rows).ToList();
            var page = new JqgridPageResponse<T>
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = (int)total,
                rows = list,
            };
            return page;
        }
        public JqgridPageResponse<T> GetPage<T>(JqgridPageRequest jqPage, Expression<Func<T, bool>> condition, Dictionary<string, Func<string, string, DynamicFilterInfo>> dicModels) where T : class, new()
        {
            var query = db.Select<T>();
            if (condition != null)
            {
                query = query.Where(condition);
            }
            var filters = new List<DynamicFilterInfo>();
            if (jqPage.isSearch)
            {
                foreach (var rule in jqPage.fitersObj.rules)
                {
                    if (dicModels.ContainsKey(rule.field))
                    {
                        var comdition = dicModels[rule.field](rule.field, rule.data);
                        if (comdition != null)
                        {
                            filters.Add(comdition);
                        }
                    }
                    else
                    {
                        filters.Add(SearchConditionHelper.GetStringLikeCondition(rule.field, rule.data));
                    }
                }
            }
            if (!filters.IsEmpty())
            {
                foreach (var filter in filters)
                {
                    query = query.WhereDynamicFilter(filter);
                }
            }
            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord} ");
            }
            var list = query.Count(out var total).Page(jqPage.page, jqPage.rows).ToList();
            var page = new JqgridPageResponse<T>
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = (int)total,
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
            var dbType = db.Ado.DataType;
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
            var total = (int)db.Ado.ExecuteScalar(countSql, dbParameter);
            var list = new List<T>();
            if (total > 0)
            {
                list = db.Ado.Query<T>(pageSql, dbParameter).ToList();
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
