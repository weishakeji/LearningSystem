$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
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
        },
        created: function () {
        },
        computed: {
            //是否登录
            islogin: (t) => { return !$api.isnull(t.account); }
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
            },
            'account': {
                handler: function (nv, ov) {
                    this.loading = false;
                    if ($api.isnull(nv)) return;
                    this.account.Ac_Sex = String(nv.Ac_Sex);
                    var th = this;
                    $api.cache('Share/FriendAll:3', { 'acid': th.account.Ac_ID }).then(function (req) {
                        if (req.data.success) {
                            th.friendsAll = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    });
                    
                }, immediate: true
            },
        },
        methods: {
            update: function () {
                var th = this;
                th.uploading = true;
                $api.post('Account/ModifySelf', { 'acc': th.account }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({ type: 'success', message: '修改成功' });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.$notify({ type: 'danger', message: err });
                    console.error(err);
                }).finally(() => th.uploading = false);
            },
            changePw: function () {
                var th = this;
                $api.post('Account/ModifyPassword', { 'oldpw': this.password.oldpw, 'newpw': this.password.newpw }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({ type: 'success', message: '密码修改成功' });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.$notify({ type: 'danger', message: err });
                    console.error(err);
                });
            },
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

});
