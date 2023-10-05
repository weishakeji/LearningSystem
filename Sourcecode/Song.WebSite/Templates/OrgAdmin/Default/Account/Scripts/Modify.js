
$ready(function () {

    window.vue = new Vue({
        el: '#vapp',
        data: {
            account: {}, //当前登录账号对象
            accPingyin: [],  //账号名称的拼音
            rules: {
                Acc_Name: [{ required: true, message: '姓名不得为空', trigger: 'blur' }]
            },
            loading: false
        },
        created: function () {
            var th = this;
            th.loading = true;
            $api.post('Admin/General').then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    var result = req.data.result;
                    vue.account = result;
                } else {
                    throw '未登录，或登录状态已失效';
                }
            }).catch(function (err) {
                alert(err);
            });

        },
        methods: {
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        $api.post('Admin/Modify', { 'acc': vue.account }).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '修改成功!',
                                    center: true
                                });
                                /*
                                 window.setTimeout(function () {
                                    var name = $dom.trim(window.name);
                                    if (window.top.$pagebox)
                                        window.top.$pagebox.shut(name);
                                }, 3000);
                                */
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err, '错误');
                        });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //名称转拼音
            pingyin: function () {
                this.accPingyin = makePy(this.account.Acc_Name);
                if (this.accPingyin.length > 0)
                    this.account.Acc_NamePinyin = this.accPingyin[0];
                //console.log(this.accPingyin);
            },
            //图片上传
            filechange: function (file) {
                var th = this;
                th.loading = true;
                $api.post('Admin/UpPhoto', { 'file': file, 'accid': th.account.Acc_Id }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.account.Acc_Photo = result;
                        try {
                            top.usermenu.datas[0].img = result;
                        } catch {

                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            }
        },
    });

},["../Scripts/hanzi2pinyin.js"]);
