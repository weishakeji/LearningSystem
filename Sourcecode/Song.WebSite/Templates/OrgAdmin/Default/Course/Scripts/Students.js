$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            organ: {},
            config: {},
            course: {},
            form: {
                'couid': $api.querystring('id'), 'stsid': 0,
                'acc': '', 'name': '', 'idcard': '', 'mobi': '', 'size': 10, 'index': 1
            },
            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数

            //导出相关
            exportVisible: false,    //显示导出面板
            exportform: {
                couid: $api.querystring('id'), start: '', end: ''
            },
            files: [],               //导出的文件列表
            fileloading: false,      //导出时的加载状态

            loading: true,
            loadingid: 0,
            loading_init: true
        },
        mounted: function () {
            this.$refs.btngroup.addbtn({
                text: '重新计算', tips: '重新计算学员综合成绩',
                id: 'batcalc', type: 'success',
                icon: 'a067'
            });
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Course/ForID', { 'id': th.form.couid }),
                $api.get('Organization/Current')
            ).then(([course, organ]) => {
                //获取结果
                th.course = course.data.result;
                document.title = '学习记录-《' + th.course.Cou_Name + '》';
                th.organ = organ.data.result;
                th.config = $api.organ(th.organ).config;
                th.getdata(1);
                th.getFiles();
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
            //加载数据页
            getdata: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 41);
                th.loading = true;
                $api.get("Course/Students", th.form).then(function (d) {
                    if (d.data.success) {
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                        console.log(th.datas);
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //重新计算综合成绩
            calcResultScore: function (stcid) {
                var th = this;
                th.loadingid = stcid;
                $api.get('Course/ResultScoreCalc', { 'stcid': stcid }).then(req => {
                    if (req.data.success) {
                        var result = req.data.result;
                        //更新结果
                        var idx = th.datas.findIndex(item => item.Stc_ID == stcid);
                        th.datas[idx].Stc_ResultScore = result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadingid = -1);
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
                var stcid = th.datas[index].Stc_ID;
                th.loadingid = stcid;
                $api.get('Course/ResultScoreCalc', { 'stcid': stcid }).then(req => {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.datas[index].Stc_ResultScore = result;
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
            //计算所有成绩
            allcalcResultScore: function () {
                this.$confirm('重新当前课程的所有综合成绩, 是否继续?<br/>注：完成后的自动刷新页面数据。', '提示', {
                    confirmButtonText: '确定', cancelButtonText: '取消',
                    type: 'warning', dangerouslyUseHTMLString: true
                }).then(() => this.allcalcResultScore_func())
                    .catch(() => { });
            },
            //计算所有成绩的具体方法
            allcalcResultScore_func: function () {
                var th=this;
                var loading = th.$fulloading();
                $api.get('Course/ResultScoreBatchCalc', { 'couid': th.form.couid }).then(req => {
                    if (req.data.success) {
                        var result = req.data.result;    
                        th.getdata();                    
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() =>  th.$nextTick(() => loading.close()));
            },
            //显示电话
            showTel: function (row) {
                if (row.Ac_MobiTel1 == '' && row.Ac_MobiTel2 == '') return '';
                if (row.Ac_MobiTel1 == '') row.Ac_MobiTel1 = row.Ac_MobiTel2;
                if (row.Ac_MobiTel1 == row.Ac_MobiTel2) return row.Ac_MobiTel1;
                return row.Ac_MobiTel1 + (row.Ac_MobiTel2 != '' ? '/' + row.Ac_MobiTel2 : '');
            },
            //显示完成度
            showcomplete: num => Math.round((num > 100 ? 100 : num) * 100) / 100,
            //显示帮助
            btnhelp: function () {
                var msg = "课程的“学员数”与课程的“学习记录数”之间，可能会存在差异；<br/>";
                msg += "课程的学员数是选修该课程的学员总数；有可能存在学员选修课程后并没有学习，没有生成学习记录。";
                this.$alert(msg, '说明', {
                    dangerouslyUseHTMLString: true
                });
            },
            //已经导出的文件列表
            getFiles: function () {
                var th = this;
                $api.get('Course/StudentsLogOutputFiles', { 'couid': th.form.couid }).then(function (req) {
                    if (req.data.success) {
                        th.files = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.fileloading = false);
            },
            //生成导出文件
            toexcel: function () {
                var th = this;
                th.fileloading = true;
                $api.post('Course/StudentsLogOutputExcel', th.exportform).then(function (req) {
                    if (req.data.success) {
                        th.getFiles();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => { alert(err); console.error(err); })
                    .finally(() => th.fileloading = false);
            },
            //删除文件
            deleteFile: function (file) {
                var th = this;
                th.fileloading = true;
                $api.get('Course/StudentsLogOutputDelete', { 'couid': th.form.couid, 'filename': file }).then(function (req) {
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
                }).catch(err => console.error(err))
                    .finally(() => th.fileloading = false);
            },
        }
    });
});
