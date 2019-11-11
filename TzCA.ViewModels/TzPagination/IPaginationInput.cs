using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.ViewModels.TzPagination
{
    /// <summary>
    /// 分页参数规约
    /// </summary>
    public interface IPaginationInput
    {
        /// <summary>
        /// 当前索引页
        /// </summary>
        int Index { get; set; }

        /// <summary>
        /// 每页最大值
        /// </summary>
        int Limit { get; set; }
    }
}
