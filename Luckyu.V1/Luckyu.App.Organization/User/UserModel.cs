using System;
using System.Collections.Generic;

namespace Luckyu.App.Organization
{
    public class UserModel
    {

        public string user_id { get; set; }

        public string company_id { get; set; }

        public string department_id { get; set; }

        public string companyname { get; set; }

        public string departmentname { get; set; }

        public List<string> group_ids { get; set; }

        public List<string> role_ids { get; set; }

        public List<string> post_ids { get; set; }

        public List<sys_departmentmanageEntity> managedepartments { get; set; }

        public List<sys_moduleEntity> modules { get; set; }

        public string usercode { get; set; }

        public string realname { get; set; }

        public string nickname { get; set; }

        public string loginname { get; set; }

        public string mobile { get; set; }

        public string email { get; set; }

        public DateTime? birthday { get; set; }

        public string qq { get; set; }

        public string wechat { get; set; }

        public int level { get; set; }

        public string remark { get; set; }

        public int is_enable { get; set; }

        /// <summary>
        /// 记录用户登录信息
        /// </summary>
        public string token { get; set; }

    }
}