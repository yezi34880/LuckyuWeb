

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
                trans.Insert(table);
                var dbCols = new List<DbColumnInfo>();
                for (int i = 0; i < cols.Count; i++)
                {
                    var col = cols[i];

                    col.form_id = table.form_id;
                    col.column_id = SnowflakeHelper.NewCode();

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

                    dbCol.DataType = col.dbtype;
                    dbCol.Length = col.dblength;
                    dbCol.DecimalDigits = col.dbdigits;
                    dbCol.DefaultValue = col.defaultvalue;
                    dbCols.Add(dbCol);
                }
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "id",
                    ColumnDescription = "主键",
                    DataType = "varchar",
                    Length = 50,
                    IsPrimarykey = true,
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "bno",
                    ColumnDescription = "编号",
                    DataType = "varchar",
                    Length = 50,
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "state",
                    ColumnDescription = "状态",
                    DataType = "int",
                    Length = 2,
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "remark",
                    ColumnDescription = "备注",
                    DataType = "varchar",
                    Length = 255,
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "create_userid",
                    ColumnDescription = "",
                    DataType = "varchar",
                    Length = 50,
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "create_username",
                    ColumnDescription = "",
                    DataType = "varchar",
                    Length = 50,
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "createtime",
                    ColumnDescription = "",
                    DataType = "datetime",
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "edittime",
                    ColumnDescription = "",
                    DataType = "datetime",
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "edit_userid",
                    ColumnDescription = "",
                    DataType = "varchar",
                    Length = 50,
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "edit_username",
                    ColumnDescription = "",
                    DataType = "varchar",
                    Length = 255,
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "is_delete",
                    ColumnDescription = "",
                    DataType = "int",
                    Length = 2,
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "deletetime",
                    ColumnDescription = "",
                    DataType = "datetime",
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "delete_userid",
                    ColumnDescription = "",
                    DataType = "varchar",
                    Length = 50,
                });
                dbCols.Add(new DbColumnInfo
                {
                    DbColumnName = "delete_username",
                    ColumnDescription = "",
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