
$ready(function () {

    window.vue = new Vue({
        el: '#app',
        data: {
            datas: [],     //所有机构
            organs: [],      //选中的机构

            files: [],      //列出的文件列表

            loading: false
        },
        created: function () {
            $api.bat(
                $api.get('Organization/All', { 'use': null, 'lv': 0, 'name': '' }),
                $api.get('Platform/Domain')
            ).then(axios.spread(function (organs, domain) {
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
                //获取结果
                vue.datas = organs.data.result;
                vue.domain = domain.data.result;
            })).catch(function (err) {
                console.error(err);
            });
            this.getFiles();
        },
        wacth: {},
        methods: {
            btnOutput: function () {
                var th = this;
                th.loading = true;
                var organs = th.organs.join(',');
                $api.get('Account/ExcelOutputForOrg', { 'organs': organs }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            message: '成功生成Excel文件！',
                            type: 'success',
                            position: 'top-right',
                            duration: 2000
                        });
                        th.getFiles();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    console.error(err);
                    vue.loading = false;
                });
            },
            //获取文件列表
            getFiles: function () {
                $api.get('Account/ExcelFiles', { 'path': 'AccountsToExcelForOrgan' }).then(function (req) {
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
                $api.delete('Account/ExcelDelete', { 'filename': file, 'path':'AccountsToExcelForOrgan' }).then(function (req) {
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
            },
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vue.loadDatas', true);
            }
        },
    });

});
