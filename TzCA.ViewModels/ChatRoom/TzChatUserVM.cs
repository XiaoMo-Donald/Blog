using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Entities.ApplicationOrganization;

namespace TzCA.ViewModels.ChatRoom
{
    /// <summary>
    /// 聊天室用户视图模型
    /// </summary>
    public class TzChatUserVM
    {
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public Guid Id { get; set; }

        public TzChatUserVM(ApplicationUser user)
        {
            this.Id = Guid.Parse(user.Id);
        }
    }
}
