
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            entity: {}, //当前对象             

            loading: false
        },
        watch: {

        },
        created: function () {
            var th = this;
            if (th.id == '') return;
            th.loading = true;
            $api.get('Pay/ForID', { 'id': th.id }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    th.entity = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                th.loading = false;
                Vue.prototype.$alert(err);
                console.error(err);
            });
        },
        methods: {
            btnEnter: function (formName) {
                return;
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

}, ['Components/interface_type.js']);
