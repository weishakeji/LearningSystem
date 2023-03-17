$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},
            organ: {},
            config: {},
            plate: {},

            loading: false
        },
        mounted: function () {
        },
        created: function () {
        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        watch: {
            'account': function (nv, ov) {
                window.login.loading = false;
                var th = this;
                if (nv == null || JSON.stringify(nv) === '{}') {
                    $dom('#login-area').show();
                    $dom('#logged-area').hide();
                } else {
                    $dom('#login-area').hide();
                    $dom('#logged-area').show().css('opacity', 0);
                    window.setTimeout(function () {
                        $dom('#logged-area').css('opacity', 1);
                        $dom('#login-area').hide();
                        th.logged(nv);
                    }, 500);
                }
            },
            'config': function (nv, ov) {
                if (nv.IsRegStudent)
                    $dom("*[v-if='config.IsRegStudent']").hide();
            },
            'plate': function (nv, ov) {
                window.platinfo = nv;
            }
        },
        methods: {
            //已经登录
            logged: function (account) {
                this.account = account;
                this.$refs.header.account = account;
                var th = this;
                //教师登录
                $api.login.teacher().then(function (teach) {
                    th.$refs.header.teacher = teach;
                }).catch((err) => { });

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
            gourl: function (url) {
                window.location.href = url;
            }
        }
    });
    window.login = $login.create({
        target: '#login-area',
        ico: 'e804',
        //width: '320px',
        title: '...',
        company: '',
        website: '',
        tel: ''
    });
    //自定义验证
    window.login.verify([{
        'ctrl': 'user',
        'regex': /^[a-zA-Z0-9_-]{4,32}$/,
        'tips': '长度不得小于4位大于16位'
    }, {
        ctrl: 'vcode',
        regex: /^\d{4}$/,
        tips: '请输入4位数字'
    }]);
    window.login.watch({
        'loading': function (obj, nv, ov) {
            if (nv) {
                obj.dom.find(".slot").hide();
            }
            else {
                var slot = obj.dom.find(".slot");
                slot.show();
            }
        }
    });
    window.login.onlayout(function (s, e) {
        //console.log('布局完成' + e.data);
        $api.cache('Platform/PlatInfo').then(function (req) {
            if (req.data.success) {
                window.platinfo = req.data.result;
                if (s != null) s.title = window.platinfo.title;
                document.title = window.platinfo.title;
            } else {
                throw req.data.message;
            }
        }).catch(function (err) {
            //alert(err);
            console.error(err);
        });

    });
    window.login.ondragfinish(function (s, e) {
        $api.post('Helper/CheckCodeImg', { 'leng': s.vcodelen, 'acc': s.user }).then(function (req) {
            if (req.data.success) {
                var result = req.data.result;
                s.vcodebase64 = result.base64;
                s.vcodemd5 = result.value;
            } else {
                throw req.data.message;
            }
        }).catch(function (err) {
            alert(err);
        });
    });
    window.login.onsubmit(function (s, e) {
        s.loading = true;
        $api.post('Account/Login', { 'acc': s.user, 'pw': s.pw, 'vcode': s.vcode, 'vmd5': s.vcodemd5 }).then(function (req) {
            window.login.loading = false;
            if (req.data.success) {
                //登录成功
                var result = req.data.result;
                $api.loginstatus('account', result.Ac_Pw, result.Ac_ID);
                $api.login.account_fresh();
                $api.post('Point/AddForLogin', { 'source': '电脑网页', 'info': '账号密码登录', 'remark': '' });
                window.vapp.logged(result);
            } else {
                var data = req.data;
                switch (String(data.state)) {
                    //验证码错误
                    case '1101':
                        s.tips(s.inputs.vcode, false, data.message);
                        break;
                    case '1102':
                        s.tips(s.inputs.pw, false, '账号或密码错误');
                        break;
                    case '1103':
                        s.tips(s.inputs.user, false, '账号被禁用');
                        break;
                    default:
                        s.tips(s.inputs.user, false, '登录失败');
                        console.error(req.data.exception);
                        console.log(req.data.message);
                        break;
                }
            }
        }).catch(function (err) {
            alert(err);
            console.error(err);
        });
    });
    Vue.component('notices', {
        props: ["org", "count"],
        data: function () {
            return {
                datas: [],
                loading: false
            }
        },
        watch: {
            'org': {
                handler: function (nv, ov) {
                    this.getdatas();
                }, immediate: true
            }
        },
        computed: {},
        mounted: function () {
        },
        methods: {
            getdatas: function () {
                var th = this;
                th.loading = true;
                var orgid = th.org.Org_ID;
                $api.get('Notice/showitems', { 'orgid': orgid, 'type': -1, 'count': th.count }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.datas = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            }
        },
        template: ` <div class="notices" v-if='datas.length>0'>
            <div class="noItem" v-for="(data,i) in datas">
                <div>
                    <b :class="'order_'+i">{{i+1}}.</b>
                    <a :href="'/web/notice/detail.'+data.No_Id" :title="data.No_Ttl">
                    {{data.No_Ttl}}</a>
                </div>
                <div class="nodate">
                    <icon>&#xe81a</icon>{{data.No_StartTime|date("yyyy-MM-dd HH:mm")}}
                </div>
            </div>
        </div>`
    });

}, ['/Utilities/Panel/Scripts/ctrls.js',
    '/Utilities/Panel/Scripts/Login.js',
    '/Utilities/Components/Sign/Login.js',
    '/Utilities/OtherLogin/config.js',      //第三方登录的配置项
    "../Components/subject_rec.js",
]);

//登录成功
function ready(result) {
    window.vapp.logged(result);
}