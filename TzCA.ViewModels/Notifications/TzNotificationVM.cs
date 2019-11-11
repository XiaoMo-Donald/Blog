using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Common.TzEnums;
using TzCA.Entities.Notifications;
using TzCA.ViewModels.ApplicationOrganization;

namespace TzCA.ViewModels.Notifications
{
    /// <summary>
    /// 消息通知视图模型
    /// </summary>
    public class TzNotificationVM : EntityVM, IEntityVM
    {
        /// <summary>
        /// 使用对象的Id
        /// </summary>
        public Guid ObjectId { get; set; }

        /// <summary>
        /// 消息通知链接
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 消息发送者
        /// </summary>
        public ApplicationUserDto Sender { get; set; }

        /// <summary>
        /// 消息接收者
        /// </summary>
        public ApplicationUserDto Receiver { get; set; }

        /// <summary>
        /// 消息通知来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 被回复的消息内容源
        /// </summary>
        public string ContentSource { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool Readed { get; set; }

        public TzNotificationVM() { }

        public TzNotificationVM(TzNotification bo)
        {
            this.SetVM<TzNotification>(bo);
            this.Sender = new ApplicationUserDto();
            this.Receiver = new ApplicationUserDto();
            this.Readed = bo.Readed;
            this.Link = bo.Link;
            this.ObjectId = bo.ObjectId;
            this.Source = bo.Source;
            //this.ContentSource = bo.ContentSource;
        }

    }
}
