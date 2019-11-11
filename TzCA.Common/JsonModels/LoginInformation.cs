namespace TzCA.Common.JsonModels
{
    /// <summary>
    /// 基本登录信息
    /// </summary>
    public class LoginInformation
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 校验码（目前使用前端实现校验）
        /// </summary>
        public string VerificationCode { get; set; }

        /// <summary>
        /// 返回的路径（登录完成后跳转原来的页面）
        /// </summary>
        public string ReturnUrl { get; set; } = null;
    }
}