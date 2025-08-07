$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            examid: $api.querystring('examid'),
            sorts: [],       //当前场次的学员组（根据参考学员判断）
            //

            files: [],               //导出的文件列表
            fileloading: false,      //导出时的加载状态
            //导出的查询对象
            exportquery: {
                'scope': 1,          //导出范围，1为所有，2为按学员组
                'sorts': []              //学员组id,多个id用逗号分隔
            },
            loading: false,
        },
        mounted: function () {
            this.getsorts();
            this.getFiles();
        },
        created: function () {

        },
        computed: {

        },
        watch: {

        },
        methods: {
            //获取学员分组
            getsorts: function () {
                var th = this;
                $api.get('Exam/ResultSort4Exam', { 'examid': this.examid }).then(function (req) {
                    if (req.data.success) {
                        th.sorts = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => { });
            },
            //已经导出的文件列表
            getFiles: function () {
                var th = this;
                th.fileloading = true;
                $api.get('Exam/ExcelResultsFiles', { 'examid': this.examid }).then(function (req) {
                    if (req.data.success) {
                        th.files = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.fileloading = false);
            },
            //生成导出文件
            toexcel: function () {
                var th = this;
                 //学员组id的字符串
                 let sort = '';   
                  //按学员组导出
                if (th.exportquery.scope == 2) {
                    if (th.exportquery.sorts.length < 1) {
                        alert('未选择学员组');
                        return;
                    }
                    sort = th.exportquery.sorts.join(',');
                }
                th.fileloading = true;
                $api.post('Exam/ResultsExport4Eaxm', { 'examid': th.examid, 'sorts': sort }).then(function (req) {
                    if (req.data.success) {
                        th.getFiles();
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.fileloading = false);
            },
            //删除文件
            deleteFile: function (file) {
                var th = this;
                this.fileloading = true;
                $api.get('Exam/ExcelDelete', { 'examid': this.examid, 'filename': file }).then(function (req) {
                    th.fileloading = false;
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
                }).catch(err => alert(err))
                    .finally(() => th.fileloading = false);
            },
        },
        filters: {

        },
        components: {

        }
    });
});