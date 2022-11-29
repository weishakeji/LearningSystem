$ready(function () {
    window.vapp = new Vue({
        el: '#logged-area',
        data: {
            account: {},
            organ: {},
            config: {},
            loading: false
        },
        mounted: function () {
            var c = $api.loginstatus('account');
            console.log(c);
            //判断是否登录
            $api.get('Account/Current', {}, function () {
                window.login.loading = true;
            }).then(function (req) {
                window.login.loading = false;
                if (req.data.success) {
                    window.vapp.logged(req.data.result);
                } else {
                    window.login.loading = false;
                    $dom('#login-area').show();
                }
            }).catch(function (err) {
                //console.log(err);
                throw err;
            });
            //平台信息
            $api.bat(
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (platinfo, organ) {
                vapp.loading_init = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                    }
                }
                //获取结果           
                vapp.platinfo = platinfo.data.result;
                vapp.organ = organ.data.result;
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;
            })).catch(function (err) {
                console.error(err);
            });
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
                if (nv == null || JSON.stringify(nv) === '{}') {
                    $dom('#login-area').show();
                    $dom('#logged-area').hide();
                } else {
                    $dom('#login-area').hide();
                    $dom('#logged-area').show().css('opacity', 0);
                    window.setTimeout(function () {
                        $dom('#logged-area').css('opacity', 1);
                    }, 500);
                }
            },
            'config': function (nv, ov) {
                if (nv.IsRegStudent)
                    $dom("*[v-if='config.IsRegStudent']").hide();
            }
        },
        methods: {
            //已经登录
            logged: function (account) {
                this.account = account;
            },
            //退出登录
            logout: function () {
                this.$dialog.confirm({
                    message: '是否确定退出登录？',
                }).then(function () {
                    $api.loginstatus('account', '');
                    this.account = {};
                    window.setTimeout(function () {
                        window.location.href = '/mobi/';
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

}, ['/Utilities/Panel/Scripts/ctrls.js',
    '/Utilities/Panel/Scripts/Login.js'
]);

//登录成功
function ready(result) {
    window.vapp.logged(result);
}