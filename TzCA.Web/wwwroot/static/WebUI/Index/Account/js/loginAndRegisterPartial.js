//禁用所有的表单提交
function preventFormDefault() {
    // document.querySelector('form').addEventListener('submit', function(e) {
    //     e.preventDefault();
    // }, false);
    $('form').on('submit', function (e) {
        e.preventDefault();
    });
}
var verificationToken = null;


window.onresize = function () {
    reLoginAndRegisterAreaSize();
}
setInterval(function () {
    //randomBackground();
}, 10000);

function randomBackground() {
    var w = $(window).width(),
        h = $(window).height(),
        imagePath = "../static/WebUI/Index/images/",
        backgroundList = ["bg_01.jpg", "bg_02.jpg", "bg_03.jpg"],
        index = random(0, 2);
    $("body").css({
        // "background": "url('" + imagePath + backgroundList[0] + "') no-repeat " + w + "px " + h + "px"
        "background-image": "url('" + imagePath + backgroundList[index] + "')",
        "background-size": "100% 100%",
        "background-repeat": "no-repeat"
    })
}

function reLoginAndRegisterAreaSize() {
    var _winH = $(window).height(),
        _$loginAndRegisterArea = $("#loginAndRegisterArea"),
        _loginAndRegisterAreaTop;
    _$loginAndRegisterArea.fadeIn();
    _loginAndRegisterAreaTop = (_winH - 100) / 2 - _$loginAndRegisterArea.height() / 2 - 80;
    _loginAndRegisterAreaTop = _loginAndRegisterAreaTop < 10 ? 10 : _loginAndRegisterAreaTop;
    _$loginAndRegisterArea.css(
        "margin-top", _loginAndRegisterAreaTop
    )
}

var tzEventsListen = {
    goToRegister: function () {
        $(".goToRegister").on("click", function (e) {
            e.preventDefault();
            registerArea.go();
        });
    },
    backLogin: function () {
        $(".backLogin").on("click", function (e) {
            e.preventDefault();
            registerArea.backLogin();
        });
    },
    all: function () {
        this.goToRegister();
        this.backLogin();
    },
}
tzEventsListen.all();
var loginBtnState = {
    loginStatusBtn: $(".loginStatusBtn"),
    loginBtn: $(".loginBtn"),
    loginForm: $("form[name=LoginForm] input"),
    verificationCode: $(".loginVerificationCode"),
    goToRegister: $(".goToRegisterTab"),
    clearData: function () {
        this.loginForm.val("");
        verificationCode.onDraw(show_num);
        listVerificationCodeEvent();
    },
    defaultShow: function () {
        this.loginStatusBtn.hide();
        this.loginBtn.show();
        this.loginForm.removeAttr("disabled");
        this.verificationCode.attr("data-disabled", "false");
        tzEventsListen.goToRegister();
    },
    statusShow: function () {
        this.loginStatusBtn.show();
        this.loginBtn.hide();
        this.loginForm.attr("disabled", "disabled");
        this.verificationCode.attr("data-disabled", "true");
        this.goToRegister.off("click");
    }
},
    registerBtnState = {
        registerStatusBtn: $(".registerStatusBtn"),
        registerBtn: $(".registerBtn"),
        registerForm: $("form[name=RegisterForm] input"),
        verificationCode: $(".registerVerificationCode"),
        backLogin: $(".backLoginTab"),
        randomNicknameBtn: $(".randomNickname"),
        agreeTermsOfUse: false,
        clearData: function () {
            this.registerForm.val("");
            verificationCode.onDraw(show_num);
            listVerificationCodeEvent();
        },
        defaultShow: function () {
            this.registerStatusBtn.hide();
            this.registerBtn.show();
            this.registerForm.removeAttr("disabled");
            this.verificationCode.attr("data-disabled", "false");
            tzEventsListen.backLogin();
            this.randomNicknameBtn.removeAttr("disabled");
            listVerificationCodeEvent();
        },
        statusShow: function () {
            this.registerStatusBtn.show();
            this.registerBtn.hide();
            this.registerForm.attr("disabled", "disabled");
            this.verificationCode.attr("data-disabled", "true");
            this.backLogin.off("click");
            this.randomNicknameBtn.attr("disabled", "disabled");
        },
        listenInputHelper: function () {
            $(".usernameHelper>i").mouseenter(function () {
                layer.tips('用于登录', '.usernameHelper', {
                    id: 'Tz_usernameHelperTips'
                });
                layer.msg('用户名说明<br/>长度最多16位，由纯数字字母或数字+字母组成<br/> 如：123456、demouser、user123', {
                    time: 20000,
                    btnAlign: 'c',
                    id: 'Tz_usernameHelper',
                    btn: ['确定']
                });
                $(this).css("color", "#333");
            }).mouseleave(function () {
                $(this).css("color", "#777");
            })
            $(".passwordHelper>i").mouseenter(function () {
                layer.tips('用于登录', '.passwordHelper', {
                    id: 'Tz_passwordHelperTips'
                });
                layer.msg('密码说明<br/>长度最少6位，由纯数字字母或者数字+字母组成<br/> 如：123456、abcqwe、abc123<p style="color:#faf062;">为了安全，请不要使用以上示例密码！</p>', {
                    time: 20000,
                    btnAlign: 'c',
                    id: 'Tz_passwordHelper',
                    btn: ['确定']
                });
                $(this).css("color", "#333");
            }).mouseleave(function () {
                $(this).css("color", "#777");
            })
        }
    },
    termsOfUseTpl = function () { // 网站使用条款模板   
        var _html = '';
        $.ajax({
            url: "../../Home/TermsAndConditions",
            type: "get",
            async: false,
            dataType: "html",
            success: function (html) {
                _html = html;
            }
        })
        return _html;
    };

//注册表单相关
var registerArea = {
    allArea: $(".loginAndRegisterArea"),
    loginArea: $(".loginArea"),
    registerArea: $(".registerArea"),
    loginVerificationCode: $(".loginVerificationCode"),
    loginVerificationCodeInput: $(".loginVerificationCodeInput"),
    registerVerificationCodeInput: $("registerVerificationCodeInput"),
    loginAvatar: $(".avatar"),
    registerTitle: $(".registerTitle"),
    loginFormAntiForgeryToken: $(".loginFormAntiForgeryToken"),
    registerFormAntiForgeryToken: $(".registerFormAntiForgeryToken"),
    show: function () {
        this.loginArea.hide();
        this.registerArea.fadeIn();
        this.loginVerificationCodeInput.attr("name", "");
        this.loginVerificationCode.attr("id", "");
        loginBtnState.clearData();
        verificationCode.onDraw(show_num); //刷新验证码
        this.loginAvatar.removeClass("wow zoomInDown");
        this.loginAvatar.removeAttr("style");
        this.loginFormAntiForgeryToken.html("");
        this.registerFormAntiForgeryToken.html(verificationToken);
    },
    hide: function () {
        this.registerArea.hide();
        this.loginArea.fadeIn();
        this.loginVerificationCodeInput.attr("name", "VerificationCode");
        this.loginVerificationCode.attr("id", "verificationCode");
        registerBtnState.clearData();
        this.registerTitle.removeClass("wow flipInX");
        this.registerTitle.removeAttr("style");
        this.loginFormAntiForgeryToken.html(verificationToken);
        this.registerFormAntiForgeryToken.html("");
    },
    go: function () {
        loginBtnState.defaultShow();
        this.allArea.height(470);
        reLoginAndRegisterAreaSize();
        this.show();
    },
    backLogin: function () {
        registerBtnState.defaultShow();
        this.allArea.height(380);
        reLoginAndRegisterAreaSize();
        this.hide();
    }
}


layui.use(['form', 'element'], function () {
    var form = layui.form,
        element = layui.element;

    //监听登录提交
    form.on('submit(loginBtn)', function (loginData) {
        preventFormDefault();
        //验证码校验
        if (!verificationCode.valid()) return;
        loginBtnState.statusShow();
        var _$loginForm = $("form[name=LoginForm]");

        //var _formData = _$loginForm.serializeArray(); //序列化表单
        //var _data1 = _$loginForm.serialize(); //通过js原本的序列化获取表单数据(Url地址栏参数格式)
        //var _data2 = JSON.stringify(loginData.field); //不需要通过js原本的序列化获取表单数据 //转为字符串

        //登录提交
        $.ajax({
            url: "../Account/Login",
            type: "post",
            data: loginData.field,
            dataType: "json",
            success: function (data, textStatus) {
                if (!data.state) {
                    loginBtnState.defaultShow();
                    if (data.goRegister) {
                        layer.msg(data.message, {
                            time: 0,
                            btn: ['立即注册', '不了'],
                            btnAlign: 'c',
                            yes: function (index) {
                                layer.close(index);
                                registerArea.go();
                            }
                        });
                        return;
                    }
                    layer.msg("登录失败：" + data.message, { icon: 5 });
                    return;
                }
                window.location.href = data.reUrl;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                loginBtnState.defaultShow();
                tzAjaxStatus(XMLHttpRequest.status);
            }
        })
        return false;
    });

    //监听注册提交
    form.on('submit(registerBtn)', function (registerData) {
        preventFormDefault();
        if (!registerBtnState.agreeTermsOfUse) {
            layer.msg('注册前请阅读并同意使用协议！', { icon: 5 });
            return;
        }
        var _formData = registerData.field;
        if (_formData.Password !== _formData.ConfirmPassword) {
            layer.msg('两次密码输入不相同！', { icon: 5 });
            return;
        }
        if (!verificationCode.valid()) {
            return;
        };
        registerBtnState.statusShow();
        var _$registerForm = $("form[name=RegisterForm]");

        //var _formData = _$registerForm.serializeArray(); //序列化表单
        var _data1 = _$registerForm.serialize(); //通过js原本的序列化获取表单数据(Url地址栏参数格式)
        var _data2 = JSON.stringify(registerData.field); //不需要通过js原本的序列化获取表单数据

        _formData.Nickname = filterXSS(_formData.Nickname);
        _formData.UserName = filterXSS(_formData.UserName);

        //注册提交
        $.ajax({
            url: "../Account/Register",
            type: "post",
            data: _formData,
            dataType: "json",
            beforeSend: function () { },
            success: function (data, textStatus) {
                if (!data.state) {
                    registerBtnState.defaultShow();
                    layer.msg("注册失败：" + data.message, { icon: 5 });
                    return;
                }
                //注册成功提示
                layer.msg(data.message, {
                    time: 0,
                    btn: ['立即登录', '稍后登录'],
                    btnAlign: 'c',
                    yes: function (index) {
                        layer.close(index);
                        //执行跳转登录页面方法
                        registerBtnState.clearData();
                        registerArea.backLogin();
                    }
                });
                registerBtnState.defaultShow();
            },
            error: function () { //错误

            }
        })

        return false;
    });

    form.on('checkbox(termsOfUse)', function (data) {
        layer.open({
            type: 1,
            title: '<div class="termsOfUseTitle">《云盟服务条款》，用户必读</div>', //不显示标题栏            
            closeBtn: false,
            area: '100%',
            shade: 0.8,
            id: 'Tz_termsOfUsePro', //设定一个id，防止重复弹出            
            btn: ['已阅读并同意', '不同意'],
            btnAlign: 'c',
            moveType: 1, //拖拽模式，0或者1            
            content: termsOfUseTpl(),
            success: function (layero) {
                var btn = layero.find('.layui-layer-btn');
                var _$agreeBtn = btn.find('.layui-layer-btn0');
                var _$disagreeBtn = btn.find('.layui-layer-btn1');
                _$agreeBtn.on("click", function (e) {
                    e.preventDefault();
                    data.othis.addClass("layui-form-checked");
                    $(data.elem).attr("checked", true);
                    registerBtnState.agreeTermsOfUse = true;
                })
                _$disagreeBtn.on("click", function (e) {
                    e.preventDefault();
                    data.othis.removeClass("layui-form-checked");
                    $(data.elem).removeAttr("checked");
                    registerBtnState.agreeTermsOfUse = false;
                })
            }
        });
    });

});

$("form[name=LoginForm] input[name=Username]").on("change", function () {
    $.get("../Account/GetLoginAvatar", { username: $(this).val() }, function (data) {
        if (data.path === "") $(".avatar").attr("src", "../images/chatAvatars/defaultAvatar.jpg");
        else $(".avatar").attr("src", data.path);
    })
})

var randomNicknameTimeoutCount = 0;
const timeoutCount = 2;
//var randomNicknameTimeout = setInterval(randomNicknameFunc(), 1000);
var randomNicknameTimeout = null;
var randomNicknameFunc = function () {
    if (randomNicknameTimeoutCount === 0) {
        clearInterval(randomNicknameTimeout);
        return;
    }
    else
        randomNicknameTimeoutCount -= 1;
    console.log(randomNicknameTimeoutCount);
}

$(".randomNickname").off("click").on("click", function (e) {
    e.preventDefault();
    if (randomNicknameTimeoutCount !== 0) {
        layer.msg("操作太快了，请" + timeoutCount + "秒后再获取");
        return;
    }
    $.get("../Account/RandomNickname", {}, function (data) {
        $("input[name=Nickname]").val(data);
        randomNicknameTimeoutCount = timeoutCount;
        randomNicknameTimeout = setInterval(randomNicknameFunc, 1000);
    })
});

$(".forgetPassword").on("click", function (e) {
    e.preventDefault();
    layer.msg('别急嘛，人家正在撸代码实现呢~');
});

function loginInit() {
    reLoginAndRegisterAreaSize();
    //randomBackground();
    registerBtnState.listenInputHelper();
    verificationToken = $("#formAntiForgeryToken").html();
    $(".loginFormAntiForgeryToken").html(verificationToken);
}
loginInit();

//抓取从注册链接传过来的值 如果为Register 则是显示注册界面
$(function () {
    var $state = GetQueryString("state");
    if ($state !== null && $state.toString().length > 1 && $state === "Register") {
        registerArea.go();
        //设置页面导航激活
        window.localStorage.setItem("navAcitve", "userRegister");
        //刷新
        loadActiveNav();
        $("input[name=Nickname]").focus();
    } else {
        //设置页面导航激活
        window.localStorage.setItem("navAcitve", "userLogin");
        //刷新
        loadActiveNav();
        $("input[name=Username]").focus();
        return;
    }
})
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r !== null) return decodeURI(r[2]); return null;
}