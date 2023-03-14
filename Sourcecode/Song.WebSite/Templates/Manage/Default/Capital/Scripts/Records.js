$ready(function () {
    window.vue = new Vue({
        el: '#app',
        data: {
            form: {
                start: '',       //时间区间的开始时间
                end: '',         //结束时间
                account: '',      //学员名称或账号
                from: '-1',     //来源
                type: '-1',     //类型，支出或充值
                moneymin: 0,      //金额的选择范围，最小值
                moneymax: '',     //同上,最大值
                serial: '',          //流水号
                state: '-1',       //状态，成功为1，失败为2,-1为所有
                size: 20,
                index: 1
            },
            selectDate: '',
            accounts: [],        //当前页的学员账号
            loading: false,
            loadingid: 0,        //当前操作中的对象id
            querybox: false,     //更多查询的面板是否显示
            datas: [],          //数据集
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行
            pickerOptions: {
                shortcuts: [{
                    text: '最近一周',
                    onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近一个月',
                    onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近三个月',
                    onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
                        picker.$emit('pick', [start, end]);
                    }
                }]
            },
            //资金来源
            moneyform: [{ value: '-1', label: '-来源-' },
            { value: '3', label: '在线支付' },
            { value: '1', label: '管理员充扣' },
            { value: '4', label: '购买课程' }],
            //操作类型
            moneytype: [{ value: '-1', label: '全部' },
            { value: '1', label: '支出' },
            { value: '2', label: '充值' }],
            //操作状态
            moneystate: [{ value: '-1', label: '全部' },
            { value: '1', label: '成功' },
            { value: '2', label: '失败' }],

            loading_query: 0     //订单查询
        },
        mounted: function () {
        },
        created: function () {
            //日期范围的初始时间值
            const end = new Date();
            const start = new Date();
            start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
            this.selectDate = [];
            this.selectDate[0] = start;
            this.selectDate[1] = end;
            //
            this.handleCurrentChange();
        },
        computed: {
            //表格高度
            tableHeight: function () {
                var height = document.body.clientHeight;
                return height - 75;
            }
        },
        watch: {
            //选择时间区间
            selectDate: function (nv, ov) {
                if (!nv) return;
                this.form.start = nv[0];
                this.form.end = nv[1];
            }
        },
        methods: {
            //删除
            deleteData: function (datas) {
                $api.delete('Money/Delete', { 'id': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        vue.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        window.vue.handleCurrentChange();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                });
            },
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                th.loading = true;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 105;
                th.form.size = Math.round(area / 43);
                $api.get('Money/Pager', th.form).then(function (d) {
                    th.loading = false;
                    if (d.data.success) {
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    th.loading = false;
                });
            },
            //双击事件
            rowdblclick: function (row, column, event) {
                console.log(row);
                this.$refs.btngroup.modifyrow(row, '查看');
            },
            //导出按钮的事件
            btnOutput: function () {
                var file = 'RecordOutput';
                var title = ' - 导出';
                var boxid = 'Capital_' + file;
                this.$refs.btngroup.pagebox(file, title, boxid, 800, 400,
                    { pid: window.name, resize: true });
            },
            //查询订单
            queryOrder: function (detail) {
                var th = this;
                th.loading_query = detail.Ma_ID;
                var query = { 'serial': detail.Ma_Serial };
                $api.get('Pay/Interface', { 'id': detail.Pai_ID }).then(function (req) {
                    if (req.data.success) {
                        var pi = req.data.result;
                        //如果是微信支付
                        if (pi.Pai_Pattern.indexOf('微信') > -1) {
                            query['appid'] = pi.Pai_ParterID;
                            //接口配置项
                            let config = $api.xmlconfig.tojson(pi.Pai_Config);
                            query['mchid'] = config["MCHID"];    //商户id
                            query['paykey'] = config["Paykey"];  //支付密钥
                            $api.get('Pay/WxOrderQuery', query).then(function (req) {
                                if (req.data.success) {
                                    var result = req.data.result;
                                    if (result['trade_state'] == 'SUCCESS') {
                                        $api.get('Pay/MoneyAccount', { 'serial': result['out_trade_no'] }).then(function (req) {
                                            if (req.data.success) {
                                                let item = req.data.result;
                                                var index = th.datas.findIndex(n => n.Ma_Serial == item.Ma_Serial);
                                                if (index > -1)
                                                    th.$set(th.datas, index, item);
                                            } else {
                                                console.error(req.data.exception);
                                                throw req.config.way + ' ' + req.data.message;
                                            }
                                        }).catch(function (err) {
                                            alert(err);
                                            console.error(err);
                                        });
                                    }
                                    console.log(result);
                                } else {
                                    console.error(req.data.exception);
                                    throw req.config.way + ' ' + req.data.message;
                                }
                            }).catch(err => console.error(err))
                                .finally(() => th.loading_query = 0);
                        }
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err));
            }
        }
    });

});