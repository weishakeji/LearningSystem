
//学员登录数据的图表
$dom.load.css([$dom.path() + 'Viewport/Components/Styles/studentlogin.css'])
Vue.component('studentlogin', {
    //month:显示的月数
    props: ["show", "org", "month", "title"],
    data: function () {
        return {
            logindata: {},      //登录数据
            regdata: {},         //注册数据

            //color: ['#ff0000', '#00ff00', '#0000ff'],
            loading: true
        }
    },
    watch: {
        'show': {
            handler: function (nv, ov) {
                if (nv) this.getdata();
            }, immediate: true
        },
        'regdata': function (nv, ov) {
            if ($api.isnull(nv)) return;
            var th = this;
            th.$nextTick(function () {
                var box = $dom('#studentlogin');
                var myChart = echarts.init(box[0]);
                var option = {
                    xAxis: {
                        type: 'category',
                        boundaryGap: false,
                        data: []
                    },
                    yAxis: {
                        type: 'value'
                    },
                    series: [
                        {
                            name: 'login', type: 'line', stack: 'Total', smooth: true,
                            data: []
                        },
                        {
                            name: 'register', type: 'line', stack: 'Total', smooth: true,
                            data: []
                        },
                    ],
                    tooltip: {
                        trigger: 'axis',
                        axisPointer: {
                            type: 'shadow'
                        }
                    }
                };
                option = th.build_option(option);
                myChart.setOption(option);
            });

        }
    },
    computed: {
        //要获取数据的时间长度，单位为月
        'datalen': function () {
            if ($api.isnull(this.month)) return 12;
            let m = Number(this.month);
            if (isNaN(m) || m <= 0) return 12;
            return m;
        }
    },
    mounted: function () {
    },
    methods: {
        //获取数据
        getdata: function () {
            var th = this;
            var orgid = this.org ? th.org.Org_ID : 0;
            let start = new Date();
            start.setMonth(start.getMonth() - th.datalen);
            th.loading = true;
            $api.bat(
                $api.get('Account/LoginTimeGroup', { 'orgid': orgid, 'interval': 'm', 'start': start, 'end': new Date() }),
                $api.get('Account/RegTimeGroup', { 'orgid': orgid, 'interval': 'm', 'start': start, 'end': new Date() }),
            ).then(axios.spread(function (login, reg) {
                th.logindata = login.data.result;
                th.regdata = reg.data.result;
            })).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        //生成图表数据
        build_option: function (option) {
            //x坐标轴  
            for (let i = this.datalen - 1; i >= 0; i--) {
                let date = new Date();
                date.setMonth(date.getMonth() - i);
                let s = date.format('yyyy-MM');
                option.xAxis.data.push(s);
            }
            //登录数据
            for (let i = 0; i < this.logindata.length; i++) {
                let group = this.logindata[i].group;
                group = group.substring(0, group.lastIndexOf('-'));
                this.logindata[i].group = group;
            }
            for (let i = 0; i < this.datalen; i++) {
                let date = option.xAxis.data[i];
                let result = this.logindata.find(function (element) {
                    return element.group == date;
                });
                option.series[0].data.push(result == null ? 0 : result.count);
            }
            //注册数据的处理
            for (let i = 0; i < this.regdata.length; i++) {
                let group = this.regdata[i].group;
                group = group.substring(0, group.lastIndexOf('-'));
                this.regdata[i].group = group;
            }
            for (let i = 0; i < this.datalen; i++) {
                let date = option.xAxis.data[i];
                let result = this.regdata.find(function (element) {
                    return element.group == date;
                });
                option.series[1].data.push(result == null ? 0 : result.count);
            }
            return option;
        }
    },
    template: `<div>
        <div class="echart_title" v-if="show">{{title}}</div>
        <section id="studentlogin"></section>
    </div>`
});