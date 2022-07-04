
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            //当前对象    
            entity: {
                Pai_IsEnable: true,
                Pai_Platform: 'web'
            },
            config: {},      //接口配置项   
            rules: {
                Pai_Name: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ],
                Pai_ParterID: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ],
                Pai_Key: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ],
                Pai_Returl: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ],
                Pai_Feerate: [
                    {
                        validator: function (rule, value, callback) {
                            if (value != '' && isNaN(Number(value)))  {
                                callback(new Error('请输入数字!'));
                            } else {
                                callback();
                            }
                        }, trigger: 'blur'
                    }
                ]
            },
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
                    th.config = $api.xmlconfig.tojson(th.entity.Pai_Config);
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
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        var obj = $api.clone(th.entity);
                        obj.Pai_Config = $api.xmlconfig.toxml(th.config);
                        //类型
                        var type = th.$refs['interface_type'].current;
                        obj.Pai_Scene = type.scene;
                        obj.Pai_Pattern = type.name;
                        obj.Pai_Platform = 'web';
                        console.log(type);
                        // return;
                        var apipath = 'Pay/' + (this.id == '' ? api = 'add' : 'Modify');
                        $api.post(apipath, { 'entity': obj }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
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
                            //window.top.ELEMENT.MessageBox(err, '错误');
                            th.$alert(err, '错误');
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
