using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.ViewModels.TzFileUpload
{
    /// <summary>
    /// 图片上传回调视图模型
    /// </summary>
    public class ImageUploadVM
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 提示信息 一般上传失败后返回
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 上传的图片回调数据
        /// </summary>
        public ImageUploadDto Data { get; set; }

        public ImageUploadVM()
        {
            this.Code = 0;
            this.Data = new ImageUploadDto();
        }

        public ImageUploadVM(ImageUploadDto bo)
        {
            this.Code = 0;
            this.Data = new ImageUploadDto();
            this.Data = bo;
        }

    }
}
