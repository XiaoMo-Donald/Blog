using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Entities.Blogs;

namespace TzCA.ViewModels.Blogs.ViewModels
{
    /// <summary>
    /// 博客文章数据模型（后台管理）
    /// </summary>
    public class AdminBlogArticleVM : EntityVM
    {
        public AdminBlogArticleVM() { }
        public AdminBlogArticleVM(BlogArticle bo)
        {
            this.SetVM<BlogArticle>(bo);
        }
    }
}
