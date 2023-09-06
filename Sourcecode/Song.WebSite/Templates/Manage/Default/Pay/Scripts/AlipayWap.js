
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
                    Privatekey: [{ required: true, validator: isrequired, trigger: 'blur' }]
                },              
            }
        },
        watch: {},
        mounted: function () {},
        methods: {},
    });

}, ['Components/interface_type.js',
    'Components/modifybox.js']);
