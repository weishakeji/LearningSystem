
$ready(function () {

    window.vapp = new Vue({
        el: '#app',
        data: {
            loading: false,  //
            id: $api.querystring('id'),
            dialogVisible:false,    //帮助面板的显示
            //当前数据对象
            entity: {
                SSO_IsUse: true,
                SSO_APPID: ''
            },
            organ: {},           //当前登录账号所在的机构
            rules: {
                SSO_Name: [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    { min: 1, max: 50, message: '长度在 1 到 50 个字符', trigger: 'blur' },
                    {
                        validator: async function (rule, value, callback) {
                            await vapp.isExist(value).then(res => {
                                if (res) callback(new Error('已经存在!'));
                            });
                        }, trigger: 'blur'
                    }
                ],
                SSO_Domain: [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    { min: 1, max: 500, message: '长度在 1 到 500 个字符', trigger: 'blur' },
                    {
                        validator: function (rule, value, callback) {
                            if (value == undefined || value == '') return callback();
                            var pattern =  /^(http|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&:/~\+#]*[\w\-\@?^=%&/~\+#])?$/gi;
                            if (!pattern.test(value)) callback(new Error('请输入域名，带http或https!'));
                            else callback();
                        }, trigger: 'blur'
                    }],
                SSO_Phone: [{
                    validator: function (rule, value, callback) {
                        if (value == undefined || value == '') return callback();
                        var pattern = /^((\+86)|(86))?(1)\d{10}$/;
                        if (!pattern.test(value)) callback(new Error('请输入电话!'));
                        else callback();
                    }, trigger: 'blur'
                }],
                SSO_Email: [{ type: 'email', message: '请输入邮箱', trigger: 'blur' }],
                SSO_Info: [{
                    validator: function (rule, value, callback) {
                        if (value == undefined || value == '') return callback();
                        if (value.length > 500) callback(new Error('最多允许 500 字符，当前 ' + value.length + ' 字符'));
                    }, trigger: 'blur'
                }]
            }
        },
        watch: {
            'loading': function (val, old) {
                console.log('loading:' + val);
            }
        },
        created: function () {
            var th = this;
            //如果是新增界面
            if (this.id == '') {
                this.getuid(uid => th.entity.SSO_APPID = uid);
                return;
            }
            //如果是修改界面
            $api.get('Sso/ForID', { 'id': th.id }).then(function (req) {
                if (req.data.success) {
                    th.entity = req.data.result;
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
            });
        },
        methods: {
            //获取APPID
            getuid: async function (callback) {
                await $api.get('Systempara/UniqueID').then(function (req) {
                    if (req.data.success) {
                        callback(req.data.result);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //判断是否已经存在
            isExist: function (val) {
                var th = this;
                return new Promise(function (resolve, reject) {
                    $api.get('Sso/Exist', { 'name': val, 'id': th.id }).then(function (req) {
                        if (req.data.success) {
                            return resolve(req.data.result);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        return reject(err);
                    });
                });
            },
            btnEnter: function (formName) {
                if (this.loading) return;
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        var apiurl = th.id == '' ? "Sso/Add" : 'Sso/Modify';
                        $api.post(apiurl, { 'entity': th.entity }).then(function (req) {
                            if (req.data.success) {
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                th.operateSuccess();
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            th.loading = false;
                            alert(err, '错误');
                        });
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
