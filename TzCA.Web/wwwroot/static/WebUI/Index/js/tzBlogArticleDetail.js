
function tzBlogArticleForm() {
    layui.use(['form', 'layedit', 'laydate'], function () {
        var form = layui.form,
            layer = layui.layer,
            layedit = layui.layedit,
            laydate = layui.laydate;
        layer.tips('记得点个赞哦^_^', '.tzBlogArticlePraise', {
            tips: 4
        });

        var _loadingBntTpl = '<i class="layui-icon layui-icon-loading layui-icon layui-anim layui-anim-rotate layui-anim-loop"></i>发布中...';
        var _normalBtnTpl = '<i class="layui-icon layui-icon-release"></i>发布';

        //渲染评论的时间
        function renderArticleCommentTime() {
            var _commentDateTimes = $(".commentDateTime");
            if (_commentDateTimes !== undefined) {
                $.each(_commentDateTimes, function (index, ele) {
                    $(ele).find("span").html(tzBlog.renderBlogArticleTimeAgo($(ele).data("commentdtime")));
                })
            }
        }

        //解析文章表情
        function renderParseArticleEmoji() {
            tzEmoji.parse({
                iconPath: "lib/",
                source: ".commentDetailContent p"
            })
            tzEmoji.parse({
                iconPath: "lib/",
                source: ".commentReplyUserDetailInfo .commentReplyContent"
            })
        }
        renderParseArticleEmoji();

        //渲染回复的时间
        function renderArticleCommentReplyTime() {
            var _commentReplyItemTimes = $(".commentReplyItemTime");
            if (_commentReplyItemTimes !== undefined) {
                $.each(_commentReplyItemTimes, function (index, ele) {
                    $(ele).find("span").html(tzBlog.renderBlogArticleTimeAgo($(ele).data("commentreplydtime")));
                })
            }
        }

        $(".blogDTime").html(tzBlog.renderBlogArticleTimeAgo($(".blogDTime").data("articledtime")));
        renderArticleCommentTime();
        renderArticleCommentReplyTime();

        //初始化表情
        tzEmoji.init({
            iconPath: "lib/",
            textarea: "#articleCommentText"
        });

        // 滚动语录
        function runQuotations() {
            var _colorArr = ['#1cc7d0', '#60afdc', '#2dde98', '#ff6c5f', '#fd9f3e', '#ff4f81', '#ffc168', '#b84592', '#009f4d'],
                _$quotations = $(".tzCloudAllianceQuotations ul"),
                _liItems,
                _liItemH,
                _runIndex = 0,
                _quotationsTpl = '',
                _transitionTime = '1s',
                _runTime = 4000,
                _hasRemove = false;
            _$quotations.html('<li><i class="layui-icon layui-icon-loading layui-icon layui-anim layui-anim-rotate layui-anim-loop"></i>加载中...</li>');
            $.ajax({
                url: "../../TzData/Quotations",
                type: "get",
                async: false,
                success: function (data) {
                    $.each(data, function (index, val) {
                        //_quotationsTpl += '<li style="background:' + _colorArr[index] + '">' + val + '</li>';
                        _quotationsTpl += '<li style="background:#62bcfa">' + val + '</li>';
                    })
                    _$quotations.html(_quotationsTpl);
                    _$quotations.append(_$quotations.children().eq(0).clone());
                    _liItems = _$quotations.children();
                    _liItemH = _liItems.eq(0).height();
                    setInterval(function () {
                        _$quotations.css({
                            top: -_liItemH * _runIndex,
                            transition: 'top ' + _transitionTime
                        })
                        _runIndex++;
                        _transitionTime = '1s';
                        if (_runIndex >= _liItems.length) {
                            _runIndex = 0;
                            _transitionTime = '0s';
                            // if (!_hasRemove) {
                            //     _hasRemove = true;
                            //     _liItems.eq(0).remove();
                            //     _liItems = _$quotations.children();
                            // }
                        }
                    }, _runTime)
                }
            })
        }
        runQuotations();

        //评论的回复区域模板(Root)
        function commentReplyItemAreaRootTpl($replyItemFormArea, $userId, $placeholder) {
            let _$commentReplyItemFormArea = $replyItemFormArea,
                _placeholder = $placeholder,
                _commentReplyItemTpl = '<form class="layui-form layui-form-pane" action="">' +
                    '   <div class="layui-form-item">' +
                    '       <input type="hidden" name="ReceiveRespondentId" value="' + $userId + '">' +
                    '       <textarea placeholder="@' + _placeholder + '：" class="layui-textarea rootCommentReply" name="RootCommentReply"></textarea>' +
                    '   </div>' +
                    '   <div class="layui-form-item">' +
                    '       <button type="button" class="layui-btn layui-btn-sm tzCommentReplyItemRootSaveBtn" lay-submit="" lay-filter="tzCommentReplyItemSaveBtn"><i class="layui-icon layui-icon-release"></i>发表</button>' +
                    '       <button type="button" class="layui-btn layui-btn-primary layui-btn-sm tzCommentReplyItemRootCancelBtn">取消</button>' +
                    '   </div>' +
                    '</form>';
            _$commentReplyItemFormArea.parent().parent().siblings().find(".commentReplyItemFormArea").html('');
            if (_$commentReplyItemFormArea.children().length === 0) {
                
                //关闭所有的评论回复区域
                closeAllCommentArea();

                _$commentReplyItemFormArea.html(_commentReplyItemTpl);
                _$commentReplyItemFormArea.find("textarea").focus();

                //初始化表情
                tzEmoji.init({
                    iconPath: "lib/",
                    textarea: ".rootCommentReply"
                });
                tzEmoji.reSetForArticleReply();
            } else {
                if (_$commentReplyItemFormArea.find("textarea").val() !== '') {
                    layer.confirm('您要放弃当前的回复吗？', {
                        title: '',
                        btn: ['确定', '取消']
                    }, function (index) {
                        _$commentReplyItemFormArea.find("textarea").val('');
                        _$commentReplyItemFormArea.html('');
                        layer.close(index);
                    });
                } else _$commentReplyItemFormArea.html('');
            }
        }

        //评论的回复区域模板（非root）
        function commentReplyItemAreaTpl($replyItemFormArea, $userId, $placeholder) {
            let _$commentReplyItemFormArea = $replyItemFormArea,
                _placeholder = $placeholder,
                _commentReplyItemTpl = '<form class="layui-form layui-form-pane" action="">' +
                    '   <div class="layui-form-item">' +
                    '       <input type="hidden" name="ReceiveRespondentId" value="' + $userId + '">' +
                    '       <textarea placeholder="@' + _placeholder + '：" class="layui-textarea unRootCommentReply" name="UnRootCommentReply"></textarea>' +
                    '   </div>' +
                    '   <div class="layui-form-item">' +
                    '       <button type="button" class="layui-btn layui-btn-sm tzCommentReplyItemSaveBtn" lay-submit="" lay-filter="tzCommentReplyItemSaveBtn"><i class="layui-icon layui-icon-release"></i>发表</button>' +
                    '       <button type="button" class="layui-btn layui-btn-primary layui-btn-sm tzCommentReplyItemCancelBtn" >取消</button>' +
                    '   </div>' +
                    '</form>';
            _$commentReplyItemFormArea.parent().parent().siblings().find(".commentReplyItemFormArea").html('');
            if (_$commentReplyItemFormArea.children().length === 0) {
                
                //关闭所有的评论回复区域
                closeAllCommentArea();

                _$commentReplyItemFormArea.html(_commentReplyItemTpl);
                _$commentReplyItemFormArea.find("textarea").focus();

                //初始化表情
                tzEmoji.init({
                    iconPath: "lib/",
                    textarea: ".unRootCommentReply"
                });
                tzEmoji.reSetForArticleReply();

            } else {
                if (_$commentReplyItemFormArea.find("textarea").val() !== '') {
                    layer.confirm('您要放弃当前的回复吗？', {
                        title: '',
                        btn: ['确定', '取消']
                    }, function (index) {
                        _$commentReplyItemFormArea.find("textarea").val('');
                        _$commentReplyItemFormArea.html('');
                        layer.close(index);
                    });
                } else _$commentReplyItemFormArea.html('');
            }
        }

        //评论留言区域模板
        function tzBlogArticleCommentListItemTpl($cContent) {
            let _$cContent = {
                commentId: '',
                commentUserId: '',
                commentUserAvatar: '',
                commentUserNickname: '',
                commentContent: '',
                commentDTime: ''
            },
                _tpl = '',
                _$fillArea = $(".tzBlogArticleCommentList>ul");
            _$cContent = $cContent;
            _tpl = '<li class="tzBlogArticleCommentItem" data-commentid="' + _$cContent.commentId + '">' +
                '<div class="commentDetailTop">' +
                '   <img class="commentatorAvatar" src="' + _$cContent.commentUserAvatar + '" alt="头像">' +
                '   <div class="commentatorInfo">' +
                '       <h4 class="commentNickname"><a href="javascript:">' + _$cContent.commentUserNickname + '</a>' +
                '           <i class="iconFont icon-ident" title="认证信息"></i>' +
                '           <i class="blogAuthorLevel layui-badge layui-bg-cyan">VIP</i>' +
                '       </h4>' +
                '       <div class="commentDateTime">' +
                '           <span>' + tzBlog.renderBlogArticleTimeAgo(_$cContent.commentDTime) + '</span>' +
                '       </div>' +
                '        <div class="commentBtns">' +
                '           <a href="javascript:" class="commentDelete" data-commentreplyid="' + _$cContent.commentId + '">删除</a>' +
                '           <a href="javascript:" class="commentReply" data-commentreplyuserid="' + _$cContent.commentUserId + '"><i class="iconFont blogCommitIcon"></i>回复(<span class="commentReplysCount" id="' + _$cContent.commentId + '">0</span>)</a>' +
                '       </div>' +
                '   </div>' +
                '</div>' +
                '   <div class="commentDetailContent">' +
                '       <p>' + _$cContent.commentContent + '</p>' +
                '       <div class="commentReplyItemFormArea"></div>' +
                '   </div>' +
                '   <ul class="commentReplyList"></ul>' +
                '</li>';
            _$fillArea.append(_tpl);
            renderParseArticleEmoji();
        }

        //评论回复模板 $cContent：回复内容 包括用户信息
        function commentReplyItemTpl($cContent) {
            let _$cContent = {
                replyId: '',
                replyUserId: '',
                replySaveBtn: '',
                replyFillArea: '',
                //replyFillAreaAfterOrAppend: '',
                replyUserAvatar: '',
                replyUserNickname: '',
                userNickname: '',
                replyContent: '',
                replyDTime: ''
            },
                _$replyArea = null,
                _tpl = '';
            _$cContent = $cContent;
            _$replyArea = _$cContent.replySaveBtn.parent().parent().parent();
            _tpl = '<li class="commentReplyItem">' +
                '<img class="commentReplyUserAvatar" src="' + _$cContent.replyUserAvatar + '" alt="' + _$cContent.replyUserNickname + '">' +
                '<div class="commentReplyUserDetailInfo">' +
                '   <a href="javascript:" class="replyBtn">' + _$cContent.replyUserNickname + '</a>' +
                '  <span>&nbsp;回复&nbsp;</span>' +
                '  <a href="javascript:" title="' + _$cContent.userNickname + '迷你资料卡">' + _$cContent.userNickname + '</a>' +
                '   <span>：</span>' +
                '   <span class="commentReplyContent">' + _$cContent.replyContent + '</span>' +
                '   <div class="commentReplyItemTime">' +
                '       <span>' + tzBlog.renderBlogArticleTimeAgo(_$cContent.replyDTime) + '</span>' +
                '       <a class="commentReplyItemDeleteBtn" href="javascript:" data-commentreplyid="' + _$cContent.replyId + '" title="删除" style="display: none;">删除</a>' +
                '       <a class="commentReplyItemBtn" href="javascript:" data-userid="' + _$cContent.replyUserId + '" title="回复" style="display: none;">回复</a>' +
                '   </div>' +
                '   <div class="commentReplyItemFormArea"></div>' +
                '</div>' +
                '</li>';

            //隐藏当前回复区域
            _$replyArea.html('');
            _$replyArea.find("textarea").val('');
            //填充新的回复内容
            _$cContent.replyFillArea.append(_tpl);
            renderParseArticleEmoji();
        }

        //关闭所有的评论回复区域
        function closeAllCommentArea() {
            $(".commentReplyItemFormArea").html('');
        }

        function listenArticleEvent() {

            //返回列表（相当于关闭当前页面）
            $(".backBlogArticleList").off("click").on("click", function (e) {
                e.preventDefault();
                articleDetailPage.hide();
            })

            //点击顶部的评论 定位到评论列表
            $(".tzBlogArticleComment,.blogCommitNum").off("click").on("click", function (e) {
                //锚点似乎无法准确定位
                //$('#tzBlogArticleDetailArea').stop().animate({ scrollTop: $('#tzBlogArticleCommentArea').offset().top - 98 }, 500);
                //硬计算
                $("#tzBlogArticleDetailArea").animate({ scrollTop: ($(".tzBlogArticleDetailBody").height() + 10) + 120 + 60 + 15 }, 500);
                //焦点
                $("form[name=tzBlogArticleCommentForm]").find("textarea").focus();
            })

            //显示评论的删除按钮
            $(".commentDetailTop").off("mouseenter").on("mouseenter", function (e) {
                e.preventDefault();
                let _$commentDeleteBtn = $(this).find(".commentDelete");
                if (_$commentDeleteBtn !== undefined) _$commentDeleteBtn.show();
                $(this).find(".commentDelete").show();
            })
            $(".commentDetailTop").off("mouseleave").on("mouseleave", function (e) {
                e.preventDefault();
                let _$commentDeleteBtn = $(this).find(".commentDelete");
                if (_$commentDeleteBtn !== undefined) _$commentDeleteBtn.hide();
                $(this).find(".commentDelete").hide();
            })

            //显示删除回复按钮
            $(".commentReplyItem").off("mouseenter").on("mouseenter", function (e) {
                e.preventDefault();
                let _$replyItemDeleteBtn = $(this).find(".commentReplyItemDeleteBtn");
                if (_$replyItemDeleteBtn !== undefined) _$replyItemDeleteBtn.show();
                $(this).find(".commentReplyItemBtn").show();
            })
            $(".commentReplyItem").off("mouseleave").on("mouseleave", function (e) {
                e.preventDefault();
                let _$replyItemDeleteBtn = $(this).find(".commentReplyItemDeleteBtn");
                if (_$replyItemDeleteBtn !== undefined) _$replyItemDeleteBtn.hide();
                $(this).find(".commentReplyItemBtn").hide();
            })

            //回复区域1（根节点回复）
            $(".commentReply").off("click").on("click", function (e) {
                e.preventDefault();
                let _$this = $(this),
                    _$commentReplyItemFormArea = _$this.parent().parent().parent().next().find(".commentReplyItemFormArea");
                commentReplyItemAreaRootTpl(_$commentReplyItemFormArea, _$this.data("commentreplyuserid"), _$this.parent().parent().find(".commentNickname>a").text());
                listenArticleEvent();
                return false;
            });

            //评论的回复区域
            $(".commentReplyItemBtn").off("click").on("click", function (e) {
                e.preventDefault();
                let _$this = $(this);
                commentReplyItemAreaTpl(_$this.parent().next(), _$this.data("userid"), _$this.parent().parent().find(".replyBtn").text());
                listenArticleEvent();
                return false;
            });

            //评论留言区域
            $(".tzBlogArticleCommentBtn").off("click").on("click", function (e) {
                e.preventDefault();
                let _$this = $(this),
                    _articleId = $("#tzBlogArticleDetailArea").data("articleid"),
                    _$commentContent = $("form[name=tzBlogArticleCommentForm]").find("textarea");
                if (_$commentContent.val() === '') {
                    layer.tips('请说点什么吧', _$this, {
                        id: 'tzBlogArticleCommentBtnTips',
                        tips: 4
                    });
                    _$commentContent.focus();
                    return;
                }
                _$this.html(_loadingBntTpl);
                // var loadingIndex = top.layer.msg('正在提交，请稍候', { icon: 16, time: false, shade: 0.8 });
                //调用验证码
                var _modelIndex = tzVerify.modalInit({
                    btn: _$this,
                    func: function () {
                        $.ajax({
                            url: "../../Blog/Comment",
                            type: "post",
                            data: { articleId: _articleId, content: filterXSS(_$commentContent.val()) },
                            success: function (res) {
                                if (res.state && res.code === 200) {
                                    tzBlogArticleCommentListItemTpl({
                                        commentId: res.data.id,
                                        commentUserId: res.data.commentUser.id,
                                        commentUserAvatar: res.data.commentUser.avatar,
                                        commentUserNickname: res.data.commentUser.nickname,
                                        commentContent: res.data.content,
                                        commentDTime: res.data.createTime
                                    });
                                    _$commentContent.val('');
                                    //滚动到底
                                    $("#tzBlogArticleDetailArea").animate({ scrollTop: 98 + ($(".tzBlogArticleDetailBody").height() + 10) + 120 + 60 + $(".tzBlogArticleDetailFoot").height() }, 500);
                                    //top.layer.close(loadingIndex);
                                    _$this.html(_normalBtnTpl);
                                    refreshCommentCount();
                                    layer.close(tzVerify.modalIndex);
                                    toastr.success("发布评论成功！");
                                    //调用消息通知
                                    notification.send({
                                        objectId: _articleId,
                                        content: res.data.content,
                                        contentSource: $(".tzBlogArticleDetailHead>h2>a").text(),
                                        receiverId: res.data.commentReplyUser.id,
                                        source: "文章评论"
                                    });
                                    $(".tzBlogArticleCommentItemNull").hide();
                                    listenArticleEvent();
                                    return;
                                }
                                _$this.html(_normalBtnTpl);
                                layer.close(tzVerify.modalIndex);
                                layer.msg(res.msg);
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                _$this.html(_normalBtnTpl);
                                layer.close(tzVerify.modalIndex);
                                tzAjaxStatus(XMLHttpRequest.status);
                            }
                        });
                    }
                });

            })

            //对评论的回复(Root)
            $(".tzCommentReplyItemRootSaveBtn").off("click").on("click", function (e) {
                e.preventDefault();
                let _$this = $(this),
                    _commentId = _$this.parent().parent().parent().parent().parent().data("commentid"),
                    _userId = _$this.parent().prev().find("input[name=ReceiveRespondentId]").val(),
                    _$replyContent = _$this.parent().prev().find("textarea"),
                    _contentSource = _$this.parent().parent().parent().parent().parent().find(".commentDetailContent>p").text();
                if (_$replyContent.val() === '') {
                    layer.tips('请说点什么吧', _$this, {
                        id: 'tzCommentReplyItemRootSaveBtnTips',
                        tips: 4
                    });
                    _$replyContent.focus();
                    return;
                }
                _$this.html(_loadingBntTpl);
                //var loadingIndex = top.layer.msg('正在提交，请稍候', { icon: 16, time: false, shade: 0.8 });
                //调用验证码
                var _modelIndex = tzVerify.modalInit({
                    btn: _$this,
                    func: function () {
                        $.ajax({
                            url: "../../Blog/Reply",
                            type: "post",
                            data: { commentId: _commentId, commentReplyUserId: _userId, content: filterXSS(_$replyContent.val()) },
                            success: function (res) {
                                if (res.state && res.code === 200) {
                                    commentReplyItemTpl({
                                        replyId: res.data.id,
                                        replyUserId: res.data.commentReplyUser.id,
                                        replySaveBtn: _$this,
                                        replyFillArea: _$this.parent().parent().parent().parent().next(),
                                        //replyFillAreaAfterOrAppend: 'append',
                                        replyUserAvatar: res.data.commentReplyUser.avatar,
                                        replyUserNickname: res.data.commentReplyUser.nickname,
                                        userNickname: res.data.commentUser.nickname,
                                        replyContent: res.data.content,
                                        replyDTime: res.data.createTime
                                    });
                                    //top.layer.close(loadingIndex);
                                    _$this.html(_normalBtnTpl);
                                    refreshReplyCount(_commentId);
                                    layer.close(tzVerify.modalIndex);
                                    toastr.success("回复成功！");
                                    //调用消息通知
                                    notification.send({
                                        objectId: _commentId,
                                        content: res.data.content,
                                        contentSource: _contentSource,
                                        receiverId: _userId,
                                        source: "评论回复"
                                    });
                                    listenArticleEvent();
                                    return;
                                }
                                _$this.html(_normalBtnTpl);
                                layer.close(tzVerify.modalIndex);
                                layer.msg(res.msg);
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                _$this.html(_normalBtnTpl);
                                layer.close(tzVerify.modalIndex);
                                tzAjaxStatus(XMLHttpRequest.status);
                            }
                        });
                    }
                });
                return false;
            });

            //对评论的回复(非Root)
            $(".tzCommentReplyItemSaveBtn").off("click").on("click", function (e) {
                e.preventDefault();
                let _$this = $(this),
                    _commentId = _$this.parent().parent().parent().parent().parent().parent().parent().data("commentid"),
                    _userId = _$this.parent().prev().find("input[name=ReceiveRespondentId]").val(),
                    _$replyContent = _$this.parent().prev().find("textarea"),
                    _contentSource = _$this.parent().parent().parent().parent().find(".commentReplyContent").text(),
                    _currentRelpyId = _$this.parent().parent().parent().parent().parent().data("id");
                if (_$replyContent.val() === '') {
                    layer.tips('请说点什么吧', _$this, {
                        id: 'tzCommentReplyItemSaveBtnTips',
                        tips: 4
                    });
                    _$replyContent.focus();
                    return;
                }
                _$this.html(_loadingBntTpl);
                //var loadingIndex = top.layer.msg('正在提交，请稍候', { icon: 16, time: false, shade: 0.8 });
                //调用验证码
                var _modelIndex = tzVerify.modalInit({
                    btn: _$this,
                    func: function () {
                        $.ajax({
                            url: "../../Blog/Reply",
                            type: "post",
                            data: { commentId: _commentId, commentReplyUserId: _userId, content: filterXSS(_$replyContent.val()) },
                            success: function (res) {
                                if (res.state && res.code === 200) {
                                    commentReplyItemTpl({
                                        replyId: res.data.id,
                                        replyUserId: res.data.commentReplyUser.id,
                                        replySaveBtn: _$this,
                                        replyFillArea: _$this.parent().parent().parent().parent().parent().parent(),
                                        //replyFillAreaAfterOrAppend: 'after',
                                        replyUserAvatar: res.data.commentReplyUser.avatar,
                                        replyUserNickname: res.data.commentReplyUser.nickname,
                                        userNickname: res.data.commentUser.nickname,
                                        replyContent: res.data.content,
                                        replyDTime: res.data.createTime
                                    });
                                    //top.layer.close(loadingIndex);
                                    _$this.html(_normalBtnTpl);
                                    refreshReplyCount(_commentId);
                                    layer.close(tzVerify.modalIndex);
                                    toastr.success("回复成功！");
                                    notification.send({
                                        objectId: _currentRelpyId,
                                        content: res.data.content,
                                        contentSource: _contentSource,
                                        receiverId: _userId,
                                        source: "用户回复"
                                    });
                                    listenArticleEvent();
                                    return;
                                }
                                _$this.html(_normalBtnTpl);
                                layer.close(tzVerify.modalIndex);
                                layer.msg(res.msg);
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                _$this.html(_normalBtnTpl);
                                layer.close(tzVerify.modalIndex);
                                tzAjaxStatus(XMLHttpRequest.status);
                            }
                        });
                    }
                });
                return false;
            });

            //取消
            $(".tzCommentReplyItemRootCancelBtn").off("click").on("click", function (e) {
                e.preventDefault();
                commentReplyItemAreaRootTpl($(this).parent().parent().parent().parent().find(".commentReplyItemFormArea"));
                return false;
            });

            //取消
            $(".tzCommentReplyItemCancelBtn").off("click").on("click", function (e) {
                e.preventDefault();
                commentReplyItemAreaTpl($(this).parent().parent().parent());
                return false;
            });

            //删除评论
            $(".commentDelete").off("click").on("click", function (e) {
                e.preventDefault();
                let _$this = $(this);
                layer.confirm('您确定删除该评论吗？', {
                    title: '删除确认',
                    btnAlign: 'c',
                    btn: ['确定', '取消']
                }, function () {
                    var loadingIndex = top.layer.msg('删除中，请稍候', { icon: 16, time: false, shade: 0.8 });
                    $.ajax({
                        url: '../../Blog/DeleteComment',
                        type: 'post',
                        data: { id: _$this.data("commentreplyid") },
                        success: function (res) {
                            if (res.state) {
                                refreshCommentCount();
                                _$this.parent().parent().parent().parent().remove();
                                layer.msg(res.message);
                                return;
                            }
                            layer.msg(res.message);
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            tzAjaxStatus(XMLHttpRequest.status);
                        }
                    })
                });
            })

            //删除回复
            $(".commentReplyItemDeleteBtn").off("click").on("click", function (e) {
                e.preventDefault();
                let _$this = $(this)
                _commentId = _$this.parent().parent().parent().parent().parent().data("commentid"),
                    _commentReplyId = _$this.data("commentreplyid");
                layer.confirm('您确定删除该回复吗？', {
                    title: '删除确认',
                    btnAlign: 'c',
                    btn: ['确定', '取消']
                }, function () {
                    var index = top.layer.msg('删除中，请稍候', { icon: 16, time: false, shade: 0.8 });
                    $.ajax({
                        url: '../../Blog/DeleteReply',
                        type: 'post',
                        data: { id: _commentReplyId },
                        success: function (res) {
                            if (res.state) {
                                refreshReplyCount(_commentId);
                                _$this.parent().parent().parent().remove();
                                layer.msg(res.message);
                                return;
                            }
                            layer.msg(res.message);
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            tzAjaxStatus(XMLHttpRequest.status);
                        }
                    })
                });
            })

            //点赞
            $(".tzBlogArticlePraiseUp").off("click").on("click", function () {
                let _$this = $(this),
                    _$up = $(".articlePraiseUpCount"),
                    _$down = $(".articlePraiseDownCount"),
                    _articleId = $("#tzBlogArticleDetailArea").data("articleid");
                $.ajax({
                    url: "../../Blog/ArticlePraiseUp",
                    type: "post",
                    data: { articleId: _articleId },
                    success: function (res, textStatus) {
                        if (res.state) {
                            $.tipsBox({
                                obj: _$this,
                                str: "+1",
                                callback: function () { }
                            });
                            niceIn(_$this);
                            _$up.text(res.data.up);
                            _$down.text(res.data.down);
                            //调用消息通知
                            notification.send({
                                objectId: _articleId,
                                content: null,
                                contentSource: _$this.parent().parent().parent().find("h2>a").text(),
                                receiverId: tzDefaultDatas.guid(),
                                source: "文章点赞"
                            });
                            layer.msg(res.msg);
                            return;
                        }
                        layer.msg(res.msg);
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        tzAjaxStatus(XMLHttpRequest.status);
                    }
                })
            })

            //踩一下
            $(".tzBlogArticlePraiseDown").off("click").on("click", function () {
                let _$this = $(this),
                    _$up = $(".articlePraiseUpCount"),
                    _$down = $(".articlePraiseDownCount"),
                    _articleId = $("#tzBlogArticleDetailArea").data("articleid");
                $.ajax({
                    url: "../../Blog/ArticlePraiseDown",
                    type: "post",
                    data: { articleId: _articleId },
                    success: function (res) {
                        if (res.state) {
                            $.tipsBox({
                                obj: _$this,
                                str: "-1",
                                callback: function () { }
                            });
                            niceIn(_$this);

                            _$up.text(res.data.up);
                            _$down.text(res.data.down);
                            //调用消息通知
                            notification.send({
                                objectId: _articleId,
                                content: null,
                                contentSource: _$this.parent().parent().parent().find("h2>a").text(),
                                receiverId: tzDefaultDatas.guid(),
                                source: "文章被踩"
                            });
                            layer.msg(res.msg);
                            return;
                        }
                        layer.msg(res.msg);
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        tzAjaxStatus(XMLHttpRequest.status);
                    }
                })
            })
        }
        listenArticleEvent();
        //if (articleDetailPage.isFirst) listenArticleEvent();

        //获取评论数量（刷新）
        function refreshCommentCount() {
            $.get("../../Blog/GetCommentCount", { articleId: $("#tzBlogArticleDetailArea").data("articleid") }, function (data) {
                if (data === 0) $(".tzBlogArticleCommentItemNull").show();
                $(".blogCommitCount").text(data);
            })
        }

        //获取评论下的回复数量（刷新）
        function refreshReplyCount($commentId) {
            $.get("../../Blog/GetReplyCount", { commentId: $commentId }, function (data) {
                $("#" + $commentId).text(data);
            })
        }
    })
}
//tzBlogArticleForm(); //初始化 放到了tzBlog.js
