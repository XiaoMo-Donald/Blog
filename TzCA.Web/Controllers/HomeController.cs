using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TzCA.DataAccess;
using TzCA.Entities.ApplicationOrganization;
using TzCA.Entities.Attachments;
using TzCA.Entities.BusinessOrganization;
using TzCA.Entities.SiteManagement;
using TzCA.SignalR;
using TzCA.ViewModels.ApplicationOrganization;
using TzCA.Web.Models;

namespace TzCA.Web.Controllers
{
    public class HomeController : TzControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEntityRepository<Person> _person;
        public HomeController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEntityRepository<BusinessImage> businessImage,
            IEntityRepository<TzSiteLog> tzSiteLog,
            IEntityRepository<Person> person
            )
            : base(userManager, businessImage, tzSiteLog)
        {
            this._signInManager = signInManager;
            this._person = person;
        }

        public async Task<IActionResult> Index()
        {
            //测试使用Cookies
            //SetCookies("testCookies","12345666666");

            return View(await GetUserVMByName(adminName));
        }

        public IActionResult Chat()
        {
            return View();
        }

        /// <summary>
        /// 全站用户页面（数据使用ajax从TzData控制器获取）
        /// </summary>
        /// <returns></returns>
        public IActionResult CloudAllianceUsers()
        {
            return View();
        }

        public JsonResult GetOnlineCount()
        {
            return Json(new { onlineCount = TzChatHub.ChatUserList.Count });
        }

        /// <summary>
        /// 网站服务条款（模板）
        /// </summary>
        /// <returns></returns>
        public IActionResult TermsAndConditions()
        {
            return PartialView();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
