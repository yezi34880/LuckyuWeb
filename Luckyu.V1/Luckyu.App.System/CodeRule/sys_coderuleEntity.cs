using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Workflow
{
	/// <summary>
	///  sys_coderule   
	/// </summary>
	[Table(Name = "SYS_CODERULE")]
	public class sys_coderuleEntity 
	{
		#region 属性

		/// <summary>
		///  rule_id   
		/// </summary>
		[Column(IsPrimary = true)]
		public string rule_id { get; set; }

		/// <summary>
		///  rulecode   
		/// </summary>
		public string rulecode { get; set; }

		/// <summary>
		///  rulename   
		/// </summary>
		public string rulename { get; set; }

		/// <summary>
		///  contentjson   
		/// </summary>
		public string contentjson { get; set; }

		/// <summary>
		///  remark   
		/// </summary>
		public string remark { get; set; }

		/// <summary>
		///  create_userId   
		/// </summary>
		public string create_userId { get; set; }

		/// <summary>
		///  create_username   
		/// </summary>
		public string create_username { get; set; }

		/// <summary>
		///  createtime   
		/// </summary>
		public DateTime? createtime { get; set; }

		/// <summary>
		///  edittime   
		/// </summary>
		public DateTime? edittime { get; set; }

		/// <summary>
		///  edit_userid   
		/// </summary>
		public string edit_userid { get; set; }

		/// <summary>
		///  edit_username   
		/// </summary>
		public string edit_username { get; set; }

		/// <summary>
		///  is_delete   
		/// </summary>
		public int is_delete { get; set; }

		/// <summary>
		///  delete_userid   
		/// </summary>
		public string delete_userid { get; set; }

		/// <summary>
		///  deletetime   
		/// </summary>
		public DateTime? deletetime { get; set; }

		/// <summary>
		///  delete_username   
		/// </summary>
		public string delete_username { get; set; }

		/// <summary>
		///  is_enable   
		/// </summary>
		public int is_enable { get; set; }

        #endregion
	}
}
