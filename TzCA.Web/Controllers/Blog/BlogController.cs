using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TzCA.Common.TzEnums;
using TzCA.Common.TzExtensions;
using TzCA.Common.TzRandomData;
using TzCA.DataAccess;
using TzCA.Entities.ApplicationOrganization;
using TzCA.Entities.Attachments;
using TzCA.Entities.Blogs;
using TzCA.Entities.Common;
using TzCA.Entities.SiteManagement;
using TzCA.SignalR;
using TzCA.ViewModels;
using TzCA.ViewModels.Blogs.Dtos;
using TzCA.ViewModels.Blogs.ViewModels;
using TzCA.ViewModels.TzPagination;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TzCA.Web.Controllers.Blog
{
    /// <summary>
    /// 博客控制器
    /// </summary>
    public class BlogController : TzControllerBase
    {
        private readonly IEntityRepository<BlogArticle> _blogArticle;
        private readonly IEntityRepository<BlogArticleCategory> _blogArticleCategory;
        private readonly IEntityRepository<BlogArticleLabel> _blogArticleLabel;
        private readonly IEntityRepository<BlogArticleComment> _blogArticleComment;
        private readonly IEntityRepository<BlogArticleReply> _blogArticleReply;
        private readonly IEntityRepository<ViewCount> _viewCount;
        private readonly IEntityRepository<BlogArticlePraise> _blogArticlePraise;
        private readonly IEntityRepository<BlogArticlePraiseRecord> _praiseRecord;
        private readonly IRandomDataHepler _randomDataHepler;
        private readonly ITzClientHelper _tzClientHelper;
        private readonly ITzNotificationHub _tzNotification;
        public BlogController(
            UserManager<ApplicationUser> userManager,
            IEntityRepository<BusinessImage> businessImage,
            IEntityRepository<TzSiteLog> tzSiteLog,
            IEntityRepository<BlogArticle> blogArticle,
            IEntityRepository<BlogArticleCategory> blogArticleCategory,
            IEntityRepository<BlogArticleLabel> blogArticleLabel,
            IEntityRepository<BlogArticleComment> blogArticleComment,
            IEntityRepository<BlogArticleReply> blogArticleReply,
            IEntityRepository<ViewCount> viewCount,
            IEntityRepository<BlogArticlePraise> blogArticlePraise,
            IEntityRepository<BlogArticlePraiseRecord> praiseRecord,
            IRandomDataHepler randomDataHepler,
            ITzClientHelper tzClientHelper,
            ITzNotificationHub tzNotification
            ) : base(userManager, businessImage, tzSiteLog)
        {
            this._blogArticle = blogArticle;
            this._blogArticleCategory = blogArticleCategory;
            this._blogArticleLabel = blogArticleLabel;
            this._blogArticleComment = blogArticleComment;
            this._blogArticleReply = blogArticleReply;
            this._viewCount = viewCount;
            this._blogArticlePraise = blogArticlePraise;
            this._praiseRecord = praiseRecord;
            this._randomDataHepler = randomDataHepler;
            this._tzClientHelper = tzClientHelper;
            this._tzNotification = tzNotification;
        }

        public async Task<IActionResult> Index()
        {
            return View(await GetUserVMByName(adminName));
        }

        /// <summary>
        /// 分页获取博客文章
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PaginationOut<List<BlogArticleVM>>> GetArticles(GetBlogArticleInput input)
        {
            var blogArticleVM = new List<BlogArticleVM>();
            var query = await _blogArticle.GetAllIncludingAsyn(x => x.User, x => x.Category, x => x.BlogArticleLabels);
            var articles = query.OrderByDescending(x => x.CreateTime).Skip(input.SkipCount).Take(input.Limit);//分页核心
            foreach (var article in articles)
            {
                var articleVM = new BlogArticleVM(article);
                articleVM.Thumbnail = string.IsNullOrEmpty(article?.Thumbnail) ? _randomDataHepler.GetRandomAvatar() : article?.Thumbnail;
                articleVM.User = await GetUserDtoByUser(article.User);
                articleVM.CommentsCount = await GetCommentCount(article.Id);
                articleVM.ViewCount = await GetArticleViewCount(article.Id);
                blogArticleVM.Add(articleVM);
            }
            var statusCode = HttpContext.Response.StatusCode.Equals(200) ? 0 : 1;
            return new PaginationOut<List<BlogArticleVM>>
            {
                Code = statusCode,
                Msg = statusCode.Equals(0) ? "" : "请求错误Ծ‸Ծ",
                Count = query.Count(),
                Data = blogArticleVM
            };
        }

        /// <summary>
        /// 获取文章总数量
        /// </summary>
        /// <returns></returns>
        public int ArticlesCount()
        {
            var query = _blogArticle.GetAll();
            return query.Count();
        }

        /// <summary>
        /// 获取文章详细页面
        /// </summary>
        /// <param name="id">文章Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ArticleDetail(Guid id)
        {
            var articleQuery = await _blogArticle.GetAllIncludingAsyn(x => x.User, x => x.Category, x => x.BlogArticleLabels);
            var article = articleQuery.FirstOrDefault(x => x.Id.Equals(id));
            var articleVM = new BlogArticleVM(article);
            articleVM.User = await GetUserDtoByUser(article.User);
            articleVM.Comments = await GetBlogArticleComments(article.Id);
            articleVM.ViewCount = await GetArticleViewCount(article.Id);
            articleVM.ArticlePraise = await GetArticlePraise(article.Id);
            await AddViewCount(article.Id);
            return PartialView(articleVM);
        }

        /// <summary>
        /// 获取博客文章所有的评论和回复
        /// </summary>
        /// <param name="articleId">文章id</param>
        /// <param name="author">作者</param>
        /// <returns></returns>
        private async Task<List<BlogArticleCommentVM>> GetBlogArticleComments(Guid articleId)
        {
            var articleCommentsVM = new List<BlogArticleCommentVM>();
            var articleCommentQuery = await _blogArticleComment.GetAllIncludingAsyn(x => x.Replys);
            if (articleCommentQuery.Count() > 0)
            {
                var articleComments = articleCommentQuery.Where(x => x.BlogArticleId.Equals(articleId));
                if (articleComments.Count() > 0)
                {
                    var currUser = TzUser;
                    foreach (var articleComment in articleComments.OrderBy(x => x.CreateTime))
                    {
                        var articleCommentVM = new BlogArticleCommentVM(articleComment);
                        if (articleComment.Replys.Count > 0)
                        {
                            foreach (var reply in articleComment.Replys.OrderBy(x => x.CreateTime))
                            {
                                var replyVM = new BlogArticleReplyVM(reply);
                                var respondent = await GetUserDtoById(reply.RespondentId);
                                replyVM.Respondent = respondent;

                                #region 回复的删除权限

                                if (currUser == null) replyVM.IsDeleted = false;
                                else if (respondent.Id.Equals(Guid.Parse(currUser.Id)) || adminName.Equals(currUser.UserName))
                                    replyVM.IsDeleted = true;
                                else replyVM.IsDeleted = false;

                                #endregion

                                replyVM.ReceiveRespondent = await GetUserDtoById(reply.ReceiveRespondentId);
                                articleCommentVM.Replys.Add(replyVM);
                            }
                        }
                        var reviewer = await GetUserDtoById(articleComment.ReviewerId);
                        articleCommentVM.Reviewer = reviewer;

                        #region 评论的删除权限

                        if (currUser == null) articleCommentVM.IsDeleted = false;
                        else if (reviewer.Id.Equals(Guid.Parse(currUser.Id)) || adminName.Equals(currUser.UserName))
                            articleCommentVM.IsDeleted = true;
                        else articleCommentVM.IsDeleted = false;

                        #endregion

                        articleCommentsVM.Add(articleCommentVM);
                    }
                }
            }
            return articleCommentsVM;
        }

        /// <summary>
        /// 评论
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<TzDataOutputDto<BlogArticleCommentReplyOutput>> Comment(BlogArticleCommentReplyInput input)
        {
            var comment = new BlogArticleComment
            {
                BlogArticleId = input.ArticleId,
                Description = input.Content,
                ReviewerId = Guid.Parse(TzUser.Id)
            };
            var r = await _blogArticleComment.AddOrEditAndSaveAsyn(comment);
            return new TzDataOutputDto<BlogArticleCommentReplyOutput>
            {
                State = r,
                Code = HttpContext.Response.StatusCode,
                Msg = r ? "" : "评论失败！",
                Data = !r ? null : new BlogArticleCommentReplyOutput
                {
                    Id = comment.Id,
                    Content = comment.Description,
                    CommentReplyUser = await GetUserDtoByUser(await GetUserByName(adminName)),
                    CommentUser = await GetUserDtoById(comment.ReviewerId),
                    CreateTime = comment.CreateTime
                }
            };
        }

        /// <summary>
        /// 回复
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<TzDataOutputDto<BlogArticleCommentReplyOutput>> Reply(BlogArticleCommentReplyInput input)
        {
            bool r = false;
            string message = string.Empty;
            var reply = new BlogArticleReply();
            var currUser = TzUser;
            if (currUser.Id.Equals(input.CommentReplyUserId.Value.ToString()))
            {
                message = "您不能回复自己！";
            }
            else
            {
                reply = new BlogArticleReply
                {
                    CommentId = input.CommentId,
                    ReceiveRespondentId = input.CommentReplyUserId.Value,
                    RespondentId = Guid.Parse(currUser.Id),
                    Description = input.Content
                };
                r = await _blogArticleReply.AddOrEditAndSaveAsyn(reply);
                if (r)
                {
                    //await _tzNotification.Send(new NotificationSendInput
                    //{
                    //    ObjectId = reply.Id,
                    //    Content = input.Content,
                    //    ReceiverId = reply.ReceiveRespondentId,
                    //    Source = TzNotificationSource.ArticleReply
                    //});
                }
                message = "回复失败!";
            }
            return new TzDataOutputDto<BlogArticleCommentReplyOutput>
            {
                State = r,
                Code = HttpContext.Response.StatusCode,
                Msg = r ? "" : message,
                Data = !r ? null : new BlogArticleCommentReplyOutput
                {
                    Id = reply.Id,
                    Content = reply.Description,
                    CommentReplyUser = await GetUserDtoById(reply.RespondentId),
                    CommentUser = await GetUserDtoById(reply.ReceiveRespondentId),
                    CreateTime = reply.CreateTime
                }
            };
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="id">要删除的评论的id</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> DeleteComment(Guid id)
        {
            if (id.Equals(Guid.Empty)) return Json(new { state = false, message = "删除失败！" });
            var comment = await _blogArticleComment.GetSingleAsyn(id);
            if (comment != null)
            {
                var currUser = TzUser;
                var adminUser = AdminUser;
                if (currUser.Id.Equals(adminUser.Id) || comment.ReviewerId.Equals(Guid.Parse(currUser.Id)))
                {
                    var r = await _blogArticleComment.DeleteAndSaveAsyn(comment.Id);
                    if (r.DeleteSatus)
                    {
                        return Json(new { state = true, message = "删除成功！" });
                    }
                    else return Json(new { state = false, message = r.Message });
                }
                else return Json(new { state = false, message = "权限不足,不要搞事情Ծ‸Ծ" });


            }
            return Json(new { state = false, message = "删除失败！" });
        }


        /// <summary>
        /// 删除回复
        /// </summary>
        /// <param name="id">要删除的回复的id</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> DeleteReply(Guid id)
        {
            if (id.Equals(Guid.Empty)) return Json(new { state = false, message = "删除失败！" });

            var reply = await _blogArticleReply.GetSingleAsyn(id);
            if (reply != null)
            {
                var currUser = TzUser;
                var adminUser = AdminUser;
                if (currUser.Id.Equals(adminUser.Id) || reply.RespondentId.Equals(Guid.Parse(currUser.Id)))
                {
                    var r = await _blogArticleReply.DeleteAndSaveAsyn(reply.Id);
                    if (r.DeleteSatus)
                        return Json(new { state = true, message = "删除成功！" });
                    else
                        return Json(new { state = false, message = r.Message });

                }
                else return Json(new { state = false, message = "权限不足,不要搞事情Ծ‸Ծ" });
            }
            return Json(new { state = false, message = "删除失败，该回复不存在或者已经被删除！" });
        }

        /// <summary>
        ///  获取文章评论数量统计（用于文章回复刷新）
        /// </summary>
        /// <param name="articleId">文章id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<int> GetCommentCount(Guid articleId)
        {
            if (Guid.Empty.Equals(articleId))
                return 0;
            var comments = await _blogArticleComment.FindByAsyn(x => x.BlogArticleId.Equals(articleId));
            return comments.Count();
        }

        /// <summary>
        /// 获取评论的回复数量
        /// </summary>
        /// <param name="commentId">评论的id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<int> GetReplyCount(Guid commentId)
        {
            if (Guid.Empty.Equals(commentId))
                return 0;
            var replys = await _blogArticleReply.FindByAsyn(x => x.CommentId.Equals(commentId));
            return replys.Count();
        }

        /// <summary>
        /// 根据文章Id获取该文章的所有评论回复数量
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<int> GetAllCommentCount(Guid articleId)
        {
            var queryComment = await _blogArticleComment.FindByAsyn(x => x.BlogArticleId.Equals(articleId));
            var commentCount = await GetCommentCount(articleId);
            var replyCount = await GetReplyCount(queryComment.FirstOrDefault().Id);
            return commentCount + replyCount;
        }


        /// <summary>
        /// 增加浏览数量+1
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AddViewCount(Guid articleId)
        {
            var query = await _viewCount.FindByAsyn(x => x.ObjectId.Equals(articleId));
            var viewCount = query.FirstOrDefault();
            viewCount.Count += 1;
            var r = await _viewCount.AddOrEditAndSaveAsyn(viewCount);
            return r;
        }

        /// <summary>
        /// 根据文章Id获取文章的浏览
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<int> GetArticleViewCount(Guid articleId)
        {
            var query = await _viewCount.FindByAsyn(x => x.ObjectId.Equals(articleId));
            return query.FirstOrDefault().Count;
        }

        /// <summary>
        /// 根据文章Id获取文章的赞踩数量
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<BlogArticlePraiseDto> GetArticlePraise(Guid articleId)
        {
            var query = await _blogArticlePraise.FindByAsyn(x => x.ArticleId.Equals(articleId));
            var articlePraise = query.FirstOrDefault();
            var articlePraiseVM = new BlogArticlePraiseDto(articlePraise);

            //获取该文章的踩、赞用户记录
            var queryPraiseRecord = await _praiseRecord.FindByAsyn(x => x.ArticleId.Equals(articleId));
            foreach (var praiseRecord in queryPraiseRecord)
            {
                articlePraiseVM.Users.Add(await GetUserDtoById(praiseRecord.UserId));
            }
            return articlePraiseVM;
        }

        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<TzDataOutputDto<BlogArticlePraiseDto>> ArticlePraiseUp(Guid articleId)
        {
            string msg = string.Empty;
            bool state = false;

            //查询记录 PraiseRecord
            var queryPraiseRecord = await _praiseRecord.FindByAsyn(x => x.ArticleId.Equals(articleId) && x.UserId.Equals(Guid.Parse(TzUser.Id)));
            var praiseUpRecord = queryPraiseRecord.FirstOrDefault(x => x.UpOrDown.Equals(BlogArticlePraiseType.Up));
            var praiseDownRecord = queryPraiseRecord.FirstOrDefault(x => x.UpOrDown.Equals(BlogArticlePraiseType.Down));
            var articlePraiseDto = new BlogArticlePraiseDto();
            if (queryPraiseRecord.Count().Equals(0))
            {
                //添加点赞记录
                await _praiseRecord.AddOrEditAndSaveAsyn(new BlogArticlePraiseRecord
                {
                    ArticleId = articleId,
                    UserId = Guid.Parse(TzUser.Id),
                    UpOrDown = BlogArticlePraiseType.Up
                });
                //文章点赞数量+1
                var query = await _blogArticlePraise.FindByAsyn(x => x.ArticleId.Equals(articleId));
                var articlePraise = query.FirstOrDefault();
                articlePraise.Up += 1;
                await _blogArticlePraise.AddOrEditAndSaveAsyn(articlePraise);
                articlePraiseDto = new BlogArticlePraiseDto(articlePraise);
                msg = "您赞了这篇文章(*^▽^*)";
                state = true;
            }
            else if (praiseUpRecord != null)
                msg = "您已经赞过这篇文章了(*^▽^*)";
            else if (praiseDownRecord != null)
            {
                var query = await _blogArticlePraise.FindByAsyn(x => x.ArticleId.Equals(articleId));
                var articlePraise = query.FirstOrDefault();
                if (articlePraise != null)
                {
                    articlePraise.Up += 1;
                    articlePraise.Down -= 1;
                    await _blogArticlePraise.AddOrEditAndSaveAsyn(articlePraise);

                    //点赞记录(还原为up)
                    praiseDownRecord.UpOrDown = BlogArticlePraiseType.Up;
                    await _praiseRecord.AddOrEditAndSaveAsyn(praiseDownRecord);

                    articlePraiseDto = new BlogArticlePraiseDto(articlePraise);
                    state = true;
                    msg = "您赞了这篇文章(*^▽^*)";
                }
                else msg = "请求错误！";
            }

            var statusCode = HttpContext.Response.StatusCode.Equals(200) ? 0 : 1;
            return new TzDataOutputDto<BlogArticlePraiseDto>
            {
                State = state,
                Code = statusCode,
                Msg = msg,
                Data = articlePraiseDto
            };
        }

        /// <summary>
        /// 踩
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<TzDataOutputDto<BlogArticlePraiseDto>> ArticlePraiseDown(Guid articleId)
        {
            string msg = string.Empty;
            bool state = false;

            //查询记录 PraiseRecord
            var queryPraiseRecord = await _praiseRecord.FindByAsyn(x => x.ArticleId.Equals(articleId) && x.UserId.Equals(Guid.Parse(TzUser.Id)));
            var praiseUpRecord = queryPraiseRecord.FirstOrDefault(x => x.UpOrDown.Equals(BlogArticlePraiseType.Up));
            var praiseDownRecord = queryPraiseRecord.FirstOrDefault(x => x.UpOrDown.Equals(BlogArticlePraiseType.Down));
            var articlePraiseDto = new BlogArticlePraiseDto();
            if (queryPraiseRecord.Count().Equals(0))
            {
                //添加踩记录
                await _praiseRecord.AddOrEditAndSaveAsyn(new BlogArticlePraiseRecord
                {
                    ArticleId = articleId,
                    UserId = Guid.Parse(TzUser.Id),
                    UpOrDown = BlogArticlePraiseType.Down
                });
                //文章踩数量+1
                var query = await _blogArticlePraise.FindByAsyn(x => x.ArticleId.Equals(articleId));
                var articlePraise = query.FirstOrDefault();
                articlePraise.Down += 1;
                await _blogArticlePraise.AddOrEditAndSaveAsyn(articlePraise);
                articlePraiseDto = new BlogArticlePraiseDto(articlePraise);
                msg = "您踩了这篇文章Ծ‸Ծ";
                state = true;
            }
            else if (praiseDownRecord != null)
                msg = "您已经踩过这篇文章了Ծ‸Ծ";
            else if (praiseUpRecord != null)
            {
                var query = await _blogArticlePraise.FindByAsyn(x => x.ArticleId.Equals(articleId));
                var articlePraise = query.FirstOrDefault();
                if (articlePraise != null)
                {
                    articlePraise.Up -= 1;
                    articlePraise.Down += 1;
                    await _blogArticlePraise.AddOrEditAndSaveAsyn(articlePraise);

                    //踩记录(还原)
                    praiseUpRecord.UpOrDown = BlogArticlePraiseType.Down;
                    await _praiseRecord.AddOrEditAndSaveAsyn(praiseUpRecord);

                    articlePraiseDto = new BlogArticlePraiseDto(articlePraise);
                    state = true;
                    msg = "您踩了这篇文章Ծ‸Ծ";
                }
                else msg = "请求错误Ծ‸Ծ!";
            }

            var statusCode = HttpContext.Response.StatusCode.Equals(200) ? 0 : 1;
            return new TzDataOutputDto<BlogArticlePraiseDto>
            {
                State = state,
                Code = statusCode,
                Msg = msg,
                Data = articlePraiseDto
            };
        }
    }
}
