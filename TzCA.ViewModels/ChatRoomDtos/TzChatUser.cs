using System;
using System.Collections.Generic;
using System.Text;
using TzCA.ViewModels.ApplicationOrganization;

namespace TzCA.ViewModels.ChatRoomDtos
{
    /// <summary>
    /// 简单用户model 
    /// </summary>
    public class TzChatUser
    {
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 用户签名
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 连接服务器之后，自动生成的connectionId(当前使用系统用户Id，该链接Id弃用)
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// 聊天所在组
        /// </summary>
        public string GroupId { get; set; }

       

        public TzChatUser()
        {
            this.UserId = Guid.NewGuid();
        }

        public TzChatUser(ApplicationUserVM user)
        {
            this.UserName = user.UserName;
            this.UserId = user.Id;
            this.Nickname = user.Nickname;
            this.Remark = user.Remark;
            this.Avatar = user.Avatar;
        }
    }
}
