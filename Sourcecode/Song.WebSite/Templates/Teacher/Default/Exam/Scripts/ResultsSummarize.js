
$ready(function () {

    window.vapp = new Vue({
        el: '#app',
        data: {
            examid: $api.querystring('id'),
            theme: {},           //当前考试主题
            exams: [],           //考试场次
            sorts: [],       //当前考试主题下参考学员的学员组

            form: {
                id: $api.querystring('id'),
                name: '', idcard: '', stsid: 0,
                size: 20, index: 1
            },
            accounts: [],            //参考学员
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行



            visibleSummari: false,       //显示综合信息

            results: [],     //成绩
            account: {},      //当前学员信息

            accountVisible: false,   //是否显示当前学员


            //
            exportVisible: false,    //成绩导出信息是否显示
            files: [],               //导出的文件列表
            fileloading: false,      //导出时的加载状态
            //导出的查询对象
            exportquery: {
                'scope': 1,          //导出范围，1为所有，2为按学员组
                'sorts': []              //学员组id,多个id用逗号分隔
            },

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
            }
        },
        mounted: function () {
            this.$refs['btngroup'].addbtn({
                text: '各场次成绩', tips: '各场次成绩',
                id: 'summari', type: 'success',
                icon: 'e6f1'
            });
            //加载学员组
            this.getsort();
            //考试场次
            this.getexams();
            this.handleCurrentChange(1);
            this.getFiles();
        },
        created: function () {

        },
        methods: {
            //获取考试场次
            getexams: function () {
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
            },
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
                this.loading = true;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 105;
                th.form.size = Math.floor(area / 40);
                $api.get("Exam/AttendThemeAccounts", th.form).then(function (d) {
                    if (d.data.success) {
                        th.accounts = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //当查看学员信息时，获取当前学员信息
            getaccount: function (row) {
                var th = this;
                this.accountVisible = true;
                this.current = row;
                $api.get('Account/ForID', { 'id': row.Ac_ID }).then(function (req) {
                    if (req.data.success) {
                        th.account = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err));
            },
            //导出按钮的事件，显示导出面板
            output: function (btn) {
                this.exportVisible = true;
            },
            //已经导出的文件列表
            getFiles: function () {
                var th = this;
                $api.get('Exam/ExcelFiles', { 'examid': th.examid }).then(function (req) {
                    if (req.data.success) {
                        th.files = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.fileloading = false);
            },
            //生成导出文件
            toexcel: function () {
                var th = this;
                //导出所有参考学员
                if (th.exportquery.scope == 1) {
                    th.fileloading = true;
                    $api.post('Exam/OutputParticipate', { 'examid': th.examid, 'sorts': '' }).then(function (req) {
                        if (req.data.success) {
                            th.getFiles();
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(err => alert(err))
                        .finally(() => th.fileloading = false);
                }
                //按学员组导出
                if (th.exportquery.scope == 2) {
                    if (th.exportquery.sorts.length < 1) {
                        alert('未选择学员组');
                        return;
                    }
                    let sort = th.exportquery.sorts.join(',');
                    $api.post('Exam/OutputParticipate', { 'examid': th.examid, 'sorts': sort }).then(function (req) {
                        if (req.data.success) {
                            th.getFiles();
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(err => alert(err))
                        .finally(() => th.fileloading = false);
                }
            },
            //删除文件
            deleteFile: function (file) {
                var th = this;
                this.fileloading = true;
                $api.get('Exam/ExcelDelete', { 'examid': th.examid, 'filename': file }).then(function (req) {
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
                }).catch(err => alert(err))
                    .finally(() => th.fileloading = false);
            },
            //打开考试回顾的窗口
            review: function (score, account, exam) {
                console.log(score);
                var boxid = "ResultsReview_" + score.exrid + "_" + score.examid;
                var url = $api.url.set("/student/exam/review", {
                    "examid": score.examid,
                    "exrid": score.exrid
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
                box.title = account.Ac_Name + '在“' + exam.Exam_Name + "”中的成绩回顾";
                box.open();
            }
        },
        components: {
            //场次得分信息
            'exam_scores': {
                props: ['examid'],
                data: function () {
                    return {
                        //平均分，最高分，最低分，及格率
                        avg: 0, high: 0, low: 0, pass: 0,
                        attend: 0       //参加考试的人数
                    }
                },
                created: function () {
                    var th = this;
                    //批量访问过中会验证结果是否异常，但不会触发catch
                    $api.bat(
                        $api.get('Exam/Score4Exam', { 'examid': th.examid }),     //当前场次平均分
                        $api.get('Exam/AttendCount', { 'examid': th.examid })     //当前场次参考人数
                    ).then(([req, num]) => {
                        th.attend = num.data.result.number;
                        var score = req.data.result;
                        console.log(score);
                        th.avg = score.average;
                        th.high = score.highest;
                        th.low = score.lowest;
                        th.pass = score.passrate;
                    }).catch(err => alert(err))
                        .finally(() => {
                            console.log('finally');
                        });
                },
                template: `<div class="exam_scores">
                    <el-tag type="primary">平均分：{{avg}}</el-tag>
                    <el-tag type="success">最高分：{{high}}</el-tag>
                    <el-tag type="info">最低分：{{low}}</el-tag>
                    <el-tag type="warning">及格率：{{pass}}</el-tag>
                    <el-tag type="info">参考人数：{{attend}}</el-tag>
                </div>`
            },
            //学员每场次得分
            'score': {
                props: ['examid', 'acid'],
                data: function () {
                    return {
                        score: {},
                        loading: false
                    }
                },
                computed: {
                    change: function () {
                        const { examid, acid } = this
                        return { examid, acid };
                    }
                },
                watch: {
                    change: {
                        handler: function (val) {
                            this.getscore(val);
                        }, immediate: true, deep: true
                    }
                },
                methods: {
                    getscore: function (val) {
                        var th = this;
                        th.loading = true;
                        $api.get('Exam/ResultScore', { 'acid': val.acid, 'examid': val.examid }).then(function (req) {
                            if (req.data.success) {
                                th.score = req.data.result;
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(err => console.error(err))
                            .finally(() => th.loading = false);
                    }
                },
                created: function () {

                },
                template: `<span class="score">
                    <loading bubble v-if="loading"></loading>
                    <template v-else-if="score.score<0">-</template>
                    <span class="link" v-else @click="$emit('review',score)">{{score.score}}</span>
                </span>`
            },
        }
    });

});
