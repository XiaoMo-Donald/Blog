using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.Common.TzExtensions
{
    /// <summary>
    /// 文件夹实现类
    /// </summary>
    public class TzFolderHelper : ITzFolderHelper
    {
        public string FolderDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

    }
}
