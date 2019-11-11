
//禁用所有的表单提交
document.querySelector('form').addEventListener('submit', function(e) {
    e.preventDefault();
}, false);

window.onload = function() {
    reBodySize();
    randomBackground();
}
window.onresize = function() {
    reBodySize();
}
setInterval(function() {
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
// document.querySelector('.layui-input').addEventListener('keyup', function(e) {
//     console.log(111);
//     var theEvent = e || window.event,
//         //浏览器兼容
//         code = theEvent.keyCode || theEvent.which || theEvent.charCode;
//     if (code === 13) {
//         theEvent.preventDefault();
//     }
// }, false);

var loginBtnState = {
    defaultShow: function() {
        $(".loginStatusBtn").hide();
        $(".loginBtn").show();
        $("form[name=LoginForm] input").removeAttr("disabled");
        $("#verificationCode").attr("data-disabled", "false");
    },
    statusShow: function() {
        $(".loginStatusBtn").show();
        $(".loginBtn").hide();
        $("form[name=LoginForm] input").attr("disabled", "disabled");
        $("#verificationCode").attr("data-disabled", "true");
    }
}


layui.use('form', function() {
    var form = layui.form;

    //监听登录提交
    form.on('submit(loginBtn)', function(data) {
        //验证码校验
        if (!verificationCode.valid()) return;
        loginBtnState.statusShow();
        var _$loginForm = $("form[name=LoginForm]"),
            _formData;

        _formData = _$loginForm.serializeArray(); //序列化表单
        var _data1 = _$loginForm.serialize(); //通过js原本的序列化获取表单数据(Url地址栏参数格式)
        var _data2 = JSON.stringify(data.field); //不需要通过js原本的序列化获取表单数据
        console.log(_data1);
        console.log(_data2);
        console.log(_formData);
        //临时跳转
        window.location.href = "../Chat/Index";
        // layer.msg('登录成功', {
        //     icon: 16,
        //     shade: 0.01
        // });
        return false;
    });
});