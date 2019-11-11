using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.Entities
{
    public interface IEntity:IEntityBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        string BussinessCode { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        DateTime UpdateTime { get; set; }
    }
}
