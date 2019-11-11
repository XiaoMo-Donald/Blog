using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.Entities.ApplicationOrganization
{
    /// <summary>
    /// 用户登录记录
    /// </summary>
    public class ApplicationUserLoginRecord : EntityBase, IEntity
    {
        //登录时间 CreateTime      

        /// <summary>
        /// 用户id
        /// </summary>
        public virtual Guid UserId { get; set; }

        /// <summary>
        /// 登录ip地址
        /// </summary>
        public virtual string IpAddress { get; set; }

        /// <summary>
        /// 登录的具体位置
        /// </summary>
        public virtual string City { get; set; }

        /// <summary>
        /// 登录的浏览器信息
        /// </summary>
        public virtual string Browser { get; set; }
    }
}
