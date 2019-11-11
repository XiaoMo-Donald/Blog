using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.ViewModels.TzPagination
{
    /// <summary>
    /// 分页输入参数
    /// </summary>
    public class PaginationInput : IPaginationInput
    {
        /// <summary>
        /// 当前索引页（非table）
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 当前索引页（table）
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// 跳页数量
        /// </summary>
        public int SkipCount
        {
            get
            {
                if (this.Index == 0)
                    return this.Page <= 1 ? 0 : (this.Page - 1) * this.Limit;
                else
                    return this.Index <= 1 ? 0 : (this.Index - 1) * this.Limit;                   
            }
        }

        public PaginationInput() { }

    }
}
