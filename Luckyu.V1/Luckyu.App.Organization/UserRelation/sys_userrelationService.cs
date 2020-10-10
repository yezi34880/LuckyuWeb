using Luckyu.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.Organization
{
    public class sys_userrelationService : RepositoryFactory<sys_userrelationEntity>
    {
        public void SetRelationByObject(int relationType, string objectId, List<string> userIds, UserModel loginInfo)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                trans.Delete<sys_userrelationEntity>(r => r.relationtype == relationType && r.object_id == objectId);
                var list = new List<sys_userrelationEntity>();
                foreach (var userId in userIds)
                {
                    var relation = new sys_userrelationEntity();
                    relation.Create(loginInfo);
                    relation.object_id = objectId;
                    relation.user_id = userId;
                    relation.relationtype = relationType;
                    list.Add(relation);
                }
                trans.Insert(list);
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }
        public void SetRelationByUser(int relationType, string userId, List<string> objectIds, UserModel loginInfo)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                trans.Delete<sys_userrelationEntity>(r => r.relationtype == relationType && r.user_id == userId);
                var list = new List<sys_userrelationEntity>();
                foreach (var objectId in objectIds)
                {
                    var relation = new sys_userrelationEntity();
                    relation.Create(loginInfo);
                    relation.object_id = objectId;
                    relation.user_id = userId;
                    relation.relationtype = relationType;
                    list.Add(relation);
                }
                trans.Insert(list);
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
