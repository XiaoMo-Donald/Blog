using TzCA.Entities.BusinessOrganization;
using TzCA.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TzCA.Entities.BusinessManagement.Audit
{
    /// <summary>
    /// 审核操作的记录
    /// </summary>
    public class AuditRecord:EntityBase,IEntity
    {
        [StringLength(50)]
        public string Name { get; set; }                 // 审核名称，由调用这个审核的实体类命名
        [StringLength(1000)]
        public string Description { get; set; }          // 审核意见
        [StringLength(150)]
        public string BussinessCode { get; set; }

        public Guid ObjectId { get; set; }               // 关联的业务对象的Id
        public AuditResultEnum AuditResult { get; set; } // 审核结果
        public DateTime AuditDateTime { get; set; }      // 审核时间
        public virtual Person Auditor { get; set; }      // 审核人
        
        public AuditRecord()
        {
            Id = Guid.NewGuid();
            BussinessCode = BusinessEntityComponentsFactory.SortCodeByDefaultDateTime<AuditRecord>();
            AuditDateTime = DateTime.Now;
        }
    }
}
