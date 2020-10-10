using SqlSugar;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Workflow
{
	/// <summary>
	///  sys_coderule_seed   
	/// </summary>
	[SugarTable("SYS_CODERULE_SEED","")]
	public class sys_coderule_seedEntity 
	{
        #region 属性

		/// <summary>
		///  seed_id   
		/// </summary>
		[SugarColumn(ColumnName = "SEED_ID" ,IsPrimaryKey = true,ColumnDescription="")]
		public string seed_id { get; set; }

		/// <summary>
		///  rule_id   
		/// </summary>
		[SugarColumn(ColumnName = "RULE_ID",ColumnDescription="")]
		public string rule_id { get; set; }

		/// <summary>
		///  seedprename   前缀
		/// </summary>
		[SugarColumn(ColumnName = "SEEDPRENAME",ColumnDescription="前缀")]
		public string seedprename { get; set; }

		/// <summary>
		///  seedvalue   流水号
		/// </summary>
		[SugarColumn(ColumnName = "SEEDVALUE",ColumnDescription="流水号")]
		public int seedvalue { get; set; }

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

        #endregion
	}
}
