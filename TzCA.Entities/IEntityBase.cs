using System;

namespace TzCA.Entities
{
    public interface IEntityBase
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        Guid Id { get; set; }
    }
}
