using Luckyu.App.Organization;
using Luckyu.App.Workflow;
using Luckyu.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Module.WorkflowModule.Controllers
{
    /// <summary>
    /// 流程设计  /WorkflowModule/Designer
    /// </summary>
    [Area("WorkflowModule")]
    public class DesignerController : MvcControllerBase
    {
        #region Var
        private WorkflowBLL flowBLL = new WorkflowBLL();
        private UserBLL userBLL = new UserBLL();
        private DepartmentBLL deptBLL = new DepartmentBLL();
        private CompanyBLL companyBLL = new CompanyBLL();
        private RoleBLL roleBLL = new RoleBLL();
        private PostBLL postBLL = new PostBLL();
        private GroupBLL groupBLL = new GroupBLL();
        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Page(JqgridPageRequest jqPage)
        {
            var page = flowBLL.Page(jqPage);
            return Json(page);
        }

        public IActionResult DeleteForm(string keyValue)
        {
            var entity = flowBLL.GetEntity(r => r.flow_id == keyValue);
            if (entity == null)
            {
                return Fail(MessageString.NoData);
            }
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            flowBLL.DeleteForm(entity, loginInfo);
            return Success();
        }
        public IActionResult CopyForm(string keyValue)
        {
            var entity = flowBLL.GetEntity(r => r.flow_id == keyValue);
            if (entity == null)
            {
                return Fail(MessageString.NoData);
            }
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            flowBLL.CopyForm(entity, loginInfo);
            return Success();
        }

        #endregion

        #region Form
        public IActionResult Form()
        {
            return View();
        }

        public IActionResult GetFormData(string keyValue)
        {
            var entity = flowBLL.GetEntity(r => r.flow_id == keyValue);
            var authorizes = flowBLL.GetAuthorizeList(r => r.flow_id == keyValue);
            var authorizesModels = authorizes.GroupBy(r => r.objecttype).Select(r =>
            {
                var model = new WFAuthorizeModel
                {
                    objecttype = r.Key,
                };
                var objectids = authorizes.Where(t => t.objecttype == r.Key).Select(r => r.object_id).ToList();
                model.objectids = string.Join(",", objectids);
                switch (model.objecttype)
                {
                    case 1: // 用户
                        var users = userBLL.GetAllByCache();
                        model.objectnames = string.Join(",", users.Where(t => objectids.Contains(t.user_id)).Select(t => t.realname));
                        break;
                    case 2: // 部门
                        var depts = deptBLL.GetAllByCache();
                        model.objectnames = string.Join(",", depts.Where(t => objectids.Contains(t.department_id)).Select(t => t.fullname));
                        break;
                    case 3: // 公司
                        var companys = companyBLL.GetAllByCache();
                        model.objectnames = string.Join(",", companys.Where(t => objectids.Contains(t.company_id)).Select(t => t.fullname));
                        break;
                    case 4: // 角色
                        var roles = roleBLL.GetAllByCache();
                        model.objectnames = string.Join(",", roles.Where(t => objectids.Contains(t.role_id)).Select(t => t.rolename));
                        break;
                    case 5: // 岗位
                        var posts = postBLL.GetAllByCache();
                        model.objectnames = string.Join(",", posts.Where(t => objectids.Contains(t.post_id)).Select(t => t.postname));
                        break;
                    case 6: // 小组
                        var groups = groupBLL.GetAllByCache();
                        model.objectnames = string.Join(",", groups.Where(t => objectids.Contains(t.group_id)).Select(t => t.groupname));
                        break;
                }
                return model;
            }
            ).ToList();
            var scheme = flowBLL.GetSchemeEntity(r => r.flow_id == keyValue);
            var data = new Dictionary<string, object>
            {
                { "Workflow",entity },
                { "Authorizes",authorizesModels },
                {"Scheme",scheme }
            };
            return Success(data);
        }

        [HttpPost, AjaxOnly]
        [AutoValidateAntiforgeryToken]
        public IActionResult SaveForm(string keyValue, string strEntity, string strAuthorizeList, string schemejson)
        {
            var entity = strEntity.ToObject<wf_flowEntity>();
            var listAuthModel = strAuthorizeList.ToObject<List<WFAuthorizeModel>>();
            if (!keyValue.IsEmpty())
            {
                var already = flowBLL.GetEntity(r => r.flowcode == entity.flowcode && r.flow_id != keyValue);
                if (already != null)
                {
                    return Fail("流程编码已存在，不能重复");
                }
            }

            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            flowBLL.SaveForm(keyValue, entity, strEntity, listAuthModel, schemejson, loginInfo);
            return Success(entity);
        }

        #endregion

        #region NodeForm
        public IActionResult NodeForm()
        {
            return View();
        }

        #endregion

        #region LineForm
        public IActionResult LineForm()
        {
            return View();
        }

        #endregion

    }
}
