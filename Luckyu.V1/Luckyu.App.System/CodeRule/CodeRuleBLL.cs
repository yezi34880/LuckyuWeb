using Luckyu.App.Organization;
using Luckyu.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.System
{
    public class CodeRuleBLL
    {
        #region Var
        private sys_coderuleService codeService = new sys_coderuleService();
        private sys_coderule_seedService seedService = new sys_coderule_seedService();

        private CompanyBLL companyBLL = new CompanyBLL();
        private DepartmentBLL deptBLL = new DepartmentBLL();
        private UserBLL userBLL = new UserBLL();
        #endregion

        #region Get
        public JqgridPageResponse<sys_coderuleEntity> Page(JqgridPageRequest jqpage)
        {
            var page = codeService.Page(jqpage);
            return page;
        }
        public sys_coderuleEntity GetEntity(Expression<Func<sys_coderuleEntity, bool>> condition)
        {
            var entity = codeService.GetEntity(condition);
            return entity;
        }

        public ResponseResult<Dictionary<string, object>> GetFormData(string keyValue)
        {
            var entity = GetEntity(r => r.rule_id == keyValue);
            if (entity == null)
            {
                return ResponseResult.Fail<Dictionary<string, object>>(MessageString.NoData);
            }
            var dic = new Dictionary<string, object>
            {
                {"CodeRule",entity }
            };
            return ResponseResult.Success(dic);
        }

        #endregion

        #region Set
        public ResponseResult DeleteForm(string keyValue, UserModel loginInfo)
        {
            var entity = GetEntity(r => r.rule_id == keyValue);
            if (entity == null)
            {
                return ResponseResult.Fail(MessageString.NoData);
            }
            codeService.DeleteForm(entity, loginInfo);
            return ResponseResult.Success();
        }
        public ResponseResult<sys_coderuleEntity> SaveForm(string keyValue, string strEntity, UserModel loginInfo)
        {
            var entity = strEntity.ToObject<sys_coderuleEntity>();
            if (!keyValue.IsEmpty())
            {
                var old = GetEntity(r => r.rule_id == keyValue);
                if (old == null)
                {
                    return ResponseResult.Fail<sys_coderuleEntity>(MessageString.NoData);
                }
            }

            codeService.SaveForm(keyValue, entity, strEntity, loginInfo);
            return ResponseResult.Success(entity);
        }
        #endregion

        public string NewCode(string code, UserModel loginInfo)
        {
            var entity = GetEntity(r => r.rulecode == code && r.is_delete == 0 && r.is_enable == 1);
            if (entity == null)
            {
                return "";
            }
            var listRuleItem = JsonConvert.DeserializeObject<List<RuleItemModel>>(entity.contentjson);
            if (listRuleItem.IsEmpty())
            {
                return "";
            }
            var strCode = new StringBuilder();
            var strPreNumber = new StringBuilder();
            if (listRuleItem.Exists(r => r.Type == "var"))
            {
                throw new Exception("配置规则需要传入字符串,请检查配置或使用重载方法");
            }
            for (int i = 0; i < listRuleItem.Count; i++)
            {
                var item = listRuleItem[i];
                switch (item.Type)
                {
                    case "custom":
                        strCode.Append(item.Format);
                        if (item.BeNumber == 1)
                        {
                            strPreNumber.Append(item.Format);
                        }
                        break;
                    case "company":
                        var company = companyBLL.GetEntityByCache(r => r.company_id == loginInfo.company_id);
                        if (company != null)
                        {
                            var companyCode = company.companycode ?? "";
                            var str = companyCode;
                            if (!item.Format.IsEmpty())
                            {
                                var formats = item.Format.SplitNoEmpty('|');
                                if (formats.Length > 1)
                                {
                                    var f0 = formats[0]; // 从左往右还是从右往左
                                    var f1 = formats[1].ToInt();  // 截几位
                                    if (f1 > 0 && f1 <= companyCode.Length)
                                    {
                                        if (f0 == "L")
                                        {
                                            str = companyCode.Substring(0, f1);
                                        }
                                        else
                                        {
                                            str = companyCode.Substring(companyCode.Length - f1, f1);
                                        }
                                    }
                                }
                            }
                            strCode.Append(str);
                            if (item.BeNumber == 1)
                            {
                                strPreNumber.Append(str);
                            }
                        }
                        break;
                    case "dept":
                        var dept = deptBLL.GetEntityByCache(r => r.department_id == loginInfo.department_id);
                        if (dept != null)
                        {
                            var deptCode = dept.departmentcode ?? "";
                            var str = deptCode;
                            if (!item.Format.IsEmpty())
                            {
                                var formats = item.Format.SplitNoEmpty('|');
                                if (formats.Length > 1)
                                {
                                    var f0 = formats[0]; // 从左往右还是从右往左
                                    var f1 = formats[1].ToInt();  // 截几位
                                    if (f1 > 0 && f1 <= deptCode.Length)
                                    {
                                        if (f0 == "L")
                                        {
                                            str = deptCode.Substring(0, f1);
                                        }
                                        else
                                        {
                                            str = deptCode.Substring(deptCode.Length - f1, f1);
                                        }
                                    }
                                }
                            }
                            strCode.Append(str);
                            if (item.BeNumber == 1)
                            {
                                strPreNumber.Append(str);
                            }
                        }
                        break;
                    case "user":
                        var user = userBLL.GetEntityByCache(r => r.user_id == loginInfo.user_id);
                        if (user != null)
                        {
                            var userCode = user.usercode ?? "";
                            var str = userCode;
                            if (!item.Format.IsEmpty())
                            {
                                var formats = item.Format.SplitNoEmpty('|');
                                if (formats.Length > 1)
                                {
                                    var f0 = formats[0]; // 从左往右还是从右往左
                                    var f1 = formats[1].ToInt();  // 截几位
                                    if (f1 > 0 && f1 <= userCode.Length)
                                    {
                                        if (f0 == "L")
                                        {
                                            str = userCode.Substring(0, f1);
                                        }
                                        else
                                        {
                                            str = userCode.Substring(userCode.Length - f1, f1);
                                        }
                                    }
                                }
                            }
                            strCode.Append(str);
                            if (item.BeNumber == 1)
                            {
                                strPreNumber.Append(str);
                            }
                        }
                        break;
                    case "date":
                        var strDate = DateTime.Now.ToString(item.Format);
                        strCode.Append(strDate);
                        if (item.BeNumber == 1)
                        {
                            strPreNumber.Append(strDate);
                        }
                        break;
                    case "number":
                        var preNumber = strPreNumber.ToString();
                        var seed = seedService.GetEntity(r => r.rule_id == entity.rule_id && r.seedprename == preNumber);
                        var number = 0;
                        if (seed != null)
                        {
                            number = seed.seedvalue;
                        }
                        else
                        {
                            seed = new sys_coderule_seedEntity(entity.rule_id, preNumber);
                        }
                        number++;
                        seed.seedvalue = number;
                        seedService.SaveForm(seed.seed_id, seed);
                        strCode.Append(number.ToString(item.Format));
                        break;
                }
            }
            return strCode.ToString();
        }

        public string NewCode(string code, List<string> varStr, UserModel loginInfo)
        {
            var entity = GetEntity(r => r.rulecode == code && r.is_delete == 0 && r.is_enable == 1);
            if (entity == null)
            {
                return "";
            }
            var listRuleItem = JsonConvert.DeserializeObject<List<RuleItemModel>>(entity.contentjson);
            if (listRuleItem.IsEmpty())
            {
                return "";
            }
            var varItemsCount = listRuleItem.Where(r => r.Type == "var").Count();
            if (varStr.IsEmpty() || varStr.Count != varItemsCount)
            {
                throw new Exception("传入字符与配置项数量不匹配");
            }
            var strCode = new StringBuilder();
            var strPreNumber = new StringBuilder();
            var varIndex = 0;
            for (int i = 0; i < listRuleItem.Count; i++)
            {
                var item = listRuleItem[i];
                switch (item.Type)
                {
                    case "var":
                        strCode.Append(varStr[varIndex++]);
                        if (item.BeNumber == 1)
                        {
                            strPreNumber.Append(item.Format);
                        }
                        break;
                    case "custom":
                        strCode.Append(item.Format);
                        if (item.BeNumber == 1)
                        {
                            strPreNumber.Append(item.Format);
                        }
                        break;
                    case "company":
                        var company = companyBLL.GetEntityByCache(r => r.company_id == loginInfo.company_id);
                        if (company != null)
                        {
                            var companyCode = company.companycode ?? "";
                            var str = companyCode;
                            if (!item.Format.IsEmpty())
                            {
                                var formats = item.Format.SplitNoEmpty('|');
                                if (formats.Length > 1)
                                {
                                    var f0 = formats[0]; // 从左往右还是从右往左
                                    var f1 = formats[1].ToInt();  // 截几位
                                    if (f1 > 0 && f1 <= companyCode.Length)
                                    {
                                        if (f0 == "L")
                                        {
                                            str = companyCode.Substring(0, f1);
                                        }
                                        else
                                        {
                                            str = companyCode.Substring(companyCode.Length - f1, f1);
                                        }
                                    }
                                }
                            }
                            strCode.Append(str);
                            if (item.BeNumber == 1)
                            {
                                strPreNumber.Append(str);
                            }
                        }
                        break;
                    case "dept":
                        var dept = deptBLL.GetEntityByCache(r => r.department_id == loginInfo.department_id);
                        if (dept != null)
                        {
                            var deptCode = dept.departmentcode ?? "";
                            var str = deptCode;
                            if (!item.Format.IsEmpty())
                            {
                                var formats = item.Format.SplitNoEmpty('|');
                                if (formats.Length > 1)
                                {
                                    var f0 = formats[0]; // 从左往右还是从右往左
                                    var f1 = formats[1].ToInt();  // 截几位
                                    if (f1 > 0 && f1 <= deptCode.Length)
                                    {
                                        if (f0 == "L")
                                        {
                                            str = deptCode.Substring(0, f1);
                                        }
                                        else
                                        {
                                            str = deptCode.Substring(deptCode.Length - f1, f1);
                                        }
                                    }
                                }
                            }
                            strCode.Append(str);
                            if (item.BeNumber == 1)
                            {
                                strPreNumber.Append(str);
                            }
                        }
                        break;
                    case "user":
                        var user = userBLL.GetEntityByCache(r => r.user_id == loginInfo.user_id);
                        if (user != null)
                        {
                            var userCode = user.usercode ?? "";
                            var str = userCode;
                            if (!item.Format.IsEmpty())
                            {
                                var formats = item.Format.SplitNoEmpty('|');
                                if (formats.Length > 1)
                                {
                                    var f0 = formats[0]; // 从左往右还是从右往左
                                    var f1 = formats[1].ToInt();  // 截几位
                                    if (f1 > 0 && f1 <= userCode.Length)
                                    {
                                        if (f0 == "L")
                                        {
                                            str = userCode.Substring(0, f1);
                                        }
                                        else
                                        {
                                            str = userCode.Substring(userCode.Length - f1, f1);
                                        }
                                    }
                                }
                            }
                            strCode.Append(str);
                            if (item.BeNumber == 1)
                            {
                                strPreNumber.Append(str);
                            }
                        }
                        break;
                    case "date":
                        var strDate = DateTime.Now.ToString(item.Format);
                        strCode.Append(strDate);
                        if (item.BeNumber == 1)
                        {
                            strPreNumber.Append(strDate);
                        }
                        break;
                    case "number":
                        var preNumber = strPreNumber.ToString();
                        var seed = seedService.GetEntity(r => r.rule_id == entity.rule_id && r.seedprename == preNumber);
                        var number = 0;
                        if (seed != null)
                        {
                            number = seed.seedvalue;
                        }
                        else
                        {
                            seed = new sys_coderule_seedEntity(entity.rule_id, preNumber);
                        }
                        number++;
                        seed.seedvalue = number;
                        seedService.SaveForm(seed.seed_id, seed);
                        strCode.Append(number.ToString(item.Format));
                        break;
                }
            }
            return strCode.ToString();
        }

    }
}
