$ready(function () {
    window.vapp = new Vue({
        el: '#app',
        data: {
            id: $api.querystring('id'),
            codeset: {},
            files: [],
            loading: false
        },
        watch: {

        },
        created: function () {
            var th = this;
            th.loading = true;
            $api.bat(
                $api.get('RechargeCode/SetForID', { 'id': th.id }),
                $api.get('RechargeCode/ExcelFiles', { 'id': th.id })
            ).then(axios.spread(function (codeset, files) {
                //获取结果
                th.codeset = codeset.data.result;
                th.files = files.data.result;
            })).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        computed: {},
        methods: {
            btnOutput: function () {
                var th = this;
                this.loading = true;
                $api.get('RechargeCode/ExcelOutput', { 'id': this.id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.getFiles();
                        th.$notify({
                            message: '文件生成成功！',
                            type: 'success',
                            position: 'bottom-right',
                            duration: 2000
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //获取文件列表
            getFiles: function () {
                var th = this;
                this.files = [];
                $api.get('RechargeCode/ExcelFiles', { 'id': this.id }).then(function (req) {
                    if (req.data.success) {
                        th.files = req.data.result;
                        th.loading = false;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //删除文件
            deleteFile: function (file) {
                var th = this;
                this.loading = true;
                $api.delete('RechargeCode/ExcelDelete', { 'filename': file }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.getFiles();
                        th.$notify({
                            message: '文件删除成功！',
                            type: 'success',
                            position: 'bottom-right',
                            duration: 2000
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            }
        },
        mounted: function () {

        }
    });
});