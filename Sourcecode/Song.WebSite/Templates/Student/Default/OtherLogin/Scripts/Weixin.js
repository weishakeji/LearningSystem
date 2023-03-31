
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            outeruser: {},      //外部用户（第三方登录的用户）

            binduser: {},        //外部用户绑定的账号
            onlineuser: {},     //当前登录账号（本系统用户）

            openid: '',
            tag: '',         //第三方登录的配置项标识
            loading: false
        },
        computed: {
            //是否是手机端
            ismobi: function () {
                return $api.ismobi();
            },
            //是否正常获取到第三方平台的账号
            'existouter': function () {
                return JSON.stringify(this.outeruser) != '{}' && this.outeruser != null;
            },
            //当前openid是否已经绑定到当前登录账户
            'bound': function () {
                if (!this.islogin) return false;
                var exist = JSON.stringify(this.binduser) != '{}' && this.binduser != null;
                if (!exist) return false;
                return this.binduser.Ac_WeixinOpenID == this.onlineuser.Ac_WeixinOpenID;
            },
            //openid已经使用，但没有绑定当前登录账户
            'used': function () {
                var exist = JSON.stringify(this.binduser) != '{}' && this.binduser != null;
                if (!exist) return false;
                return this.binduser.Ac_WeixinOpenID != this.onlineuser.Ac_WeixinOpenID;
            },
            //是否为登录状态
            'islogin': function () {
                return JSON.stringify(this.onlineuser) != '{}' && this.onlineuser != null;
            },
            //本地用户，也许是当前登录，也许是被绑定的用户
            'localuser': function () {
                //如果已经绑定，或没有绑定到当前绑定
                if (this.bound || !this.used) return this.onlineuser;
                return this.binduser;
            }

        },
        watch: {
            'openid': {
                handler: function (val, old) {
                    if (val == null || val == '') return;
                    this.getuser(val);
                }, immediate: true
            },
        },
        mounted: function () {
            //获取微信用户信息，来自地址栏传参
            var params = $api.querystring();
            var obj = {};
            for (let i = 0; i < params.length; i++)
                obj[params[i].key] = decodeURIComponent(params[i].val);
            //
            this.outeruser = obj;
            this.openid = obj.openid;
            this.tag = obj.tag;
            console.log(obj);
        },
        created: function () {

        },
        methods: {
            //获取登录账号与已经绑定的账号
            getuser: function (openid) {
                var th = this;
                th.loading = true;
                $api.bat(
                    $api.get('Account/Current'),
                    $api.get('Account/User4Openid', { 'openid': openid, 'field': th.tag })
                ).then(axios.spread(function (user, bound) {
                    //判断结果是否正常
                    for (var i = 0; i < arguments.length; i++) {
                        if (arguments[i].status != 200)
                            console.error(arguments[i]);
                        var data = arguments[i].data;
                        if (!data.success && data.exception != null) {
                            console.error(data.exception);
                            throw arguments[i].config.way + ' ' + data.message;
                        }
                    }
                    //获取结果
                    th.onlineuser = user.data.result;
                    th.binduser = bound.data.result;
                })).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //绑定
            bindhandler: function () {
                var th = this;
                console.error(this.openid);
                th.loading = true;
                $api.get('Account/UserBind', { 'openid': th.openid, 'field': th.tag }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.getuser(th.openid);
                        th.operateSuccess();
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //操作成功
            operateSuccess: function () {
                if ($api.ismobi()) {
                    window.setTimeout(function () {
                        var singin_referrer = $api.storage('singin_referrer');
                        if (singin_referrer != '') window.location.href = singin_referrer;
                        else
                            window.location.href = '/mobi/';
                    }, 300);
                } else {
                    if (!!window.top.vapp.shut) {
                        window.top.vapp.shut(window.name, 'vapp.fresh');
                    }
                    else {
                        alert('绑定账号成功，请返回来源页,并刷新');
                        window.setTimeout(function () {
                            window.close();
                        }, 3000);
                    }
                }               
            }
        }
    });

});


