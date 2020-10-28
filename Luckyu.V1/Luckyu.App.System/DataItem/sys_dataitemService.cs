﻿using FreeSql.Internal.Model;
using Luckyu.App.Organization;
using Luckyu.DataAccess;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.System
{
    public class sys_dataitemService : RepositoryFactory<sys_dataitemEntity>
    {
        public JqgridPageResponse<sys_dataitemEntity> Page(JqgridPageRequest jqpage)
        {
            Expression<Func<sys_dataitemEntity, bool>> exp = r => r.is_delete == 0;
            var dicCondition = new Dictionary<string, Func<string, string, DynamicFilterInfo>>();
            dicCondition.Add("is_enable",
                (field, data) => SearchConditionHelper.GetStringEqualCondition(field, data, "-1")
                );
            var page = BaseRepository().GetPage(jqpage, exp, dicCondition);
            return page;
        }

        public void DeleteForm(sys_dataitemEntity entity, UserModel loginInfo)
        {
            entity.Remove(loginInfo);
            BaseRepository().UpdateOnlyColumns(entity, r => new { r.is_delete, r.delete_userid, r.delete_username, r.deletetime });
        }

        public void SaveForm(string keyValue, sys_dataitemEntity entity, string strEntity, UserModel loginInfo)
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
                    db.Update<sys_dataitem_detailEntity>().Where(r => r.itemcode == entity.itemcode).Set(r => r.itemcode == entity.itemcode).ExecuteAffrows();
                    trans.UpdateAppendColumns(entity, strEntity, r => new { r.edittime, r.edit_userid, r.edit_username });
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
