using FreeSql.Internal.Model;
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
    public class sys_nmessageService : RepositoryFactory<sys_messageEntity>
    {
        public JqgridPageResponse<sys_messageEntity> Page(JqgridPageRequest jqPage)
        {
            Expression<Func<sys_messageEntity, bool>> exp = r => r.is_delete == 0;

            var page = BaseRepository().GetPage(jqPage, exp);
            return page;
        }

        public JqgridPageResponse<sys_messageEntity> ShowPage(JqgridPageRequest jqPage, UserModel loginInfo)
        {
            Expression<Func<sys_messageEntity, bool>> exp = r => r.is_delete == 0 && r.to_userid == loginInfo.user_id;

            var page = BaseRepository().GetPage(jqPage, exp);
            return page;
        }

        public void Insert(List<sys_messageEntity> list, UserModel loginInfo)
        {
            foreach (var item in list)
            {
                item.Create(loginInfo);
            }
            BaseRepository().Insert(list);
        }
    }
}
