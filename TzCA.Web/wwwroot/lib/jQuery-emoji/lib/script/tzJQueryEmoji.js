
/**表情初始化脚本 
 * 1.使用方法：
 *      1.1.必须的引用：
 *          CSS:  -- jQuery-emoji/lib/css/jquery.mCustomScrollbar.min.css
                  -- jQuery-emoji/dist/css/jquery.emoji.css
            JS:   -- jQuery-emoji/lib/script/jquery.min.js 
                  -- jQuery-emoji/lib/script/highlight.pack.js
                  -- jQuery-emoji/lib/script/jquery.mCustomScrollbar.min.js
                  -- jQuery-emoji/dist/js/jquery.emoji.min.js
                  -- tzJQueryEmoji.js
        1.2.初始化： 
                  -- tzEmoji.init(option)
        1.3.转换：
                  -- tzEmoji.parse(option)
    2.示例Demo
        2.1.初始化 
            tzEmoji.init({
                iconPath: "lib/",
                textarea: ".content"
            });
        2.2.转换
          tzEmoji.parse({
                iconPath: "lib/",
                source: ".fillContent span"
            });
 * 
*/
var tzEmoji = {
    init: function (option) {
        hljs.initHighlightingOnLoad();
        let $option = {
            iconPath: "lib/",
            textarea: ""
        };
        $option = $.extend({}, $option, option);
        if ($option.iconPath === "") {
            layer.msg("请设置插件根路径");
            console.log("请设置插件根路径");
            return;
        }
        if ($option.textarea === "") {
            layer.msg("请设置textarea");
            console.log("请设置textarea");
            return;
        }
        //console.log("emoji init success!");
        $($option.textarea).emoji({
            showTab: true,
            animation: 'fade',
            icons: [{
                name: "经典表情",
                path: $option.iconPath + "jQuery-emoji/dist/img/tieba/",
                maxNum: 50,
                file: ".jpg",
                placeholder: ":{alias}:",
                alias: {
                    1: "hehe",
                    2: "haha",
                    3: "tushe",
                    4: "a",
                    5: "ku",
                    6: "lu",
                    7: "kaixin",
                    8: "han",
                    9: "lei",
                    10: "heixian",
                    11: "bishi",
                    12: "bugaoxing",
                    13: "zhenbang",
                    14: "qian",
                    15: "yiwen",
                    16: "yinxian",
                    17: "tu",
                    18: "yi",
                    19: "weiqu",
                    20: "huaxin",
                    21: "hu",
                    22: "xiaonian",
                    23: "neng",
                    24: "taikaixin",
                    25: "huaji",
                    26: "mianqiang",
                    27: "kuanghan",
                    28: "guai",
                    29: "shuijiao",
                    30: "jinku",
                    31: "shengqi",
                    32: "jinya",
                    33: "pen",
                    34: "aixin",
                    35: "xinsui",
                    36: "meigui",
                    37: "liwu",
                    38: "caihong",
                    39: "xxyl",
                    40: "taiyang",
                    41: "qianbi",
                    42: "dnegpao",
                    43: "chabei",
                    44: "dangao",
                    45: "yinyue",
                    46: "haha2",
                    47: "shenli",
                    48: "damuzhi",
                    49: "ruo",
                    50: "OK"
                },
                title: {
                    1: "呵呵",
                    2: "哈哈",
                    3: "吐舌",
                    4: "啊",
                    5: "酷",
                    6: "怒",
                    7: "开心",
                    8: "汗",
                    9: "泪",
                    10: "黑线",
                    11: "鄙视",
                    12: "不高兴",
                    13: "真棒",
                    14: "钱",
                    15: "疑问",
                    16: "阴脸",
                    17: "吐",
                    18: "咦",
                    19: "委屈",
                    20: "花心",
                    21: "呼~",
                    22: "笑脸",
                    23: "冷",
                    24: "太开心",
                    25: "滑稽",
                    26: "勉强",
                    27: "狂汗",
                    28: "乖",
                    29: "睡觉",
                    30: "惊哭",
                    31: "生气",
                    32: "惊讶",
                    33: "喷",
                    34: "爱心",
                    35: "心碎",
                    36: "玫瑰",
                    37: "礼物",
                    38: "彩虹",
                    39: "星星月亮",
                    40: "太阳",
                    41: "钱币",
                    42: "灯泡",
                    43: "茶杯",
                    44: "蛋糕",
                    45: "音乐",
                    46: "haha",
                    47: "胜利",
                    48: "大拇指",
                    49: "弱",
                    50: "OK"
                }
            }, {
                name: '动态表情',
                path: $option.iconPath + "jQuery-emoji/dist/img/qq/",
                maxNum: 91,
                excludeNums: [41, 45, 54],
                file: ".gif",
                placeholder: "#qq_{alias}#"
            }]
        });
        
    },
    parse: function (option) {
        let $option = {
            iconPath: "lib/",
            source: ""
        };
        hljs.initHighlightingOnLoad();
        $option = $.extend({}, $option, option);
        if ($option.iconPath === '') {
            layer.msg("请设置插件根路径");
            console.log("请设置插件根路径");
            return;
        }
        if ($option.source === '') {
            //layer.msg("请设置转换源");
            console.log("转换源为空");
            return;
        }
        $($option.source).emojiParse({
            icons: [{
                path: $option.iconPath + "jQuery-emoji/dist/img/tieba/",
                file: ".jpg",
                placeholder: ":{alias}:",
                alias: {
                    1: "hehe",
                    2: "haha",
                    3: "tushe",
                    4: "a",
                    5: "ku",
                    6: "lu",
                    7: "kaixin",
                    8: "han",
                    9: "lei",
                    10: "heixian",
                    11: "bishi",
                    12: "bugaoxing",
                    13: "zhenbang",
                    14: "qian",
                    15: "yiwen",
                    16: "yinxian",
                    17: "tu",
                    18: "yi",
                    19: "weiqu",
                    20: "huaxin",
                    21: "hu",
                    22: "xiaonian",
                    23: "neng",
                    24: "taikaixin",
                    25: "huaji",
                    26: "mianqiang",
                    27: "kuanghan",
                    28: "guai",
                    29: "shuijiao",
                    30: "jinku",
                    31: "shengqi",
                    32: "jinya",
                    33: "pen",
                    34: "aixin",
                    35: "xinsui",
                    36: "meigui",
                    37: "liwu",
                    38: "caihong",
                    39: "xxyl",
                    40: "taiyang",
                    41: "qianbi",
                    42: "dnegpao",
                    43: "chabei",
                    44: "dangao",
                    45: "yinyue",
                    46: "haha2",
                    47: "shenli",
                    48: "damuzhi",
                    49: "ruo",
                    50: "OK"
                }
            }, {
                path: $option.iconPath + "jQuery-emoji/dist/img/qq/",
                file: ".gif",
                placeholder: "#qq_{alias}#"
            }]
        });
    },
    toolGetCode: function (content) {
        if (content === "") {
            //layer.msg("请加载内容参数");
            return;
        }
        var regex = /\#(.*)\#|\:(.*)\:/g;
        return content.match(regex);
    },
    toolReplaceCode: function (content) {
        if (content === "") {
            //layer.msg("请加载内容参数");
            return;
        }
        return content.replace(/\#(.*)\#|\:(.*)\:/g, '')
    },
    reSetForArticleComment: function () {
        let _$emoji_btn = null,
            _$emoji_container = null,
            _$emojiBtnFillArea = $("form[name=tzBlogArticleCommentForm] .layui-form-item").eq(0);
        setTimeout(function () {
            _$emoji_btn = $("body>.emoji_btn").prop("outerHTML");
            _$emoji_container = $("body>.emoji_container").prop("outerHTML");
            _$emoji_btn = $(_$emoji_btn);
            _$emoji_container = $(_$emoji_container);

            $("body>.emoji_btn").remove();
            $("body>.emoji_container").remove();

            _$emojiBtnFillArea.before(_$emoji_btn);
            _$emojiBtnFillArea.before(_$emoji_container);

            _$emoji_btn.removeAttr("style");
            _$emoji_btn.css({
                "bottom": "8px",
                "left": 0,
                "z-index": 4
            })
            _$emoji_container.removeAttr("style");
            _$emoji_container.css({
                "bottom": "40px",
                "left": 0,
                "z-index": 4
            })
            _$emoji_btn.click(function (e) {
                e.preventDefault();
            })
        }, 50);
    },
    reSetForArticleReply: function () {
        let _$emoji_btn = null,
            _$emoji_container = null,
            _$emojiBtnFillArea = $(".commentReplyItemFormArea>form>.layui-form-item").eq(0);
        setTimeout(function () {
            _$emoji_btn = $("body>.emoji_btn").prop("outerHTML");
            _$emoji_container = $("body>.emoji_container").prop("outerHTML");

            _$emoji_btn = $(_$emoji_btn);
            _$emoji_container = $(_$emoji_container);

            $("body>.emoji_btn").remove();
            $("body>.emoji_container").remove();

            _$emojiBtnFillArea.before(_$emoji_btn);
            _$emojiBtnFillArea.before(_$emoji_container);
            _$emoji_btn.removeAttr("style");
            _$emoji_btn.css({
                "bottom": "8px",
                "left": 0,
                "z-index": 4
            })
            _$emoji_container.removeAttr("style");
            if (_$emoji_btn.offset().top > 570) {
                _$emoji_container.css({
                    "bottom": "40px",
                    "left": 0,
                    "z-index": 4
                })
            } else {
                _$emoji_container.css({
                    "top": "140px",
                    "left": 0,
                    "z-index": 4
                })
            }
            _$emoji_btn.click(function (e) {
                e.preventDefault();
            })
        }, 50);
    }
}