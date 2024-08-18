$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: {
                'lcsid': $api.querystring('id'),    //学习卡设置项的id
                'name': '', 'acc': '', 'phone': '', 'gender': -1, 'couname': '', 'size': 20, 'index': 1
            },
            loading: false,
            loadingid: -1,        //当前操作中的对象id

            datas: [],          //数据集
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行         


            //导出
            output_panel: false,      //导出面板  
            files: [],          //已经生成的excel文件列表
            loading_out: false
        },
        mounted: function () {
            this.$refs.btngroup.addbtn({
                text: '重新计算', tips: '重新计算综合成绩',
                id: 'batcalc', type: 'success',
                icon: 'a067'
            });
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
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                th.loading = true;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 105;
                th.form.size = Math.round(area / 43);
                $api.get('Learningcard/Outcomes', th.form).then(function (d) {
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
                this.$confirm('重新计算学习卡关联课程的综合成绩, 是否继续?<br/>注：完成后的自动刷新页面数据。', '提示', {
                    confirmButtonText: '确定', cancelButtonText: '取消',
                    type: 'warning', dangerouslyUseHTMLString: true
                }).then(() => this.allcalcResultScore_func())
                    .catch(() => { });
            },
            //计算所有成绩的具体方法
            allcalcResultScore_func: function () {
                var th = this;
                var loading = th.$fulloading();
                $api.get('Learningcard/ResultScoreCalc', { 'lcsid': th.form.lcsid }).then(req => {
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
            //显示完成度
            showcomplete: num => Math.round((num > 100 ? 100 : num) * 100) / 100,
            //导出学员的学习成果
            btnOutput: function () {
                //创建生成Excel
                this.loading_out = true;
                var th = this;
                $api.get('Learningcard/ResultScoreOutputExcel', { 'lcsid': th.form.lcsid }).then(function (req) {
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
                $api.get('Learningcard/ResultScoreFiles', { 'lcsid': th.form.lcsid }).then(function (req) {
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
                $api.get('Learningcard/ResultScoreFileDelete', { 'lcsid': th.form.lcsid, 'filename': file }).then(function (req) {
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