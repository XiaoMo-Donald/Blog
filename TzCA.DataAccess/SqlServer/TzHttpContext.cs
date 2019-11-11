using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.DataAccess.SqlServer
{
    /// <summary>
    /// 自定义请求
    /// </summary>
    public class TzHttpContext
    {
        public static IServiceProvider ServiceProvider;
        static TzHttpContext()
        { }
        public static HttpContext Current
        {
            get
            {
                object factory = ServiceProvider.GetService(typeof(IHttpContextAccessor));
                HttpContext context = ((IHttpContextAccessor)factory).HttpContext;
                return context;
            }
        }

    }
}
