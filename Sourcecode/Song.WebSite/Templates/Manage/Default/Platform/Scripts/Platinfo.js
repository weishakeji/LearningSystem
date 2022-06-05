
$ready(function () {
    window.vapp = new Vue({
        el: '#app',
        data: {
            //平台信息
            platinfo: {
                title: '',
                intro: ''
            },
            MultiOrgan: '1',
            loading: false,
            rules: {
                title: [{ required: true, message: '不得为空', trigger: 'blur' }]
            }
        },
        created: function () {
            this.loading = true;
            $api.bat(
                $api.post('Platform/PlatInfo'),
                $api.get("Platform/MultiOrgan")
            ).then(axios.spread(function (p, m) {
                if (p.data.success)
                    vapp.platinfo = p.data.result;
                if (m.data.success)
                    vapp.MultiOrgan = String(m.data.result);
                vapp.loading = false;
            })).catch(function (err) {
                console.error(err);
            });

        },
        methods: {
            btnEnter: function (formName) {
                if (window.vapp.loading) return;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        window.vapp.loading = true;
                        $api.post('Platform/PlatInfoUpdate', this.platinfo).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                top.window.login.onlayout();
                                vapp.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                            } else {
                                throw req.data.message;
                            }
                            window.vapp.loading = false;
                        }).catch(function (err) {
                            alert(err);
                            console.error(err);
                        });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //保存多机构的设置
            btnMultiOrgan: function () {
                $api.post('Platform/MultiOrganUpdate', { 'multi': this.MultiOrgan }).then(function (req) {
                    if (req.data.success) {
                        vapp.$message({
                            type: 'success',
                            message: '操作成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            }
        }
    });

});


