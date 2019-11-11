using System;
using System.Collections.Generic;
using System.Text;
using TzCA.ViewModels.ApplicationOrganization;

namespace TzCA.ViewModels.Blogs.Dtos
{
    /// <summary>
    /// 评论回复返回的具体数据
    /// </summary>
    public class BlogArticleCommentReplyOutput
    {
        /// <summary>
        /// 当前的Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 评论的用户信息
        /// </summary>
        public ApplicationUserDto CommentUser { get; set; }

        /// <summary>
        /// 回复的用户的信息
        /// </summary>
        public ApplicationUserDto CommentReplyUser { get; set; }

        /// <summary>
        /// 评论或者回复的内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 评论或者回复的时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
