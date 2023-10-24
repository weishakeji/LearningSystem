
//月度收入图表
$dom.load.css([$dom.path() + 'Capital/Components/Styles/monthlychart.css'])
Vue.component('monthlychart', {
    props: ["show", "orgid"],
    data: function () {
        return {
            interval: 'm',      //间隔
            data: {},
            color: ['#acf1c6', '#fde3ad', '#ffd7e8', '#c2e4ff'],
            loading: true
        }
    },
    watch: {
        'orgid': {
            handler: function (nv, ov) {
                if (nv) this.getdata();
            }, immediate: true
        },
        'data': function (nv, ov) {
            if ($api.isnull(nv)) return;
            this.$nextTick(function () {
                var box = $dom('#monthly_chart');
                var myChart = echarts.init(box[0]);
                var option = {
                    xAxis: {
                        type: 'category', data: []
                    },
                    yAxis: {
                        type: 'value'
                    },
                    series: [
                        {
                            type: 'bar', data: [], itemStyle: {
                                normal: {
                                    color: '#7cb5ec',
                                    shadowBlur: 5,
                                    shadowColor: 'rgba(0, 0, 0, 0.3)'
                                }
                            }
                        }
                    ],
                    tooltip: {
                        trigger: 'axis',
                        axisPointer: {
                            type: 'shadow'
                        }
                    }
                };
                for (let i = 0; i < nv.length; i++) {
                    option.xAxis.data.push(nv[i].key);
                    //数据项的值与样式
                    var obj = { 'value': nv[i].value };
                    obj['itemStyle'] = {
                        color: new echarts.graphic.LinearGradient(
                            0, 0, 0, 1,
                            [
                                { offset: 0, color: this.color[i % this.color.length] },
                                { offset: 1, color: this.color[this.color.length - i % this.color.length - 1] }
                            ]
                        )
                    }
                    option.series[0].data.push(obj);
                    //console.log(nv[i]);
                }
                myChart.setOption(option);
                window.addEventListener('resize', myChart.resize);
            });

        }
    },
    computed: {
    },
    mounted: function () {
    },
    methods: {
        getdata: function () {
            var th = this;
            var orgid = this.org ? th.org.Org_ID : 0;
            th.loading = true;
            const start = th.todate(new Date());
            start.setDate(1);
            start.setMonth(0);
            const end = th.todate(new Date(start.getFullYear(), 12, 0));
            end.setDate(end.getDate() + 1);
            $api.get('Money/Statistics', { 'unit': 'm', 'orgid': th.orgid, 'acid': '', 'type': 1, 'from': 4, 'start': start, 'end': end }).then(function (req) {
                if (req.data.success) {
                    th.datahandler(req.data.result);
                    console.error(req.data.result);
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => { });
        },
        //只保留时间部分
        todate: function (time) {
            if (time == null) return null;
            // 获取日期的年、月、日
            let year = time.getFullYear();
            let month = time.getMonth() + 1;
            let day = time.getDate();
            return new Date(year + '/' + month + '/' + day);
        },
        //加工数据
        datahandler: function (data) {
            //构造月份数据
            let today = new Date();
            let year = today.getFullYear();
            let arr = [];
            for (let i = 1; i <= 12; i++) {
                arr.push({ 'key': year + '-' + (i < 10 ? '0' + i : i), 'value': 0 });
            }
            for (let key in data) {
                let d = key.substring(0, key.indexOf(' '));
                d = $api.trim(d.replace(/\//ig, "-"));
                d = d.substring(0, d.lastIndexOf('-'));
                for (let i = 0; i < arr.length; i++) {
                    if (arr[i].key == d) {
                        arr[i].value = data[key];
                    }
                }
            }
            this.data = arr;
        }
    },
    template: `<div>
        <div class="echart_title" v-if="true">月收入图表</div>
        <section id="monthly_chart"></section>
    </div>`
});