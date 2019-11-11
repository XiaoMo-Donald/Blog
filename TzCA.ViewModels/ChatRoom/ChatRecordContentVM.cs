using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Common.JsonModels;
using TzCA.Entities.ChatRoom;

namespace TzCA.ViewModels.ChatRoom
{
    /// <summary>
    /// 聊天记录实体视图模型
    /// </summary>
    public class ChatRecordContentVM : EntityVM, IEntityVM
    {
        /// <summary>
        /// 消息归属者(用于判断是谁发送的)
        /// </summary>
        public Guid AscriptionUserId { get; set; }

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

        public ListPageParameter ListPageParameter { get; set; }

        public ChatRecordContentVM() { }

        public  ChatRecordContentVM(ChatRecordContent bo)
        {           
            this.SetVM<ChatRecordContent>(bo);
            this.AscriptionUserId = bo.AscriptionUserId;
            this.SenderId = bo.SenderId;
            this.ReceiverId = bo.ReceiverId;
            this.Message = bo.Message;
        } 
    }
}
