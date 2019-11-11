using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.ViewModels.ApplicationOrganization.Ohter
{
    /// <summary>
    /// 用户个人信息Dto
    /// </summary>
    public class UserProfileDto
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
        
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 地理位置
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// QQ链接
        /// </summary>
        public string QQLink { get; set; }

        /// <summary>
        /// 微博链接
        /// </summary>
        public string WeiboLink { get; set; }
        
        /// <summary>
        /// 个人网站链接
        /// </summary>
        public string ProSiteLink { get; set; }

        /// <summary>
        /// 个人网站链接
        /// </summary>
        public string GithubLink { get; set; }
    }
}
