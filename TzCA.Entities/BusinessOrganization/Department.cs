using TzCA.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TzCA.Entities.BusinessOrganization
{
    public class Department:EntityBase,IEntity
    {
        public bool IsActiveDepartment { get; set; }

        [ForeignKey("ParentDepartmentId")]
        public virtual Department ParentDepartment { get; set; }  // 上级部门，一般情况下，类似这样的

        public Department()
        {
            this.Id = Guid.NewGuid();
            this.IsActiveDepartment = true;
            this.BussinessCode = BusinessEntityComponentsFactory.SortCodeByDefaultDateTime<Department>();
        }
    }
}
