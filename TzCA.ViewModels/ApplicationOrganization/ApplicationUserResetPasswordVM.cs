using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TzCA.ViewModels.ApplicationOrganization
{
    public class ApplicationUserResetPasswordVM
    {
        [Required]
        public string Id { get; set; }   // 用户Id

        [Required(ErrorMessage = "必须给出密码。")]
        [Display(Name = "密码")]
        [StringLength(150, ErrorMessage = "你输入的数据超出限制150个字符的长度。",MinimumLength =6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "必须给出重复密码。")]
        [Display(Name = "密码")]
        [Compare("Password", ErrorMessage = "密码和重复密码不匹配。")]
        [StringLength(150, ErrorMessage = "你输入的数据超出限制150个字符的长度。",MinimumLength =6)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
}
