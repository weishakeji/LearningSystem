$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项       
            loading: false,
            loading_init: true,
            loading_query: false,

            show_detail: false,      //显示详情
            show_item: {},           //当前显示的项

            datas: [],           //数据列表
            finished: false,
            query: { 'acid': '', 'start': '', 'end': '', 'type': -1, 'from': -1, 'search': '', 'state': -1, 'size': 6, 'index': 0 },
            total: 0
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (account, platinfo, organ) {
                th.loading_init = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                    }
                }
                //获取结果
                th.account = account.data.result;
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            },
            details: function () {
                var datas = this.datas;
                var fmt = "yyyy年 M月";
                var month = [];
                for (var i = 0; i < datas.length; i++) {
                    var str_mh = datas[i]["Ma_CrtTime"].format(fmt);
                    let isexist = false;
                    for (var j = 0; j < month.length; j++) {
                        if (month[j]['month'] == str_mh) {
                            isexist = true;
                            break;
                        }
                    }
                    if (!isexist)
                        month.push({
                            'month': str_mh,
                            'items': []
                        });
                }
                for (var i = 0; i < datas.length; i++) {
                    var str_mh = datas[i]["Ma_CrtTime"].format(fmt);
                    for (var j = 0; j < month.length; j++) {
                        if (month[j]['month'] == str_mh) {
                            month[j]['items'].push(datas[i]);
                        }
                    }
                }
                console.log(month);
                return month;
            }
        },
        watch: {
        },
        methods: {
            login: function () {
                window.location.href = "/mobi/sign/in";
            },
            myself: function () {
                window.location.href = "/mobi/account/myself";
            },
            toRecharge: function () {
                var url = "/mobi/Recharge/index";
                window.location.href = url;
            },
            onload: function () {
                var th = this;
                if (!th.islogin) {
                    window.setTimeout(function () {
                        vapp.onload();
                    }, 100);
                    return;
                }
                th.query.acid = th.account.Ac_ID;
                th.query.index++;
                var query = $api.clone(this.query);
                console.log(query);
                $api.get('Money/PagerForAccount', query).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.total = req.data.total;
                        var result = req.data.result;
                        for (var i = 0; i < result.length; i++) {
                            th.datas.push(result[i]);
                        }
                        var totalpages = req.data.totalpages;
                        // 数据全部加载完成
                        if (th.datas.length >= th.total || result.length == 0) {
                            th.finished = true;
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }

                }).catch(function (err) {
                    th.loading = false;
                    th.error = err;
                    th.finished = true;
                    console.error(err);
                });
            },
            //删除记录
            btnDelete: function (item) {
                this.$dialog.confirm({
                    title: '删除记录',
                    message: '您是否确定删除当前记录？',
                }).then(() => {
                    $api.delete('Money/DeleteForAccount', { 'id': item.Ma_ID }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            if (result == true) {
                                vapp.$toast.success('删除成功');
                                vapp.datas = [];
                                vapp.query.index = 0;
                                vapp.finished = false;
                                vapp.total = false;
                                vapp.onload();
                            }
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });
                }).catch(() => {
                    // on cancel
                });
            },
            //进入详情页
            godetail: function (item) {
                this.show_detail = true;
                this.show_item = item;
            },
            //查询订单
            queryOrder: function (detail) {
                var th = this;
                //var d = th.datas.find(item => item.Ma_Serial == detail.Ma_Serial);
                //var i = th.datas.findIndex(item => item.Ma_Serial == detail.Ma_Serial);
                //console.log(d);
                //if (d != null) th.detail = detail;
                //console.log(detail);
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
                                        $api.get('Pay/MoneyAccount', { 'serial': result['out_trade_no'] }).then(function (req) {
                                            if (req.data.success) {
                                                th.show_item = req.data.result;
                                                var index = th.datas.findIndex(item => item.Ma_Serial == th.show_item.Ma_Serial);
                                                if (index > -1)
                                                    th.$set(th.datas, index, th.show_item);
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
                                .finally(() => th.loading_query = false);
                        }
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err));
            }
        }
    });

}, ['../Components/page_header.js']);
