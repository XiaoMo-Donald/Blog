using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Entities.Blogs;

namespace TzCA.ViewModels.Blogs.ViewModels
{
    /// <summary>
    /// 博客文章类别视图模型
    /// </summary>
    public class BlogArticleCategoryVM : EntityVM
    {
        public BlogArticleCategoryVM() { }
        public BlogArticleCategoryVM(BlogArticleCategory bo)
        {
            this.SetVM<BlogArticleCategory>(bo);
        }
    }
}
