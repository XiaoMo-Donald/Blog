using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Common.TzEnums;

namespace TzCA.ViewModels.Notifications.Dtos
{
    /// <summary>
    /// 消息发送参数
    /// </summary>
    public class NotificationSendInput
    {
        /// <summary>
        /// 关联的对象Id
        /// </summary>
        public Guid ObjectId { get; set; }

        /// <summary>
        /// 消息接收者Id
        /// </summary>
        public Guid ReceiverId { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 被回复消息内容源
        /// </summary>
        public string ContentSource { get; set; }

        /// <summary>
        /// 消息来源
        /// </summary>
        public string Source { get; set; }
    }
}
