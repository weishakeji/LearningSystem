
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            entity: {}, //当前数据对象
            organ: {},           //当前登录账号所在的机构
            rules: {
                Title_Name: [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    { min: 1, max: 100, message: '长度在 1 到 100 个字符', trigger: 'blur' },
                    {
                        validator: async function (rule, value, callback) {
                            await vapp.isExist(value).then(res => {
                                if (res) callback(new Error('已经存在!'));
                            });
                        }, trigger: 'blur'
                    }
                ]
            },
            loading: false,  //
        },
        watch: {
        },
        computed: {
            //是否新增账号
            isadd: t => t.id == null || t.id == '' || this.id == 0,
        },
        created: function () {
            //如果是新增界面
            if (this.id == '') {
                var th = this;
                this.entity.Title_IsUse = true;
                $api.get('Admin/Organ').then(function (req) {
                    if (req.data.success) {
                        th.organ = req.data.result;
                        th.entity.Org_ID = th.organ.Org_ID;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
                return;
            }
            //如果是修改界面
            var th = this;
            $api.get('Admin/TitleForID', { 'id': th.id }).then(function (req) {
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
            //判断是否已经存在
            isExist: function (val) {
                var th = this;
                return new Promise(function (resolve, reject) {
                    $api.get('admin/TitleExist', { 'name': val, 'id': th.id, 'orgid': th.entity.Org_ID }).then(function (req) {
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
                if (this.loading) return;
                var th = this;
                this.$refs[formName].validate((valid, fields) => {
                    if (valid) {
                        th.loading = true;
                        let apiurl = th.id == '' ? "Admin/TitleAdd" : 'Admin/TitleModify';
                        $api.post(apiurl, { 'entity': th.entity }).then(function (req) {
                            if (req.data.success) {
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
                window.top.$pagebox.source.tab(window.name, 'vapp.loadDatas', isclose);
            }
        },
    });

});
