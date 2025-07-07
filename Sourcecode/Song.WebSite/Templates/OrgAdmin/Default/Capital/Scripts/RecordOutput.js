
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            loading: false,  //
            id: $api.querystring('id'),
            files: [],          //已经生成的excel文件列表
            //path: 'MoneyOutputToExcel',     //导出的文件的存储路径
            form: {
                path: 'MoneyOutputToExcel',     //导出的文件的存储路径
                orgid: -1,
                from: -1,     //来源                
                type: -1,     //类型，支出或充值               
                start: '',       //时间区间的开始时间
                end: ''         //结束时间               
            },
        },
        watch: {
        },
        mounted: function () {

            var th = this;
            $api.get('Organization/Current').then(function (req) {
                if (req.data.success) {
                    let result = req.data.result;
                    th.form.orgid = result.Org_ID;
                    th.getFiles();
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => {
                alert('获取当前机构信息错误');
                console.error(err);
            }).finally(() => { });

        },
        created: function () {
        },
        methods: {
            //选择时间区间
            selectDate: function (start, end) {
                this.form.start = start;
                this.form.end = end;
                this.handleCurrentChange(1);
            },
            //设置时间区间，从当前时间到之前的时间，subtract：要减去的月份
            //返回时间数组
            setTimeInterval: function (subtract) {
                let end = new Date();           // 获取当前时间     
                let month = end.getMonth();     // 获取当前月份
                let year = end.getFullYear();    //当前年份
                // 计算要减去的月份后的目标月份            
                month = month - subtract;
                year = month < 0 ? end.getFullYear() - 1 : end.getFullYear();
                if (month < 0) month += 12;
                // 设置目标日期为当前日期
                let start = new Date(end);
                start.setFullYear(year, month); // 设置目标年份和月份
                return [start, end];
            },
            //创建生成Excel
            btnOutput: function () {
                if (this.loading) return;
                var th = this;
                if (th.loading) return;
                th.loading = true;
                $api.get('Money/ExcelOutput', this.form).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            message: '成功生成Excel文件！',
                            type: 'success',
                            position: 'bottom-left',
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
                }).finally(() => setTimeout(() => th.loading = false, 1000));
            },
            //获取文件列表
            getFiles: function () {
                var th = this;
                th.loading = true;
                $api.get('Money/ExcelFiles', { 'path': th.form.path, 'orgid': th.form.orgid }).then(function (req) {
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
                if (this.loading) return;
                var th = this;
                th.loading = true;
                $api.get('Money/ExcelDelete', { 'path': this.form.path, 'orgid': th.form.orgid, 'filename': file }).then(function (req) {
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
            }
        }
    });
});
