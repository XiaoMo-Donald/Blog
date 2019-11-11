using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TzCA.Entities.ApplicationOrganization;
using TzCA.Entities.Attachments;
using TzCA.Entities.BusinessManagement.Audit;
using TzCA.Entities.BusinessOrganization;
using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Entities.ChatRoom;
using TzCA.Entities.Blogs;
using TzCA.Entities.SiteManagement;
using TzCA.Entities.Notifications;
using TzCA.Entities.Common;
using TzCA.Entities.ApplicationOrganization.Other;

namespace TzCA.DataAccess.SqlServer
{
    public class EntityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public EntityDbContext(DbContextOptions<EntityDbContext> options) : base(options)
        {
        }

        #region 用户与角色相关
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationUserGrade> ApplicationUserGrades { get; set; }
        public DbSet<ApplicationUserLoginCount> ApplicationUserLoginCounts { get; set; }
        public DbSet<ApplicationUserLoginRecord> ApplicationUserLoginRecords { get; set; }
        public DbSet<InvitationCode> InvitationCodes { get; set; }
        public DbSet<UserLink> UserLinks { get; set; }

        #endregion

        #region 用户工作区与菜单相关
        public DbSet<SystemWorkPlace> SystemWorkPlaces { get; set; }
        public DbSet<SystemWorkSection> SystemWorkSections { get; set; }
        public DbSet<SystemWorkTask> SystemWorkTasks { get; set; }
        #endregion

        #region 业务组织相关
        public DbSet<Person> Persons { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        #endregion

        #region 一些基础业务对象相关
        public DbSet<AuditRecord> AuditRecords { get; set; }
        public DbSet<BusinessFile> BusinessFiles { get; set; }
        public DbSet<BusinessImage> BusinessImages { get; set; }
        public DbSet<BusinessVideo> BusinessVideos { get; set; }
        #endregion


        #region 聊天室相关
        public DbSet<ChatRecord> ChatRecords { get; set; }
        public DbSet<ChatRecordContent> ChatRecordContents { get; set; }

        #endregion

        #region 博客相关 
        public DbSet<BlogArticleCategory> BlogArticleCategories { get; set; }
        public DbSet<BlogArticle> BlogArticles { get; set; }
        public DbSet<BlogArticleLabel> BlogArticleLabels { get; set; }
        public DbSet<BlogArticleComment> BlogArticleComments { get; set; }
        public DbSet<BlogArticleReply> BlogArticleReplys { get; set; }
        public DbSet<BlogArticlePraise> BlogArticlePraises { get; set; }
        public DbSet<BlogArticlePraiseRecord> BlogArticlePraiseRecords { get; set; }
        #endregion

        #region 网站管理相关
        public DbSet<TzSiteLog> TzSiteLogs { get; set; }
        public DbSet<TzNotification> TzNotifications { get; set; }
        public DbSet<ViewCount> ViewCounts { get; set; }
        #endregion

        /// <summary>
        /// 如果不需要 DbSet<T> 所定义的属性名称作为数据库表的名称，可以在下面的位置自己重新定义
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<Person>().ToTable("Person");
            base.OnModelCreating(modelBuilder);

        }
    }
}
