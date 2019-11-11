using TzCA.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TzCA.Entities.Attachments
{
    public class BusinessFile:EntityBase,IEntity
    {
        /// <summary>
        /// 附件原始文件名称
        /// </summary>
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

        /// <summary>
        /// 附件存放格式，如果使用二进制方式存在数据库中，则使用下一个属性进行处理
        /// </summary>
        public virtual bool IsInDB { get; set; }

        /// <summary>
        /// 上传文件的后缀名
        /// </summary>
        [StringLength(10)]
        public virtual string FileSuffix { get; set; }

        /// <summary>
        /// 附件存放的二进制内容
        /// </summary>
        public virtual byte[] BinaryContent { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public virtual long FileSize { get; set; }

        /// <summary>
        /// 文件物理格式图标
        /// </summary>
        [StringLength(120)]
        public virtual string Icon { get; set; }

        /// <summary>
        /// 关联对象Id
        /// </summary>
        public virtual Guid RelevanceObjectId { get; set; }

        /// <summary>
        /// 关联上传人Id
        /// </summary>
        public virtual Guid UploaderId { get; set; }

        public BusinessFile() 
        {            
            this.BussinessCode = BusinessEntityComponentsFactory.SortCodeByDefaultDateTime<BusinessFile>();
        }
    }
}
