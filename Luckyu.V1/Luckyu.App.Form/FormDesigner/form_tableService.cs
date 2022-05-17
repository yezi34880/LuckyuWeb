using Luckyu.App.Organization;
using Luckyu.App.System;
using Luckyu.DataAccess;
using Luckyu.Log;
using Luckyu.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.Form
{
    public class form_tableService : RepositoryFactory<form_tableEntity>
    {
        #region Var

        #endregion

        public JqgridPageResponse<form_tableEntity> Page(JqgridPageRequest jqPage, UserModel loginInfo)
        {
            Expression<Func<form_tableEntity, bool>> exp = r => r.is_delete == 0;
            var page = BaseRepository().GetPage(jqPage, exp);
            return page;
        }

        public void DeleteForm(form_tableEntity entity, UserModel loginInfo)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                entity.Remove(loginInfo);
                trans.UpdateOnlyColumns(entity, r => new { r.is_delete, r.delete_userid, r.delete_username, r.deletetime });

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }

        public ResponseResult InsertTable(form_tableEntity table, List<form_columnEntity> cols, UserModel loginInfo)
        {
            var repo = BaseRepository();
            var dbtableName = $"cusform_{table.formcode}";
            var isExists = repo.db.DbMaintenance.IsAnyTable(dbtableName);
            if (isExists)
            {
                return ResponseResult.Fail("数据库表已存在，请联系管理员确认");
            }

            var trans = repo.BeginTrans();
            try
            {
                table.Create(loginInfo);
                table.dbname = dbtableName;
                trans.Insert(table);
                var dbCols = new List<DbColumnInfo>();
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "l_id",
                    ColumnDescription = "主键",
                    DataType = "varchar",
                    Length = 50,
                    IsPrimarykey = true,
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "l_bno",
                    ColumnDescription = "编号",
                    DataType = "varchar",
                    Length = 50,
                });

                for (int i = 0; i < cols.Count; i++)
                {
                    var col = cols[i];
                    if (col.columntype == "upload")
                    {
                        continue;
                    }

                    col.form_id = table.form_id;
                    col.is_visible = 1;
                    col.column_id = SnowflakeHelper.NewCode();
                    col.createtime = DateTime.Now;
                    col.create_userid = loginInfo.user_id;
                    col.create_username = $"{loginInfo.loginname}-{loginInfo.realname}";

                    var dbCol = new DbColumnInfo();
                    dbCol.DbColumnName = col.columncode;
                    dbCol.ColumnDescription = col.columnname;
                    if (trans.db.CurrentConnectionConfig.DbType == DbType.SqlServer)
                    {
                        if (col.dbtype == "varchar")
                        {
                            col.dbtype = "nvarchar";
                        }
                        else if (col.dbtype == "text")
                        {
                            col.dbtype = "nvarchar(MAX)";
                        }
                    }

                    if (col.dbtype == "varchar" || col.dbtype == "text")
                    {
                        dbCol.IsNullable = true;
                    }

                    dbCol.DataType = col.dbtype;
                    dbCol.Length = col.dblength;
                    dbCol.DecimalDigits = col.dbdigits;
                    dbCol.DefaultValue = col.defaultvalue;
                    dbCols.Add(dbCol);
                }

                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "l_remark",
                    IsNullable = true,
                    ColumnDescription = "备注",
                    DataType = "varchar",
                    Length = 255,
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "l_state",
                    ColumnDescription = "状态",
                    DataType = "int",
                    Length = 2,
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "l_create_userid",
                    IsNullable = true,
                    DataType = "varchar",
                    Length = 50,
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "l_create_username",
                    IsNullable = true,
                    DataType = "varchar",
                    Length = 50,
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "l_createtime",
                    IsNullable = true,
                    DataType = "datetime",
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "l_edittime",
                    IsNullable = true,
                    DataType = "datetime",
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "l_edit_userid",
                    IsNullable = true,
                    DataType = "varchar",
                    Length = 50,
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "l_edit_username",
                    IsNullable = true,
                    DataType = "varchar",
                    Length = 255,
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "l_is_delete",
                    DataType = "int",
                    Length = 2,
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "l_deletetime",
                    IsNullable = true,
                    DataType = "datetime",
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "l_delete_userid",
                    IsNullable = true,
                    DataType = "varchar",
                    Length = 50,
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "l_delete_username",
                    IsNullable = true,
                    DataType = "varchar",
                    Length = 255,
                });

                trans.Insert(cols);
                trans.db.DbMaintenance.CreateTable(dbtableName, dbCols);

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            return ResponseResult.Success();
        }

        public ResponseResult UpdateTable(string keyValue, form_tableEntity table, List<form_columnEntity> cols, UserModel loginInfo)
        {
            var repo = BaseRepository();
            var dbtableName = $"cusform_{table.formcode}";
            var isExists = repo.db.DbMaintenance.IsAnyTable(dbtableName);
            if (!isExists)
            {
                return ResponseResult.Fail("数据库表不存在，不能修改，请联系管理员确认");
            }

            var trans = repo.BeginTrans();
            try
            {
                table.edittime = DateTime.Now;
                table.edit_userid = loginInfo.user_id;
                table.edit_username = $"{loginInfo.loginname}-{loginInfo.realname}";
                trans.UpdateOnlyColumns(table, r => new
                {
                    r.edittime,
                    r.edit_userid,
                    r.edit_username,
                    r.formhtml,
                    r.formjson,
                    r.formname,
                    r.remark
                });

                var oldCols = trans.db.Queryable<form_columnEntity>().Where(r => r.form_id == keyValue).ToList();
                // 旧有新没有 隐藏 不要直接删除列
                var oldExsits = oldCols.Where(r => !cols.Exists(t => t.columncode == r.columncode)).ToList();
                trans.db.Updateable(oldExsits).SetColumns(r => new form_columnEntity
                {
                    is_delete = 1,
                    deletetime = DateTime.Now,
                    delete_userid = loginInfo.user_id,
                    delete_username = $"{loginInfo.loginname}-{loginInfo.realname}"
                }).ExecuteCommand();

                // 新有旧没有 增加列
                var currentExsits = cols.Where(r => !oldCols.Exists(t => t.columncode == r.columncode)).ToList();
                for (int i = 0; i < cols.Count; i++)
                {
                    var col = cols[i];
                    if (col.columntype == "upload")
                    {
                        continue;
                    }

                    col.form_id = table.form_id;
                    col.is_visible = 1;
                    col.column_id = SnowflakeHelper.NewCode();
                    col.createtime = DateTime.Now;
                    col.create_userid = loginInfo.user_id;
                    col.create_username = $"{loginInfo.loginname}-{loginInfo.realname}";

                    var dbCol = new DbColumnInfo();
                    dbCol.DbColumnName = col.columncode;
                    dbCol.ColumnDescription = col.columnname;
                    if (trans.db.CurrentConnectionConfig.DbType == DbType.SqlServer)
                    {
                        if (col.dbtype == "varchar")
                        {
                            col.dbtype = "nvarchar";
                        }
                        else if (col.dbtype == "text")
                        {
                            col.dbtype = "nvarchar(max)";
                        }
                    }

                    if (col.dbtype == "varchar" || col.dbtype == "text")
                    {
                        dbCol.IsNullable = true;
                    }

                    dbCol.DataType = col.dbtype;
                    dbCol.Length = col.dblength;
                    dbCol.DecimalDigits = col.dbdigits;
                    dbCol.DefaultValue = col.defaultvalue;
                    trans.db.DbMaintenance.AddColumn(dbtableName, dbCol);
                }

                // 新有 旧有 修改属性
                var allExsits = cols.Where(r => !oldCols.Exists(t => t.columncode == r.columncode)).ToList();
                for (int i = 0; i < allExsits.Count; i++)
                {
                    var col = allExsits[i];
                    trans.db.Updateable(col).UpdateColumns(r => new
                    {
                        r.dataitemcode,
                        r.columnname,
                        r.defaultvalue,
                        r.formlength,
                    }).ExecuteCommand();
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            return ResponseResult.Success();
        }

    }
}