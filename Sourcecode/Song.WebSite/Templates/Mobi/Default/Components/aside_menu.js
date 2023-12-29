
// 侧滑菜单
Vue.component('aside_menu', {
    props: ['account'],
    data: function () {
        return {
            show: false,
            //login为0，登录后显示；1未登录显示；-1一直显示
            menus: [
                { name: '登录', login: 1, url: this.commonaddr('signin'), icon: '&#xa035', size: 16, evt: null },
                { name: '注册', login: 1, url: 'sign/up', icon: '&#xe7cd', size: 18, evt: null },
                { name: '课程中心', login: 1, url: 'course/index', icon: '&#xe765', size: 16, evt: null },
                { name: '我的课程', login: 0, url: 'Account/MyCourse', icon: '&#xe813', size: 20, evt: null },
                { name: '资金明细', login: 0, url: 'Money/Details', icon: '&#xe749', size: 20, evt: null },
                { name: '积分兑换', login: 0, url: 'Point/index', icon: '&#xe88a', size: 20, evt: null },
                { name: '学习卡', login: 0, url: 'Card/learn', icon: '&#xe60f', size: 18, evt: null },
                { name: '第三方账号绑定', login: 0, url: 'Account/OtherLogin', icon: '&#xe808', size: 20, evt: null },
                { name: 'hr', login: 0 },
                { name: '我的朋友', login: 0, url: 'Account/Myfriends', icon: '&#xe635', size: 20, evt: null },
                { name: '新闻资讯', login: -1, url: 'News/index', icon: '&#xe75c', size: 20, evt: null },
                { name: '通知', login: -1, url: 'Notice/index', icon: '&#xe697', size: 20, evt: null },

                { name: 'hr', login: 0 },
                { name: '缓存管理', login: -1, url: 'Cache/Index', icon: '&#xe6a4', size: 19, evt: null },
                { name: '注销登录', login: 0, url: '', icon: '&#xe70a', size: 20, evt: this.logout },
                { name: 'hr', login: 0 },
                { name: '测试地理位置服务', login: -1, url: 'Reference/Demo', icon: '&#xe64c', size: 20, evt: null }
            ],
            foot: [
                { name: '联系我们', login: 1, url: 'ContactUs', icon: '&#xe766', size: 16, evt: null },
                { name: '关于', login: 1, url: 'About', icon: '&#xa031', size: 16, evt: null },
            ]
        }
    },
    watch: {},
    computed: {
        //是否登录
        islogin: (t) => { return !$api.isnull(t.account); },
    },
    mounted: function () { },
    methods: {
        //退出登录
        logout: function () {
            this.$dialog.confirm({
                message: '是否确定退出登录？',
            }).then(function () {
                $api.loginstatus('account', '');
                this.account = null;
                window.setTimeout(function () {
                    window.location.reload();
                }, 100);
            }).catch(function () { });
        },
        //默认事件
        evtDefault: function (item) {
            if (item.url == "") return;
            if (item.url.substring(0, 1) == '/') return window.navigateTo(item.url);
            var root = "";
            var route = $dom("meta[route]").attr("route");
            var i = 1;
            do {
                root += route.substring(0, route.indexOf("/") + 1);
                route = route.substring(route.indexOf("/") + 1);
                i++;
            }
            while (route.indexOf("/") > -1 && i == 2)
            var url = $api.url.set(root + item.url, {});
            window.navigateTo(url);
        },
        //编辑个人信息
        goself: function () {
            window.navigateTo('/mobi/account/myself');
        }
    },
    // 同样也可以在 vm 实例中像 "this.message" 这样使用
    template: `<van-popup v-model="show" position="right" :style="{ height: '100%' }" id="aside_menu">
            <div class="aside_menu">
                <div class="account_info" v-if="!islogin" remark="未登录">
                    <div class="acc_photo nophoto"></div>
                    <div class="accInfo">
                        <div class="acc-name"> <a :href="commonaddr('signin')">未登录 </a> 
                        </div>                        
                        <span class="acc-money"> ... </span>                                             
                     </div>
                </div>
                <div class="account_info" v-if="islogin" remark="已经登录">
                    <div class="acc_photo"  @click="goself" v-if="!!account.Ac_Photo && account.Ac_Photo!=''" :style="'background-image: url('+account.Ac_Photo+');'"></div>                  
                    <div v-else  @click="goself" :class="{'acc_photo':true,'woman':account.Ac_Sex==2,'man':account.Ac_Sex!=2}"></div>
                    <div class="accInfo">
                    <div class="acc-name" @click="goself"> 
                        <span v-if="account.Ac_Name!=''">{{account.Ac_Name}}</span>
                        <span v-else>(没有名字)</span>                      
                    </div>                        
                    <div class="acc-money">
                        <a href="/mobi/Money/Details">{{account.Ac_Money}} 元</a>                   
                    </div>                                  
                 </div>
                </div>
               <div class="aside_list">
                    <template v-for="(item,index) in menus">
                        <p v-if="item.name=='hr'">&nbsp;</p>
                        <a v-else-if="(islogin && item.login==0 ) || (!islogin && item.login==1 ) || item.login<0" 
                        @click="!!item.evt ? item.evt(item) : evtDefault(item)">
                        <b v-html="item.icon" :style="'font-size: '+item.size+'px'"></b>
                            {{item.name}}
                        </a>                      
                    </template>
               </div>
               <div class="aside_foot">
                <a v-for="(item,index) in foot"  @click="!!item.evt ? item.evt(item) : evtDefault(item)">            
                <b v-html="item.icon" :style="'font-size: '+item.size+'px'"></b>
                    {{item.name}}
                </a> 
               </div>
            </div>
        </van-popup>`
});
