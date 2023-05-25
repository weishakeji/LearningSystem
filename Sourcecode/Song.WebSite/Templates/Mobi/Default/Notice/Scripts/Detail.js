$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项          

            sear_str: '',

            id: $api.dot(),     //通知公告的id
            data: {},
            isformat: $api.storage('notice_isformat') == 'true',         //是否格式化
            loading: true

        },
        mounted: function () {
            var th = this;
            th.loading = true;    
            //通知公告
            $api.cache('Notice/ForID', { 'id': this.id }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    th.data = req.data.result;
                    $api.cache('Notice/ViewNum:60', { 'id': th.id, 'num': 1 }).then(function (req) {
                        if (req.data.success) {
                            th.data.No_ViewNum = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err));
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                th.loading = false;
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
            //是否为空，即通知公告不存在
            isempty: function () {
                return !(JSON.stringify(this.data) != '{}' && this.data != null);
            },
            //是否格式化
            'isformat': {
                handler: function (nv, ov) {
                    if (nv != null)
                        $api.storage('notice_isformat', nv);
                }, immediate: false,
            }
        },
        methods: {
            onSearch: function () {
                var url = '/mobi/Notice/index?search=' + encodeURIComponent(this.sear_str);
                window.location.href = url;
            }
        }
    });

});
