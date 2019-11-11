using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using TzCA.DataAccess;
using TzCA.Entities.ApplicationOrganization;
using TzCA.Entities.Attachments;
using TzCA.Entities.ChatRoom;
using TzCA.ViewModels.ApplicationOrganization;
using TzCA.Common.TzEncrypt;
using TzCA.ViewModels.ChatRoomDtos;
using TzCA.ViewModels.ChatRoom;

namespace TzCA.SignalR
{
    /// <summary>
    /// 聊天室数据处理接口实现
    /// </summary>
    public class TzChatRepository : ITzChatRepository
    {
        /// <summary>
        /// 获取当前用户的Id
        /// </summary>
        public Guid GetThisUserId { get { return GetUserId(); } }

        /// <summary>
        /// 获取当前用户（视图模型）
        /// </summary>
        public ApplicationUserVM GetThisUserVM { get { return UserVM(); } }

        /// <summary>
        /// 获取所有用户（视图模型）
        /// </summary>
        public List<ApplicationUserVM> GetAllUserVM { get { return GetUsersVM(); } }
       
        // 依赖注入
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEntityRepository<BusinessImage> _businessImage;
        private readonly IEntityRepository<ChatRecord> _chatRecordRepository;
        private readonly IEntityRepository<ChatRecordContent> _chatRecordContentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public TzChatRepository(
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                IEntityRepository<BusinessImage> businesvsImage,
                IEntityRepository<ChatRecord> chatRecordRepository,
                IEntityRepository<ChatRecordContent> chatRecordContentRepository,
                IHttpContextAccessor httpContextAccessor
                )
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._businessImage = businesvsImage;
            this._chatRecordRepository = chatRecordRepository;
            this._chatRecordContentRepository = chatRecordContentRepository;
            this._httpContextAccessor = httpContextAccessor;      
        }

        #region 聊天记录相关

        /// <summary>
        /// 添加用户的会话记录（用户之间仅一条）
        /// </summary>
        /// <returns></returns>
        public async Task AddChatRecord(Guid receiverId)
        {
            //查询当前用户会话是否已经存在
            var chatRecord = _chatRecordRepository.GetAll()
                .FirstOrDefault(x => x.SenderId == GetThisUserId && x.ReceiverId == receiverId || x.SenderId == receiverId && x.ReceiverId == GetThisUserId);

            if (chatRecord == null)
            {
                chatRecord = new ChatRecord
                {
                    SenderId = GetThisUserId,
                    ReceiverId = receiverId
                };
                await _chatRecordRepository.AddOrEditAndSaveAsyn(chatRecord);
            }
        }

        /// <summary>
        /// 保存聊天记录到数据库
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public async Task AddChatRecordContent(ChatRecordContentInput input)
        {
            var thisUserId = GetThisUserId;
            var chatRecord = _chatRecordRepository.GetAll()
                .FirstOrDefault(x => x.SenderId == GetThisUserId && x.ReceiverId == input.ReceiverId || x.SenderId == input.ReceiverId && x.ReceiverId == thisUserId);
            var chatRecordContent = new ChatRecordContent
            {
                ChatRecordId = chatRecord.Id,
                AscriptionUserId=thisUserId,
                SenderId = thisUserId,
                ReceiverId = input.ReceiverId,
                Message = input.Message
            };
            await _chatRecordContentRepository.AddOrEditAndSaveAsyn(chatRecordContent);
        }

        /// <summary>
        /// 获取当前用户的聊天记录(视图模型)
        /// 默认获取最近50条记录（可设置）
        /// </summary>
        /// <param name="receiverId">接收者的Id</param>
        /// <returns></returns>
        public async Task<List<ChatRecordContentVM>> GetChatRecordContent(Guid receiverId)
        {
            var maxCount = 50; //默认获取50条历史消息
            var query = await _chatRecordRepository
                .FindByAsyn(x => x.SenderId == GetThisUserId && x.ReceiverId == receiverId || x.SenderId == receiverId && x.ReceiverId == GetThisUserId);
            var chatRecordContentsVM = new List<ChatRecordContentVM>();
            if (query.FirstOrDefault() != null)
            {
                var chatRecordContentsQuery = await _chatRecordContentRepository.FindByAsyn(x => x.ChatRecordId == query.FirstOrDefault().Id);
                if (chatRecordContentsQuery.Count() > 0)
                {
                    var queryCount = chatRecordContentsQuery.Count();                  
                    var chatRecordContents = chatRecordContentsQuery.OrderBy(x=>x.CreateTime)
                        .Skip(queryCount >= maxCount ? queryCount-maxCount : 0)
                        .Take(queryCount);
                    foreach (var item in chatRecordContents)
                    {  
                        chatRecordContentsVM.Add(new ChatRecordContentVM(item));
                    }
                }
            }         
            return chatRecordContentsVM;
        }


        #endregion

        #region 用户相关

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        public ApplicationUser User()
        {
            var currentUser= _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User)?.Result;
            return currentUser;
        }

         /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        /// <summary>
        public async Task<ApplicationUser> GetThisUser()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            return user;
        }

        /// <summary>
        /// 获取当前用户视图模型
        /// </summary>
        /// <returns></returns>
        public ApplicationUserVM UserVM()
        {
            var userVM = new ApplicationUserVM(User());
            var avatar = _businessImage.GetAll().FirstOrDefault(x => x.RelevanceObjectId == userVM.Id);
            userVM.Avatar = avatar.RelativePath;
            userVM.UserAvatar = avatar;

            return userVM;
        }

        /// <summary>
        /// 根据Id获取用户
        /// </summary>
        /// <returns></returns>
        public async Task<ApplicationUser> GetUserByIdAsync(Guid id)
        {
            try
            {
                var user = await this._userManager.FindByIdAsync(id.ToString());
                return user = user == null ? null : user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <returns></returns>
        public async Task<ApplicationUser> GetUserByNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        /// <summary>
        /// 获取指定用户（视图模型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApplicationUserVM> GetUserVM(Guid id)
        {
            var userVM = new ApplicationUserVM(await GetUserByIdAsync(id));
            var avatar = _businessImage.GetAll().FirstOrDefault(x => x.RelevanceObjectId == userVM.Id);
            userVM.Avatar = avatar.RelativePath;
            userVM.UserAvatar = avatar;
            return userVM;
        }

        /// <summary>
        /// 获取所有用户VM
        /// </summary>
        /// <returns></returns>
        public List<ApplicationUserVM> GetUsersVM()
        {
            var query = _userManager.Users;
            var users = new List<ApplicationUserVM>();
            foreach (var item in query)
            {
                var user = new ApplicationUserVM(item);
                users.Add(user);
            }
            return users;
        }


        /// <summary>
        ///  根据用户Id获取用户部分信息（Dto）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApplicationUserDto> GetUserDtoById(Guid userId)
        {
            var userVM = new ApplicationUserVM(await GetUserByIdAsync(userId));
            return new ApplicationUserDto(await GetUserVMContainAvatar(userVM));
        }

        /// <summary>
        ///  根据用户获取用户部分信息（Dto）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ApplicationUserDto> GetUserDtoByUser(ApplicationUser user)
        {
            var userVM = new ApplicationUserVM(user);
            return new ApplicationUserDto(await GetUserVMContainAvatar(userVM));
        }

        /// <summary>
        /// 获取头像
        /// </summary>
        /// <param name="userVM">用户视图模型</param>
        /// <returns></returns>
        public async Task<ApplicationUserVM> GetUserVMContainAvatar(ApplicationUserVM userVM)
        {
            var images = await _businessImage.GetAllAsyn();
            var avatar = images.FirstOrDefault(x => x.RelevanceObjectId == userVM.Id && x.Type.Equals(BusinessImageEnum.Avatars));
            var defaultAvatar = "../images/chatAvatars/defaultAvatar.jpg";
            userVM.Avatar = string.IsNullOrEmpty(avatar?.RelativePath) ? defaultAvatar : avatar?.RelativePath;
            userVM.MinAvatar = string.IsNullOrEmpty(avatar?.MinRelativePath) ? defaultAvatar : avatar?.MinRelativePath;
            return userVM;
        }

        /// <summary>
        /// 获取当前用户的Id
        /// </summary>
        /// <returns></returns>
        public Guid GetUserId()
        {
            var currentUser = User();
            var userId = currentUser == null ? Guid.Empty : Guid.Parse(currentUser?.Id);
            return userId;
        }

        /// <summary>
        /// 获取当前用户名
        /// </summary>
        /// <returns></returns>
        public string GetThisName()
        {                  
            var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
            //加密混淆    
            userName = TzEncryptHelper.StringEncrypt(userName);
            return userName;
        }
        #endregion
    }
}
