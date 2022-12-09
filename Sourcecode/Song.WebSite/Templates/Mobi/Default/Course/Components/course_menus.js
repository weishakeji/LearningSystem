// 课程按钮组
Vue.component('course_menus', {
    props: ["account", "course", "canstudy", "studied", "loading", "purchase"],
    data: function () {
        return {
            loading_show: false,     //预载中
            login_show: false,          //登录
            try_show: false,
            buy_show: false,
            menus: [
                { name: '视频/直播', url: 'study', icon: '&#xe761', size: 30, show: true, evt: null },
                { name: '试题练习', url: '../question/course', icon: '&#xe75e', size: 29, show: true, evt: null },
                { name: '在线测试', url: '../Test/Index', icon: '&#xe84b', size: 29, show: true, evt: null },
                { name: '知识库', url: 'Knowledges', icon: '&#xe76b', size: 30, show: true, evt: null },
                { name: '结课考试', url: '../Test/Finality', icon: '&#xe810', size: 32, show: true, evt: null },
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
        },
        //是否购买记录
        purchased: function () {
            return JSON.stringify(this.purchase) != '{}' && this.purchase != null;
        },
        //是否为试学
        istry: function () {
            return JSON.stringify(this.purchase) != '{}' && this.purchase.Stc_IsTry;
        },
        //可以学习
        canstudy: function () {
            return this.studied && (this.purchased && this.purchase.Stc_IsEnable);
        },
        //下方弹出按钮区的高度
        popup_height: function () {
            var height = 60;
            return '{ height: ' + height + 'px}';
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
            if (this.course.Cou_IsFree || this.course.Cou_IsLimitFree || this.studied) {
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
    //
    template: `<div class="mainmenu">
                <div class="mainmenuBox">
                     <div v-for="(m,i) in menus" @click="!!m.evt ? m.evt(m) : btnEvt(m)" v-if="m.show">
                         <icon  v-html="m.icon"  :style="'font-size: '+m.size+'px'"></icon>
                        <name>{{m.name}}</name>
                    </div>                                   
                </div>
            <van-popup v-model="login_show" class="login" position="bottom" :style="popup_height">   
                <van-button type="primary" @click="gologin"><icon>&#xe639</icon>请登录后学习</van-button>
            </van-popup> 
            <van-popup v-model="loading_show" class="login" position="bottom" :style="popup_height" >   
                <van-loading size="24px" type="spinner" >数据正在初始化...</van-loading>
            </van-popup>             
            <van-popup v-model="try_show" class="try" position="bottom" :style="popup_height">   
                <van-button type="primary" @click="gotry"><icon>&#xe813</icon>试学</van-button>
                <van-button type="info" @click="gobuy"><icon>&#xe84d</icon>选修该课程</van-button>
            </van-popup> 
            <van-popup v-model="buy_show" class="buy" position="bottom" :style="popup_height">   
                <van-button type="info" @click="gobuy"><icon>&#xe84d</icon>选修该课程</van-button>
            </van-popup>          
        </div> `
});