$ready(function () {
    window.vue = new Vue({
        el: '#app',
        data: {
            form: {
                name: '',
                size: 20,
                index: 1
            },
            loading: false,
            loadingid: 0,        //当前操作中的对象id
            accounts: [], //账号列表
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [] //数据表中选中的行
        },
        created: function () {
            this.handleCurrentChange(1);
        },
        methods: {
            //删除
            deleteData: function (datas) {
                $api.delete('Admin/Delete', {
                    'id': datas
                }).then(function (req) {
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
                $api.get("Admin/List", th.form).then(function (d) {
                    if (d.data.success) {
                        for (var i = 0; i < d.data.result.length; i++) {
                            d.data.result[i].isAdminPosi = false;
                        }
                        th.accounts = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                        //th.result = d.data;
                        //是否处于管理岗
                        for (var i = 0; i < th.accounts.length; i++) {

                            $api.get('Position/ForID', {
                                'id': th.accounts[i].Posi_Id
                            }).then(function (req) {
                                if (req.data.success) {
                                    var result = req.data.result;
                                    for (var j = 0; j < window.vue.accounts.length; j++) {
                                        if (window.vue.accounts[j].Posi_Id == result.Posi_Id) {
                                            //if(result.Posi_IsAdmin)
                                            window.vue.accounts[j].isAdminPosi = result.Posi_IsAdmin
                                        }

                                    }
                                    //th.accounts[i].isAdminPosi = result.Posi_IsAdmin;

                                } else {
                                    throw req.data.message;
                                }
                            }).catch(function (err) {
                                //alert(err);
                            });
                        }
                        var t = th.result;
                    } else {
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //双击事件
            rowdblclick: function (row, column, event) {
                this.$refs.btngroup.modify(row.Acc_Id)
            },
            //更改使用状态
            changeUse: function (row) {
                var th = this;
                this.loadingid = row.Acc_Id;
                $api.post('Admin/Modify', { 'acc': row }).then(function (req) {
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
            //重置密码的弹窗
            EmployeePwreset:function(row){
                var pbox = top.$pagebox.create({
                    width: this.width ? this.width : 400,
                    height: this.height ? this.height : 300,
                    url: this.getfullpath(id),
                    pid: window.name,
                    id: window.name + '_' + id + '[modify]',
                    title: $dom('title').text() + ' - 修改'
                });
                pbox.open();
            }
        }
    });

});