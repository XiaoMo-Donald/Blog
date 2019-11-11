using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TzCA.Entities.ApplicationOrganization
{
    /// <summary>
    /// 用户登录次数
    /// </summary>
   public class ApplicationUserLoginCount : EntityBase, IEntity
    {
        ///// <summary>
        ///// 关联的用户id
        ///// </summary>
        //[Required]
        //public virtual Guid UserId { get; set; }

        /// <summary>
        /// 关联的用户
        /// </summary>
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        public virtual int Count { get; set; }
    }
}
