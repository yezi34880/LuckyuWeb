using Luckyu.Log;
using Luckyu.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Organization
{
	/// <summary>
	///  sys_department   
	/// </summary>
	[SugarTable("SYS_DEPARTMENT","")]
	public class sys_departmentEntity : ExtensionEntityBase
	{
        #region 属性

		/// <summary>
		///  department_id   
		/// </summary>
		[SugarColumn(ColumnName = "DEPARTMENT_ID" ,IsPrimaryKey = true,ColumnDescription="")]
		public string department_id { get; set; }

		/// <summary>
		///  company_id   
		/// </summary>
		[SugarColumn(ColumnName = "COMPANY_ID",ColumnDescription="")]
		public string company_id { get; set; }

		/// <summary>
		///  parent_id   
		/// </summary>
		[SugarColumn(ColumnName = "PARENT_ID",ColumnDescription="")]
		public string parent_id { get; set; }

		/// <summary>
		///  departmentcode   
		/// </summary>
		[SugarColumn(ColumnName = "DEPARTMENTCODE",ColumnDescription="")]
		public string departmentcode { get; set; }

		/// <summary>
		///  fullname   
		/// </summary>
		[SugarColumn(ColumnName = "FULLNAME",ColumnDescription="")]
		public string fullname { get; set; }

		/// <summary>
		///  shortname   
		/// </summary>
		[SugarColumn(ColumnName = "SHORTNAME",ColumnDescription="")]
		public string shortname { get; set; }

		/// <summary>
		///  email   
		/// </summary>
		[SugarColumn(ColumnName = "EMAIL",ColumnDescription="")]
		public string email { get; set; }

		/// <summary>
		///  phone   
		/// </summary>
		[SugarColumn(ColumnName = "PHONE",ColumnDescription="")]
		public string phone { get; set; }

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
			this.department_id = SnowflakeHelper.NewCode();
			this.createtime = DateTime.Now;
			this.create_userid = loginInfo.user_id;
			this.create_username = loginInfo.realname;

			this.parent_id = this.parent_id.IsEmpty() ? "0" : this.parent_id;
		}
		public void Modify(string keyValue, UserModel loginInfo)
		{
			this.department_id = keyValue;
			this.edittime = DateTime.Now;
			this.edit_userid = loginInfo.user_id;
			this.edit_username = loginInfo.realname;

			this.parent_id = this.parent_id.IsEmpty() ? "0" : this.parent_id;
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
