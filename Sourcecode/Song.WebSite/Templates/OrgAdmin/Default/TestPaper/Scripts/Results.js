$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring('id'),
            org: {},
            config: {},      //当前机构配置项

            form: {
                'stid': '', 'tpid': '', 'tpname': '', 'couid': $api.querystring('id'),
                'sbjid': '', 'orgid': '',
                'stname': '', 'cardid': '', 'score_min': '', 'score_max': '', 'time_min': '', 'time_max': '',
                'size': 20, 'index': 1
            },

            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行

            loading: false,
            loadingid: 0,
            loading_init: true
        },
        mounted: function () {
            this.$refs['btngroup'].addbtn({
                text: '重新计算', tips: '重新计算成绩',
                id: 'calc', type: 'primary',
                class: 'el-icon-finished'
            });
            var th = this;
            $api.bat(
                $api.get('Organization/Current')
            ).then(axios.spread(function (org) {
                vapp.loading_init = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                    }
                }
                //获取结果             
                th.org = org.data.result;
                //机构配置信息
                th.config = $api.organ(th.org).config;
                th.form.orgid = th.org.Org_ID;
                th.handleCurrentChange(1);
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {

        },
        watch: {
        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 64);
                th.loading = true;
                $api.get("TestPaper/ResultsQueryPager", th.form).then(function (d) {
                    th.loading = false;
                    if (d.data.success) {
                        var result = d.data.result;
                        //添加一些字段，用于增加学员选修时间的表单
                        for (let i = 0; i < result.length; i++) {
                            result[i]['loading'] = false;
                        }
                        th.datas = result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err, '错误');
                    th.loading = false;
                    console.error(err);
                });
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                th.loading = true;
                $api.delete('TestPaper/ResultDelete', { 'trid': datas }).then(function (req) {
                    th.loading = false;
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
                    th.$alert(err, '错误');
                    console.error(err);
                });
            },
            //批量计算
            batchcalc: function (datas) {
                console.log(3);
            },
            //计算成绩
            calcscore: function (data) {
                var th = this;
                data.loading = true;
                $api.get('TestPaper/ResultsCalc', { 'trid': data.Tr_ID }).then(function (req) {
                    data.loading = false;
                    if (req.data.success) {
                        data.Tr_Score = req.data.result;                      
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
                    data.loading = false;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            //在列中显示信息，包含检索
            showInfo: function (txt) {
                if (txt != '' && this.form.search != '') {
                    var regExp = new RegExp(this.form.search, 'g');
                    txt = txt.replace(regExp, `<red>${this.form.search}</red>`);
                }
                return txt;
            },
        }
    });

});
