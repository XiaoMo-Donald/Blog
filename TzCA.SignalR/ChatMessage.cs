using System;
using System.Collections.Generic;
using System.Text;
using TzCA.ViewModels.ChatRoomDtos;

namespace TzCA.SignalR
{
    /// <summary>
    /// 聊天数据模型
    /// </summary>
    public class ChatMessage
    {
        /// <summary>
        /// 发送者
        /// </summary>
        public TzChatUser Sender { get; set; }

        /// <summary>
        /// 接收者Id
        /// </summary>
        public Guid ReceiverId { get; set; }

        /// <summary>
        /// 接收者
        /// </summary>
        public TzChatUser Receiver { get; set; }

        /// <summary>
        /// 接收者的在线状态
        /// </summary>
        public bool ReceiverOnlineState { get; set; }     

        /// <summary>
        /// 发送的消息
        /// </summary>
        public string Message  { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public string SendTime { get; set; }

        public ChatMessage()
        {
            this.SendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }

    }
}
