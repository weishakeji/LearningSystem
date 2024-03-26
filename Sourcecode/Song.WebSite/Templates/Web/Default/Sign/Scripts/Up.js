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
                code: '', vcode: '', sms: '', vsms: ''
            },
            //录入校验的规划
            rules: {
                acc: [
                    { required: true, message: '不得为空', trigger: 'change' },
                    {
                        validator: function (rule, value, callback) {
                            if (vapp.isRegSms) {//如果手机号注册，则验证是否是手机号
                                if (!/^[1][3-9][0-9]{9}$/i.test(value)) {
                                    callback(new Error('请输入正确的手机号码'));
                                } else {
                                    callback();
                                }
                            } else {
                                if (/[\u4E00-\u9FA5]/i.test(value)) {
                                    callback(new Error('账号不允许用中文'));
                                } else if (!/^[a-zA-z_]|[0-9]$/.test(value)) {
                                    callback(new Error('仅限字母和数字'));
                                } else if (value.length < 6) {
                                    callback(new Error('账号长度需大于等于6个字符'));
                                } else {
                                    callback();
                                }
                            }
                        }, trigger: 'change'
                    },
                    {
                        validator: async function (rule, value, callback) {
                            if (value == '') return callback();
                            await vapp.isExist(value).then(res => {
                                if (res) callback(new Error('账号已经存在'));
                            });
                        }, trigger: 'blur'
                    }
                ],
                pw1: [
                    { required: true, message: '请输入密码', trigger: 'change' },
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
                    { required: true, message: '不得为空', trigger: 'change' },
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
                code: [
                    { required: true, message: '不得为空', trigger: 'change' },
                    {
                        validator: function (rule, value, callback) {
                            if (!/[0-9]{4,4}$/.test(value)) {
                                callback(new Error('必须为4位数字!'));
                            } else {
                                callback();
                            }
                        }, trigger: 'blur'
                    },
                    {
                        validator: async function (rule, value, callback) {
                            if (value == '') return callback();
                            await vapp.isVerifyVcode(value).then(res => {
                                if (!res) callback(new Error('校验码错误'));
                            });
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
            //短信验证
            sms_form: { 'phone': '', 'vcode': '' },
            sms_seconds: 0,        //发送短信后的数秒
            sms_storage: 'register_sms_seconds',
            sms_rules: {
                sms: [
                    { required: true, message: '不得为空', trigger: 'change' },
                    {
                        validator: function (rule, value, callback) {
                            if (!/[0-9]{4,4}$/.test(value)) {
                                callback(new Error('必须为6位数字!'));
                            } else {
                                callback();
                            }
                        }, trigger: 'blur'
                    },
                    {
                        validator: async function (rule, value, callback) {
                            if (value == '') return callback();
                            await vapp.isVerifySms(value).then(res => {
                                if (!res) callback(new Error('短信验证错误'));
                            });
                        }, trigger: 'blur'
                    }
                ]
            },
            loading_sms: false,      //验证码加载

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
            existref: t => !$api.isnull(t.referrer),
            //注册时是否需要短信验证
            isRegSms: t => t.config && t.config.IsRegSms === true
        },
        watch: {
            //是否选中协议“已经阅读”的选项
            'agreement.checked': {
                handler: function (nv, ov) {
                    if (nv) this.agreement.show = false;
                }, deep: true
            },
            //机构
            'organ': {
                handler: function (nv, ov) {
                    if (!$api.isnull(nv)) this.getvcode();
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
                }).catch(err => alert(err));
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
                        th.form.vcode = result.value;
                        $dom("#vcode").attr('src', vcodebase64);
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                });
            },
            //获取短信
            getsms: function () {
                var th = this;
                th.$delete(th.rules, 'sms');
                let form = th.$refs['register'];
                form.clearValidate();
                form.rules = th.rules;
                form.validate(function (valid) {
                    if (valid) {
                        th.loading_sms = true;
                        $api.post('Sms/SendVcode', { 'phone': th.form.acc, 'len': 6 }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;   //校验码
                                th.countdown(result);
                                window.register_sms_seconds = window.setInterval(function () {
                                    th.countdown();
                                }, 1000);

                            } else {
                                console.error(req.data.exception);
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                            console.error(err);
                        }).finally(() => th.loading_sms = false);
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
                return;
            },
            //倒计时
            countdown: function (vcode) {
                let storage = this.sms_storage;
                var obj = $api.storage(storage);
                if (obj == undefined) obj = {};
                if (vcode != '' && vcode != undefined) {
                    obj = { 'second': 60, 'expire': new Date(new Date().getTime() + 1000 * 180), 'vcode': vcode };
                }
                else {
                    if (obj.second > 0) obj.second--;
                    else window.clearInterval(window.register_sms_seconds);
                }
                this.sms_seconds = obj.second;
                $api.storage(storage, obj);
                console.log(obj.second);
            },

            //判断账号是否存在
            isExist: function (val) {
                return new Promise(function (resolve, reject) {
                    $api.get('Account/IsExistAcc', { 'acc': val, 'id': -1 }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            return resolve(result);
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        return reject(err);
                    });
                });
            },
            //判断图片校验码是否正确
            isVerifyVcode: function (val) {
                return new Promise(function (resolve, reject) {
                    $api.get('Account/RegisterVerifyVcode', { 'code': val, 'vcode': vapp.form.vcode }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            console.log(result);
                            return resolve(result);
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        return reject(err);
                    });
                });
            },
            //判断短信验证码是否正确
            isVerifySms: function (val) {
                let storage = this.sms_storage;
                var obj = $api.storage(storage);
                let ciphertext = '';
                if (obj != undefined) ciphertext = obj.vcode;
                return new Promise(function (resolve, reject) {
                    $api.get('Account/RegisterSMSVcode', { 'sms': val, 'vsms': ciphertext }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            console.log(result);
                            return resolve(result);
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        return reject(err);
                    });
                });
            },
            //按钮提交事件
            btn_submit: function () {
                if (!this.agreement.checked) {
                    this.$alert({
                        message: '请阅读并同意《平台协议》',
                    });
                    return;
                }
                var th = this;
                th.$set(th.rules, 'sms', th.sms_rules['sms']);
                let form = th.$refs['register'];
                form.clearValidate();
                form.rules = th.rules;
                //短信验证码
                let storage = this.sms_storage;
                var obj = $api.storage(storage);
                if (obj != undefined) th.form.vsms = obj.vcode;
                form.validate((valid) => {
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
                                    $api.login.in('account', result.Ac_Pw, result.Ac_ID);
                                    th.$alert('注册成功！', '成功', {
                                        confirmButtonText: '确定',
                                        callback: () => {
                                            th.goback(result);
                                        }
                                    });
                                    th.$refs['login'].success(result, 'web端', '注册登录', '');
                                } else {
                                    $api.login.out('account', '');
                                    //需要审核
                                    th.$alert('注册成功，请等待审核！', '成功', {
                                        confirmButtonText: '确定',
                                        callback: () => {
                                            th.goback(result);
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
             //已经登录
             successful: function (account) {             
            },
            //注册成功后的跳转
            goback: function (account) {
                let referrer = $api.querystring('referrer');
                if ($api.isnull(referrer)) referrer = $api.storage('singin_referrer');
                if ($api.isnull(referrer)) referrer = '/';
                //注册成功后，是否需要填写详情
                if (this.config && this.config.IsRegDetail === true) {
                    window.navigateTo($api.url.set('detail', {
                        'referrer': referrer,
                        'acid': account.Ac_ID,
                        'uid': account.Ac_CheckUID
                    }));
                    return;
                }
                window.navigateTo(decodeURIComponent(referrer));
            }
        }
    });

}, ['/Utilities/Components/Sign/Login.js']);
