$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            cardset: {},
            files: [],
            loading: false
        },
        watch: {

        },
        created: function () {
            var th = this;
            th.loading = true;
            $api.bat(
                $api.get('Learningcard/SetForID', { 'id': this.id }),
                $api.get('Learningcard/ExcelFiles', { 'id': this.id })
            ).then(axios.spread(function (cardset, files) {
                //获取结果
                th.cardset = cardset.data.result;
                th.files = files.data.result;
            })).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        computed: {},
        methods: {
            Output: function () {
                var th = this;
                th.loading = true;
                $api.post('Learningcard/ExcelOutput', { 'id': this.id }).then(function (req) {
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
                th.loading = true;
                $api.get('Learningcard/ExcelFiles', { 'id': this.id }).then(function (req) {
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
                th.loading = true;
                $api.delete('Learningcard/ExcelDelete', { 'filename': file }).then(function (req) {
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