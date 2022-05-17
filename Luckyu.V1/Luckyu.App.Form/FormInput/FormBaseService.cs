using Luckyu.App.Organization;
using Luckyu.DataAccess;
using Luckyu.Log;
using Luckyu.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.Form
{
    public class FormBaseService
    {
        private DataAuthorizeBLL dataBLL = new DataAuthorizeBLL();

        public JqgridDatatablePageResponse Page(JqgridPageRequest jqPage, string form_id, UserModel loginInfo)
        {
            var repo = new Repository();
            var formEntity = repo.db.Queryable<form_tableEntity>().Where(r => r.form_id == form_id).First();
            if (formEntity == null)
            {
                throw new Exception("没有配置自定义表单");
            }
            var sqlparams = new List<SugarParameter>();
            var sql = new StringBuilder();
            sql.Append($"select * from {formEntity.dbname} where l_is_delete = 0 ");

            #region 数据权限
            var dataauth = dataBLL.GetDataAuthByUser(2, form_id, loginInfo);
            if (dataauth != null)
            {
                var strAuth = new StringBuilder();
                strAuth.Append($" l_create_userid = @userid ");
                sqlparams.Add(new SugarParameter("userid", loginInfo.user_id));
                if (!dataauth.IsAll)
                {
                    var authusers = dataauth.AllUserIds;
                    var strAuth1 = new StringBuilder($" l_create_userid in (@authusers) ");
                    sqlparams.Add(new SugarParameter("authusers", authusers));
                    if (dataauth.staterange == 0)
                    {
                        strAuth.Append($" and state = 1 ");
                    }
                    strAuth.Append($" or ( {strAuth1} ) ");
                }
                else
                {
                    if (dataauth.staterange == 0)
                    {
                        strAuth.Append($" or state = 1 ");
                    }
                }
                sql.Append($" and ( {strAuth} ) ");
            }
            else
            {
                sql.Append($" and l_create_userid = @userid ");
                sqlparams.Add(new SugarParameter("userid", loginInfo.user_id));
            }
            #endregion

            var page = repo.GetDataTablePage(jqPage, sql.ToString(), sqlparams);
            return page;
        }

        public Dictionary<string, object> GetEntity(string form_id, string keyValue)
        {
            var repo = new Repository();
            var formEntity = repo.db.Queryable<form_tableEntity>().Where(r => r.form_id == form_id).First();
            if (formEntity == null)
            {
                throw new Exception("没有配置自定义表单");
            }
            var sql = $"select * from {formEntity.dbname} where l_id = @keyValue ";
            var sqlparams = new List<SugarParameter>();
            sqlparams.Add(new SugarParameter("keyValue", keyValue));

            var dt = repo.db.Ado.GetDataTable(sql, sqlparams);
            var dicEntity = new Dictionary<string, object>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var colname = dt.Columns[i].ColumnName;
                    var value = dt.Rows[0][colname];

                    dicEntity.Add(colname, value);
                }
            }
            else
            {
                dicEntity = null;
            }
            return dicEntity;
        }

        /// <summary>
        /// 返回列定义数据源
        /// </summary>
        /// <param name="formcode"></param>
        /// <param name="columncode"></param>
        /// <returns></returns>
        public DataTable GetDataSource(string formcode, string columncode)
        {
            var repo = new Repository();
            var formEntity = repo.db.Queryable<form_tableEntity>().Where(r => r.formcode == formcode).First();
            if (formEntity == null)
            {
                return null;
            }
            var formColumn = repo.db.Queryable<form_columnEntity>().Where(r => r.form_id == formEntity.form_id && r.columncode == columncode).First();
            if (formColumn == null)
            {
                return null;
            }
            var sql = formColumn.datasource;
            var dt = repo.db.Ado.GetDataTable(sql);
            return dt;
        }

        public void DeleteForm(string form_id, string keyValue, UserModel loginInfo)
        {
            var trans = new Repository().BeginTrans();

            try
            {
                var formEntity = trans.db.Queryable<form_tableEntity>().Where(r => r.form_id == form_id).First();
                if (formEntity == null)
                {
                    throw new Exception("没有配置自定义表单");
                }
                var sql = $@"
update {formEntity.dbname} 
set l_is_delete = 1, l_deletetime = @now, l_delete_userid = @userid, l_delete_username = @username
where l_id = @keyValue ";
                var sqlparams = new List<SugarParameter>();
                sqlparams.Add(new SugarParameter("keyValue", keyValue));
                sqlparams.Add(new SugarParameter("now", DateTime.Now));
                sqlparams.Add(new SugarParameter("userid", loginInfo.user_id));
                sqlparams.Add(new SugarParameter("username", $"{loginInfo.loginname}-{loginInfo.realname}"));

                trans.db.Ado.ExecuteCommand(sql, sqlparams);
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }

        public string SaveForm(string form_id, string keyValue, Dictionary<string, object> dicEntity, UserModel loginInfo)
        {
            var trans = new Repository().BeginTrans();

            try
            {
                var formEntity = trans.db.Queryable<form_tableEntity>().Where(r => r.form_id == form_id).First();
                if (formEntity == null)
                {
                    throw new Exception("没有配置自定义表单");
                }
                if (keyValue.IsEmpty())
                {
                    var dbCols = trans.db.DbMaintenance.GetColumnInfosByTableName(formEntity.dbname);

                    var strFields = new StringBuilder();
                    var strValues = new StringBuilder();
                    var sqlparams = new List<SugarParameter>();

                    for (int i = 0; i < dbCols.Count; i++)
                    {
                        var dbcol = dbCols[i];
                        strFields.Append($"{dbcol.DbColumnName},");
                        strFields.Append($"@{dbcol.DbColumnName},");
                        if (dicEntity.ContainsKey(dbcol.DbColumnName))
                        {
                            sqlparams.Add(new SugarParameter(dbcol.DbColumnName, dicEntity[dbcol.DbColumnName]));
                        }
                        else
                        {
                            switch (dbcol.DbColumnName)
                            {
                                case "l_id":
                                    keyValue = SnowflakeHelper.NewCode();
                                    sqlparams.Add(new SugarParameter(dbcol.DbColumnName, keyValue));
                                    break;
                                case "l_create_userid":
                                    sqlparams.Add(new SugarParameter(dbcol.DbColumnName, loginInfo.user_id));
                                    break;
                                case "l_create_username":
                                    sqlparams.Add(new SugarParameter(dbcol.DbColumnName, $"{loginInfo.loginname}-{loginInfo.realname}"));
                                    break;
                                case "l_createtime":
                                    sqlparams.Add(new SugarParameter(dbcol.DbColumnName, DateTime.Now));
                                    break;
                                case "l_state":
                                    sqlparams.Add(new SugarParameter(dbcol.DbColumnName, 0));
                                    break;
                                case "l_bno":
                                    sqlparams.Add(new SugarParameter(dbcol.DbColumnName, ""));
                                    break;
                                default:
                                    sqlparams.Add(new SugarParameter(dbcol.DbColumnName, LuckyuHelper.DefaultForType(dbcol.PropertyType)));
                                    break;
                            }
                        }
                    }

                    var sql = $"insert into {formEntity.dbname} ( {strFields.ToString().TrimEnd(',')} ) value ( {strValues.ToString().TrimEnd(',')} )";
                    trans.db.Ado.ExecuteCommand(sql, sqlparams);
                }
                else
                {
                    var strFields = new StringBuilder();
                    var sqlparams = new List<SugarParameter>();
                    sqlparams.Add(new SugarParameter("keyValue", keyValue));
                    foreach (var dic in dicEntity)
                    {
                        strFields.Append($"{dic.Key} = @{dic.Key},");
                        sqlparams.Add(new SugarParameter(dic.Key, dic.Value));
                    }

                    var sql = $"update {formEntity.dbname} set {strFields} where l_id = @keyValue ";
                    trans.db.Ado.ExecuteCommand(sql, sqlparams);
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            return keyValue;
        }
    }
}
