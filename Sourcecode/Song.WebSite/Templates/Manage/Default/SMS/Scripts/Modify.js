
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            mark: $api.querystring('mark'),
            entity: {}, //当前对象        
            count: -1,   //短信条数
            error: '',       //   
            rules: {
                user: [{ required: true, message: '不得为空', trigger: 'blur' }],
                pw: [{ required: true, message: '不得为空', trigger: 'blur' }]
            },
            loading: false
        },
        watch: {

        },
        created: function () {
            var th = this;
            if (th.id == '') return;
            th.loading = true;
            $api.get('Sms/UserInfo', { 'mark': th.mark }).then(function (req) {
                if (req.data.success) {
                    th.entity = req.data.result;
                    th.getcount();
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => alert(err))
                .finally(() => th.loading = false);
        },
        computed: {

        },
        methods: {
            //获取短信条数
            getcount: function () {
                var th = this;
                th.loading = true;
                $api.get('Sms/Count', { 'mark': th.mark, 'smsacc': th.entity.user, 'smspw': th.entity.pw }).then(function (req) {
                    if (req.data.success) {
                        th.count = req.data.result;
                        th.error = '';
                    } else throw req.data.message;
                }).catch(err => { th.error = err; th.count = -1; })
                    .finally(() => th.loading = false);
            },
            //刷新短信余额
            btnFresh: function (formName) {
                var th = this;
                th.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.getcount();
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            btnEnter: function (formName) {
                var th = this;
                th.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        $api.post('Sms/Modify', { 'mark': th.mark, 'account': th.entity.user, 'pwd': th.entity.pw }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                th.getcount();
                                th.operateSuccess();
                            } else {
                                throw req.data.message;
                            }
                        }).catch(err => alert(err))
                            .finally(() => th.loading = false);
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vapp.loadDatas', true);
            }
        },
    });

});
