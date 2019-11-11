using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Entities.Blogs;

namespace TzCA.ViewModels.Blogs.ViewModels
{
    /// <summary>
    /// 博客文章标签
    /// </summary>
    public class BlogArticleLabelVM : EntityVM
    {
        public BlogArticleLabelVM() { }
        public BlogArticleLabelVM(BlogArticleLabel bo)
        {
         this.SetVM<BlogArticleLabel>(bo);
        }
    }
}
