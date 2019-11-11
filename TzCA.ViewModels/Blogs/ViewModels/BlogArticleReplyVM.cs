using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Entities.Blogs;
using TzCA.ViewModels.ApplicationOrganization;

namespace TzCA.ViewModels.Blogs.ViewModels
{
    /// <summary>
    /// 博客文章评论回复视图模型
    /// </summary>
    public class BlogArticleReplyVM : EntityVM
    {
        /// <summary>
        /// 关联对应的评论Id
        /// </summary>
        public Guid CommentId { get; set; }

        /// <summary>
        /// 回复的人
        /// </summary>  
        public ApplicationUserDto Respondent { get; set; }

        /// <summary>
        /// 被回复的人
        /// </summary>   
        public ApplicationUserDto ReceiveRespondent { get; set; }

        /// <summary>
        /// 是否拥有删除权限（按钮显示）
        /// </summary>
        public bool IsDeleted { get; set; }

        public BlogArticleReplyVM() { }

        public BlogArticleReplyVM(BlogArticleReply bo)
        {
            this.SetVM<BlogArticleReply>(bo);
            this.CommentId = bo.CommentId;
            this.Respondent = new ApplicationUserDto();
            this.ReceiveRespondent = new ApplicationUserDto();
        }
    }
}
