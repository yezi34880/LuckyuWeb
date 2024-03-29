﻿
using Luckyu.DataAccess;
using Luckyu.Utility;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Luckyu.App.Organization
{
    public class sys_userService : RepositoryFactory<sys_userEntity>
    {
        public JqgridPageResponse<sys_userEntity> Page(JqgridPageRequest jqpage, string organizationTag, string organizationId)
        {
            Expression<Func<sys_userEntity, bool>> expCondition = r => r.is_delete == 0;
            if (!organizationId.IsEmpty() && organizationId != "-1")
            {
                if (organizationTag.StartsWith("company"))
                {
                    expCondition = expCondition.LinqAnd(r => r.company_id == organizationId);
                }
                else
                {
                    expCondition = expCondition.LinqAnd(r => r.department_id == organizationId);
                }
            }
            var page = BaseRepository().GetPage(jqpage, expCondition);
            return page;
        }
        public void DeleteForm(sys_userEntity entity, UserModel loginInfo)
        {
            entity.Remove(loginInfo);
            BaseRepository().UpdateOnlyColumns(entity, r => new { r.is_delete, r.delete_userid, r.delete_username, r.deletetime });
        }
        
        /// <summary>
        /// 用户启用、禁用
        /// </summary>
        public void SetOnOff(sys_userEntity entity, UserModel loginInfo)
        {
            entity.is_enable = 1 - entity.is_enable;
            entity.edittime = DateTime.Now;
            entity.edit_userid = loginInfo.user_id;
            entity.edit_username = loginInfo.realname;
            BaseRepository().UpdateOnlyColumns(entity, r => new { r.is_enable, r.edit_userid, r.edit_username, r.edittime });
        }

        /// <summary>
        /// 更新最后登录时间、登录ip
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateLastLogin(sys_userEntity entity)
        {
            BaseRepository().UpdateOnlyColumns(entity, r => new { r.lastlogintime, r.lastloginip });
        }

        public void SaveForm(string keyValue, sys_userEntity entity, string strEntity, UserModel loginInfo)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                var db = trans.db;
                if (keyValue.IsEmpty())
                {
                    entity.Create(loginInfo);
                    trans.Insert(entity);

                    var defaultRoles = db.Queryable<sys_roleEntity>().Where(r => r.is_default == 1 && r.is_delete == 0 && r.is_enable == 1).ToList();
                    if (!defaultRoles.IsEmpty())
                    {
                        foreach (var role in defaultRoles)
                        {
                            var auth = new sys_userrelationEntity();
                            auth.Create(loginInfo);
                            auth.user_id = entity.user_id;
                            auth.relationtype = (int)UserRelationEnum.Role;
                            auth.object_id = role.role_id;
                            db.Insertable(auth).ExecuteCommand();
                        }
                    }

                    var defaultPosts = db.Queryable<sys_postEntity>().Where(r => r.is_default == 1 && r.is_delete == 0 && r.is_enable == 1).ToList();
                    if (!defaultPosts.IsEmpty())
                    {
                        foreach (var post in defaultPosts)
                        {
                            var auth = new sys_userrelationEntity();
                            auth.Create(loginInfo);
                            auth.user_id = entity.user_id;
                            auth.relationtype = (int)UserRelationEnum.Post;
                            auth.object_id = post.post_id;
                            db.Insertable(auth).ExecuteCommand();
                        }
                    }

                }
                else
                {
                    entity.Modify(keyValue, loginInfo);
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

        /// <summary>
        /// 修改密码
        /// </summary>
        public void ModifyPassword(sys_userEntity entity)
        {
            BaseRepository().UpdateOnlyColumns(entity, r => new { r.loginpassword });
        }
    }
}
