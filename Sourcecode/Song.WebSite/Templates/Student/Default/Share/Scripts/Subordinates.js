$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: {
                'acid': 0, 'acc': '', 'name': '', 'phone': '',
                'index': 1, 'size': 20
            },
            account: {},
            friendAll: 0, friends: 0,

            datas: [],      //数据集，此处是学员列表
            total: 1, //总记录数
            totalpages: 1, //总页数

            loading_init: true,
            lading_count: true,     //获取数量的预载
            loading: false
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.get('Account/Current').then(function (req) {
                th.loading_init = false;
                if (req.data.success) {
                    th.account = req.data.result;
                    th.getCount();
                    th.form.acid = th.account.Ac_ID;
                    th.handleCurrentChange();
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                Vue.prototype.$alert(err);
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {

        },
        watch: {
        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 46);
                th.loading = true;
                $api.get("Share/FriendPager", th.form).then(function (d) {
                    th.loading = false;
                    if (d.data.success) {
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                        //console.log(th.accounts);
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            },
            //获取下级学员的数量
            getCount: function () {
                var th = this;
                var acid = th.account.Ac_ID;
                th.lading_count = true;
                $api.bat(
                    $api.get('Share/FriendAll', { 'acid': acid }),
                    $api.get("Share/Friends", { 'acid': acid })
                ).then(axios.spread(function (friendAll, friends) {
                    th.lading_count = false;
                    //获取结果
                    th.friendAll = friendAll.data.result;
                    th.friends = friends.data.result;
                })).catch(function (err) {
                    th.lading_count = false;
                    console.error(err);
                });
            }
        }
    });

});
