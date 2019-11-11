using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TzCA.Entities.ApplicationOrganization;

namespace TzCA.Entities.Blogs
{
    /// <summary>
    /// 评论下的回复
    /// </summary>
    public class BlogArticleReply : EntityBase, IEntity
    {
        //回复的内容使用的是基类的Description属性

        /// <summary>
        /// 关联对应的评论Id
        /// </summary>
        [Required]
        public virtual Guid CommentId { get; set; }

        /// <summary>
        /// 关联对应的评论
        /// </summary>
        [ForeignKey("CommentId")]
        public virtual BlogArticleComment Comment { get; set; }

        /// <summary>
        /// 回复的人
        /// </summary>
        [Required]
        public virtual Guid RespondentId { get; set; }

        /// <summary>
        /// 被回复的人
        /// </summary>
        [Required]
        public virtual Guid ReceiveRespondentId { get; set; }
    }
}
