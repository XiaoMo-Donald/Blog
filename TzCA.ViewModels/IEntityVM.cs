﻿using TzCA.Common.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TzCA.ViewModels
{
    /// <summary>
    /// 业务实体视图模型统一的接口规范，用于规约所有的视图模型的基础属性
    /// </summary>
    public interface IEntityVM
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }

       /// <summary>
       /// 业务唯一编码
       /// </summary>
        string BussinessCode { get; set; }        

        /// <summary>
        /// 列表时候需要的序号（排序码）
        /// </summary>
        int SortCode { get; set; }

        /// <summary>
        /// 是否是新创建的对象，要特别注意在实际使用时候的赋值
        /// </summary>
        bool IsNew { get; set; }

        ///// <summary>
        ///// 页面参数规格
        ///// </summary>
        //ListPageParameter ListPageParameter { get; set; } 
    }
}
