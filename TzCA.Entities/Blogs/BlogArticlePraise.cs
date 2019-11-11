using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.Entities.Blogs
{
    /// <summary>
    /// 文章点赞或者踩
    /// </summary>
    public class BlogArticlePraise : EntityBase, IEntity
    {
        /// <summary>
        /// 关联的文章
        /// </summary>
        public virtual Guid ArticleId { get; set; }

        /// <summary>
        /// 点赞数量
        /// </summary>
        public virtual int Up { get; set; }

        /// <summary>
        /// 踩数量
        /// </summary>
        public virtual int Down { get; set; }
    }
}
