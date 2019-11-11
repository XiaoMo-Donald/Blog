using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Entities.ApplicationOrganization;
using TzCA.ViewModels.ApplicationOrganization.Ohter;

namespace TzCA.ViewModels.ApplicationOrganization
{
    /// <summary>
    /// 用户信息Dto
    /// </summary>
    public class ApplicationUserDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 头像（Min）
        /// </summary>
        public string MinAvatar { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }

        public string Remark { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public string Level { get; set; } = "VIP6";

        /// <summary>
        /// 认证信息
        /// </summary>
        public string Authentication { get; set; } = "云盟认证";

        /// <summary>
        /// 地理位置
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        /// QQ链接（填写QQ即可）
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// 用户链接
        /// </summary>
        public List<UserLinksDto> UserLinks { get; set; }

        /// <summary>
        /// 当前登录IP
        /// </summary>
        public string CurrentLoginIP { get; set; }

        /// <summary>
        /// 上一次登录IP
        /// </summary>
        public string LastLoginIP { get; set; }

        /// <summary>
        /// 上一次登录时间
        /// </summary>
        public string LastLoginTime { get; set; }


        public ApplicationUserDto() { }

        public ApplicationUserDto(ApplicationUserVM userVM)
        {
            this.Id = userVM.Id;
            this.UserName = userVM.UserName;
            this.Avatar = userVM.Avatar;
            this.MinAvatar = userVM.MinAvatar;
            this.Nickname = userVM.Nickname;
            this.Remark = userVM.Remark;
            this.Birthday = userVM.Birthday.ToString("yyyy-MM-dd");
            this.Location = userVM.Location;
            this.UserLinks = new List<UserLinksDto>();
        }
    }
}
