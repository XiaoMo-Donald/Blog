using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TzCA.Common.PictureCompression;
using TzCA.Common.TzRandomData;
using TzCA.DataAccess;
using TzCA.Entities.ApplicationOrganization;
using TzCA.Entities.Attachments;
using TzCA.Entities.Blogs;
using TzCA.Entities.Common;
using TzCA.Entities.SiteManagement;
using TzCA.ViewModels.Blogs.Dtos;
using TzCA.ViewModels.Blogs.ViewModels;
using TzCA.ViewModels.TzFileUpload;
using TzCA.ViewModels.TzPagination;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TzCA.Web.Controllers.Admin
{
    /// <summary>
    /// 后台数据控制器
    /// </summary>
    [Authorize]
    [Authorize(Roles = "Admin")]
    public class AdminDataController : TzControllerBase
    {
        private IHostingEnvironment _hostingEnv;
        private readonly IEntityRepository<BlogArticle> _blogArticle;
        private readonly IEntityRepository<BlogArticleCategory> _blogArticleCategory;
        private readonly IEntityRepository<BlogArticleLabel> _blogArticleLabel;
        private readonly IEntityRepository<BlogArticlePraise> _blogArticlePraise;
        private readonly IEntityRepository<ViewCount> _viewCount;
        private readonly IRandomDataHepler _randomDataHepler;
        private readonly ITzPictureCompressionHelper _tzPictureCompressionHelper;
        public AdminDataController(
            IHostingEnvironment hostingEnv,
            UserManager<ApplicationUser> userManager,
            IEntityRepository<BusinessImage> businessImage,
            IEntityRepository<TzSiteLog> tzSiteLog,
            IEntityRepository<BlogArticle> blogArticle,
            IEntityRepository<BlogArticleCategory> blogArticleCategory,
            IEntityRepository<BlogArticleLabel> blogArticleLabel,
            IEntityRepository<BlogArticlePraise> blogArticlePraise,
            IEntityRepository<ViewCount> viewCount,
            IRandomDataHepler randomDataHepler,
            ITzPictureCompressionHelper tzPictureCompressionHelper
            ) : base(userManager, businessImage, tzSiteLog)
        {
            this._hostingEnv = hostingEnv;
            this._blogArticle = blogArticle;
            this._blogArticleCategory = blogArticleCategory;
            this._blogArticleLabel = blogArticleLabel;
            this._blogArticlePraise = blogArticlePraise;
            this._viewCount = viewCount;
            this._randomDataHepler = randomDataHepler;
            this._tzPictureCompressionHelper = tzPictureCompressionHelper;
        }

        #region 博客文章相关

        /// <summary>
        /// 获取博客文章（列表显示）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns> 
        [HttpGet]
        public async Task<PaginationOut<List<BlogArticleVM>>> GetBlogArticles(GetBlogArticleInput input)
        {
            var blogArticleVM = new List<BlogArticleVM>();
            var query = await _blogArticle.GetAllAsyn();
            var articles = query.OrderBy(x => x.CreateTime).Skip(input.SkipCount).Take(input.Limit);//分页核心
            var counter = 0;//序号
            foreach (var article in articles)
            {
                var articleVM = new BlogArticleVM(article);
                articleVM.SortCode = ++counter + (input.Page - 1) * input.Limit;
                blogArticleVM.Add(articleVM);
            }
            var statusCode = HttpContext.Response.StatusCode.Equals(200);
            return new PaginationOut<List<BlogArticleVM>>
            {
                Code = statusCode ? 0 : 1,
                Msg = statusCode ? "" : "Error",
                Count = query.Count(),
                Data = blogArticleVM
            };
        }

        /// <summary>
        /// 上传博客文章图片
        /// </summary>  
        /// <returns></returns>   
        [HttpPost]
        public async Task<ImageUploadVM> UploadBlogArticleImage()
        {
            try
            {
                var image = Request.Form.Files.First();
                if (image == null)
                {
                    return new ImageUploadVM
                    {
                        Code = 1,
                        Msg = "没有选择图片",
                        Data = null
                    };
                }
                var currImageName = image.FileName;
                var timeForFile = (DateTime.Now.ToString("yyyyMMddHHmmss") + "-").Trim();
                string extensionName = currImageName.Substring(currImageName.LastIndexOf("."));
                var imageName = ContentDispositionHeaderValue
                                .Parse(image.ContentDisposition)
                                .FileName
                                .Trim('"')
                                .Substring(image.FileName.LastIndexOf("\\") + 1);
                var newImageName = timeForFile + Guid.NewGuid().ToString() + extensionName;
                var boPath = "../../images/UploadImages/" + BusinessImageEnum.ArticleImages.ToString() + "/" + DateTime.Now.ToString("yyyy-MM-dd") + "/" + newImageName;
                var imagePath = _hostingEnv.WebRootPath + $@"\images\UploadImages\" + BusinessImageEnum.ArticleImages.ToString() + "\\" + DateTime.Now.ToString("yyyy-MM-dd");
                imageName = _hostingEnv.WebRootPath + $@"\images\UploadImages\" + BusinessImageEnum.ArticleImages.ToString() + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\" + newImageName;
                var minFileFolder = _hostingEnv.WebRootPath + $@"\images\UploadImages\" + BusinessImageEnum.ArticleImages.ToString() + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\Min";
                Directory.CreateDirectory(imagePath); //创建目录
                Directory.CreateDirectory(minFileFolder);//创建压缩目录
                using (FileStream fs = System.IO.File.Create(imageName))
                {
                    image.CopyTo(fs);
                    fs.Flush();
                }
                var minFileSavaPath = _hostingEnv.WebRootPath + $@"\images\UploadImages\" + BusinessImageEnum.ArticleImages.ToString() + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\Min" + "\\min-" + newImageName;
                var minRelativePath = _tzPictureCompressionHelper.GetPicThumbnail(imageName, minFileSavaPath, 1, 1, 75);

                var articleImage = new BusinessImage
                {
                    Type = BusinessImageEnum.ArticleImages,
                    UpdateTime = DateTime.Now,
                    RelativePath = boPath,
                    MinRelativePath = minFileSavaPath,
                    PhysicalPath = imageName,
                    MinPhysicalPath = minFileSavaPath,
                    FileSize = image.Length
                };
                var r = await _businessImage.AddOrEditAndSaveAsyn(articleImage);
                if (r)
                    return new ImageUploadVM
                    {
                        Code = 0,
                        Msg = "上传成功",
                        Data = new ImageUploadDto
                        {
                            Src = articleImage.RelativePath,
                            Title = currImageName
                        }
                    };
                else
                    return new ImageUploadVM
                    {
                        Code = 1,
                        Msg = "上传失败",
                        Data = null
                    };
            }
            catch (Exception)
            {
                return new ImageUploadVM
                {
                    Code = 1,
                    Msg = "上传失败",
                    Data = null
                };
            }
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns> 
        [HttpPost]
        public async Task<JsonResult> AddBlogArticle(AddBlogArticleInput input)
        {
            var categories = await _blogArticleCategory.GetAllAsyn(); //临时分类

            var blogArticle = new BlogArticle
            {
                Name = input.Name,
                Abstract = input.Abstract,
                Description = input.Description,
                User = TzUser,
                Category = categories.FirstOrDefault(), //临时分类
                Thumbnail = string.IsNullOrEmpty(input.Thumbnail) ? _randomDataHepler.GetRandomAvatar() : input.Thumbnail,
            };
            var r = await _blogArticle.AddOrEditAndSaveAsyn(blogArticle);
            if (r)
            {
                string[] labelNames = { "大大二颜", "情感", "私语" }; //临时标签
                for (int i = 0; i < 3; i++)
                {
                    await _blogArticleLabel.AddOrEditAndSaveAsyn(new BlogArticleLabel
                    {
                        BlogArticle = blogArticle,
                        Name = labelNames[i]
                    });
                }

                //初始化浏览统计        
                await _viewCount.AddOrEditAndSaveAsyn(new ViewCount
                {
                    ObjectId = blogArticle.Id,
                    Count = 0
                });

                //添加点赞
                await _blogArticlePraise.AddOrEditAndSaveAsyn(new BlogArticlePraise
                {
                    ArticleId = blogArticle.Id,
                    Up = 0,
                    Down = 0
                });

                return Json(new { state = true, message = "文章发布成功" });
            }
            else
                return Json(new { state = false, message = "文章发布失败。" });
        }

        #endregion
    }
}
