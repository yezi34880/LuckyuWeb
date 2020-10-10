using Luckyu.Log;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Organization
{
	/// <summary>
	///  sys_role   
	/// </summary>
	[SugarTable("SYS_ROLE","")]
	public class sys_roleEntity 
	{
        #region 属性

		/// <summary>
		///  role_id   
		/// </summary>
		[SugarColumn(ColumnName = "ROLE_ID" ,IsPrimaryKey = true,ColumnDescription="")]
		public string role_id { get; set; }

		/// <summary>
		///  rolecode   
		/// </summary>
		[SugarColumn(ColumnName = "ROLECODE", ColumnDescription="")]
		public string rolecode { get; set; }

		/// <summary>
		///  rolename   
		/// </summary>
		[SugarColumn(ColumnName = "ROLENAME",ColumnDescription="")]
		public string rolename { get; set; }

		/// <summary>
		///  sort   
		/// </summary>
		[SugarColumn(ColumnName = "SORT",ColumnDescription="")]
		public int sort { get; set; }

		/// <summary>
		///  remark   
		/// </summary>
		[SugarColumn(ColumnName = "REMARK",ColumnDescription="")]
		public string remark { get; set; }

		/// <summary>
		///  create_userid   
		/// </summary>
		[SugarColumn(ColumnName = "CREATE_USERID",ColumnDescription="")]
		public string create_userid { get; set; }

		/// <summary>
		///  create_username   
		/// </summary>
		[SugarColumn(ColumnName = "CREATE_USERNAME",ColumnDescription="")]
		public string create_username { get; set; }

		/// <summary>
		///  createtime   
		/// </summary>
		[SugarColumn(ColumnName = "CREATETIME",ColumnDescription="")]
		public DateTime? createtime { get; set; }

		/// <summary>
		///  edittime   
		/// </summary>
		[SugarColumn(ColumnName = "EDITTIME",ColumnDescription="")]
		public DateTime? edittime { get; set; }

		/// <summary>
		///  edit_userid   
		/// </summary>
		[SugarColumn(ColumnName = "EDIT_USERID",ColumnDescription="")]
		public string edit_userid { get; set; }

		/// <summary>
		///  edit_username   
		/// </summary>
		[SugarColumn(ColumnName = "EDIT_USERNAME",ColumnDescription="")]
		public string edit_username { get; set; }

		/// <summary>
		///  is_delete   
		/// </summary>
		[SugarColumn(ColumnName = "IS_DELETE",ColumnDescription="")]
		public int is_delete { get; set; }

		/// <summary>
		///  delete_userid   
		/// </summary>
		[SugarColumn(ColumnName = "DELETE_USERID",ColumnDescription="")]
		public string delete_userid { get; set; }

		/// <summary>
		///  delete_username   
		/// </summary>
		[SugarColumn(ColumnName = "DELETE_USERNAME",ColumnDescription="")]
		public string delete_username { get; set; }

		/// <summary>
		///  deletetime   
		/// </summary>
		[SugarColumn(ColumnName = "DELETETIME", ColumnDescription = "")]
		public DateTime? deletetime { get; set; }

		/// <summary>
		///  is_enable   
		/// </summary>
		[SugarColumn(ColumnName = "IS_ENABLE",ColumnDescription="")]
		public int is_enable { get; set; }

		#endregion

		#region 方法
		public void Create(UserModel loginInfo)
		{
			this.role_id = SnowflakeHelper.NewCode();
			this.createtime = DateTime.Now;
			this.create_userid = loginInfo.user_id;
			this.create_username = loginInfo.realname;
		}
		public void Modify(string keyValue, UserModel loginInfo)
		{
			this.role_id = keyValue;
			this.edittime = DateTime.Now;
			this.edit_userid = loginInfo.user_id;
			this.edit_username = loginInfo.realname;
		}
		public void Remove(UserModel loginInfo)
		{
			this.is_delete = 1;
			this.deletetime = DateTime.Now;
			this.delete_userid = loginInfo.user_id;
			this.delete_username = loginInfo.realname;
		}
		#endregion

	}
}
