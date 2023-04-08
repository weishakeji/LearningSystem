// 郑州工商学院的身份认证
Vue.component('zzgongshang', {
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

            //第三方平台的用户信息
            user: {}
            /** 示例 json对象
             * aud: "NmMyNzcwZTAwYTAxNDliZGI2ZWI0NTI2ZjlmNzA5ZTY"
                authType: "PWD"
                email: "liushihong@gmail.com"
                exp: 1680261199
                iat: 1680174799
                iss: "http://www.weishakeji.net/auth/oidc/567D115746AF46ED830A77696D14CC97"
                jti: "ylpKhbyNm6Z6vTA95tYQ7A"
                name: "测试员"
                nbf: 1680174739
                roles: []
                sub: "tester01"
                subjectId: "74e8e3c9-ffed-449f-bd0d-44ec39723988"
                userId: "8fa70155-9a3a-4c64-b91d-4443a25c1623"

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
                    th.object = req.data.result;
                    if (gettoken)
                        th.getjwt(th.object);
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
        //获取jwt
        getjwt: function (obj) {
            var url = obj.Tl_Domain + '/auth/oauth2/token';
            url = $api.url.set(url, {
                'client_id': obj.Tl_APPID,
                'client_secret': obj.Tl_Secret,
                'redirect_uri': encodeURIComponent(obj.Tl_Returl),
                'grant_type': 'authorization_code',
                'code': this.code,
            });
            var th = this;
            $api.post('OtherLogin/HttpRequest', { 'url': url }).then(function (req) {
                if (req.data.success) {
                    var result = $api.parseJson(req.data.result);
                    if (result.access_token) {
                        th.token = result.access_token;
                        th.getUserInfo(result.id_token);
                    } else {
                        //失败
                        throw result;
                    }
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                //alert(err);
                console.error(err);
            });

        },
        //获取用户信息
        getUserInfo: function (id_token) {
            //console.error(id_token);
            var token = id_token;  //在请求头中获取token
            let strings = token.split("."); //截取token，获取载体
            //解析，需要吧‘_’,'-'进行转换否则会无法解析
            var userinfo = JSON.parse(decodeURIComponent(escape(window.atob(strings[1].replace(/-/g, "+").replace(/_/g, "/")))));          
            userinfo['openid'] = userinfo.userId;
            userinfo['tag'] = this.tag;     //第三方登录的配置项标识          
            //console.error(userinfo);
            this.$emit('load', userinfo, this.type);
        }
    },
    template: `<div >
       
    </div>`
});
