
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: {
                orgid: $api.querystring('orgid'),
                start: '', end: ''
            },
            export_interval: {},      //导出时间间隔

            //批量生成的进度
            exportProgress: {
                complete: 1, timespan: '00:00:00',
                progress: 0
            },

            exportloading: false,
            loading: false,
            files: []
        },
        mounted: function () {
            this.getFiles();
            this.getprogress();
        },
        watch: {
            //导出时间间隔
            export_interval: function (nv, ov) {
                if (nv && nv.length > 0) {
                    this.form['start'] = nv[0];
                    this.form['end'] = nv[1];
                }
                console.log(this.form);
            }
        },
        computed: {

        },
        methods: {
            //生成导出文件
            enter: function () {

            },
            //获取文件列表
            getFiles: function () {
                var th = this;
                th.loading = true;
                $api.get('Course/StudentsLogBatOutputFiles', { 'orgid': this.form.orgid }).then(function (req) {
                    if (req.data.success) {
                        th.files = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //删除文件
            deleteFile: function (file) {
                var th = this;
                this.loading = true;
                $api.delete('Course/StudentsLogBatOutputDelete', { 'filename': file }).then(function (req) {
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
                }).finally(() => th.loading = false);
            },
            //清理冗余文件
            //silent:是否静默，为true，不显示提示
            clearredundance: function (silent) {
                var th = this;
                th.loading = true;
                $api.delete('Course/StudentsLogBatOutputClear', { 'orgid': th.form.orgid }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        if (!silent)
                            th.$notify({
                                message: '清理冗余信息成功！',
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
                }).finally(() => th.loading = false);
            },
            //批量导出excel
            batch2excel: function () {
                var th = this;
                th.exportloading = true;
                $api.post('Course/StudentsLogBatExcel', th.form).then(function (req) {
                    if (req.data.success) {
                        th.exportProgress = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => alert(err))
                    .finally(() => th.getprogress());
            },
            //获取进度            
            getprogress: function () {
                var th = this;
                var orgid = $api.querystring('orgid');
                th.exportloading = true;
                $api.get('Course/StudentsLogBatExcelProgress', { 'orgid': orgid }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        let progress = Math.floor(result["complete"] / result["total"] * 1000) / 10;
                        result["progress"] = isNaN(progress) ? 0 : progress;
                        th.exportProgress = result;
                        if (th.exportProgress["successed"] === true) {
                            th.exportloading = false;
                            if (th.exportProgress["complete"] <= 0) {
                                alert('没有可供导出的课程学习信息');
                            }
                            window.setTimeout(function () {
                                th.getFiles();
                            }, 300);
                            th.clearredundance(true);
                        } else {
                            window.setTimeout(function () {
                                th.getprogress();
                            }, 3000);
                        }
                    } else {
                        th.exportloading = false;
                    }
                }).catch(function (err) {
                    console.error(err);
                    th.exportloading = false;
                    ///th.clearredundance(true);
                });
            }
        },
    });
});
