using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TzCA.Common.JsonModels;
using TzCA.Common.ViewModelComponents;
using TzCA.DataAccess.Utilities;
using TzCA.Entities.BusinessOrganization;

namespace TzCA.ViewModels.BusinessOrganization
{
    public class DepartmentVM : IEntityVM
    {
        public Guid Id { get; set; }
        public int SortCode { get; set; }
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        [Required(ErrorMessage = "名称不能为空值。")]
        [Display(Name = "部门名称")]
        [StringLength(100, ErrorMessage = "你输入的数据超出限制100个字符的长度。")]
        public string Name { get; set; }

        [Display(Name = "简要说明")]
        [StringLength(1000, ErrorMessage = "你输入的数据超出限制1000个字符的长度。")]
        public string Description { get; set; }

        [Display(Name = "业务编码")]
        [Required(ErrorMessage = "类型业务编码不能为空值。")]
        [StringLength(150, ErrorMessage = "你输入的数据超出限制150个字符的长度。")]
        public string BussinessCode { get; set; }

        [Display(Name = "上级部门")]
        public string ParentItemId { get; set; } // 这个用于编辑、新建时候选择项的绑定属性
        public List<SelfReferentialItem> ParentItemCollection { get; set; } // 这个用于提供给前端实现时所需要的下拉或者列表选择的时候的元素

        [Display(Name = "上级部门")]
        public SelfReferentialItem ParentItem { get; set; } // 这个用于在明细或者列表时候呈现关联信息

        [Display(Name = "是否活动")]
        public bool IsActiveDepartment { get; set; }

        public string IsActiveDepartmentString { get; set; }
        public List<PlainFacadeItem> IsActiveDepartmentSelector { get; set; }

        [Display(Name = "部门人员")]
        public string PersonIds { get; set; }

        [Display(Name = "部门人员")]
        public Person Persons { get; set; }

        [Display(Name = "部门人员")]
        public ICollection<Person> PersonItems { get; set; }



        public DepartmentVM()
        {
            IsActiveDepartmentSelector = PlainFacadeItemFactory<Department>.GetByBool();
        }

        public DepartmentVM(Department bo)
        {
            Id = bo.Id;
            Name = bo.Name;
            Description = bo.Description;
            BussinessCode = bo.BussinessCode;
            IsActiveDepartment = bo.IsActiveDepartment;
            if (bo.IsActiveDepartment)
            {
                IsActiveDepartmentString = "是";
            }
            else
            {
                IsActiveDepartmentString = "否";
            }

            if (bo.ParentDepartment != null)
            {
                ParentItemId = bo.ParentDepartment.Id.ToString();
                ParentItem = SelfReferentialItemFactory<Department>.Get(bo);
            }

            IsActiveDepartmentSelector = PlainFacadeItemFactory<Department>.GetByBool(bo.IsActiveDepartment);
        }

        public void MapToBo(Department bo, Department parentBo)
        {
            bo.Name = Name;
            bo.Description = Description;
            bo.BussinessCode = BussinessCode;
            bo.IsActiveDepartment = IsActiveDepartment;
            bo.ParentDepartment = parentBo;
        }
    }
}