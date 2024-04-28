
$ready(function () {

    window.vapp = new Vue({
        el: '#app',
        data() {
            let checkNum = (rule, value, callback) => {
                if (!value && value != 0) {
                    callback(new Error('必填信息'));
                } else {
                    if (!Number.isInteger(value)) {
                        callback(new Error('请输入整数'));
                    } else {
                        return callback();
                    }
                }
            }
            let checkDate = (rule, value, callback) => {
                if (this.datespan) {
                    if (this.datespan.length == 0) {
                        return callback(new Error('请选择时间'));
                    }
                    return callback();
                } else {
                    return callback();
                }
            }
            return {
                id: $api.querystring('id'),
                minLength: 0,        //充值卡的卡号最小长度
                entity: {
                    Rs_Count: 100,
                    Rs_IsEnable: true,
                    Rs_CodeLength: 16,
                    Rs_PwLength: 3
                },

                datespan: [],            //时间区间,内有两个值，一个开始，一个结束
                rules: {
                    Rs_Theme: [
                        { required: true, message: '不得为空', trigger: ["blur", "change"] },
                        { min: 1, max: 200, message: '长度在 1 到 200 个字符', trigger: 'blur' },
                        {
                            validator: async function (rule, value, callback) {
                                await vapp.isExist(value).then(res => {
                                    if (res) callback(new Error('已经存在!'));
                                });
                            }, trigger: 'blur'
                        }
                    ],
                    Rs_Price: [
                        { required: true, message: '不得为空', trigger: ["blur", "change"] },
                        { type: 'number', min: 1, message: '不能小于等于0', trigger: ["blur", "change"] }
                    ],
                    Rs_Count: [
                        { required: true, message: '不得为空', trigger: ["blur", "change"] },
                        { validator: checkNum, trigger: ["blur", "change"] },
                        { type: 'number', min: 1, message: '不能小于等于0', trigger: ["blur", "change"] }
                    ],
                    Rs_CodeLength: [
                        { required: true, message: '不得为空', trigger: ["blur", "change"] },
                        { validator: checkNum, trigger: ["blur", "change"] },
                        {
                            validator: function (rule, value, callback) {
                                var count = window.vapp.entity.Rs_Count;
                                var min_len = String(count).length + 1;
                                min_len += window.vapp.minLength;
                                if (value < min_len) {
                                    callback(new Error('卡号长度不得小于' + min_len));
                                } else {
                                    return callback();
                                }
                            }, trigger: ["blur", "change"]
                        },
                        { type: 'number', min: 1, message: '不能小于等于0', trigger: ["blur", "change"] },
                        { type: 'number', max: 20, message: '不能大于20', trigger: ["blur", "change"] }
                    ],
                    Rs_PwLength: [
                        { required: true, message: '不得为空', trigger: ["blur", "change"] },
                        { validator: checkNum, trigger: ["blur", "change"] },
                        { type: 'number', min: 1, message: '不能小于等于0', trigger: ["blur", "change"] },
                        { type: 'number', max: 8, message: '不能大于8', trigger: ["blur", "change"] }
                    ],
                    Rs_Limit: [
                        { validator: checkDate, type: 'date', trigger: ["blur", "change"] }
                    ]
                },
                loading: false,
                lading_init: true        //初始化
            }
        },
        computed: {
            //是否新增对象
            isadd: t => { return t.id == null || t.id == ''; },
        },
        watch: {
            //有效期
            'datespan': { //监听的对象
                deep: true, //深度监听设置为 true
                handler: function (newV, oldV) {
                    if (newV != null) {
                        this.entity.Rs_LimitStart = newV[0];
                        this.entity.Rs_LimitEnd = newV[1];
                    }
                }
            }
        },
        created: function () {
            var th = this;
            $api.get('RechargeCode/MinLength').then(function (req) {
                if (req.data.success) {
                    th.minLength = req.data.result;
                    if (th.id == '') return;
                    th.loading = true;
                    $api.get('RechargeCode/SetForID', { 'id': th.id }).then(function (req) {
                        if (req.data.success) {
                            th.entity = req.data.result;
                            th.datespan.push(th.entity.Rs_LimitStart);
                            th.datespan.push(th.entity.Rs_LimitEnd);
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(err => console.error(err)).finally(th.loading = false);
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(th.lading_init = false);
        },
        methods: {
            //判断是否已经存在
            isExist: function (val) {
                var th = this;
                return new Promise(function (resolve, reject) {
                    $api.get('RechargeCode/SetExist', { 'name': val, 'id': th.id }).then(function (req) {
                        if (req.data.success) {
                            return resolve(req.data.result);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        return reject(err);
                    });
                });
            },
            btnEnter: function (formName, isclose) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        var apipath = 'RechargeCode/Set' + (th.id == '' ? api = 'add' : 'Modify');
                        $api.post(apipath, { 'entity': th.entity }).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                th.operateSuccess(isclose);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            console.error(err);
                            alert(err, '错误');
                        }).finally(th.loading = false);
                    } else {
                        console.error('error submit!!');
                    }
                });
            },
            //操作成功
            operateSuccess: function (isclose) {
                if (window.top.$pagebox)
                    window.top.$pagebox.source.tab(window.name, 'vapp.freshrow("' + this.id + '")', isclose);

            }
        },
    });

});
