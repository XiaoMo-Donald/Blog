var userProFile = {
    _$inputArea: $(".inputCACommonStyle"),
    _$inputTips: $(".tips"),
    _$saveBtnsArea: $("form[name=UserProfileForm] .userProfileForm-btns"),
    init: function () {
        userProFile.listenInputEditBtn();
        userProFile.listenEle();
    },
    initFormValid: function () {
        layui.use(['form'], function () {
            let form = layui.form;
            form.verify({
                nickname: function (value, item) { //value：表单的值、item：表单的DOM对象
                    if (value === '') {
                        return '昵称不能为空！';
                    }
                    // if (!new RegExp("^[a-zA-Z0-9_\u4e00-\u9fa5\\s·]+$").test(value)) {
                    //     return '昵称不能有特殊字符';
                    // }
                    // if (/(^\_)|(\__)|(\_+$)/.test(value)) {
                    //     return '昵称首尾不能出现下划线\'_\'';
                    // }
                    // if (/^\d+\d+\d$/.test(value)) {
                    //     return '昵称不能全为数字';
                    // }
                }

                //我们既支持上述函数式的方式，也支持下述数组的形式
                //数组的两个值分别代表：[正则匹配、匹配不符时的提示文字]
                ,
                tzUrl: function (value, item) {
                    if (value !== '') {
                        if (!new RegExp("javascript:").test(value)) {
                            let reg = /(http|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&:/~\+#]*[\w\-\@?^=%&/~\+#])?/;
                            if (!new RegExp("(http|https)://").test(value)) {
                                return '链接不正确，必须包含 http:// 或者 https://';
                            }
                            if (!reg.test(value)) {
                                return '链接不完整，请检查！';
                            }
                        }
                    }
                }
            });
        });
    },
    renderDate: function () {
        layui.use(['laydate'], function () {
            let laydate = layui.laydate;
            laydate.render({
                elem: '#birthday'
            });
        });
    },
    reStatus: function () {
        userProFile._$inputArea.addClass("inputCAStyle");
        userProFile._$inputArea.attr("disabled", "disabled");
        userProFile._$inputTips.hide();
        userProFile._$saveBtnsArea.removeClass("btnsShow");
    },
    initBtns: function () {
        userProFile.save();
        userProFile.cancel();
        userProFile.initFormValid();
    },
    save: function () {
        layui.use(['form'], function () {
            var form = layui.form,
                layer = layui.layer;
            form.on('submit(userProFileSaveBtn)', function (data) {
                layer.confirm('您确定要修改信息吗？', {
                    title: '提示',
                    btn: ['确定', '再考虑一下'],
                    btnAlign: 'c',
                }, function () {
                    //xss过滤
                    let _data = {
                        Nickname: filterXSS(data.field.Nickname),
                        Birthday: filterXSS(data.field.Birthday),
                        Location: filterXSS(data.field.Location),
                        QQ: filterXSS(data.field.QQ),
                        QQLink: filterXSS(data.field.QQLink),
                        WeiboLink: filterXSS(data.field.WeiboLink),
                        GithubLink: filterXSS(data.field.GithubLink),
                        ProSiteLink: filterXSS(data.field.ProSiteLink),
                        Remark: filterXSS(data.field.Remark)
                    };
                    $.ajax({
                        url: "../../Account/SaveChangeProfile",
                        type: "post",
                        data: _data,
                        success: function (res) {
                            // 处理
                            if (res.state) {
                                userProFile.reStatus();
                                layer.msg('保存成功', { icon: 1 });
                                return;
                            }
                            layer.msg(res.message);
                        },
                        error: function () {
                            layer.msg("请求超时Ծ‸Ծ");
                        }
                    });
                });
                return false;
            });
        })
    },
    cancel: function () {
        $(".userProFileCancelBtn").off("click").on("click", function (e) {
            e.preventDefault();
            layer.confirm('您确定不保存当前修改吗？', {
                title: '提示',
                btn: ['确定', '再考虑一下'],
                btnAlign: 'c',
            }, function (index) {
                userProFile.reStatus();
                //TODO:重新加载数据

                layer.close(index);
            });
        })
    },
    listenEle: function () {
        userProFile._$inputArea = $(".inputCACommonStyle");
        userProFile._$inputTips = $(".tips");
        userProFile._$saveBtnsArea = $("form[name=UserProfileForm] .userProfileForm-btns");
    },
    listenInputEditBtn: function () {
        $(".inputEdit").on("click", function (e) {
            e.preventDefault();
            let _$this = $(this),
                _$inputArea = _$this.parent().prev().find(".inputCACommonStyle"),
                _$inputTips = _$this.parent().next().find(".tips"),
                _hasClass = _$inputArea.hasClass("inputCAStyle"),
                _$saveBtnsArea = $("form[name=UserProfileForm] .userProfileForm-btns");
            if (_hasClass) {
                _$inputTips.show();
                _$inputArea.removeClass("inputCAStyle");
                _$inputArea.removeAttr("disabled");
                userProFile.renderDate();
                _$inputArea.focus();
            }
            if (!_$saveBtnsArea.hasClass("btnsShow")) {
                _$saveBtnsArea.addClass("btnsShow");
                userProFile.initBtns();
            }
        })
    }
}
