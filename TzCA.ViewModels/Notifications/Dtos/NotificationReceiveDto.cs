using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Common.TzEnums;
using TzCA.ViewModels.ApplicationOrganization;

namespace TzCA.ViewModels.Notifications.Dtos
{
    /// <summary>
    /// 消息接收数据模型
    /// </summary>
    public class NotificationReceiveDto
    {
        /// <summary>
        /// 发送者
        /// </summary>
        public ApplicationUserDto Sender { get; set; }

        /// <summary>
        /// 接收者
        /// </summary>
        public ApplicationUserDto Receiver { get; set; }

        ///// <summary>
        ///// 消息内容
        ///// </summary>
        //public NotificationMsgDto Message { get; set; }

        /// <summary>
        /// 消息链接
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 被回复的内容源
        /// </summary>
        public string ContentSource { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 消息来源
        /// </summary>
        public string Source { get; set; }

        public NotificationReceiveDto()
        {
            this.Sender = new ApplicationUserDto();
            this.Receiver = new ApplicationUserDto();
            //this.Message = new NotificationMsgDto();
        }
    }
}
