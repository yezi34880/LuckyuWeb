using Luckyu.App.Organization;
using Luckyu.Cache;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.System
{
    public class DataitemBLL
    {
        #region Var
        private sys_dataitemService dataitemService = new sys_dataitemService();
        private sys_dataitem_detailService dataitemdetailService = new sys_dataitem_detailService();

        #endregion

        #region Cache
        private string cacheKeyClassify = CacheFactory.GetCurrentDomain() + "luckyu_dataitem_";
        private string cacheKeyDetail = CacheFactory.GetCurrentDomain() + "luckyu_dataitem_detail_";
        private ICache cache = CacheFactory.Create();

        #endregion

        #region Classify

        #region Get
        public JqgridPageResponse<sys_dataitemEntity> ClassifyPage(JqgridPageRequest jqpage)
        {
            var page = dataitemService.Page(jqpage);
            return page;
        }

        public sys_dataitemEntity GetClassifyEntity(Expression<Func<sys_dataitemEntity, bool>> condition)
        {
            var entity = dataitemService.GetEntity(condition);
            return entity;
        }

        public List<sys_dataitemEntity> GetClassifyList(Expression<Func<sys_dataitemEntity, bool>> condition, Expression<Func<sys_dataitemEntity, object>> orderExp = null, bool isDesc = false)
        {
            var list = dataitemService.GetList(condition, orderExp, isDesc);
            return list;
        }

        public List<sys_dataitemEntity> GetAllClassifyByCache()
        {
            var list = cache.Read<List<sys_dataitemEntity>>(cacheKeyClassify);
            if (list.IsEmpty())
            {
                list = GetClassifyList(r => r.is_delete == 0, r => r.sort);
                cache.Write(cacheKeyClassify, list);
            }
            return list;
        }
        public sys_dataitemEntity GetClassifyEntityByCache(Func<sys_dataitemEntity, bool> condition)
        {
            var list = GetAllClassifyByCache();
            var entity = list.Where(condition).FirstOrDefault();
            return entity;
        }

        public List<xmSelectTree> GetSelectTree()
        {
            var all = GetAllClassifyByCache();
            var tree = ToSelectTree("0", all);
            return tree;
        }

        private List<xmSelectTree> ToSelectTree(string parentId, List<sys_dataitemEntity> list)
        {
            var nodes = list.Where(r => r.parent_id == parentId).ToList();
            if (nodes.IsEmpty())
            {
                return null;
            }
            var tree = nodes.Select(r => new xmSelectTree
            {
                value = r.dataitem_id,
                name = r.itemname,
                children = ToSelectTree(r.dataitem_id, list),
            }).ToList();
            return tree;
        }
        public List<eleTree> GetTree()
        {
            var all = GetAllClassifyByCache();
            var tree = ToTree("0", all);
            return tree;
        }
        private List<eleTree> ToTree(string parentId, List<sys_dataitemEntity> list)
        {
            var nodes = list.Where(r => r.parent_id == parentId).ToList();
            if (nodes.IsEmpty())
            {
                return null;
            }
            var tree = nodes.Select(r => new eleTree
            {
                id = r.dataitem_id,
                label = r.itemname,
                ext = new Dictionary<string, string> { { "code", r.itemcode } },
                children = ToTree(r.dataitem_id, list),
            }).ToList();
            return tree;
        }

        #endregion

        #region Set
        public void SaveClassifyForm(string keyValue, sys_dataitemEntity entity, string strEntity, UserModel loginInfo)
        {
            dataitemService.SaveForm(keyValue, entity, strEntity, loginInfo);
            cache.Remove(cacheKeyClassify);
        }
        public void DeleteClassifyForm(sys_dataitemEntity entity, UserModel loginInfo)
        {
            dataitemService.DeleteForm(entity, loginInfo);
            cache.Remove(cacheKeyClassify);
        }
        #endregion

        #endregion

        #region Detail

        #region Get
        public JqgridPageResponse<sys_dataitem_detailEntity> DetailPage(JqgridPageRequest jqpage, string classifyId, bool isALL)
        {
            var page = dataitemdetailService.Page(jqpage, classifyId, isALL);
            return page;
        }

        public sys_dataitem_detailEntity GetDetailEntity(Expression<Func<sys_dataitem_detailEntity, bool>> condition )
        {
            var entity = dataitemdetailService.GetEntity(condition);
            return entity;
        }

        public List<sys_dataitem_detailEntity> GetDetailList(Expression<Func<sys_dataitem_detailEntity, bool>> condition, Expression<Func<sys_dataitem_detailEntity, object>> orderExp = null, bool isDesc = false)
        {
            var list = dataitemdetailService.GetList(condition, orderExp, isDesc);
            return list;
        }

        public List<sys_dataitem_detailEntity> GetDetailByCache(string itemCode = "")
        {
            var list = cache.Read<List<sys_dataitem_detailEntity>>(cacheKeyDetail + itemCode);
            if (list.IsEmpty())
            {
                if (!itemCode.IsEmpty())
                {
                    list = GetDetailList(r => r.itemcode == itemCode && r.is_delete == 0, r => r.sort);
                }
                else
                {
                    list = GetDetailList(r => r.is_delete == 0, r => r.sort);
                }
                cache.Write(cacheKeyDetail + itemCode, list);
            }
            return list;
        }

        public Dictionary<string, Dictionary<string, ClientDataMapModel>> GetModelMap()
        {
            var dic = cache.Read<Dictionary<string, Dictionary<string, ClientDataMapModel>>>(cacheKeyDetail + "dic");
            if (dic == null)
            {
                dic = new Dictionary<string, Dictionary<string, ClientDataMapModel>>();
                var list = GetClassifyList(r => r.is_delete == 0 && r.is_enable == 1);
                foreach (var item in list)
                {
                    var detailList = GetDetailByCache(item.itemcode);
                    if (!dic.ContainsKey(item.itemcode))
                    {
                        dic.Add(item.itemcode, new Dictionary<string, ClientDataMapModel>());
                    }
                    foreach (var detailItem in detailList)
                    {
                        if (dic[item.itemcode].ContainsKey(detailItem.detail_id))
                        {
                            continue;
                        }
                        dic[item.itemcode].Add(detailItem.detail_id, new ClientDataMapModel()
                        {
                            code = detailItem.itemcode,
                            name = detailItem.showname,
                            value = detailItem.itemvalue
                        });
                    }
                }
                cache.Write(cacheKeyDetail + "dic", dic);
            }
            return dic;
        }
        #endregion

        #region Set
        public void SaveDetailForm(string keyValue, sys_dataitem_detailEntity entity, string strEntity, UserModel loginInfo)
        {
            dataitemdetailService.SaveForm(keyValue, entity, strEntity, loginInfo);
            cache.Remove(cacheKeyDetail);
            cache.Remove(cacheKeyDetail + entity.itemcode);
        }
        public void DeleteDetailForm(sys_dataitem_detailEntity entity, UserModel loginInfo)
        {
            dataitemdetailService.DeleteForm(entity, loginInfo);
            cache.Remove(cacheKeyDetail);
            cache.Remove(cacheKeyDetail + entity.itemcode);
        }

        #endregion

        #endregion



    }
}
