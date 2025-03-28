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
                    return window.navigateTo(url);
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
                        th.$refs['login'].success(th.binduser, '手机端', 'QQ登录', '');
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
                obj.Ac_QqOpenID = user.openid;
                obj.Ac_Name = user.nickname;
                obj.Ac_Photo = user.figureurl_2;
                obj.Ac_Sex = user.gender == "男" ? 1 : 2;
                var th = this;
                th.loading_crt = true;
                $api.post('Account/UserCreate', { 'acc': obj, 'openid': user.openid, 'field': th.tag }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$refs['login'].success(result, '手机端', 'QQ登录', '');
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

}, ['/Utilities/OtherLogin/qq.js',
    '/Utilities/Components/Sign/Login.js']);
