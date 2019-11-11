﻿using TzCA.Entities.Attachments;
using TzCA.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TzCA.Entities.BusinessOrganization
{
    /// <summary>
    /// 系统业务人员
    /// </summary>
    public class Person:EntityBase,IEntity
    { 
        public DateTime ExpiredDateTime { get; set; }
        public int Duration { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }                            // 业务工号
        [StringLength(50)]
        public string FirstName { get; set; }                               // 姓氏
        [StringLength(50)]
        public string LastName { get; set; }                                // 名字
        public bool Sex { get; set; }                                       // 性别
        [StringLength(20)]
        public string TelephoneNumber { get; set; }                     // 电话号码
        [StringLength(20)]
        public string Mobile { get; set; }                            // 手机号码
        [StringLength(100)]
        public string Email { get; set; }                                   // 电子邮箱
        public DateTime Birthday { get; set; }                              // 出生日期
        [StringLength(26)]
        public string CredentialsCode { get; set; }                         // 身份证编号
        [StringLength(50)]
        public string InquiryPassword { get; set; }                         // 查询密码，仅仅用于查询是否已经已经建立数据

        public virtual Department Department { get; set; }                  // 所属部门
        public virtual BusinessImage Avatar { get; set; }
        public virtual JobTitle JobTitle { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string Nickname { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        public virtual string Remark { get; set; }



        public Person()
        {
            Id = Guid.NewGuid();
            UpdateTime = CreateTime = Birthday = ExpiredDateTime = DateTime.Now;
            BussinessCode = BusinessEntityComponentsFactory.SortCodeByDefaultDateTime<Person>();
        }

    }
}
