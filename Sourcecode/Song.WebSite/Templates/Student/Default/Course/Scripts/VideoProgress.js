$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.dot(),
            stid: $api.querystring('stid', 0),     //学员id
            account: {},     //当前登录账号       

            outlines: [],
            logdatas: [],

            loading_init: true,
            loading: false
        },
        mounted: function () {
            var th = this;
            th.getAccount().then(function (d) {
                th.account = d;
                th.stid = d ? d.Ac_ID : 0;
                if (th.islogin) th.getlogs();
            });
            th.loading_init = true;
            $api.cache('Outline/TreeList', { 'couid': th.couid }).then(function (req) {
                if (req.data.success) {
                    th.outlines = req.data.result;
                    console.log(th.outlines);
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
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
        },
        methods: {
            //获取当前学员
            getAccount: async function () {
                var th = this;
                return new Promise(function (resolve, reject) {
                    var api = Number(th.stid) > 0 ? $api.get('Account/ForID', { 'id': th.stid }) : $api.get('Account/Current');
                    api.then(function (req) {
                        if (req.data.success) {
                            resolve(req.data.result);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        reject(err);
                    });
                });
            },
            //加载日志数据
            getlogs: function () {
                var th = this;
                th.loading = true;
                var acid = th.account.Ac_ID;
                $api.cache('Course/LogForOutlineVideo:10', { 'stid': acid, 'couid': th.couid }).then(function (req) {
                    if (req.data.success) {
                        th.logdatas = req.data.result;
                        th.$message({
                            message: '当前数据有10分钟的缓存',
                            duration: 10000,
                            showClose: true,
                            type: 'success'
                        });
                        console.log(th.logdatas);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            }
        }
    });

}, ['Components/outline_progress.js']);
