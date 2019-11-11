using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TzCA.DataAccess;
using TzCA.Entities.ApplicationOrganization;
using TzCA.Entities.ChatRoom;
using TzCA.SignalR;
using TzCA.ViewModels.ApplicationOrganization;
using TzCA.ViewModels.ChatRoom;
using TzCA.ViewModels.ChatRoomDtos;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TzCA.Web.Controllers.TzChat
{
    /// <summary>
    /// Chat聊天控制器
    /// </summary>
    public class ChatController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITzChatRepository _tzChatRepository;
        private readonly IEntityRepository<ChatRecord> _chatRecordRepository;
        private readonly IEntityRepository<ChatRecordContent> _chatRecordContentRepository;
        public ChatController(
            UserManager<ApplicationUser> userManager,
            ITzChatRepository tzChatRepository,
            IEntityRepository<ChatRecord> chatRecordRepository,
            IEntityRepository<ChatRecordContent> chatRecordContentRepository

            )
        {
            this._userManager = userManager;
            this._tzChatRepository = tzChatRepository;
            this._chatRecordRepository = chatRecordRepository;
            this._chatRecordContentRepository = chatRecordContentRepository;
        }

        /// <summary>
        /// 聊天室首页
        /// </summary>
        /// <returns></returns>
        [Authorize]

        public async Task<IActionResult> Index()
        {
            //临时传参数（后面在TzChatHub类中实现）
            var model = new TzChatUserVM(await GetUser());
            return View(model);
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>   
        public async Task<ApplicationUser> GetUser()
        {
            try
            {
                var user = await this._userManager.FindByNameAsync(User.Identity.Name); return user = user == null ? null : user;
            }
            catch (Exception)
            {
                return null;
            }
        }       
    }
}
