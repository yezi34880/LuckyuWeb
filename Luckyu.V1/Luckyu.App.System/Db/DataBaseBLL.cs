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

        public void InsertLog<T>(T entity, UserModel loginInfo) where T : class, new()
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
            var log = new sys_logEntity();
            log.log_type = (int)LogType.Operation;
            log.module = $"{ tableInfo.showname} {tableInfo.dbname}";
            log.op_type = "新增";

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
                strResult.Append($"{col.dbcolumnname} {realvalue}");
            }

            log.log_content = strResult.ToString();
            LogBLL.WriteLog(log);
        }

        public void DeleteLog<T>(T entity, UserModel loginInfo) where T : class, new()
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
            var log = new sys_logEntity();
            log.log_type = (int)LogType.Operation;
            log.module = $"{ tableInfo.showname} {tableInfo.dbname}";
            log.op_type = "删除";

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
                strResult.Append($"{col.dbcolumnname} {realvalue}");
            }

            log.log_content = strResult.ToString();
            LogBLL.WriteLog(log);
        }

        public void UpdateLog<T>(T from, T to, UserModel loginInfo) where T : class, new()
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
            var log = new sys_logEntity();
            log.log_type = (int)LogType.Operation;
            log.module = $"{ tableInfo.showname} {tableInfo.dbname}";
            log.op_type = "修改";

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
                var fromValue = propety.GetValue(from).ToString();
                var toValue = propety.GetValue(to).ToString();
                if (fromValue != toValue)
                {
                    var fromRealValue = GetRealValue(col, fromValue);
                    var toRealValue = GetRealValue(col, toValue);
                    strResult.Append($"{fromRealValue} => {toRealValue}");
                }
            }

            log.log_content = strResult.ToString();
            LogBLL.WriteLog(log);
        }

        private string GetRealValue(sys_dbcolumnEntity col, string value)
        {
            string realvalue = "";
            switch (col.showtype)
            {
                case "text":
                case "datetime":
                case "number":
                    realvalue = $"{value}\r\n";
                    break;
                case "user":
                    {
                        var user = userBLL.GetEntityByCache(r => r.user_id == value);
                        if (user != null)
                        {
                            realvalue = $"{user.realname}\r\n";
                        }
                        break;
                    }
                case "department":
                    {
                        var dept = deptBLL.GetEntityByCache(r => r.department_id == value);
                        if (dept != null)
                        {
                            realvalue = $"{dept.fullname}\r\n";
                        }
                        break;
                    }
                case "company":
                    {
                        var company = companyBLL.GetEntityByCache(r => r.company_id == value);
                        if (company != null)
                        {
                            realvalue = $"{company.fullname}\r\n";
                        }
                        break;
                    }
                case "group":
                    {
                        var group = groupBLL.GetEntityByCache(r => r.group_id == value);
                        if (group != null)
                        {
                            realvalue = $"{group.groupname}\r\n";
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
                        realvalue = $"{strTemp.ToString().TrimEnd(',')}\r\n";
                        break;
                    }
                case "dataitem":
                    {
                        var format = JsonConvert.DeserializeObject<DbFormatModel>(col.showformat);
                        if (format != null)
                        {
                            var items = dataitemBLL.GetDetailByCache(format.itemcode);
                            if (format.multiple)
                            {
                                var vals = value.SplitNoEmpty(",");
                                var selectitems = items.Where(r => vals.Contains(r.itemvalue)).ToList();
                                realvalue = $"{string.Join(",", selectitems.Select(r => r.showname))}\r\n";
                            }
                            else
                            {
                                var selectitem = items.Where(r => r.itemvalue == value).FirstOrDefault();
                                if (selectitem != null)
                                {
                                    realvalue = $"{selectitem.showname}\r\n";
                                }
                            }
                        }
                        break;
                    }
                case "datasource":
                    {
                        var format = JsonConvert.DeserializeObject<DbFormatModel>(col.showformat);
                        if (format != null)
                        {

                        }
                        break;
                    }
                case "datalocal":
                    {
                        var format = JsonConvert.DeserializeObject<DbFormatModel>(col.showformat);
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
