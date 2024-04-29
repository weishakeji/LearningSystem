$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: {
                acid: '',      //学员id 
                type: '-1',     //类型，支出或充值
                from: '-1',     //来源
                start: '',       //时间区间的开始时间
                end: '',         //结束时间
                search: '',     //按内容检索
                moneymin: '-1',      //金额的选择范围，最小值
                moneymax: '-1',     //同上,最大值
                serial: '',          //流水号               
                state: '-1',       //状态，成功为1，失败为2,-1为所有
                size: 10, index: 0
            },
            account: {},

            datas: [],      //数据集，此处是学员列表
            total: 1, //总记录数
            totalpages: 1, //总页数

            detail: {},        //当前展示详情的对象
            show_detail: false,     //详情展示

            loading_init: true,
            loading_query: false,        //订单查询的预载
            loading: false
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.get('Account/Current').then(function (req) {
                if (req.data.success) {
                    th.account = req.data.result;
                    th.form.acid = th.account.Ac_ID;
                    th.handleCurrentChange(1);
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        created: function () {

        },
        computed: {

        },
        watch: {           
            //当前需要查看的详情资金流水项
            'detail': function (nv, ov) {
                var th = this;
                //获取当前流水项的支付接口
                if (nv.Ma_From == 3) {
                    $api.cache('Pay/ForID', { 'id': nv.Pai_ID }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            nv['payinterface'] = result;
                            th.detail['Pai_Name'] = result.Pai_Name;

                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(err => console.error(err));
                }
                console.log(nv);
            }
        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 120;
                th.form.size = Math.floor(area / 49);
                th.loading = true;
                $api.get("Money/PagerForAccount", th.form).then(function (d) {
                    if (d.data.success) {
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                        //刷新当前查看的对象，右侧详情
                        if (th.detail && th.detail.Ma_Serial) {
                            var detail = th.datas.find(d => d.Ma_Serial == th.detail.Ma_Serial);
                            if (detail != null) th.detail = detail;
                        }
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //查询订单
            queryOrder: function (detail) {
                var th = this;
                th.loading_query = true;
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
                                        th.handleCurrentChange();
                                    }
                                    console.log(result);
                                } else {
                                    console.error(req.data.exception);
                                    throw req.config.way + ' ' + req.data.message;
                                }
                            }).catch(err => console.error(err))
                                .finally(() => th.loading_query = false);
                        }
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err));
            }
        }
    });

});
