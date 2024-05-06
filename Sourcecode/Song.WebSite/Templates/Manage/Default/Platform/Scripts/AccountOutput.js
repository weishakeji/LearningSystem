
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
            var th = this;
            $api.bat(
                $api.get('Organization/All', { 'use': null, 'lv': 0, 'name': '' }),
                $api.get('Platform/Domain')
            ).then(([organs, domain]) => {
                //获取结果
                th.datas = organs.data.result;
                th.domain = domain.data.result;
            }).catch(function (err) {
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
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //获取文件列表
            getFiles: function () {
                var th = this;
                $api.get('Account/ExcelFiles', { 'path': 'AccountsToExcelForOrgan' }).then(function (req) {
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
                $api.delete('Account/ExcelDelete', { 'filename': file, 'path': 'AccountsToExcelForOrgan' }).then(function (req) {
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
            },
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vapp.loadDatas', true);
            }
        },
    });

});
