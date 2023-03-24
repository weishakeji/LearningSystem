$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            code: $api.querystring('code'),
            state: $api.querystring('state'),
            //第三登录的配置项
            object: {},
            token: '',
            openid: '',

            user: {},  //用户信息


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
                $api.get('OtherLogin/GetObject', { 'tag': th.state }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        if (result.Tl_Config != '') {
                            try {
                                var conf = eval('(' + result.Tl_Config + ')');
                                for (k in conf) result[k] = conf[k];
                            } catch (err) { }
                        }
                        th.object = result;
                        th.gettoken(th.object);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //获取token
            gettoken: function (obj) {
                var ismobi = $api.ismobi();
                var isweixin = $api.isWeixin();//是否处于微信中
                var url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
                url = $api.url.set(url, {
                    'appid': !isweixin ? obj.Tl_APPID : obj.pubAppid,
                    'secret': !isweixin ? obj.Tl_Secret : obj.pubSecret,
                    'code': this.code,
                    'grant_type': 'authorization_code'
                });
                var th = this;
                $api.get('OtherLogin/HttpRequest', { 'url': url }).then(function (req) {
                    if (req.data.success) {
                        var result = $api.parseJson(req.data.result);
                        if (!result.errcode) {
                            //成功
                            //示例："{"access_token":"66_e22Lk3_9qlwIlYM9_T5pYEF3ZuStSk08D3PutHRlJW1S1CXf1rN6CIJWDGLV1QUQ1oK1iCeAyiZj3U9-dHHKXlM0B5LOGS0gKlCbqcvhHu4","expires_in":7200,"refresh_token":"66_M_02gRnk6F2jcX5_DVpNR5tE4ZRq5M6U-zHp5kqChS0O2AEtzPse-aN8YFwjFIa32HviovRFm-1xXVX0vvb7RGAgZA4PIpWIc2ZV1OS0acE","openid":"ofQVY1arThLpPqI2F6onNClfo1oE","scope":"snsapi_login","unionid":"oviR-0ujiC7OLI45s0xt0ja1-YSI"}"
                            th.token = result.access_token;
                            th.openid = result.openid;
                            th.getUserInfo(th.token,th.openid);
                        } else {
                            //失败
                            //示例：{"errcode":40013,"errmsg":"invalid appid, rid: 641d130f-2302406a-492bc56c"};
                            var obj = $api.parseJson(result);
                            throw obj;
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });

            },
            //获取微信用户信息
            getUserInfo: function (token, openid) {
                var url = "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN";
                          // https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}
                url = url.format(token, this.object.Tl_APPID, openid);
                var th = this;
                $api.get('OtherLogin/HttpRequest', { 'url': url }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        alert(result);
                        console.log(result);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            }
        }
    });

});
