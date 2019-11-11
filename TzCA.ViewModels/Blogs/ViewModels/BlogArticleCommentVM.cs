using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Entities.Blogs;
using TzCA.ViewModels.ApplicationOrganization;

namespace TzCA.ViewModels.Blogs.ViewModels
{
    /// <summary>
    /// 博客文章评论视图模型
    /// </summary>
    public class BlogArticleCommentVM : EntityVM
    {
        /// <summary>
        /// 对应的文章
        /// </summary>    
        public Guid BlogArticleId { get; set; }

        /// <summary>
        /// 该评论下的所有回复
        /// </summary>
        public List<BlogArticleReplyVM> Replys { get; set; }

        /// <summary>
        /// 评论的人
        /// </summary>     
        public ApplicationUserDto Reviewer { get; set; }

        /// <summary>
        /// 是否拥有删除权限
        /// </summary>
        public bool IsDeleted { get; set; }

        public BlogArticleCommentVM() { }

        public BlogArticleCommentVM(BlogArticleComment bo)
        {
            this.SetVM<BlogArticleComment>(bo);
            this.BlogArticleId = bo.BlogArticleId;
            this.Replys = new List<BlogArticleReplyVM>();
            this.Reviewer = new ApplicationUserDto();
        }
    }
}
