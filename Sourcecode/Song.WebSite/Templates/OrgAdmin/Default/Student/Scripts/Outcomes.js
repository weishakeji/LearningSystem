$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: {
                'acid': $api.querystring('id'),      //学员id 
                'sbjid': '', 'search': '', 'start': '', 'end': '',
                'size': 20, 'index': 1
            },

            loading: false,
            loadingid: 0,        //当前操作中的对象id

            datas: [],          //数据集
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行         


            //导出
            output_panel: false,      //导出面板
            query: {
                path: 'MoneyOutputToExcel_' + $api.querystring('id'),     //导出的文件的存储路径
                orgid: 0,
                acid: $api.querystring('id'),      //学员id 
                from: -1,     //来源
                type: -1,     //类型，支出或充值               
                start: '',       //时间区间的开始时间
                end: ''         //结束时间    
            },
            files: [],          //已经生成的excel文件列表
            loading_out: false
        },
        mounted: function () {

            this.handleCurrentChange();
            this.getFiles();
        },
        created: function () {

        },
        computed: {
            //表格高度
            tableHeight: function () {
                var height = document.body.clientHeight;
                return height - 75;
            }
        },
        watch: {
        },
        methods: {          
            //删除
            deleteData: function (datas) {
                var th = this;
                $api.delete('Money/Delete', { 'id': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        th.handleCurrentChange();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                }).finally(() => { });
            },
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                th.loading = true;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 105;
                th.form.size = Math.round(area / 43);
                $api.get('Account/outcomes4student', th.form).then(function (d) {
                    if (d.data.success) {
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //显示完成度
            showcomplete: num => Math.round((num > 100 ? 100 : num) * 100) / 100,
            //导出学员的资金流水
            btnOutput: function () {
                //创建生成Excel
                this.loading_out = true;
                var th = this;
                $api.get('Money/ExcelAccountOutput', th.query).then(function (req) {
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
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading_out = false);
            },
            //获取文件列表
            getFiles: function () {
                var th = this;
                $api.get('Money/ExcelFiles', { 'path': this.query.path, 'orgid': -1 }).then(function (req) {
                    if (req.data.success) {
                        th.files = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading_out = false);
            },
            //删除文件
            deleteFile: function (file) {
                var th = this;
                th.loading_out = true;
                $api.get('Money/ExcelDelete', { 'path': th.query.path, 'orgid': -1, 'filename': file }).then(function (req) {
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
                }).finally(() => th.loading_out = false);
            }
        }
    });

});