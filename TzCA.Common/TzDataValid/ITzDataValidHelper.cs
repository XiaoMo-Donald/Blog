using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.Common.TzDataValid
{
    /// <summary>
    /// 数据校验帮助接口
    /// </summary>
   public interface ITzDataValidHelper
    {
        /// <summary>
        /// 判断输入的字符串是否只包含数字和英文字母
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool IsNumAndEnCh(string input);

        /// <summary>
        /// 检查系统保留的登录用户名称
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool CheckReservedUserName(string username);
    }
}
