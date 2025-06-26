$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring('couid'),      //课程ID          
            stcid: $api.querystring('stcid'),    //学习记录的ID，即Student_Course的主键ID
            account: {},     //当前登录账号       
            course: {},      //当前课程

            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项     

            purchase: {},        //课程购买的记录
            purchase_query: { 'couid': -1, 'stid': -1 },

            showhelp: false,     //显示帮助说明的面板

            loading_init: true,
            loading_fresh: 0,        //刷新的预载
            loading: false
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Account/current'),
                $api.get("Course/ForID", { "id": th.couid }),
                $api.cache('Platform/PlatInfo'),
                $api.get('Organization/Current')
            ).then(([account, cou, platinfo, organ]) => {
                //获取结果
                th.account = account.data.result;
                th.course = cou.data.result;
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.getpurchase();

            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: t => !$api.isnull(t.account),
            //课程是否存在
            existcourse: t => !$api.isnull(t.course),
            //是否有购买记录
            isnull: t => $api.isnull(t.purchase)
        },
        watch: {

        },
        methods: {
            //获取购买课程的记录
            getpurchase: function () {
                var th = this;
                th.loading = true;
                $api.get('Course/Purchaselog:5', { 'stid': th.account.Ac_ID, 'couid': th.couid }).then(function (req) {
                    if (req.data.success) {
                        th.purchase = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //获取机构的配置参数
            orgconfig: function (para, def) {
                var val = Number(this.config[para]);
                if (isNaN(val)) return def ? def : '';
                return val;
            },
            //取整
            //val:要取整的值
            //digit:小数位数
            round: function (val, digit) {
                return Math.round(val * Math.pow(10, digit)) / Math.pow(10, digit);
            }
        },
        filters: {

        },
        components: {

        }
    });
});