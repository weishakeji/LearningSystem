$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: {
                acid: '',      //学员id 
                type: '-1',     //类型，支出或充值
                from: '5',     //来源
                start: '',       //时间区间的开始时间
                end: '',         //结束时间
                search: '',     //按内容检索
                moneymin: '-1',      //金额的选择范围，最小值
                moneymax: '-1',     //同上,最大值
                serial: '',          //流水号               
                state: '-1',       //状态，成功为1，失败为2,-1为所有
                size: 10, index: 1
            },
            date_picker: [],
            account: {},

            datas: [],      //数据集，此处是学员列表
            total: 1, //总记录数
            totalpages: 1, //总页数

            sum: 0,              //收入总额

            loading_init: true,
            loading_sum: true,      //获取总额
            loading: false
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.get('Account/Current').then(function (req) {
                if (req.data.success) {
                    th.account = req.data.result;
                    th.form.acid = th.account.Ac_ID;
                    th.getsum();
                    th.handleCurrentChange();
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
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
            //获取总额
            getsum: function () {
                var th = this;
                th.loading_sum = true;
                $api.get('Money/Summary', { 'orgid': -1, 'acid': th.account.Ac_ID, 'type': '', 'from': '5', 'start': '', 'end': '' })
                    .then(function (req) {
                        if (req.data.success) {
                            th.sum = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(err => console.error(err))
                    .finally(() => th.loading_sum = false);
            },
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
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //行的点击事件
            rowclick: function (row) {
                this.show_detail = true;
                this.detail = row;
            }
        }
    });

});
