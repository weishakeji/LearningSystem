$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},
            organ: {},
            config: {},
            plate: {},
            referrer: {},            //推荐人

            //输入的信息
            form: {
                acc: '', pw1: '', pw2: '',
                rec: '', recid: 0,
                vcode: '', vcodemd5: ''
            },
            rules: {
                acc: [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    {
                        validator: function (rule, value, callback) {
                            if (/[\u4E00-\u9FA5]/i.test(value)) {
                                callback(new Error('账号不允许用中文'));
                            } else if (!/^[a-zA-z_]|[0-9]$/.test(value)) {
                                callback(new Error('仅限字母和数字'));
                            } else if (value.length < 6) {
                                callback(new Error('账号长度需大于等于6个字符'));
                            } else {
                                callback();
                            }
                        }, trigger: 'blur'
                    }
                ],
                pw1: [
                    { required: true, message: '请输入密码', trigger: 'blur' },
                    {
                        validator: function (rule, value, callback) {
                            if (value.length < 6) {
                                callback(new Error('长度需大于等于6个字符'));
                            } else {
                                callback();
                            }
                        }, trigger: 'blur'
                    }],
                pw2: [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    {
                        validator: function (rule, value, callback) {
                            if (value !== vapp.form.pw1) {
                                callback(new Error('两次输入密码不一致!'));
                            } else {
                                callback();
                            }
                        }, trigger: 'blur'
                    }
                ],
                vcode: [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    {
                        validator: function (rule, value, callback) {
                            if (!/[0-9]{4,4}$/.test(value)) {
                                callback(new Error('必须为4位数字!'));
                            } else {
                                callback();
                            }
                        }, trigger: 'blur'
                    }
                ]
            },
            //注册协议
            agreement: {
                text: '',        //协议内容
                show: false,     //是否显示协议
                checked: false       //是否选择“已经同意协议”
            },
            loading_submit: false,      //提交数据
            loading_init: true,
            loading_vcode: false     //校验码加载
        },
        mounted: function () {
            this.getAgreement();
            this.getreferrer();
        },
        created: function () { },
        computed: {
            //是否有推荐人
            existref: function () {
                return JSON.stringify(this.referrer) != '{}' && this.referrer != null;
            },
        },
        watch: {
            'agreement.checked': {
                deep: true,
                handler: function (nv, ov) {
                    if (nv) this.agreement.show = false;
                }
            },
            'organ': {
                handler: function (nv, ov) {
                    if (JSON.stringify(nv) != '{}' && nv != null) {
                        this.getvcode();
                    }
                }, immediate: true
            }
        },
        methods: {
            //查看注册协议
            showagreement: function () {
                if (this.agreement.text != '') {
                    this.agreement.show = true;
                    return;
                }
                this.getAgreement(this.agreement.show);
            },
            //获取推荐账号
            getreferrer: function () {
                if (window.sharekeyid == null || window.sharekeyid == '') return;
                var th = this;
                $api.get('Account/ForID', { 'id': window.sharekeyid }).then(function (req) {
                    if (req.data.success) {
                        th.referrer = req.data.result;
                        if (th.existref) {
                            th.form.rec = '推荐人：' + th.referrer.Ac_Name;
                            th.form.recid = th.referrer.Ac_ID;
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err));
            },
            //获取校验吗
            getvcode: function () {
                var th = this;
                th.loading_vcode = true;
                //校验码生成
                $api.post('Helper/CheckCodeImg', { 'leng': 4, 'acc': th.organ.Org_PlatformName }).then(function (req) {
                    th.loading_vcode = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        var vcodebase64 = result.base64;
                        th.form.vcodemd5 = result.value;
                        console.log(th.form.vcodemd5);
                        //
                        $dom("#vcode").attr('src', vcodebase64);
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                });
            },
            //获取注册协议的内容
            getAgreement: function (show) {
                var th = this;
                $api.get('Platform/RegisterAgreement').then(function (req) {
                    if (req.data.success) {
                        th.agreement.text = req.data.result;
                        if (show != null) show = true;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //按钮提交事件
            btn_submit: function (formName) {
                if (!this.agreement.checked) {
                    this.$alert({
                        message: '请阅读并同意《平台协议》',
                    });
                    return;
                }
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading_submit = true;
                        //修订一下注册信息
                        var entity = $api.clone(th.form);
                        entity["pw"] = th.form.pw1;
                        delete entity["pw1"];
                        delete entity["pw2"];
                        $api.post('Account/Register', entity).then(function (req) {
                            th.loading_submit = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                if (result.Ac_IsPass) {
                                    //直接注册通过，写入登录状态
                                    $api.loginstatus('account', result.Ac_Pw, result.Ac_ID);
                                    th.$alert('注册成功！', '成功', {
                                        confirmButtonText: '确定',
                                        callback: () => {
                                            th.goback();
                                        }
                                    });
                                    th.$refs['login'].success(result, 'web端', '注册登录', '');
                                } else {
                                    //需要审核
                                    th.$alert('注册成功，请等待审核！', '成功', {
                                        confirmButtonText: '确定',
                                        callback: () => {
                                            th.goback();
                                        }
                                    });
                                }
                            } else {
                                throw req.data;
                            }
                        }).catch(function (err) {
                            th.$alert(err.message);
                            console.error(err);
                        });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //注册成功后的跳转
            goback: function () {
                var referrer = $api.querystring('referrer');
                if (referrer == null || referrer == '')
                    referrer = $api.storage('singin_referrer');
                //注册成功后，是否需要填写详情
                if (this.config && this.config.IsRegDetail === true) {
                    window.location.href = $api.url.set('detail', {
                        'referrer': referrer
                    });
                    return;
                }
                referrer = decodeURIComponent(referrer);
                window.location.href = referrer != '' ? referrer : '/';
            }
        }
    });

}, ['/Utilities/Components/Sign/Login.js']);
