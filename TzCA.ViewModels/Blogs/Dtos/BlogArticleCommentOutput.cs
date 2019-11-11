using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.ViewModels.Blogs.Dtos
{
    /// <summary>
    /// 评论成功后返回的数据格式
    /// </summary>
    public class BlogArticleCommentOutput
    {
        /// <summary>
        /// 状态码 0成功 1失败
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 回调的消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 回调的数据对象
        /// </summary>
        public BlogArticleCommentReplyOutput Data { get; set; }
    }
}
