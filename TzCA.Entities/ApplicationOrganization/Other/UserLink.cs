using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TzCA.Entities.ApplicationOrganization.Other
{
    /// <summary>
    /// 用户个人链接
    /// </summary>
    public class UserLink : EntityBase, IEntity
    {
        /// <summary>
        /// 关联的用户Id
        /// </summary>

        public virtual Guid UserId { get; set; }

        /// <summary>
        /// HTTP链接
        /// </summary>
        public virtual string Link { get; set; }

        /// <summary>
        /// 链接打开方式（_blank:新窗口   _self:当前窗口）
        /// </summary>
        public virtual UserLinkTarget Target { get; set; }

        /// <summary>
        /// 链接类型
        /// </summary>
        public virtual UserLinkType Type { get; set; }
    }
}
