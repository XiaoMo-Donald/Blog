using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.Common.PictureCompression
{
    /// <summary>
    /// 图片处理接口
    /// </summary>
    public interface ITzPictureCompressionHelper
    {
        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片路径</param>
        /// <param name="dFile">压缩后保存位置</param>
        /// <param name="dHeight">高度(0 使用原本的尺寸)</param>
        /// <param name="dWidth">宽度(0 使用原本的尺寸)</param>
        /// <param name="flag">压缩质量 1-100</param>
        /// <returns>压缩成功后的相对路径minRelativePath</returns>
        string GetPicThumbnail(string sFile, string dFile, double dHeight, double dWidth, int flag);
    }
}
