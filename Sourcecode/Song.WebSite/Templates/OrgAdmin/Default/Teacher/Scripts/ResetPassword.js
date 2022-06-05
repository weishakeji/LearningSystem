$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            teacher: {},        //当前教师
            account: {},         //当前教师关联的账号  

            second: 10,
            loading: false
        },
        mounted: function () {
            var th = this;
            th.loading = true;
            $api.get('Teacher/ForID', { 'id': th.id }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.teacher = result;
                    if (th.teacher == null) return;
                    $api.get('Account/ForID', { 'id': th.teacher.Ac_ID }).then(function (req) {
                        th.loading = false;
                        if (req.data.success) {
                            th.account = req.data.result;
                            window.setInterval(function () {
                                th.second--;
                            }, 1000);
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        th.loading = false;
                        //
                        th.createAccount(th.teacher);
                        console.error(err);
                    });
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                Vue.prototype.$alert(err);
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
            //是否存在基础账号
            existacc: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        watch: {
            'second': function (n, o) {
                if (n <= 0) this.btnEnter();
            }
        },
        methods: {
            btnEnter: function () {
                var id = this.account.Ac_ID;
                var url = $api.url.set('../Student/ResetPassword', {
                    'id': id
                });
                window.location.href = url;
            },
            //创建学员账号
            createAccount: function (teacher) {
                var th=this;
                this.$confirm('教师的基础账号不存在，是否创建?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    $api.post('Teacher/CreatAccount', { 'entity': teacher }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            window.location.reload();
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        //alert(err);
                        Vue.prototype.$alert(err);
                        console.error(err);
                    });
                }).catch(() => {
                });
            }
        }
    });

});
