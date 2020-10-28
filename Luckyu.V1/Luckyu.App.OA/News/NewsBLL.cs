using Luckyu.App.Organization;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.OA
{
    public class NewsBLL
    {
        #region Var
        private oa_newsService newsService = new oa_newsService();

        #endregion

        #region Get
        public JqgridPageResponse<oa_newsEntity> Page(JqgridPageRequest jqpage, UserModel loginInfo)
        {
            var page = newsService.Page(jqpage, loginInfo);
            return page;
        }
        public JqgridPageResponse<oa_newsEntity> ShowPage(JqgridPageRequest jqpage, UserModel loginInfo)
        {
            var page = newsService.ShowPage(jqpage, loginInfo);
            return page;
        }

        public oa_newsEntity GetEntity(Expression<Func<oa_newsEntity, bool>> condition)
        {
            var entity = newsService.GetEntity(condition);
            return entity;
        }
        public ResponseResult<Dictionary<string, object>> GetFormData(string keyValue)
        {
            var entity = GetEntity(r => r.id == keyValue);
            if (entity == null)
            {
                return ResponseResult.Fail<Dictionary<string, object>>("该数据不存在");
            }
            var dic = new Dictionary<string, object>
            {
                {"News",entity }
            };
            return ResponseResult.Success(dic);
        }

        #endregion

        #region Set
        /// <summary>
        /// 发布/取消发布
        /// </summary>
        public ResponseResult Publish(string keyValue, UserModel loginInfo)
        {
            var entity = GetEntity(r => r.id == keyValue);
            if (entity == null)
            {
                return ResponseResult.Fail("该数据不存在");
            }
            //if (entity.state != (int)StateEnum.Draft)
            //{
            //    return ResponseResult.Fail("只有起草状态才能删除");
            //}
            newsService.Publish(entity, loginInfo);
            return ResponseResult.Success();
        }
        /// <summary>
        /// 置顶/取消置顶
        /// </summary>
        public ResponseResult SetTop(string keyValue, UserModel loginInfo)
        {
            var entity = GetEntity(r => r.id == keyValue);
            if (entity == null)
            {
                return ResponseResult.Fail("该数据不存在");
            }
            //if (entity.state != (int)StateEnum.Draft)
            //{
            //    return ResponseResult.Fail("只有起草状态才能删除");
            //}
            newsService.SetTop(entity, loginInfo);
            return ResponseResult.Success();
        }

        public ResponseResult DeleteForm(string keyValue, UserModel loginInfo)
        {
            var entity = GetEntity(r => r.id == keyValue);
            if (entity == null)
            {
                return ResponseResult.Fail("该数据不存在");
            }
            //if (entity.state != (int)StateEnum.Draft)
            //{
            //    return ResponseResult.Fail("只有起草状态才能删除");
            //}
            newsService.DeleteForm(entity, loginInfo);
            return ResponseResult.Success();
        }

        public ResponseResult<oa_newsEntity> SaveForm(string keyValue, string strEntity, UserModel loginInfo)
        {
            var entity = strEntity.ToObject<oa_newsEntity>();
            if (!keyValue.IsEmpty())
            {
                var old = GetEntity(r => r.id == keyValue);
                if (old == null)
                {
                    return ResponseResult.Fail<oa_newsEntity>("该数据不存在");
                }
                //if (old.state != (int)StateEnum.Draft && old.state != (int)StateEnum.Reject)
                //{
                //    return ResponseResult.Fail<oa_newsEntity>("只有起草状态才能编辑");
                //}
            }

            newsService.SaveForm(keyValue, entity, strEntity, loginInfo);
            return ResponseResult.Success(entity);
        }

        #endregion

    }
}
