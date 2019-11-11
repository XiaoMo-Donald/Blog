using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TzCA.Common.TzRandomData
{
    /// <summary>
    /// 用户相关：生成随机值
    /// </summary>
    public class RandomDataHepler : IRandomDataHepler
    {
        /// <summary>
        /// 生成随机用户名
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public string GetRandomUsername(int length)
        {
            string randStr = "";
            Random rd = new Random();
            byte[] str = new byte[length];
            int i;
            for (i = 0; i < length - 1; i++)
            {
                int a = 0;
                while (!((a >= 48 && a <= 57) || (a >= 97 && a <= 122)))
                {
                    a = rd.Next(48, 122);
                }
                str[i] = (byte)a;
            }
            string username = new string(UnicodeEncoding.ASCII.GetChars(str));
            Random r = new Random(unchecked((int)DateTime.Now.Ticks));
            string s1 = ((char)r.Next(97, 122)).ToString();
            username = username.Replace("/0", "");
            randStr = s1 + username;
            return randStr;
        }

        /// <summary>
        /// 获取随机昵称
        /// </summary>
        /// <returns></returns>
        public string GetRandomNickname()
        {
            var nicknames = Nicknames();
            return nicknames[Random(0, nicknames.Count - 1)];
        }

        /// <summary>
        /// 随机生成姓名
        /// </summary>
        /// <returns></returns>
        public string GetRandomName()
        {
            var surnames = Surnames();
            var name = "";
            foreach (var n in GenerateChineseWords(Random(2, 4)))
            {
                name += n;
            }
            return surnames[Random(0, surnames.Count - 1)] + " " + name;
        }

        /// <summary>
        /// 获取随机头像
        /// </summary>
        /// <returns></returns>
        public string GetRandomAvatar()
        {
            string avatar = string.Empty;
            var num = Random(1, 30);
            var numStr = num < 10 ? "0" + num.ToString() : num.ToString();
            avatar = "../images/chatAvatars/userAvatar_" + numStr + ".jpeg";
            return avatar;
        }

        /// <summary>
        /// 系统默认头像数据
        /// </summary>
        /// <returns></returns>
        public List<string> DefaultAvatars()
        {
            var avatars = new List<string>();
            for (int i = 1; i <= 45; i++)
            {
                var numStr = i < 10 ? "0" + i.ToString() : i.ToString();
                avatars.Add("../images/chatAvatars/userAvatar_" + numStr + ".jpeg");
            }
            return avatars;
        }

        /// <summary>
        /// 获取随机个性签名
        /// </summary>
        /// <returns></returns>
        public string GetRandomRemark()
        {
            var remarks = Remarks();
            return remarks[Random(0, remarks.Count - 1)];
        }

        /// <summary>
        /// 个性签名数据
        /// </summary>
        /// <returns></returns>
        public List<string> Remarks()
        {
            return new List<string>
            {
                "talk is cheap,show me the code.",
                "代码在囧途，也要写到底。",
                "you will never be able to see me.（你永远也看不懂我",
                "YOU belong with me ![你跟我在一起才合适!]",
                "Drifting further and further away 渐行渐远",
                "人心凉薄 我早已明白 我亦不强求",
                "让我做你的太阳吧 温暖你",
                "没有口水与汗水，就没有成功的泪水。",
                "初一羡慕初二的旅游、初二羡慕初三的暑假、初三羡慕初一的青春。",
                "我从来就不爱和别人争东西 你喜欢就拿去 拿得走就拿去。",
                "我们说好要一起大声欢笑。笑到地老天荒。",
                "时间依旧不紧不慢走着，带上谁丢下谁都是不知不觉、无可奈何。",
                "雨 中 。 预 支 烦 恼 ， 沉 淀 自 己 。",
                "我不怎么样啊，我的眼泪都比不过她的稍稍皱眉。",
                "为兄弟俩肋插刀，为女人插兄弟俩刀，我真领悟中间的道理了",
                "人生没有彩排，每天都是直播，不仅收视率低，而且工资不高。",
                "若有人欺负你我愿意为你从绅士变成流氓。",
                "淋了一场雨，看清楚了这个世界。",
                "什么都挺你到底，我一直陪你到底。",
                "不要以为你晒黑了，就能掩盖你是白痴的事实。",
                "我以为我们还可以像以前一样，却没想那只是我以为。",
                "相好如厮.竟会至此,我权当我矫情,无病呻吟,伪弱的善者。",
                "辛辛苦苦的走了这段感情路，回头的时候才发现是多么的泥泞不堪",
                "不是我们没有可能，是我太没信心站在你身旁。",
                "过错只是一时的冲动，错过却是一生的遗憾。",
                "我猜你早已忘了我，而我仍然在心底守着你的背影。",
                "不合适就是穷 没感觉就是丑 一见钟情就是好看 深思熟虑就是有钱 这就是现实。",
                "好想写出有关于我们的那些故事 却看着屏幕发着呆不知道该敲打那个键盘。",
                "很多人不需要再见，因为只是路过而已。遗忘就是我们给彼此最好的纪念。",
                "那曾经的繁花盛事，也许不过南柯一梦，终将无痕",
                "曾经相信能把日子过成段子，如今只盼别把日子变成案子。",
                "原来，我们这些人的青春，每一个人都是暗伤连城。",
            };
        }

        /// <summary>
        /// 昵称数据
        /// </summary>
        /// <returns></returns>
        public List<string> Nicknames()
        {
            return new List<string>
            {
                "路人甲","咽泪人","半心人","咱别闹i","骂醒我","你好怪i",
                "违心狗i","几分曾经.","隔岸观火","尽情嘲笑","三千痴缠","半生浮名",
                "七瑾丶染年","谈笑残年","心软是病","温柔假象.","稚始稚终","麋鹿少年",
                "放下执着、别哭我走","最佳损友","无人能及他i","你眼中的逞强","因你难入眠","放肆的青春゛つ",
                "不要敷衍我","世界叫我太孤单i","心凉不过瞬间の","灯火阑珊.","久违的心跳i","徒步づ鬼門關",
                "有了眼泪才会伤心つ","薄荷加冰i","西岛猫纪年@","久久不离弃°","她像个孩子╮","/、剪去的那段回忆/、",
                "你永远不懂劳资的痛","让我来替他。","一片情","终会腻",
            };
        }

        /// <summary>
        /// 获取姓氏
        /// </summary>
        /// <returns></returns>
        public List<string> Surnames()
        {
            return new List<string>{
                  "赵", "钱", "孙", "李", "周", "吴", "郑", "王", "冯", "陈", "楮", "卫", "蒋", "沈", "韩", "杨",
                  "朱", "秦", "尤", "许", "何", "吕", "施", "张", "孔", "曹", "严", "华", "金", "魏", "陶", "姜",
                  "戚", "谢", "邹", "喻", "柏", "水", "窦", "章", "云", "苏", "潘", "葛", "奚", "范", "彭", "郎",
                  "鲁", "韦", "昌", "马", "苗", "凤", "花", "方", "俞", "任", "袁", "柳", "酆", "鲍", "史", "唐",
                  "费", "廉", "岑", "薛", "雷", "贺", "倪", "汤", "滕", "殷", "罗", "毕", "郝", "邬", "安", "常",
                  "乐", "于", "时", "傅", "皮", "卞", "齐", "康", "伍", "余", "元", "卜", "顾", "孟", "平", "黄",
                  "和", "穆", "萧", "尹", "姚", "邵", "湛", "汪", "祁", "毛", "禹", "狄", "米", "贝", "明", "臧",
                  "计", "伏", "成", "戴", "谈", "宋", "茅", "庞", "熊", "纪", "舒", "屈", "项", "祝", "董", "梁",
                  "杜", "阮", "蓝", "闽", "席", "季", "麻", "强", "贾", "路", "娄", "危", "江", "童", "颜", "郭",
                  "梅", "盛", "林", "刁", "锺", "徐", "丘", "骆", "高", "夏", "蔡", "田", "樊", "胡", "凌", "霍",
                  "虞", "万", "支", "柯", "昝", "管", "卢", "莫", "经", "房", "裘", "缪", "干", "解", "应", "宗",
                  "丁", "宣", "贲", "邓", "郁", "单", "杭", "洪", "包", "诸", "左", "石", "崔", "吉", "钮", "龚",
                  "程", "嵇", "邢", "滑", "裴", "陆", "荣", "翁", "荀", "羊", "於", "惠", "甄", "麹", "家", "封",
                  "芮", "羿", "储", "靳", "汲", "邴", "糜", "松", "井", "段", "富", "巫", "乌", "焦", "巴", "弓",
                  "牧", "隗", "山", "谷", "车", "侯", "宓", "蓬", "全", "郗", "班", "仰", "秋", "仲", "伊", "宫",
                  "宁", "仇", "栾", "暴", "甘", "斜", "厉", "戎", "祖", "武", "符", "刘", "景", "詹", "束", "龙",
                  "叶", "幸", "司", "韶", "郜", "黎", "蓟", "薄", "印", "宿", "白", "怀", "蒲", "邰", "从", "鄂",
                  "索", "咸", "籍", "赖", "卓", "蔺", "屠", "蒙", "池", "乔", "阴", "郁", "胥", "能", "苍", "双",
                  "闻", "莘", "党", "翟", "谭", "贡", "劳", "逄", "姬", "申", "扶", "堵", "冉", "宰", "郦", "雍",
                  "郤", "璩", "桑", "桂", "濮", "牛", "寿", "通", "边", "扈", "燕", "冀", "郏", "浦", "尚", "农",
                  "温", "别", "庄", "晏", "柴", "瞿", "阎", "充", "慕", "连", "茹", "习", "宦", "艾", "鱼", "容",
                  "向", "古", "易", "慎", "戈", "廖", "庾", "终", "暨", "居", "衡", "步", "都", "耿", "满", "弘",
                  "匡", "国", "文", "寇", "广", "禄", "阙", "东", "欧", "殳", "沃", "利", "蔚", "越", "夔", "隆",
                  "师", "巩", "厍", "聂", "晁", "勾", "敖", "融", "冷", "訾", "辛", "阚", "那", "简", "饶", "空",
                  "曾", "毋", "沙", "乜", "养", "鞠", "须", "丰", "巢", "关", "蒯", "相", "查", "后", "荆", "红",
                  "游", "竺", "权", "逑", "盖", "益", "桓", "公", "仉", "督", "晋", "楚", "阎", "法", "汝", "鄢",
                  "涂", "钦", "岳", "帅", "缑", "亢", "况", "后", "有", "琴", "归", "海", "墨", "哈", "谯", "笪",
                  "年", "爱", "阳", "佟", "商", "牟", "佘", "佴", "伯", "赏",
                  "万俟", "司马", "上官", "欧阳", "夏侯", "诸葛", "闻人", "东方", "赫连", "皇甫", "尉迟", "公羊",
                  "澹台", "公冶", "宗政", "濮阳", "淳于", "单于", "太叔", "申屠", "公孙", "仲孙", "轩辕", "令狐",
                  "锺离", "宇文", "长孙", "慕容", "鲜于", "闾丘", "司徒", "司空", "丌官", "司寇", "子车", "微生",
                  "颛孙", "端木", "巫马", "公西", "漆雕", "乐正", "壤驷", "公良", "拓拔", "夹谷", "宰父", "谷梁",
                  "段干", "百里", "东郭", "南门", "呼延", "羊舌", "梁丘", "左丘", "东门", "西门", "南宫"};

        }

        /// <summary>  
        /// 随机产生常用汉字  
        /// </summary>  
        /// <param name="count">要产生汉字的个数</param>  
        /// <returns>常用汉字</returns>  
        public List<string> GenerateChineseWords(int count)
        {
            List<string> chineseWords = new List<string>();
            Random rm = new Random();
            //注册
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding gb = Encoding.GetEncoding("gb2312");

            for (int i = 0; i < count; i++)
            {
                // 获取区码(常用汉字的区码范围为16-55)  
                int regionCode = rm.Next(16, 56);
                // 获取位码(位码范围为1-94 由于55区的90,91,92,93,94为空,故将其排除)  
                int positionCode;
                if (regionCode == 55)
                {
                    // 55区排除90,91,92,93,94  
                    positionCode = rm.Next(1, 90);
                }
                else
                {
                    positionCode = rm.Next(1, 95);
                }

                // 转换区位码为机内码  
                int regionCode_Machine = regionCode + 160;// 160即为十六进制的20H+80H=A0H  
                int positionCode_Machine = positionCode + 160;// 160即为十六进制的20H+80H=A0H  

                // 转换为汉字  
                byte[] bytes = new byte[] { (byte)regionCode_Machine, (byte)positionCode_Machine };
                chineseWords.Add(gb.GetString(bytes));
            }

            return chineseWords;
        }

        /// <summary>
        /// 系统默认语录
        /// </summary>
        /// <returns></returns>
        public List<string> GetQuotations()
        {
            return new List<string>
            {
                "不期待突如其来的好运，只希望所有的努力终有回报。",
                "你不是最好的，但是有你，却比什么都好。",
                "世界上，没有挤不出的时间，只有不想赴的约。",
                "没有特别幸运，那么请先特别努力。",
                "为什么努力，因为喜欢的东西很贵，爱的人很优秀。",
                "别在最应奋斗的年纪，辜负了最好的自己。",
                "过简单的生活，因为最简单的也是最真实的，往往也是最宝贵的。",
                "我从来不是善良的那个，可我没对任何人不好。",
                "任何陪过自己走完一段难熬时光的人，都应该用心感激。"
            };
        }

        /// <summary>
        /// 给定一个范围生成一个随机数
        /// </summary>
        /// <param name="minVal">最小值</param>
        /// <param name="maxVal">最大值</param>
        /// <returns></returns>
        public int Random(int minVal, int maxVal)
        {
            Random r = new Random();
            return Math.Abs(r.Next(minVal, maxVal));
        }

    }
}
