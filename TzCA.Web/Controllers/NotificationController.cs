using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TzCA.DataAccess;
using TzCA.Entities.ApplicationOrganization;
using TzCA.Entities.Attachments;
using TzCA.Entities.Blogs;
using TzCA.Entities.Notifications;
using TzCA.Entities.SiteManagement;
using TzCA.ViewModels.Notifications;
using TzCA.ViewModels.TzPagination;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TzCA.Web.Controllers
{
    /// <summary>
    /// 消息控制器
    /// </summary>
    public class NotificationController : TzControllerBase
    {
        private readonly IEntityRepository<TzNotification> _notice;
        private readonly IEntityRepository<BlogArticle> _blogArticle;
        private readonly IEntityRepository<BlogArticleComment> _blogArticleComment;
        private readonly IEntityRepository<BlogArticleReply> _blogArticleReply;
        public NotificationController(
              UserManager<ApplicationUser> userManager,
              IEntityRepository<BusinessImage> businessImage,
              IEntityRepository<TzSiteLog> tzSiteLog,
              IEntityRepository<TzNotification> notice,
              IEntityRepository<BlogArticle> blogArticle,
              IEntityRepository<BlogArticleComment> blogArticleComment,
              IEntityRepository<BlogArticleReply> blogArticleReply
            ) : base(userManager, businessImage, tzSiteLog)
        {
            this._notice = notice;
            this._blogArticle = blogArticle;
            this._blogArticleComment = blogArticleComment;
            this._blogArticleReply = blogArticleReply;
        }

        /// <summary>
        /// 获取未读消息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> UnReadModalData()
        {
            return PartialView(await GetUnRead(1));
        }

        /// <summary>
        /// 获取未读消息
        /// </summary>
        /// <param name="page">页码</param>
        /// <returns></returns>
        [Authorize]
        public async Task<PaginationOut<List<TzNotificationVM>>> GetUnRead(int page)
        {
            var query = await _notice.FindByAsyn(x => x.ReceiverId.Equals(Guid.Parse(TzUser.Id)) && !x.Readed);
            var notificationsVM = new List<TzNotificationVM>();
            if (!query.Count().Equals(0))
            {
                //分页
                var notifications = query.OrderByDescending(x => x.CreateTime).Skip((page - 1) * 4).Take(4);
                foreach (var item in notifications)
                {
                    var notificationVM = new TzNotificationVM(item);
                    notificationVM.Sender = await GetUserDtoById(item.SenderId);
                    var contentSourceIsDeleted = string.Empty;
                    switch (item.Source)
                    {
                        case "文章评论":
                            var article = await _blogArticle.GetSingleAsyn(item.ObjectId);
                            if (article == null)
                            {
                                notificationVM.ContentSource = item.ContentSource;
                                contentSourceIsDeleted = "<span class='contentSourceIsDeleted'>(原文已删除)</span>";
                            }
                            else
                                notificationVM.ContentSource = article.Name;
                            notificationVM.Description = "评论了文章 " + contentSourceIsDeleted + "《<span class='contentSource' title='" + notificationVM.ContentSource + "'></span> 》：<span class='content' title='" + item.Description + "'>" + item.Description + "</span>";
                            break;
                        case "文章点赞":
                            var article2 = await _blogArticle.GetSingleAsyn(item.ObjectId);
                            if (article2 == null)
                            {
                                notificationVM.ContentSource = item.ContentSource;
                                contentSourceIsDeleted = "<span class='contentSourceIsDeleted'>(原文已删除)</span>";
                            }
                            else
                                notificationVM.ContentSource = article2.Name;
                            notificationVM.Description = "赞了文章" + contentSourceIsDeleted + "《<span class='contentSourceUnSub' title='" + notificationVM.ContentSource + "'>" + notificationVM.ContentSource + "</span> 》";
                            break;
                        case "文章被踩":
                            var article3 = await _blogArticle.GetSingleAsyn(item.ObjectId);
                            if (article3 == null)
                            {
                                notificationVM.ContentSource = item.ContentSource;
                                contentSourceIsDeleted = "<span class='contentSourceIsDeleted'>(原文已删除)</span>";
                            }
                            else
                                notificationVM.ContentSource = article3.Name;
                            notificationVM.Description = "踩了文章" + contentSourceIsDeleted + "《<span class='contentSourceUnSub' title='" + notificationVM.ContentSource + "'>" + notificationVM.ContentSource + "</span> 》";
                            break;
                        case "评论回复":
                            var omment = await _blogArticleComment.GetSingleAsyn(item.ObjectId);
                            if (omment == null)
                            {
                                notificationVM.ContentSource = item.ContentSource;
                                contentSourceIsDeleted = "<span class='contentSourceIsDeleted'>(原文已删除)</span>";
                            }
                            else
                                notificationVM.ContentSource = omment.Description;
                            notificationVM.Description = "回复了内容 " + contentSourceIsDeleted + "<span class='contentSource'  title='" + notificationVM.ContentSource + "'></span> ：<span class='content' title='" + item.Description + "'>" + item.Description + "</span>";
                            break;
                        case "用户回复":
                            var reply = await _blogArticleReply.GetSingleAsyn(item.ObjectId);
                            if (reply == null)
                            {
                                notificationVM.ContentSource = item.ContentSource;
                                contentSourceIsDeleted = "<span class='contentSourceIsDeleted'>(原文已删除)</span>";
                            }
                            else
                                notificationVM.ContentSource = reply.Description;
                            notificationVM.Description = "回复了内容 " + contentSourceIsDeleted + "<span class='contentSource' title='" + notificationVM.ContentSource + "'></span> ：<span class='content' title='" + item.Description + "'>" + item.Description + "</span>";
                            break;
                        case "删除评论":
                        case "删除回复":
                            notificationVM.Description = "删除了评论内容：<span class='content' title='" + item.Description + "'>" + item.Description + "</span>";
                            break;
                        default: notificationVM.Description = "<span class='content' title='" + item.Description + "'>" + item.Description + "</span>"; break;
                    }
                    notificationsVM.Add(notificationVM);
                }
            }
            var statusCode = HttpContext.Response.StatusCode.Equals(200) ? 0 : 1;
            var pageCount = Math.Ceiling(query.Count() / 4.0);
            var count = Int32.Parse(Math.Round(pageCount, 0).ToString());
            return new PaginationOut<List<TzNotificationVM>>
            {
                Code = statusCode,
                Msg = statusCode.Equals(0) ? "" : "请求错误Ծ‸Ծ",
                Count = count,
                Data = notificationsVM
            };
        }

        /// <summary>
        /// 未读消息数量
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<int> GetUnReadCount()
        {
            var currentUser = TzUser;
            var query = await _notice.FindByAsyn(x => x.ReceiverId.Equals(Guid.Parse(currentUser.Id)) && !x.Readed);
            var count = query.Count();
            return count;
        }
    }
}
