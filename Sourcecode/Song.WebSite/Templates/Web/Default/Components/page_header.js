$dom.load.css([$dom.path() + 'Components/Styles/page_header.css']);
//顶部导航
//事件：
//login:当学员登录
//teacher：当教师登录
//search: 搜索
//load: 加载成功，三个参数，机构、机构参数、平台参数
Vue.component('page_header', {
    props: [],
    data: function () {
        return {
            show: false,
            path: $dom.path(),   //模板路径
            menus: [],        //导航菜单

            search: $api.querystring("search"),      //搜索字符
            //登录相关
            loading_login: true,        //请求登录中
            account: {},        //当前登录的学员账号
            teacher: {},         //当前登录的教师账号
            //平台信息
            platinfo: {},           //平台信息
            config: {},             //当前机构的配置项
            organ: {},        //当前机构

            visible_userdrop: false,     //用户登录后的菜单面板的显示与隐藏

            error: [],       //错误信息
            loading: false
        }
    },
    watch: {
        'organ': {
            handler: function (nv, ov) {
                this.$nextTick(function () {
                    $dom("header img.logo").bind('load,error', function (event) {
                        var node = event.target ? event.target : event.srcElement;
                        var img = $dom(node);
                        var rightWh = $dom("header").width() - img.width() - parseInt(img.css("margin-left")) * 2;
                        img.next().width(rightWh);
                        img.width(img.width());
                    });
                });
                this.getnavi();
            }, immediate: true, deep: true
        },
        'menus': {
            handler: function (nv, ov) {
                if (nv.length < 1) return;
                this.$nextTick(function () {
                    //$dom(".menubar").text('ddd');
                    window.usermenu = window.$dropmenu.create({
                        target: '#menubar',
                        deftype: 'link',
                        plwidth: 180,
                        height: 40,
                        level: 40000
                    }).onclick(this.menuClick);
                    window.usermenu.add(this.nodeconvert(nv));
                });
            }, immediate: true, deep: true
        },
        'account': {
            handler: function (nv, ov) {
                //if (nv != null && JSON.stringify(nv) != '{}') this.$emit('login', nv);
            }, immediate: true, deep: true
        },
        'search': {
            handler: function (nv, ov) {
                //this.$emit('search', nv);
            }, immediate: true
        }
    },
    computed: {
        //是否登录
        islogin: function () {
            return JSON.stringify(this.account) != '{}' && this.account != null;
        },
        //是否存在教师角色
        isteacher: function () {
            return JSON.stringify(this.teacher) != '{}' && this.teacher != null;
        }
    },
    mounted: function () {
        var th = this;
        th.init();
        //学员登录
        th.loading_login = true;
        $api.login.account().then(function (acc) {
            th.account = acc;
            th.$emit('login', th.account);
            th.loading_login = false;
            $api.login.account_fresh();
        }).catch((err) => {
            console.log(err);
            th.loading_login = false;
        });
        //教师登录
        $api.login.teacher().then(function (teach) {
            th.teacher = teach;
            th.$emit('teacher', th.teacher);
        }).catch((err) => { });
        //搜索事件
        this.$emit('search', this.search);
    },
    methods: {
        init: function () {
            var th = this;
            th.loading = true;
            $api.bat(
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (platinfo, organ) {
                th.loading = false;
                //获取结果             
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                document.title += ' - '+ th.organ.Org_PlatformName;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                //加载成功的事件
                th.$emit('load', th.organ, th.config, th.platinfo);
            })).catch(function (err) {
                th.loading = false;
                console.error(err);
            });
        },
        //获取导航菜单
        getnavi: function () {
            if (!(this.organ && this.organ.Org_ID)) return;
            var th = this;
            $api.get('Navig/web', { 'orgid': this.organ.Org_ID, 'type': 'main' }).then(function (req) {
                if (req.data.success) {
                    th.menus = req.data.result;
                    console.log(th.menus);
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                th.error.push(err);
                console.error(err);
            });
        },
        //导航菜单的点击事件
        menuClick: function (sender, eventArgs) {
            var data = eventArgs.data;
            if (!data || data.url == '') return;
            window.location.href = data.url;         
        },
        //节点转换
        nodeconvert: function (obj) {
            var result = '';
            if (typeof (obj) != 'string')
                result = JSON.stringify(obj);
            //result = result.replace(/MM_WinID/g, "id");
            result = result.replace(/Nav_Name/g, "title");
            result = result.replace(/Nav_Name/g, "tit");
            result = result.replace(/Nav_Icon/g, "ico");
            result = result.replace(/children/g, "childs");
            result = result.replace(/Nav_Url/g, "url");
            return JSON.parse(result);
        },
        //搜索框的事件
        gosearch: function () {
            var str = encodeURIComponent(this.search);
            var path = "/web/course/index";
            var route = $dom('meta[view]').attr("route");
            //如果正处在课程频道页      
            var url = $api.url.set(path == route ? null : path, 'search', str);
            if (path == route) {
                history.pushState({}, "", url);
                this.$emit('search', this.search);
            } else {
                window.location.href = url;
            }          
        },
        //搜索字符串被改变
        changesearch: function (str) {
            this.search = str;
            this.gosearch();
        },
        //右上角下拉菜单的事件
        handleCommand: function (command) {
            if (command == 'logout') return this.logout();
            if (command == 'money') {
                window.location.href = '/web/account?uid=f4c2e87c58a014d0eaaed7ba1a459314';
            } else {
                window.location.href = command;
            }

        },
        //退出登录
        logout: function () {
            this.$confirm('是否确定退出登录？').then(function () {
                $api.loginstatus('account', '');
                this.account = {};
                window.setTimeout(function () {
                    window.location.href = '/web/';
                }, 500);
            }).catch(function () { });
        },
        //页面跳转,路径上增加当前页地址作为来源页
        gourl:function(url){
            return $api.url.set(url, {               
                'referrer': encodeURIComponent(location.href)
            });
        }
    },
    // 
    template: `<weisha_header_navi>
        <header v-if="loading"> <loading>... </loading></header>
        <header v-else-if="organ && JSON.stringify(organ) != '{}'">
            <a href="/" class="logo">
                <img :src="organ.Org_Logo" v-if="organ.Org_Logo!=''" />
                <img src="/Utilities/Images/def_logo.jpg" v-else />
            </a>          
            <search>
                <input type="text" name="fname" v-model.trim="search" placeholder="课程查询" @keyup.enter="gosearch()"></input>
                <icon @click="gosearch()">&#xa00b</icon>
            </search>
            <userbar>
                <loading v-if="loading_login">... </loading>
                <template v-else-if="!islogin">
                    <a :href="gourl('/web/sign/in')">登录</a> | <a :href="gourl('/web/sign/up')">注册</a>
                </template>
                <el-dropdown v-else  @command="handleCommand" @visible-change="show=>visible_userdrop=show" show-timeout="10" remark="登录后的状态">
                    <span :class="{'el-dropdown-link':true,'user-dropdown-show':visible_userdrop}">
                        <img v-if="!!account.Ac_Photo && account.Ac_Photo!=''" :src="account.Ac_Photo">
                        <template v-else>
                            <img v-if="account.Ac_Sex==2" src="/Utilities/Images/head2.jpg" />
                            <img v-else src="/Utilities/Images/head1.jpg" />
                        </template>
                        <span v-if="!!account.Ac_Name" class="acname" v-html="account.Ac_Name"></span>
                        <span v-else class="noname">(无名)</span>
                        <i class="el-icon-arrow-right el-icon--right"></i>
                    </span>
                    <el-dropdown-menu slot="dropdown">
                        <el-dropdown-item command="/web/account"><icon style="font-size:19px;">&#xe687</icon>个人中心</el-dropdown-item>
                        <el-dropdown-item command="/web/teach" v-if="isteacher"><icon>&#xe650</icon>教学管理</el-dropdown-item>
                        <el-dropdown-item command="money"><icon>&#xe81c</icon>余额: {{Math.floor(account.Ac_Money*100)/100}} 元</el-dropdown-item>                          
                        <el-dropdown-item divided  command="logout"><icon>&#xe739</icon>退出登录</el-dropdown-item>
                    </el-dropdown-menu>
                </el-dropdown>
                </userbar>            
        </header>
        <span v-else v-for="(e,i) in error" >{{i+1}}.{{e}}</span>
        <div id="menubar">           
        </div>
    </weisha_header_navi>`
});
