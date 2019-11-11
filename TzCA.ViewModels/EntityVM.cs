using TzCA.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TzCA.ViewModels
{
    /// <summary>
    /// 完全继承自 IEntity 的实体类共用的视图模型
    /// </summary>
    public class EntityVM
    {
        public Guid Id { get; set; }

        /// <summary>
        ///列表时候需要的序号
        /// </summary>
        public int SortCode { get; set; }
        public bool IsNew { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [Display(Name = "角色编码")]
        [StringLength(150, ErrorMessage = "你输入的数据超出限制150个字符的长度。")]
        public string BussinessCode { get; set; }

        /// <summary>
        /// 创建的时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 更新的时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        public EntityVM() { }

        public void SetVM<T>(T bo) where T : class, IEntity, new()
        {
            Id = bo.Id;
            Name = bo.Name;
            Description = bo.Description;
            BussinessCode = bo.BussinessCode;
            CreateTime = bo.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"); //转成固定格式的时间
            UpdateTime = bo.UpdateTime;
        }

        public void MapToBo<T>(T bo) where T : class, IEntity, new()
        {
            bo.Name = this.Name;
            bo.Description = this.Description;
            bo.BussinessCode = this.BussinessCode;
        }
    }
}
