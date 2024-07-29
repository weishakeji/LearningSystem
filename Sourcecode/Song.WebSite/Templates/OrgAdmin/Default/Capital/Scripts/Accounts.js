$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: {
                'orgid': '', 'sortid': -1, 'use': null, 'acc': '', 'name': '', 'phone': '', 'idcard': '','gender': '-1', 'isuse': '',
                size: 20, index: 1
            },
            accounts: [], //账号列表
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [],  //数据表中选中的行

            loading: false,
            loadingid: 0       //当前操作中的对象id
        },
        created: function () {
            var th = this;
            $api.get('Organization/Current').then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.form.orgid = result.Org_ID;
                    th.handleCurrentChange(1);
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => { });

        },
        mounted: function () {

        },
        computed: {
        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                th.loading = true;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 105;
                th.form.size = Math.floor(area / 41);
                $api.get("Account/Pager", th.form).then(function (d) {
                    if (d.data.success) {
                        th.accounts = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //显示手机号
            showmobi: function (row) {
                var phone = row.Ac_MobiTel1;
                return phone != '' ? phone : row.Ac_MobiTel2;
            },
            //复制到粘贴板
            copytext: function (val, textbox) {
                this.copy(val, textbox).then(function (th) {
                    th.$message({
                        message: '复制 “' + val + '” 到粘贴板',
                        type: 'success'
                    });
                });
            },
            //资金操作
            moneyHandle: function (row) {
                var title = '“' + row.Ac_Name + '(' + row.Ac_AccName + ')”的资金操作'
                this.$refs.btngroup.pagebox('AccountMoney?id=' + row.Ac_ID, title, null, '600', '400');
            },
            //打开子页面
            openSubpage: function (page, account, title, width, height, icon) {
                var title = '“' + account.Ac_Name + '(' + account.Ac_AccName + ')”的' + title;
                var param = { ico: icon };
                this.$refs.btngroup.pagebox(page + '?id=' + account.Ac_ID, title, null, width, height, param);
            }
        }
    });

});