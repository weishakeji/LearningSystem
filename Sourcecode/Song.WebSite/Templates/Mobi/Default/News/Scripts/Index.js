$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {         
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
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
            ).then(axios.spread(function (account, platinfo, organ) {
                //获取结果          
                vapp.account = account.data.result;
                vapp.platinfo = platinfo.data.result;
                vapp.organ = organ.data.result;
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;
                $api.cache('News/ColumnsShow', { 'orgid': vapp.organ.Org_ID, 'pid': '','count': 0}).then(function (req) {
                    vapp.loading = false;
                    if (req.data.success) {
                        vapp.columns = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () { },
        computed: {},
        watch: {},
        methods: {}
    });
}, ['Components/Articles.js',
    'Components/SearchInput.js']);
