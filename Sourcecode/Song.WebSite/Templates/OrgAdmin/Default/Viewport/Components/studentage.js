
//学员年龄分布的图表
$dom.load.css([$dom.path() + 'Viewport/Components/Styles/studentage.css'])
Vue.component('studentage', {
    props: ["show", "org"],
    data: function () {
        return {
            interval: 5,      //年龄间隔
            data: {},
            color: ['#ff0000', '#00ff00', '#0000ff'],
            loading: true
        }
    },
    watch: {
        'show': {
            handler: function (nv, ov) {
                if (nv) this.getdata();
            }, immediate: true
        },
        'data': function (nv, ov) {
            if ($api.isnull(nv)) return;
            this.$nextTick(function () {
                var box = $dom('#student_chart');
                var myChart = echarts.init(box[0]);
                var option = {
                    title: {
                        text: '学员年龄段分布',
                        textStyle: {
                            fontWeight: 'normal',
                            fontSize: '18px', align: 'center'
                        }
                    },

                    xAxis: {
                        type: 'category', data: []
                    },
                    yAxis: {
                        type: 'value'
                    },
                    series: [
                        { type: 'bar', data: [] }
                    ],
                    tooltip: {
                        trigger: 'axis',
                        axisPointer: {
                            type: 'shadow'
                        }
                    }
                };
                for (let i = 0; i < nv.length; i++) {
                    option.xAxis.data.push(nv[i].group);
                    //数据项的值与样式
                    var obj = { 'value': nv[i].count };
                    obj['itemStyle'] = {
                        color: new echarts.graphic.LinearGradient(
                            0, 0, 0, 1,
                            [
                                { offset: 0, color: '#92cb65' },
                                { offset: 1, color: '#28950e52' }
                            ]
                        )
                    }
                    option.series[0].data.push(obj);
                    //console.log(nv[i]);
                }
                myChart.setOption(option);
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
            $api.get('Account/AgeGroup', { 'orgid': orgid, 'interval': th.interval }).then(function (req) {
                if (req.data.success) {
                    th.data = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        }
    },
    template: `<section class="student_chart" id="student_chart">

    </section>`
});