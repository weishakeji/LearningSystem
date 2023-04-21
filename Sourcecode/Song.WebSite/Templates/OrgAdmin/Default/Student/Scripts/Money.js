
$ready(function () {

    window.vue = new Vue({
        el: '#app',
        data: {
            id: $api.querystring('id'),
            account: {}, //当前登录账号对象            
            loading: false,

            datas: [],           //数据列表
            query: {
                acid: '',      //学员id 
                type: '-1',     //类型，支出或充值
                from: '-1',     //来源
                start: '',       //时间区间的开始时间
                end: '',         //结束时间
                search: '',     //按内容检索
                moneymin: '-1',      //金额的选择范围，最小值
                moneymax: '-1',     //同上,最大值
                serial: '',          //流水号               
                state: '-1',       //状态，成功为1，失败为2,-1为所有
                size: 6, index: 0
            },
            total: 0,

            show_detail: false,      //显示详情
            show_item: {},           //当前显示的项
        },
        created: function () {
            var th = this;
            $api.get('Account/ForID', { 'id': th.id }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.account = result;
                    th.query.acid = th.account.Ac_ID;
                    th.handleCurrentChange(1);
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                console.error(err);
            });

        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        watch: {
        },
        methods: {
            handleCurrentChange: function (index) {
                if (index != null) this.query.index = index;
                var th = this;            
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 40;
                th.query.size = Math.floor(area / 42);
                th.loading = true;
                $api.get('Money/PagerForAccount', th.query).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.datas = req.data.result;
                        th.totalpages = Number(req.data.totalpages);
                        th.total = req.data.total;
                        console.log(th.datas);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }

                }).catch(function (err) {
                    th.loading = false;
                    th.error = err;
                    console.error(err);
                });
            },
            //表格中，行的点击事件
            rowclick: function (row, column, event) {
                this.show_item = row;
                this.show_detail = true;
                console.log(row);
            }
        },
    });

});
