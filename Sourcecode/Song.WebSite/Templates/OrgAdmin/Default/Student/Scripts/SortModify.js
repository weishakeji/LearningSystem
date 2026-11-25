
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            org: {},       //当前机构
            config: {},
            //当前数据实体
            entity: {
                Sts_IsUse: true,
                Sts_IsDefault: false,
                Sts_SwitchPlay: false,
                Org_ID: 0
            },
            rules: {
                Sts_Name: [{ required: true, message: '名称不得为空', trigger: 'blur' },
                { validator: validate.name.proh, trigger: 'change' },   //禁止使用特殊字符
                { validator: validate.name.danger, trigger: 'change' },
                {
                    validator: async function (rule, value, callback) {
                        await vapp.isExist(value).then(res => {
                            if (res) callback(new Error('当前名称已经存在!'));
                        });
                    }, trigger: 'blur'
                }]
            },

            loading_init: false,
            loading: false
        },
        computed: {
            //是否新增账号
            isadd: t => t.id == null || t.id == '' || this.id == 0,
        },
        mounted: function () {
        },
        created: function () {
            var th = this;
            th.org = window.org;
            th.config = window.config;
            if (th.id == "") th.entity.Org_ID = th.org.Org_ID;
            th.getEntity();
        },
        methods: {
            //获取实体
            getEntity: function () {
                if (this.id == '') return;
                var th = this;
                th.loading = true;
                $api.get('Account/SortForID', { 'id': th.id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.entity = result;
                    } else {
                        throw '未查底到数据';
                    }
                }).catch(function (err) {
                    alert(err);
                }).finally(() => th.loading = false);
            },
            //判断名称是否存在
            isExist: function (val) {
                var th = this;
                return new Promise(function (resolve, reject) {
                    $api.get('Account/SortIsExist', { 'name': val, 'id': th.id, 'orgid': th.org.Org_ID }).then(function (req) {
                        if (req.data.success) {
                            return resolve(req.data.result);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        return reject(err);
                    }).finally(() => { });
                });
            },
            //保存信息
            btnEnter: function (formName, isclose) {
                if (!isclose && this.isadd) return;
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        if (th.loading) return;
                        th.loading = true;
                        var apipath = th.id == '' ? 'Account/Sortadd' : 'Account/SortModify';
                        $api.post(apipath, { 'entity': th.entity }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$notify({
                                    type: 'success', position: 'bottom-left',
                                    message: isclose ? '保存成功，并关闭！' : '保存当前编辑成功！'
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
                window.top.$pagebox.source.tab(window.name, 'vapp.freshrow("' + this.id + '")', isclose);
            }
        },
    });



});
