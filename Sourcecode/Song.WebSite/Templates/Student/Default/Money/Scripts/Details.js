$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: { 'acid': '', 'start': '', 'end': '', 'type': '-1', 'from': '-1', 'search': '', 'state': '-1', 'size': 10, 'index': 1 },
            date_picker: [],
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
                th.loading_init = false;
                if (req.data.success) {
                    th.account = req.data.result;
                    th.form.acid = th.account.Ac_ID;
                    th.handleCurrentChange();
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                Vue.prototype.$alert(err);
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {

        },
        watch: {
            'date_picker': function (nv, ov) {
                this.form.start = nv[0];
                this.form.end = nv[1];
            },
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
                    }).catch(function (err) {
                        Vue.prototype.$alert(err);
                        console.error(err);
                    });
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
                    th.loading = false;
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
                }).catch(function (err) {
                    console.error(err);
                });
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
