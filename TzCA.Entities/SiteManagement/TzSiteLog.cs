using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Entities.ApplicationOrganization;

namespace TzCA.Entities.SiteManagement
{
    /// <summary>
    /// 网站日志
    /// </summary>
    public class TzSiteLog : EntityBase, IEntity
    {
        /// <summary>
        /// 请求类型（Get、Post）
        /// </summary>
        public virtual string RequestType { get; set; }

        /// <summary>
        /// 请求的地址
        /// </summary>
        public virtual string Url { get; set; }

        /// <summary>
        /// 访问ip地址
        /// </summary>
        public virtual string IpAddress { get; set; }

        /// <summary>
        /// 访问用户
        /// </summary>
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// 用户昵称（未登录：游客）
        /// </summary>
        public virtual string UserNickname { get; set; }

        /// <summary>
        /// 请求耗时（秒/s）
        /// </summary>
        public virtual string ResponseTime { get; set; }
    }
}
