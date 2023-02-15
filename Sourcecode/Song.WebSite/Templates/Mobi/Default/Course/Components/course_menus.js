// 课程按钮组
Vue.component('course_menus', {
    //datainfo:课程的数据信息
    props: ["account", "course", "canstudy", "studied", "owned", "loading", "purchase", "datainfo"],
    data: function () {
        return {
            loading_show: false,     //预载中
            login_show: false,          //登录
            try_show: false,
            buy_show: false,
            //mustbuy:必须购买或学员组关联后才能学习
            //disable:是否禁用，即不响应鼠标事件，并灰色显示
            menus: [
                {
                    id: 'video', name: '视频/直播', url: 'study', icon: '&#xe761', size: 30, show: true,
                    count: 0, disabled: false, mustbuy: false, evt: null
                },
                {
                    id: 'question', name: '试题练习', url: '../question/course', icon: '&#xe75e', size: 29, show: true,
                    count: 0, disabled: false, mustbuy: false, evt: null
                },
                {
                    id: 'testpaper', name: '在线测试', url: '../Test/Index', icon: '&#xe84b', size: 29, show: true,
                    count: 0, disabled: false, mustbuy: true, evt: null
                },
                {
                    id: 'knowledge', name: '知识库', url: 'Knowledges', icon: '&#xe76b', size: 30, show: true,
                    count: 0, disabled: false, mustbuy: true, evt: null
                },
                {
                    id: 'testfinal', name: '结课考试', url: '../Test/Finality', icon: '&#xe810', size: 32, show: true,
                    count: 0, disabled: false, mustbuy: true, evt: null
                },
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
        },
        //课程数据变更
        'datainfo': function (nv, ov) {
            if (nv) {
                for (let i = 0; i < this.menus.length; i++) {
                    if (this.menus[i].id != 'video')
                        this.menus[i].disabled = nv[this.menus[i].id] == 0;
                    this.menus[i].count = nv[this.menus[i].id];
                }
            }
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
            if (item != null && item.disabled) return;
            //如果item为空,则来自于章节列表点击
            if (item == null && !this.owned) {
                return this.gobuy();
            }
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
            //如果菜单项必须课程购买后才能学习，且的确没有购买
            if (item.mustbuy && !this.owned && !this.course.Cou_IsFree) {
                this.buy_show = true;
                return;
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
                     <div v-for="(m,i) in menus" @click="!!m.evt ? m.evt(m) : btnEvt(m)" v-if="m.show" :disabled="m.disabled">
                        <icon  v-html="m.icon"  :style="'font-size: '+m.size+'px'"></icon>
                        <name>{{m.name}}</name>
                        <span v-if="m.count>0 && m.id!='testfinal'">{{m.count}}</span>
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