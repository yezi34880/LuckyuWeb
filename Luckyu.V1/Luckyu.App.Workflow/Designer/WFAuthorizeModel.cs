using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.Workflow
{
    public class WFAuthorizeModel
    {
        /// <summary>
        /// 1-用户 2-部门 3-公司 4-岗位 5-角色 6-小组 9-提交人自己
        /// </summary>
        public int objecttype { get; set; }

        public string objectids { get; set; }

        public string objectnames { get; set; }

        /// <summary>
        /// 1-同一公司 2-同一部门 3-分管此部门
        /// </summary>
        public int objectrange { get; set; }
    }
}
