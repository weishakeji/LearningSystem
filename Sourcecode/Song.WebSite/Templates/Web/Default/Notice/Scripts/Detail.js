$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项          
            loading: true,
            sear_str: '',

            notices: [],         //通知公告列表
            id: $api.dot(),
            data: {}

        },
        mounted: function () {
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (account, platinfo, organ) {
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
                vapp.organ = organ.data.result;
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;
                vapp.getnotices();
            })).catch(function (err) {
                console.error(err);
            });
            //通知公告
            $api.cache('Notice/ShowForID', { 'id': this.id }).then(function (req) {
                if (req.data.success) {
                    vapp.data = req.data.result;
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                //alert(err);
                console.error(err);
            });
        },
        created: function () {
        },
        computed: {
            //是否为空，即通知公告不存在
            isempty: function () {
                return !(JSON.stringify(this.data) != '{}' && this.data != null);
            }
        },
        watch: {
        },
        methods: {
            onSearch: function () {
                var url = '/mobi/Notice/list?search=' + encodeURIComponent(this.sear_str);
                window.location.href = url;
            },
            //获取通知公告
            getnotices: function () {
                var orgid = this.organ.Org_ID;
                var th = this;
                $api.get('Notice/ShowItems', { 'orgid': orgid, 'type': -1, 'count': 10 }).then(function (req) {
                    if (req.data.success) {
                        th.notices = req.data.result;
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    console.error(err);
                });
            }
        }
    });

});
