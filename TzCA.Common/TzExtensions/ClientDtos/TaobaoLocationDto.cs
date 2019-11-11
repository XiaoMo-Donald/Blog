using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.Common.TzExtensions.ClientDtos
{
    /// <summary>
    /// 淘宝地理位置API 数据对象
    /// </summary>
    public class TaobaoLocationDto
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 详细数据
        /// </summary>
        public TaobaoLocationDetail Data { get; set; }

    }

    public class TaobaoLocationDetail
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }
    
        public string Area { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        public string County { get; set; }

        public string Isp { get; set; }
 
        public string Country_id { get; set; }
   
        public string Area_id { get; set; }
       
        public string Region_id { get; set; }
    
        public string City_id { get; set; }
    
        public string County_id { get; set; }
    
        public string Isp_id { get; set; }

    }
}
