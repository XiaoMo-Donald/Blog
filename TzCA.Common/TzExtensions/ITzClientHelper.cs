using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Common.TzExtensions.ClientDtos;

namespace TzCA.Common.TzExtensions
{
    /// <summary>
    /// 获取客户端信息接口
    /// </summary>
    public interface ITzClientHelper : ITzApplicationBase
    {
        /// <summary>
        /// 客户端IP地址
        /// </summary>
        string IPAddress { get; }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        string GetIPAddress();

        /// <summary>
        /// 客户端浏览器信息
        /// </summary>
        string Browser { get; }

        /// <summary>
        /// 获取客户端浏览器信息
        /// </summary>
        string GetBrowser();

        /// <summary>
        /// 百度：地理位置（省份+城市）
        /// </summary>
        string BaiduLocation { get; }
        
        /// <summary>
        /// 淘宝：地理位置（省份+城市）
        /// </summary>
        string TaobaoLocation { get; }

        /// <summary>
        /// 百度：获取地理位置（省份+城市）
        /// </summary>
        string GetBaiduLocation(string ip);

        /// <summary>
        /// 淘宝：获取地理位置（省份+城市）
        /// </summary>
        string GetTaobaoLocation(string ip);

        /// <summary>
        /// 获取地理位置的详细信息（百度API）
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        BaiduLocationDto GetBaiduLocationAll(string ip);

        /// <summary>
        /// 获取地理位置详细信息（淘宝API）
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        TaobaoLocationDto GetTaobaoLocationAll(string ip);
    }
}
