//这里书写用户中心的脚本代码
(function () {
    var $userNavList = $(".user-nav ul").children();
    $userNavList.each(function (e, index) {
        $(this).click(function () {
            $(this).addClass("userNavActive").siblings().removeClass("userNavActive");
            $(this).find("a").addClass("cloudAllianceThemeColor").parent().siblings().find("a").removeClass("cloudAllianceThemeColor");
        })
    });

})(jQuery)