$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: { 'acid': '', 'start': '', 'end': '', 'type': '-1', 'from': '-1', 'search': '', 'state': '-1', 'size': 10, 'index': 1 },
            date_picker: [],
            account: {},

            datas: [],      //数据集，此处是学员列表
            total: 1, //总记录数
            totalpages: 1, //总页数

            detail: {},        //当前展示详情的对象
            show_detail: false,     //详情展示

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
            },
            //当前需要查看的详情资金流水项
            'detail': function (nv, ov) {
                var th = this;
                //获取当前流水项的支付接口
                if (nv.Ma_From == 3) {
                    $api.cache('Pay/ForID', { 'id': nv.Pai_ID }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            nv['payinterface'] = result;
                            th.detail['Pai_Name'] = result.Pai_Name;

                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        Vue.prototype.$alert(err);
                        console.error(err);
                    });
                }
                console.log(nv);
            }
        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 120;
                th.form.size = Math.floor(area / 49);
                th.loading = true;
                $api.get("Money/PagerForAccount", th.form).then(function (d) {
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
            //行的点击事件
            rowclick: function (row) {
                this.show_detail = true;
                this.detail = row;
            }
        }
    });

});
