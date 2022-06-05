$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项
            //用于修改密码
            password: {
                oldpw: '',
                newpw: '',
                newpw2: ''
            },
            friendsAll: 0,       //所有的“我的朋友”
            loading: true,  //
            uploading: false,        //修改信息中
            activeNames: []
        },
        mounted: function () {
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (account, platinfo, organ) {
                vapp.loading = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                    }
                }
                //获取结果
                vapp.account = account.data.result;
                if (vapp.account)
                    vapp.account.Ac_Sex = String(vapp.account.Ac_Sex);
                vapp.platinfo = platinfo.data.result;
                vapp.organ = organ.data.result;
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;
                if (vapp.account != null) {
                    $api.cache('Share/FriendAll:3', { 'acid': vapp.account.Ac_ID }).then(function (req) {
                        if (req.data.success) {
                            vapp.friendsAll = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    });
                }
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        watch: {
            //提交信息中的状态变更
            'uploading': function (nv, ov) {
                if (nv) {
                    this.$toast.loading({
                        message: '信息提交中...',
                        forbidClick: true,
                    });
                }
            }
        },
        methods: {
            update: function () {
                vapp.uploading = true;
                $api.post('Account/ModifySelf', { 'acc': this.account }).then(function (req) {
                    vapp.uploading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        vapp.$notify({ type: 'success', message: '修改成功' });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    vapp.$notify({ type: 'danger', message: err });
                    console.error(err);
                });
            },
            changePw: function () {
                $api.post('Account/ModifyPassword', { 'oldpw': this.password.oldpw, 'newpw': this.password.newpw }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        vapp.$notify({ type: 'success', message: '密码修改成功' });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    vapp.$notify({ type: 'danger', message: err });
                    console.error(err);
                });
            },
            //退出登录
            logout: function () {
                this.$dialog.confirm({
                    message: '是否确定退出登录？',
                }).then(() => {
                    $api.loginstatus('account', '');
                    this.account = null;
                    window.setTimeout(() => {
                        window.location.href = "/mobi/"
                    }, 200);
                }).catch(() => { });
            }
        }
    });

});
