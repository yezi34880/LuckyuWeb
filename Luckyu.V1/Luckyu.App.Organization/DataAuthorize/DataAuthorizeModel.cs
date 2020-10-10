using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.Organization
{
    public class DataAuthorizeModel
    {
        public bool IsAll { get; set; }

        public List<string> UserIds { get; set; }

        public List<string> CompanyIds { get; set; }

        public List<string> DepartmentIds { get; set; }

        public List<string> GroupIds { get; set; }

        public List<string> PostIds { get; set; }

        public List<string> RoleIds { get; set; }

        #region Var
        private UserBLL userBLL = new UserBLL();
        private UserRelationBLL relationBLL = new UserRelationBLL();
        #endregion
        /// <summary>
        /// 转换各种机构为其下的人员 如公司 转换为该公司下的所有用户
        /// </summary>
        public List<string> AllUserIds
        {
            get
            {
                var userIds = new List<string>();
                if (!this.UserIds.IsEmpty())
                {
                    userIds.AddRangeNoRepeat(this.UserIds);
                }
                var alluser = userBLL.GetAllByCache();
                if (!this.CompanyIds.IsEmpty())
                {
                    foreach (var comapnyid in this.CompanyIds)
                    {
                        var users = alluser.Where(r => r.company_id == comapnyid).Select(r => r.user_id).ToList();
                        userIds.AddRangeNoRepeat(users);
                    }
                }
                if (!this.DepartmentIds.IsEmpty())
                {
                    foreach (var deptid in this.DepartmentIds)
                    {
                        var users = alluser.Where(r => r.department_id == deptid).Select(r => r.user_id).ToList();
                        userIds.AddRangeNoRepeat(users);
                    }
                }
                var relations = relationBLL.GetAllByCache();
                if (!this.PostIds.IsEmpty())
                {
                    foreach (var postid in this.PostIds)
                    {
                        var users = relations.Where(r => r.relationtype == (int)UserRelationType.Post && r.object_id == postid).Select(r => r.user_id).ToList();
                        userIds.AddRangeNoRepeat(users);
                    }
                }
                if (!this.RoleIds.IsEmpty())
                {
                    foreach (var roleid in this.RoleIds)
                    {
                        var users = relations.Where(r => r.relationtype == (int)UserRelationType.Role && r.object_id == roleid).Select(r => r.user_id).ToList();
                        userIds.AddRangeNoRepeat(users);
                    }
                }
                if (!this.GroupIds.IsEmpty())
                {
                    foreach (var groupid in this.GroupIds)
                    {
                        var users = relations.Where(r => r.relationtype == (int)UserRelationType.Group && r.object_id == groupid).Select(r => r.user_id).ToList();
                        userIds.AddRangeNoRepeat(users);
                    }
                }
                userIds = userIds.Distinct().ToList();
                return userIds;
            }
        }
    }
}
