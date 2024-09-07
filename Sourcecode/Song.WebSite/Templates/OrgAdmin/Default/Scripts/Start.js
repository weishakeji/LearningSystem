$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项        
            stat: {},       //机构的各种统计数据
            //机构课程购买数据
            buydata: {
                courseCount: 0,       //被购买的课程总数，课程被购买多次也算一次
                studentCount: 0,     //购买过课程的学生总数，购买多次也算一次
                courseSum: 0,           //课程购买的人次，一个学员买多个课程，算多次              
            },
            loading_init: true,
            loading_stat: true,
            loading_buy: false
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(([platinfo, org]) => {
                //获取结果           
                th.platinfo = platinfo.data.result;
                th.org = org.data.result;
                //机构配置信息
                th.config = $api.organ(th.org).config;
                th.getStatistics(th.org);
                th.getBuydata(th.org);
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        created: function () { },
        computed: {},
        watch: {},
        methods: {
            //获取统计数据
            getStatistics: function (org) {                
                var th = this;
                th.loading_stat = true;
                $api.cache('Organization/Statistics', { 'orgid': org.Org_ID }).then(function (req) {
                    if (req.data.success) {
                        th.stat = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading_stat = false);
            },
            //获取课程购买的次数与人次
            getBuydata: function (org) {
                var th = this;
                th.loading_buy = true;
                var orgid = org.Org_ID;
                $api.bat(
                    $api.get('Organization/CourseSumBuy', { 'orgid': orgid, 'isfree': '', 'start': '', 'end': '' }),
                    $api.get('Organization/CourseCountBuy', { 'orgid': orgid, 'isfree': '', 'start': '', 'end': '' }),
                    $api.get('Organization/StudentCountBuy', { 'orgid': orgid, 'isfree': '', 'start': '', 'end': '' })
                ).then(([sum, count, stcount]) => {
                    th.buydata.courseSum = sum.data.result;
                    th.buydata.courseCount = count.data.result;
                    th.buydata.studentCount = stcount.data.result;
                }).catch(err => console.error(err))
                    .finally(() => th.loading_buy = false);
            }
        },
        filters: {
            //数字转三位带逗号
            'commas': function (number) {
                return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            }
        }
    });


}, ['Viewport/Components/piece.js',
    'Viewport/Components/dataitem.js',
    'Viewport/Components/studentage.js',
    'Viewport/Components/studentlogin.js',
    'Viewport/Components/courses_hot.js',

    '/Utilities/echarts/echarts.min.js',
    'Viewport/Components/map_display.js',
]);
