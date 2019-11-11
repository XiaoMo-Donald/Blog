using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.Common.JsonModels
{
    /// <summary>
    /// 基本的注册信息
    /// </summary>
    public class RegisterInformation
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        public string ConfirmPassword { get; set; }
        
        /// <summary>
        /// 校验码
        /// </summary>
        public string VerificationCode { get; set; }

        /// <summary>
        /// 网站服务条款状态（已读、未读）
        /// </summary>
        public string TermsOfUse { get; set; }
      
    }
}
