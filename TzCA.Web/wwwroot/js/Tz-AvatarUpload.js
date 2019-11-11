//头像裁剪上传
//弹出框水平垂直居中
(window.onresize = function () {
    var win_height = $(window).height();
    var win_width = $(window).width();
    if (win_width <= 768) {
        $(".tailoring-content").css({
            "top": (win_height - $(".tailoring-content").outerHeight()) / 2,
            "left": 0
        });
    } else {
        $(".tailoring-content").css({
            "top": (win_height - $(".tailoring-content").outerHeight()) / 2,
            "left": (win_width - $(".tailoring-content").outerWidth()) / 2
        });
    }
})();


$(document).on("click", "#editAvatar,.editAvatarBtn", function () {
    $(".tailoring-container").toggle();
});

//图像上传
function selectImg(file) {
    if (!file.files || !file.files[0]) {
        return;
    }
    var reader = new FileReader();
    reader.onload = function (evt) {
        var replaceSrc = evt.target.result;
        //更换cropper的图片
        $('#tailoringImg').cropper('replace', replaceSrc, false); //默认false，适应高度，不失真
    }
    reader.readAsDataURL(file.files[0]);
}
//cropper图片裁剪
$('#tailoringImg').cropper({
    aspectRatio: 1 / 1, //默认比例
    preview: '.previewImg', //预览视图
    guides: false, //裁剪框的虚线(九宫格)
    autoCropArea: 0.5, //0-1之间的数值，定义自动剪裁区域的大小，默认0.8
    dragCrop: true, //是否允许移除当前的剪裁框，并通过拖动来新建一个剪裁框区域
    movable: true, //是否允许移动剪裁框
    resizable: true, //是否允许改变裁剪框的大小
    zoomable: false, //是否允许缩放图片大小
    mouseWheelZoom: false, //是否允许通过鼠标滚轮来缩放图片
    touchDragZoom: true, //是否允许通过触摸移动来缩放图片
    rotatable: true, //是否允许旋转图片
    crop: function (e) {
        // 输出结果数据裁剪图像。
    }
});
//旋转
$(".cropper-rotate-btn").on("click", function () {
    $('#tailoringImg').cropper("rotate", 45);
});
//复位
$(".cropper-reset-btn").on("click", function () {
    $('#tailoringImg').cropper("reset");
});
//换向
var flagX = true;
$(".cropper-scaleX-btn").on("click", function () {
    if (flagX) {
        $('#tailoringImg').cropper("scaleX", -1);
        flagX = false;
    } else {
        $('#tailoringImg').cropper("scaleX", 1);
        flagX = true;
    }
    flagX !== flagX;
});

//裁剪后的处理
$("#sureCut").on("click", function () {
    if ($("#tailoringImg").attr("src") === null) {
        return false;
    } else {
        var cas = $('#tailoringImg').cropper('getCroppedCanvas'); //获取被裁剪后的canvas   
        if (cas === null) {
            layer.msg("请选择一张图片");
            return;
        }
        var base64url = cas.toDataURL('image/png'); //转换为base64地址形式
        $("#finalImg").prop("src", base64url); //显示为图片的形式
        var blobImg = convertBase64UrlToBlob(base64url);
        var formData = new FormData();
        var nameImg = new Date().getTime() + '.png';
        formData.append("file", blobImg, nameImg);

        //执行服务器上传
        uploadAvatar(formData)
    }
});
//关闭裁剪框
function closeTailor() {
    $(".tailoring-container").toggle();
}

function uploadAvatar($file) {
    var file = $file;
    $.ajax({
        url: "../../Account/SaveChangeAvatar",
        type: "post",
        data: file,
        contentType: false, //必须关掉
        processData: false, //必须关掉
        beforeSend: function () {
            $("#sureCut").hide();
            $("#sureCuting").show();
        },
        success: function (data) {
            if (data.state) {
                $(".avatar").attr("src", data.url);

                $("#sureCut").show();
                $("#sureCuting").hide();

                //成功
                layer.msg(data.message);
                //关闭裁剪框
                closeTailor();
            }
        },
        error: function () {
            $("#sureCut").show();
            $("#sureCuting").hide();
        }
    })
}

function convertBase64UrlToBlob(urlData) {
    var bytes = window.atob(urlData.split(',')[1]); //去掉url的头，并转换为byte 
    //处理异常,将ascii码小于0的转换为大于0 
    var ab = new ArrayBuffer(bytes.length);
    var ia = new Uint8Array(ab);
    for (var i = 0; i < bytes.length; i++) {
        ia[i] = bytes.charCodeAt(i);
    }
    return new Blob([ab], {
        type: 'image/png'
    });
}

//系统头像代码
//设置选中状态
$(document).on("click", ".tzUserAvatars li", function () {
    //$(this).find("img").addClass("selectedAvatar").parent().parent().siblings().find("img").removeClass("selectedAvatar");
    $(this).addClass("selectedAvatar").siblings().removeClass("selectedAvatar");
})

//加载用户头像
$(document).on("click", ".systemAvatarBtn", function (e) {
    if ($(".systemAvatars").children().length > 0) return;
    var _$this = $(this);
    _avatarHtml = '<div class="tzModel" id="tzModel"><div class="tzModelBody"><form name="AvatarForm"><ul class="tzUserAvatars">',
        _eleId = null;
    $.ajax({
        url: "../../TzData/DefaultAvatars",
        async: false,
        type: 'get',
        success: function (data) {
            $.each(data, function (index, val) {
                _eleId = "AvatarSrc" + index.toString();
                _avatarHtml += '<li>'
                    + '<label for="' + _eleId + '"><img class="lazyload" width="100" height="100"  data-original= "' + val + '" ></label>'
                    + '<input type="radio" class="avatarSrc" id="' + _eleId + '" name="AvatarSrc" value="' + val + '"/></li>';
            })
            _avatarHtml += '</ul></form></div>'
                + '<div class="tzModelFoot">'
                + ' <button class="l-btn layui-btn-normal tzModelSaving" disabled="disabled"> <i class="loginLoading layui-icon layui-icon-loading layui-icon layui-anim layui-anim-rotate layui-anim-loop"></i>保存中...</button>'
                + ' <button class="l-btn layui-btn-normal tzModelSave"> 保存 </button>'
                + ' <button class="tzModelCancel"> 取消 </button>'
                + '</div></div>';

            $(".systemAvatars").html(_avatarHtml);
            //tzModelOpen({
            //    content: _avatarHtml,
            //    title: '修改头像（系统头像）',
            //    width: '550px',
            //    height: '500px',
            //    func: function ($index) {
            //        var _$avatarForm = $("form[name=AvatarForm]"),
            //            _src = _$avatarForm.find("input[type=radio]:checked").val();
            //        $.post("../../Account/SaveChangeUserAvatar", { src: _src }, function (res) {
            //            if (res.state) {
            //                //_$this.find("img").attr("src", _src);
            //                $(".avatar").attr("src", _src);
            //                layer.msg(res.message);
            //                layer.close($index);
            //            } else layer.msg(res.message, function () { });
            //        })
            //    }
            //});
            $("img.lazyload").lazyload({
                placeholder: "../../images/tzLoading/tz_loading_005.gif", //用图片提前占位
                //effect: "fadeIn"
            });
        }
    })
})

$(document).on("click", ".tzModelSave", function (e) {
    var _$this = $(this),
        _$avatarForm = $("form[name=AvatarForm]"),
        _src = _$avatarForm.find("input[type=radio]:checked").val();
    $.ajax({
        type: "post",
        url: "../../Account/SaveChangeUserAvatar",
        data: { src: _src },
        beforeSend: function () {
            _$this.hide();
            $(".tzModelSaving").show();
        },
        success: function (res) {
            if (res.state) {
                $(".avatar").attr("src", _src);

                $(".tzModelSaving").hide();
                _$this.show();

                layer.msg(res.message);
                //关闭裁剪框
                closeTailor();
            } else {
                $(".tzModelSaving").hide();
                _$this.show();
                layer.msg(res.message, function () { });
            }
        },
        error: function () {
            $(".tzModelSaving").hide();
            _$this.show();
        }
    })
})
$(document).on("click", ".tzModelCancel", function (e) {
    closeTailor();
})