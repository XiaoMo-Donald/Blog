
var tzBlog = {
    init: function () {
        window.onload = function () {
            tzBlog.ArticlesCount();
            tzBlog.GetArticles({
                curr: 1, limit: 4
            });
        }
    },
    FirstInit: true,
    ArticlesTotal: 0,
    ArticlesCount: function () {
        $.get("../../Blog/ArticlesCount", {}, function (data) {
            tzBlog.ArticlesTotal = data;
            //if (data < 4) {
            //    $("#tzBlogNoMore").fadeIn();
            //} else renderBlogArticles();
            renderBlogArticles();
        })
    },
    supplementZero: function (num) {
        return num < 10 ? "0" + num : num;
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
            _blogDTimeAgo = y + '-' + tzBlog.supplementZero(M + 1) + '-' + tzBlog.supplementZero(d) + ' ' + tzBlog.supplementZero(H) + ':' + tzBlog.supplementZero(m) + ':' + tzBlog.supplementZero(s) + '&nbsp;&nbsp;' + _weekdays[_weekday] + "&nbsp;&nbsp;" + _agoStr;
        })
        return _blogDTimeAgo;
    },
    GetArticles: function (paginationInput) {
        var loadingIndex = top.layer.msg('加载中，请稍候', { icon: 16, time: false, shade: 0.8 });
        $.ajax({
            url: "../../Blog/GetArticles",
            type: "get",
            data: { index: paginationInput.curr, limit: paginationInput.limit },
            success: function (res) {
                top.layer.close(loadingIndex);
                tzBlog.ArticlesTotal = res.count;
                //模板渲染
                var _blogArticlesTpl = '',
                    _$blogArticleFill = $(".blogArticleList"),
                    _articleDelayNum = tzBlog.FirstInit ? 1 : 0,
                    _articleDelay;
                $.each(res.data, function (index, val) {
                    var _blogLabels = '';
                    $.each(val.articleLabels, function (blIndex, blVal) {
                        _blogLabels += '<a href="javascript:;" data-blogArticleLabelId="' + blVal.id + '" title="' + blVal.name + '" class="tzBlogArticleLabel">' + blVal.name + '</a>';
                    })
                    _articleDelay = _articleDelayNum + "s";
                    _blogArticlesTpl += '<li class="blogArticleItem wow fadeInDown" data-wow-duration=".6s" data-wow-delay="' + _articleDelay + '">'
                        + '<h3 class="blogTitle">'
                        + '     <span>' + _blogLabels + '</span>'
                        + '     <a class="blogTitleLink" href="javascript:" data-articleid="' + val.id + '">' + val.name + '</a>'
                        + '</h3>'
                        + '<div class="blogPartInfo">'
                        + '     <span class="blogThumbnail">'
                        + '         <a href="javascript:;" title="' + val.name + '">'
                        + '             <img src="' + val.thumbnail + '" alt="' + val.name + '">'
                        + '         </a>'
                        + '     </span>'
                        + '     <p class="blogContent mult_line_ellipsis">' + val.abstract + '</p>'
                        + '</div>'
                        + '<div class="tzAuthor">'
                        + '      <span class="tzBAR tzBlogArticleAuthorAvatar"><img src="' + val.user.minAvatar + '" alt="' + val.user.nickname + '"></span>'
                        + '     <span class="authorIdent">'
                        + '         <span>' + val.user.nickname + '</span>'
                        + '         <i class="iconFont icon-ident" title="认证信息：管理员"></i>'
                        + '         <i class="blogAuthorLevel layui-badge layui-bg-cyan">VIP</i>'
                        + '     </span>'
                        + '     <span class="blogDTime">' + tzBlog.renderBlogArticleTimeAgo(val.createTime) + '</span>'
                        + '     <span class="blogViewNum">浏览（<span class="blogViewCount">' + val.viewCount + '</span>）</span>'
                        + '     <span class="blogCommitNum"><i class="iconFont blogCommitIcon" title="回复"></i>评论（<span class="blogCommitCount">' + val.commentsCount + '</span>）</span>'
                        + '     <span class="blogReadMore">'
                        + '         <a href="javascript:" class="tzBlogMore" data-articleid="' + val.id + '"><span class="layui-badge layui-btn">阅读全文&gt;&gt;</span></a>'
                        + '     </span>'
                        + '</div>'
                        + '<div class="tzBlogLine"></div>'
                        + '</li>';
                    _articleDelayNum += 0.4;
                })
                _$blogArticleFill.html("");
                _$blogArticleFill.html(_blogArticlesTpl);
                if (tzBlog.FirstInit) {
                    tzBlog.FirstInit = false;
                    _articleDelayNum = 0;
                }
            },
            error: function () {
                top.layer.close(loadingIndex);
                layer.msg("请求超时Ծ‸Ծ");
            }
        })
    }
}
tzBlog.init();

function renderBlogArticles() {
    layui.use(['laypage', 'layer'], function () {
        var laypage = layui.laypage,
            layer = layui.layer;

        //完整功能(博客分页)
        laypage.render({
            elem: 'tzBlogPage',
            count: tzBlog.ArticlesTotal,
            theme: '#1E9FFF',
            layout: ['count', 'prev', 'page', 'next', 'refresh', 'skip'],
            //layout: ['count', 'prev', 'page', 'next', 'limit', 'refresh', 'skip'],
            limit: 4,//默认每页数量4
            //limits: [4, 10, 20, 30, 40, 50],//自定义每页数量
            jump: function (obj, first) {
                //console.log(obj)
                //console.log(obj.curr);
                //console.log(obj.limit);
                if (!first) {
                    tzBlog.GetArticles(obj);
                }
            }
        });
    })
}

$(document).on("click", ".tzBlogMore,.blogTitleLink", function () {
    var _id = $(this).data("articleid"),
        _$fillArea = $("#blogArticleDetailArea");
    var loadingIndex = top.layer.msg('加载中，请稍候', { icon: 16, time: false, shade: 0.8 });
    $.ajax({
        url: "../../Blog/ArticleDetail",
        typr: "get",
        data: { id: _id },
        success: function (html) {
            top.layer.close(loadingIndex);
            _$fillArea.html(html);
            bodyScroll.hiddenX();
            articleDetailPage.show();
            tzBlogArticleForm();
        },
        error: function () {
            top.layer.close(loadingIndex);
            layer.msg("请求超时Ծ‸Ծ");
        }
    })
})


var articleDetailPage = {
    detailArea: $("#blogArticleDetailArea"),
    show: function () {
        articleDetailPage.detailArea.css({
            "visibility": "visible",
            "animation-duration": "1.5s",
            "animation-delay": "0s",
            "animation-name": "bounceInRight"
        });
        //setTimeout(function () { bodyScroll.showX(); }, 2000);
    },
    hide: function () {
        articleDetailPage.detailArea.css({
            "animation-name": "bounceOutRight"
        })
        setTimeout(function () {
            articleDetailPage.detailArea.html("");
            articleDetailPage.detailArea.css({ "visibility": "hidden" });
        }, 1500);
    }
}