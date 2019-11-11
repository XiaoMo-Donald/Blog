
//内容区域布局
var mainLayout = {
    $mainLeft: $(".main-left"),
    $mainRight: $(".main-right"),
    default: function () {

    },
    one: function () {
        mainLayout.$mainLeft.css({
            "float": "right",
            "marign-left": 10 + "px"
        });
        mainLayout.$mainLeft.css({
            "marign-left": 0
        });
    }
};




//用于判断是否已经开启了新纪元 false为未开启 true为开启
var _loadingIsActive = window.localStorage.getItem("loadingIsActive");
//用于判断是否已经打开了弹幕发布窗口
var barragerShow = false;

bodyScroll.hidden(); //默认隐藏滚动条

if (!_loadingIsActive) {
    _loadingIsActive = "false";
    window.localStorage.setItem('loadingIsActive', false);
    bodyScroll.hidden(); //隐藏滚动条
}

//弹幕信息窗口居中js 开始
$(window).resize(function () {
    tc_center();
    reLoadMainSize();
});

function tc_center() {
    var _top = ($(window).height() - $("#barrager_Content").height()) / 2;
    var _left = ($(window).width() - $("#barrager_Content").width()) / 2;

    $("#barrager_Content").css({ top: _top, left: _left });
}
//弹幕信息窗口居中js 结束

//重置Load层高度
function reLoadMainSize() {
    var _w = $(window).width(),
        _h = $(window).height(),
        _headH = 120,// $(".head").height()
        _footH = 300,// $("footer").height()
        _$loadMain = $("#loadMain"),
        _$main = $("#main"),
        _$loading = $(".loading"),
        _loadMainH = _h - (_headH + _footH);
    if (window.localStorage.getItem("loadingIsActive") === "false") {
        var _loadingH = _loadMainH / 2 - _$loading.height() / 2; //60px
        _loadingH = _loadingH < 61 ? 61 : _loadingH;
        _$loading.css({
            "margin-top": _loadingH
        });
    }

    _$loadMain.height(_loadMainH);
    if (window.localStorage.getItem("loadingIsActive") === "false") _$main.height(_loadMainH - 100);
}
reLoadMainSize();

////弹幕信息窗口可移动 js开始
$(document).ready(function () {

    $(".b_Content_title").mousedown(function (e) {
        $(this).css("cursor", "move"); //改变鼠标指针的形状
        var offset = $(this).offset(); //DIV在页面的位置
        var x = e.pageX - offset.left; //获得鼠标指针离DIV元素左边界的距离
        var y = e.pageY - offset.top; //获得鼠标指针离DIV元素上边界的距离
        $(document).bind("mousemove", function (ev) { //绑定鼠标的移动事件，因为光标在DIV元素外面也要有效果，所以要用doucment的事件，而不用DIV元素的事件

            $("#barrager_Content").stop(); //加上这个之后

            var _x = ev.pageX - x; //获得X轴方向移动的值
            var _y = ev.pageY - y; //获得Y轴方向移动的值

            //$("#barrager_Content").animate({left:_x+"px",top:_y+"px"},5);

            if (_y > $(window).height() - 40 || _y < 100) {
                tc_center();
                $(this).unbind("mousemove");
            } else {
                $("#barrager_Content").animate({ left: _x + "px", top: _y + "px" }, 1);
            }

        });

    });
    $(document).mouseup(function () {
        $("#barrager_Content").css("cursor", "default");
        $(this).unbind("mousemove");
    });
})
//弹幕信息窗口可移动 js结束

String.prototype.format = function (args) {
    var result = this;
    if (arguments.length < 1) {
        return result;
    }
    var data = arguments;
    if (arguments.length === 1 && typeof (args) === "object") {
        data = args;
    }
    for (var key in data) {
        var value = data[key];
        if (undefined !== value) {
            result = result.replace("{" + key + "}", value);
        }
    }
    return result;
}
var barrager_code =
    'var item={\n' +
    "   img:'{img}', //图片 \n" +
    "   info:'{info}', //文字 \n" +
    "   href:'{href}', //链接 \n" +
    "   close:{close}, //显示关闭按钮 \n" +
    "   speed:{speed}, //延迟,单位秒,默认6 \n" +
    "   bottom:{bottom}, //距离底部高度,单位px,默认随机 \n" +
    "   color:'{color}', //颜色,默认白色 \n" +
    "   old_ie_color:'{old_ie_color}', //ie低版兼容色,不能与网页背景相同,默认黑色 \n" +
    " }\n" +
    "$('body').barrager(item);";

function run() {

    var info = $('input[name=info]').val();
    (info === '') ? info = '请填写弹幕文字' : info = info;
    var href = $('input[name=href]').val();
    //var  speed=parseInt($('input[name=speed]').val());
    var speed = 6;
    //var  bottom=parseInt($('input[name=bottom]').val());
    var bottom = 0;
    var code = barrager_code;
    if ($('input:radio[name=bottomradio]:checked').val() === 0) {
        var window_height = $(window).height() - 150;
        bottom = Math.floor(Math.random() * window_height + 40);
        code = code.replace("   bottom:{bottom}, //距离底部高度,单位px,默认随机 \n", '');

    }

    //var  img=$('input:radio[name=img]:checked').val();
    var img = "barrager.gif";
    if (img === 'none') {
        code = code.replace("   img:'{img}', //图片 \n", '');
    }
    var item = {
        'img': 'images/' + img,
        'info': info,
        'href': href,
        'close': true,
        'speed': speed,
        'bottom': bottom,
        'color': '#' + $('input[name=color]').val(),
        'old_ie_color': '#' + $('input[name=color]').val()
    };

    //if(!$('input[name=close]').is(':checked')){
    //    item.close=false;
    //}
    code = code.format(item);
    eval(code);
}



function ShowMiniInfoCar() {

    //右边浮动资料卡显示事件
    $("#aboutXiaoMo").css({ "right": "0px", "box-shadow": "0px 0px 6px rgba(255,255,255,0.8)" });
    $("#aboutXiaoMoMini").css({ "right": "-40px" })

    //右侧迷你资料卡 js代码结束
}

//$(function () {
//发个弹幕试试 js代码开始
//显示
$("#barrager_Hidden").click(function () {
    $("#barrager_Botton").css({ "width": "155px", "transition": "width 0.8s" });
    $("#barragerShow").css({ "width": "100%", "transition": "width 1s" });
    $("#barragerShow img").css({ "left": "5px", "transition": "left 1.3s" });
    $("#barrager_Show").css({ "display": "block" });
    $("#barrager_Hidden").css({ "display": "none" });
})

//隐藏
$("#barrager_Botton").mouseleave(function () {
    $("#barrager_Botton").css({ "width": "10px", "transition": "width 0.8s" });
    $("#barragerShow").css({ "width": "10px", "transition": "width 1s" });
    $("#barragerShow img").css({ "left": "-50px", "transition": "left 1.3s" });
    $("#barrager_Show").css({ "display": "none" });
    $("#barrager_Hidden").css({ "display": "block" });
})

//图片360度Y轴旋转
$("#barragerShow img").mouseenter(function () {
    $("#barragerShow img").css({ "transform": "rotateY(360deg)", "transition": "transform 1s" });
})
$("#barragerShow img").mouseleave(function () {
    $("#barragerShow img").css({ "transform": "rotateY(0deg)", "transition": "transform 1s" });
})

//显示填弹幕信息页面
$("#barragerShow").click(function () {
    if (window.localStorage.getItem("loadingIsActive") === "true") {
        if (!barragerShow) {
            barragerShow = true;
            //弹幕信息窗口居中显示
            $("#barrager_Content").css({ "opacity": "1", "z-index": "999", "transition": "opacity 1s" });
            tc_center();
            return false;
        } else {
            alert("弹幕窗口已经打开！");
            return false;
        }
    } else {
        alert("很抱歉，请开启新纪元后再尝试操作！");
        return false;
    }
})
//关闭填弹幕信息页面
$("#b_Content_title_close").click(function () {
    barragerShow = false;
    $("#barrager_Content").css({ "opacity": "0", "z-index": "-1", "transition": "opacity 1s" });
})


//发个弹幕试试 js代码结束


var tzSiteWow = {
    header: $("header"),
    logo: $(".logo"),
    logoImg: $(".logo a img"),
    nav: $("#nav"),
    navItem: $("#nav ul").children(),
    footer: $(".footer").parent(),
    remove: function () {
        this.header.removeClass("wow fadeInDown");
        this.header.css("visibility", "initial");
        this.logo.removeClass("wow bounceIn");
        this.logoImg.removeClass("wow flip");
        this.nav.removeClass("wow flipInX");
        this.footer.removeClass("wow fadeInUp");
        $.each(this.navItem, function (index, ele) {
            $(ele).removeClass("wow flipInY");
        })
    },
    add: function () {
        this.header.addClass("wow fadeInDown");
        this.header.css("visibility", "hidden");
        this.logo.addClass("wow bounceIn");
        this.logoImg.addClass("wow flip");
        this.nav.addClass("wow flipInX");
        this.footer.addClass("wow fadeInUp");
        $.each(this.navItem, function (index, ele) {
            $(ele).addClass("wow flipInY");
        })
    }

}

//开发中 默认隐藏新纪元遮罩层
if (window.localStorage.getItem("loadingIsActive") === "true") {
    intoCloudAlliance();
}

var loadingAudio = $("#loadingAudio")[0];
$("#loading").fadeIn();
//鼠标经过默认遮罩的时候文字上滑   
$("#loading").mouseenter(function () {
    loadingAudio.play();
    $("#loadingMark").css({ "top": "0px", "transition": "top 0.2s" });
    $(".loadingNow").css({ "transform": "rotateY(360deg)", "transition": "transform 0.5s" });
})
//反之 则隐藏
$("#loading").mouseleave(function () {
    loadingAudio.pause();
    loadingAudio.currentTime = 0
    $("#loadingMark").css({ "top": "100px", "transition": "top 0.5s" });
    $(".loadingNow").css({ "transform": "rotateY(-360deg)", "transition": "transform 0.5s" });
})

//点击初始化的遮罩层 进入主页面
$(".loadingNow").click(function () {
    intoCloudAlliance();
})

//进入次元的核心方法
function intoCloudAlliance() {
    bodyScroll.show(); //显示滚动条
    tzSiteWow.remove();
    window.localStorage.setItem('loadingIsActive', true);
    window.localStorage.setItem('loadingIsActive', true);
    var _$loadMain = $("#loadMain"),
        _$main = $("#main");
    $("section").removeClass("sectionBGHidden");
    _$main.css({
        "height": "880px",
        "transition": "height 1s"
    });
    _$loadMain.css({ "top": -(_$loadMain.height() + 220), "transition": "top 1s" });
    //runCloud();
    $("#aboutXiaoMoMini").fadeIn();
}


//主导航
$(document).on("click", ".tzLink", function () {
    var _this = $(this);
    if (_this.data("tzlinkto") === '') {
        layer.msg('站长正在努力码代码实现呢Ծ‸Ծ');
        return;
    }
    if (window.localStorage.getItem("loadingIsActive") === "true") {
        var _$navItem = $(".nav ul").children(),
            _$userLoginStateNavItem = $(".userLoginState").children();
        if (_this.parent().hasClass("userLoginState")) {
            $.each(_$navItem, function (index, ele) {
                $(ele).find("a").removeClass("navActive");
            })
            _this.addClass("navActive").siblings().removeClass("navActive");
        }
        else {
            $.each(_$userLoginStateNavItem, function (index, ele) {
                $(ele).removeClass("navActive");
            })
            _this.find("a").addClass("navActive").parent().siblings().find("a").removeClass("navActive");
        }
        intoCloudAlliance();
        window.localStorage.setItem("navAcitve", _this.data("tzlink"));
        if (_this.data("tzlink") === "userLogin" || _this.data("tzlink") === "userRegister")
            window.open(_this.data("tzlinkto"));
        else
            window.location.href = _this.data("tzlinkto");
    }
    else {
        layer.msg('很抱歉，请开启新纪元后再尝试操作！\n\n请问您确定现在开启站长的新纪元吗？', {
            time: 0,
            btn: ['立即开启', '撤退'],
            btnAlign: 'c',
            yes: function (index) {
                layer.close(index);
                layer.msg('新纪元成功开启 O.o', { icon: 6 });
                window.localStorage.setItem("navAcitve", _this.data("tzlink"));
                intoCloudAlliance();
            }
        });
    }
});

//右侧迷你资料卡 js代码开始
//右边浮动资料卡日期
var $dateNow = new Date();
var $year = $dateNow.getFullYear();
var $month = $dateNow.getMonth() + 1;
var $day = $dateNow.getDate();
var $week = $dateNow.getDay();
var $weekday = '';
if ($week === 0) $weekday = '星期日';
else if ($week === 1) $weekday = '星期一';
else if ($week === 2) $weekday = '星期二';
else if ($week === 3) $weekday = '星期三';
else if ($week === 4) $weekday = '星期四';
else if ($week === 5) $weekday = '星期五';
else if ($week === 6) $weekday = '星期六';
$("#aTimeNow").html('今天是:  ' + $year + '年' + $month + '月' + $day + '日' + ' ' + $weekday);

//右边浮动资料卡显示事件
$("#aboutXiaoMoMini").click(function () {
    $("#aboutXiaoMo").css({ "right": "0px", "box-shadow": "0px 0px 6px #ccc" });
    $("#aboutXiaoMoMini").css({ "right": "-40px" })
})

//右边浮动资料卡关闭事件
$("#aClose").click(function () {
    $("#aboutXiaoMo").css({ "right": "-300px", "box-shadow": "none" });
    $("#aboutXiaoMoMini").css({ "right": "0px" })
})

//右侧迷你资料卡 js代码结束

//用户头像和迷你资料卡的头像旋转效果360度js代码开始
$("#userAvatarDeg").mouseenter(function () {
    $(this).css({ "transform": "rotate(360deg)", "transition": "transform 0.8s" })
});
$("#userAvatarDeg").mouseleave(function () {
    $(this).css({ "transform": "rotate(0deg)", "transition": "transform 1s" })
})

$("#miniAvatarDeg").mouseenter(function () {
    $(this).css({ "transform": "rotate(360deg)", "transition": "all 0.8s" })
});
$("#miniAvatarDeg").mouseleave(function () {
    $(this).css({ "transform": "rotate(0deg)", "transition": "all 1s" })
})

//用户头像和迷你资料卡的头像旋转效果360度js代码结束


//顶部导航栏特效

//foot底部版权日期js代码开始
$("#copyDate").html($year);

//foot底部版权日期js代码结束

//main主页内容左侧 Y轴旋转模块
//切换到用户的的信息 单击事件
$("#tabMenu_User").click(function () {
    $("#main-left").css({ "border-radius": "0px 5px 5px 0px", "transform": "rotateY(180deg)", "transition": "all 0.5s" });
    $("#xiaoMoShow").css({ "opacity": "0", "display": "none", "transition": "all 1s" });
    $("#userShow").css({ "opacity": "1", "display": "block", "transition": "all 1s" });
});

//切换到站长的信息 单击事件
$("#tabMenu_XiaoMo").click(function () {
    //旋转回站长的信息卡
    $("#main-left").css({ "border-radius": "5px 0px 0px 5px", "transform": "rotateY(0deg)", "transition": "all 0.5s" });
    $("#xiaoMoShow").css({ "opacity": "1", "display": "block", "transition": "all 1s" });
    $("#userShow").css({ "opacity": "0", "display": "none", "transition": "all 1s" });
});

function hasLogin() {
    $.get("../../Account/GetUserHasLogin", {}, function (data) {
        var _$avatarALink = $(".userLoginStateAvatar a"),
            _$avatar = $(".userLoginStateAvatar a .avatar"),
            _$minAvatar = $("#miniAvatarDeg"),
            _$userLoginState = $("#userLoginState"),
            _$login = $(".login"),
            _$hasLoginTpl = '<a href="javascript:" class="cloudAllianceNotices" style="font-size: 12px;" title="我的消息">' +
                '<i class="layui-icon layui-icon-notice cloudAllianceNotice wow" data-wow-iteration="5" data-wow-duration="0.15s" data-wow-delay="0s" >' +
                '<span class="layui-badge-dot cloudAllianceNoticeDot"></i></span></a>' +
                '&nbsp;丨&nbsp;<a class="tzLink" data-tzlink="userCenter" data-tzlinkto="../../Account/UserCenter" href="javascript:" title="个人中心" id="loginShow">个人中心</a>' +
                '&nbsp;丨&nbsp;<a href="javascript:" title="退出次元" id="userLogout">退出次元</a>',
            _$notLoginTpl = '<a class="tzLink" data-tzlink="userLogin" data-tzlinkto="../../Account/Login" href="javascript:" title="登录享受更多特权" id="loginShow">登录</a>' +
                '&nbsp;丨&nbsp;<a class="tzLink" data-tzlink="userRegister"  data-tzlinkto="../../Account/Login?state=Register"  href="javascript:" title="没有账号？点我注册" id="registerShow">注册</a>';
        if (data.state) {
            window.localStorage.setItem('loadingIsActive', true);
            _$avatar.attr("src", data.avatar);
            _$minAvatar.attr("src", data.avatar);
            _$userLoginState.html(_$hasLoginTpl);
            _$avatarALink.attr("href", "../../Account/UserCenter");
            _$login.fadeIn();
            return;
        }
        _$avatar.attr("src", "/images/chatAvatars/defaultAvatar.jpg");
        _$minAvatar.attr("src", "/images/chatAvatars/defaultAvatar.jpg");
        _$userLoginState.html(_$notLoginTpl);
        _$avatarALink.attr("href", "../../Account/LoginAndRegister");
        _$login.fadeIn();
    })
}
hasLogin();
$(document).on("click", "#userLogout", function (e) {
    e.preventDefault();
    layer.confirm('您确定要退出系统吗？', {
        title: "注销提示",
        btn: ['确定', '取消'],
        shift: 6,
        btnAlign: 'c',
    }, function () {
        window.localStorage.setItem('loadingIsActive', false);
        window.location.href = "../Account/Logout";
    });
})
//});


//main主页内容浮动的云朵js代码开始
function runCloud() {
    setInterval(function () {
        $(".main_yun1")
            .css({ "z-index": "0" })
            .fadeIn('1000')
            .animate({ left: '860' }, 20000, 'linear')
            .animate({ left: '0' }, 20000, 'linear');
        $(".main_yun2")
            .css({ "z-index": "0" })
            .fadeIn('1000')
            .animate({ right: '750' }, 20000, 'linear')
            .animate({ right: '0' }, 20000, 'linear');

        $(".main_yun3")
            .css({ "z-index": "0" })
            .fadeIn('1000')
            .animate({ left: '660' }, 15000, 'linear')
            .animate({ left: '0' }, 15000, 'linear');
    }, 0);
}
//main主页内容浮动的云朵js代码结束



//加载导航激活状态
function loadActiveNav() {
    var _hasActiveNav = window.localStorage.getItem("navAcitve"),
        _isLink = false,
        _links = $(".tzLink"),
        _$navItem = $(".nav ul").children(),
        _$userLoginStateNavItem = $(".userLoginState a");
    _hasActiveNav = _hasActiveNav === undefined ? "tzNavIndex" : _hasActiveNav;

    $.each(_links, function (index, ele) {
        if ($(ele).data("tzlink") === _hasActiveNav) {
            if ($(ele).parent().hasClass("userLoginState")) {
                if (_hasActiveNav === "userRegister") $(".userLoginState a:last-child").css("padding-right", "5px");
                $(ele).addClass("navActive").siblings().removeClass("navActive");
                $.each(_$navItem, function (index, navEle) {
                    $(navEle).find("a").removeClass("navActive");
                })
            }
            else {
                $(ele).addClass("wow rubberBand").find("a").addClass("navActive").parent().siblings().removeClass("wow rubberBand").find("a").removeClass("navActive");
                $.each(_$userLoginStateNavItem, function (index, navEle) {
                    $(navEle).removeClass("navActive");
                })
            }
            _isLink = true;
            return;
        }
    })
    if (!_isLink)
        $(".tzLink").eq(0).find("a").addClass("navActive");
}
setTimeout(function () {
    loadActiveNav();
}, 1000);

layui.use(['laydate'], function () {
    var laydate = layui.laydate;
    //直接嵌套显示
    var ins1 = laydate.render({
        elem: '#tzIndexDate',
        position: 'static',
        theme: 'grid',
        calendar: true,
        btns: ['now'],
        mark: {
            '0-6-30': '大学毕业', //每年6月30日
            '0-9-25': '站长生日', //每年9月25日  
            '2018-12-30': '预发',//如果为空字符，则默认显示数字+徽章
            '2019-2-22': '发布'
        },
        done: function (value, date) {
            //if (date.year === 2017 && date.month === 8 && date.date === 15) { 
            //    //点击2017年8月15日，弹出提示语
            //    ins1.hint('中国人民抗日战争胜利72周年');
            //}
            if (date.month === 6 && date.date === 30) {
                let tzYear = 2018,
                    tzYearCount = date.year - tzYear, tzYearStr = "";
                if (tzYearCount > 0) {
                    tzYearStr = tzYearCount + "周年。";
                } else {
                    tzYearStr = "。";
                }
                ins1.hint('站长大学毕业' + tzYearStr);
            }
            if (date.month === 9 && date.date === 25) {
                ins1.hint('站长生日，举杯庆祝^_^');
            }
        }
    });
})

//背景图片
var tzCloudAllianceBackground = {
    images: [
        "../../../../images/backgrounds/tz_bg_001.jpg",
        "../../../../images/backgrounds/tz_bg_002.jpg",
        "../../../../images/backgrounds/tz_bg_003.jpg",
        "../../../../images/backgrounds/tz_bg_004.jpg",
        "../../../../images/backgrounds/tz_bg_005.jpg",
        "../../../../images/backgrounds/tz_bg_006.jpg",
        "../../../../images/backgrounds/tz_bg_007.jpg",
        "../../../../images/backgrounds/tz_bg_008.jpg",
        "../../../../images/backgrounds/tz_bg_009.jpg",
        "../../../../images/backgrounds/tz_bg_010.jpg",
        "../../../../images/backgrounds/tz_bg_011.jpg",
        "../../../../images/backgrounds/tz_bg_012.jpg",
        "../../../../images/backgrounds/tz_bg_013.jpg",
        "../../../../images/backgrounds/tz_bg_014.jpg",
        "../../../../images/backgrounds/tz_bg_015.jpg"
    ],
    get: function () {
        let tzCAbg = window.localStorage.getItem("tzCloudAllianceBackground");
        if (!tzCAbg) {
            tzCAbg = "../../../../images/backgrounds/tz_bg_003.jpg";
        }
        $("section").css({
            "background-image": "url(" + tzCAbg + ")"
        })
    },
    set: function ($url) {
        $("section").css({
            "background-image": "url(" + $url + ")"
        })
        window.localStorage.setItem("tzCloudAllianceBackground", $url);
    }
},
    tzCloudAlliance = {
        init: function () {
            tzCloudAlliance.runTime();
        },
        site: null,
        getSite: function () {
            $.ajax({
                url: "../../TzData/Site",
                type: "get",
                async: true,
                success: function (res) {


                    tzCloudAlliance.site = res;
                }
            })
        },
        runTime: function ($runTime) {
            let _runTime = $runTime || "2018/09/01 00:00:01" || tzCloudAlliance.site.runTime;
            var date1 = new Date(_runTime); //开始时间
            setInterval(function () {
                var date2 = new Date(); //结束时间
                var date3 = date2.getTime() - date1.getTime(); //时间差的毫秒数

                //计算出相差天数
                var days = Math.floor(date3 / (24 * 3600 * 1000));
                //计算出小时数
                var leave1 = date3 % (24 * 3600 * 1000); //计算天数后剩余的毫秒数
                var hours = Math.floor(leave1 / (3600 * 1000));
                //计算相差分钟数
                var leave2 = leave1 % (3600 * 1000); //计算小时数后剩余的毫秒数
                var minutes = Math.floor(leave2 / (60 * 1000));
                //计算相差秒数
                var leave3 = leave2 % (60 * 1000); //计算分钟数后剩余的毫秒数
                var seconds = Math.round(leave3 / 1000);
                var timeResult = days + "天 " + hours + "小时 " + minutes + "分钟 " + tzTools.fullNum(seconds) + "秒";
                $(".cloudAllianceRunTime>span").text(timeResult);
            }, 1000);
        }
    };
tzCloudAllianceBackground.get();
tzCloudAlliance.init();


