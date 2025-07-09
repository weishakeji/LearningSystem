$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: function () {
            var validatePass = function (rule, value, callback) {
                var th = vapp;
                //结课成绩的权重
                var finaltest_weight_video = !!th.config.finaltest_weight_video ? th.config.finaltest_weight_video : 0;
                var finaltest_weight_ques = !!th.config.finaltest_weight_ques ? th.config.finaltest_weight_ques : 0;
                var finaltest_weight_exam = !!th.config.finaltest_weight_exam ? th.config.finaltest_weight_exam : 0;
                var finaltest_score = finaltest_weight_video + finaltest_weight_ques + finaltest_weight_exam;
                if (finaltest_score != 100) {
                    callback(new Error('各项权重必须合计等于100%'));
                    return false;
                } else {
                    callback();
                }
            }
            return {
                organ: {},
                config: {},      //当前机构配置项
                rules: {
                    'finaltest_weight_video': [
                        { required: true, message: '不得为空', trigger: 'blur' },
                        { validator: validatePass, trigger: 'blur' }
                    ],
                    'finaltest_weight_ques': [
                        { required: true, message: '不得为空', trigger: 'blur' },
                        { validator: validatePass, trigger: 'blur' }
                    ],
                    'finaltest_weight_exam': [
                        { required: true, message: '不得为空', trigger: 'blur' },
                        { validator: validatePass, trigger: 'blur' }
                    ],
                    'finaltest_score_pass': [
                        { required: true, message: '及格分不得为空', trigger: 'blur' }
                    ],
                },
                loading: false
            };
        },

        mounted: function () {
            var th = this;
            th.loading = true;
            $api.bat(
                $api.get('Organization/Current')
            ).then(([organ]) => {
                //获取结果             
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        created: function () { },
        computed: {},
        watch: {},
        methods: {
            validatePass: function (rule, value, callback) {
                var th = vapp;
                //结课成绩的权重
                var finaltest_weight_video = !!th.config.finaltest_weight_video ? th.config.finaltest_weight_video : 0;
                var finaltest_weight_ques = !!th.config.finaltest_weight_ques ? th.config.finaltest_weight_ques : 0;
                var finaltest_weight_exam = !!th.config.finaltest_weight_exam ? th.config.finaltest_weight_exam : 0;
                var finaltest_score = finaltest_weight_video + finaltest_weight_ques + finaltest_weight_exam;
                if (finaltest_score != 100) {
                    callback(new Error('各项权重必须合计等于100%'));
                    return false;
                } else {
                    callback();
                }

                return true;
                return;
                if (value === '') {
                    callback(new Error('请输入密码'));
                } else {
                    if (this.ruleForm.checkPass !== '') {
                        this.$refs.ruleForm.validateField('checkPass');
                    }
                    callback();
                }
            },
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid, fields) => {
                    if (valid) {
                        //仅保留当前页要修改的字段
                        let fields = th.$refs[formName].fields;
                        let data = {};      //          
                        for (let i = 0; i < fields.length; i++)
                            data[fields[i].prop] = this.config[fields[i].prop];
                        //保存    
                        if (th.loading) return;         
                        th.loading = true;
                        $api.post('Organization/ConfigUpdate', {
                            "orgid": th.organ.Org_ID, 'config': data
                        }).then(function (req) {
                            if (req.data.success && req.data.result) {
                                th.$message({
                                    type: 'success', center: true,
                                    message: '操作成功!'
                                });
                            } else {
                                throw req.data.message;
                            }
                        }).catch(err => alert(err, '错误'))
                            .finally(() => th.loading = false);
                    }
                });
            },
        }
    });

});
