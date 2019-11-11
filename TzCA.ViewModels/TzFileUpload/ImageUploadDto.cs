using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.ViewModels.TzFileUpload
{
    /// <summary>
    /// 图片回调传输对象
    /// </summary>
    public class ImageUploadDto
    {
        /// <summary>
        /// 回调的图片路径
        /// </summary>
        public string Src { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        public string Title { get; set; }
    }
}
