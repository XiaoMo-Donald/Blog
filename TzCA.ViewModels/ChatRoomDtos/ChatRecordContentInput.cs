using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.ViewModels.ChatRoomDtos
{
    /// <summary>
    /// 聊天记录输入参数
    /// </summary>
    public class ChatRecordContentInput
    {
        /// <summary>
        /// 接收者Id
        /// </summary>
        public Guid ReceiverId { get; set; }

        /// <summary>
        /// 聊天的内容
        /// </summary>
        public string Message { get; set; }
    }
}
