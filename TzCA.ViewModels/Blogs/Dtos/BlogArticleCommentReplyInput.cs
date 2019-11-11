using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.ViewModels.Blogs.Dtos
{
    /// <summary>
    /// 博客文章评论回复输入参数
    /// </summary>
    public class BlogArticleCommentReplyInput
    {
        /// <summary>
        /// 文章Id
        /// </summary>
       public Guid ArticleId { get; set; }

        /// <summary>
        /// 被回复者Id
        /// </summary>
        public Guid? CommentReplyUserId { get; set; }

        /// <summary>
        /// 评论Id
        /// </summary>
        public Guid CommentId{ get; set; }

        /// <summary>
        /// 评论或者回复内容
        /// </summary>
        public string Content { get; set; }
    }
}
