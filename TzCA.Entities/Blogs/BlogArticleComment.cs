using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TzCA.Entities.ApplicationOrganization;

namespace TzCA.Entities.Blogs
{
    /// <summary>
    /// 博客文章评论(实现多级评论)
    /// </summary>
    public class BlogArticleComment : EntityBase, IEntity
    {
        //评论的内容使用的是基类的Description属性

        /// <summary>
        /// 对应的文章
        /// </summary>
        [Required]
        public virtual Guid BlogArticleId { get; set; }

        /// <summary>
        /// 该评论下的回复
        /// </summary>
        public virtual ICollection<BlogArticleReply> Replys { get; set; }

        /// <summary>
        /// 评论的人
        /// </summary>
        [Required]
        public virtual Guid ReviewerId { get; set; }
    }
}
