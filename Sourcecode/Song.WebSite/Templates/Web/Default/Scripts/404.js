$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项
            columns: [],        //新闻栏目
            loading: false
        },
        mounted: function () {
            this.loading = true;
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (account, platinfo, org) {
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
                vapp.account = account.data.result;
                vapp.platinfo = platinfo.data.result;
                vapp.org = org.data.result;
                //机构配置信息
                vapp.config = $api.organ(vapp.org).config;
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () { },
        computed: {},
        watch: {},
        methods: {
            //获取当前地址
            geturl: function () {
                var url = String(window.document.location.pathname);              
                if (url.indexOf("?") > -1)
                    url = url.substring(0, url.lastIndexOf("?"));
                return url;
            }
        }
    });
});
