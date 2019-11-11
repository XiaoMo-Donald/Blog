
//消息推送
//配置链接
const notificationConnection = new signalR.HubConnectionBuilder()
    .withUrl('/tzNotificationHub')
    .configureLogging(signalR.LogLevel.Information)
    .build();

//连接之前，可能需要监听用户登录
//启用链接
function startNotificationConnection() {
    $.ajax({
        url: "../../Account/GetUserHasLogin",
        type: "get",
        async: false,
        success: function (res) {
            if (res.state) notificationConnection.start().catch(err => consle.error(err.toString()));
        }
    })
}
startNotificationConnection();

//定义消息接收
notificationConnection.on("Receive", (res) => {
    //toastr.success("提示消息：" + res);
    if (res !== null) {
        layer.open({
            title: '新通知提醒',
            content: '您收到了一条新的通知',
            time: 10000, //10s后自动关闭
            offset: 'rb',
            btn: [],
            shade: false,
            anim: 2,
            resize: false
        });
    }
    //铃铛震动
    $(".cloudAllianceNotice").addClass("shake");
    setTimeout(function () {
        $(".cloudAllianceNotice").removeClass("shake");
    }, 5000)
    //如果模态框打开，添加最新的消息到列表
    let _$fillAppendArea = $(".oQNoticesList");
    if (_$fillAppendArea !== undefined) {
        if (res !== null) {
            let _unReadNotificationTpl = ' <li class="oQNoticeItem">' +
                ' <a href="javascript:">' +
                '     <i class="layui-icon layui-icon-log oQNoticeItemLogIcon"></i>' +
                '      <div class="oQNoticeItem-top">' +
                '         <div class="oQNoticeUserInfo">' +
                '              <img class="oQNoticeUserAvatar" src="' + res.sender.minAvatar + '" alt="头像">' +
                '            <h3 class="oQNoticeUserNickname">来自：<span class="oQNoticeNickname">' + res.sender.nickname + '</span></h3>' +
                '       </div>' +
                '    </div>' +
                '    <div class="oQNoticeItem-content" title="">' + res.content + ' </div>' +
                '  <div class="oQNoticeItem-bottom">' +
                '    <span class="oQNoticeType">消息来源：<span class="oQNoticeItemType">' + res.source + '</span></span>' +
                '  <span class="oQNoticeItemTime">' + tzTimeAgoFormat.renderBlogArticleTimeAgo(res.sendTime) + '</span>' +
                '     </div>' +
                '   </a>' +
                '  </li>',
                _$fillBeforeArea = $(".oQNoticesList li:first-child");

            if (_$fillBeforeArea !== undefined)
                _$fillBeforeArea.before(_unReadNotificationTpl)
            else
                _$fillAppendArea.append(_unReadNotificationTpl);

            notification.getUnReadCount(); //刷新消息统计          
            notification.renderContentSourceSubStr(); //重新渲染截取的字符串
        }
    }
})


//定义消息对象
var notification = {
    send: function (option) {
        let $option = {
            objectId: '',
            content: '',
            contentSource: '',
            receiverId: '',
            source: ''
        };
        $option = $.extend({}, $option, option);
        notificationConnection.invoke('Send', {
            ObjectId: $option.objectId,
            ReceiverId: $option.receiverId,
            Content: $option.content,
            ContentSource: $option.contentSource,
            Source: $option.source
        }).catch(err => console.log(err.toString()));
    },
    sendAll: function (option) {
        notificationConnection.invoke('SendSendAll', {
            //TODO:参数
        }).catch(err => console.log(err.toString()));
    },
    modal: function () {
        layui.use(['form', 'laydate'], function () {
            let layer = layui.layer,
                laydate = layui.laydate;
            layer.open({
                type: 1,
                title: '未读消息通知（<span class="unReadCount"></span>条)',
                id: 'OverallSituationCloudAllianceNoticeArea',
                shadeClose: true,
                resize: false,
                scrollbar: false,
                area: ['500px', '575px'],
                content: '<div class="overallSituationCloudAllianceNoticeArea" id="overallSituationCloudAllianceNoticeArea"><ul class="oQNoticesList"></ul></div>'
            });
            //notification.getDefaultData(); //不需要页面加载了
            notification.getFlowData();
            notification.getUnReadCount();
        })
    },
    getDefaultData: function () {
        $.ajax({
            url: "../../Notification/UnReadModalData",
            type: 'get',
            async: false,
            success: function (html) {
                $("#overallSituationCloudAllianceNoticeArea").html(html);

                //重新渲染时间
                var _sendTimes = $(".oQNoticesList").find(".oQNoticeItemTime");
                if (_sendTimes !== undefined) {
                    $.each(_sendTimes, function (index, ele) {
                        $(ele).html(tzTimeAgoFormat.renderBlogArticleTimeAgo($(ele).data("sendtime")));
                    })
                }
            }
        })
    },
    getFlowData: function () {
        layui.use('flow', function () {
            var $ = layui.jquery;
            var flow = layui.flow;
            flow.load({
                isAuto: true,
                elem: '.oQNoticesList', //指定列表容器
                done: function (page, next) {
                    var _list = [];
                    $.get('../../Notification/GetUnRead?page=' + page, function (res) {
                        layui.each(res.data, function (index, item) {
                            let _unReadNotificationTpl = ' <li class="oQNoticeItem">' +
                                ' <a href="javascript:">' +
                                '     <i class="layui-icon layui-icon-log oQNoticeItemLogIcon"></i>' +
                                '      <div class="oQNoticeItem-top">' +
                                '         <div class="oQNoticeUserInfo">' +
                                '              <img class="oQNoticeUserAvatar" src="' + item.sender.minAvatar + '" alt="头像">' +
                                '            <h3 class="oQNoticeUserNickname">来自：<span class="oQNoticeNickname">' + item.sender.nickname + '</span></h3>' +
                                '       </div>' +
                                '    </div>' +
                                '    <div class="oQNoticeItem-content">' + item.description + ' </div>' +
                                '  <div class="oQNoticeItem-bottom">' +
                                '    <span class="oQNoticeType">消息来源：<span class="oQNoticeItemType">' + item.source + '</span></span>' +
                                '  <span class="oQNoticeItemTime">' + tzTimeAgoFormat.renderBlogArticleTimeAgo(item.createTime) + '</span>' +
                                '     </div>' +
                                '   </a>' +
                                '  </li>';
                            _list.push(_unReadNotificationTpl);
                        });
                        next(_list.join(''), page < res.count);
                        notification.renderContentSourceSubStr(); //重新渲染截取的字符串     
                    });
                }
            });
        });
    },
    getUnReadCount: function () {
        let _unReadCount = 0,
            _$cloudAllianceNotice = $(".cloudAllianceNotice"),
            _dotTpl = '<span class="layui-badge-dot cloudAllianceNoticeDot"></span>';
        $.ajax({
            url: "../../Notification/GetUnReadCount",
            type: "get",
            async: false,
            success: function (data) {
                _unReadCount = data;
            },
            error: function () {
                layer.msg("请求超时Ծ‸Ծ");
            }
        })
        if (_unReadCount > 0) _$cloudAllianceNotice.html(_dotTpl);
        else {
            _$cloudAllianceNotice.html("");
        }
        $(".unReadCount").text(_unReadCount);
        return _unReadCount;
    },
    renderContentSourceSubStr: function () {
        setTimeout(function () {
            let _$contentSourceEles = $(".oQNoticesList").find("span.contentSource"),
                _$contentSourceUnSubEles = $(".oQNoticesList").find("span.contentSourceUnSub");
            $.each(_$contentSourceEles, function (index, ele) {
                let _content = "",
                    _subContent = "",
                    _emoji = "",
                    _emojiStr = "",
                    _val = "";
                _content = $(ele).attr("title");
                _emoji = tzEmoji.toolGetCode(_content);//取出表情          
                $.each(_emoji, function (index, val) {
                    if (index > 3) return;
                    _emojiStr += val;
                })
                _subContent = tzEmoji.toolReplaceCode(_content);  //取出文本         
                _subContent = notification.toolSubStr(_subContent, 12);//不带表情
                //_subContent = notification.toolSubStr(_subContent, 6);//带表情
                //_val = _emojiStr + _subContent; //带表情
                _val = _subContent;
                $(ele).text(_val);
            });
            $.each(_$contentSourceUnSubEles, function (index, ele) {
                $(ele).text(notification.toolSubStr($(ele).attr("title"), 22));
            });
            notification.renderNotificationEmoji();
        }, 100);
    },
    toolSubStr: function (content, length) {
        let _oldContent = content,
            _result = ''; 
        if (content !== undefined) {
            if (content.length <= length)
                _result = content;
            else
                _result = _oldContent.substring(0, length) + "...";
        }
        return _result;
    },
    renderNotificationEmoji: function () {
        tzEmoji.parse({
            iconPath: "../lib/",
            source: ".oQNoticeItem-content span"
        });
    }
};


//单击消息铃铛
$(document).on("click", ".cloudAllianceNotices", function (e) {
    e.preventDefault();
    notification.modal();
})

