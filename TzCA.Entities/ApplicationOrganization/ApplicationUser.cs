using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace TzCA.Entities.ApplicationOrganization
{
    /// <summary>
    /// 系统用户定义，这是直接继承 IdentityUser 实现的
    /// </summary>
    public class ApplicationUser : IdentityUser 
    {
        /// <summary>
        /// 姓
        /// </summary>
        [StringLength(100)]
        public string FirstName { get; set; }

        /// <summary>
        /// 名
        /// </summary>
        [StringLength(100)]
        public string LastName { get; set; }

        /// <summary>
        /// 中文全名
        /// </summary>
        [StringLength(100)]

        public string FullName { get; set; }
  
        /// <summary>
        /// 手机号码
        /// </summary>
        [StringLength(50)]
        public string MobileNumber { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string Nickname { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public virtual DateTime Birthday { get; set; }

        /// <summary>
        /// 地理位置
        /// </summary>
        public virtual string Location { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime UpdateTime { get; set; }

        public ApplicationUser():base()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreateTime = DateTime.Now;
        }
        public ApplicationUser(string userName) : base(userName)
        {
            this.Id = Guid.NewGuid().ToString();
            this.UserName = userName;
        }
    }
}
