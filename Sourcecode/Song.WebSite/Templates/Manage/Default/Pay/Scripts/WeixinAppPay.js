
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
                                let iftype = vapp.$refs['interface_type'];
                                await iftype.isexist(value).then(res => {
                                    if (res) {
                                        var type = iftype.current;
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
        computed: {},
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
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        methods: {
            //保存信息
            enter: function (obj, isclose, callback) {
                var th = this;
                obj.Pai_Config = $api.xmlconfig.toxml(th.config);
                var apipath = 'Pay/' + (this.id == '' ? api = 'add' : 'Modify');
                th.loading = true;
                $api.post(apipath, { 'entity': obj }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message({
                            type: 'success', center: true,
                            message: '操作成功!',
                        });
                        callback(isclose);
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => alert(err))
                    .finally(() => th.loading = false);

            },
        },
    });

}, ['Components/interface_type.js',
    'Components/modifybox.js']);
