using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.Entities.ChatRoom
{
    /// <summary>
    /// 用户聊天中间实体
    /// </summary>
    public class ChatRecord : EntityBase, IEntity
    {
        /// <summary>
        /// 发送者
        /// </summary>
        public virtual Guid SenderId { get; set; }

        /// <summary>
        /// 接收者
        /// </summary>
        public virtual Guid ReceiverId { get; set; }

        /// <summary>
        /// 该会话下所有的聊天记录
        /// </summary>
        public virtual ICollection<ChatRecordContent> ChatRecordContents { get; set; }
      
    }
}
