using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.ViewModels.Notifications.Dtos
{
    /// <summary>
    /// 消息通知的用户中间对象
    /// </summary>
    public class NotificationHubUserDto
    {
        /// <summary>
        /// 当前连接的id
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// 对应的用户id
        /// </summary>
        public Guid UserId { get; set; }
    }
}
