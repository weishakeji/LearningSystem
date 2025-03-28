$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //1为绑定，2为登录
            type: $api.querystring('type'),

            outeruser: {},      //外部用户（第三方登录的用户）  
            /** 示例
            {
                "openid": "ohjXUjtd5WeiShakeji1fwk0che0", 
                "nickname": "宋", 
                "sex": 0, 
                "language": "", 
                "city": "", 
                "province": "", 
                "country": "", 
                "headimgurl": "https://thirdwx.qlogo.cn/mmopen/vi_32/DYAIOgq83eo6KrCia9leuX73Y84YRzcsWhUicECVCE04q7ESOAeWJFJFj3nbgFjxxaBX1aticCgxWnVn7154H5H7A/132", 
                "privilege": [ ], 
                "unionid": "oviR-www.weishakeji.net-YSI"
            }
            */
            binduser: {},        //外部用户绑定的本地账号   

            unionid: '',
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
                //console.log(user);
                //type:1为登录，2为绑定
                if (type == 2 || type == '2') {
                    for (k in user) user[k] = encodeURIComponent(user[k]);
                    var url = $api.url.set('/student/OtherLogin/weixin', user);
                    return window.navigateTo(url);
                }
                this.unionid = user.unionid;
                this.tag = user.tag;
                this.getuser();
                this.getentity(this.tag);
            },
            //获取登录账号与已经绑定的账号
            getuser: function () {
                var th = this;
                th.loading = true;
                $api.get('Account/UserLogin', { 'openid': th.unionid, 'type': th.tag }).then(function (req) {
                    if (req.data.success) {
                        th.binduser = req.data.result;
                        th.$refs['login'].success(th.binduser, '手机端', '微信登录', '');
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
                obj.Ac_WeixinOpenID = user.unionid;
                obj.Ac_AccName = user.unionid;
                obj.Ac_Name = user.nickname;             //昵称
                obj.Ac_Photo = user.headimgurl;      //用户头像
                obj.Ac_Sex = user.sex == 0 ? 1 : 2;  //性别，1为男，2为女
                var th = this;
                th.loading_crt = true;
                $api.post('Account/UserCreate', { 'acc': obj, 'openid': user.unionid, 'field': th.tag }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$refs['login'].success(result, '手机端', '微信登录', '');
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
                    let referrer = $api.querystring('referrer');
                    if ($api.isnull(referrer)) referrer = $api.storage('singin_referrer');
                    if ($api.isnull(referrer) || referrer == 'undefined' || referrer == '') referrer = '/mobi';
                    $api.storage('singin_referrer', null);      //去除本地记录的来源页信息
                    if (referrer != '') window.navigateTo(decodeURIComponent(referrer));
                }, 300);
            }
        }
    });

}, ['/Utilities/OtherLogin/weixin.js',
    '/Utilities/Components/Sign/Login.js']);
