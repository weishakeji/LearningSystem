$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring("id") == "" ? $api.dot() : $api.querystring("id"),        //课程id
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项     

            course: {},         //当前课程对象
            videolog: [],        //课程章节的视频学习记录
            sum: 0,              //购买课程的人数
            teacher: null,     //课程教师
            outlines: [],     //课程章节
            guides: [],          //课程通知
            prices: [],          //课程价格
            isbuy: false,        //是否购买课程
            record: null,          //课程购买记录
            canStudy: false,     //是否能够学习

            loading: false,       //加载状态
            loading_init: false,

            showState: 1,         //内容显示的切换状态
            guides_show: false,           //通知公告是否显示
            guide: {},                   //当前要显示的通知公告
            tabActive: 0
        },
        watch: {
            'curr_sbjid': function (nv, ov) {
                console.log(nv);
            }
        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        created: function () {

        },
        mounted: function () {
            var th = this;
            this.loading_init = true;
            //当前的机构、登录学员、课程
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo'),
                $api.post('Organization/Current'),
                $api.cache('Course/ForID', { 'id': th.couid })
            ).then(axios.spread(function (account, platinfo, organ, course) {
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.exception);
                    }
                }
                //当前登录学员、平台信息         
                th.account = account.data.result;
                th.platinfo = platinfo.data.result;
                //机构配置信息
                th.organ = organ.data.result;
                th.config = $api.organ(th.organ).config;
                //当前课程
                th.course = course.data.result;
                th.course.Cou_Target = th.clearTag(th.course.Cou_Target);
                th.course.Cou_Intro = $api.trim(th.course.Cou_Intro);
                document.title = th.course.Cou_Name;
                if (!th.course) return;

                //课程章节，价格，购买人数,通知，教师，是否购买,购买的记录，是否可以学习（如果课程免费不购买也可以）               
                $api.bat(
                    $api.cache('Outline/TreeList', { 'couid': th.couid }),
                    $api.cache('Course/Prices', { 'uid': th.course.Cou_UID }),
                    $api.get('Course/StudentSum', { 'couid': th.couid }),
                    $api.cache('Guide/Guides', { 'couid': th.couid, 'count': 20 }),
                    $api.cache('Teacher/ForID', { 'id': th.course.Th_ID }),
                    $api.get('Course/Studied', { 'couid': th.couid })
                ).then(axios.spread(function (outlines, prices, sum, guides, teacher, isbuy) {
                    th.loading_init = false;
                    //判断结果是否正常
                    for (var i = 0; i < arguments.length; i++) {
                        if (arguments[i].status != 200)
                            console.error(arguments[i]);
                        var data = arguments[i].data;
                        if (!data.success && data.exception != null) {
                            console.error(data.exception);
                        }
                    }
                    //获取结果
                    th.outlines = outlines.data.result;
                    th.prices = prices.data.result;
                    th.sum = sum.data.result;
                    th.guides = guides.data.result;
                    th.teacher = teacher.data.result;
                    th.isbuy = isbuy.data.result;
                    //如果已经登录
                    if (th.islogin) {
                        $api.bat(
                            $api.get('Course/StudyAllow', { 'couid': th.couid }),
                            $api.get('Course/Purchaselog', { 'couid': th.couid, 'stid': th.account ? th.account.Ac_ID : 0 }),
                            $api.cache('Course/LogForOutlineVideo:5', { 'stid': th.account.Ac_ID, 'couid': th.couid })   //章节的视频学习记录
                        ).then(axios.spread(function (canStudy, record, videolog) {
                            //判断结果是否正常
                            for (var i = 0; i < arguments.length; i++) {
                                if (arguments[i].status != 200)
                                    console.error(arguments[i]);
                                var data = arguments[i].data;
                                if (!data.success && data.exception != null) {
                                    console.error(data.exception);
                                    throw data.message;
                                }
                            }
                            //获取结果
                            th.canStudy = canStudy.data.result;
                            th.record = record.data.result;
                            th.videolog = videolog.data.result;
                        })).catch(function (err) {
                            console.error(err);
                        });
                    }
                })).catch(function (err) {
                    console.error(err);
                });
            })).catch(function (err) {
                console.error(err);
            });
        },
        methods: {
            //清理Html标签
            clearTag: function (html) {
                var txt = html.replace(/<\/?.+?>/g, "");
                txt = $api.trim(txt);
                return txt;
            }
        }
    });
    // 课程按钮组
    Vue.component('course_menus', {
        props: ["account", "course", "canstudy", "isbuy", "loading"],
        data: function () {
            return {
                loading_show: false,     //预载中
                login_show: false,          //登录
                try_show: false,
                buy_show: false,
                menus: [
                    { name: '视频/直播', url: 'study', icon: '&#xe761', size: 30, show: true, evt: null },
                    { name: '试题练习', url: '../question/course', icon: '&#xe680', size: 29, show: true, evt: null },
                    { name: '在线测试', url: '../Test/Index', icon: '&#xe84b', size: 29, show: true, evt: null },
                    { name: '知识库', url: 'Knowledges', icon: '&#xe76b', size: 30, show: true, evt: null },
                    { name: '结课考试', url: '../Test/Finality', icon: '&#xe810', size: 32, show: true, evt: this.goFinality },
                ],
                curr_menus: {},  //当前点击的按钮项
                outline: {},     //当前点击的章节
                loading: false
            }
        },
        watch: {
            //预载结束，隐藏提示信息
            'loading': function (nv, ov) {
                if (!nv) this.loading_show = false;
            }
        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        mounted: function () {

        },
        methods: {
            //按钮事件，首先是状态判断
            btnEvt: function (item, outline) {
                this.outline = outline;
                this.curr_menus = item;
                var olid = outline != null ? outline.Ol_ID : 0;
                //预载中
                if (this.loading) {
                    this.loading_show = true;
                    return;
                }
                //未登录，则弹出登录按钮
                if (!this.islogin) {
                    this.login_show = true;
                    return;
                }
                if (item == null) {
                    for (var i = 0; i < this.menus.length; i++) {
                        if (!this.menus[i].show) continue;
                        item = this.menus[i];
                        break;
                    }
                }

                //可以学习，购买，或课程免费
                if (this.course.Cou_IsFree || this.course.Cou_IsLimitFree || this.isbuy) {
                    this.gourl(item.url, this.course.Cou_ID, olid);
                    return;
                } else {
                    //可以试学
                    if (this.course.Cou_IsTry) {
                        if (outline != null) {
                            if (outline.Ol_IsFree)
                                this.gourl(item.url, this.course.Cou_ID, olid);
                            else
                                this.buy_show = true;
                        } else {
                            this.try_show = true;
                        }
                    } else {
                        this.buy_show = true;
                    }
                }
            },
            //结课考试的事件
            goFinality: function (item) {               
                var url = $api.url.set(item.url, {
                    'couid': this.course.Cou_ID
                });
                window.location.href = url;
            },
            //跳转，课程id和章节id
            gourl: function (url, couid, olid) {
                var url = $api.url.set(url, {
                    'couid': couid,
                    'olid': olid
                });
                window.location.href = url;
            },
            //组件内部的按钮事件
            //跳转，登录页
            gologin: function () {
                window.location.href = '/mobi/sign/in';
            },
            //跳转，试学
            gotry: function () {
                var item = this.curr_menus;
                this.gourl(item.url, this.course.Cou_ID, 0);
            },
            //跳转，购买页
            gobuy: function () {
                var olid = this.outline != null ? this.outline.Ol_ID : 0;
                var link = this.curr_menus != null ? this.curr_menus.url : this.menus[0].url;
                link = $api.url.set(link, { 'couid': this.course.Cou_ID });
                var url = $api.url.set('/mobi/course/buy', {
                    'couid': this.course.Cou_ID,
                    'olid': olid,
                    'link': encodeURIComponent(link)
                });
                // console.log(url);               
                window.location.href = url;
            },
            //外部方法
            //从章节列表点击进入视频学习
            outlineVideo: function (outline) {

            }
        },
        // 同样也可以在 vm 实例中像 "this.message" 这样使用
        template: `<div class="mainmenu">
                <div class="mainmenuBox">
                     <div v-for="(m,i) in menus" @click="!!m.evt ? m.evt(m) : btnEvt(m)" v-if="m.show">
                         <span  class="font_icon" v-html="m.icon"  :style="'font-size: '+m.size+'px'"></span>
                        <name>{{m.name}}</name>
                    </div>                                   
                </div>
            <van-popup v-model="login_show" class="login" position="bottom" :style="{ height: '44px'}" >   
                <van-button type="primary" @click="gologin"><span class="font_icon">&#xe639</span>请登录后学习</van-button>
            </van-popup> 
            <van-popup v-model="loading_show" class="login" position="bottom" :style="{ height: '44px'}" >   
                <van-loading size="24px" type="spinner" >数据正在初始化...</van-loading>
            </van-popup>             
            <van-popup v-model="try_show" class="try" position="bottom" :style="{ height: '44px'}" >   
                <van-button type="primary" @click="gotry"><span class="font_icon">&#xe813</span>试学</van-button>
                <van-button type="info" @click="gobuy"><span class="font_icon">&#xe84d</span>选修该课程</van-button>
            </van-popup> 
            <van-popup v-model="buy_show" class="buy" position="bottom" :style="{ height: '44px'}" >   
                <van-button type="info" @click="gobuy"><span class="font_icon">&#xe84d</span>选修该课程</van-button>
            </van-popup>
        </div> `
    });
}, ["Components/progress_video.js"]);
