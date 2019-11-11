using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TzCA.Common.TzDataValid
{
    /// <summary>
    /// 数据校验帮助
    /// </summary>
    public class TzDataValidHelper : ITzDataValidHelper
    {
        /// <summary>
        /// 判断输入的字符串是否只包含数字和英文字母
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsNumAndEnCh(string input)
        {
            string pattern = @"^[A-Za-z0-9]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }


        /// <summary>
        /// 检查系统保留的登录用户名称
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool CheckReservedUserName(string username)
        {
            var reserveds = new List<string>
            {
                "admin","xiaomo","tzxiaomo","moguangyuan","yunmeng","yunmengwangluo",
                "administrator","administrators","eryan","xijinpin","cloud","tzalliance",
                "tzcloud","tzcloudalliance","cloudalliance"
            };
            var has = reserveds.SingleOrDefault(x => x.Equals(username.ToLower()));
            return string.IsNullOrEmpty(has) ? false : true;
        }
    }
}
