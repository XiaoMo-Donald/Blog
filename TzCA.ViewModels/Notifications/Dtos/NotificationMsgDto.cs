using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Common.TzEnums;

namespace  TzCA.ViewModels.Notifications.Dtos
{
    /// <summary>
    /// 消息模型
    /// </summary>
    public class NotificationMsgDto
    {
        /// <summary>
        /// 消息链接
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 消息来源
        /// </summary>
        public TzNotificationSource Source { get; set; }
    }
}
