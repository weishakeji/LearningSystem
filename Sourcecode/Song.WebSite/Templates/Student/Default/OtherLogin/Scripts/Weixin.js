
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            outeruser: {},      //外部用户（第三方登录的用户）
            /** 示例
            {
                //不同微信应用下openid不同，例如微信公众号与微信小程序下获取同一微信号的openid并不相同
                "openid": "ohjXUjtd5WeiShakeji1fwk0che0",   
                "nickname": "宋", 
                "sex": 0, 
                "language": "", 
                "city": "", 
                "province": "", 
                "country": "", 
                "headimgurl": "https://thirdwx.qlogo.cn/mmopen/vi_32/DYAIOgq83eo6KrCia9leuX73Y84YRzcsWhUicECVCE04q7ESOAeWJFJFj3nbgFjxxaBX1aticCgxWnVn7154H5H7A/132", 
                "privilege": [ ], 
                "unionid": "oviR-www.weishakeji.net-YSI"       //这才是微信唯一id
            }
            */
            binduser: {},        //外部用户绑定的账号
            onlineuser: {},     //当前登录账号（本系统用户）

            openid: '',
            tag: '',         //第三方登录的配置项标识
            loading: false
        },
        computed: {
            //是否是手机端
            ismobi: () => $api.ismobi(),
            //是否正常获取到第三方平台的账号
            'existouter': function () {
                return !$api.isnull(this.outeruser);
            },
            //当前openid是否已经绑定到当前登录账户
            'bound': function () {
                if (!this.islogin || $api.isnull(this.binduser)) return false;
                return this.binduser.Ac_WeixinOpenID == this.onlineuser.Ac_WeixinOpenID;
            },
            //openid已经使用，但没有绑定当前登录账户
            'used': function () {
                if ($api.isnull(this.binduser)) return false;
                return this.binduser.Ac_WeixinOpenID != this.onlineuser.Ac_WeixinOpenID;
            },
            //是否为登录状态
            'islogin': t => !$api.isnull(t.onlineuser),
            //本地用户，也许是当前登录，也许是被绑定的用户
            'localuser': function () {
                //如果已经绑定，或没有绑定到当前绑定,返回当前登录账号
                if (this.bound || !this.used) return this.onlineuser;
                return this.binduser;       //如果已经绑定，则返回绑定的账号
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
            //采用unionid替代openid
            this.openid = obj.unionid;
            this.tag = obj.tag;
            console.log(obj);
        },
        created: function () {
            /* 测试，可以删除
             //获取登录账号
            var th = this;
            $api.get('Account/Current').then(req => {
                if (req.data.success) {
                    th.onlineuser = req.data.result;                   
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => { });*/
        },
        methods: {
            //获取登录账号与已经绑定的账号
            getuser: function (openid) {
                var th = this;
                th.loading = true;
                $api.bat(
                    $api.get('Account/Current'),
                    $api.get('Account/User4Openid', { 'openid': openid, 'field': th.tag })
                ).then(([user, bound]) => {
                    //获取结果
                    th.onlineuser = user.data.result;
                    th.binduser = bound.data.result;
                }).catch(function (err) { console.error(err); })
                    .finally(function () { th.loading = false; });
            },
            //绑定
            bindhandler: function () {
                var th = this;
                //console.error(this.openid);
                th.loading = true;
                $api.get('Account/UserBind', { 'openid': th.openid, 'nickname': th.outeruser.nickname, 'headurl': th.outeruser.headimgurl, 'field': th.tag })
                    .then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            th.getuser(th.openid);
                            th.operateSuccess();
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) { console.error(err); })
                    .finally(function () { th.loading = false; });
            },
            //操作成功
            operateSuccess: function () {
                if ($api.ismobi()) {
                    window.setTimeout(function () {
                        var singin_referrer = $api.storage('singin_referrer');
                        if ($api.isnull(singin_referrer) || singin_referrer == 'undefined')
                            window.location.href = '/mobi/';
                        else
                            window.location.href = singin_referrer;
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
            },
            //数据脱敏
            //first:从第几个字符开始
            //last：从倒数第几个开始
            dataMasking: function (str, first, last) {
                if (first > 0 && str.length > first) {

                }
            }
        }
    });

});


