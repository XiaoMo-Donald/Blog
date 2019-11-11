using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.ViewModels.Blogs.Dtos
{
    /// <summary>
    /// 新建文章输入参数
    /// </summary>
    public class AddBlogArticleInput : EntityDto
    {
        /// <summary>
        /// 文章摘要
        /// </summary>
        public string Abstract { get; set; }

        /// <summary>
        /// 博客文章缩略图
        /// </summary>
        public string Thumbnail { get; set; }
    }
}
