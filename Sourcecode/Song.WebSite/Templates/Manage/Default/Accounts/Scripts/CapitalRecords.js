$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: {
                acid: $api.querystring('id'),      //学员id 
                type: '-1',     //类型，支出或充值
                from: '-1',     //来源
                start: '',       //时间区间的开始时间
                end: '',         //结束时间
                search: '',     //按内容检索
                moneymin: '',      //金额的选择范围，最小值
                moneymax: '',     //同上,最大值
                serial: '',          //流水号               
                state: '-1',       //状态，成功为1，失败为2,-1为所有
                size: 20,
                index: 1
            },
            selectDate: '',
            loading: false,
            loadingid: 0,        //当前操作中的对象id

            datas: [],          //数据集
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行
            entity: null,        //要显示的当前数据行
            pickerOptions: {
                shortcuts: [{
                    text: '最近一周',
                    onClick(p) {
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
                        p.$emit('pick', [start, new Date()]);
                    }
                }, {
                    text: '最近一个月',
                    onClick: (p) => p.$emit('pick', vapp.setTimeInterval(1))
                }, {
                    text: '本月', onClick(picker) {
                        const start = new Date();
                        start.setDate(1);
                        var yy = start.getFullYear();
                        var mm = start.getMonth() + 1;
                        if (mm > 12) {
                            mm = 1;
                            yy = yy + 1;
                        }
                        var end = new Date(yy, mm, 0);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近三个月',
                    onClick: (p) => p.$emit('pick', vapp.setTimeInterval(3))
                }, {
                    text: '本季度', onClick(picker) {
                        const start = new Date();
                        var yy = start.getFullYear();
                        var mm = start.getMonth();
                        if (mm >= 1 && mm <= 3) mm = 0;
                        if (mm >= 4 && mm <= 6) mm = 3;
                        if (mm >= 7 && mm <= 9) mm = 6;
                        if (mm >= 10 && mm <= 12) mm = 9;
                        start.setDate(1);
                        start.setMonth(mm);
                        const end = new Date(yy, mm + 3, 0);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近半年',
                    onClick: (p) => p.$emit('pick', vapp.setTimeInterval(6))
                }, {
                    text: '最近一年',
                    onClick: (p) => p.$emit('pick', vapp.setTimeInterval(12))
                }, {
                    text: '本年', onClick(picker) {
                        const start = new Date();
                        start.setDate(1);
                        start.setMonth(0);
                        const end = new Date(start.getFullYear(), 12, 0);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近三年',
                    onClick: (p) => p.$emit('pick', vapp.setTimeInterval(36))
                }, {
                    text: '最近五年',
                    onClick: (p) => p.$emit('pick', vapp.setTimeInterval(60))
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

            fromType: ['管理员充扣', '充值码充值', '在线支付', '购买课程'],
            loading_query: 0,     //订单查询

            //资金流水导出
            output_panel: false,      //导出面板
            query: {
                path: 'MoneyOutputToExcel_' + $api.querystring('id'),     //导出的文件的存储路径
                acid: $api.querystring('id'),      //学员id 
                from: -1,     //来源
                type: -1,     //类型，支出或充值               
                start: '',       //时间区间的开始时间
                end: ''         //结束时间    
            },
            files: [],          //已经生成的excel文件列表
            loading_out: false
        },
        mounted: function () {
            this.selectDate = this.setTimeInterval(12);
            //
            this.handleCurrentChange();
            this.getFiles();
        },
        created: function () {

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
            //设置时间区间，从当前时间到之前的时间，subtract：要减去的月份
            //返回时间数组
            setTimeInterval: function (subtract) {
                let end = new Date();           // 获取当前时间     
                let month = end.getMonth();     // 获取当前月份
                let year = end.getFullYear();    //当前年份
                // 计算要减去的月份后的目标月份            
                month = month - subtract;
                year = month < 0 ? end.getFullYear() - 1 : end.getFullYear();
                if (month < 0) month += 12;
                // 设置目标日期为当前日期
                let start = new Date(end);
                start.setFullYear(year, month); // 设置目标年份和月份
                return [start, end];
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                $api.delete('Money/Delete', { 'id': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        th.handleCurrentChange();
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
                $api.get('Money/PagerForAccount', th.form).then(function (d) {
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
            },
            //导出学员的资金流水
            btnOutput: function () {
                //创建生成Excel
                this.loading_out = true;
                var th = this;
                $api.get('Money/ExcelAccountOutput', this.query).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            message: '成功生成Excel文件！',
                            type: 'success',
                            position: 'top-right',
                            duration: 2000
                        });
                        th.getFiles();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                    th.loading_out = false;
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                    th.loading_out = false;
                });
            },
            //获取文件列表
            getFiles: function () {
                var th = this;
                $api.get('Money/ExcelFiles', { 'path': this.query.path, 'orgid': -1 }).then(function (req) {
                    if (req.data.success) {
                        th.files = req.data.result;
                        th.loading_out = false;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    th.loading_out = false;
                    console.error(err);
                });
            },
            //删除文件
            deleteFile: function (file) {
                var th = this;
                th.loading_out = true;
                $api.get('Money/ExcelDelete', { 'path': th.query.path, 'orgid': -1, 'filename': file }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.getFiles();
                        th.$notify({
                            message: '文件删除成功！',
                            type: 'success',
                            position: 'bottom-right',
                            duration: 2000
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            }
        }
    });

});