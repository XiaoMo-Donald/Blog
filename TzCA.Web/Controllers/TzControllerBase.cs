using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TzCA.DataAccess;
using TzCA.Entities.ApplicationOrganization;
using TzCA.Entities.Attachments;
using TzCA.Entities.SiteManagement;
using TzCA.ViewModels.ApplicationOrganization;
using TzCA.ViewModels.ApplicationOrganization.Ohter;
using TzCA.ViewModels.TzPagination;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TzCA.Web.Controllers
{
    /// <summary>
    /// 云盟控制器Controller基类
    /// </summary>
    public class TzControllerBase : Controller
    {
        /// <summary>
        /// 用于存储响应时间开始的Key
        /// </summary>
        private const string responseTimeKey = "X-Response-Time-ms";

        /// <summary>
        /// 响应时间
        /// </summary>
        protected string responseTime = string.Empty;

        /// <summary>
        /// 超级管理员登录名称
        /// </summary>
        protected const string adminName = "admin";

        /// <summary>
        /// 获取超级管理员
        /// </summary>
        protected ApplicationUser AdminUser => GetUserByName(adminName).Result;

        /// <summary>
        /// 获取当前登录用户信息（非异步）
        /// </summary>
        protected ApplicationUser TzUser => GetUser().Result;

        /// <summary>
        /// 获取当前登录的用户的视图（非异步）
        /// </summary>
        protected ApplicationUserVM TzUserVM => GetUserVM().Result;

        /// <summary>
        /// 是否存在指定用户名的用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        protected bool _HasTheSameUser(string userName) => _userManager.Users.Any(x => x.UserName == userName);

        /// <summary>
        /// 是否存在指定的昵称
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        protected bool _HasTheSameNickname(string nickname) => _userManager.Users.Any(x => x.Nickname.Equals(nickname));

        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly IEntityRepository<BusinessImage> _businessImage;
        protected readonly IEntityRepository<TzSiteLog> _tzSiteLog;

        public TzControllerBase(
            UserManager<ApplicationUser> userManager,
            IEntityRepository<BusinessImage> businessImage,
            IEntityRepository<TzSiteLog> tzSiteLog
            )
        {
            this._userManager = userManager;
            this._businessImage = businessImage;
            this._tzSiteLog = tzSiteLog;
        }

        /// <summary>
        /// 获取当前用户的登录名称或者昵称
        /// </summary>
        /// <returns></returns>
        public string GetUserNameOrNickname()
        {
            try
            {
                var username = User.Identity.Name;
                if (!string.IsNullOrEmpty(username))
                {
                    var user = _userManager.FindByNameAsync(username).Result;
                    if (user != null)
                    {
                        var result = string.IsNullOrEmpty(user.Nickname) ? user.UserName : user.Nickname;
                        return result;
                    }
                    else return null;
                }
                else return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取头像
        /// </summary>
        /// <param name="userVM">用户视图模型</param>
        /// <returns></returns>
        protected async Task<ApplicationUserVM> GetUserVMContainAvatar(ApplicationUserVM userVM)
        {
            var images = await _businessImage.GetAllAsyn();
            var avatar = images.FirstOrDefault(x => x.RelevanceObjectId == userVM.Id && x.Type.Equals(BusinessImageEnum.Avatars));
            var defaultAvatar = "../images/chatAvatars/defaultAvatar.jpg";
            userVM.Avatar = string.IsNullOrEmpty(avatar?.RelativePath) ? defaultAvatar : avatar?.RelativePath;
            userVM.MinAvatar = string.IsNullOrEmpty(avatar?.MinRelativePath) ? defaultAvatar : avatar?.MinRelativePath;
            return userVM;
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        protected async Task<ApplicationUser> GetUser()
        {
            try
            {
                var username = User.Identity.Name;
                if (!string.IsNullOrEmpty(username))
                {
                    return await _userManager.FindByNameAsync(username);
                }
                else return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据用户Id获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected async Task<ApplicationUser> GetUserById(Guid userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        /// <summary>
        /// 根据用户登录名称获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected async Task<ApplicationUser> GetUserByName(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        /// <summary>
        /// 获取当前用户信息(视图模型)
        /// </summary>
        /// <returns></returns>
        protected async Task<ApplicationUserVM> GetUserVM()
        {
            var userVM = new ApplicationUserVM(TzUser);
            return await GetUserVMContainAvatar(userVM);
        }

        /// <summary>
        ///  根据用户Id获取用户信息（视图模型）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected async Task<ApplicationUserVM> GetUserVMById(Guid userId)
        {
            var userVM = new ApplicationUserVM(await GetUserById(userId));
            return await GetUserVMContainAvatar(userVM);
        }

        /// <summary>
        ///  根据用户登录名称获取用户信息（视图模型）
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        protected async Task<ApplicationUserVM> GetUserVMByName(string username)
        {
            var userVM = new ApplicationUserVM(await GetUserByName(username));
            return await GetUserVMContainAvatar(userVM);
        }

        /// <summary>
        ///  根据用户Id获取用户部分信息（Dto）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected async Task<ApplicationUserDto> GetUserDtoById(Guid userId)
        {
            var userVM = new ApplicationUserVM(await GetUserById(userId));
            return new ApplicationUserDto(await GetUserVMContainAvatar(userVM));
        }

        /// <summary>
        ///  根据用户获取用户部分信息（Dto）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected async Task<ApplicationUserDto> GetUserDtoByUser(ApplicationUser user)
        {
            var userVM = new ApplicationUserVM(user);
            return new ApplicationUserDto(await GetUserVMContainAvatar(userVM));
        }

        /// <summary>
        /// 获取全站所有的用户
        /// </summary>
        /// <returns></returns>
        protected async Task<PaginationOut<List<ApplicationUserDto>>> GetAllUsersDto(GetAllUsersInput input)
        {
            //虚拟链接数据
            var userProLinks = new List<UserLinksDto>
            {
                new UserLinksDto{Name="QQ",Target="_blank",Id=Guid.NewGuid(),Link="javascript:"},
                new UserLinksDto{Name="微博",Target="_blank",Id=Guid.NewGuid(),Link="javascript:"},
                new UserLinksDto{Name="个人网站",Target="_blank",Id=Guid.NewGuid(),Link="javascript:"},
                new UserLinksDto{Name="Github",Target="_blank",Id=Guid.NewGuid(),Link="javascript:"}
            };
            var queryUsers = _userManager.Users;
            var users = queryUsers.OrderBy(x => x.CreateTime).Skip(input.SkipCount).Take(input.Limit);
            var usersDto = new List<ApplicationUserDto>();
            foreach (var user in users)
            {
                var userDto = await GetUserDtoByUser(user);
                userDto.UserLinks = userProLinks;
                usersDto.Add(userDto);
            }
            usersDto.ForEach(delegate (ApplicationUserDto userDto)
            {
                if (userDto.UserName.Equals(adminName))
                    userDto.Authentication = "云盟创始人";
                else
                    userDto.Authentication = "云盟认证";
            });
            usersDto.ForEach(x => x.UserName = string.Empty);
            var statusCode = HttpContext.Response.StatusCode.Equals(200) ? 0 : 1;
            return new PaginationOut<List<ApplicationUserDto>>
            {
                Code = statusCode,
                Msg = statusCode.Equals(0) ? "" : "请求错误Ծ‸Ծ",
                Count = queryUsers.Count(),
                Data = usersDto
            };
        }

        //请求拦截器（使用bese控制器实现）


        //public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        //{
        //    return base.OnActionExecutionAsync(context, next);
        //}

        /// <summary>
        /// 开始请求
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items[responseTimeKey] = Stopwatch.StartNew();
            base.OnActionExecuting(context);
        }

        /// <summary>
        /// 结束请求
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //停止计时
            Stopwatch stopwatch = (Stopwatch)context.HttpContext.Items[responseTimeKey];
            responseTime = Math.Round((stopwatch.ElapsedMilliseconds / 1000.0), 2) + " 秒(s)";
            var connection = context.HttpContext.Connection;
            var request = context.HttpContext.Request;
            var user = TzUser;
            var siteLog = new TzSiteLog
            {
                RequestType = request.Method,
                Url = request.Scheme + "://" + request.Host + request.Path + request?.QueryString,
                IpAddress = connection?.RemoteIpAddress.ToString(),
                ResponseTime = responseTime,
                User = user,
                UserNickname = user == null ? "游客" : user.Nickname
            };
            _tzSiteLog.AddAndSave(siteLog);
            base.OnActionExecuted(context);
        }


        #region Cookies相关

        /// <summary>
        /// 设置本地cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>  
        /// <param name="minutes">过期时长，单位：分钟</param>      
        protected void SetCookies(string key, string value, int minutes = 30)
        {
            HttpContext.Response.Cookies.Append(key, value, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(minutes)
            });
        }

        /// <summary>
        /// 删除指定的cookie
        /// </summary>
        /// <param name="key">键</param>
        protected void DeleteCookies(string key)
        {
            HttpContext.Response.Cookies.Delete(key);
        }

        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回对应的值</returns>
        protected string GetCookies(string key)
        {
            HttpContext.Request.Cookies.TryGetValue(key, out string value);
            if (string.IsNullOrEmpty(value))
                value = string.Empty;
            return value;
        }
        #endregion
    }
}
