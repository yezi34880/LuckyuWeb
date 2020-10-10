using Luckyu.Log;
using Luckyu.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Workflow
{
    /// <summary>
    ///  wf_flow_authorize   
    /// </summary>
    [SugarTable("WF_FLOW_AUTHORIZE", "")]
    public class wf_flow_authorizeEntity
    {
        #region 属性

        /// <summary>
        ///  auth_id   
        /// </summary>
        [SugarColumn(ColumnName = "AUTH_ID", IsPrimaryKey = true, ColumnDescription = "")]
        public string auth_id { get; set; }

        /// <summary>
        ///  flow_id   
        /// </summary>
        [SugarColumn(ColumnName = "FLOW_ID", ColumnDescription = "")]
        public string flow_id { get; set; }

        /// <summary>
        ///  objecttype   1用户2部门3公司4岗位5角色
        /// </summary>
        [SugarColumn(ColumnName = "OBJECTTYPE", ColumnDescription = "1用户2部门3公司4岗位5角色")]
        public int objecttype { get; set; }

        /// <summary>
        ///  object_id   
        /// </summary>
        [SugarColumn(ColumnName = "OBJECT_ID", ColumnDescription = "")]
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
