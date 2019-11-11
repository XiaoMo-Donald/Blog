using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TzCA.Entities.ApplicationOrganization
{
    /// <summary>
    /// 用户等级信息、认证信息
    /// </summary>
    public class ApplicationUserGrade : EntityBase, IEntity
    {
        ///// <summary>
        ///// 关联的用户Id
        ///// </summary>
        //[Required]
        //public virtual Guid UserId { get; set; }

        /// <summary>
        /// 关联的用户
        /// </summary>
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// 是否已经认证
        /// </summary>
        public virtual bool IsAuthentication { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public virtual int Level { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public virtual string LevelName { get; set; }

        /// <summary>
        /// 云盟积分
        /// </summary>
        public virtual int Score { get; set; }

        /// <summary>
        ///云盟币
        /// </summary>
        public virtual int Currency { get; set; }
    }
}
