function renderArticlePage() {
    layui.use(['form', 'layer', 'layedit', 'laydate', 'upload'], function () {
        var form = layui.form
        layer = parent.layer === undefined ? layui.layer : top.layer,
            laypage = layui.laypage,
            upload = layui.upload,
            layedit = layui.layedit,
            laydate = layui.laydate,
            $ = layui.jquery;

        //创建一个编辑器
        var editIndex = layedit.build('news_content', {
            height: 535,
            //height: 482,
            uploadImage: {
                url: "../../AdminData/UploadBlogArticleImage"
            }
        });

        //用于同步编辑器内容到textarea
        layedit.sync(editIndex);

        //上传缩略图(上传封面Cover)
        upload.render({
            elem: '.thumbBox',
            url: '../../AdminData/UploadBlogArticleImage',
            method: "post",
            done: function (res, index, upload) {
                console.log(res);
                if (res.code === 0) {
                    $('.thumbBox img').attr('src', res.data.src);
                    $('.thumbBox').css("background", "#fff");
                } else layer.msg(res.msg);
            }
        });

        //格式化时间
        function filterTime(val) {
            if (val < 10) {
                return "0" + val;
            } else {
                return val;
            }
        }
        //定时发布
        var time = new Date();
        var submitTime = time.getFullYear() + '-' + filterTime(time.getMonth() + 1) + '-' + filterTime(time.getDate()) + ' ' + filterTime(time.getHours()) + ':' + filterTime(time.getMinutes()) + ':' + filterTime(time.getSeconds());
        laydate.render({
            elem: '#release',
            type: 'datetime',
            trigger: "click",
            done: function (value, date, endDate) {
                submitTime = value;
            }
        });
        form.on("radio(release)", function (data) {
            if (data.elem.title === "定时发布") {
                $(".releaseDate").removeClass("layui-hide");
                $(".releaseDate #release").attr("lay-verify", "required");
            } else {
                $(".releaseDate").addClass("layui-hide");
                $(".releaseDate #release").removeAttr("lay-verify");
                submitTime = time.getFullYear() + '-' + (time.getMonth() + 1) + '-' + time.getDate() + ' ' + time.getHours() + ':' + time.getMinutes() + ':' + time.getSeconds();
            }
        });

        form.verify({
            articleName: function (val) {              
                if (val === '') {
                    return "文章标题不能为空";
                }
            },
            articleContent: function (val) {   
                // layedit.sync(editIndex); 似乎无法同步数据
                if (layedit.getText(editIndex) === '') {
                    return "文章内容不能为空";
                }
            }
        })



        form.on("submit(addBlogArticle)", function (data) {

            //截取文章内容中的一部分文字放入文章摘要
            var abstract = layedit.getText(editIndex).substring(0, 250);
            //弹出loading
            var index = top.layer.msg('数据提交中，请稍候', { icon: 16, time: false, shade: 0.8 });
            // 实际使用时的提交信息
            $.post("../../AdminData/AddBlogArticle", {
                name: $(".articleName").val(),  //文章标题
                abstract: $(".abstract").val() === "" ? abstract : $(".abstract").val(),  //文章摘要
                description: layedit.getContent(editIndex).split('<audio controls="controls" style="display: none;"></audio>')[0],  //文章内容
                thumbnail: $(".thumbImg").attr("src"),  //缩略图
                //classify : '1',    //文章分类
                //newsStatus : $('.newsStatus select').val(),    //发布状态
                //newsTime : submitTime,    //发布时间
                //newsTop : data.filed.newsTop == "on" ? "checked" : "",    //是否置顶
            }, function (res) {
                if (res.state) {
                    top.layer.close(index);
                    top.layer.msg("文章添加成功！");
                    layer.closeAll("iframe");
                    //刷新父页面
                    parent.location.reload();
                } else layer.msg(res.message);
            })
            return false;
        })

        //预览
        form.on("submit(look)", function () {
            layer.alert("此功能需要前台展示，实际开发中传入对应的必要参数进行文章内容页面访问");
            return false;
        })
    })
}
renderArticlePage();