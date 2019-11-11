using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.Common.TzRandomData
{
    /// <summary>
    /// 随机数据接口
    /// </summary>
    public interface IRandomDataHepler
    {
        /// <summary>
        /// 生成随机用户名
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns></returns>
         string GetRandomUsername(int length);
        
        /// <summary>
        /// 获取随机昵称
        /// </summary>
        /// <returns></returns>
        string GetRandomNickname();
       
        /// <summary>
        /// 随机生成姓名
        /// </summary>
        /// <returns></returns>
        string GetRandomName();
    
        /// <summary>
        /// 获取随机头像
        /// </summary>
        /// <returns></returns>
        string GetRandomAvatar();

        /// <summary>
        /// 系统默认头像数据
        /// </summary>
        /// <returns></returns>
        List<string> DefaultAvatars();

        /// <summary>
        /// 获取随机个性签名
        /// </summary>
        /// <returns></returns>
       string GetRandomRemark();
     
        /// <summary>
        /// 个性签名数据
        /// </summary>
        /// <returns></returns>
        List<string> Remarks();        

        /// <summary>
        /// 昵称数据
        /// </summary>
        /// <returns></returns>
         List<string> Nicknames();

        /// <summary>
        /// 获取姓氏
        /// </summary>
        /// <returns></returns>
        List<string> Surnames();

        /// <summary>  
        /// 随机产生常用汉字  
        /// </summary>  
        /// <param name="count">要产生汉字的个数</param>  
        /// <returns>常用汉字</returns>  
        List<string> GenerateChineseWords(int count);

        /// <summary>
        /// 系统默认语录
        /// </summary>
        /// <returns></returns>
        List<string> GetQuotations();

        /// <summary>
        /// 给定一个范围生成一个随机数
        /// </summary>
        /// <param name="minVal">最小值</param>
        /// <param name="maxVal">最大值</param>
        /// <returns></returns>
        int Random(int minVal, int maxVal);

    }
}
