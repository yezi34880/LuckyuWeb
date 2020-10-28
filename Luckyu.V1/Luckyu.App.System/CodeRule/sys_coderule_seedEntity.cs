using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Workflow
{
	/// <summary>
	///  sys_coderule_seed   
	/// </summary>
	[Table(Name = "SYS_CODERULE_SEED")]
	public class sys_coderule_seedEntity 
	{
		#region 属性

		/// <summary>
		///  seed_id   
		/// </summary>
		[Column(IsPrimary = true)]
		public string seed_id { get; set; }

		/// <summary>
		///  rule_id   
		/// </summary>
		public string rule_id { get; set; }

		/// <summary>
		///  seedprename   前缀
		/// </summary>
		public string seedprename { get; set; }

		/// <summary>
		///  seedvalue   流水号
		/// </summary>
		public int seedvalue { get; set; }

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

        #endregion
	}
}
