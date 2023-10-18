
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
            selectDate: '',
            pickerOptions: {
                shortcuts: [{
                    text: '最近一周', onClick(p) {
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
                        p.$emit('pick', [start, new Date()]);
                    }
                }, {
                    text: '最近一个月', onClick: (p) => p.$emit('pick', vapp.setTimeInterval(1))
                }, {
                    text: '最近三个月', onClick: (p) => p.$emit('pick', vapp.setTimeInterval(3))
                }, {
                    text: '最近六个月', onClick: (p) => p.$emit('pick', vapp.setTimeInterval(6))
                }, {
                    text: '本周', onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setDate(start.getDate() - start.getDay() + 1);
                        end.setDate(start.getDate() + 6);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '本月', onClick(picker) {
                        const start = new Date();
                        start.setDate(1);
                        var yy = start.getFullYear();
                        var mm = start.getMonth() + 1;
                        if (mm > 12) {
                            mm = 1;
                            yy = yy + 1;
                        }
                        var end = new Date(yy, mm, 0);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '本季度', onClick(picker) {
                        const start = new Date();
                        var yy = start.getFullYear();
                        var mm = start.getMonth();
                        if (mm >= 1 && mm <= 3) mm = 0;
                        if (mm >= 4 && mm <= 6) mm = 3;
                        if (mm >= 7 && mm <= 9) mm = 6;
                        if (mm >= 10 && mm <= 12) mm = 9;
                        start.setDate(1);
                        start.setMonth(mm);
                        const end = new Date(yy, mm + 3, 0);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '本年', onClick(picker) {
                        const start = new Date();
                        start.setDate(1);
                        start.setMonth(0);
                        const end = new Date(start.getFullYear(), 12, 0);
                        picker.$emit('pick', [start, end]);
                    }
                }]
            }
        },
        watch: {
            //选择时间区间
            selectDate: {
                handler(nv, ov) {
                    if (!nv) {
                        this.form.start = '';
                        this.form.end = '';
                        return;
                    }
                    //设置日间区间，从开始时间的零时，到结束时间的最后一秒
                    //例如：2021-3-1至2021-3-2，实际是2021-3-1 0:0:0至2021-3-2 23:59:59
                    this.form.start = Date.parse(nv[0].format('yyyy/MM/dd 0:0:0'));
                    let end = Date.parse(nv[1].format('yyyy/MM/dd 0:0:0'));
                    end.setDate(end.getDate() + 1);
                    this.form.end = new Date(end.getTime() - 1000);;
                },
                immediate: true,  //刷新加载 立马触发一次handler
                deep: true
            }
        },
        mounted: function () {
            this.selectDate = this.setTimeInterval(1);
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
            //日期范围的初始时间值
            const start = new Date();
            start.setDate(1);
            var yy = start.getFullYear();
            var mm = start.getMonth() + 1;
            if (mm > 12) {
                mm = 1;
                yy = yy + 1;
            }
            var end = new Date(yy, mm, 0);
            this.selectDate = [];
            this.selectDate[0] = start;
            this.selectDate[1] = end;

           
        },
        methods: {
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
                }).finally(() => th.loading = false);
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
