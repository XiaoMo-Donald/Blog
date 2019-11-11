using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TzCA.Entities.ChatRoom
{
    /// <summary>
    /// 具体的聊天记录
    /// </summary>
    public class ChatRecordContent : EntityBase, IEntity
    {
        /// <summary>
        /// 关联好友的会话
        /// </summary>
        [ForeignKey("ChatRecordId")]
        public virtual ChatRecord ChatRecord { get; set; }

        /// <summary>
        /// 关联好友的会话Id
        /// </summary>
        public virtual Guid ChatRecordId { get; set; }

        /// <summary>
        /// 消息归属者(用于判断是谁发送的)
        /// </summary>
        public virtual Guid AscriptionUserId { get; set; }

        /// <summary>
        /// 发送者
        /// </summary>
        public virtual Guid SenderId { get; set; }

        /// <summary>
        /// 接收者
        /// </summary>
        public virtual Guid ReceiverId { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public virtual string Message { get; set; }
    }
}
