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

        /// <summary>
        /// 0 仅查看生效 1 查看所有状态
        /// </summary>
        public int staterange { get; set; }

        /// <summary>
        /// 0 仅查看 1 可编辑起草 驳回 2 可编辑所有
        /// </summary>
        public int edittype { get; set; }

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
                        var users = relations.Where(r => r.relationtype == (int)UserRelationEnum.Post && r.object_id == postid).Select(r => r.user_id).ToList();
                        userIds.AddRangeNoRepeat(users);
                    }
                }
                if (!this.RoleIds.IsEmpty())
                {
                    foreach (var roleid in this.RoleIds)
                    {
                        var users = relations.Where(r => r.relationtype == (int)UserRelationEnum.Role && r.object_id == roleid).Select(r => r.user_id).ToList();
                        userIds.AddRangeNoRepeat(users);
                    }
                }
                if (!this.GroupIds.IsEmpty())
                {
                    foreach (var groupid in this.GroupIds)
                    {
                        var users = relations.Where(r => r.relationtype == (int)UserRelationEnum.Group && r.object_id == groupid).Select(r => r.user_id).ToList();
                        userIds.AddRangeNoRepeat(users);
                    }
                }
                userIds = userIds.Distinct().ToList();
                return userIds;
            }
        }
    }
}
