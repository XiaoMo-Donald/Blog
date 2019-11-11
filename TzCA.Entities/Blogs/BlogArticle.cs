using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TzCA.Entities.ApplicationOrganization;

namespace TzCA.Entities.Blogs
{
    /// <summary>
    /// 博客文章实体
    /// </summary>
    public class BlogArticle : EntityBase, IEntity
    {
        /// <summary>
        /// 文章摘要
        /// </summary>
        public virtual string Abstract { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public virtual string Thumbnail { get; set; }

        /// <summary>
        /// 归属用户
        /// </summary>   
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// 归属分类
        /// </summary>
        public virtual BlogArticleCategory Category { get; set; }

        /// <summary>
        /// 博客标签
        /// </summary>
        public virtual ICollection<BlogArticleLabel> BlogArticleLabels { get; set; }
    }
}
