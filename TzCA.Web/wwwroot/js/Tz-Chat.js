
//配置链接
const connection = new signalR.HubConnectionBuilder()
    .withUrl('/tzChatHub')
    .configureLogging(signalR.LogLevel.Information)
    .build();
//启用链接
connection.start().catch(err => consle.error(err.toString()));

//定义消息接收方法
connection.on("ReceiveMessage", (data) => {
    var _currUserConnectionId = $("#myName").data("myconnectionid");
    var _messageHtml = '';
    if (_currUserConnectionId === data.sender.userId) {
        //自己发送的消息
        _messageHtml = '<li class="layim-chat-mine">' +
            '   <div class="layim-chat-user">' +
            '       <img src = "' + data.sender.avatar + '" ><cite><i>' + data.sendTime + '</i>' + data.sender.nickname + '</cite></div >' +
            '  <div class="layim-chat-text">' + messageTemplate(true, data.message) + '</div>' +
            '</li>';
    } else {
        var _$receiverConnectionId = $("input[name=receiverConnectionId]").val();
        if ($("#chatRoom").css("display") === "none") {
            layer.msg(data.sender.nickname + '发来了新的消息！', function () {
                //关闭后的操作
            });
            return;
        }
        if ($("input[name=receiverConnectionId]").val() !== data.sender.userId) {
            layer.msg(data.sender.nickname + '发来了新的消息！', function () {
                //关闭后的操作
            });
            return;
        }
        //接收到的消息
        _messageHtml = '<li>' +
            '<div class="layim-chat-user"><img src="' + data.sender.avatar + '"><cite>' + data.sender.nickname + '<i>' + data.sendTime + '</i></cite></div>' +
            '<div class="layim-chat-text"> ' + messageTemplate(false, data.message) + '</div>' +
            '</li>';
    }
    var $fillArea = $(".layim-chat-main ul");
    $fillArea.append(_messageHtml);
    $(".layim-chat-main").scrollTop($fillArea.height());
    listenOpenNotLocalUrl();
})


$('#sendMessageBtn').on('click', function () {
    toSendMessage();
})

$('.layim-chat-textarea textarea').on("keyup", function (e) {
    e.preventDefault();
    var theEvent = e || window.event,
        //浏览器兼容
        code = theEvent.keyCode || theEvent.which || theEvent.charCode;
    if (code === 13) {
        toSendMessage();
    }
})

function toSendMessage() {
    var _receiverId = $("input[name=receiverConnectionId]").val(),
        $message = $(".layim-chat-textarea textarea");
    if ($message.val().trim() === '') {
        layer.tips('请说点什么吧...', '#sendMessageBtn', {
            tips: [1, '#3595CC'],
            time: 2000
        });
        return;
    };
    connection.invoke('SendMessage', _receiverId, filterXSS($message.val())).catch(err => console.log(err.toString()));
    $message.val('');
    $message.text('');
}

//获取在线的用户
//connection.on("GetOnlineUsers", function (data) {
//    var onlineTpl = '';
//    $.each(data, function (index, val) {
//        onlineTpl += '<li layim-event="chat" data-connectionId="' + val.connectionId + '" data-username="' + val.name + '" data-type="friend" data-index="0" class="layim-friend' + val.id + ' myFriend">' +
//            '    <img src="' + val.avatar + '"><span>' + val.name + '</span>' +
//            '    <p>' + val.remark + '</p><span class="layim-msg-status">new</span>' +
//            '</li>';
//    })

//    var $onlineUsers = $("#onlineUsers");
//    $("#onlineUsersCount").text(data.length);
//    $onlineUsers.html(onlineTpl);
//})

//监听用户离线（更新在线用户的列表）
connection.on("UserUnonline", function (data) {
    renderOnlineUser(data, 1);
})

//监听用户上线
connection.on("SendUserLogin", function (data) {
    renderOnlineUser(data, 0);
})

//渲染用户列表
function renderOnlineUser(data, type) {
    var onlineTpl = '';
    $.each(data.chatUserList, function (index, val) {
        //let _isNew = val.hasNotReadMsg === true ? 'msg-status-new' : ''; //后台未实现
        let _isNew = 'msg-status-new';
        onlineTpl += '<li layim-event="chat" data-connectionId="' + val.userId + '" data-username="' + val.nickname + '" data-type="friend" data-index="0" class="layim-friend' + val.userId + ' myFriend">' +
            '    <img src="' + val.avatar + '"><span>' + val.nickname + '</span>' +
            '    <p>' + val.remark + '</p>' +
            '    <span class="layim-msg-status ' + _isNew + '"> new</span > ' +
            '</li>';
    });

    if (type === 0) {
        //用户上线提醒（右下角提醒）
        if ($("#myName").data("myconnectionid") !== data.loginUser.userId) {
            layer.open({
                //type: 1,
                //icon: 1,
                title: '好友上线提醒',
                offset: 'rb', //具体配置参考：offset参数项
                content: '【 ' + data.loginUser.nickname + '】已上线。',
                //closeBtn: 0, //不显示关闭按钮
                btn: [],
                shade: 0, //不显示遮罩
                anim: 2,
                time: 10000, //3s后自动关闭 
                yes: function (index) {
                    layer.close(layer.index);
                }
            });
        }
    }
    var $onlineUsers = $("#onlineUsers");
    $("#onlineUsersCount").text(data.chatUserList.length);
    $onlineUsers.html(onlineTpl);
}

connection.on("GetOwn", function (data) {
    $("#myName").text(data.nickname);
    $(".layui-layim-remark").val(data.remark);
});

//监听用户(好友)离线状态
connection.on("FriendsUnOnline", function (data) {
    console.info(data);
    //layer.msg(data, { icon: 5 });
});

//加载好友聊天窗口
connection.on("RenderFriendChatWindow", function (data) {
    var _messageHtml = '<li class="tz-chat-messageRecordArea">'
        + '     <div class="tz-chat-messageRecordTips layim-chat-text">'
        + '         历史消息&nbsp;<a href="javascript:" class="tz-chat-messageHistory">查看更多</a>'
        + '     </div>'
        + '     <span class="tz-chat-hr"></span>'
        + '</li>';
    if (data.chatRecordContents.length > 0) {
        $.each(data.chatRecordContents, function (index, val) {
            if (val.ascriptionUserId !== data.sender.userId) {
                _messageHtml += '<li>' +
                    '<div class="layim-chat-user"><img src="' + data.receiver.avatar + '"><cite>' + data.receiver.nickname + '<i>' + val.createTime + '</i></cite></div>' +
                    '<div class="layim-chat-text"> ' + messageTemplate(false, val.message) + '</div>' +
                    '</li>';
            } else {
                _messageHtml += '<li class="layim-chat-mine">' +
                    '   <div class="layim-chat-user">' +
                    '       <img src = "' + data.sender.avatar + '" ><cite><i>' + val.createTime + '</i>' + data.sender.nickname + '</cite></div >' +
                    '  <div class="layim-chat-text">' + messageTemplate(true, val.message) + '</div>' +
                    '</li>';
            }
        });
    }
    else {
        //提示还没有消息记录
        _messageHtml = '<li class="tz-chat-messageRecordArea">'
            + '<div class="tz-chat-messageRecordTips layim-chat-text">'
            + '   还没有消息，先打个招呼吧！'
            + '</div>'
            + '<span class="tz-chat-hr"></span>'
            + '</li>';
    }
    var _$chatRecordArea = $(".layim-chat-main ul");
    _$chatRecordArea.html(_messageHtml);
    setTimeout(function () {
        $(".layim-chat-main").scrollTop(_$chatRecordArea.height(), 500);
    }, 100);

    $("#username").text(data.receiver.nickname);
    $("#userAvatar").attr("src", data.receiver.avatar);
    $("#userRemark").text(data.receiver.remark);
    $("#chatRoom").show();
    $("input[name=receiverConnectionId]").val(data.receiver.userId);
    listenOpenNotLocalUrl();
})

//获取url
function getUrl(content) {
    //let matcht = /^(https?:\/\/)([0-9a-z.]+)(:[0-9]+)?([/0-9a-z.]+)?(\?[0-9a-z&=]+)?(#[0-9-a-z]+)?/i;
    let matcht = /^(?:([A-Za-z]+):)?(\/{0,3})([0-9.\-A-Za-z]+)(?::(\d+))?(?:\/([^?#]*))?(?:\?([^#]*))?(?:#(.*))?$/;
    let _matchtResult = matcht.exec(content.trim());
    //console.log(_matchtResult);
    //截取非url内容
    let _getUrlResult = '';
    if (_matchtResult !== null) {
        _getUrlResult = _matchtResult[0].trim().split(/\s+/)[0];
    } else {
        _getUrlResult = null;
    }
    //console.log(_getUrlResult);
    return _getUrlResult;
}

//let _testUrl = "https://localhost:39070/Chat/?Index=啊啊&你好啊 哈哈哈哈";
//getUrl(_testUrl);

//检测当前连接是否为url
function isUrl(content) {
    var regex = '^((https|http|ftp|rtsp|mms)?://)'
        + '?(([0-9a-z_!~*().&=+$%-]+: )?[0-9a-z_!~*().&=+$%-]+@)?' //ftp的user@
        + '(([0-9]{1,3}.){3}[0-9]{1,3}' // IP形式的URL- 199.194.52.184
        + '|' // 允许IP和DOMAIN（域名）
        + '([0-9a-z_!~*()-]+.)*' // 域名- www.
        + '([0-9a-z][0-9a-z-]{0,61})?[0-9a-z].' // 二级域名
        + '[a-z]{2,6})' // first level domain- .com or .museum
        + '(:[0-9]{1,4})?' // 端口- :80
        + '((/?)|' // a slash isn't required if there is no file name
        + '(/[0-9a-z_!~*().;?:@&=+$,%#-]+)+/?)$';
    var re = new RegExp(regex);
    var reResult = re.test(content.trim());
    //console.info("来源：" + content.trim() + "    URL校验结果:" + reResult);
    return reResult;
}

//检测当前url是否为本站url
function isLocalhostUrl(url) {
    let matcht = /^(https?:\/\/)([0-9a-z.]+)(:[0-9]+)?([/0-9a-z.]+)?(\?[0-9a-z&=]+)?(#[0-9-a-z]+)?/i;
    let matchtResult = matcht.exec(url.trim());
    let messageUrl = matchtResult[1] + matchtResult[2];
    //console.info(matchtResult);
    //console.info(messageUrl);

    //document.location.protocol + '//' + window.document.location.hostname
    let localUrl = document.location.protocol + '//' + window.document.domain;
    //console.info("本地域名：" + localUrl);
    if (messageUrl !== null && messageUrl === localUrl) return true;
    else return false;
}

//消息链接 isMine:是否是自己 message:消息内容
function messageTemplate(isMine, message) {
    let _getMessageUrl = getUrl(message);
    let _messageResult = '';
    if (_getMessageUrl !== null) {
        if (isMine) {
            _messageResult = '<a href="' + _getMessageUrl + '" target="_blank" title="' + _getMessageUrl + '" class="chat-message-link">' + message + '</a>';
        } else {
            if (isLocalhostUrl(_getMessageUrl)) {
                _messageResult = '<a href="' + _getMessageUrl + '" target="_blank" title="来自本站的链接！" class="chat-message-link">' + message + '</a><span class="is-local-url">（本站链接）</span>';
            } else {
                _messageResult = '<a href="javascript:" data-href="' + _getMessageUrl + '" class="chat-message-link chat-message-not-local-link" title="来自非本站链接！">' + message + '</a><span class="not-local-url">（非本站链接）</span>';
            }
        }
    } else _messageResult = message;
    return _messageResult;
}

/***监听非本站链接点击事件 */
function listenOpenNotLocalUrl() {
    $(".chat-message-not-local-link").off("click").on("click", function (e) {
        let _$this = $(this);
        let _thisHref = _$this.data("href");
        e.preventDefault();
        layer.confirm('您即将打开来自非本站的链接，是否继续？</br>（<span style="color:red;">官方提示：访问来自非本站链接请注意！</span>）', {
            title: '站外链接访问提示',
            btn: ['是', '否'],
            btnAlign: 'c'
        }, function (index) {
            layer.close(index);
            window.open(_thisHref);
        });
    });
}

function own_init() {
    setTimeout(function () {
        connection.invoke('GetOwn').catch(err => console.log(err.toString()));
    }, 800)
}
function friends_init() {
    setTimeout(function () {
        connection.invoke('GetOnlineUsers').catch(err => console.log(err.toString()));
    }, 1000)
}
function render_friends_init() {
    setTimeout(function () {
        connection.invoke('SendUserLogin').catch(err => console.log(err.toString()));
    }, 1000)
}
function init() {
    own_init();
    render_friends_init();
}


//双击好友列表中的好友
$("#onlineUsers").on("dblclick", ".myFriend", function (e) {
    var _this = $(this),
        _own = $("#myName").data("myconnectionid"),
        _receiverId = _this.data("connectionid");

    if (_own === _receiverId) {
        layer.msg('您不能和自己发起会话！');
        //layer.msg('您不能和自己发起会话！', { icon: 5 });
        //layer.msg('您不能和自己发起会话', function () {
        //    //关闭后的操作
        //});
        return;
    }
    //创建会话中间件
    connection.invoke('AddChatRecord', _receiverId).catch(err => console.log(err.toString()));

    //移除新消息提示
    let _$msgStatus = _this.find(".layim-msg-status");
    if (_$msgStatus.hasClass("msg-status-new")) {
        //console.info(_$msgStatus);
        //console.info(_$msgStatus.hasClass("msg-status-new"));
        _$msgStatus.removeClass("msg-status-new");
    }
})

//关闭聊天室
$("#chatRoomClose,.chatRoomClose").click(function (e) {
    $("#chatRoom").hide();
})

var oldClick = null;
$(".layim-list-friend>li>h5").click(function (e) {
    var _groupName = $(this).find("span").text();
    if (oldClick === _groupName) {
        $(this).parent().find("ul").hide().parent().siblings().find("ul").hide();
        oldClick = null;
        return;
    }
    $(this).parent().find("ul").show().parent().siblings().find("ul").hide();
    oldClick = _groupName;

})


$(document).on("click", ".tz-chat-messageHistory", function (e) {
    e.preventDefault();
    layer.msg('别急嘛，人家正在撸代码实现呢~');
})

$(document).on("click", ".chatTodo", function (e) {
    e.preventDefault();
    layer.msg('别急嘛，人家正在撸代码实现呢~');
})

$(".userConfigure").on("click", function (e) {
    e.preventDefault();
    layer.confirm('您确定要退出系统吗？', {
        btn: ['确定', '取消'] //按钮
    }, function () {
        window.location.href = "../Account/Logout";
    });
})

function reBodySize() {
    $("body").height($(window).height());
}
window.onresize = function () {
    reBodySize()
}
window.onload = function () {
    //初始化数据
    setTimeout(function () {
        init();
    }, 500)
    reBodySize();
}
$(".myFriendsPanel").css("left", "");