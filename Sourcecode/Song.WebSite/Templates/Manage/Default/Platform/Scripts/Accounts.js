$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: {
                'orgid': '', 'sortid': -1, 'use': null, 'acc': '', 'name': '', 'phone': '', 'idcard': '',
                'gender': '-1',  'orderby': '','orderpattr':'',
                size: 20, index: 1
            },
            organs: [],
            accounts: [], //账号列表
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [],  //数据表中选中的行

            loading: false,
            loadingdel: false,       //删除的预载状态
            loadingid: 0       //当前操作中的对象id
        },
        created: function () {
            var th = this;
            $api.get('Organization/All', { 'use': null, 'lv': 0, 'name': '' }).then(function (req) {
                if (req.data.success) {
                    th.organs = req.data.result;
                    th.handleCurrentChange(1);
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });

        },
        mounted: function () {
            var btngroup = this.$refs['btngroup'];
            console.log(btngroup);
            btngroup.addbtn({
                text: '批量删除', tips: '通过导入账号进行大批量的删除',
                id: 'bathdelete', type: 'warning',
                url: 'AccountBathDel',
                icon: 'e800'
            });
        },
        computed: {
            //表格高度
            tableHeight: function () {
                var height = document.body.clientHeight;
                return height - 75;
            }
        },
        methods: {
            //删除
            deleteData: function (datas) {
                var th = this;
                th.loadingdel = true;
                $api.delete('Account/DeleteBatch', { 'ids': datas }).then(function (req) {
                    th.loadingdel = false;
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
                }).catch(err => console.error(err))
                    .finally(() => th.loadingdel = false);
            },
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
            //双击事件
            rowdblclick: function (row, column, event) {
                this.$refs.btngroup.modifyrow(row);
            },
            //更改使用状态
            changeUse: function (row) {
                var th = this;
                this.loadingid = row.Ac_ID;
                $api.post('Account/ModifyState', { 'acid': row.Ac_ID, 'use': row.Ac_IsUse, 'pass': row.Ac_IsPass }).then(function (req) {
                    if (req.data.success) {
                        th.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err, '错误');
                }).finally(() => th.loadingid = 0);
            },
            //导出
            output: function (btn) {
                var title = btn.tips;
                this.$refs.btngroup.pagebox('AccountOutput', title, null, '600', '400');
            },
            //资金操作
            moneyHandle: function (row) {
                var title = '“' + row.Ac_Name + '(' + row.Ac_AccName + ')”的资金操作'
                this.$refs.btngroup.pagebox('AccountMoney?id=' + row.Ac_ID, title, null, '600', '400');
            },
            //资金流水
            capitalRecords: function (row) {
                var file = '../accounts/capitalRecords?id=' + row.Ac_ID;
                var title = '“' + row.Ac_Name + '(' + row.Ac_AccName + ')”的资金流水'
                var boxid = 'capitalRecords_' + file;
                this.$refs.btngroup.pagebox(file, title, boxid, 800, 600,
                    { pid: window.name, resize: true });
            },
            //批量删除的按钮事件
            bathdel: function (btn) {
                console.log(btn);
                this.$refs.btngroup.pagebox(btn.url, btn.text, null, '80%', '80%');
            }
        }
    });

});