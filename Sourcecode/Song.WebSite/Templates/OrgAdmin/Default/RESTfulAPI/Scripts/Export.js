$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: {
                orgid: 0, name: '',
                size: 20, index: 1
            },
            files: [],  //已经导出的文件列表

            
            datas: [], //账号列表
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行
            loadstate: {
                init: false,        //初始化
                def: false,         //默认
                get: false,         //加载数据
                update: false,      //更新数据
                del: false          //删除数据
            }
        },
        mounted: function () {

        },
        created: function () {

        },
        computed: {
            loading: function () {
                if (!this.loadstate) return false;
                for (let key in this.loadstate) {
                    if (this.loadstate.hasOwnProperty(key)
                        && this.loadstate[key])
                        return true;
                }
                return false;
            }
        },
        watch: {

        },
        methods: {
            //删除
            deleteData: function (datas) {
                var th = this;
                th.loadstate.del = true;
                $api.delete('Admin/Delete', { 'id': datas }).then(function (req) {
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
                }).finally(() => th.loadstate.del = false);
            },
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                th.loading = true;
                //每页多少条，通过界面高度自动计算
                let area = $dom.height() - 100;
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
                            $api.get('Position/ForID', { 'id': th.datas[i].Posi_Id }).then(function (req) {
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
                }).finally(() => th.loading = false);
            },
        },
        filters: {

        },
        components: {

        }
    });
});