using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TzCA.DataAccess;
using TzCA.DataAccess.Seeds;
using TzCA.DataAccess.SqlServer;
using TzCA.DataAccess.SqlServerr;
using TzCA.Entities.ApplicationOrganization;
using TzCA.Entities.Attachments;
using TzCA.Entities.BusinessOrganization;
using TzCA.Web.Utilities;

namespace TzCA.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 添加 EF Core 框架
            services.AddDbContext<EntityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),b=>b.MigrationsAssembly("TzCA.DataAccess")));
            //services.AddDbContext<EntityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            // 添加微软自己的用户登录令牌资料
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<EntityDbContext>()
                .AddDefaultTokenProviders();//.AddPasswordValidator();

            // Add framework services.
            services.AddMvc();

            // 配置 Identity
            services.Configure<IdentityOptions>(options =>
            {
                // 密码策略的常规设置
                options.Password.RequireDigit = true;            // 是否需要数字字符
                options.Password.RequiredLength = 6;             // 必须的长度
                options.Password.RequireNonAlphanumeric = true;  // 是否需要非拉丁字符，如%，@ 等
                options.Password.RequireUppercase = false;        // 是否需要大写字符
                options.Password.RequireLowercase = false;        // 是否需要小写字符

                // 登录尝试锁定策略
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // Cookie 设置
                options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(150);
                options.Cookies.ApplicationCookie.LoginPath = "/Account/NoLogin";   // 缺省的登录路径
                options.Cookies.ApplicationCookie.LogoutPath = "/Home/Index";       // 注销以后的路径

                // 其它的一些设置
                options.User.RequireUniqueEmail = true;
            });

            #region 域控制器相关的依赖注入服务清单
            services.AddTransient<IEntityRepository<SystemWorkPlace>, EntityRepository<SystemWorkPlace>>();
            services.AddTransient<IEntityRepository<SystemWorkSection>, EntityRepository<SystemWorkSection>>();
            services.AddTransient<IEntityRepository<SystemWorkTask>, EntityRepository<SystemWorkTask>>();
            services.AddTransient<IEntityRepository<BusinessFile>, EntityRepository<BusinessFile>>();
            services.AddTransient<IEntityRepository<BusinessImage>, EntityRepository<BusinessImage>>();
            services.AddTransient<IEntityRepository<Person>, EntityRepository<Person>>();
            services.AddTransient<IEntityRepository<Department>, EntityRepository<Department>>();


            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, EntityDbContext context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseStatusCodePagesWithReExecute("/Error/Error");
                //app.UseExceptionHandler("/Error/Error");
                app.UseStatusCodePagesWithReExecute("/Home/Error");
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseIdentity();

            ////配置SignalR路由
            //app.UseSignalR(route =>
            //{
            //    route.MapHub<MyChatHub>("/myChathub");
            //});

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
