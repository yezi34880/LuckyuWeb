using Luckyu.App.Organization;
using Luckyu.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Module.OrganizationModule.Controllers
{
    /// <summary>
    /// 岗位  /OrganizationModule/Post
    /// </summary>
    [Area("OrganizationModule")]
    public class PostController : MvcControllerBase
    {
        #region Var
        private PostBLL postBLL = new PostBLL();
        private UserBLL userBLL = new UserBLL();
        private UserRelationBLL userrelationBLL = new UserRelationBLL();
        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Page(JqgridPageRequest jqpage)
        {
            var page = postBLL.Page(jqpage);
            return Json(page);
        }

        [HttpPost, AjaxOnly]
        public IActionResult DeleteForm(string keyValue)
        {
            var entity = postBLL.GetEntity(r => r.post_id == keyValue);
            if (entity == null)
            {
                return Fail("该数据不存在");
            }
            //var user = userBLL.GetEntity(r => r.post_id == entity.post_id && r.is_delete == 0 && r.is_enable == 1);
            //if (!user.IsEmpty())
            //{
            //    return Fail("该部门下还有人员，不能删除");
            //}

            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            postBLL.DeleteForm(entity, loginInfo);
            return Success();
        }
        #endregion

        #region Form
        [HttpGet]
        public IActionResult Form()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetFormData(string keyValue)
        {
            var entity = postBLL.GetEntity(r => r.post_id == keyValue);
            var data = new
            {
                Post = entity
            };
            return Success(data);
        }

        [HttpPost, AjaxOnly]
        [ValidateAntiForgeryToken]
        public IActionResult SaveForm(string keyValue, string strEntity)
        {
            var entity = strEntity.ToObject<sys_postEntity>();
            var already = postBLL.GetList(r => r.postcode == entity.postcode && r.post_id != keyValue);
            if (already == null)
            {
                return Fail("岗位编码已存在，请重新输入!");
            }

            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            postBLL.SaveForm(keyValue, entity, strEntity, loginInfo);
            return Success(entity);
        }

        #endregion

        #region Select
        [HttpGet]
        public IActionResult PostSelectForm()
        {
            return View();
        }

        [HttpGet, AjaxOnly]
        public IActionResult GetPostList(JqgridPageRequest jqPage)
        {
            var list = postBLL.GetSelect(jqPage);
            return Json(list);
        }
        #endregion

        #region  Interface
        /// <summary>
        /// 获取映射数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, AjaxOnly]
        public IActionResult GetMap(string ver)
        {
            var data = postBLL.GetModelMap();
            string md5 = EncrypHelper.MD5_Encryp(data.ToJson());
            if (md5 == ver)
            {
                return Success("no update");
            }
            else
            {
                var jsondata = new
                {
                    data = data,
                    ver = md5
                };
                return Success(jsondata);
            }
        }

        #endregion

    }
}
