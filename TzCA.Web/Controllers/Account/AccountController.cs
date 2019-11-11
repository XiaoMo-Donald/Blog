using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TzCA.Entities.ApplicationOrganization;
using TzCA.DataAccess;
using Microsoft.AspNetCore.Identity;
using TzCA.Common.JsonModels;
using TzCA.ViewModels.ApplicationOrganization;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using TzCA.Entities.Attachments;
using TzCA.Entities.BusinessOrganization;
using TzCA.Common.TzRandomData;
using TzCA.Common.TzDataValid;
using TzCA.Entities.Blogs;
using TzCA.Common.PictureCompression;
using TzCA.Entities.SiteManagement;
using TzCA.Common.TzExtensions;
using TzCA.ViewModels.ApplicationOrganization.Ohter;
using TzCA.Entities.ApplicationOrganization.Other;

namespace TzCA.Web.Controllers.Account
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    public class AccountController : TzControllerBase
    {
        /// <summary>
        /// 用户中心视图根目录
        /// </summary>
        const string userPartialViewRootPath = "../../Views/Account/UserCenterPartialViews/";
        private IHostingEnvironment _hostingEnv;
        private readonly ITzClientHelper _tzClientHelper;
        private readonly ITzPictureCompressionHelper _tzPictureCompressionHelper;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEntityRepository<Person> _peson;
        private readonly IRandomDataHepler _randomDataHepler;
        private readonly ITzDataValidHelper _tzDataValidHelper;
        private readonly IEntityRepository<BlogArticle> _blogArticle;
        private readonly IEntityRepository<ApplicationUserGrade> _userGrade;
        private readonly IEntityRepository<ApplicationUserLoginRecord> _userLoginRecord;
        private readonly IEntityRepository<UserLink> _userLink;

        public AccountController(
            IHostingEnvironment hostingEnv,
            ITzClientHelper tzClientHelper,
            ITzPictureCompressionHelper tzPictureCompressionHelper,
            IEntityRepository<BusinessImage> businessImage,
            IEntityRepository<TzSiteLog> tzSiteLog,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IEntityRepository<Person> person,
            IRandomDataHepler randomDataHepler,
            ITzDataValidHelper tzDataValidHelper,
            IEntityRepository<BlogArticle> blogArticle,
            IEntityRepository<ApplicationUserGrade> userGrade,
            IEntityRepository<ApplicationUserLoginRecord> userLoginRecord,
            IEntityRepository<UserLink> userLink
            ) : base(userManager, businessImage, tzSiteLog)
        {
            this._hostingEnv = hostingEnv;
            this._tzClientHelper = tzClientHelper;
            this._tzPictureCompressionHelper = tzPictureCompressionHelper;
            this._roleManager = roleManager;
            this._signInManager = signInManager;
            this._peson = person;
            this._randomDataHepler = randomDataHepler;
            this._tzDataValidHelper = tzDataValidHelper;
            this._blogArticle = blogArticle;
            this._userGrade = userGrade;
            this._userLoginRecord = userLoginRecord;
            this._userLink = userLink;
        }

        /// <summary>
        /// 随机昵称
        /// </summary>
        /// <returns></returns>
        public string RandomNickname()
        {
            return _randomDataHepler.GetRandomNickname();
        }

        /// <summary>
        /// 获取用户是否已经登录
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetUserHasLogin()
        {
            var username = User.Identity.Name;
            if (!string.IsNullOrEmpty(username))
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null) return Json(new { state = false });
                else
                {
                    var avatar = _businessImage.GetAll().FirstOrDefault(x => x.RelevanceObjectId == Guid.Parse(user.Id));
                    var avatarPath = string.IsNullOrEmpty(avatar.MinRelativePath) ? avatar.RelativePath : avatar.MinRelativePath;
                    return Json(new { state = true, avatar = avatarPath });
                }
            }
            else { return Json(new { state = false }); };

        }


        /// <summary>
        /// 用户个人中心页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> UserCenter()
        {
            return View(await GetUserVM());
        }

        /// <summary>
        /// 保存系统默认头像
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> SaveChangeUserAvatar(string src = null)
        {
            if (!string.IsNullOrEmpty(src))
            {
                var avatars = await _businessImage.FindByAsyn(x => x.RelevanceObjectId.Equals(Guid.Parse(TzUser.Id)));
                var avatar = avatars.FirstOrDefault();
                if (avatar != null)
                {
                    avatar.RelativePath = src;
                    avatar.MinRelativePath = src;
                    avatar.PhysicalPath = string.Empty;
                    avatar.MinPhysicalPath = string.Empty;
                    avatar.UpdateTime = DateTime.Now;
                    var result = await _businessImage.AddOrEditAndSaveAsyn(avatar);
                    if (result) return Json(new { state = true, message = "头像修改成功！" });
                    else return Json(new { state = false, message = "头像修改失败！" });
                }
                else { return Json(new { state = false, message = "头像修改失败！" }); }
            }
            else { return Json(new { state = false, message = "修改失败：请选择一张头像！" }); }
        }

        /// <summary>
        /// 保存头像(自定义上传)
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> SaveChangeAvatar()
        {
            try
            {
                var image = Request.Form.Files.First();
                if (image == null)
                {
                    return Json(new { state = false, message = "没有选择头像，请选择头像后再保存。" });
                }
                var currImageName = image.FileName;
                var timeForFile = (DateTime.Now.ToString("yyyyMMddHHmmss") + "-").Trim();
                string extensionName = currImageName.Substring(currImageName.LastIndexOf("."));
                var imageName = ContentDispositionHeaderValue
                                .Parse(image.ContentDisposition)
                                .FileName
                                .Trim('"')
                                .Substring(image.FileName.LastIndexOf("\\") + 1);
                var newImageName = timeForFile + TzUser.Id + extensionName;
                var boPath = "../../images/UploadImages/" + BusinessImageEnum.Avatars.ToString() + "/" + DateTime.Now.ToString("yyyy-MM-dd") + "/" + newImageName;
                var imageSavePath = _hostingEnv.WebRootPath + $@"\images\UploadImages\" + BusinessImageEnum.Avatars.ToString() + "\\" + DateTime.Now.ToString("yyyy-MM-dd");
                imageName = _hostingEnv.WebRootPath + $@"\images\UploadImages\" + BusinessImageEnum.Avatars.ToString() + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\" + newImageName;
                var minFileFolder = _hostingEnv.WebRootPath + $@"\images\UploadImages\" + BusinessImageEnum.Avatars.ToString() + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\Min";
                Directory.CreateDirectory(imageSavePath); //创建目录
                Directory.CreateDirectory(minFileFolder); //创建压缩目录
                using (FileStream fs = System.IO.File.Create(imageName))
                {
                    image.CopyTo(fs);
                    fs.Flush();
                }

                var minFileSavaPath = _hostingEnv.WebRootPath + $@"\images\UploadImages\" + BusinessImageEnum.Avatars.ToString() + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\Min" + "\\min-" + newImageName;
                var minRelativePath = _tzPictureCompressionHelper.GetPicThumbnail(imageName, minFileSavaPath, 0.5, 0.5, 30);

                var avatarQuery = await _businessImage.FindByAsyn(x => x.RelevanceObjectId.Equals(Guid.Parse(TzUser.Id)) && x.Type.Equals(BusinessImageEnum.Avatars));
                var avatar = avatarQuery.FirstOrDefault();
                if (!string.IsNullOrEmpty(avatar.PhysicalPath))
                {
                    System.IO.File.Delete(avatar.PhysicalPath);
                }
                if (!string.IsNullOrEmpty(avatar.MinPhysicalPath))
                {
                    System.IO.File.Delete(avatar.MinPhysicalPath);
                }
                avatar.Type = BusinessImageEnum.Avatars;
                avatar.UploaderId = Guid.Parse(TzUser.Id);
                avatar.UpdateTime = DateTime.Now;
                avatar.RelativePath = boPath;
                avatar.MinRelativePath = minRelativePath;
                avatar.PhysicalPath = imageName;
                avatar.MinPhysicalPath = minFileSavaPath;
                avatar.FileSize = image.Length;
                var r = await _businessImage.AddOrEditAndSaveAsyn(avatar);
                if (r)
                    return Json(new { state = true, message = "修改成功！", url = avatar.RelativePath });
                else
                    return Json(new { state = false, message = "修改失败" });
            }
            catch (Exception)
            {
                return Json(new { state = false, message = "修改失败" });
            }
        }

        /// <summary>
        /// 保存密码修改
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> SaveChangePassword([Bind("Password,ConfirmPassword")]ApplicationUserVM boVM)
        {
            try
            {
                if (boVM.Password == null || boVM.ConfirmPassword == null)
                {
                    return Json(new { result = false, message = "密码修改存在空值，请检查！" });
                }
                if (boVM.Password != boVM.ConfirmPassword)
                {
                    return Json(new { result = false, message = "新密码两次输入不相同，请检查！" });
                }
                // 获取重置密码的令牌
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(TzUser);
                // 重置密码
                await _userManager.ResetPasswordAsync(TzUser, resetToken, boVM.Password);
                return Json(new { result = true, message = "密码修改成功！" });
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "系统出现异常，暂时无法修改密码，请反馈。" });
            }

        }

        /// <summary>
        /// 保存个人信息修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> SaveChangeProfile(UserProfileDto input)
        {
            var currentUser = TzUser;
            var currentId = Guid.Parse(currentUser.Id);
            var userLinks = await _userLink.FindByAsyn(x => x.UserId.Equals(currentId));
            var qqLink = userLinks.FirstOrDefault(x => x.Type.Equals(UserLinkType.QQ));
            qqLink.Link = input.QQLink;
            await _userLink.AddOrEditAndSaveAsyn(qqLink);
            var weiboLink = userLinks.FirstOrDefault(x => x.Type.Equals(UserLinkType.Weibo));
            weiboLink.Link = input.WeiboLink;
            await _userLink.AddOrEditAndSaveAsyn(weiboLink);
            var githubLink = userLinks.FirstOrDefault(x => x.Type.Equals(UserLinkType.Github));
            githubLink.Link = input.GithubLink;
            await _userLink.AddOrEditAndSaveAsyn(githubLink);
            var proSiteLink = userLinks.FirstOrDefault(x => x.Type.Equals(UserLinkType.ProSite));
            proSiteLink.Link = input.ProSiteLink;
            await _userLink.AddOrEditAndSaveAsyn(proSiteLink);

            currentUser.Nickname = input.Nickname;
            currentUser.Remark = input.Remark;
            currentUser.Location = input.Location;
            currentUser.Birthday = input.Birthday;
            var r = await _userManager.UpdateAsync(currentUser);
            if (r.Succeeded) return Json(new { state = true });
            else return Json(new { state = false, message = "保存失败Ծ‸Ծ！" });
        }

        #region 用户注册和登录相关操作

        /// <summary>
        /// 登录注册云盟(独立页面)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult LoginAndRegister()
        {
            if (!string.IsNullOrEmpty(GetUserNameOrNickname()))
            {
                return RedirectToAction("UserCenter");
            }
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(GetUserNameOrNickname()))
            {
                return RedirectToAction("UserCenter");
            }
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// 添加一些默认的用户 、当然在种子数据里面已经添加过了另外的用户
        /// ?在种子数据添加的用户不能够登录，原因可能在于哈希密码
        /// </summary>
        public async Task<IActionResult> AddDefaultUsers()
        {
            //使用文件方式进行判断
            string installLockFile = Path.Combine(_hostingEnv.WebRootPath, @"Install\install.lock");
            bool installLockFileExists = System.IO.File.Exists(installLockFile);

            var uExist = await _userManager.FindByNameAsync("admin");
            if (uExist == null && !installLockFileExists)
            {
                ApplicationRole adminRole = null;
                ApplicationRole userRole = null;
                //?判断用户组是否存在             
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    adminRole = new ApplicationRole() { Name = "Admin", DisplayName = "超级管理员", Description = "适用于系统管理人员", ApplicationRoleType = ApplicationRoleTypeEnum.适用于系统管理人员 };
                    userRole = new ApplicationRole() { Name = "AverageUser", DisplayName = "普通用户", Description = "适用于普通注册用户", ApplicationRoleType = ApplicationRoleTypeEnum.适用于普通注册用户 };
                    await _roleManager.CreateAsync(adminRole);
                    await _roleManager.CreateAsync(userRole);
                }

                //添加默认用户
                string[] userLastStr = new string[] { "", "a", "b", "c", "d", "e", "f", "g" };
                string[] linkNames = { "QQ", "微博", "Github", "个人网站" };
                UserLinkType[] types = { UserLinkType.QQ, UserLinkType.Weibo, UserLinkType.Github, UserLinkType.ProSite };
                const string password = "123@abc";
                for (int i = 0; i < userLastStr.Length + 1; i++)
                {
                    var user = new ApplicationUser();
                    if (i == 0)
                    {
                        const string adPassword = "0925!smile";
                        user = new ApplicationUser("admin")
                        {
                            FullName = "超级管理员",
                            Nickname = "大大二颜",
                            Remark = "talk is cheap,show me the code.",
                            Email = "admin@925i.cn",
                            Birthday = DateTime.Parse("1995-09-25"),
                            Location = "广西·南宁"
                        };
                        var addAdmin = await _userManager.CreateAsync(user);
                        var addAdminPassword = await _userManager.AddPasswordAsync(user, adPassword);

                        //设置默认博客文章用户(发布时，请记得删除)
                        var blogArticles = await _blogArticle.GetAllAsyn();
                        foreach (var blogArticle in blogArticles)
                        {
                            blogArticle.User = user;
                            await _blogArticle.AddOrEditAndSaveAsyn(blogArticle);
                        }

                        //查询用户是否已经添加了权限 若不在添加进用户组
                        if (!await _userManager.IsInRoleAsync(user, adminRole.Name))
                            await _userManager.AddToRoleAsync(user, adminRole.Name);
                    }
                    else
                    {
                        user = new ApplicationUser("user" + userLastStr[i - 1])
                        {
                            Nickname = _randomDataHepler.GetRandomNickname(),
                            Remark = _randomDataHepler.GetRandomRemark(),
                            FullName = "普通用户" + userLastStr[i - 1],
                            Email = "user" + userLastStr[i - 1] + "@925i.cn",
                            Birthday = DateTime.Now,
                            Location = "广西·南宁"
                        };
                        var addUser = await _userManager.CreateAsync(user);
                        var addUserPassword = await _userManager.AddPasswordAsync(user, password);
                        if (!await _userManager.IsInRoleAsync(user, userRole.Name))
                        {
                            var roleOK = await _userManager.AddToRoleAsync(user, userRole.Name);
                        }
                    }

                    //初始化用户等级信息
                    await _userGrade.AddOrEditAndSaveAsyn(new ApplicationUserGrade
                    {
                        User = user,
                        IsAuthentication = false,
                        Level = 1,
                        LevelName = "VIP",
                        Currency = 925,
                        Score = 0
                    });

                    //初始化用户链接
                    for (int linkI = 0; linkI < 4; linkI++)
                    {
                        await _userLink.AddOrEditAndSaveAsyn(new UserLink
                        {
                            UserId = Guid.Parse(user.Id),
                            Name = linkNames[linkI],
                            Link = "javascript:",
                            Target = UserLinkTarget._blank,
                            Type = types[linkI]
                        });
                    }

                    //初始化默认头像
                    var userAvatar = _randomDataHepler.GetRandomAvatar();
                    var avatar = new BusinessImage
                    {
                        Type = BusinessImageEnum.Avatars,
                        Name = string.Empty,
                        DisplayName = string.Empty,
                        OriginalFileName = string.Empty,
                        RelevanceObjectId = Guid.Parse(user.Id),
                        UploaderId = Guid.Parse(user.Id),
                        Description = "这是用户 " + user.FullName + " 的头像",
                        FileSize = 0,
                        RelativePath = userAvatar,
                        MinRelativePath = userAvatar,
                        PhysicalPath = string.Empty,
                        MinPhysicalPath = string.Empty
                    };
                    await _businessImage.AddOrEditAndSaveAsyn(avatar);
                }
                ViewBag.Message = "数据添加完成，您可以通过下方按钮选择一些操作！";

                //添加锁定文件
                using (Stream fs = System.IO.File.Create(installLockFile)) fs.Flush();
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Message = "安装已经完成，如需重新安装请删除安装目录下的install.lock文件，再进行操作。";
                return RedirectToAction("Login");
            }
        }

        /// <summary>
        /// 处理用户登录
        /// </summary>
        /// <param name="loginInformation"></param>
        /// <returns></returns>
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginInformation loginInformation)
        {
            if (!string.IsNullOrEmpty(GetUserNameOrNickname()))
            {
                return RedirectToAction("UserCenter");
            }
            var loginVM = loginInformation;
            if (_HasTheSameUser(loginVM.Username))
            {
                var result = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    //添加用户登录记录
                    var loginUser = await GetUserByName(loginVM.Username);
                    await _userLoginRecord.AddOrEditAndSaveAsyn(new ApplicationUserLoginRecord
                    {
                        UserId = Guid.Parse(loginUser.Id),
                        IpAddress = _tzClientHelper.IPAddress,
                        Browser = string.Empty,
                        City = _tzClientHelper.TaobaoLocation
                    });

                    // 下面的登录成功后的导航应该具体依赖用户所在的角色组来进行处理的。       
                    var tempReturnUrl = loginVM.ReturnUrl?.ToString();
                    var returnUrl = string.IsNullOrEmpty(tempReturnUrl) ? Url.Action("UserCenter", "Account") : tempReturnUrl;
                    return Json(new { state = true, message = "登录成功，正在跳转...", reUrl = returnUrl });
                }
                else
                {
                    return Json(new { state = false, goRegister = false, message = "用户名或密码错误！" });
                }
            }
            else
            {
                return Json(new { state = false, goRegister = true, message = "用户名不存在,是否注册？" });
            }
        }


        /// <summary>
        /// 普通用户资料注册管理
        /// </summary>
        /// <param name="registerInformation"></param>
        /// <returns></returns>
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterInformation registerInformation)
        {
            var registerVM = registerInformation;

            //验证用户名
            if (!_tzDataValidHelper.IsNumAndEnCh(registerVM.UserName))
            {
                return Json(new { state = false, message = "用户名只能包含字母和数字！" });
            }

            if (_tzDataValidHelper.CheckReservedUserName(registerVM.UserName))
            {
                return Json(new { state = false, message = "系统保留用户名无法注册！" });
            }

            if (!string.IsNullOrEmpty(GetUserNameOrNickname()))
            {
                return RedirectToAction("UserCenter");
            }
            if (string.IsNullOrEmpty(registerVM.UserName) || string.IsNullOrEmpty(registerVM.Password) || string.IsNullOrEmpty(registerVM.ConfirmPassword))
            {
                return Json(new { state = false, message = "表单中的所有必填项存在空值，请检查！" });
            }
            if (_HasTheSameNickname(registerVM.Nickname))
            {
                return Json(new { state = false, message = "当前昵称已经被使用Ծ‸Ծ！" });
            }
            if (_HasTheSameUser(registerVM.UserName))
            {
                return Json(new { state = false, message = "当前用户名已经被使用Ծ‸Ծ！" });
            }
            var user = new ApplicationUser(registerVM.UserName)
            {
                Nickname = registerVM.Nickname,
                Remark = _randomDataHepler.GetRandomRemark(),
                Email = string.Empty,
                MobileNumber = string.Empty
            };

            //普通用户
            const string averageUser = "AverageUser";
            //判断用户组是否存在           
            if (!await _roleManager.RoleExistsAsync(averageUser))
            {
                ApplicationRole userRole = new ApplicationRole() { Name = averageUser, DisplayName = "普通注册用户", Description = "适用于普通注册用户", ApplicationRoleType = ApplicationRoleTypeEnum.适用于普通注册用户, SortCode = "99avf56g" };
                await _roleManager.CreateAsync(userRole);
            }
            var a1 = await _userManager.CreateAsync(user);
            var a2 = await _userManager.AddPasswordAsync(user, registerVM.Password);

            //查询用户是否已经添加了权限 若不在添加进用户组
            if (!await _userManager.IsInRoleAsync(user, averageUser))
            {
                var roleOK = await _userManager.AddToRoleAsync(user, averageUser);
            }
            if (a1.Succeeded && a2.Succeeded)
            {
                //初始化用户等级信息
                await _userGrade.AddOrEditAndSaveAsyn(new ApplicationUserGrade
                {
                    User = user,
                    IsAuthentication = false,
                    Level = 1,
                    LevelName = "VIP",
                    Currency = 925,
                    Score = 0
                });

                //初始化链接

                //初始化用户链接
                string[] linkNames = { "QQ", "微博", "Github", "个人网站" };
                var types = new List<UserLinkType> { UserLinkType.QQ, UserLinkType.Weibo, UserLinkType.Github, UserLinkType.ProSite };
                for (int linkI = 0; linkI < 4; linkI++)
                {
                    await _userLink.AddOrEditAndSaveAsyn(new UserLink
                    {
                        UserId = Guid.Parse(user.Id),
                        Name = linkNames[linkI],
                        Link = "javascript:",
                        Target = UserLinkTarget._blank,
                        Type = types[linkI]
                    });
                }

                //注册完成添加默认头像
                var userAvatar = _randomDataHepler.GetRandomAvatar();
                var avatar = new BusinessImage
                {
                    Type = BusinessImageEnum.Avatars,
                    Name = string.Empty,
                    DisplayName = string.Empty,
                    OriginalFileName = string.Empty,
                    RelevanceObjectId = Guid.Parse(user.Id),
                    UploaderId = Guid.Parse(user.Id),
                    Description = "这是用户【" + user.UserName + "】的个人头像",
                    RelativePath = userAvatar,
                    MinRelativePath = userAvatar,
                    PhysicalPath = string.Empty,
                    MinPhysicalPath = string.Empty
                };
                await _businessImage.AddOrEditAndSaveAsyn(avatar);

                return Json(new { state = true, message = "注册成功，请牢记您的账号密码！" });
            }
            else
            {
                return Json(new { state = false, message = "密码强度不够，至少包含：小写字母+数字" });
            }
        }

        /// <summary>
        /// 网站条款和声明
        /// 说明：用户注册注册的时候选项之一
        /// </summary>
        /// <returns></returns>
        public IActionResult SiteTermsAndStatement()
        {
            ViewData["Message"] = "在这里写网站的使用条款，以及其他的一些声明等。。。的综合页面";
            return PartialView();
        }

        /// <summary>
        /// 未登录提示页面
        /// </summary>
        /// <returns></returns>
        public IActionResult NoLogin()
        {
            ViewData["Message"] = " 还未登录，请先登录后再访问";
            return PartialView();
        }

        /// <summary>
        /// 处理用户注销操作
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        /// <summary>
        /// 登录之前获取用户的头像（如果存在）
        /// </summary>
        /// <param name="username"></param>
        /// <returns>用户的头像路径</returns>
        public async Task<IActionResult> GetLoginAvatar(string username)
        {
            try
            {
                var user = await this._userManager.FindByNameAsync(username);
                if (user == null)
                {
                    return Json(new { path = "" });
                }
                else
                {
                    var avatar = this._businessImage.GetAll().FirstOrDefault(i => i.RelevanceObjectId == Guid.Parse(user.Id));
                    if (avatar != null)
                    {
                        var avatarPath = string.IsNullOrEmpty(avatar.MinRelativePath) ? avatar.RelativePath : avatar.MinRelativePath;
                        return Json(new { path = avatarPath });
                    }
                    else
                    {
                        return Json(new { path = "" });
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { path = "" });
            }
        }

        #endregion

        #region 用户中心视图加载

        /// <summary>
        /// 用户中心首页
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Index()
        {
            return PartialView(userPartialViewRootPath + "_Index");
        }

        /// <summary>
        /// 个人信息界面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var currentUser = TzUser;
            var currentUserId = Guid.Parse(currentUser.Id);

            var queryLoginRecord = await _userLoginRecord.FindByAsyn(x => x.UserId.Equals(currentUserId));
            var lastLoginRecord = new ApplicationUserLoginRecord(); //上一次登录记录
            var currentLoginRecord = new ApplicationUserLoginRecord(); //当前登录记录
            var loginRecordCount = queryLoginRecord.Count();

            if (loginRecordCount > 1)
            {
                lastLoginRecord = queryLoginRecord.Skip(loginRecordCount - 1).Take(1).FirstOrDefault();
                currentLoginRecord = queryLoginRecord.FirstOrDefault();
            }
            else
            {
                lastLoginRecord = queryLoginRecord.FirstOrDefault();
                currentLoginRecord = queryLoginRecord.FirstOrDefault();
            }

            var loginRecord = queryLoginRecord.Skip(1).Take(1);
            var model = await GetUserDtoByUser(currentUser);
            model.LastLoginTime = lastLoginRecord.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
            model.LastLoginIP = lastLoginRecord.IpAddress + "(" + lastLoginRecord.City + ")";
            model.CurrentLoginIP = currentLoginRecord.IpAddress + "(" + currentLoginRecord.City + ")";
            model.UserName = currentUser.UserName; //特殊赋值，根据情况而赋值        
            model.UserLinks = new List<UserLinksDto> //虚拟数据
            {
                new UserLinksDto{Name="QQ",Target="_blank",Id=Guid.NewGuid(),Link="javascript:",Type=UserLinkType.QQ},
                new UserLinksDto{Name="微博",Target="_blank",Id=Guid.NewGuid(),Link="javascript:",Type=UserLinkType.Weibo},
                new UserLinksDto{Name="个人网站",Target="_blank",Id=Guid.NewGuid(),Link="javascript:",Type=UserLinkType.ProSite},
                new UserLinksDto{Name="Github",Target="_blank",Id=Guid.NewGuid(),Link="javascript:",Type=UserLinkType.Github}
            };
            return PartialView(userPartialViewRootPath + "_Profile", model);
        }


        /// <summary>
        /// 用户订单界面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Order()
        {

            return PartialView(userPartialViewRootPath + "_Order");
        }

        #endregion

    }
}
