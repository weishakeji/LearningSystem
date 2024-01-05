$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //1为绑定，2为登录
            type: '',
            state: $api.querystring('state'),   //自己配置的信息，由第三方平台原样返回，此处为tag+','+type

            outeruser: {},      //外部用户（第三方登录的用户）      
            /** 示例 json对象
             * aud: "NmMyNzcwZTAwYTAxNDliZGI2ZWI0NTI2ZjlmNzA5ZTY"
                authType: "PWD"
                email: "10522779@qq.com"
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
            //是否正常获取到第三方平台的账号
            'existouter': function () {
                return JSON.stringify(this.outeruser) != '{}' && this.outeruser != null;
            },
            //当前openid是否已经绑定到当前登录账户
            'bound': function () {
                return JSON.stringify(this.binduser) != '{}' && this.binduser != null;
            },
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
                    }
                }, immediate: true,
            },
        },
        methods: {
            //加载第三方账号成功
            loaduser: function (user, type) {
                this.outeruser = user;
                //type:1为登录，2为绑定
                if (type == 2 || type == '2') {
                    for (k in user) user[k] = encodeURIComponent(user[k]);
                    var url = $api.url.set('/student/OtherLogin/ZzGongshang', user);
                    window.navigateTo(url);
                    return;
                }
                this.openid = user.openid;
                this.tag = user.tag;
                //alert(user);
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
                        th.$refs['login'].success(th.binduser, '手机端', '郑州工商学院账号登录', '');
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);

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
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //创建用户
            createuser: function () {
                var user = this.outeruser;
                var obj = {};
                obj.Ac_ZzGongshang = user.userId;
                obj.Ac_Name = user.name;
                obj.Ac_Email = user.email;
                obj.Ac_AccName = user.sub;
                //obj.Ac_Sex = user.gender == "男" ? 1 : 2;
                var th = this;
                th.loading_crt = true;
                $api.post('Account/UserCreate', { 'acc': obj, 'openid': user.userId, 'field': th.tag }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$refs['login'].success(result, '手机端', '郑州工商学院账号登录', '');
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading_crt = false);
            },
            //登录成功后的事件,acc:当前登录的账户对象
            successful: function (acc) {
                window.setTimeout(function () {
                    var singin_referrer = $api.storage('singin_referrer');
                    if (singin_referrer != '') window.navigateTo(singin_referrer);
                    else
                        window.navigateTo('/mobi/');
                }, 300);
            }
        }
    });

}, ['/Utilities/OtherLogin/config.js',
    '/Utilities/OtherLogin/zzgongshang.js',
    '/Utilities/Components/Sign/Login.js']);
