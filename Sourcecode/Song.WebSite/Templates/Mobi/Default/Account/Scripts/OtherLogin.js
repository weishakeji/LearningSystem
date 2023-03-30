$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项

            loading: true,
            loading_bind: ''

        },
        mounted: function () {
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (account, platinfo, organ) {
                vapp.loading = false;
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
                vapp.account = account.data.result;
                if (vapp.account)
                    vapp.account.Ac_Sex = String(vapp.account.Ac_Sex);
                vapp.platinfo = platinfo.data.result;
                vapp.organ = organ.data.result;
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;
                if (vapp.account != null) {
                    $api.cache('Share/FriendAll:3', { 'acid': vapp.account.Ac_ID }).then(function (req) {
                        if (req.data.success) {
                            vapp.friendsAll = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    });
                }
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

        },
        methods: {
            //获取当前登录账号
            getAccount: function (func) {
                var th = this;
                th.loading_init = true;
                $api.get('Account/Current').then(function (req) {
                    th.loading_init = false;
                    if (req.data.success) {
                        th.account = req.data.result;
                        if (func != null) func(th.account);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            //打开弹窗
            openbox: function (url, title, icon, width, height) {
                var pathname = window.location.pathname;
                if (pathname.indexOf('/') > -1)
                    url = pathname.substring(0, pathname.lastIndexOf('/') + 1) + url;
                var obj = {};
                obj = {
                    'url': url, 'ico': icon, 'title': title,
                    'pid': window.name, 'showmask': true, 'min': false, 'max': false,
                    'width': width ? width : 600, 'height': height ? height : 400
                }
                if (window.top.$pagebox)
                    window.top.$pagebox.create(obj).open();
            },
            //刷新页面
            fresh: function () {
                window.location.reload();
            },
            //是否绑定
            isbind: function (tag) {
                let field = 'Ac_' + tag;
                return this.account[field] != '' && this.account[field] != null;
            },
            //取消绑定
            cancelbind: function (tag) {
                this.$dialog.confirm({
                    message: '是否确定取消绑定？',
                }).then(function () {
                    var th = this;
                    th.loading_bind = tag;
                    $api.get('Account/UserBind', { 'openid': '', 'field': tag }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            window.location.reload();
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    }).finally(() => th.loading_bind = '');
                }).catch(function () { });

            }

        }
    });

}, ['/Utilities/Components/avatar.js',
    '/Utilities/OtherLogin/config.js']);
