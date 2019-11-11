using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.Common.TzExtensions.ClientDtos
{
    /// <summary>
    /// 百度地理位置API 数据对象
    /// </summary>
    public class BaiduLocationDto
    {
        /// <summary>
        /// 综合地址信息
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 地址内容
        /// </summary>
        public Content Content { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public int Status { get; set; }
    }

    public class Content
    {
        /// <summary>
        /// 综合地址信息
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 详细信息
        /// </summary>
        public TzLocationDetail Address_detail { get; set; }

        /// <summary>
        /// 坐标
        /// </summary>
        public Point Point { get; set; }
    }

    /// <summary>
    /// 百度API地理位置content部分
    /// </summary>
    public class TzLocationDetail
    {
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 城市代码
        /// </summary>
        public int City_code { get; set; }

        /// <summary>
        /// 地区
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 街道
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// 街道号码
        /// </summary>
        public string Street_number { get; set; }

    }

    /// <summary>
    /// 坐标
    /// </summary>
    public class Point
    {
        public string X { get; set; }
        public string Y { get; set; }
    }
}
