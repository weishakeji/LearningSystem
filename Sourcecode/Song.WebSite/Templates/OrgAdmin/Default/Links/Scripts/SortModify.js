
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            //当前数据实体
            entity: {
                Ls_IsUse: true,
                Ls_IsShow: true,
                Ls_IsImg: true,
                Ls_IsText: true
            },
            rules: {
                Ls_Name: [{ required: true, message: '分类名称不得为空', trigger: 'blur' },
                { min: 2, max: 255, message: '长度在 2 到 255 个字符', trigger: 'blur' }]
            },
            loading: false
        },
        created: function () {
            var th = this;
            if (th.id == '') return;
            th.loading = true;
            $api.get('Link/SortForID', { 'id': th.id }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.entity = result;
                } else {
                    throw '未查底到数据';
                }
            }).catch(function (err) {
                alert(err, '错误');
            }).finally(() => th.loading = false);

        },
        computed: {
            //是否新增对象
            isadd: t => { return t.id == null || t.id == ''; },
        },
        methods: {
            btnEnter: function (formName, isclose) {
                if (!isclose && this.isadd) return;
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        if (th.loading) return;
                        th.loading = true;
                        var apipath = 'Link/Sort' + (th.id == '' ? 'add' : 'Modify');
                        $api.post(apipath, { 'entity': th.entity }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success', center: true,
                                    message: isclose ? '保存成功，并关闭！' : '保存当前编辑成功！'
                                });
                                th.operateSuccess(isclose);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                        }).finally(() => th.loading = false);
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //操作成功
            operateSuccess: function (isclose) {
                window.top.$pagebox.source.tab(window.name, 'vapp.handleCurrentChange', isclose);
            }
        },
    });

});
