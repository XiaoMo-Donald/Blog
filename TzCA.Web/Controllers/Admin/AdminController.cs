using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TzCA.Common.JsonModels;
using TzCA.DataAccess;
using TzCA.Entities.ApplicationOrganization;
using TzCA.Entities.Attachments;
using TzCA.Entities.SiteManagement;

namespace TzCA.Web.Controllers.Admin
{
    /// <summary>
    /// 后台管理员控制器
    /// </summary>
    public class AdminController : TzControllerBase
    {
        const string adminPartialViewRootPath = "../../Views/Admin/PartialViews/";
        private IHostingEnvironment _hostingEnv;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AdminController(
              IHostingEnvironment hostingEnv,
              RoleManager<ApplicationRole> roleManager,
              SignInManager<ApplicationUser> signInManager,
              UserManager<ApplicationUser> userManager,
              IEntityRepository<BusinessImage> businessImage,
              IEntityRepository<TzSiteLog> tzSiteLog
            ) : base(userManager, businessImage,tzSiteLog)
        {
            this._hostingEnv = hostingEnv;
            this._roleManager = roleManager;
            this._signInManager = signInManager;

        }

        /// <summary>
        /// 管理员登录界面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (this.TzUser != null)
            {
                return RedirectToAction("Index");
            }
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="loginInformation"></param>
        /// <returns></returns>
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginInformation loginInformation)
        {
            if (this.TzUser != null)
            {
                return RedirectToAction("Index");
            }
            var loginVM = loginInformation;
            if (_HasTheSameUser(loginVM.Username))
            {
                //TODO:验证用户组
                if (!await _userManager.IsInRoleAsync(await GetUserByName(loginVM.Username), "Admin"))
                {
                    return Json(new { state = false, message = "您没有足够的权限！" });
                    //return Json(new { state = false, message = "登录失败！" });
                }

                //!执行登录
                var result = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // 下面的登录成功后的导航应该具体依赖用户所在的角色组来进行处理的。   
                    var tempReturnUrl = loginVM.ReturnUrl?.ToString();
                    var returnUrl = string.IsNullOrEmpty(tempReturnUrl) ? Url.Action("Index", "Admin") : tempReturnUrl;
                    return Json(new { state = true, message = "登录成功，正在跳转...", reUrl = returnUrl });
                }
                else
                {
                    return Json(new { state = false, message = "用户名或密码错误！" });
                }
            }
            else
            {
                return Json(new { state = false, message = "用户名不存在！" });
            }
        }

        /// <summary>
        /// 后台首页（布局页面）
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 后台首页
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Authorize(Roles = "Admin")]
        public IActionResult Main()
        {
            return PartialView();
        }

        /// <summary>
        /// 博客文章管理
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Authorize(Roles = "Admin")]
        public IActionResult TzBlogArticle()
        {
            return PartialView();
        }
              

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Authorize(Roles = "Admin")]
        public IActionResult AddBlogArticle()
        {
            return PartialView(adminPartialViewRootPath + "_AddBlogArticle");
        }


        /// <summary>
        /// 系统用户管理
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Authorize(Roles = "Admin")]
        public IActionResult TzUsers()
        {
            return PartialView();
        }

        /// <summary>
        /// 用户等级
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Authorize(Roles = "Admin")]
        public IActionResult TzUserGrade()
        {
            return PartialView();
        }

        /// <summary>
        /// 系统图片管理
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Authorize(Roles = "Admin")]
        public IActionResult TzImages()
        {
            return PartialView();
        }

    }
}
