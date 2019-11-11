using TzCA.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TzCA.Entities.Attachments
{
    public class BusinessVideo:EntityBase,IEntity
    {      
        [StringLength(500)]
        public virtual string OriginalFileName { get; set; }

        /// <summary>
        /// 物理路径
        /// </summary>
        public virtual string PhysicalPath { get; set; }

        /// <summary>
        /// 相对路径
        /// </summary>
        public virtual string RelativePath { get; set; }
        public virtual bool IsInDB { get; set; }                              
        [StringLength(10)]
        public virtual  string FileSuffix { get; set; }
        public virtual byte[] BinaryContent { get; set; }
        public virtual long FileSize { get; set; }                            
        [StringLength(120)]
        public virtual  string Icon { get; set; }

        public virtual  Guid RelevanceObjectId { get; set; }
        public virtual Guid UploaderId { get; set; }                           // 关联上传人Id
        
        public BusinessVideo() 
        {           
            this.BussinessCode = BusinessEntityComponentsFactory.SortCodeByDefaultDateTime<BusinessVideo>();
        }
    }
}
