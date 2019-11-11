using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.Common.TzEnums
{
    /// <summary>
    /// 消息通知来源
    /// </summary>
    public enum TzNotificationSource
    {
        /// <summary>
        /// 系统
        /// </summary>
        App,

        /// <summary>
        /// 管理员用户
        /// </summary>
        AppUser,

        /// <summary>
        /// 博客文章评论回复
        /// </summary>
        ArticleCommentReply

    }
}
