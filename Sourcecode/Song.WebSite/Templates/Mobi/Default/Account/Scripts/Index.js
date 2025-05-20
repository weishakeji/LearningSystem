$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项        

            items: [
                {
                    name: '基本信息', icon: 'e669', url: '', items: [
                        { name: '个人信息', icon: 'e66a', url: 'Details' },
                        { name: '联系方式', icon: 'a047', url: 'Details' },
                        { name: '身份信息', icon: 'e68f', url: 'IDCardInfo' },
                        { name: '修改签名', icon: 'a02e', url: 'Details' },
                    ]
                },
                {
                    name: '账号安全', icon: 'e76a', url: '', items: [
                        { name: '绑定其它账号', icon: 'e81d', url: 'Bind' },
                        { name: '绑定手机', icon: 'e677', url: 'BindPhone' },
                        { name: '密码修改', icon: 'e76a', url: 'Pwd' },
                        { name: '安全问题', icon: 'e613', url: 'Ques' }
                    ]
                },
                {
                    name: '其它', icon: 'e808', url: '', items: [
                        { name: '问题反馈', icon: 'e84e', url: '' },
                        { name: '登录日志', icon: 'a01d', url: 'Loginlog' },
                        { name: '分享', icon: 'e690', url: '/mobi/Account/Myfriends' },
                    ]
                }
            ],

            loading: false
        },
        mounted: function () {

        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: t => !$api.isnull(t.account)
        },
        watch: {

        },
        methods: {
            //退出登录
            logout: function () {
                this.$dialog.confirm({
                    message: '是否确定退出登录？',
                }).then(() => {
                    this.account = null;
                    $api.login.out('account', function () {
                        window.setTimeout(() => {
                            window.navigateTo("/mobi/");
                        }, 200);
                    });

                }).catch(() => { });
            }
        }
    });

}, ['Components/account_header.js']);
