$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项          

            sear_str: '',

            notices: [],         //通知公告列表
            id: $api.dot(),         //通知公告的id
            preview: $api.querystring('preview'),    //是否为预览内容，参数为true时预览
            data: {},
            isformat: $api.storage('notice_isformat') == 'true',         //是否格式化

            loading: true,

        },
        mounted: function () {
            var th = this;
            th.loading = true;
            //通知公告
            var apiurl = this.preview == 'true' ? 'Notice/ForID' : 'Notice/ShowForID';
            var apiobj = this.preview == 'true' ? $api.get(apiurl, { 'id': this.id }) : $api.cache(apiurl, { 'id': this.id });
            apiobj.then(function (req) {
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

            th.getnotices();
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
            },
            //获取通知公告
            getnotices: function () {
                var orgid = this.organ.Org_ID;
                var th = this;
                $api.get('Notice/showItems', { 'orgid': orgid, 'type': -1, 'count': '10' }).then(function (req) {
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
