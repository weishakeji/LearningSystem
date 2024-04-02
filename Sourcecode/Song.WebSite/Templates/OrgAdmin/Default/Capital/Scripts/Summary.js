$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            orgid: 0,
            total: { 'pay': 0, 'recharge': 0 },       //总金额，包括支付与充值
            year: { 'pay': 0, 'recharge': 0 },
            quarter: { 'pay': 0, 'recharge': 0 },
            month: { 'pay': 0, 'recharge': 0 },

            listday: {},     //当月收入记录

            currdate: new Date()
        },
        watch: {
            //当前日期变更时
            'currdate': function (nv, ov) {
                console.log(ov);
                let nm = nv.getMonth();
                let om = ov.getMonth();
                console.error(nm);
                console.error(om);
                if (nv.getMonth() != ov.getMonth())
                    this.getDayStatistics();
            }
        },
        created: function () {


        },
        mounted: function () {
            var th = this;
            $api.get('Organization/Current').then(function (req) {
                if (req.data.success) {
                    let result = req.data.result;
                    th.orgid = result.Org_ID;
                    th.getTotaldata();
                    th.getYeardata();
                    th.getQuarterdata();
                    th.getMonthdata();
                    th.getDayStatistics();
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => {
                alert('获取当前机构信息错误');
                console.error(err);
            }).finally(() => { });

        },
        computed: {

        },
        methods: {
            //获取总金额
            getTotaldata: function () {
                var th = this;
                $api.bat(
                    $api.get('Money/Summary', { 'orgid': th.orgid, 'acid': '', 'type': 1, 'from': 4, 'start': '', 'end': '' }),
                    $api.get('Money/Summary', { 'orgid': th.orgid, 'acid': '', 'type': 2, 'from': 3, 'start': '', 'end': '' })
                ).then(axios.spread(function (pay, recharge) {
                    th.total['pay'] = pay.data.result;
                    th.total['recharge'] = recharge.data.result;
                })).catch(err => alert(err))
                    .finally(() => { });
            },
            //年度收入
            getYeardata: function () {
                var th = this;
                const start = th.todate(new Date());
                start.setDate(1);
                start.setMonth(0);
                const end = th.todate(new Date(start.getFullYear(), 12, 0));
                end.setDate(end.getDate() + 1);
                $api.bat(
                    $api.get('Money/Summary', { 'orgid': th.orgid, 'acid': '', 'type': 1, 'from': 4, 'start': start, 'end': end }),
                    $api.get('Money/Summary', { 'orgid': th.orgid, 'acid': '', 'type': 2, 'from': 3, 'start': start, 'end': end })
                ).then(axios.spread(function (pay, recharge) {
                    th.year['pay'] = pay.data.result;
                    th.year['recharge'] = recharge.data.result;
                })).catch(err => alert(err))
                    .finally(() => { });
            },
            //本季度
            getQuarterdata: function () {
                var th = this;
                const start = th.todate(new Date());
                let yy = start.getFullYear();
                let mm = start.getMonth() + 1;
                mm = Math.floor(mm % 3 == 0 ? mm / 3 : mm / 3 + 1);
                start.setDate(1);
                start.setMonth((mm - 1) * 3);
                const end = th.todate(new Date(yy, start.getMonth() + 3, 0));
                end.setDate(end.getDate() + 1);
                $api.bat(
                    $api.get('Money/Summary', { 'orgid': th.orgid, 'acid': '', 'type': 1, 'from': 4, 'start': start, 'end': end }),
                    $api.get('Money/Summary', { 'orgid': th.orgid, 'acid': '', 'type': 2, 'from': 3, 'start': start, 'end': end })
                ).then(axios.spread(function (pay, recharge) {
                    th.quarter['pay'] = pay.data.result;
                    th.quarter['recharge'] = recharge.data.result;
                })).catch(err => alert(err))
                    .finally(() => { });
            },
            //本月收入
            getMonthdata: function () {
                var th = this;
                const start = th.todate(new Date());
                start.setDate(1);
                let yy = start.getFullYear();
                let mm = start.getMonth() + 1;
                if (mm > 12) {
                    mm = 1;
                    yy = yy + 1;
                }
                let end = th.todate(new Date(yy, mm, 0));
                end.setDate(end.getDate() + 1);
                $api.bat(
                    $api.get('Money/Summary', { 'orgid': th.orgid, 'acid': '', 'type': 1, 'from': 4, 'start': start, 'end': end }),
                    $api.get('Money/Summary', { 'orgid': th.orgid, 'acid': '', 'type': 2, 'from': 3, 'start': start, 'end': end })
                ).then(axios.spread(function (pay, recharge) {
                    th.month['pay'] = pay.data.result;
                    th.month['recharge'] = recharge.data.result;
                })).catch(err => alert(err))
                    .finally(() => { });
            },
            //当前月每天的收入
            getDayStatistics: function () {
                var th = this;
                const start = th.todate(th.currdate);
                start.setDate(1);
                let yy = start.getFullYear();
                let mm = start.getMonth() + 1;
                if (mm > 12) { mm = 1; yy = yy + 1; }
                let end = th.todate(new Date(yy, mm, 0));
                end.setDate(end.getDate() + 1);
                $api.get('Money/Statistics', { 'unit': 'd', 'orgid': th.orgid, 'acid': '', 'type': 1, 'from': 4, 'start': start, 'end': end }).then(function (req) {
                    if (req.data.success) {
                        th.listday = req.data.result;
                        //console.error(req.data.result);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => { });
            },
            //当前的金额
            daymoney: function (data) {
                //判断当天有没有金额
                if ($api.isnull(this.listday)) return '';
                let day = data.day;
                for (let key in this.listday) {
                    let d = key.substring(0, key.indexOf(' '));
                    d = $api.trim(d.replace(/\//ig, "-"));
                    if (d == data.day)
                        return this.listday[key];
                }
            },
            //只保留时间部分
            todate: function (time) {
                if (time == null) return null;
                // 获取日期的年、月、日
                let year = time.getFullYear();
                let month = time.getMonth() + 1;
                let day = time.getDate();
                return new Date(year + '/' + month + '/' + day);
            }
        },
        filters: {
            //数字转三位带逗号
            'commas': function (number) {
                return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            }
        }
    });

}, ['/Utilities/echarts/echarts.min.js',
    'Components/monthlychart.js',]);