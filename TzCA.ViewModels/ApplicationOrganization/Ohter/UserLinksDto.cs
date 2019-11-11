using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Entities.ApplicationOrganization.Other;

namespace TzCA.ViewModels.ApplicationOrganization.Ohter
{
    /// <summary>
    /// 用户个人网站链接对象
    /// </summary>
    public class UserLinksDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        /// <summary>
        /// 链接名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// HTTP链接
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 链接打开方式（_blank:新窗口   _self:当前窗口）
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public UserLinkType Type { get; set; }

    }
}
