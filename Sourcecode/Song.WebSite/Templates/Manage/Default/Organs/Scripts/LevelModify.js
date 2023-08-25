
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            entity: {}, //当前对象    
            profits: [], //分润方案列表
            profit_id: '',   //当前分润方案
            rules: {
                Olv_Name: [
                    { required: true, message: '名称不得为空', trigger: 'blur' },
                    {
                        validator: async function (rule, value, callback) {
                            await vapp.isNameExist(value).then(res => {
                                if (res) callback(new Error('当前名称已经存在!'));
                            });
                        }, trigger: 'blur'
                    }
                ],
                Olv_Tag: [
                    { required: true, message: '标识不得为空', trigger: 'blur' },
                    { min: 4, max: 20, message: '长度在 4 到 20 个字符', trigger: 'blur' },
                    {
                        validator: function (rule, value, callback) {
                            var pat = /^[a-zA-Z0-9_-]{4,20}$/;
                            if (!pat.test(value))
                                callback(new Error('标识仅限字母与数字!'));
                            else callback();
                        }, trigger: 'blur'
                    },
                    {
                        validator: async function (rule, value, callback) {
                            await vapp.isTagExist(value).then(res => {
                                if (res) callback(new Error('当前标识已经存在!'));
                            });
                        }, trigger: 'blur'
                    }
                ]
            },
            loading: false
        },
        watch: {
            'profit_id': function (nl, ol) {
                this.entity.Ps_ID = nl;
            }
        },
        computed: {
            //是否新增
            isadd: t => t.id == null || t.id == '' || this.id == 0,
        },
        created: function () {
            var th = this;
            if (th.id != '') {
                $api.get('Organization/LevelForID', { 'id': th.id }).then(function (req) {
                    if (req.data.success) {
                        th.entity = req.data.result;
                        th.profit_id = th.entity.Ps_ID == 0 ? '' : th.entity.Ps_ID;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            } else {
                th.entity.Olv_IsUse = true;
            }
            $api.get('ProfitSharing/ThemeUselist').then(function (req) {
                if (req.data.success) {
                    th.profits = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        },
        methods: {
            //判断名称是否存在
            isNameExist: function (val) {
                var th = this;
                return new Promise(function (resolve, reject) {
                    $api.get('Organization/LevelNameExist', { 'name': val, 'id': th.id }).then(function (req) {
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
            //判断tag标识是否存在
            isTagExist: function (val) {
                var th = this;
                return new Promise(function (resolve, reject) {
                    $api.get('Organization/LevelTagExist', { 'tag': val, 'id': th.id }).then(function (req) {
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
            btnEnter: function (formName, isclose) {
                var th = this;
                th.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        var apipath = 'Organization/Level' + (th.id == '' ? api = 'add' : 'Modify');
                        $api.post(apipath, { 'entity': th.entity }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                th.operateSuccess(isclose);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err, '错误');
                        }).finally(() => th.loading = false);
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //操作成功
            operateSuccess: function (isclose) {
                window.top.$pagebox.source.tab(window.name, 'vapp.getlist', isclose);
            }
        },
    });

});
