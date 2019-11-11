var tzCloudUsersAndTeams = {
    init: function () {
        window.onload = function () {
            tzCloudUsersAndTeams.renderUsers({
                index: 1,
                limit: 21
            });
            tzCloudUsersAndTeams.renderUsersPaging();
        }
    },
    usersTotal: function () {
        let _count = 0;
        $.ajax({
            url: "../../TzData/AllUsersCount",
            type: "get",
            async: false,
            success: function (data) {
                _count = data;
            }
        })
        return _count;
    },
    renderUsers: function (paginationInput) {
        let loadingIndex = top.layer.msg('加载中，请稍候', { icon: 16, time: false, shade: 0.8 });
        $.ajax({
            url: "../../TzData/AllUsers",
            type: "get",
            async: true,
            data: { index: paginationInput.curr, limit: paginationInput.limit },
            success: function (res) {
                let _delay = 0,
                    _delayStr = "",
                    _tpl = "";
                top.layer.close(loadingIndex);
                $.each(res.data, function (index, val) {
                    let _userLinks = "";
                    _delayStr = _delay + "s";
                    $.each(val.userLinks, function (lIndex, lVal) {
                        _userLinks += '<a href="' + lVal.link + '" target="' + lVal.target + '" title="' + lVal.name + '">' + lVal.name + '</a>';
                    })
                    _tpl += '<li class="tz-cloudTeam-item wow bounceInDown"  data-wow-duration="1s" data-wow-delay="' + _delayStr + '">' +
                        '   <img src="' + val.minAvatar + '" alt="头像" title="头像">' +
                        '   <a href="javascript:" title="访问个人主页">个人主页</a>' +
                        '   <h2>' + val.nickname + '</h2>' +
                        '   <h3>认证信息:<span class="team-member-authentication">'+val.authentication+'</span></h3>' +
                        '   <div class="team-member-links">' + _userLinks + '</div>' +
                        '</li>';
                    _delay += 0.2;
                });
                $(".tz-cloudTeam-list").html("");
                $(".tz-cloudTeam-list").html(_tpl);
            },
            error: function () {
                top.layer.close(loadingIndex);
                layer.msg("请求超时Ծ‸Ծ");
            }
        });
        //设置页面导航激活
        window.localStorage.setItem("navAcitve", "tzNavCAUsers");
        //刷新
        loadActiveNav();
    },
    renderUsersPaging: function () {
        //渲染用户列表分页
        layui.use(['laypage', 'layer'], function () {
            var laypage = layui.laypage,
                layer = layui.layer;

            //完整功能(分页)
            laypage.render({
                elem: 'tzCloudTeamPagingArea',
                count: tzCloudUsersAndTeams.usersTotal(),
                theme: '#1E9FFF',
                layout: ['count', 'prev', 'page', 'next', 'refresh', 'skip'],
                //layout: ['count', 'prev', 'page', 'next', 'limit', 'refresh', 'skip'],
                limit: 21,//默认每页数量21                
                jump: function (obj, first) {
                    if (!first) {
                        tzCloudUsersAndTeams.renderUsers(obj);
                    }
                }
            });
        })
    },
    toolFullNum: function ($num) {
        return $num < 10 ? "0" + $num : $num;
    }
}
tzCloudUsersAndTeams.init();