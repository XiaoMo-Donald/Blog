using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace TzCA.Common.PictureCompression
{
    /// <summary>
    /// 图片压缩处理
    /// </summary>
    public class TzPictureCompressionHelper : ITzPictureCompressionHelper
    {
        private IHostingEnvironment _hostingEnv;
        public TzPictureCompressionHelper(IHostingEnvironment hostingEnv)
        {
            this._hostingEnv = hostingEnv;
        }

        /// <summary>
        /// 根据指定尺寸得到按比例缩放的尺寸,返回true表示以更改尺寸
        /// </summary>
        /// <param name="picWidth">图片宽度</param>
        /// <param name="picHeight">图片高度</param>
        /// <param name="specifiedWidth">指定宽度</param>
        /// /// <param name="specifiedHeight">指定高度</param>
        /// <returns>返回true表示以更改尺寸</returns>
        private bool GetPicZoomSize(ref int picWidth, ref int picHeight, int specifiedWidth, int specifiedHeight)
        {
            int sW = 0, sH = 0;
            Boolean isZoomSize = false;
            //按比例缩放
            Size tem_size = new Size(picWidth, picHeight);
            if (tem_size.Width > specifiedWidth || tem_size.Height > specifiedHeight)
            {
                if ((tem_size.Width * specifiedHeight) > (tem_size.Height * specifiedWidth))
                {
                    sW = specifiedWidth;
                    sH = (specifiedWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = specifiedHeight;
                    sW = (tem_size.Width * specifiedHeight) / tem_size.Height;
                }
                isZoomSize = true;
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }
            picHeight = sH;
            picWidth = sW;
            return isZoomSize;
        }

        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片路径</param>
        /// <param name="dFile">压缩后保存位置</param>
        /// <param name="dHeight">高度（百分比0-1）</param>
        /// <param name="dWidth">宽度(百分比 0-1)</param>
        /// <param name="flag">压缩质量 1-100</param>
        /// <returns>压缩成功后的相对路径minRelativePath</returns>
        public string GetPicThumbnail(string sFile, string dFile, double dHeight, double dWidth, int flag)
        {
            Image iSource = Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = iSource.Width, sH = iSource.Height;

            int percentDHeight = Int32.Parse(Math.Round(dHeight.Equals(0) || dHeight.Equals(1) ? sH : sH * (0 > dHeight && dHeight > 1 ? 1 : dHeight), 0).ToString());
            int percentDWidth = Int32.Parse(Math.Round(dWidth.Equals(0) || dWidth.Equals(1) ? sW : sW * (0 > dWidth && dWidth > 1 ? 1 : dWidth), 0).ToString());
            percentDHeight = percentDHeight < 100 ? 100 : percentDHeight;
            percentDWidth = percentDWidth < 100 ? 100 : percentDWidth;
            GetPicZoomSize(ref sW, ref sH, percentDWidth, percentDHeight);
            Bitmap ob = new Bitmap(percentDWidth, percentDHeight);
            Graphics g = Graphics.FromImage(ob);
            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(iSource, new Rectangle((percentDWidth - sW) / 2, (percentDHeight - sH) / 2, sW, sH), 1, 1, iSource.Width - 1, iSource.Height - 1, GraphicsUnit.Pixel);
            g.Dispose();
            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null) ob.Save(dFile, jpegICIinfo, ep);
                else ob.Save(dFile, tFormat);

                //处理压缩成功后的相对路径
                int webRootPathLength = _hostingEnv.WebRootPath.Length;
                string dFileSub = dFile.Substring(webRootPathLength, dFile.Length - webRootPathLength);
                return Path.Combine("../../", dFileSub.Replace("\\", "/"));
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }
    }
}
