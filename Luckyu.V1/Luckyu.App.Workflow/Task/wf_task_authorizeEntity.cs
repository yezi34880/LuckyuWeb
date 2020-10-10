using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Workflow
{
	/// <summary>
	///  wf_task_authorize   
	/// </summary>
	[SugarTable("WF_TASK_AUTHORIZE", "")]
	public class wf_task_authorizeEntity
	{
		#region 属性

		/// <summary>
		///  auth_id   
		/// </summary>
		[SugarColumn(ColumnName = "AUTH_ID", IsPrimaryKey = true,ColumnDescription="")]
		public string auth_id { get; set; }

		/// <summary>
		///  task_id   
		/// </summary>
		[SugarColumn(ColumnName = "task_id", ColumnDescription="")]
		public string task_id { get; set; }

		/// <summary>
		///  user_id   
		/// </summary>
		[SugarColumn(ColumnName = "USER_ID", ColumnDescription="")]
		public string user_id { get; set; }

		/// <summary>
		///  department_id   
		/// </summary>
		[SugarColumn(ColumnName = "DEPARTMENT_ID", ColumnDescription="")]
		public string department_id { get; set; }

		/// <summary>
		///  company_id   
		/// </summary>
		[SugarColumn(ColumnName = "COMPANY_ID", ColumnDescription="")]
		public string company_id { get; set; }

		/// <summary>
		///  role_id   
		/// </summary>
		[SugarColumn(ColumnName = "ROLE_ID", ColumnDescription="")]
		public string role_id { get; set; }

		/// <summary>
		///  post_id   
		/// </summary>
		[SugarColumn(ColumnName = "POST_ID", ColumnDescription="")]
		public string post_id { get; set; }

		/// <summary>
		///  group_id   
		/// </summary>
		[SugarColumn(ColumnName = "GROUP_ID", ColumnDescription="")]
		public string group_id { get; set; }

		/// <summary>
		///  manage_dept_id   
		/// </summary>
		[SugarColumn(ColumnName = "MANAGE_DEPT_ID", ColumnDescription="")]
		public string manage_dept_id { get; set; }

		/// <summary>
		///  is_add   
		/// </summary>
		[SugarColumn(ColumnName = "is_add", ColumnDescription="")]
		public int is_add { get; set; }

		/// <summary>
		///  create_userid   
		/// </summary>
		[SugarColumn(ColumnName = "CREATE_USERID", ColumnDescription="")]
		public string create_userid { get; set; }
        #endregion

        #region 方法
		public void Create(UserModel loginInfo)
        {
            if (this.auth_id.IsEmpty())
            {
				this.auth_id = SnowflakeHelper.NewCode();
            }
			this.create_userid = loginInfo.user_id;
		}
        #endregion

    }
}
