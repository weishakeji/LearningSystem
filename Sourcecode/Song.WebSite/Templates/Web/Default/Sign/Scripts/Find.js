$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项        

            loading_init: true,
            //输入的学员账号
            acc: {
                input: '',   //输入信息
                vcode: '',   //校验码
                base64: '', md5: '', loading: false
            },
            accrules: {
                input: [
                    { required: true, message: '账号不得为空', trigger: 'blur' },
                    { min: 5, max: 50, message: '长度在 5 到 50 个字符', trigger: 'blur' }
                ],
                vcode: [
                    { required: true, message: '不得为空', trigger: 'input' },
                    { min: 4, max: 4, message: '请输入4位字符', trigger: 'blur' },
                    {
                        validator: function (rule, value, callback) {
                            var md5 = $api.md5(value);
                            if (md5 != vapp.acc.md5)
                                callback(new Error('校验码错误'));
                            else
                                callback();
                        }, trigger: 'input'
                    }
                ],
            },
            //安全问题
            ques: {
                input: '',   //输入信息
                error: false,   //是否错误
                message: ''  //提示信息
            },
            //重置密码
            pw: {
                input1: '',
                input2: '',
                error: false,
                message: ''
            },
            account: {},     //要操作的学员账号
            loading: false,
            step: 1
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (platinfo, organ) {
                th.loading_init = false;
                //获取结果             
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.getvcode();
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {

        },
        watch: {
        },
        methods: {
            //加载验证码图片
            getvcode: function () {
                var th = this;
                th.acc.loading = true;
                $api.post('Helper/CodeImg', { 'leng': 4, 'type': 5, 'acc': '' }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.acc.base64 = result.base64;
                        th.acc.md5 = result.value;
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.acc.loading = false);
            },
            //测试账号是否存在
            testAcc: function (formName) {
                var field = this.$refs[formName].fields[0];
                this.$refs[formName].clearValidate();
                field.error = '';
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        var th = this;
                        if (th.checkcount(3)) {
                            field.error = '每分钟最多3次，稍后再试';
                            alert(field.error);
                            return;
                        };
                        $api.get('Account/ForAcc', { 'acc': th.acc.input }).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                th.account = req.data.result;
                                th.step = 2;
                            } else {
                                field.error = '账号不存在';
                                throw field.error;
                            }
                        }).catch(function (err) {
                            //alert(err);
                            console.error(err);
                        });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //校验次数
            checkcount: function (most) {
                var storage = 'checkcount_register';
                var array = $api.storage(storage);
                if (array == null) array = [];
                //判断每分钟的次数
                let count = 0;
                var max = new Date(), min = new Date(new Date().getTime() - 1000 * 60);
                for (let i = 0; i < array.length; i++) {
                    if (array[i] > min && array[i] <= max)
                        count++;
                }
                if (count < most) {
                    array.push(new Date());
                    $api.storage(storage, array);
                }
                return count >= most;
                console.log(max.format('yyyy-MM-dd HH:mm:ss'));
                console.log(min.format('yyyy-MM-dd HH:mm:ss'));
            },
            //测试安全问题是否正确
            testQues: function () {
                var th = this;
                this.ques.error = false;
                this.ques.message = '';
                this.loading = true;
                $api.get('Account/CheckQues', { 'acc': th.account.Ac_AccName, 'answer': th.ques.input }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.account = req.data.result;
                        th.step = 3;
                    } else {
                        th.ques.error = true;
                        th.ques.message = '安全问题回答不正确';
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            },
            //设置密码
            setupPw: function () {
                var th = this;
                if ($api.trim(this.pw.input1) == '' || $api.trim(this.pw.input2) == '') {
                    this.pw.error = true;
                    this.pw.message = '密码不得为空';
                    return;
                }
                if ($api.trim(this.pw.input1) != $api.trim(this.pw.input2)) {
                    this.pw.error = true;
                    this.pw.message = '两次密码输入不同';
                    return;
                }
                this.pw.error = false;
                this.pw.message = '';
                this.loading = true;
                var query = { 'acc': this.account.Ac_AccName, 'answer': th.ques.input, 'pw': th.pw.input2 };
                $api.post('Account/ModifyPwForAnswer', query).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.$alert('设置密码成功', '成功', {
                            confirmButtonText: '确定',
                            callback: () => {
                                window.location.href = 'in';
                            }
                        });
                    } else {
                        th.pw.error = true;
                        th.pw.message = req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            }
        }
    });

}, ['../Components/page_header.js']);
