using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;

namespace Luckyu.App.System
{
	/// <summary>
	///  sys_config   
	/// </summary>
	[Table(Name = "SYS_CONFIG")]
	public class sys_configEntity 
	{
		#region 属性

		/// <summary>
		///  id   
		/// </summary>
		[Column(IsPrimary = true)]
		public string id { get; set; }

		/// <summary>
		///  configcode   
		/// </summary>
		public string configcode { get; set; }

		/// <summary>
		///  configname   
		/// </summary>
		public string configname { get; set; }

		/// <summary>
		///  configvalue   
		/// </summary>
		public string configvalue { get; set; }

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
