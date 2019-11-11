using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Common.TzEnums;

namespace TzCA.Entities.Blogs
{
    /// <summary>
    /// 文章赞踩记录
    /// </summary>
    public class BlogArticlePraiseRecord : EntityBase, IEntity
    {
        /// <summary>
        /// 关联的博客文章id
        /// </summary>
        public virtual Guid ArticleId { get; set; }

        /// <summary>
        /// 点赞的用户
        /// </summary>
        public virtual Guid UserId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public virtual BlogArticlePraiseType UpOrDown { get; set; }
    }
}
