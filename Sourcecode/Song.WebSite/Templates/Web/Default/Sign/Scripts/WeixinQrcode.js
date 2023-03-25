$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {

            tag: $api.querystring('tag'),
            //第三登录的配置项
            object: {},
            token: '',
            openid: '',

            user: {},  //用户信息
            /*
            {
                "openid": "ohjXUjtd56L30DEbXM81fwk0che0", 
                "nickname": "宋", 
                "sex": 0, 
                "language": "", 
                "city": "", 
                "province": "", 
                "country": "", 
                "headimgurl": "https://thirdwx.qlogo.cn/mmopen/vi_32/DYAIOgq83eo6KrCia9leuX73Y84YRzcsWhUicECVCE04q7ESOAeWJFJFj3nbgFjxxaBX1aticCgxWnVn7154H5H7A/132", 
                "privilege": [ ], 
                "unionid": "oviR-0ujiC7OLI45s0xt0ja1-YSI"
            }*/
            loading: true
        },
        mounted: function () {
            this.getobject();
        },
        created: function () {
        },
        computed: {
        },
        watch: {
        },
        methods: {
            //获取登录配置项的记录
            getobject: function () {
                var th = this;
                $api.get('OtherLogin/GetObject', { 'tag': th.tag }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        if (result.Tl_Config != '') {
                            try {
                                var conf = eval('(' + result.Tl_Config + ')');
                                for (k in conf) result[k] = conf[k];
                            } catch (err) { }
                        }
                        th.object = result;
                        th.showQrcode(th.object);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //显示二维码
            showQrcode: function (obj) {
                /*
                url = $api.url.set(url, {
                    'appid': item.obj.Tl_APPID,
                    'redirect_uri': encodeURIComponent(item.obj.Tl_Returl + '/web/sign/weixin'),
                    'response_type': 'code',
                    'scope': 'snsapi_login',
                    'state': item.obj.Tl_Tag,
                    'style': 'black',
                }) + '#wechat_redirect';*/
                var obj = new WxLogin({
                    self_redirect: true,
                    id: "login_container",
                    appid: obj.Tl_APPID,
                    scope: "snsapi_login",
                    redirect_uri: encodeURIComponent(obj.Tl_Returl + '/web/sign/weixin'),
                    state: obj.Tl_Tag,
                    style: "",
                    href: ""
                });
            }
        }
    });

});
