using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TzCA.Common.TzExtensions.ClientDtos;

namespace TzCA.Common.TzExtensions
{
    /// <summary>
    /// 获取客户端信息
    /// </summary>
    public class TzClientHelper : ITzClientHelper
    {
        #region 百度API AK密钥

        /// <summary>
        /// 开发版：AK密钥
        /// </summary>
        private const string THE_KEY_TEST = "Ivn5eP2GjDALCAmwWgd0atRcZuxCcqgw";

        /// <summary>
        /// 发布版：AK密钥
        /// </summary>
        private const string THE_KEY = "nNn7l5qnxQnClrjw6VGXMvkPdu8HtfFD";

        #endregion

        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public string IPAddress { get { return GetIPAddress(); } }

        /// <summary>
        /// 客户端浏览器信息
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// 百度：地理位置
        /// </summary>
        public string BaiduLocation { get { return GetBaiduLocation(); } }

        /// <summary>
        /// 淘宝：地理位置
        /// </summary>
        public string TaobaoLocation { get { return GetTaobaoLocation(); } }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        public TzClientHelper(
            IHttpContextAccessor httpContextAccessor
            )
        {
            this._httpContextAccessor = httpContextAccessor;
            this._httpContext = httpContextAccessor.HttpContext;
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        public string GetIPAddress()
        {
            try
            {
                var ipAddress = string.Empty;
                var httpContext = _httpContextAccessor.HttpContext ?? _httpContext;
                ipAddress = httpContext?.Connection?.RemoteIpAddress?.ToString();
                if (ipAddress != string.Empty)
                {
                    return ipAddress;
                }
                return ipAddress;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取客户端浏览器信息
        /// </summary>
        public string GetBrowser()
        {
            return string.Empty;
        }

        #region 405异常（弃用）

        ///// <summary>
        ///// 获取地理位置
        ///// </summary>
        ///// <returns></returns>
        //public string GetLocation(string ip = null)
        //{
        //    var currentIp = string.Empty;
        //    if (string.IsNullOrEmpty(ip))
        //        currentIp = IPAddress;
        //    else currentIp = ip;

        //    //返回格式：var lo="四川省", lc="成都市"; var localAddress={city:"成都市", province:"四川省"}
        //    string postUrl = "http://ip.ws.126.net/ipquery?ip=" + currentIp;
        //    var result = GetDataByPost(postUrl);

        //    return result;
        //}

        ///// <summary>
        ///// 渲染返回地址格式
        ///// </summary>
        ///// <returns></returns>
        //protected string RenderLocation(string location)
        //{
        //    //TODO:
        //    return string.Empty;
        //}

        ///// <summary>
        ///// Post请求数据
        ///// </summary>
        ///// <param name="url"></param>
        ///// <returns></returns>
        //public string GetDataByPost(string url)
        //{
        //    try
        //    {
        //        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        //        string s = "anything";
        //        byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(s);
        //        req.Method = "POST";
        //        req.ContentType = "application/x-www-form-urlencoded";
        //        req.ContentLength = requestBytes.Length;
        //        Stream requestStream = req.GetRequestStream();
        //        requestStream.Write(requestBytes, 0, requestBytes.Length);
        //        requestStream.Close();

        //        HttpWebResponse res = (HttpWebResponse)req.GetResponse();
        //        StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.Default);
        //        string backstr = sr.ReadToEnd();
        //        sr.Close();
        //        res.Close();
        //        return backstr;
        //    }
        //    catch (Exception e)
        //    {
        //        var ee = e.Message;
        //        throw;
        //    }
        //}

        #endregion

        #region 百度API获取地理位置

        /// <summary>
        /// 获取地理位置的详细信息
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public BaiduLocationDto GetBaiduLocationAll(string ip = null)
        {
            var currentIp = string.Empty;
            if (string.IsNullOrEmpty(ip))
                currentIp = IPAddress;
            else currentIp = ip;
            currentIp = IsIP(currentIp) ? currentIp : "127.0.0.1";
            var location = GetInfoByUrl(GetBaiduPostUrl(currentIp));
            location = UnescapeRegex(location);
            var locationDto = GetBaiduLocationDto(location);
            return locationDto;
        }

        /// <summary>
        /// 获取地理位置的详细信息
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public TaobaoLocationDto GetTaobaoLocationAll(string ip = null)
        {
            var currentIp = string.Empty;
            if (string.IsNullOrEmpty(ip))
                currentIp = IPAddress;
            else currentIp = ip;
            currentIp = IsIP(currentIp) ? currentIp : "127.0.0.1";
            var location = GetInfoByUrl(GetTaobaoPostUrl(currentIp));
            location = UnescapeRegex(location);
            var locationDto = GetTaobaoLocationDto(location);
            return locationDto;
        }

        /// <summary>
        /// 百度：获取地理位置（省份+城市）
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public string GetBaiduLocation(string ip = null)
        {
            var baiduLocation = GetBaiduLocationAll(ip);           
            var result = baiduLocation.Content.Address_detail.Province + "·" + baiduLocation.Content.Address_detail.City;
            return result;
        }

        /// <summary>
        /// 淘宝：获取地理位置（省份+城市）
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public string GetTaobaoLocation(string ip = null)
        {
            var taobaoLocation = GetTaobaoLocationAll(ip);
            var result = taobaoLocation.Data.Region + "·" + taobaoLocation.Data.City;
            return result;
        }

        /// <summary>
        /// 转换下字符串中的转义字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string UnescapeRegex(string str)
        {
            var result = System.Text.RegularExpressions.Regex.Unescape(str);
            return result;
        }

        /// <summary>
        /// 百度： 转地理位置对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private BaiduLocationDto GetBaiduLocationDto(string json)
        {
            return JsonHelper.DeserializeJsonToObject<BaiduLocationDto>(json);
        }

        /// <summary>
        /// 淘宝： 转地理位置对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private TaobaoLocationDto GetTaobaoLocationDto(string json)
        {
            return JsonHelper.DeserializeJsonToObject<TaobaoLocationDto>(json);
        }

        /// <summary>
        /// 百度：返回UTF-8编码服务地址
        /// </summary>
        /// <returns>服务地址</returns>
        private string GetBaiduPostUrl(string ip = null)
        {
            string postUrl = "http://api.map.baidu.com/location/ip?ak=" + THE_KEY_TEST + "&ip=" + ip + "&coor=bd09ll";
            return postUrl;
        }

        /// <summary>
        /// 淘宝：返回UTF-8编码服务地址
        /// </summary>
        /// <returns>服务地址</returns>
        private string GetTaobaoPostUrl(string ip = null)
        {
            string postUrl = "http://ip.taobao.com/service/getIpInfo.php?ip=" + ip;
            return postUrl;
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// 返回结果（地址解析的结果） 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetInfoByUrl(string url)
        {
            //调用时只需要把拼成的URL传给该函数即可。判断返回值即可
            string strRet = null;

            if (url == null || url.Trim().ToString() == "")
            {
                return strRet;
            }
            string targeturl = url.Trim().ToString();
            try
            {
                HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targeturl);
                hr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                hr.Method = "GET";
                hr.Timeout = 30 * 60 * 1000;
                WebResponse hs = hr.GetResponse();
                Stream sr = hs.GetResponseStream();
                StreamReader ser = new StreamReader(sr, Encoding.Default);
                strRet = ser.ReadToEnd();
            }
            catch (Exception ex)
            {
                var e = ex.Message;
                strRet = null;
            }
            return strRet;
        }

        #endregion
    }
}
