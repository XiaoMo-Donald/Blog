using TzCA.Common.JsonModels;
using TzCA.Entities.ApplicationOrganization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TzCA.ViewModels.ApplicationOrganization
{
    public class ApplicationRoleVM:IEntityVM
    {
        public Guid Id { get; set; }
        public int SortCode { get; set; } // 列表时候需要的序号
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        [Display(Name = "名称")]
        [StringLength(100, ErrorMessage = "你输入的数据超出限制100个字符的长度。")]
        public string Name { get; set; }

        [Display(Name = "简要说明")]
        [StringLength(1000, ErrorMessage = "你输入的数据超出限制1000个字符的长度。")]
        public string Description { get; set; }

        [Display(Name = "角色编码")]
        [StringLength(150, ErrorMessage = "你输入的数据超出限制150个字符的长度。")]
        public string BussinessCode { get; set; }

        [Display(Name = "显示名称")]
        [StringLength(100, ErrorMessage = "你输入的数据超出限制100个字符的长度。")]
        public string DisplayName { get; set; }

        public int UserAmount { get; set; }

        public ApplicationRoleVM() { }

        public ApplicationRoleVM(ApplicationRole bo)
        {
            this.Id = Guid.Parse(bo.Id);
            this.Name = bo.Name;
            this.DisplayName = bo.DisplayName;
            this.Description = bo.Description;
            this.BussinessCode = bo.SortCode;
        }

        public void MapToBo(ApplicationRole bo)
        {
            bo.Name = this.Name;
            bo.DisplayName = this.DisplayName;
            bo.Description = this.Description;
            bo.SortCode = this.BussinessCode;
        }
    }
}
