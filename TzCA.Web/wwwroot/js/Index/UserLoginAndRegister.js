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
window.onload = function () {
    reBodySize();
    randomBackground();
    registerBtnState.listenInputHelper();
    new WOW().init();
    verificationToken = $("#formAntiForgeryToken").html();
    $(".loginFormAntiForgeryToken").html(verificationToken);
}
window.onresize = function () {
    reBodySize();
}
setInterval(function () {
    randomBackground();
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

function reBodySize() {
    var h = $(window).height(),
        _$loginAndRegisterArea = $("#loginAndRegisterArea"),
        _loginAndRegisterAreaTop;
    _$loginAndRegisterArea.fadeIn();
    _loginAndRegisterAreaTop = h / 2 - _$loginAndRegisterArea.height() / 2;
    $("body").height(h - _loginAndRegisterAreaTop);

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
                $(this).css("color", "#333");
            }).mouseleave(function () {
                $(this).css("color", "#777");
            })
            $(".passwordHelper>i").mouseenter(function () {
                layer.tips('密码强度', '.passwordHelper', {
                    id: 'Tz_passwordHelperTips'
                });
                layer.msg('密码说明<br/>长度最少6位，且包含数字、字母 如：demo123', {
                    time: 10000,
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
    termsOfUseTpl = {
        // 网站使用条款模板
        content: '<div style="padding: 50px; line-height: 22px; background-color: #393D49; color: #fff; font-weight: 300;">' +
            '1.使用条款xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx' +
            '<br><br>' +
            '2.使用条款xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx' +
            '<br><br>' +
            '3.使用条款xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx' +
            '<br><br>' +
            '4.使用条款xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx' +
            '<br><br>' +
            '5.使用条款xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx' +
            '</div>',
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
        reBodySize();
        this.show();
    },
    backLogin: function () {
        registerBtnState.defaultShow();
        this.allArea.height(380);
        reBodySize();
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
                            time: 0 //不自动关闭
                            , btn: ['立即注册', '不了']
                            , yes: function (index) {
                                layer.close(index);
                                //执行跳转注册页面方法
                                //layer.msg("TODO:跳转注册页面", { icon: 6 });
                                registerArea.go();
                            }
                        });
                        return;
                    }
                    layer.msg("登录失败：" + data.message, { icon: 5 });
                    //layer.msg("登录失败" + data.message, {
                    //    icon: 16,
                    //    shade: 0.01
                    //});
                    return;
                }
                window.location.href = data.reUrl;
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
                    time: 0 //不自动关闭
                    , btn: ['立即登录', '不了']
                    , yes: function (index) {
                        layer.close(index);
                        //执行跳转登录页面方法
                        registerArea.backLogin();
                    }
                });
                registerBtnState.defaultShow();
                registerBtnState.clearData();
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
            area: '500px;',
            shade: 0.8,
            id: 'Tz_termsOfUsePro', //设定一个id，防止重复弹出            
            btn: ['已阅读并同意', '不同意'],
            btnAlign: 'c',
            moveType: 1, //拖拽模式，0或者1            
            content: termsOfUseTpl.content,
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

$(".randomNickname").on("click", function (e) {
    e.preventDefault();
    $.get("../Account/RandomNickname", {}, function (data) {
        $("input[name=Nickname]").val(data);
    })
});

$(".forgetPassword").on("click", function (e) {
    e.preventDefault();
    layer.msg('别急嘛，人家正在撸代码实现呢~');
});

