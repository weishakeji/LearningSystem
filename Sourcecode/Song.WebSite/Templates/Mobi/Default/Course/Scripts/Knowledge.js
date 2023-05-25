$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            'id': $api.querystring("id", 0),    //知识id
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项        
            datas: {},      //知识对象
            loading_init: true
        },
        mounted: function () {
            var th=this;
            //获取知识项
            $api.get('Knowledge/ForID', { 'id': this.id }).then(function (req) {
                if (req.data.success) {
                    th.datas= req.data.result;                   
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: (t) => { return !$api.isnull(t.account); }
        },
        watch: {
            'org': {
                handler: function (nv, ov) {
                    if ($api.isnull(nv)) return;
                    this.loading_init = false;
                }, immediate: true
            },
        },
        methods: {
        }
    });

}, ['Components/knlheader.js']);
