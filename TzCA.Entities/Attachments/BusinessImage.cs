using TzCA.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TzCA.Entities.Attachments
{
    /// <summary>
    /// 图片附件
    /// </summary>
    public class BusinessImage : EntityBase, IEntity
    {
        /// <summary>
        /// 图片显示名称
        /// </summary>
        [StringLength(100)]
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// 图片片原始文件
        /// </summary>
        [StringLength(256)]
        public virtual string OriginalFileName { get; set; }

        /// <summary>
        /// 物理路径
        /// </summary>
        public virtual string PhysicalPath { get; set; }

        /// <summary>
        /// 压缩后的物理路径
        /// </summary>
        public virtual string MinPhysicalPath { get; set; }

        /// <summary>
        /// 相对路径
        /// </summary>
        public virtual string RelativePath { get; set; }

        /// <summary>
        /// 压缩后的相对路径
        /// </summary>
        public virtual string MinRelativePath { get; set; }

        /// <summary>
        /// 图片扩展名
        /// </summary>
        [StringLength(256)]
        public virtual string FileSuffix { get; set; }

        /// <summary>
        /// 图片大小
        /// </summary>
        public virtual long FileSize { get; set; }

        /// <summary>
        /// 图片物理文件图标
        /// </summary>
        [StringLength(120)]
        public virtual string Icon { get; set; }

        /// <summary>
        /// 使用该图片的业务对象的 Id
        /// </summary>
        public virtual Guid RelevanceObjectId { get; set; }

        /// <summary>
        /// 图片类型
        /// </summary>
        public virtual BusinessImageEnum Type { get; set; }

        /// <summary>
        /// 关联上传人Id
        /// </summary>
        public virtual Guid UploaderId { get; set; }

        public virtual int X { get; set; }
        public virtual int Y { get; set; }
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
          

        public BusinessImage()
        {
            this.BussinessCode = BusinessEntityComponentsFactory.SortCodeByDefaultDateTime<BusinessImage>();
        }
    }
}
