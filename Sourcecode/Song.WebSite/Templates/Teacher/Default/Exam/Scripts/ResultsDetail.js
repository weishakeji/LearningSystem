
$ready(['Components/setscore.js'],
    function () {
        window.vapp = new Vue({
            el: '#app',
            data: {
                form: {
                    examid: $api.querystring('id'),
                    name: '', idcard: '', stsid: 0,
                    min: -1, max: -1, manual: null,
                    size: 20, index: 1
                },
                entity: {},     //当前考试对象
                sorts: {},       //当前场次的学员组（根据参考学员判断）
                datas: [],        //成绩数据，用于列表显示
                account: {},      //当前学员信息
                current: {},        //当前行对象
                accountVisible: false,   //是否显示当前学员
                scorerange: '成绩',     //成绩范围选择的提示信息             

                loading: false,
                loadingid: 0,
                total: 1, //总记录数
                totalpages: 1, //总页数
                selects: [], //数据表中选中的行

                //考试中的人数
                examiningCount: 0
            },
            computed: {
            },
            watch: {

            },
            mounted: function () {
                this.$refs['btngroup'].addbtn({
                    text: '清空', tips: '清空当前考试下的所有成绩',
                    id: 'clear', type: 'warning',
                    icon: 'e839'
                });
                this.$refs['btngroup'].addbtn({
                    text: '重新计算', tips: '重新计算成绩',
                    id: 'batcalc', type: 'primary',
                    icon: 'a067'
                });
            },
            created: function () {
                //获取考试信息
                var th = this;
                $api.get('Exam/ForID', { 'id': this.form.examid }).then(function (req) {
                    if (req.data.success) {
                        th.entity = req.data.result;
                        th.form.max = th.entity.Exam_Total;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch((err) => console.error(err));
                th.getsorts();
                th.handleCurrentChange();

                //获取正在考试中的人数 
                th.getExamingcount();

            },
            methods: {
                //自动生成成绩
                automatically: function (attr, el) {
                    let exrid = attr.exrid;
                    //exrid不等于空，则表示是行内事件
                    if (exrid != null) {
                        //alert(exrid);
                        this.exrVisible = true;
                    }

                    //console.error(attr);
                    //console.error(el);
                    //console.error('automatically 自动生成行的数据');
                },
                //显示修改学员成绩的面板
                setAccountExr: function (row) {
                    this.exrVisible = true;
                    this.current = row;
                },
                //获取学员分组
                getsorts: function () {
                    var th = this;
                    $api.get('Exam/ResultSort4Exam', { 'examid': this.form.examid }).then(function (req) {
                        if (req.data.success) {
                            th.sorts = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                        .finally(() => { });
                },
                //下拉菜单事件
                dorphandle: function (command) {
                    switch (Number(command)) {
                        //全部
                        case -1:
                            this.form.min = -1;
                            this.form.max = -1;
                            this.scorerange = '成绩';
                            break;
                        //优秀
                        case 1:
                            this.form.min = Math.floor(this.entity.Exam_Total * 0.8);
                            this.form.max = this.entity.Exam_Total;
                            this.scorerange = '优秀（' + this.form.min + '分以上)';
                            break;
                        //及格
                        case 2:
                            this.form.min = this.entity.Exam_PassScore;
                            this.form.max = this.entity.Exam_Total;
                            this.scorerange = '及格（' + this.form.min + '分以上)';
                            break;
                        //不及格
                        case 3:
                            this.form.min = 0;
                            this.form.max = this.entity.Exam_PassScore - 0.01;
                            this.scorerange = '不及格（' + this.entity.Exam_PassScore + '分以下)';
                            break;
                        //零分
                        case 4:
                            this.form.min = 0;
                            this.form.max = 0;
                            this.scorerange = '零分';
                            break;
                    }
                    if (Number(command) <= 4)
                        this.handleCurrentChange(1);
                },
                //加载数据集
                handleCurrentChange: function (index) {
                    if (index != null) this.form.index = index;
                    var th = this;
                    //console.log('最小值' + th.form.min);
                    //console.log('最大值' + th.form.max);
                    th.loading = true;
                    //每页多少条，通过界面高度自动计算
                    var area = document.documentElement.clientHeight - 105;
                    th.form.size = Math.floor(area / 40);
                    $api.get("Exam/Result4Exam", th.form).then(function (d) {
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
                //计算所有成绩
                allcalcResultScore: function () {
                    this.$confirm('重新计算所有成绩, 是否继续?<br/>注：完成后的自动刷新页面数据。', '提示', {
                        confirmButtonText: '确定', cancelButtonText: '取消',
                        type: 'warning', dangerouslyUseHTMLString: true
                    }).then(() => this.allcalcResultScore_func())
                        .catch(() => { });
                },
                //计算所有成绩的具体方法
                allcalcResultScore_func: function () {
                    var th = this;
                    var loading = th.$fulloading();
                    $api.get('Exam/ResultBatchClac', { 'examid': th.form.examid }).then(req => {
                        if (req.data.success) {
                            var result = req.data.result;
                            th.handleCurrentChange();
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                        .finally(() => th.$nextTick(() => loading.close()));
                },
                //批量计算成绩
                batcalcResultScore: function () {
                    this.$confirm('重新计算当前页面的 ' + this.datas.length + ' 条成绩, 是否继续?', '提示', {
                        confirmButtonText: '确定',
                        cancelButtonText: '取消',
                        type: 'warning'
                    }).then(() => this.calcScore_onebyone(0))
                        .catch(() => { });
                },
                //逐一计算成绩
                calcScore_onebyone: function (index) {
                    var th = this;
                    if (index >= th.datas.length) {
                        this.$message({
                            message: '当前页的所有成绩全部计算完成！',
                            type: 'success'
                        });
                        return;
                    }
                    var id = th.datas[index].Exr_ID;
                    th.loadingid = id;
                    $api.get('Exam/ResultClacScore', { 'exrid': id }).then(req => {
                        if (req.data.success) {
                            var result = req.data.result;
                            th.datas[index].Exr_ScoreFinal = result;
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                        .finally(() => {
                            th.loadingid = -1;
                            th.calcScore_onebyone(++index);
                        });
                },
                //计算考试成绩
                clacResultScore: function (item) {
                    var th = this;
                    //当前行正在处理中
                    th.$set(item, 'inprocess', true);
                    $api.get('Exam/ResultClacScore', { 'exrid': item.Exr_ID }).then(function (req) {
                        if (req.data.success) {
                            let result = req.data.result;
                            th.$set(item, 'Exr_ScoreFinal', result);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                        .finally(() => th.$set(item, 'inprocess', false));
                },
                //删除
                deleteData: function (datas) {
                    var th = this;
                    $api.delete('Exam/ResultDelete', { 'exrid': datas }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            th.$notify({
                                type: 'success',
                                message: '成功删除' + result + '条数据',
                                center: true
                            });
                            th.handleCurrentChange();
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(err => console.error(err));
                },
                //清空
                clear: function () {
                    this.$confirm('删除所有成绩，是否继续？', '提示', {
                        confirmButtonText: '确定',
                        cancelButtonText: '取消',
                        type: 'warning'
                    }).then(() => {
                        this.$confirm('此操作将永久删除所有成绩，且无法恢复, 是否继续？', '再次提示', {
                            confirmButtonText: '确定',
                            cancelButtonText: '取消',
                            type: 'error'
                        }).then(() => {
                            var th = this;
                            var loading = this.$fulloading();
                            $api.delete('Exam/ResultClear', { 'examid': this.form.examid }).then(function (req) {
                                if (req.data.success) {
                                    th.$message({
                                        type: 'success',
                                        message: '删除成功!'
                                    });
                                    th.handleCurrentChange();
                                } else {
                                    console.error(req.data.exception);
                                    throw req.config.way + ' ' + req.data.message;
                                }
                            }).catch(err => console.error(err))
                                .finally(() => loading.close());

                        }).catch(() => { });
                    }).catch(() => { });
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
                //获取正在考试的人数
                getExamingcount: function () {
                    var th = this;
                    $api.get('Exam/ExaminingCount', { 'examid': th.form.examid }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            th.examiningCount = result.count;
                            window.setTimeout(function () {
                                th.getExamingcount();
                            }, 60 * 1000);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                        .finally(() => { });
                },
                //打开考试回顾的窗口
                review: function (row) {
                    let url = $api.url.set("/student/exam/review", {
                        "examid": row.Exam_ID,
                        "exrid": row.Exr_ID
                    });
                    let boxid = "ResultsReview_" + row.Exr_ID + "_" + row.Exam_ID;
                    let title = row.Ac_Name + '在“' + this.entity.Exam_Name + "”中的成绩回顾";
                    this._openbox(url, title, boxid, '80%', '80%', 'e696');
                },
                //导出的窗口
                output: function () {
                    let url = $api.url.set("/teacher/exam/ResultsExport", { "examid": this.form.examid });
                    let boxid = "ResultsExport_" + this.form.examid;
                    let title = '成绩导出 - “' + this.entity.Exam_Name + "”";
                    this._openbox(url, title, boxid, 800, 600, 'e73e');
                },
                _openbox: function (url, title, boxid, width, height, icon) {
                    //创建
                    if (!window.top.$pagebox) {
                        alert('弹窗对象不存在，无法打开内容');
                        return;
                    }
                    var box = window.top.$pagebox.create({
                        width: width, height: height,
                        resize: true, id: boxid,
                        pid: window.name, id: boxid,
                        url: url,
                        title: title,
                        'showmask': true, 'min': false, 'ico': icon
                    });
                    box.open();
                },
                //设置得分后，更新成绩
                setScoreupate: function (item) {                   
                    let index = this.datas.findIndex(t => t.Exr_ID == item.Exr_ID);                   
                    this.$set(this.datas, index, item);                   
                    this.$nextTick(() => {
                        this.$message({
                            message: '更新成功，新成绩 ' + item.Exr_ScoreFinal + ' 分',
                            type: 'success'
                        });
                    });
                }
            },
            filters: {
                //考试用时
                'duration': function (item) {
                    if (item == null) return '';
                    let d1 = item.Exr_CrtTime;      //考试开始时间
                    let d2 = item.Exr_IsSubmit ? item.Exr_SubmitTime : item.Exr_LastTime;     //最后答题时间
                    d2 = item.Exr_OverTime > d2 ? d2 : item.Exr_OverTime;
                    if (d1 == null || d2 == null) return '';
                    let minute = Math.round((d2.getTime() - d1.getTime()) / 1000 / 60);      //考试用时（分钟）                        
                    return (minute <= 0 ? "<1" : minute) + ' 分钟';
                }
            },
            components: {
                //得分的输出，为了小数点对齐
                'score': {
                    props: ['item'],
                    data: function () {
                        return {
                            prev: '',
                            dot: '.',
                            after: ''
                        }
                    },
                    watch: {
                        'item': {
                            handler: function (nv, ov) {
                                let score = this.item.Exr_ScoreFinal;
                                var num = String(Math.round(score * 100) / 100);
                                if (num.indexOf('.') > -1) {
                                    this.prev = num.substring(0, num.indexOf('.'));
                                    this.after = num.substring(num.indexOf('.') + 1);
                                } else {
                                    this.prev = num;
                                    this.dot = '&nbsp;';
                                }
                            }, immediate: true,
                        }
                    },
                    template: `<div class="score link" @click="$emit('click')">
                    <span class="prev">{{prev}}</span>
                    <span class="dot" v-html="dot"></span>
                    <span class="after">{{after}}</span>
                </div>`
                },
                //显示学员组名称
                'stsname': {
                    props: ['stsid'],
                    data: function () {
                        return {
                            loading: false,
                            sort: {}
                        }
                    },
                    created: function () {
                        this.init();
                    },
                    methods: {
                        init: function () {
                            var th = this;
                            th.loading = true;
                            $api.get('Account/SortForID', { 'id': th.stsid }).then(req => {
                                if (req.data.success) {
                                    th.sort = req.data.result;
                                } else {
                                    console.error(req.data.exception);
                                    throw req.config.way + ' ' + req.data.message;
                                }
                            }).catch(err => console.error(err))
                                .finally(() => th.loading = false);
                        }
                    },
                    template: `<div class="stsname">
                    <loading v-if="loading"></loading>
                    <template v-else-if="!$api.isnull(sort)">{{sort.Sts_Name}}</template>
                </div>`
                }
            }
        });
    });

