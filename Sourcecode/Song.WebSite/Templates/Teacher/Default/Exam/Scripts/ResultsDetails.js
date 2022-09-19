
$ready(function () {

    window.vapp = new Vue({
        el: '#app',
        data: {
            id: $api.querystring('id'),
            entity: {}, //当前对象    
            profits: [], //分润方案列表
            profit_id: '',   //当前分润方案
            rules: {
                Olv_Name: [
                    { required: true, message: '名称不得为空', trigger: 'blur' }
                ],
                Olv_Tag: [
                    { required: true, message: '标识不得为空', trigger: 'blur' },
                    { min: 4, max: 20, message: '长度在 4 到 20 个字符', trigger: 'blur' }
                ]
            },
            loading: false
        },
        watch: {
            'profit_id': function (nl, ol) {
                this.entity.Ps_ID = nl;
            }
        },
        created: function () {
            var th = this;
            if (th.id != '') {
                $api.get('Exam/ForID', { 'id': th.id }).then(function (req) {
                    if (req.data.success) {
                        th.entity = req.data.result;                     
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    console.error(err);
                });
            } else {
                th.entity.Olv_IsUse = true;
            }
            
        },
        methods: {
            btnEnter: function (formName) {
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        var apipath = 'Organization/Level' + (this.id == '' ? api = 'add' : 'Modify');
                        $api.post(apipath, { 'entity': vapp.entity }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                vapp.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                vapp.operateSuccess();
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            //window.top.ELEMENT.MessageBox(err, '错误');
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
                window.top.$pagebox.source.tab(window.name, 'vue.handleCurrentChange', true);
            }
        },
    });

});
