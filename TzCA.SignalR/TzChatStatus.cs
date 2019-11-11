using System;
using System.Collections.Generic;
using System.Text;
using TzCA.ViewModels.ApplicationOrganization;

namespace TzCA.SignalR
{
    /// <summary>
    /// 聊天状态（临时）
    /// </summary>
    public static class TzChatStatus
    {
        /// <summary>
        /// 存当前的用户，给离线时候使用(视图模型)
        /// </summary>
        public static ApplicationUserVM OnUserVM = null;
    }
}
