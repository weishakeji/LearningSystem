
$ready(function () {

    window.vapp = new Vue({
        el: '#app',
        data: {
            loading: false,  //
            id: $api.querystring('id'),
            entity: {}, //当前数据对象          
            rules: {
                LD_Name: [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    { min: 1, max: 20, message: '长度在 1 到 20 个字符', trigger: 'blur' },
                    {
                        validator: function (rule, value, callback) {
                            var pat = /^[a-zA-Z0-9_-]{1,20}$/;
                            if (!pat.test(value))
                                callback(new Error('仅限字符与数字'));
                            else callback();
                        }, trigger: 'blur'
                    },
                    {
                        validator: async function (rule, value, callback) {
                            await vapp.isDomainExist(value).then(res => {
                                if (res) callback(new Error('已经存在!'));
                            });
                        }, trigger: 'blur'
                    }
                ]
            }
        },
        watch: {
            'loading': function (val, old) {
                console.log('loading:' + val);
            }
        },
        created: function () {
            //如果是新增界面
            if (this.id == '') {
                this.entity.LD_IsUse = true;
                return;
            }
            //如果是修改界面
            var th = this;
            $api.get('Domain/ForID', { 'id': this.id }).then(function (req) {
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
            //判断域名是否已经存在
            isDomainExist: function (val) {
                var th = this;
                return new Promise(function (resolve, reject) {
                    $api.get('Domain/Exist', { 'name': val, 'id': th.id }).then(function (req) {
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
                        var apiurl = this.id == '' ? "Domain/Add" : 'Domain/Modify';
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
