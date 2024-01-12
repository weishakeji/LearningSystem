$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: { 'orgid': 0, 'acid': '', 'start': '', 'end': '', 'size': 10, 'index': 1 },
            date_picker: [],
            account: {},

            datas: [],      //数据集，此处是登录记录的列表
            total: 1, //总记录数
            totalpages: 1, //总页数

            loading_init: true,
            loading: false
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.login.current('account', function (acc) {
                th.account = acc;
                th.form.acid = acc.Ac_ID;
                th.handleCurrentChange();
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
                //console.error(this.form);
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 115;
                th.form.size = Math.floor(area / 47);
                th.loading = true;
                $api.get("Account/LoginLogs", th.form).then(function (d) {
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
            //显浏览时间
            timeclac: function (second) {
                if (second < 60) return second + ' 秒';
                let mm = Math.floor(second / 60);
                if (mm < 60) return mm + ' 分钟';
                let hh = Math.floor(mm / 60);
                if (hh < 24) return hh + ' 小时' + (mm % 60 > 0 ? mm % 60 + '分钟' : '');
            }
        },
    });

});
