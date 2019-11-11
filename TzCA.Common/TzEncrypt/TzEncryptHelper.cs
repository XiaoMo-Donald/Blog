using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.Common.TzEncrypt
{
    /// <summary>
    /// 加密帮助类
    /// </summary>
    public class TzEncryptHelper
    {
        /// <summary>
        /// 字符串加密混淆
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StringEncrypt(string str)
        {
            var strEncrypt = string.Empty;
            var strs1 = new List<string>
            {
                "a", "A", "b", "B", "c", "c",
                "d", "e", "f", "g", "h", "i"
            };
            var strs2 = new List<string>
            {
                 "j", "J", "k", "L", "M", "n",
                 "o","P","Q","X","x","z","Z"
            };
            var i = 1;
            foreach (var item in str)
            {
                if (i % 2 == 0)
                {
                    foreach (var s1 in strs1)
                    {
                        strEncrypt += s1 + item.ToString();
                    }
                }
                else
                {
                    foreach (var s2 in strs2)
                    {
                        strEncrypt += s2 + item.ToString();
                    }
                }
                i++;
            }
            return strEncrypt;
        }
    }
}
