$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: { 'orgid': '', 'name': '', 'acname': '', 'start': '', 'end': '', 'index': 1, 'size': 10 },
            account: {},

            datas: [],      //数据集，此处是登录记录的列表
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [] //数据表中选中的行

            ,
            loading: false
        },
        mounted: function () {
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
        computed: {
            //表格高度
            tableHeight: function () {
                var height = document.body.clientHeight;
                return height - 75;
            }
        },
        watch: {
        },
        methods: {
            //选择时间区间
            selectDate: function (start, end) {
                this.form.start = start;
                this.form.end = end;
                this.handleCurrentChange(1);
            },
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                //console.error(this.form);
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 105;
                th.form.size = Math.floor(area / 41);
                th.loading = true;
                $api.get("Account/LoginLogs", th.form).then(function (d) {
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
            //删除
            deleteData: function (datas) {
                var th = this;
                $api.delete('Account/LoginLogDelete', { 'id': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        th.handleCurrentChange();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                });
            },
            //显浏览时间
            timeclac: function (second) {
                if (second < 60) return second + ' 秒';
                let mm = Math.floor(second / 60);
                if (mm < 60) return mm + ' 分钟';
                let hh = Math.floor(mm / 60);
                if (hh < 24) return hh + ' 小时' + (mm % 60 > 0 ? mm % 60 + '分钟' : '');
            },
            //地理位置
            address: function (row) {
                return row.Lso_Province + row.Lso_City + row.Lso_District;
            },
            //打开详情
            opendetail: function (row) {
                this.$refs.btngroup.modifyrow(row);
                return;
                var url = 'LoginLogDetail.' + row.Lso_ID;
                this.$refs.btngroup.pagebox(url, '登录信息详情', window.name + '[LoginLogDetail]', 800, 600, {
                    'showmask': false, 'min': true, 'ico': 'a01d'
                });
                return false;
            }
        },
    });

});
