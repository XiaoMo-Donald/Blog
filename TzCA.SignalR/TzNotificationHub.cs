using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using TzCA.DataAccess;
using TzCA.Entities.Notifications;
using TzCA.Common.TzEnums;
using TzCA.ViewModels.Notifications.Dtos;

namespace TzCA.SignalR
{
    /// <summary>
    /// 消息通知
    /// </summary>
    public class TzNotificationHub : Hub, ITzNotificationHub
    {
        private static List<Guid> Users = new List<Guid>();
        private static List<NotificationHubUserDto> OnlineUsers = new List<NotificationHubUserDto>();
        private readonly ITzChatRepository _tzChatRepository;
        private readonly IEntityRepository<TzNotification> _tzNotification;

        public TzNotificationHub(
            ITzChatRepository tzChatRepository,
            IEntityRepository<TzNotification> tzNotification
            )
        {
            this._tzChatRepository = tzChatRepository;
            this._tzNotification = tzNotification;
        }

        /// <summary>
        /// 全站消息推送（一般由站长使用）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SendAll(NotificationSendInput input)
        {
            await Clients.All.SendAsync("Receive", new NotificationReceiveDto
            {
                Sender = await _tzChatRepository.GetUserDtoById(_tzChatRepository.GetThisUserId),
                //Message = new NotificationMsgDto
                //{
                //    Content = input.Content,
                //    SendTime = DateTime.Now
                //}
            });
            //TODO:存储到数据库
        }

        /// <summary>
        /// 消息发送(通过接口目前有问题，只能暂时通过js调用)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Send(NotificationSendInput input)
        {
            try
            {
                var admin = await _tzChatRepository.GetUserByNameAsync("admin");
                var receiverId = Guid.Empty.Equals(input.ReceiverId) ? Guid.Parse(admin.Id) : input.ReceiverId;
                var senderId = _tzChatRepository.GetThisUserId;
                if (senderId.Equals(Guid.Parse(admin.Id)) && receiverId.Equals(Guid.Parse(admin.Id)))
                    return;
                else
                {
                    var notification = new TzNotification
                    {
                        ObjectId = input.ObjectId,
                        Description = input.Content,
                        Readed = false,
                        Link = "javascript:",
                        SenderId = senderId,
                        ReceiverId = receiverId,
                        Source = input.Source, //1.文章评论 、2.评论回复 、3.系统通知
                        ContentSource = input.ContentSource
                    };
                    var r = await _tzNotification.AddOrEditAndSaveAsyn(notification);
                    if (r)
                    {
                        if (OnlineUsers.SingleOrDefault(x => x.UserId.Equals(receiverId)) != null)
                        {
                            var sender = await _tzChatRepository.GetUserDtoById(senderId);
                            //判断源
                            var content = string.Empty;
                            switch (input.Source)
                            {
                                case "文章评论":
                                    content = "评论了文章《<span class='contentSource' title='" + input.ContentSource + "'>" + input.ContentSource + "</span> 》：<span class='content' title='" + input.Content + "'>" + input.Content + "</span>";
                                    break;
                                case "文章点赞":
                                    content = "赞了文章《<span class='contentSourceUnSub' title='" + input.ContentSource + "'>" + input.ContentSource + "</span> 》";
                                    break;
                                case "文章被踩":
                                    content = "踩了文章《<span class='contentSourceUnSub' title='" + input.ContentSource + "'>" + input.ContentSource + "</span> 》";
                                    break;
                                case "评论回复":
                                    content = "回复了内容 <span class='contentSource' title='" + input.ContentSource + "'>" + input.ContentSource + "</span>：<span class='content' title='" + input.Content + "'>" + input.Content + "</span>";
                                    break;
                                case "用户回复":
                                    content = "回复了内容 <span class='contentSource' title='" + input.ContentSource + "'>" + input.ContentSource + "</span>：<span class='content' title='" + input.Content + "'>" + input.Content + "</span>";
                                    break;
                                case "删除评论":
                                case "删除回复":
                                    content = "删除了评论内容：<span class='content' title='" + input.Content + "'>" + input.Content + "</span>";
                                    break;
                                default: content = "<span class='content' title='" + input.Content + "'>" + input.Content + "</span>"; break;
                            }
                            await Clients.User(receiverId.ToString()).SendAsync("Receive", new NotificationReceiveDto
                            {
                                ContentSource = input.ContentSource,
                                Content = content,
                                Link = "javascript:",
                                Sender = sender,
                                SendTime = notification.CreateTime,
                                Source = input.Source
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// 重写用户上线
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            var currentUserId = _tzChatRepository.GetThisUserId;
            if (!currentUserId.Equals(Guid.Empty))
            {
                var connectionId = this.Context.ConnectionId;
                var userExists = OnlineUsers.SingleOrDefault(x => x.UserId.Equals(currentUserId));
                if (userExists == null)
                    OnlineUsers.Add(new NotificationHubUserDto
                    {
                        UserId = currentUserId,
                        ConnectionId = connectionId
                    });
                //if (Guid.Empty.Equals(userExists) || userExists == null)
                //    Users.Add(currentUserId);
            }
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// 重写用户离线
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {

            var connectionId = this.Context.ConnectionId;
            var userExists = OnlineUsers.SingleOrDefault(x => x.ConnectionId.Equals(connectionId));
            if (userExists != null)
                OnlineUsers.Remove(userExists);

            //var currentUserId = _tzChatRepository.GetThisUserId;
            //var userExists = Users.SingleOrDefault(x => x.Equals(currentUserId));
            //if (!Guid.Empty.Equals(userExists) || userExists != null)
            //    Users.Remove(currentUserId);
            return base.OnDisconnectedAsync(exception);
        }

    }
}
