﻿namespace TzCA.Common.JsonModels
{
    /// <summary>
    ///     登录用户状态
    /// </summary>
    public class LoginUserStatus
    {
        public bool IsLogin { get; set; } //是否登录成功
        public string Message { get; set; } //状态信息
        public bool IsDefaultPassword { get; set; } //是否使用默认密码登录
    }
}