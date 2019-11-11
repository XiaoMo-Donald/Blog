using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TzCA.Entities
{
    /// <summary>
    /// 实体基类
    /// </summary>
    public class EntityBase
    {
        /// <summary>
        /// 记录的唯一标识
        /// </summary>
        [Key]
        public virtual Guid Id { get; set; }

        /// <summary>
        /// 创建的时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新的时间
        /// </summary>
        public virtual DateTime UpdateTime { get; set; }

        /// <summary>
        ///  显示名称
        /// </summary>
        [StringLength(1000)]
        public virtual string Name { get; set; }            

        /// <summary>
        /// 描述（说明）
        /// </summary>  
        public virtual string Description { get; set; }

        /// <summary>
        /// 内部业务编码
        /// </summary>
        [StringLength(150)]
        public virtual string BussinessCode { get; set; }        

        /// <summary>
        /// 基类构造函数
        /// </summary>
        public EntityBase()
        {
            this.Id =Guid.NewGuid();
            this.CreateTime = DateTime.Now;
        }
    }
}
