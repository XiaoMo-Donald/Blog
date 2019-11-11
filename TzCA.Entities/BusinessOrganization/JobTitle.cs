using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TzCA.Entities.BusinessOrganization
{
    /// <summary>
    /// 工作职位
    /// </summary>
    public class JobTitle :EntityBase, IEntity
    {
        [StringLength(200)]
        public string Name { get; set; }         // 工作职位名称
        [StringLength(500)]
        public string Description { get; set; }  // 工作职位要求说明
        [StringLength(150)]
        public string BussinessCode { get; set; }     // 业务编码

        public JobTitle()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
