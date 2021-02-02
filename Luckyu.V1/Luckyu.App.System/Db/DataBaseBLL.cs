using Luckyu.App.Organization;
using Luckyu.DataAccess;
using Luckyu.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Luckyu.Utility;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reflection;

namespace Luckyu.App.System
{
    public class DataBaseBLL
    {
        #region Var
        private sys_dbtableService tableService = new sys_dbtableService();
        private sys_dbcolumnService columnService = new sys_dbcolumnService();
        private LogBLL logBLL = new LogBLL();

        private UserBLL userBLL = new UserBLL();
        private DepartmentBLL deptBLL = new DepartmentBLL();
        private CompanyBLL companyBLL = new CompanyBLL();
        private GroupBLL groupBLL = new GroupBLL();
        private DataitemBLL dataitemBLL = new DataitemBLL();
        #endregion

        #region Log
        public void LogInsert<T>(T entity, UserModel loginInfo) where T : class, new()
        {
            var db = BaseConnection.InitDatabase();
            var entityInfo = db.CodeFirst.GetTableByEntity<T>();
            if (entityInfo == null)
            {
                return;
            }
            if (entityInfo.Primarys.Length < 1)
            {
                return;
            }
            //var primaryName = entityInfo.Primarys[0].Attribute.Name;
            var keyValue = entityInfo.Primarys[0].GetValue(entity).ToString();
            var tableInfo = tableService.GetEntity(r => r.dbname == entityInfo.DbName);
            if (tableInfo == null)
            {
                return;
            }
            var log = new sys_logEntity();
            log.app_name = LuckyuHelper.AppID;
            log.log_type = (int)LogType.Business;
            log.module = $"{ tableInfo.showname}";
            log.op_type = "新增";
            log.process_id = keyValue;

            log.user_id = loginInfo.user_id;
            log.user_name = loginInfo.realname;

            var strResult = new StringBuilder();
            var columns = columnService.GetList(r => r.table_id == tableInfo.table_id);
            var propeties = typeof(T).GetProperties();
            foreach (var col in columns)
            {
                var propety = propeties.Where(r => r.Name.ToLower() == col.dbcolumnname.ToLower()).FirstOrDefault();
                if (propety == null)
                {
                    continue;
                }
                var value = propety.GetValue(entity).ToString();
                var realvalue = GetRealValue(col, value);
                strResult.Append($"{col.showcolumnname} {realvalue}\r\n");
            }

            log.log_content = strResult.ToString();
            LogBLL.WriteLog(log);
        }

        public void LogDelete<T>(T entity, UserModel loginInfo) where T : class, new()
        {
            var db = BaseConnection.InitDatabase();
            var entityInfo = db.CodeFirst.GetTableByEntity<T>();
            if (entityInfo == null)
            {
                return;
            }
            var tableInfo = tableService.GetEntity(r => r.dbname == entityInfo.DbName);
            if (tableInfo == null)
            {
                return;
            }
            if (entityInfo.Primarys.Length < 1)
            {
                return;
            }
            //var primaryName = entityInfo.Primarys[0].Attribute.Name;
            var keyValue = entityInfo.Primarys[0].GetValue(entity).ToString();
            var log = new sys_logEntity();
            log.app_name = LuckyuHelper.AppID;
            log.log_type = (int)LogType.Business;
            log.module = $"{ tableInfo.showname}";
            log.op_type = "删除";
            log.process_id = keyValue;

            log.user_id = loginInfo.user_id;
            log.user_name = loginInfo.realname;

            var strResult = new StringBuilder();
            var columns = columnService.GetList(r => r.table_id == tableInfo.table_id);
            var propeties = typeof(T).GetProperties();
            foreach (var col in columns)
            {
                var propety = propeties.Where(r => r.Name.ToLower() == col.dbcolumnname.ToLower()).FirstOrDefault();
                if (propety == null)
                {
                    continue;
                }
                var value = propety.GetValue(entity).ToString();
                var realvalue = GetRealValue(col, value);
                strResult.Append($"{col.showcolumnname} {realvalue}\r\n");
            }

            log.log_content = strResult.ToString();
            LogBLL.WriteLog(log);
        }

        public void LogUpdate<T>(T from, T to, string json, UserModel loginInfo) where T : class, new()
        {
            var db = BaseConnection.InitDatabase();
            var entityInfo = db.CodeFirst.GetTableByEntity<T>();
            if (entityInfo == null)
            {
                return;
            }
            var tableInfo = tableService.GetEntity(r => r.dbname == entityInfo.DbName);
            if (tableInfo == null)
            {
                return;
            }
            if (entityInfo.Primarys.Length < 1)
            {
                return;
            }
            var keyValue = entityInfo.Primarys[0].GetValue(to).ToString();
            var onlyCols = new List<string>();
            if (!json.IsEmpty())
            {
                var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                onlyCols.AddRange(dict.Keys.ToList());
            }
            var log = new sys_logEntity();
            log.app_name = LuckyuHelper.AppID;
            log.log_type = (int)LogType.Business;
            log.module = $"{ tableInfo.showname}";
            log.op_type = "修改";
            log.process_id = keyValue;

            log.user_id = loginInfo.user_id;
            log.user_name = loginInfo.realname;

            var strResult = new StringBuilder();
            var columns = columnService.GetList(r => r.table_id == tableInfo.table_id);
            var propeties = typeof(T).GetProperties();
            foreach (var col in columns)
            {
                var propety = propeties.Where(r => r.Name.ToLower() == col.dbcolumnname.ToLower()).FirstOrDefault();
                if (propety == null)
                {
                    continue;
                }
                if (!json.IsEmpty() && !onlyCols.IsEmpty())
                {
                    if (!onlyCols.Contains(propety.Name))
                    {
                        continue;
                    }
                }

                var fromValue = propety.GetValue(from).ToString();
                var toValue = propety.GetValue(to).ToString();
                if (fromValue != toValue)
                {
                    var fromRealValue = GetRealValue(col, fromValue);
                    var toRealValue = GetRealValue(col, toValue);
                    if (fromRealValue != toRealValue)
                    {
                        strResult.Append($"{col.showcolumnname} {fromRealValue} => {toRealValue}\r\n");
                    }
                }
            }

            log.log_content = strResult.ToString();
            LogBLL.WriteLog(log);
        }

        public void LogUpdate<T>(T from, T to, UserModel loginInfo) where T : class, new()
        {
            LogUpdate(from, to, null, loginInfo);
        }

        public void LogUpdate<T>(T to, string json, UserModel loginInfo) where T : class, new()
        {
            var db = BaseConnection.InitDatabase();
            var entityInfo = db.CodeFirst.GetTableByEntity<T>();
            if (entityInfo == null)
            {
                return;
            }
            if (entityInfo.Primarys.Length < 1)
            {
                return;
            }
            var primaryName = entityInfo.Primarys[0].Attribute.Name;
            var keyValue = entityInfo.Primarys[0].GetValue(to);
            var sql = $"SELECT * FROM {entityInfo.DbName} WHERE {primaryName} = {BaseConnection.ParaPre}{primaryName}";
            var dicParas = new Dictionary<string, object> { { primaryName, keyValue } };
            var from = db.Select<T>().WithSql(sql, dicParas).First();
            LogUpdate(from, to, json, loginInfo);
        }

        #endregion

        /// <summary>
        /// 后台验证
        /// </summary>
        public ResponseResult CheckEntity<T>(T entity) where T : class, new()
        {
            var db = BaseConnection.InitDatabase();
            var entityInfo = db.CodeFirst.GetTableByEntity<T>();
            if (entityInfo == null)
            {
                return ResponseResult.Fail(MessageString.Fail);
            }
            if (entityInfo.Primarys.Length < 1)
            {
                return ResponseResult.Fail(MessageString.Fail);
            }
            //var primaryName = entityInfo.Primarys[0].Attribute.Name;
            var keyValue = entityInfo.Primarys[0].GetValue(entity).ToString();
            var tableInfo = tableService.GetEntity(r => r.dbname == entityInfo.DbName);
            if (tableInfo == null)
            {
                return ResponseResult.Fail(MessageString.Fail);
            }
            var columns = columnService.GetList(r => r.table_id == tableInfo.table_id && r.layverify.Contains("required"));
            var propeties = typeof(T).GetProperties();
            foreach (var col in columns)
            {
                var propety = propeties.Where(r => r.Name.ToLower() == col.dbcolumnname.ToLower()).FirstOrDefault();
                if (propety == null)
                {
                    continue;
                }
                var value = propety.GetValue(entity).ToString();
                if (value.IsEmpty())
                {
                    var msg = col.showcolumnname + " " + MessageString.Required;
                    return ResponseResult.Fail(msg);
                }
            }
            return ResponseResult.Success();
        }


        private string GetRealValue(sys_dbcolumnEntity col, string value)
        {
            string realvalue = "";
            switch (col.showtype)
            {
                case "text":
                case "datetime":
                case "number":
                    realvalue = $"{value.ToDecimal()}";
                    break;
                case "user":
                    {
                        var user = userBLL.GetEntityByCache(r => r.user_id == value);
                        if (user != null)
                        {
                            realvalue = $"{user.realname}";
                        }
                        break;
                    }
                case "department":
                    {
                        var dept = deptBLL.GetEntityByCache(r => r.department_id == value);
                        if (dept != null)
                        {
                            realvalue = $"{dept.fullname}";
                        }
                        break;
                    }
                case "company":
                    {
                        var company = companyBLL.GetEntityByCache(r => r.company_id == value);
                        if (company != null)
                        {
                            realvalue = $"{company.fullname}";
                        }
                        break;
                    }
                case "group":
                    {
                        var group = groupBLL.GetEntityByCache(r => r.group_id == value);
                        if (group != null)
                        {
                            realvalue = $"{group.groupname}";
                        }
                        break;
                    }
                case "groups":
                    {
                        var vals = value.SplitNoEmpty(",");
                        var strTemp = new StringBuilder();
                        foreach (var item in vals)
                        {
                            var group = groupBLL.GetEntityByCache(r => r.group_id == value);
                            strTemp.Append($"{group.groupname},");
                        }
                        realvalue = $"{strTemp.ToString().TrimEnd(',')}";
                        break;
                    }
                case "dataitem":
                    {
                        var format = col.showformat.ToObject<DbFormatModel>();
                        if (format != null)
                        {
                            var items = dataitemBLL.GetDetailByCache(format.itemcode);
                            if (format.multiple)
                            {
                                var vals = value.SplitNoEmpty(",");
                                var selectitems = items.Where(r => vals.Contains(r.itemvalue)).ToList();
                                realvalue = $"{string.Join(",", selectitems.Select(r => r.showname))}";
                            }
                            else
                            {
                                var selectitem = items.Where(r => r.itemvalue == value).FirstOrDefault();
                                if (selectitem != null)
                                {
                                    realvalue = $"{selectitem.showname}";
                                }
                            }
                        }
                        break;
                    }
                case "datasource":
                    {
                        var format = col.showformat.ToObject<DbFormatModel>();
                        if (format != null)
                        {

                        }
                        break;
                    }
                case "datalocal":
                    {
                        var format = col.showformat.ToObject<DbFormatModel>();
                        if (format != null)
                        {

                        }
                        break;
                    }
                default:
                    break;
            }
            return realvalue;
        }

    }
}
