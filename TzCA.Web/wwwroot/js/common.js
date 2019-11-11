
//topbar.show(); //显示进度条
//$(document).ready(function () {
//    topbar.hide(); //隐藏进度条
//})
$(function () {
    new WOW().init();

    //$("img.lazyload").lazyload();

    //插件初始化和使用Demo

    //时间日期插件：datetimepicker
    //<input type="text" value="2018/07/30 12:30:05" title="时间" id="taskRemindTime" class="btn btn-default task-remindTime" />
    //$("input[name=remindTime]").datetimepicker();


    //分页插件：pagination
    //<div class="task-list-pagination"></div>
    //$(".task-list-pagination").pagination({
    //    totalData: 1000,
    //    showData: 5,
    //    pageCount: 5,
    //    jump: true,
    //    coping: true,
    //    homePage: '首页',
    //    endPage: '末页',
    //    prevContent: '上页',
    //    nextContent: '下页',
    //    callback: function (api) {
    //        //console.log(api.getCurrent());
    //        //TODO
    //    }
    //});

    //防xss注入插件：xss
    //filterXSS(值)


    //消息提醒插件：toastr
    toastr.options.positionClass = 'toast-bottom-right';
    //toastr.success("提示消息"); //还有很多的方法：消息、警告


    //弹窗插件：layer
    //查看官网：http://layer.layui.com/

    //表单校验
    //$("form").validate(); //初始化
    //if (!form.valid()) return; //校验开始（表单元素必须加上require


    //封装一个获取表单数据为json对象的扩展 //调用：$("form").serializeJson();
    (function (window, $) {
        $.fn.serializeJson = function () {
            var serializeObj = {};
            var array = this.serializeArray();
            var str = this.serialize();
            $(array).each(
                function () {
                    if (serializeObj[this.name]) {
                        if ($.isArray(serializeObj[this.name])) {
                            serializeObj[this.name].push(this.value);
                        } else {
                            serializeObj[this.name] = [
                                serializeObj[this.name], this.value
                            ];
                        }
                    } else {
                        serializeObj[this.name] = this.value;
                    }
                });
            return serializeObj;
        };
    })(window, jQuery);

    function consoleInit() {
        console.log('%c 小白者（Me）:UI框架王、插件王、复制粘贴王...', 'color:#009688');
        console.log('%c 小莫云联盟-网络工作室（www.925i.cn）', 'color:#01AAED');
        console.log('%c 二颜：小莫云联盟工作室创始人。', 'color:#FF5722');
        console.log('%c =========================================', 'color:#FFB800');
        console.log('%c 二颜（ErYan）小菜鸡程序猿', 'color:#393D49');
    }
    consoleInit();
})

$(document).on("mouseenter", ".main-right", function (e) {
    e.preventDefault();
    $('html,body').animate({ scrollTop: 0 }, 300);
    setTimeout(function () {
        bodyScroll.unScroll();
    }, 300);
})

$(document).on("mouseleave", ".main-right", function (e) {
    e.preventDefault();
    setTimeout(function () {
        bodyScroll.removeUnScroll();
    }, 300);
})

var bodyScroll = {
    unScroll: function () {
        var top = $(document).scrollTop();
        $(document).on('scroll.unable', function (e) {
            $(document).scrollTop(top);
        })
    },
    removeUnScroll: function () {
        $(document).unbind("scroll.unable");
    },
    hidden: function () {
        document.body.parentNode.style.overflow = "hidden";
    },
    hiddenX: function () {
        document.body.parentNode.style.overflowX = "hidden";
    },
    hiddenY: function () {

    },
    show: function () {
        document.body.parentNode.style.overflow = "auto";
    },
    showX: function () {
        document.body.parentNode.style.overflowX = "auto";
    }
}

var tzTimeAgoFormat = {
    supplementZero: function (number) {
        return number < 10 ? "0" + number : number;
    },
    renderBlogArticleTimeAgo: function (date) {
        var _blogDTimeAgo = null;
        layui.use(['util'], function () {
            var util = layui.util;
            var _date = new Date(date),
                y = _date.getFullYear(),
                M = _date.getMonth(),
                d = _date.getDate(),
                H = _date.getHours(),
                m = _date.getMinutes(),
                s = _date.getSeconds(),
                _weekday = _date.getDay(),
                _weekdays = ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"],
                _agoStr = util.timeAgo(new Date(y, M || 0, d || 1, H || 0, m || 0, s || 0));
            _blogDTimeAgo = y + '-' + tzTimeAgoFormat.supplementZero(M + 1) + '-' + tzTimeAgoFormat.supplementZero(d) + ' ' + tzTimeAgoFormat.supplementZero(H) + ':' + tzTimeAgoFormat.supplementZero(m) + ':' + tzTimeAgoFormat.supplementZero(s) + '&nbsp;&nbsp;' + _weekdays[_weekday] + "&nbsp;&nbsp;" + _agoStr;
        })
        return _blogDTimeAgo;
    }
}

//ajax状态码判断
function tzAjaxStatus(code) {
    let _msg = "";
    switch (code) {
        case 401: _msg = "未授权，请先登录系统Ծ‸Ծ"; break; //未授权
        case 403: _msg = "请求错误，请反馈Ծ‸Ծ"; break; //403
        case 404: _msg = "请求数据不存在，请反馈Ծ‸Ծ"; break; //404
        case 500: _msg = "系统内部错误，请反馈Ծ‸Ծ"; break; //系统错误
    }
    layer.msg(_msg);
}

//默认数据
var tzDefaultDatas = {
    guid: function () {
        return "00000000-0000-0000-0000-000000000000";
    }
}

//工具
var tzTools = {
    fullNum: function ($num) {
        return $num < 10 ? "0" + $num : $num;
    }
}

////尝试获取浏览器cookies
//function getTzCookies() {
//    //var _cookise = $.cookie("testCookies");
//    console.log($.tzCookies.get("testCookies"));
//}
//setTimeout(function () {
//    getTzCookies();
//}, 3000)

//$.tzCookies = {
//    get: function (n) {
//        var m = document.cookie.match(new RegExp("(^| )" + n + "=([^;]*)(;|$)"));
//        return !m ? "" : unescape(m[2]);
//    }
//}

////测试cooies过期
//$(document).on("click", ".more", function () {
//    getTzCookies();
//})