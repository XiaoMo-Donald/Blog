using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TzCA.ViewModels.ApplicationOrganization;
using TzCA.ViewModels.ChatRoom;
using TzCA.ViewModels.ChatRoomDtos;

namespace TzCA.SignalR
{
    public class TzChatHub : Hub
    {
        //private static Lazy<List<HubUser>> _onlineUser = new Lazy<List<HubUser>>();
        //public static List<HubUser> OnlineUser { get { return _onlineUser.Value; } }

        /// <summary>
        /// 当前的用户Id
        /// </summary>
        public Guid UserId { get { return _tzChatRepository.GetThisUserId; } }

        /// <summary>
        /// 当前的用户(视图模型)
        /// </summary>
        public ApplicationUserVM UserVM { get { return _tzChatRepository.GetThisUserVM; } }

        ///// <summary>
        ///// 存当前的用户，给离线时候使用(视图模型)
        ///// </summary>
        //private ApplicationUserVM OnUserVM = null;

        /// <summary>
        /// 当前在线用户（系统用户）
        /// </summary>
        public static List<ApplicationUserVM> OnlineUser = new List<ApplicationUserVM>();

        /// <summary>
        /// 用于输出到前端展示的用户列表
        /// </summary>
        public static List<TzChatUser> ChatUserList = new List<TzChatUser>();

        /// <summary>
        /// 接口注入
        /// </summary>
        private readonly ITzChatRepository _tzChatRepository;
        public TzChatHub(ITzChatRepository tzChatRepository)
        {
            this._tzChatRepository = tzChatRepository;         
            if (TzChatStatus.OnUserVM == null) TzChatStatus.OnUserVM = UserVM;
        }

        #region 2018-07-29 实现点对点聊天

        /// <summary>
        /// 重写链接事件
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            // 查询用户。
            //var chatUser = ChatUserList.SingleOrDefault(u => u.UserName.Equals(_tzChatRepository.GetThisName()));
            //var chatUser = ChatUserList.SingleOrDefault(u => u.UserName == TzEncryptHelper.StringEncrypt(_tzChatRepository.GetThisName()));
            var chatUser = ChatUserList.SingleOrDefault(u => u.UserId == UserId);
            //var chatUser = ChatUserList.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);
            //var systemUser = OnlineUser.SingleOrDefault(u => u.Id == UserId);
            //判断用户是否存在,否则添加进集合
            if (chatUser == null)
            {
                chatUser = new TzChatUser(UserVM);
                chatUser.ConnectionId = Context.ConnectionId;
                chatUser.UserName = _tzChatRepository.GetThisName();
                ChatUserList.Add(chatUser);
            }

            return base.OnConnectedAsync();
        }

        /// <summary>
        /// 重写断开事件
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            // var user = ChatUserList.FirstOrDefault(u => u.UserName.Equals(_tzChatRepository.GetThisName()));
            //var user = ChatUserList.SingleOrDefault(u => u.UserName == TzEncryptHelper.StringEncrypt(_tzChatRepository.GetThisName()));
            var user = ChatUserList.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);
            //判断用户是否存在,存在则删除
            if (user != null)
            {
                //删除用户
                ChatUserList.Remove(user);
            }
            //更新所有用户的列表
            GetUserList();
            var chatUserLoginVM = new ChatUserLoginVM
            {
                //LoginUser = new TzChatUser(UserVM),
                LoginUser = new TzChatUser(TzChatStatus.OnUserVM),
                ChatUserList = ChatUserList
            };
            Clients.All.SendAsync("UserUnonline", chatUserLoginVM);
            return base.OnDisconnectedAsync(exception);
        }


        /// <summary>
        /// 获取在线的用户
        /// </summary>
        /// <returns></returns>
        public void GetOnlineUsers()
        {
            Clients.Client(Context.ConnectionId).SendAsync("GetOnlineUsers", ChatUserList);
        }


        /// <summary>
        /// 获取自己的信息
        /// </summary>
        /// <returns></returns>
        public async Task GetOwn()
        {
            //  var user = ChatUserList.SingleOrDefault(u => u.UserId == UserId);
            var user = ChatUserList.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);
            if (user != null)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("GetOwn", user);
            }
            GetUserList();
        }

        /// <summary>
        /// 用户登录（需要更新列表）
        /// </summary>
        public async Task SendUserLogin()
        {
            GetUserList();
            var chatUserLoginVM = new ChatUserLoginVM
            {
                LoginUser = new TzChatUser(UserVM),
                ChatUserList = ChatUserList
            };
            await Clients.All.SendAsync("SendUserLogin", chatUserLoginVM);
        }

        /// <summary>
        /// 更新所有用户的在线列表
        /// </summary>
        private void GetUserList()
        {
            var itme = from a in ChatUserList
                       select new { a.ConnectionId, a.Nickname, a.UserId, a.UserName, a.Avatar, a.Remark };
            string jsondata = JsonConvert.SerializeObject(itme.ToList());
            Clients.Users(jsondata);
        }

        ///// <summary>
        ///// 发送消息
        ///// </summary>
        ///// <param name="ConnectionId"></param>
        ///// <param name="Message"></param>
        //public async Task SendMessage(string connectionId, string message)
        //{
        //    var sender = UserList.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);
        //    var receiver = UserList.SingleOrDefault(u => u.ConnectionId == connectionId);
        //    var receiverOnlineState = false;
        //    var unOnlineMsg = "";
        //    //判断用户是否存在,存在则发送
        //    if (receiver != null)
        //    {
        //        var msg = new ChatMessage
        //        {
        //            Sender = sender,
        //            ReceiverOnlineState = receiverOnlineState,
        //            ReceiverUnOnlineMsg = unOnlineMsg,
        //            Message = message
        //        };

        //        #region 使用SignalR自己的链接Id发送消息
        //        //给指定用户发送
        //        //await Clients.All.SendAsync("SendMessage", msg);
        //        await Clients.Client(connectionId).SendAsync("ReceiveMessage", msg);
        //        //给自己发送,把用户的ID传给自己
        //        await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", msg);
        //        #endregion 
        //    }
        //    else
        //    {
        //        unOnlineMsg = "该用户已离线";
        //        receiverOnlineState = false;
        //        await Clients.Client(Context.ConnectionId).SendAsync(unOnlineMsg);
        //    }
        //    var record = new ChatRecordDto
        //    {
        //        SenderId = sender.Id,
        //        ReceiverId = receiver.Id,
        //        Message = message
        //    };
        //    await _tzChatRepository.AddChatRecord(record);
        //}

        #endregion


        #region 使用用户Id进行消息通讯

        public async Task SendMessage(Guid receiverId, string message)
        {
            var receiverUser = await _tzChatRepository.GetUserByIdAsync(receiverId);
            if (receiverUser != null)
            {

                //var thisReceiver = new TzChatUser(await _tzChatRepository.GetUserVM(receiverId));
                var sender = new TzChatUser(UserVM);
                sender.UserName = null;
                var msg = new ChatMessage
                {
                    //Sender = new TzChatUser(UserVM),
                    //Receiver = thisReceiver,
                    Sender = sender,
                    ReceiverId = receiverId,
                    Message = message
                };

                //发送前保存消息到数据库
                await AddChatRecordContent(msg);

                var receiver = ChatUserList.SingleOrDefault(x => x.UserId == receiverId);
                //判断用户是否存在,存在则发送
                if (receiver != null)
                {
                    #region 使用系统用户的Id发送消息
                    await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", msg);                  
                    #endregion
                }
                else
                {
                    await Clients.User(UserId.ToString()).SendAsync("FriendsUnOnline", "该好友已离线，但消息已经保存!");

                }
                //给自己发送,把用户的ID传给自己
                await Clients.User(UserId.ToString()).SendAsync("ReceiveMessage", msg);
            }
            else
            {
                //脚本需要监听UserUnOnline事件 用户不存在                  
                await Clients.User(UserId.ToString()).SendAsync("UserUnOnline", "用户不存在或用户账号已经被注销。");
            }
            #endregion
        }

        /// <summary>
        /// 添加当前与好友的会话
        /// </summary>
        /// <param name="receiverId"></param>
        /// <returns></returns>
        public async Task AddChatRecord(Guid receiverId)
        {
            await _tzChatRepository.AddChatRecord(receiverId);
            await LoadFriendChatWindow(receiverId);
        }

        /// <summary>
        /// 保存聊天记录
        /// </summary>
        /// <returns></returns>
        public async Task AddChatRecordContent(ChatMessage message)
        {
            var chatRecordContentInput = new ChatRecordContentInput
            {
                ReceiverId = message.ReceiverId,
                Message = message.Message
            };
            await _tzChatRepository.AddChatRecordContent(chatRecordContentInput);
        }

        /// <summary>
        /// 获取单个好友
        /// </summary>
        /// <returns></returns>
        public async Task LoadFriendChatWindow(Guid id)
        {
            //获取好友信息
            var friendVM = await _tzChatRepository.GetUserVM(id);
            var chatRecordContents = await _tzChatRepository.GetChatRecordContent(id);

            //获取好友聊天记录
            var output = new GetFriendOutput
            {
                Receiver = new TzChatUser(friendVM),
                Sender = new TzChatUser(UserVM),
                ChatRecordContents = chatRecordContents
            };
            await Clients.Client(Context.ConnectionId).SendAsync("RenderFriendChatWindow", output);
        }
    }
}