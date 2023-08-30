$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项        
            datas: {},
            loading_init: true,
            //输入的学员账号
            acc: {
                input: '',   //输入信息
                error: false,   //是否错误
                message: ''  //提示信息
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
            $api.bat(
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (platinfo, organ) {
                vapp.loading_init = false;
                //获取结果             
                vapp.platinfo = platinfo.data.result;
                vapp.organ = organ.data.result;
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;
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
            //测试账号是否存在
            testAcc: function () {
                var th = this;
                if ($api.trim(this.acc.input) == '') {
                    this.acc.error = true;
                    this.acc.message = '账号不得为空';
                } else {
                    this.acc.error = false;
                    this.acc.message = '';
                    if (th.checkcount(3)) {
                        this.acc.message = '每分钟最多3次，稍后再试';
                        alert(this.acc.message);
                        return;
                    };
                    this.loading = true;
                    $api.get('Account/ForAcc', { 'acc': th.acc.input }).then(function (req) {
                        th.loading = false;
                        if (req.data.success) {
                            th.account = req.data.result;
                            th.step = 2;
                        } else {
                            th.acc.error = true;
                            th.acc.message = '账号不存在';
                        }
                    }).catch(function (err) {
                        console.error(err);
                    });
                }
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
                        th.$dialog.alert({
                            message: '设置密码成功',
                        }).then(() => {
                            window.navigateTo('in');
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
