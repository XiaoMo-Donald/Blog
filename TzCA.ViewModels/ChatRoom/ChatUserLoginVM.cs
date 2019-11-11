using System;
using System.Collections.Generic;
using System.Text;
using TzCA.ViewModels.ChatRoomDtos;

namespace TzCA.ViewModels.ChatRoom
{
    /// <summary>
    /// Chat用户登录模型
    /// </summary>
    public class ChatUserLoginVM
    {
        /// <summary>
        /// 当前登录的用户信息
        /// </summary>
        public TzChatUser LoginUser { get; set; }

        /// <summary>
        /// 当前在线的用户
        /// </summary>
        public List<TzChatUser> ChatUserList { get; set; }

        public ChatUserLoginVM()
        {
            this.ChatUserList = new List<TzChatUser>();
        }
    }
}
