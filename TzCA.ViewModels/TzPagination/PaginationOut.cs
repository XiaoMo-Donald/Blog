using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.ViewModels.TzPagination
{
    /// <summary>
    /// 分页输出对象
    /// </summary>
    /// <typeparam name="T">数据实体</typeparam>
    public class PaginationOut<T>
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public int Count { get; set; }
        public T Data { get; set; }

        public PaginationOut() {}
    }

}
