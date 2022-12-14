
$ready(function () {
    window.vapp = new Vue({
        el: '#app',
        data: {
            search: '',
            loading: false,
            loadingid: 0,
            datas: []     //数据源           

        },
        created: function () {
            this.getData();
        },
        methods: {
            //获取数据
            getData: function () {
                $api.get('Platform/Parameters').then(function (req) {
                    if (req.data.success) {
                        vapp.datas = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //删除
            deleteData: function (datas) {
                if (datas == '') return;
                this.$confirm('此操作将永久删除该参数, 是否继续?', '再次确认', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    $api.delete('Platform/ParamDelete', { 'key': datas.Sys_Key }).then(function (req) {
                        if (req.data.success) {
                            vapp.$message({
                                type: 'success',
                                message: '删除成功!'
                            });
                            vapp.getData();
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });

                }).catch(() => { });
            },
            //加载数据页
            calcDatas: function () {
                var arr = [];
                for (let i = 0; i < this.datas.length; i++) {
                    const item = this.datas[i];
                    if (item.Sys_Key.indexOf(this.search) > -1)
                        arr.push(item);
                }
                return arr;
            },
            //双击事件
            rowdblclick: function (row, column, event) {
                this.$refs.btngroup.modifyrow(row);
            }
        }
    });

});


