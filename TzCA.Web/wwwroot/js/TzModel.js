
//打开模态框
function tzModelOpen($option) {
    var option = {
        content: '模态框内容，可以使用Html元素',
        title: "标题",
        width: '450px',
        height: '500px',
        func: function ($index) {
            layer.close($index);
        }
    };
    option = $option;
    var _modelHtml = '<div class="tzModelContent">' + option.content + '</div>';
    layui.use(['layer'], function () {
        var layer = layui.layer;
        layer.open({
            type: 1,
            title: option.title,
            closeBtn: 1,
            area: [option.width, option.height],
            fixed: false, // 不固定
            maxmin: false,
            //skin: 'layui-layer-nobg', //没有背景色
            btn: ['确定', '取消'],
            shadeClose: true,
            content: _modelHtml,
            yes: function (index) {
                option.func(index);
            },
            end: function () {
                console.log("退出了模态框");
            }
        });
        //$(".tzModelContent").parent().next().next().css("text-align", "center");
    })
}