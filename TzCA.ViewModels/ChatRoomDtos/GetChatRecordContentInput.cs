using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.ViewModels.ChatRoomDtos
{
    /// <summary>
    /// 获取用户之间的聊天记录Dto输入对象
    /// </summary>
   public class GetChatRecordContentInput
    {

        /// <summary>
        /// 接收者Id
        /// </summary>
        public Guid ReceiverId { get; set; }

    }
}
