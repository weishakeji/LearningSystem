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
                }, immediate: true
            },
        },
        methods: {
            changePw: function () {
                var th = this;
                this.$dialog.confirm({
                    title: '修改密码',
                    message: '是否确定要修改密码？',
                }).then(function () {
                    $api.post('Account/ModifyPassword', { 'oldpw': th.password.oldpw, 'newpw': th.password.newpw }).then(function (req) {
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
                }).catch(function () { });
            },

        }
    });

}, ['Components/account_header.js']);
