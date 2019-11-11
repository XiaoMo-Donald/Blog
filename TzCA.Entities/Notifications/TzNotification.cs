using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Common.TzEnums;

namespace TzCA.Entities.Notifications
{
    /// <summary>
    /// 消息通知
    /// </summary>
    public class TzNotification : EntityBase, IEntity
    {
        //消息接收者
        //消息发送者       
        //消息通知名称
        //消息内容
        //消息发送时间
        //消息来源类型：系统（管理员）、普通（用户）
        //消息状态
        //消息通知链接

        /// <summary>
        /// 关联的对象Id
        /// </summary>
        public virtual Guid ObjectId { get; set; }

        /// <summary>
        /// 消息通知链接
        /// </summary>
        public virtual string Link { get; set; }

        /// <summary>
        /// 消息发送者
        /// </summary>
        public virtual Guid SenderId { get; set; }

        /// <summary>
        /// 消息接收者
        /// </summary>
        public virtual Guid ReceiverId { get; set; }

        /// <summary>
        /// 消息通知来源
        /// </summary>
        public virtual string Source { get; set; }

        /// <summary>
        /// 被回复消息内容源
        /// </summary>
        public virtual string ContentSource { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public virtual bool Readed { get; set; }
    }
}
