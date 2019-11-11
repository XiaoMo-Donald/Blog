using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TzCA.Web.Controllers.Error
{
    /// <summary>
    /// 一个错误页面控制器
    /// </summary>
    public class ErrorController : Controller
    {
      /// <summary>
      /// 404错误
      /// </summary>
      /// <param name="statusCode">错误状态码</param>
      /// <returns></returns>
        [Route("Error/Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            return PartialView("../../Views/Error/404");
        }

        /// <summary>
        /// 警告页面
        /// </summary>
        /// <returns></returns>
        public IActionResult AWarmWarning()
        {
            return PartialView();
        }

        /// <summary>
        /// 权限不足，页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Denied() {

            return PartialView();
        }
    }
}
