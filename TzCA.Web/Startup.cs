using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TzCA.Common.PictureCompression;
using TzCA.Common.TzDataValid;
using TzCA.Common.TzExtensions;
using TzCA.Common.TzRandomData;
using TzCA.DataAccess;
using TzCA.DataAccess.Seeds;
using TzCA.DataAccess.SqlServer;
using TzCA.DataAccess.SqlServerr;
using TzCA.Entities.ApplicationOrganization;
using TzCA.Entities.ApplicationOrganization.Other;
using TzCA.Entities.Attachments;
using TzCA.Entities.Blogs;
using TzCA.Entities.BusinessOrganization;
using TzCA.Entities.ChatRoom;
using TzCA.Entities.Common;
using TzCA.Entities.Notifications;
using TzCA.Entities.SiteManagement;
using TzCA.SignalR;
using TzCA.Web.Utilities;

namespace TzCA.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 添加 EF Core 框架
            services.AddDbContext<EntityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("TzCA.DataAccess")));
            //services.AddDbContext<EntityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            // 添加微软自己的用户登录令牌资料
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<EntityDbContext>()
                .AddDefaultTokenProviders();//.AddPasswordValidator();

            //设置允许跨域请求（比如Api调用）
            // services.AddCors(options => options.AddPolicy("CorsPolicy",
            //             builder =>
            //             {
            //                 builder.AllowAnyMethod().AllowAnyHeader()
            //                        .WithOrigins("http://localhost:55830")
            //                        .AllowCredentials();
            //             })
            //);


            #region SignalR相关配置

            //添加AddSignalR服务
            services.AddSignalR();
            //配置接口注入
            services.AddTransient<ITzChatRepository, TzChatRepository>();

            #endregion

            //使用Session           
            //services.AddDistributedMemoryCache();//启用session之前必须先添加内存
            //services.AddSession(options =>
            //{
            //    options.Cookie.Name = ".AdventureWorks.Session";
            //    //options.IdleTimeout = TimeSpan.FromSeconds(10);//设置session的过期时间
            //    options.Cookie.HttpOnly = true;//设置在浏览器不能通过js获得该cookie的值
            //});
            //services.AddSession();

            //添加Mvc框架
            services.AddMvc();

            // 配置 Identity
            services.Configure<IdentityOptions>(options =>
            {
                // 密码策略的常规设置
                options.Password.RequireDigit = true;            // 是否需要数字字符
                options.Password.RequiredLength = 6;             // 必须的长度
                options.Password.RequireNonAlphanumeric = false;  // 是否需要非拉丁字符，如%，@ 等
                options.Password.RequireUppercase = false;        // 是否需要大写字符
                options.Password.RequireLowercase = false;        // 是否需要小写字符

                // 登录尝试锁定策略
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;//10次失败的尝试将账户锁定
                options.Lockout.AllowedForNewUsers = false;//是否锁定新用户
                // Cookie 设置
                options.User.RequireUniqueEmail = false; //校验邮箱唯一

                //.net core 1.0 配置
                //options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(150);
                //options.Cookies.ApplicationCookie.LoginPath = "/Account/NoLogin";   // 缺省的登录路径
                //options.Cookies.ApplicationCookie.LogoutPath = "/Home/Index";       // 注销以后的路径
            });

            //配置Cookis
            services.ConfigureApplicationCookie(options =>
            {
                //.net core 2.0+ 配置
                options.ExpireTimeSpan = TimeSpan.FromDays(150);
                options.LoginPath = "/Account/Login"; //未登录跳转
                options.LogoutPath = "/Account/Login"; //注销跳转
                options.AccessDeniedPath = "/Error/Denied"; //权限不足跳转页面

            });

            ////添加jwt验证：
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = true,//是否验证Issuer
            //            ValidateAudience = true,//是否验证Audience
            //            ValidateLifetime = true,//是否验证失效时间
            //            ValidateIssuerSigningKey = true,//是否验证SecurityKey
            //            ValidAudience = "925i.cn",//Audience
            //            ValidIssuer = "925i.cn",//Issuer，这两项和前面签发jwt的设置一致
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"]))//拿到SecurityKey
            //        };
            //    });


            #region 域控制器相关的依赖注入服务清单
            services.AddTransient<IEntityRepository<SystemWorkPlace>, EntityRepository<SystemWorkPlace>>();
            services.AddTransient<IEntityRepository<SystemWorkSection>, EntityRepository<SystemWorkSection>>();
            services.AddTransient<IEntityRepository<SystemWorkTask>, EntityRepository<SystemWorkTask>>();
            services.AddTransient<IEntityRepository<BusinessFile>, EntityRepository<BusinessFile>>();
            services.AddTransient<IEntityRepository<BusinessImage>, EntityRepository<BusinessImage>>();
            services.AddTransient<IEntityRepository<Person>, EntityRepository<Person>>();
            services.AddTransient<IEntityRepository<Department>, EntityRepository<Department>>();

            #region 用户相关
            services.AddTransient<IEntityRepository<ApplicationUserGrade>, EntityRepository<ApplicationUserGrade>>();
            services.AddTransient<IEntityRepository<ApplicationUserLoginCount>, EntityRepository<ApplicationUserLoginCount>>();
            services.AddTransient<IEntityRepository<ApplicationUserLoginRecord>, EntityRepository<ApplicationUserLoginRecord>>();
            services.AddTransient<IEntityRepository<InvitationCode>, EntityRepository<InvitationCode>>();
            services.AddTransient<IEntityRepository<UserLink>, EntityRepository<UserLink>>();
            #endregion

            #region 聊天室相关
            services.AddTransient<IEntityRepository<ChatRecord>, EntityRepository<ChatRecord>>();
            services.AddTransient<IEntityRepository<ChatRecordContent>, EntityRepository<ChatRecordContent>>();
            #endregion

            #region 博客相关
            services.AddTransient<IEntityRepository<BlogArticleCategory>, EntityRepository<BlogArticleCategory>>();
            services.AddTransient<IEntityRepository<BlogArticle>, EntityRepository<BlogArticle>>();
            services.AddTransient<IEntityRepository<BlogArticleLabel>, EntityRepository<BlogArticleLabel>>();
            services.AddTransient<IEntityRepository<BlogArticleComment>, EntityRepository<BlogArticleComment>>();
            services.AddTransient<IEntityRepository<BlogArticleReply>, EntityRepository<BlogArticleReply>>();
            services.AddTransient<IEntityRepository<BlogArticlePraise>, EntityRepository<BlogArticlePraise>>();
            services.AddTransient<IEntityRepository<BlogArticlePraiseRecord>, EntityRepository<BlogArticlePraiseRecord>>();
            services.AddTransient<ITzNotificationHub, TzNotificationHub>();
            #endregion

            #endregion

            #region 注册单例上下文请求接口  
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #endregion

            #region Common层接口相关
            services.AddSingleton<IRandomDataHepler, RandomDataHepler>();
            services.AddSingleton<ITzDataValidHelper, TzDataValidHelper>();
            services.AddSingleton<ITzClientHelper, TzClientHelper>();
            services.AddSingleton<ITzPictureCompressionHelper, TzPictureCompressionHelper>();
            #endregion

            #region 网站管理相关
            services.AddTransient<IEntityRepository<TzSiteLog>, EntityRepository<TzSiteLog>>();
            services.AddTransient<IEntityRepository<TzNotification>, EntityRepository<TzNotification>>();
            services.AddTransient<IEntityRepository<ViewCount>, EntityRepository<ViewCount>>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, EntityDbContext context, IServiceProvider svp)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Error/Error/{0}");
                app.UseExceptionHandler("/Error/Error/{0}");
                //app.UseStatusCodePagesWithReExecute("/Home/Error");
                //app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            //使用身份认证（微软）
            app.UseAuthentication();

            //启用跨域请求
            //app.UseCors("CorsPolicy");

            //配置SignalR路由
            app.UseSignalR(route =>
            {
                route.MapHub<TzNotificationHub>("/tzNotificationHub"); //消息通知
                route.MapHub<TzChatHub>("/tzChatHub"); //聊天室
            });

            //配置自定义请求上下文
            //TzHttpContext.ServiceProvider = svp;

            //使用Session
            //app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //数据初始化
            DbInitializer.Initialize(context);
            MenuItemCollection.Initializer(context);
        }
    }
}
