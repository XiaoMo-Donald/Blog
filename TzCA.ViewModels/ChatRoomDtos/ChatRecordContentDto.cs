using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.ViewModels.ChatRoomDtos
{
    /// <summary>
    /// 具体的聊天记录Dto对象
    /// </summary>
    public class ChatRecordContentDto
    {
        /// <summary>
        /// 聊天会话的Id
        /// </summary>
        public Guid ChatRecordId { get; set; }

        /// <summary>
        /// 聊天的内容
        /// </summary>
        public string Message { get; set; }
    }
}
