
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
                th.loading = false;
                if (req.data.success) {
                    var result = req.data.result;
                    vapp.entity = result;
                } else {
                    throw '未查底到数据';
                }
            }).catch(function (err) {
                vapp.$alert(err, '错误');
            });

        },
        methods: {
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        var apipath = 'Link/Sort' + (th.id == '' ? 'add' : 'Modify');
                        $api.post(apipath, { 'entity': th.entity }).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                vapp.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                window.setTimeout(function () {
                                    vapp.operateSuccess();
                                }, 600);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            vapp.$alert(err, '错误');
                        });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vapp.handleCurrentChange', true);
            }
        },
    });

});
