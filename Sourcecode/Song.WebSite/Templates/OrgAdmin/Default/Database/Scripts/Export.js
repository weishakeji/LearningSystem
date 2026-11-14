$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            files: [],  //已经导出的文件列表
            search: '', //搜索条件

            loadstate: {
                init: false,        //初始化
                def: false,         //默认
                get: false,         //加载数据
                update: false,      //更新数据
                del: false          //删除数据
            }
        },
        mounted: function () {
            this.getFiles();
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
            //导出
            exportbtn: function () {
                this.$confirm('是否要导出数据库结构与字段、索引的说明文档？', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    // 点击确定执行
                    this.exportfunc();
                }).catch(() => {
                    // 点击取消执行（可不写）
                });

            },
            //导出数据库结构与字段、索引的说明文档
            exportfunc: function () {
                var th = this;
                th.loadstate.update = true;
                var loading = th.$fulloading();
                $api.get("DataBase/ExportToWord")
                    .then(req => {
                        if (req.data.success) {
                            let result = req.data.result;
                            th.getFiles();
                            th.$message({
                                message: '操作成功',
                                type: 'success'
                            });
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                    .finally(() => {
                        th.$nextTick(() => loading.close());
                        th.loadstate.update = false;
                    });
            },
            //获取文件列表
            getFiles: function () {
                var th = this;
                th.loadstate.get = true;
                $api.get('Database/ExportWordFiles').then(function (req) {
                    if (req.data.success) {
                        th.files = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadstate.get = false);
            },
            //删除文件
            deleteFile: function (file) {
                var th = this;
                th.loadstate.del = true;
                $api.delete('Database/WordFileDelete', { 'filename': file }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.getFiles();
                        th.$notify({
                            message: '文件删除成功！',
                            type: 'success',
                            position: 'bottom-left',
                            duration: 2000
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loadstate.del = false);
            },
        },
        filters: {

        },
        components: {

        }
    });
});