using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Entities.ChatRoom;
using TzCA.ViewModels.ChatRoom;

namespace TzCA.ViewModels.ChatRoomDtos
{
    /// <summary>
    /// 获取好友（包括好友信息和聊天记录）
    /// </summary>
    public class GetFriendOutput
    {
        /// <summary>
        /// 发送者信息
        /// </summary>
        public TzChatUser Sender { get; set; }

        /// <summary>
        /// 好友信息
        /// </summary>
        public TzChatUser Receiver { get; set; }

        /// <summary>
        /// 聊天记录（历史消息）
        /// </summary>
        public List<ChatRecordContentVM> ChatRecordContents { get; set; }
    }
}
