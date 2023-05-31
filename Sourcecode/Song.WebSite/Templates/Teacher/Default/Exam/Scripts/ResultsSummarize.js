
$ready(function () {

    window.vapp = new Vue({
        el: '#app',
        data: {
            examid: $api.querystring('id'),
            theme: {},           //考试主题
            exams: [],           //考试场次

            form: {
                examid: $api.querystring('id'),
                sort: 0,
                name: '', idcard: '',
                min: -1, max: -1, manual: null,
                size: 20, index: 1
            },
            sorts: [],       //学员组

            visibleSummari: false,       //显示综合信息

            results: [],     //成绩
            account: {},      //当前学员信息

            accountVisible: false,   //是否显示当前学员

            //
            exportVisible: false,    //成绩导出信息是否显示
            files: [],               //导出的文件列表
            fileloading: false,      //导出时的加载状态
            loading: false,

        },
        computed: {
            //是否存在主题
            existtheme: (t) => { return !$api.isnull(t.theme) },
        },
        watch: {
            //显示考试综合
            visibleSummari: function (nv, ov) {
                if (nv == false || this.existtheme) return;
                var th = this;
                //当前考试
                $api.get('Exam/ForID', { 'id': th.examid }).then(function (req) {
                    if (req.data.success) {
                        th.theme = req.data.result;
                        $api.get('Exam/Exams', { 'uid': th.theme.Exam_UID }).then(function (req) {
                            if (req.data.success) {
                                th.exams = req.data.result;
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(err => console.error(err))
                            .finally(() => { });
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => { });
            }
        },
        mounted: function () {
            this.$refs['btngroup'].addbtn({
                text: '综合', tips: '各场次成绩',
                id: 'summari', type: 'success',
                icon: 'e6ef'
            });
            //加载学员组
            this.getsort();
        },
        created: function () {

        },
        methods: {
            //获取当前考试主题下的所有学员组
            getsort: function () {
                var th = this;
                $api.get('Exam/sort4theme', { 'examid': th.examid }).then(function (req) {
                    if (req.data.success) {
                        th.sorts = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => { });
            },
            //加载数据集
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                console.log('最小值' + th.form.min);
                console.log('最大值' + th.form.max);
                this.loading = true;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 105;
                th.form.size = Math.floor(area / 42);
                $api.get("Exam/Result4Exam", th.form).then(function (d) {
                    th.loading = false;
                    if (d.data.success) {
                        th.results = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    console.error(err);
                });
            },
            //计算考试用时
            calcSpan: function (d1, d2) {
                if (d1 == null || d2 == null) return '';
                var total = (d2.getTime() - d1.getTime()) / 1000;
                var span = Math.floor(total / 60);
                return span <= 0 ? "<1" : span;
            },
            //计算考试成绩
            clacScore: function (exrid) {
                var th = this;
                th.loadingid = exrid;
                $api.get('Exam/ClacScore', { 'exrid': exrid }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.loadingid = 0;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loadingid = 0;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            //查看综述
            summari: function () {

            },
            //当查看学员信息时，获取当前学员信息
            getaccount: function (row) {
                this.accountVisible = true;
                this.current = row;
                $api.get('Account/ForID', { 'id': row.Ac_ID }).then(function (req) {
                    if (req.data.success) {
                        vapp.account = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    console.error(err);
                });
            },
            //导出按钮的事件，显示导出面板
            output: function (btn) {
                this.exportVisible = true;
            },
            //已经导出的文件列表
            getFiles: function () {
                var th = this;
                $api.get('Exam/ExcelFiles', { 'examid': this.form.examid }).then(function (req) {
                    th.fileloading = false;
                    if (req.data.success) {
                        th.files = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    console.error(err);
                });
            },
            //生成导出文件
            toexcel: function () {
                var th = this;
                th.fileloading = true;
                $api.post('Exam/ExcelOutput', { 'examid': this.form.examid }).then(function (req) {
                    if (req.data.success) {
                        th.getFiles();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    console.error(err);
                });
            },
            //删除文件
            deleteFile: function (file) {
                var th = this;
                this.fileloading = true;
                $api.get('Exam/ExcelDelete', { 'examid': this.form.examid, 'filename': file }).then(function (req) {
                    th.fileloading = false;
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
                    //alert(err);
                    console.error(err);
                });
            },
            //打开考试回顾的窗口
            review: function (row) {
                var boxid = "ResultsReview_" + row.Exr_ID + "_" + row.Exam_ID;
                console.log(row);
                var url = $api.url.set("/student/exam/review", {
                    "examid": row.Exam_ID,
                    "exrid": row.Exr_ID
                });
                //创建
                if (!window.top.$pagebox) {
                    alert('弹窗对象不存在');
                    return;
                }
                var box = window.top.$pagebox.create({
                    width: '80%', height: '80%',
                    resize: true, id: boxid,
                    pid: window.name,
                    url: url,
                    id: boxid,
                    'showmask': true, 'min': false, 'ico': 'e696'
                });
                box.title = row.Ac_Name + '在“' + this.entity.Exam_Name + "”中的成绩回顾";
                box.open();
            }
        },
        components: {
            //得分的输出，为了小数点对齐
            'avg': {
                props: ['examid'],
                data: function () {
                    return {
                        avg: 0, high: 0, low: 0, pass: 0,
                        attend: 0
                    }
                },
                created: function () {
                    var th = this;
                    //批量访问过中会验证结果是否异常，但不会触发catch
                    $api.bat(
                        $api.get('Exam/Score4Exam', { 'examid': th.examid }),     //当前场次平均分
                        $api.get('Exam/AttendNumber', { 'examid': th.examid })     //当前场次参考人数
                    ).then(axios.spread(function (req, num) {
                        th.attend = num.data.result.number;
                        let score = req.data.result;
                        th.avg = score.average;
                        th.high == score.highest;
                        th.low == score.lowest;
                        th.pass == score.passrate;
                    })).catch(err => alert(err))
                        .finally(() => {
                            console.log('finally');
                        });
                },
                template: `<div class="score">
                    <el-tag type="primary">平均分：{{avg}}</el-tag>
                    <el-tag type="success">最高分：{{high}}</el-tag>
                    <el-tag type="info">最低分：{{low}}</el-tag>
                    <el-tag type="warning">及格率：{{pass}}</el-tag>
                    <el-tag type="info">参考人数：{{attend}}</el-tag>
                </div>`
            }
        }
    });

});
