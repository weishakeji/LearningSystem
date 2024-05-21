
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.dot(),
            organ: {},       //当前机构
            config: {},
            sort: {},        //当前学员组

            files: [],      //导出的excel文件列表

            loading_init: false,
            loading: false
        },
        computed: {
            //是否新增账号
            isadd: t => t.id == null || t.id == '' || this.id == 0,
        },
        mounted: function () {
        },
        created: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Organization/Current'),
                $api.get('Account/SortForID', { 'id': th.id })
            ).then(([organ, sort]) => {
                //获取结果             
                th.organ = organ.data.result;
                th.sort = sort.data.result;
                //机构配置信息
                th.config = $api.organ(vapp.organ).config;
                this.getFiles(false);
            }).catch(function (err) {
                console.error(err);
            }).finally(() => th.loading_init = false);

        },
        methods: {
            //创建生成Excel
            btnOutput: function (isall) {
                var th = this;
                th.loading = true;
                $api.get('Account/SortOutcomesToExcel', { 'stsid': th.id, 'isall': isall }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            message: '成功生成Excel文件！',
                            type: 'success',
                            position: 'bottom-left',
                            duration: 2000
                        });
                        th.filepanel = true;
                        th.getFiles(false);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //获取文件列表
            getFiles: function (hide) {
                var th = this;
                th.loading = true;
                $api.get('Account/SortOutcomesExcelFiles', { 'stsid': th.id }).then(function (req) {
                    if (req.data.success) {
                        th.files = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //删除文件
            deleteFile: function (file) {
                var th = this;
                this.loading = true;
                $api.delete('Account/ExcelDelete', { 'filename': file, 'path': 'SortOutcomesToExcel\\' + th.id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.getFiles(true);
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
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
        },
    });

});
