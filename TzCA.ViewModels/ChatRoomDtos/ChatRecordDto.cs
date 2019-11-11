using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Entities;

namespace TzCA.ViewModels.ChatRoomDtos
{
    /// <summary>
    /// 聊天记录Dto对象
    /// </summary>
    public class ChatRecordDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 发送者
        /// </summary>
        public Guid SenderId { get; set; }

        /// <summary>
        /// 接收者
        /// </summary>
        public Guid ReceiverId { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }
       
    }
}
