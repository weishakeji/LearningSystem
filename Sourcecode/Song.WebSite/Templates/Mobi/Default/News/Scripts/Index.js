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
        },
        created: function () { },
        computed: {},
        watch: {
            'org': {
                handler: function (nv, ov) {
                    if ($api.isnull(nv)) return;
                    var th=this;
                    th.loading = true;
                    $api.cache('News/ColumnsShow', { 'orgid': nv.Org_ID, 'pid': '','count': 0}).then(function (req) {
                        th.loading = false;
                        if (req.data.success) {
                            th.columns = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });
                   
                }, immediate: true
            },
        },
        methods: {}
    });
}, ['Components/Articles.js',
    'Components/SearchInput.js']);
