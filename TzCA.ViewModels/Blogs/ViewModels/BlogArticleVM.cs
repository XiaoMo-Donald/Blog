using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Entities.Blogs;
using TzCA.ViewModels.ApplicationOrganization;
using TzCA.ViewModels.Blogs.Dtos;

namespace TzCA.ViewModels.Blogs.ViewModels
{
    /// <summary>
    /// 博客文章视图模型
    /// </summary>
    public class BlogArticleVM : EntityVM
    {
        /// <summary>
        /// 文章摘要
        /// </summary>
        public string Abstract { get; set; }

        /// <summary>
        /// 博客文章缩略图
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public ApplicationUserDto User { get; set; }

        /// <summary>
        /// 文章分类
        /// </summary>
        public BlogArticleCategoryVM Category { get; set; }

        /// <summary>
        /// 博客文章标签
        /// </summary>
        public List<BlogArticleLabelVM> ArticleLabels { get; set; }

        /// <summary>
        /// 文章评论
        /// </summary>
        public List<BlogArticleCommentVM> Comments { get; set; }

        /// <summary>
        /// 评论回复统计
        /// </summary>
        public int CommentsCount{ get; set; }

        /// <summary>
        /// 浏览统计
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// 文章点赞
        /// </summary>
        public BlogArticlePraiseDto  ArticlePraise{ get; set; }

        public BlogArticleVM() { }

        public BlogArticleVM(BlogArticle bo)
        {
            this.SetVM<BlogArticle>(bo);
            this.Abstract = bo.Abstract;
            this.Thumbnail = bo.Thumbnail;
            this.User = new ApplicationUserDto();
            this.Category =bo.Category!=null? new BlogArticleCategoryVM(bo.Category): new BlogArticleCategoryVM();
            if (bo.BlogArticleLabels != null)
            {
                this.ArticleLabels = new List<BlogArticleLabelVM>();
                foreach (var blogArticleLabel in bo.BlogArticleLabels)
                {
                    this.ArticleLabels.Add(new BlogArticleLabelVM(blogArticleLabel));
                }
            }
            this.Comments = new List<BlogArticleCommentVM>();
            this.ArticlePraise = new BlogArticlePraiseDto();
        }
    }
}
