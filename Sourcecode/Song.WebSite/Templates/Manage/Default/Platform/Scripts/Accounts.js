$ready(function () {
    window.vue = new Vue({
        el: '#app',
        data: {
            form: {
                orgid: '',
                search: '',
                size: 20,
                index: 1
            },
            organs: [],
            loading: false,
            loadingid: 0,        //当前操作中的对象id
            accounts: [], //账号列表
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [] //数据表中选中的行
        },
        created: function () {
            $api.get('Organization/All', { 'use': null, 'lv': 0, 'name': '' }).then(function (req) {
                if (req.data.success) {
                    window.vue.organs = req.data.result;
                    window.vue.handleCurrentChange(1);
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });

        },
        computed: {

        },
        methods: {
            //删除
            deleteData: function (datas) {
                $api.delete('Account/Delete', { 'id': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        vue.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        window.vue.handleCurrentChange();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                });
            },
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 41);
                $api.get("Account/PagerOfAll", th.form).then(function (d) {
                    if (d.data.success) {
                        for (var i = 0; i < d.data.result.length; i++) {
                            d.data.result[i].isAdminPosi = false;
                        }
                        th.accounts = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);

                });
            },
            //在列中显示信息，包含检索
            showInfo: function (txt) {
                if (txt != '' && this.form.name != '') {
                    var regExp = new RegExp(this.form.search, 'g');
                    txt = txt.replace(regExp, `<red>${this.form.search}</red>`);
                }
                return txt;
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
                        vue.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                    th.loadingid = 0;
                }).catch(function (err) {
                    vue.$alert(err, '错误');
                });
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
            }
        }
    });

});