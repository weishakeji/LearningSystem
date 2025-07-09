
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            organ: {},       //当前机构
            config: {},
            //当前数据实体
            entity: {
                Ths_IsUse: true,
                Ths_IsDefault: false,
                Org_ID: 0
            },
            rules: {
                Ths_Name: [{ required: true, message: '名称不得为空', trigger: 'blur' }]
            },

            loading_init: false,
            loading: false
        },
        computed: {
            //是否新增账号
            isadd: t => t.id == null || t.id == '' || this.id == 0,
        },
        created: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Organization/Current')
            ).then(([organ]) => {
                //获取结果             
                th.organ = organ.data.result;
                if (th.id == "") th.entity.Org_ID = th.organ.Org_ID;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.getEntity();
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);

        },
        methods: {
            //获取实体
            getEntity: function () {
                if (this.isadd) return;
                var th = this;
                th.loading = true;
                $api.get('Teacher/TitleForID', { 'id': th.id }).then(function (req) {
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
            //保存信息
            btnEnter: function (formName, isclose) {
                if (!isclose && this.isadd) return;
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        if (th.loading) return;
                        th.loading = true;
                        var apipath = 'Teacher/Title' + (th.id == '' ? 'add' : 'Modify');
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
