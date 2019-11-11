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
using TzCA.DataAccess;
using TzCA.Entities.ApplicationOrganization;
using TzCA.Entities.Attachments;
using TzCA.Entities.SiteManagement;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TzCA.Web.Controllers
{
    /// <summary>
    /// 文件控制器
    /// </summary>
    public class TzFileController : TzControllerBase
    {
        private IHostingEnvironment _hostingEnv;

        public TzFileController(
               IHostingEnvironment hostingEnv,
               IEntityRepository<BusinessImage> businessImage,
               UserManager<ApplicationUser> userManager,
               IEntityRepository<TzSiteLog> tzSiteLog
            ) : base(userManager, businessImage,tzSiteLog)
        {
            this._hostingEnv = hostingEnv;
        }


        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 图片统一上传（单张上传）
        /// </summary>
        /// <param name="id">使用对象Id</param>
        /// <param name="iEnum">图片类型</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        protected async Task<string> ImageUpload(Guid id, BusinessImageEnum iEnum)
        {
            try
            {
                var image = Request.Form.Files.First();
                if (image == null)
                {
                    return string.Empty;
                }
                var currImageName = image.FileName;
                var timeForFile = (DateTime.Now.ToString("yyyyMMddHHmmss") + "-").Trim();
                string extensionName = currImageName.Substring(currImageName.LastIndexOf("."));
                var imageName = ContentDispositionHeaderValue
                                .Parse(image.ContentDisposition)
                                .FileName
                                .Trim('"')
                                .Substring(image.FileName.LastIndexOf("\\") + 1);
                var newImageName = timeForFile + id + extensionName;
                var boPath = "../../images/UploadImages/" + iEnum.ToString() + "/" + DateTime.Now.ToString("yyyy-MM-dd") + "/" + newImageName;
                var imagePath = _hostingEnv.WebRootPath + $@"\images\UploadImages\" + iEnum.ToString() + "\\" + DateTime.Now.ToString("yyyy-MM-dd");
                imageName = _hostingEnv.WebRootPath + $@"\images\UploadImages\" + iEnum.ToString() + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\" + newImageName;

                Directory.CreateDirectory(imagePath); //创建目录
                using (FileStream fs = System.IO.File.Create(imageName))
                {
                    image.CopyTo(fs);
                    fs.Flush();
                }
                var avatar = _businessImage.GetAll().FirstOrDefault(a => a.RelevanceObjectId.Equals(id));

                if (!string.IsNullOrEmpty(avatar.PhysicalPath))
                {
                    System.IO.File.Delete(avatar.PhysicalPath);
                }
                avatar.UploaderId = id;
                avatar.UpdateTime = DateTime.Now;
                avatar.RelativePath = boPath;
                avatar.PhysicalPath = imageName;
                avatar.FileSize = image.Length;
                await _businessImage.AddOrEditAndSaveAsyn(avatar);
                return avatar.RelativePath;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
