
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: function () {
            //config的字段是否为空
            let isrequired = function (rule, value, callback) {
                var field = rule.field;
                if (!!vapp.config[field] && vapp.config[field] != '')
                    return callback();
                callback(new Error("不得为空"));
            };
            return {
                id: $api.querystring('id'),
                //当前对象    
                entity: {
                    Pai_IsEnable: true,
                    Pai_Platform: 'mobi'
                },
                config: {},      //接口配置项   

                rules: {
                    Pai_Name: [
                        { required: true, message: '不得为空', trigger: 'blur' },
                        {
                            validator: async function (rule, value, callback) {
                                await vapp.isexist(value).then(res => {
                                    if (res) {
                                        var type = vapp.$refs['interface_type'].current;
                                        var name = type.name;
                                        callback(new Error('支付名称在“' + name + '”中已经存在!'));
                                    }

                                });
                            }, trigger: 'blur'
                        }
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
                                if ((value != '' && value != null) && isNaN(Number(value))) {
                                    callback(new Error('请输入数字!'));
                                } else {
                                    callback();
                                }
                            }, trigger: 'blur'
                        }
                    ],
                    MCHID: [{ required: true, validator: isrequired, trigger: 'blur' }],
                    Paykey: [{ required: true, validator: isrequired, trigger: 'blur' }]
                },
                loading: false
            }
        },
        watch: {},
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
            //判断接口名称是否重复
            isexist: function (val) {
                var th = this;
                th.entity.Pai_Name = val;
                var type = th.$refs['interface_type'].current;
                th.entity.Pai_Scene = type.scene;
                th.entity.Pai_Pattern = type.name;
                th.entity.Pai_InterfaceType = type.pattern;
                th.entity.Pai_Platform = type.platform;
                return new Promise(function (resolve, reject) {
                    $api.post('Pay/IsExist', { 'entity': th.entity, 'scope': 2 }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            console.log(result);
                            return resolve(result);
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
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        var obj = $api.clone(th.entity);
                        obj.Pai_Config = $api.xmlconfig.toxml(th.config);
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
