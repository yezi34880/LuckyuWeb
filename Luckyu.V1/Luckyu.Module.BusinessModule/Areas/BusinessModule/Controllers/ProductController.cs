using Luckyu.App.Business;
using Luckyu.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Module.BusinessModule.Controllers
{
    /// <summary>
    /// 约车  /BusinessModule/Product
    /// </summary>
    [Area("BusinessModule")]
    public class ProductController : MvcControllerBase
    {
        #region Var
        private ProductBLL productBLL = new ProductBLL();

        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }

        #endregion

        #region Form
        public IActionResult Form()
        {
            return View();
        }
        #endregion

    }
}
