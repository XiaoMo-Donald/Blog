using TzCA.DataAccess.SqlServer;
using TzCA.Entities.ApplicationOrganization;
using TzCA.Entities.BusinessOrganization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TzCA.Entities.Blogs;
using TzCA.Entities.Common;
using TzCA.Entities.ApplicationOrganization.Other;

namespace TzCA.DataAccess.Seeds
{
    /// <summary>
    /// 构建一个初始化原始数据的组件，用于程序启动的时候执行一些数据初始化的操作
    /// </summary>
    public static class DbInitializer
    {
        static EntityDbContext _Context;

        public static void Initialize(EntityDbContext context)
        {
            _Context = context;
            context.Database.EnsureCreated(); //如果创建了，则不会重新创建

            //_AddApplicationRoles();
            _SetWorkPlace();
            _AddBlogArticles();
        }


        private static void _AddApplicationRoles()
        {
            if (_Context.ApplicationRoles.Any())
                return;
            var roles = new List<ApplicationRole>()
            {
               new ApplicationRole(){Name="Admin",DisplayName="系统管理人员", Description="适用于系统管理人员",ApplicationRoleType=ApplicationRoleTypeEnum.适用于系统管理人员,SortCode="69a5f56g" },
               new ApplicationRole(){Name="Maintain",DisplayName="业务数据维护人员", Description="适用于业务数据维护人员",ApplicationRoleType=ApplicationRoleTypeEnum.适用于业务数据维护人员,SortCode="49aaf56g" },
               new ApplicationRole(){Name="AverageUser",DisplayName="普通注册用户", Description="适用于普通注册用户",ApplicationRoleType=ApplicationRoleTypeEnum.适用于普通注册用户 ,SortCode="99avf56g"}
            };

            foreach (var role in roles)
            {
                _Context.ApplicationRoles.Add(role);
            }
            _Context.SaveChanges();
        }


        private static void _SetWorkPlace()
        {
            var wp01 = new SystemWorkPlace() { Name = "系统管理", Description = "", BussinessCode = "wp01", IconString = "mif-cog" };

            var ws01 = new SystemWorkSection() { Name = "角色用户", Description = "", BussinessCode = "wp01ws01" };
            var ws02 = new SystemWorkSection() { Name = "导航菜单", Description = "", BussinessCode = "wp01ws02" };

            var wt0101 = new SystemWorkTask() { Name = "系统角色管理", Description = "", BussinessCode = "wp01ws01wt001", IconName = "mif-tools", BusinessEntityName = "ApplicationRole", ControllerName = "ApplicationRole", ControllerMethod = "", ControllerMethodParameter = "" };
            var wt0102 = new SystemWorkTask() { Name = "系统用户管理", Description = "", BussinessCode = "wp01ws01wt002", IconName = "mif-user-3", BusinessEntityName = "ApplicationUser", ControllerName = "ApplicationUser", ControllerMethod = "", ControllerMethodParameter = "" };

            ws01.SystemWorkTasks = new List<SystemWorkTask>();
            ws01.SystemWorkTasks.Add(wt0101);
            ws01.SystemWorkTasks.Add(wt0102);

            var wt0201 = new SystemWorkTask() { Name = "通用菜单配置管理", Description = "", BussinessCode = "wp01ws01wt001", IconName = "mif-tools", BusinessEntityName = "SystemConfig", ControllerName = "SystemConfig", ControllerMethod = "", ControllerMethodParameter = "" };
            ws02.SystemWorkTasks = new List<SystemWorkTask>();
            ws02.SystemWorkTasks.Add(wt0201);

            wp01.SystemWorkSections = new List<SystemWorkSection>();
            wp01.SystemWorkSections.Add(ws01);
            wp01.SystemWorkSections.Add(ws02);
            _Context.SystemWorkPlaces.Add(wp01);

            _Context.SaveChanges();

        }

        /// <summary>
        /// 添加博客文章标签
        /// </summary>
        private static void _AddBlogArticleLabels()
        {
            if (_Context.BlogArticleLabels.Any())
                return;
            var blogArticleLabels = new List<BlogArticleLabel>
            {
               new BlogArticleLabel{Name="技术"},
               new BlogArticleLabel{Name="后台"},
               new BlogArticleLabel{Name="前端"},
            };
        }

        /// <summary>
        /// 添加博客文章种子数据
        /// </summary>
        private static void _AddBlogArticles()
        {
            #region 添加默认用户（无法用做登录）
            if (_Context.ApplicationUsers.Any())
                return;
            var applicationUsers = new List<ApplicationUser>();
            for (int i = 0; i < 4; i++)
            {
                applicationUsers.Add(new ApplicationUser
                {
                    UserName = "demoUser" + i.ToString(),
                    Nickname = "测试用户" + i.ToString(),
                    Remark = "测试用户" + i.ToString() + "的个性签名",
                    Birthday = DateTime.Now
                });
            }
            _Context.ApplicationUsers.AddRange(applicationUsers);

            if (_Context.UserLinks.Any())
                return;
            var userLinks = new List<UserLink>();
            string[] linkNames = { "QQ", "微博", "Github", "个人网站" };
            var types = new List<UserLinkType> {
               UserLinkType.QQ,UserLinkType.Weibo,UserLinkType.Github,UserLinkType.ProSite
           };
            for (int i = 0; i < applicationUsers.Count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    userLinks.Add(new UserLink
                    {
                        UserId = Guid.Parse(applicationUsers[i].Id),
                        Name = linkNames[j],
                        Link = "javascript:",
                        Target = UserLinkTarget._blank,
                        Type = types[j]
                    });
                }
                _Context.UserLinks.AddRange(userLinks);
            }


            #endregion

            #region 添加分类
            if (_Context.BlogArticleCategories.Any())
                return;
            var blogArticleCategories = new List<BlogArticleCategory>
            {
                new BlogArticleCategory{Name="前端技术" },
                new BlogArticleCategory{Name="后台技术" },
                new BlogArticleCategory{Name="技术文章" }
            };
            _Context.BlogArticleCategories.AddRange(blogArticleCategories);
            #endregion

            #region 添加文章
            if (_Context.BlogArticles.Any())
                return;
            var blogArticles = new List<BlogArticle>();
            for (int i = 1; i <= 1; i++)
            {
                var blogArticle = new BlogArticle
                {
                    User = null,
                    Name = "大大二颜の博客标题 " + i,
                    Abstract = "这是大大二颜的博客测试内容1111111",
                    Description = "<p>这是大大二颜的博客测试内容1111111</p>"
                };
                blogArticle.Category = blogArticleCategories[0];
                //if (i <= 6)
                //    blogArticle.Category = blogArticleCategories[0];
                //else if (6 < i && i < 15)
                //    blogArticle.Category = blogArticleCategories[1];
                //else
                //    blogArticle.Category = blogArticleCategories[2];

                blogArticles.Add(blogArticle);
            };
            _Context.BlogArticles.AddRange(blogArticles);
            #endregion

            #region 添加标签
            if (_Context.BlogArticleLabels.Any())
                return;
            var blogArticleLabels = new List<BlogArticleLabel>();
            for (int i = 0; i < blogArticles.Count; i++)
            {
                blogArticleLabels.Add(new BlogArticleLabel
                {
                    Name = "大大二颜",
                    Description = blogArticles[i].Name + "的标签",
                    BlogArticleId = blogArticles[i].Id
                });
                blogArticleLabels.Add(new BlogArticleLabel
                {
                    Name = "后台",
                    Description = blogArticles[i].Name + "的标签",
                    BlogArticleId = blogArticles[i].Id
                });
                blogArticleLabels.Add(new BlogArticleLabel { Name = "前端", Description = blogArticles[i].Name + "的标签", BlogArticleId = blogArticles[i].Id });
            };
            _Context.BlogArticleLabels.AddRange(blogArticleLabels);
            #endregion

            #region 添加评论
            if (_Context.BlogArticleComments.Any())
                return;
            var blogArticleComments = new List<BlogArticleComment>();
            for (int i = 0; i < blogArticles.Count; i++)
            {
                var blogArticleComment = new BlogArticleComment
                {
                    BlogArticleId = blogArticles[i].Id,
                    Description = "你好啊，你的文章不错哦" + i.ToString()
                };
                blogArticleComment.ReviewerId = Guid.Parse(applicationUsers[0].Id);
                //if (i <= 6)
                //    blogArticleComment.ReviewerId = Guid.Parse(applicationUsers[0].Id);
                //else if (6 < i && i < 15)
                //    blogArticleComment.ReviewerId = Guid.Parse(applicationUsers[1].Id);
                //else
                //    blogArticleComment.ReviewerId = Guid.Parse(applicationUsers[2].Id);

                blogArticleComments.Add(blogArticleComment);
            }
            _Context.BlogArticleComments.AddRange(blogArticleComments);
            #endregion

            #region 添加回复
            if (_Context.BlogArticleReplys.Any())
                return;
            var blogArticleReplys = new List<BlogArticleReply>();
            for (int i = 0; i < blogArticleComments.Count; i++)
            {
                var blogArticleReply = new BlogArticleReply
                {
                    Description = "回复内容哈哈哈" + i.ToString(),
                    CommentId = blogArticleComments[i].Id,
                    RespondentId = Guid.Parse(applicationUsers[3].Id)
                };
                blogArticleReply.ReceiveRespondentId = Guid.Parse(applicationUsers[0].Id);
                //if (i <= 6)
                //    blogArticleReply.ReceiveRespondentId = Guid.Parse(applicationUsers[0].Id);
                //else if (6 < i && i < 15)
                //    blogArticleReply.ReceiveRespondentId = Guid.Parse(applicationUsers[1].Id);
                //else
                //    blogArticleReply.ReceiveRespondentId = Guid.Parse(applicationUsers[2].Id);

                blogArticleReplys.Add(blogArticleReply);
            }
            _Context.BlogArticleReplys.AddRange(blogArticleReplys);
            #endregion

            #region 添加浏览统计
            if (_Context.ViewCounts.Any())
                return;
            var viewCounts = new List<ViewCount>();
            for (int i = 0; i < blogArticles.Count; i++)
            {
                viewCounts.Add(new ViewCount
                {
                    ObjectId = blogArticles[i].Id,
                    Count = 0
                });
            }
            _Context.ViewCounts.AddRange(viewCounts);
            #endregion

            #region 添加点赞
            if (_Context.BlogArticlePraises.Any())
                return;
            var blogArticlePraises = new List<BlogArticlePraise>();
            for (int i = 0; i < blogArticles.Count; i++)
            {
                blogArticlePraises.Add(new BlogArticlePraise
                {
                    ArticleId = blogArticles[i].Id,
                    Up = 0,
                    Down = 0
                });
            }
            _Context.BlogArticlePraises.AddRange(blogArticlePraises);
            #endregion

            _Context.SaveChanges();
        }

    }
}
