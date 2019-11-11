using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Entities.Blogs;
using TzCA.ViewModels.ApplicationOrganization;

namespace TzCA.ViewModels.Blogs.Dtos
{
    /// <summary>
    /// 博客文章点赞Dto
    /// </summary>
    public class BlogArticlePraiseDto
    {
        /// <summary>
        /// 关联的文章
        /// </summary>
        public Guid ArticleId { get; set; }

        /// <summary>
        /// 点赞的用户
        /// </summary>
        public List<ApplicationUserDto> Users { get; set; }

        /// <summary>
        /// 点赞数量
        /// </summary>
        public int Up { get; set; }

        /// <summary>
        /// 踩数量
        /// </summary>
        public int Down { get; set; }
        public BlogArticlePraiseDto() { }

        public BlogArticlePraiseDto(BlogArticlePraise bo)
        {
            this.ArticleId = bo.ArticleId;
            this.Users = new List<ApplicationUserDto>();
            this.Up = bo.Up;
            this.Down = bo.Down;
        }
    }
}
