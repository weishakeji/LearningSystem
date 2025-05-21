$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项       
            loading: false,
            loading_init: true,  

            datas: [],           //数据列表
            finished: false,
            query: {
                'acid': 0, 'acc': '', 'name': '', 'phone': '',
                'index': 0, 'size': 20
            },
            total: 0,
            //下级所有学员，直接下级学员数量
            friendAll: 0, friends: 0,
        },
        mounted: function () {
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: (t) => { return !$api.isnull(t.account); },           
        },
        watch: {
            'account': {
                handler: function (nv, ov) {
                    if ($api.isnull(nv)) return;
                    this.loading_init = false;
                    this.getCount();
                }, immediate: true
            },
        },
        methods: {
            onload: function () {
                var th = this;
                if (!th.islogin) {
                    window.setTimeout(function () {
                        vapp.onload();
                    }, 100);
                    return;
                }
                th.query.acid = th.account.Ac_ID;
                th.query.index++;
                var query = $api.clone(this.query);
                console.log(query);
                $api.get('Share/FriendPager', query).then(function (req) {
                    if (req.data.success) {
                        th.total = req.data.total;
                        var result = req.data.result;
                        for (var i = 0; i < result.length; i++) {
                            th.datas.push(result[i]);
                        }
                        var totalpages = req.data.totalpages;
                        // 数据全部加载完成
                        if (th.datas.length >= th.total || result.length == 0) {
                            th.finished = true;
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }

                }).catch(function (err) {
                    th.error = err;
                    th.finished = true;
                    console.error(err);
                }).finally(() => th.loading = false);
            }, 
             //获取下级学员的数量
             getCount: function () {
                var th = this;
                var acid = th.account.Ac_ID;
                th.lading_count = true;
                $api.bat(
                    $api.get('Share/FriendAll', { 'acid': acid }),
                    $api.get("Share/Friends", { 'acid': acid })
                ).then(([friendAll, friends]) => {
                    //获取结果
                    th.friendAll = friendAll.data.result;
                    th.friends = friends.data.result;
                }).catch(err => console.error(err))
                    .finally(() => th.lading_count = false);
            },
            //学员的手机号
            accmobi:function(acc){
                return $api.accmobi(acc);
            }
        }
    });

}, ['../Components/page_header.js']);
