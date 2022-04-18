using SqlSugar;
using Luckyu.Log;
using Luckyu.Utility;

namespace Luckyu.App.Workflow
{
	/// <summary>
	///  wf_flow_scheme   
	/// </summary>
	[SugarTable("wf_flow_scheme")]
	public class wf_flow_schemeEntity 
	{
		#region 属性

		/// <summary>
		///  scheme_id   
		/// </summary>
		[SugarColumn(IsPrimaryKey = true)]
		public string scheme_id { get; set; }

		/// <summary>
		///  flow_id   
		/// </summary>
		public string flow_id { get; set; }

		/// <summary>
		///  schemejson   
		/// </summary>
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
