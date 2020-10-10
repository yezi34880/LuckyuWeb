using SqlSugar;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Workflow
{
	/// <summary>
	///  sys_coderule   
	/// </summary>
	[SugarTable("SYS_CODERULE","")]
	public class sys_coderuleEntity 
	{
        #region 属性

		/// <summary>
		///  rule_id   
		/// </summary>
		[SugarColumn(ColumnName = "RULE_ID" ,IsPrimaryKey = true,ColumnDescription="")]
		public string rule_id { get; set; }

		/// <summary>
		///  rulecode   
		/// </summary>
		[SugarColumn(ColumnName = "RULECODE",ColumnDescription="")]
		public string rulecode { get; set; }

		/// <summary>
		///  rulename   
		/// </summary>
		[SugarColumn(ColumnName = "RULENAME",ColumnDescription="")]
		public string rulename { get; set; }

		/// <summary>
		///  contentjson   
		/// </summary>
		[SugarColumn(ColumnName = "CONTENTJSON",ColumnDescription="")]
		public string contentjson { get; set; }

		/// <summary>
		///  remark   
		/// </summary>
		[SugarColumn(ColumnName = "REMARK",ColumnDescription="")]
		public string remark { get; set; }

		/// <summary>
		///  create_userId   
		/// </summary>
		[SugarColumn(ColumnName = "CREATE_USERID",ColumnDescription="")]
		public string create_userId { get; set; }

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
		///  deletetime   
		/// </summary>
		[SugarColumn(ColumnName = "DELETETIME",ColumnDescription="")]
		public DateTime? deletetime { get; set; }

		/// <summary>
		///  delete_username   
		/// </summary>
		[SugarColumn(ColumnName = "DELETE_USERNAME",ColumnDescription="")]
		public string delete_username { get; set; }

		/// <summary>
		///  is_enable   
		/// </summary>
		[SugarColumn(ColumnName = "IS_ENABLE",ColumnDescription="")]
		public int is_enable { get; set; }

        #endregion
	}
}
