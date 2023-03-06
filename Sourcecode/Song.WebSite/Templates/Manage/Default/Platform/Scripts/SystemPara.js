
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
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
                var th = this;
                $api.get('Platform/Parameters').then(function (req) {
                    if (req.data.success) {
                        th.datas = req.data.result;
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
            //当前行是否显示
            rowshow: function (row) {
                if (this.search == '') return true;
                var search = this.search.toLowerCase();
                return row.Sys_Key.toLowerCase().indexOf(search) > -1
                    || row.Sys_Value.toLowerCase().indexOf(search) > -1
                    || row.Sys_ParaIntro.toLowerCase().indexOf(search) > -1;
            },
            //清理html标签
            clearhtml: function (html, len) {
                html = html.replace(/<\/?.+?\/?>/g, '');
                if (len == null || len >= html.length) return html;
                return html.substring(0, len);
            }
        }
    });

});


