$ready(function () {
    window.vue = new Vue({
        el: '#app',
        data: {
            id: $api.querystring('id'),
            cardset: {},
            files: [],
            loading: false
        },
        watch: {

        },
        created: function () {
            this.loading = true;
            $api.bat(
                $api.get('Learningcard/SetForID', { 'id': this.id }),
                $api.get('Learningcard/ExcelFiles', { 'id': this.id })
            ).then(axios.spread(function (cardset, files) {
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.exception);
                        throw data.message;
                    }
                }
                vue.loading = false;
                //获取结果
                vue.cardset = cardset.data.result;
                vue.files = files.data.result;
            })).catch(function (err) {
                console.error(err);
            });
        },
        computed: {},
        methods: {
            Output: function () {
                this.loading = true;
                $api.post('Learningcard/ExcelOutput', { 'id': this.id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        vue.getFiles();
                        vue.$notify({
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
                $api.get('Learningcard/ExcelFiles', { 'id': this.id }).then(function (req) {
                    if (req.data.success) {
                        vue.files = req.data.result;
                        vue.loading = false;
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
                this.loading = true;
                $api.delete('Learningcard/ExcelDelete', { 'filename': file }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        vue.getFiles();
                        vue.$notify({
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