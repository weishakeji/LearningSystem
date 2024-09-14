$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            tpid: $api.querystring('tpid'),       //试卷id
            org: {},
            config: {},      //当前机构配置项
            testpaper: {},      //试卷对象
            scorerange: '成绩',     //成绩范围选择的提示信息

            form: {
                'stid': '', 'tpid': '', 'tpname': '', 'couid': '', 'sbjid': '', 'orgid': '',
                'stname': '', 'cardid': '', 'score_min': '', 'score_max': '', 'time_min': '', 'time_max': '',
                'size': 20, 'index': 1
            },

            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行

            exportVisible: false,    //导出面板是否显示
            files: [],               //导出的文件列表
            fileloading: false,      //导出时的加载状态

            loading: false,
            loadingid: 0,
            loading_init: true
        },
        mounted: function () {
            this.$refs['btngroup'].addbtn({
                text: '重新计算', tips: '重新计算成绩',
                id: 'batcalc', type: 'primary',
                icon: 'a067'
            });
            this.$refs['btngroup'].addbtn({
                text: '清空', tips: '清空当前考试下的所有成绩',
                id: 'clear', type: 'warning',
                icon: 'e839'
            });
            var th = this;
            $api.bat(
                $api.get('Organization/Current'),
                $api.get('TestPaper/ForID', { 'id': th.tpid })
            ).then(([org, paper]) => {
                //获取结果             
                th.org = org.data.result;
                //机构配置信息
                th.config = $api.organ(th.org).config;
                th.form.orgid = th.org.Org_ID;
                th.testpaper = paper.data.result;
                th.handleCurrentChange(1);
                th. getFiles();
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        created: function () {

        },
        computed: {

        },
        watch: {
        },
        methods: {
            //下拉菜单事件
            dorphandle: function (command) {
                switch (Number(command)) {
                    //全部
                    case -1:
                        this.form.score_min = '';
                        this.form.score_max = '';
                        this.scorerange = '成绩';
                        break;
                    //优秀
                    case 1:
                        this.form.score_min = Math.floor(this.testpaper.Tp_Total * 0.8);
                        this.form.score_max = this.testpaper.Tp_Total;
                        this.scorerange = '优秀（' + this.form.score_min + '分以上)';
                        break;
                    //及格
                    case 2:
                        this.form.score_min = this.testpaper.Tp_PassScore;
                        this.form.score_max = this.testpaper.Tp_Total;
                        this.scorerange = '及格（' + this.form.score_min + '分以上)';
                        break;
                    //不及格
                    case 3:
                        this.form.score_min = 0;
                        this.form.score_max = this.testpaper.Tp_PassScore - 0.01;
                        this.scorerange = '不及格（' + this.testpaper.Tp_PassScore + '分以下)';
                        break;
                    //零分
                    case 4:
                        this.form.score_min = 0;
                        this.form.score_max = 0;
                        this.scorerange = '零分';
                        break;
                }
                if (Number(command) <= 4)
                    this.handleCurrentChange(1);
            },
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 42);
                th.loading = true;
                var loading_obj = this.$fulloading();
                var form = $api.clone(this.form);
                if (form.score_min === '') form.score_min = -1;
                if (form.score_max === '') form.score_max = -1;
                form.tpid = th.tpid;
                $api.get("TestPaper/ResultsQueryPager", form).then(function (d) {
                    if (d.data.success) {
                        var result = d.data.result;
                        //添加一些字段，用于增加学员选修时间的表单
                        for (let i = 0; i < result.length; i++) {
                            result[i]['loading'] = false;
                        }
                        th.datas = result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                        th.$nextTick(function () {
                            loading_obj.close();
                        });
                    } else {
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err, '错误');
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                th.loading = true;
                $api.delete('TestPaper/ResultDelete', { 'trid': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        th.handleCurrentChange();
                        th.$nextTick(function () {
                            //loading.close();
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading = false);
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
                        $api.delete('TestPaper/resultclear', { 'tpid': th.tpid }).then(function (req) {
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
                $api.get('TestPaper/ResultsBatchCalc', { 'tpid': th.tpid }).then(req => {
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
                var id = th.datas[index].Tr_ID;
                th.loadingid = id;
                $api.get('TestPaper/ResultsCalc', { 'trid': id }).then(req => {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.datas[index].Tr_Score = result;
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
            //计算成绩
            calcscore: function (data) {
                var th = this;
                th.loadingid = data.Tr_ID;
                $api.get('TestPaper/ResultsCalc', { 'trid': data.Tr_ID }).then(function (req) {
                    if (req.data.success) {
                        data.Tr_Score = req.data.result;
                        var idx = th.datas.findIndex(item => item.Tr_ID == data.Tr_ID);
                        th.datas[idx].Tr_Score = data.Tr_Score;
                        th.$notify({
                            type: 'success',
                            message: '重新计算成功',
                            center: true
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loadingid = -1);
            },
            //已经导出的文件列表
            getFiles: function () {
                var th = this;
                th.fileloading = true;
                $api.get('TestPaper/ResultFiles', { 'tpid': th.tpid }).then(function (req) {
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
                th.fileloading = true;
                $api.post('TestPaper/ResultsOutput', { 'tpid': th.tpid}).then(function (req) {
                    if (req.data.success) {
                        th.getFiles();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => alert(err))
                    .finally(() => th.fileloading = false);
            },
            //删除文件
            deleteFile: function (file) {
                var th = this;
                this.fileloading = true;
                $api.get('TestPaper/ResultExcelDelete', { 'tpid': th.tpid, 'filename': file }).then(function (req) {
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
            //成绩回顾
            viewresult: function (data) {
                var url = '/student/test/Review';
                url = $api.url.set(url, {
                    'tr': data.Tr_ID,
                    'tp': data.Tp_Id,
                    'couid': data.Cou_ID,
                    'stid': data.Ac_ID
                });
                var title = data.Ac_Name + '-成绩回顾';
                this.$refs.btngroup.pagebox(url, title, null, 800, 600, { 'ico': 'e6f1' });
                return false;
            }
        },
        components: {
            //得分的输出，为了小数点对齐
            'score': {
                props: ['number'],
                data: function () {
                    return {
                        prev: '',
                        dot: '.',
                        after: ''
                    }
                },
                watch: {
                    'number': function (nv, ov) {
                        this.init();
                    }
                },
                created: function () {
                    this.init();
                },
                methods: {
                    init: function () {
                        var num = String(Math.round(this.number * 100) / 100);
                        if (num.indexOf('.') > -1) {
                            this.prev = num.substring(0, num.indexOf('.'));
                            this.after = num.substring(num.indexOf('.') + 1);
                        } else {
                            this.prev = num;
                            this.dot = '&nbsp;';
                        }
                    }
                },
                template: `<div class="score">
                    <span class="prev">{{prev}}</span>
                    <span class="dot" v-html="dot"></span>
                    <span class="after">{{after}}</span>
                </div>`
            }
        }
    });

});
