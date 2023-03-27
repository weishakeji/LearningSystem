// 第三方登录的配置项
Vue.component('weixin', {
    props: [],
    data: function () {
        return {
            code: $api.querystring('code'),     //第三方平台登录成功后返回的code
            state: $api.querystring('state'),   //自己配置的信息，由第三方平台原样返回，此处为tag+','+type
            tag: $api.querystring('tag'),
            type: $api.querystring('type'),

            token: $api.querystring('token'),
            openid: $api.querystring('openid'),

            //第三登录的配置项
            object: {},

            //微信用户信息
            user: {}
            /** 示例
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
            }
            */
        }
    },
    watch: {
        //返回状态值
        'state': {
            handler: function (nv, ov) {
                if (nv == null || nv == '') return;
                nv = decodeURIComponent(nv);
                if (nv.indexOf(',') > -1) {
                    var arr = nv.split(',');
                    this.tag = arr[0];
                    this.type = arr[1];
                    this.getobject(true);
                }
            }, immediate: true,
        },
        //qq的唯一标识
        'openid': {
            handler: function (nv, ov) {
                //当openid不为空，且code为空时，说明已经跳转过
                if (nv != null && nv != '' && this.code == '') {
                    this.getobject();
                    //this.getUserInfo(this.token, nv);
                }
            }, immediate: true,
        }
    },
    computed: {
    },
    mounted: function () {

    },
    created: function () {

    },
    methods: {
        //获取登录配置项的记录
        getobject: function (gettoken) {
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
                    if (gettoken)
                        th.gettoken(th.object);
                    else
                        th.getUserInfo(th.token, th.openid);

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
                        //th.getUserInfo(th.token, th.openid);
                        //跳转
                        var url = window.location.href;
                        if (url.indexOf('?') > -1) url = url.substring(0, url.lastIndexOf('?'));
                        url = $api.url.set(url, {
                            token: th.token,
                            openid: th.openid,
                            tag: th.tag,
                            type: th.type
                        });
                        window.parent.location.href = url;
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
                    var result = JSON.parse(req.data.result);
                    result['tag'] = th.tag;     //第三方登录的配置项标识
                    th.$emit('load', result);
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
    },
    template: `<div >
       
    </div>`
});
