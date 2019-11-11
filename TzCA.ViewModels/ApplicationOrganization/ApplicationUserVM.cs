using TzCA.Common.JsonModels;
using TzCA.Common.ViewModelComponents;
using TzCA.Entities.ApplicationOrganization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TzCA.Entities.Attachments;

namespace TzCA.ViewModels.ApplicationOrganization
{
    public class ApplicationUserVM : IEntityVM
    {
        public Guid Id { get; set; }
        public int SortCode { get; set; }
        public bool IsNew { get; set; }
       
        public string Name { get; set; }
      
        public string Description { get; set; }
        public string BussinessCode { get; set; }
       
        public string Password { get; set; }
        
        public string ConfirmPassword { get; set; }

        [Display(Name = "归属用户组")]
        public List<string> RoleItemIdCollection { get; set; }

        [Display(Name = "归属用户组")]
        public string RoleItemNameString { get; set; }

        [PlainFacadeItemSpecification("RoleItemIdCollection")]
        public List<PlainFacadeItem> RoleItemColection { get; set; }
   
        public string UserName { get; set; }

        [Display(Name = "归属部门")]
        public string DepartmentName { get; set; }

        [Display(Name = "手机")]
        [RegularExpression(@"^((13[0-9])|(14[5|7])|(15([0-3]|[5-9]))|(18[0,5-9]))\d{8}$", ErrorMessage = "非法的移动电话格式。")]
        public string MobileNumber { get; set; }
    
        [Display(Name = "电子邮件")]
        [StringLength(100, ErrorMessage = "你输入的数据超出限制100个字符的长度。")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "非法的电子邮件格式。")]
        public string EMail { get; set; }

        [Display(Name = "关联人员")]
        public string PersonId { get; set; }

        [Display(Name = "关联人员")]
        public string PersonName { get; set; }

        [PlainFacadeItemSpecification("PersonId")]
        public List<PlainFacadeItem> PersonItemCollection { get; set; }

        public bool LockoutEnabled { get; set; }//用户被禁用状态
        public Guid RoleId { get; set; }         // 角色Id

        /// <summary>
        /// 用户头像（相对路径）
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 用户头像Min（相对路径）
        /// </summary>
        public string MinAvatar { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public BusinessImage UserAvatar { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 用户个性签名
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 地理位置
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 用户在线状态
        /// </summary>
        public bool OnlineState { get; set; }

        public ApplicationUserVM(){ }

        public ApplicationUserVM(ApplicationUser bo)
        {
            this.Id = Guid.Parse(bo.Id);
            this.UserName = bo.UserName;
            this.MobileNumber = bo.MobileNumber;
            this.EMail = bo.Email;
            this.Name = bo.FullName;
            this.LockoutEnabled = bo.LockoutEnabled;
            this.Remark = bo.Remark;
            this.Nickname = bo.Nickname;
            this.Birthday = bo.Birthday;
            this.Location = bo.Location;
        }

    }
}
