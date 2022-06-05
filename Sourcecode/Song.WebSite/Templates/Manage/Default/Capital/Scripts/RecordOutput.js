
$ready(function () {
    window.vue = new Vue({
        el: '#app',
        data: {
            loading: false,  //
            id: $api.querystring('id'),
            files: [],          //已经生成的excel文件列表
            form: {
                from: -1,     //来源
                type: -1,     //类型，支出或充值               
                start: '',       //时间区间的开始时间
                end: ''         //结束时间               
            },
            selectDate: '',
            pickerOptions: {
                shortcuts: [{
                    text: '最近一周', onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近一个月', onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近三个月', onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近六个月', onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 180);
                        picker.$emit('pick', [start, end]);
                    }
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
            },
            //资金来源
            moneyform: [{ value: -1, label: '所有' },
            { value: 3, label: '在线支付' },
            { value: 1, label: '管理员操作' },
            { value: 4, label: '购买课程' }],
            //操作类型
            moneytype: [{ value: -1, label: '所有' },
            { value: 1, label: '支出' },
            { value: 2, label: '充值' }]
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

            this.getFiles();
        },
        methods: {
            btnOutput: function () {
                //创建生成Excel
                this.loading = true;
                $api.get('Money/ExcelOutput', this.form).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        vue.$notify({
                            message: '成功生成Excel文件！',
                            type: 'success',
                            position: 'top-right',
                            duration: 2000
                        });
                        vue.getFiles();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                    vue.loading = false;
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                    vue.loading = false;
                });
            },
            //获取文件列表
            getFiles: function () {
                $api.get('Money/ExcelFiles').then(function (req) {
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
                $api.get('Money/ExcelDelete', { 'filename': file }).then(function (req) {
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
        }
    });
});
