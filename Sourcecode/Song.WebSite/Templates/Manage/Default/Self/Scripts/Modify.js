
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
            $api.post('Admin/Super').then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.account = result;
                } else {
                    throw '未登录，或登录状态已失效';
                }
            }).catch(function (err) {
                //alert(err);
            });

        },
        methods: {
            btnEnter: function (formName) {
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        var th = this;
                        $api.post('Admin/Modify', { 'acc': th.account }).then(function (req) {
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
                console.log(this.accPingyin);

            },
            //图片上传
            handleAvatarSuccess: function (res, file) {
                if (file.status == 'success') {
                    this.account.Acc_Photo = res.url;
                    this.btnEnter();
                    //管理后台的右上角图片更换
                    if (top.usermenu)
                        top.usermenu.datas[0].img = res.url;
                }
            },
            //上传之前的验证
            beforeAvatarUpload: function (file) {
                //console.log(file);
                const isJPG = file.type === 'image/jpeg' || file.type === 'image/png';
                const isLt2M = file.size / 1024 / 1024 < 2;

                if (!isJPG) {
                    this.$message.error('上传头像图片只能是 JPG 格式!');
                }
                if (!isLt2M) {
                    this.$message.error('上传头像图片大小不能超过 2MB!');
                }
                return isJPG && isLt2M;
            }
        },
    });

});
