using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TzCA.Entities.ApplicationOrganization;
using TzCA.Entities.ChatRoom;
using TzCA.ViewModels.ApplicationOrganization;
using TzCA.ViewModels.ChatRoom;
using TzCA.ViewModels.ChatRoomDtos;

namespace TzCA.SignalR
{
    /// <summary>
    /// 聊天室数据处理接口
    /// </summary>
    public interface ITzChatRepository
    {
        #region Chat相关方法定义

        /// <summary>
        /// 添加用户的会话记录（用户之间仅一条）
        /// </summary>
        /// <param name="receiverId"></param>
        /// <returns></returns>
        Task AddChatRecord(Guid receiverId);

        /// <summary>
        /// 保存聊天记录到数据库
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        Task AddChatRecordContent(ChatRecordContentInput input);

        /// <summary>
        /// 获取当前用户的聊天记录
        /// </summary>
        /// <param name="receiverId">接收者的Id</param>
        /// <returns></returns>
        Task<List<ChatRecordContentVM>> GetChatRecordContent(Guid receiverId);
        #endregion


        #region 用户相关方法的定义

        /// <summary>
        /// 当前用户的Id
        /// </summary>
        Guid GetThisUserId { get; }

        /// <summary>
        /// 获取当前用户（视图模型）
        /// </summary>
        ApplicationUserVM GetThisUserVM { get; }

        /// <summary>
        ///  根据用户Id获取用户部分信息（Dto）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApplicationUserDto> GetUserDtoById(Guid userId);
        /// <summary>
        ///  根据用户获取用户部分信息（Dto）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ApplicationUserDto> GetUserDtoByUser(ApplicationUser user);

        /// <summary>
        /// 获取头像
        /// </summary>
        /// <param name="userVM">用户视图模型</param>
        /// <returns></returns>
        Task<ApplicationUserVM> GetUserVMContainAvatar(ApplicationUserVM userVM);

        /// <summary>
        /// 获取当前用户（视图模型）
        /// </summary>
        /// <returns></returns>
        ApplicationUserVM UserVM();

        /// <summary>
        /// 获取所有用户（视图模型）
        /// </summary>
        List<ApplicationUserVM> GetAllUserVM { get; }

        /// <summary>
        /// 根据Id获取用户
        /// </summary>
        /// <returns></returns>
        Task<ApplicationUser> GetUserByIdAsync(Guid id);

        /// <summary>
        ///  根据用户名获取用户
        /// </summary>
        /// <returns></returns>
        Task<ApplicationUser> GetUserByNameAsync(string username);

        /// <summary>
        /// 根据指定Id获取用户（视图模型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApplicationUserVM> GetUserVM(Guid id);

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        ApplicationUser User();

        /// <summary>
        /// 异步获取当前用户
        /// </summary>
        /// <returns></returns>
        Task<ApplicationUser> GetThisUser();

        /// <summary>
        /// 获取当前用户名
        /// </summary>
        /// <returns></returns>
        string GetThisName();
        #endregion
    }
}
