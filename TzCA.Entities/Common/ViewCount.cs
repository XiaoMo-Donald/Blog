using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.Entities.Common
{
    /// <summary>
    /// 公共的浏览统计
    /// </summary>
    public class ViewCount : EntityBase, IEntity
    {
        /// <summary>
        /// 统计数量
        /// </summary>
        public virtual int Count { get; set; }

        /// <summary>
        /// 使用该统计的对象id
        /// </summary>
        public virtual Guid ObjectId { get; set; }
    }
}
