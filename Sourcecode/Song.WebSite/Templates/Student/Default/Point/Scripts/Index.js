$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: { 'acid': '', 'start': '', 'end': '', 'type': '-1', 'search': '', 'size': 10, 'index': 1 },
            date_picker: [],
            account: {},

            datas: [],      //数据集，此处是学员列表
            total: 1, //总记录数
            totalpages: 1, //总页数

            loading_init: true,
            loading: false
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.get('Account/Current').then(function (req) {
                th.loading_init = false;
                if (req.data.success) {
                    th.account = req.data.result;
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
            'date_picker': function (nv, ov) {
                this.form.start = nv[0];
                this.form.end = nv[1];
            }
        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 115;
                th.form.size = Math.floor(area / 47);
                th.loading = true;
                $api.get("Point/PagerForAccount", th.form).then(function (d) {
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
        }
    });

});
