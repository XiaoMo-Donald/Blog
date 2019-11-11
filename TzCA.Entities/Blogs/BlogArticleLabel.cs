using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TzCA.Entities.Blogs
{
    /// <summary>
    /// 博客标签
    /// </summary>
    public class BlogArticleLabel : EntityBase, IEntity
    {
        /// <summary>
        /// 关联的博客文章Id
        /// </summary>
        [Required]
        public virtual Guid BlogArticleId { get; set; }

        /// <summary>
        /// 博客文章
        /// </summary>
        [ForeignKey("BlogArticleId")]
        public virtual BlogArticle BlogArticle { get; set; }
    }
}
