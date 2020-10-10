
using Luckyu.Log;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luckyu.App.Organization
{
	/// <summary>
	///  sys_user   
	/// </summary>
	[SugarTable("SYS_USER","")]
	public class sys_userEntity 
	{
        #region 属性

		/// <summary>
		///  user_id   
		/// </summary>
		[SugarColumn(ColumnName = "USER_ID" ,IsPrimaryKey = true,ColumnDescription="")]
		public string user_id { get; set; }

		/// <summary>
		///  company_id   
		/// </summary>
		[SugarColumn(ColumnName = "COMPANY_ID",ColumnDescription="")]
		public string company_id { get; set; }

		/// <summary>
		///  department_id   
		/// </summary>
		[SugarColumn(ColumnName = "DEPARTMENT_ID",ColumnDescription="")]
		public string department_id { get; set; }

		/// <summary>
		///  usercode   
		/// </summary>
		[SugarColumn(ColumnName = "USERCODE", ColumnDescription="")]
		public string usercode { get; set; }

		/// <summary>
		///  realname   
		/// </summary>
		[SugarColumn(ColumnName = "REALNAME",ColumnDescription="")]
		public string realname { get; set; }

		/// <summary>
		///  nickname   
		/// </summary>
		[SugarColumn(ColumnName = "NICKNAME",ColumnDescription="")]
		public string nickname { get; set; }

		/// <summary>
		///  loginname   
		/// </summary>
		[SugarColumn(ColumnName = "LOGINNAME",ColumnDescription="")]
		public string loginname { get; set; }

		/// <summary>
		///  loginpassword   
		/// </summary>
		[SugarColumn(ColumnName = "LOGINPASSWORD",ColumnDescription="")]
		public string loginpassword { get; set; }

		/// <summary>
		///  loginsecret   
		/// </summary>
		[SugarColumn(ColumnName = "LOGINSECRET",ColumnDescription="")]
		public string loginsecret { get; set; }

		/// <summary>
		///  mobile   
		/// </summary>
		[SugarColumn(ColumnName = "MOBILE",ColumnDescription="")]
		public string mobile { get; set; }

		/// <summary>
		///  email   
		/// </summary>
		[SugarColumn(ColumnName = "EMAIL",ColumnDescription="")]
		public string email { get; set; }

		/// <summary>
		///  sex   1男 2女
		/// </summary>
		[SugarColumn(ColumnName = "sex", ColumnDescription="")]
		public int sex { get; set; }

		/// <summary>
		///  birthday   
		/// </summary>
		[SugarColumn(ColumnName = "BIRTHDAY",ColumnDescription="")]
		public DateTime? birthday { get; set; }

		/// <summary>
		///  qq   
		/// </summary>
		[SugarColumn(ColumnName = "QQ",ColumnDescription="")]
		public string qq { get; set; }

		/// <summary>
		///  wechat   
		/// </summary>
		[SugarColumn(ColumnName = "WECHAT",ColumnDescription="")]
		public string wechat { get; set; }

		/// <summary>
		///  level   0-一般用户1-管理员2-超级管理员
		/// </summary>
		[SugarColumn(ColumnName = "LEVEL",ColumnDescription="0-一般用户1-管理员2-超级管理员")]
		public int level { get; set; }

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

		#region 扩展
		[NotMapped]
		public string np_roles { get; set; }
		[NotMapped]
		public string np_posts { get; set; }
		[NotMapped]
		public string np_groups { get; set; }
		[NotMapped]
		public string np_depts { get; set; }
		#endregion

		#region 方法
		public void Create(UserModel loginInfo)
		{
			this.user_id = SnowflakeHelper.NewCode();
			this.createtime = DateTime.Now;
			this.create_userid = loginInfo.user_id;
			this.create_username = loginInfo.realname;
		}
		public void Modify(string keyValue, UserModel loginInfo)
		{
			this.user_id = keyValue;
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
