using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TzCA.ViewModels.Notifications.Dtos;

namespace TzCA.SignalR
{
    /// <summary>
    /// 消息通知接口
    /// </summary>
    public interface ITzNotificationHub
    {
        /// <summary>
        /// 全站消息推送（一般由站长使用）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SendAll(NotificationSendInput input);

        /// <summary>
        /// 消息推送（指定用户）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Send(NotificationSendInput input);
    }
}
