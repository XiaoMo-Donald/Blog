using System;
using System.Collections.Generic;
using System.Text;

namespace TzCA.Entities.ApplicationOrganization
{
    /// <summary>
    /// 网站注册邀请码
    /// 说明：
    /// 1.可以配置是否开启使用邀请码注册
    /// 2.可配置邀请码是否可在注册页面获取
    ///     2.1.管理员分配，直接分配
    ///     2.2.注册页面获取：（根据IP地址分配邀请码）
    /// </summary>
    public class InvitationCode : EntityBase, IEntity
    {
        //邀请码属性使用基类的Name属性
    }
}
