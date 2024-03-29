﻿
using Luckyu.App.Organization;
using Luckyu.DataAccess;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Luckyu.App.System
{
    public class sys_dataitem_detailService : RepositoryFactory<sys_dataitem_detailEntity>
    {
        public JqgridPageResponse<sys_dataitem_detailEntity> Page(JqgridPageRequest jqpage, string classifyId, bool isSystem)
        {
            Expression<Func<sys_dataitem_detailEntity, bool>> exp = r => r.is_delete == 0;
            if (!isSystem)
            {
                exp = exp.LinqAnd(r => r.is_system == 0);
            }
            exp = exp.LinqAnd(r => r.dataitem_id == classifyId);
            if (jqpage.sidx.IsEmpty())
            {
                jqpage.sidx = "sort";
                jqpage.sord = "ASC";
            }
            var page = BaseRepository().GetPage(jqpage, exp);
            return page;
        }
        public JqgridPageResponse<sys_dataitem_detailEntity> SelectPage(JqgridPageRequest jqpage, string classifyId)
        {
            Expression<Func<sys_dataitem_detailEntity, bool>> exp = r => r.is_delete == 0 && r.is_enable == 1;
            exp = exp.LinqAnd(r => r.dataitem_id == classifyId);
            if (jqpage.sidx.IsEmpty())
            {
                jqpage.sidx = "sort";
                jqpage.sord = "ASC";
            }
            var page = BaseRepository().GetPage(jqpage, exp);
            return page;
        }

        public void DeleteForm(sys_dataitem_detailEntity entity, UserModel loginInfo)
        {
            entity.Remove(loginInfo);
            BaseRepository().UpdateOnlyColumns(entity, r => new { r.is_delete, r.delete_userid, r.delete_username, r.deletetime });
        }

        public void SaveForm(string keyValue, sys_dataitem_detailEntity entity, string strEntity, UserModel loginInfo)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                var db = trans.db;
                if (keyValue.IsEmpty())
                {
                    entity.Create(loginInfo);
                    trans.Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue, loginInfo);
                    trans.Update(entity, strEntity, r => new { r.edittime, r.edit_userid, r.edit_username }, r => new { r.dataitem_id, r.itemcode });
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }

    }

}
