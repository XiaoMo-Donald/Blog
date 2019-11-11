using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.ViewModels
{
    /// <summary>
    /// 定义的网站统一回调格式输出
    /// </summary>
    public class TzDataOutputDto<T>
    {
        /// <summary>
        /// 操作执行状态
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        /// 状态码 0成功 1失败
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 回调的消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 回调的数据
        /// </summary>
        public T Data { get; set; }
    }
}
