$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //1为绑定，2为登录
            type: $api.querystring('type'),

            outeruser: {},      //外部用户（第三方登录的用户）      
            binduser: {},        //外部用户绑定的账号   

            openid: '',
            tag: '',         //第三方登录的配置项标识

            entity: {},       //第三方登录的配置项
            loading: true,
            loading_crt: false       //创建用户的预载
        },
        mounted: function () {

        },
        created: function () {
        },
        computed: {
            //是否是手机端
            ismobi: function () {
                return $api.ismobi();
            },
            //当前openid是否已经绑定到当前登录账户
            'bound': function () {
                return JSON.stringify(this.binduser) != '{}' && this.binduser != null;
            },
        },
        watch: {
        },
        methods: {
            //加载第三方账号成功
            loaduser: function (user, type) {
                this.outeruser = user;
                //type:1为登录，2为绑定
                if (type == 2 || type == '2') {
                    for (k in user) user[k] = encodeURIComponent(user[k]);
                    var url = $api.url.set('/student/OtherLogin/qq', user);
                    window.location.href = url;
                    return;
                }
                this.openid = user.openid;
                this.tag = user.tag;
                this.getuser();
                this.getentity(this.tag);
            },
            //获取登录账号与已经绑定的账号
            getuser: function () {
                var th = this;
                th.loading = true;
                $api.get('Account/UserLogin', { 'openid': th.openid, 'type': th.tag }).then(function (req) {
                    if (req.data.success) {
                        th.binduser = req.data.result;
                        th.$refs['login'].success(th.binduser, 'web端', 'QQ登录', '');
                        window.setTimeout(function () {
                            var singin_referrer = $api.storage('singin_referrer');
                            if (singin_referrer != '') window.location.href = singin_referrer;
                            else
                                window.location.href = '/';
                        }, 300);

                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                }).finally(() => th.loading = false);

            },
            //获取第三方登录配置项
            getentity: function (tag) {
                var th = this;
                th.loading = true;
                $api.get('OtherLogin/GetObject', { 'tag': tag }).then(function (req) {
                    if (req.data.success) {
                        th.entity = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    console.error(err);
                }).finally(function () {
                    th.loading = false;
                });
            },
            //创建用户
            createuser: function () {
                var user = this.outeruser;
                var obj = {};
                obj.Ac_QqOpenID = user.openid;
                obj.Ac_Name = user.nickname;
                obj.Ac_Photo = user.figureurl_2;
                obj.Ac_Sex = user.gender == "男" ? 1 : 2;
                var th = this;
                th.loading_crt = true;
                $api.post('Account/UserCreate', { 'acc': obj, 'openid': user.openid }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$refs['login'].success(result, 'web端', 'QQ登录', '');
                        window.setTimeout(function () {
                            var singin_referrer = $api.storage('singin_referrer');
                            if (singin_referrer != '') window.location.href = singin_referrer;
                            else
                                window.location.href = '/';
                        }, 300);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                }).finally(() => th.loading_crt = false);
            }
        }
    });

}, ['/Utilities/OtherLogin/qq.js',
    '/Utilities/Components/Sign/Login.js']);
