using System;
using System.Collections.Generic;
using System.Text;
using TzCA.Entities.Attachments;

namespace TzCA.ViewModels.TzDataVM
{
    /// <summary>
    /// 用户头像（随机头像）视图模型
    /// </summary>
    public class UserAvaterVM
    {
        /// <summary>
        /// 头像路径
        /// </summary>
        string Path { get; set; }

        public UserAvaterVM() { }

        public UserAvaterVM(BusinessImage bo)
        {
            this.Path = bo.RelativePath;
        }
        public UserAvaterVM(string path)
        {
            this.Path = path;
        }
    }
}
