$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: {
                name: '',
                size: 20,
                index: 1
            },
            loading: false,
            loadingid: 0,        //当前操作中的对象id
            datas: [], //账号列表
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
                var th = this;
                $api.delete('Admin/Delete', {
                    'id': datas
                }).then(function (req) {
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
                }).finally(() => { });
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
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                        //th.result = d.data;
                        //是否处于管理岗
                        for (var i = 0; i < th.datas.length; i++) {

                            $api.get('Position/ForID', {
                                'id': th.datas[i].Posi_Id
                            }).then(function (req) {
                                if (req.data.success) {
                                    var result = req.data.result;
                                    for (var j = 0; j < th.datas.length; j++) {
                                        if (th.datas[j].Posi_Id == result.Posi_Id)                                            
                                            th.datas[j].isAdminPosi = result.Posi_IsAdmin;                                       
                                    }                                  
                                } else {
                                    throw req.data.message;
                                }
                            }).catch(err => console.error(err)).finally(() => { });
                        }
                        var t = th.result;
                    } else {
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => { });
            },
            //刷新行数据，
            freshrow: function (id) {
                if (id == null || id == '' || id == 0) return this.handleCurrentChange();
                if (this.datas.length < 1) return;
                //要刷新的行数据
                let entity = this.datas.find(item => item.Acc_Id == id);
                if (entity == null) return;
                //获取最新数据，刷新
                var th = this;
                th.loadingid = id;
                $api.post('Admin/ForID', { 'id': id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        let index = th.datas.findIndex(item => item.Acc_Id == id);
                        if (index >= 0) th.$set(th.datas, index, result);
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadingid = 0);
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
                        th.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => alert(err, '错误'))
                    .finally(() => th.loadingid = 0);
            },
            //重置密码的弹窗
            EmployeePwreset: function (row) {
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