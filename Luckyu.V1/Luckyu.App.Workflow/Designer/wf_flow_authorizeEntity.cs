using SqlSugar;
using Luckyu.Log;
using Luckyu.Utility;

namespace Luckyu.App.Workflow
{
    /// <summary>
    ///  wf_flow_authorize   
    /// </summary>
    [SugarTable( "wf_flow_authorize")]
    public class wf_flow_authorizeEntity
    {
        #region 属性

        /// <summary>
        ///  auth_id   
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string auth_id { get; set; }

        /// <summary>
        ///  flow_id   
        /// </summary>
        public string flow_id { get; set; }

        /// <summary>
        ///  objecttype   1用户2部门3公司4岗位5角色
        /// </summary>
        public int objecttype { get; set; }

        /// <summary>
        ///  object_id   
        /// </summary>
        public string object_id { get; set; }

        #endregion

        #region 方法
        public void Create(string flowId)
        {
            if (this.auth_id.IsEmpty())
            {
                this.auth_id = SnowflakeHelper.NewCode();
            }
            this.flow_id = flowId;
        }
        #endregion
    }

}
