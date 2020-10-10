using Luckyu.Log;
using Luckyu.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Workflow
{
	/// <summary>
	///  wf_flow_scheme   
	/// </summary>
	[SugarTable("WF_FLOW_SCHEME","")]
	public class wf_flow_schemeEntity 
	{
        #region 属性

		/// <summary>
		///  scheme_id   
		/// </summary>
		[SugarColumn(ColumnName = "SCHEME_ID" ,IsPrimaryKey = true,ColumnDescription="")]
		public string scheme_id { get; set; }

		/// <summary>
		///  flow_id   
		/// </summary>
		[SugarColumn(ColumnName = "FLOW_ID",ColumnDescription="")]
		public string flow_id { get; set; }

		/// <summary>
		///  schemejson   
		/// </summary>
		[SugarColumn(ColumnName = "SCHEMEJSON",ColumnDescription="")]
		public string schemejson { get; set; }

		#endregion

		#region 方法
		public void Create(string flowId)
		{
			if (this.scheme_id.IsEmpty())
            {
				this.scheme_id = SnowflakeHelper.NewCode();
            }
			this.flow_id = flowId;
		}

		#endregion
	}
}
