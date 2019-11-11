
var userMenu = {
    init: function () {
        userMenu.setActive(undefined);
        userMenu.defaultView();
        userMenu.listenClickEvent();
    },
    defaultView: function () {
        userMenu.loadPartialView({
            url: "../../Account/Index",
            target: false,
            ele: $(".userNavActive"),
            loadjs: "UserIndex"
        });
    },
    setActive: function ($navName) {
        window.localStorage.setItem("userMenuActive", $navName);
    },
    getActive: function () {
        let _userMenuActive = window.localStorage.getItem("userMenuActive");
        _userMenuActive = _userMenuActive === undefined ? "UserIndex" : _userMenuActive;
        return _userMenuActive;
    },
    loadPartialView: function (option) {
        let _option = {
            url: null,
            target: false,
            ele: null,
            loadjs: null
        },
            _$areaRightContainer = $(".area-right-container"),
            _$userRightHeadTitle = $(".user-right-head-title"),
            _userMenuActive = userMenu.getActive();
        _option = $.extend({}, _option, option);
        _$userRightHeadTitle.text($(_option.ele).text());
        if (!_option.target) {
            if (_option.loadjs !== _userMenuActive) {
                $.ajax({
                    url: _option.url,
                    async: false,
                    data: {},
                    success: function (html) {
                        _$areaRightContainer.html(html);
                        //初始化一些必要的数据
                        setTimeout(function () {
                            switch (_option.loadjs) {
                                case "UserIndex":
                                    userMenu.setActive(_option.loadjs);
                                    break;
                                case "Order":
                                    _renderOrderTable();
                                    userMenu.setActive(_option.loadjs);
                                    break;
                                case "Profile":
                                    userProFile.init();
                                    userMenu.setActive(_option.loadjs);
                                    break;
                            }
                        }, 50);
                    },
                    error: function () {
                        layer.msg("请求超时Ծ‸Ծ");
                    }
                });
            }
        } else window.location.href = _option.url;
    },
    listenClickEvent: function () {
        setTimeout(function () {
            $(".user-nav ul li a").off("click").on("click", function (e) {
                e.preventDefault();
                if ($(this).data("tzaction") === '') {
                    layer.msg('站长正在努力码代码实现呢Ծ‸Ծ');
                    return;
                }
                let _this = $(this),
                    option = {
                        url: _this.data("tzaction"),
                        target: _this.data("tztarget"),
                        ele: _this,
                        loadjs: _this.data("enname")
                    };
                userMenu.loadPartialView(option);
            })
        }, 50);
    },
    renderActive: function () {

    }
}
userMenu.init();


