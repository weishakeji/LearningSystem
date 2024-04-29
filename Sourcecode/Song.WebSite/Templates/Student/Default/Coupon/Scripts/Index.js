$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            organ: {},
            config: {},      //当前机构配置项       

            form: { 'acid': '', 'start': '', 'end': '', 'type': '-1', 'search': '', 'size': 10, 'index': 1 },           

            datas: [],      //数据集，此处是学员列表
            total: 1, //总记录数
            totalpages: 1, //总页数
            //使用充值卡
            useCardShow: false,      //使用学习卡的面板是否打开
            useCardForm: {
                card: ''
            },
            useCardRules: {
                'card': [
                    { required: true, message: '不得为空', trigger: ["blur", "change"] },
                    {
                        validator: function (rule, value, callback) {
                            if (value.indexOf('-') < 0) {
                                callback(new Error('格式为“学习卡-密码”，破折号不可缺少'));
                            } else {
                                var arr = value.split('-');
                                if (!(/^\d+(\.\d+)?$/.test($api.trim(arr[0])))) {
                                    callback(new Error('卡号必须为数字'));
                                }
                                if (arr.length > 1) {
                                    if (!(/^\d+(\.\d+)?$/.test($api.trim(arr[1])))) {
                                        callback(new Error('密码必须为数字'));
                                    }
                                }
                            }
                            return callback();

                        }, trigger: ["blur", "change"]
                    }
                ]
            },

            //兑换充值卡
            exchangeShow: false,
            exchangeForm: {
                point: ''
            },
            param: {},       //积分相关设置
            exchangeRules: {
                'point': [
                    { required: true, message: '不得为空', trigger: ["blur", "change"] },
                    {
                        validator: function (rule, value, callback) {
                            if (!(/^\d+(\.\d+)?$/.test(value))) {
                                callback(new Error('必须为正整数'));
                            } else {
                                var n = Number(value);
                                if (n <= 0) {
                                    callback(new Error('必须大于零'));
                                } else if (n > vapp.account.Ac_Point) {
                                    callback(new Error('不得大于当前拥有的积分数'));
                                } else {
                                    var result = Math.floor(n / vapp.param.PointConvert);
                                    if (result <= 0) {
                                        callback(new Error('当前积分不足以兑换一个卡券'));
                                    }
                                }
                            }
                            return callback();

                        }, trigger: ["blur", "change"]
                    }
                ]
            },

            loading_init: true,
            loading_up: false,
            loading: false
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Account/Current'),
                $api.get('Point/Param')
            ).then(axios.spread(function (account, param) {
                //获取结果
                th.account = account.data.result;
                th.form.acid = th.account.Ac_ID;
                th.handleCurrentChange();
                th.param = param.data.result;
            })).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        created: function () {

        },
        computed: {

        },
        watch: {
           
        },
        methods: {
            //获取当前登录账号
            getAccount: function () {
                var th = this;
                $api.post('Account/Current').then(function (req) {
                    if (req.data.success) {
                        th.account = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                    th.$toast.success(err);
                });
            },
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 115;
                th.form.size = Math.floor(area / 47);
                th.loading = true;
                $api.get("Coupon/PagerForAccount", th.form).then(function (d) {
                    th.loading = false;
                    if (d.data.success) {
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                        //console.log(th.accounts);
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //使用充值卡
            useCard: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading_up = true;
                        $api.post('Coupon/Recharge', { 'code': th.useCardForm.card }).then(function (req) {
                            th.loading_up = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$alert('通过使用该充值卡，您成功充值' + result.Ca_Value + '个卡券', '操作成功', {
                                    confirmButtonText: '确定',
                                    callback: action => {
                                        th.handleCurrentChange();
                                        th.useCardForm.card = '';
                                        th.useCardShow = false;
                                    }
                                });
                                th.getAccount();
                            } else {
                                console.error(req.data.exception);
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                            console.error(err);
                        }).finally(() => th.loading_up = false);
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //计算示例
            calc_demo: function (point) {
                //计算兑换结果
                if (point == null) point = this.account.Ac_Point;
                var result = Math.floor(point / this.param.PointConvert);
                return result;
            },
            //兑换卡券的按钮事件
            exchange: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        console.log(3);
                        var num = parseInt(th.exchangeForm.point);
                        if (isNaN(num)) return;
                        if (num > th.account.Ac_Point) return;
                        var coupon = Math.floor(num / th.param.PointConvert);
                        if (coupon <= 0) return;
                        th.loading_up = true;
                        $api.get('Point/Exchange', { 'coupon': coupon }).then(function (req) {
                            th.loading_up = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$alert('成功兑换' + result.Ca_Value + '个卡券', '操作成功', {
                                    confirmButtonText: '确定',
                                    callback: action => {
                                        th.handleCurrentChange();
                                        th.exchangeForm.point = '';
                                        th.exchangeShow = false;
                                    }
                                });
                            } else {
                                console.error(req.data.exception);
                                throw req.data.message;
                            }
                        }).catch(err => console.error(err))
                            .finally(() => th.loading_up = false);
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            }
        }
    });

});
