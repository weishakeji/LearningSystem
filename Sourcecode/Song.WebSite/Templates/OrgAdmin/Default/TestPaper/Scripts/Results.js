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
                $api.get('Organization/Current'),
                $api.get('TestPaper/ForID',{'id':th.tpid})
            ).then(axios.spread(function (org,paper) {
                th.loading_init = false;
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
                th.testpaper = paper.data.result;
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
                form.tpid = this.tpid;
                $api.get("TestPaper/ResultsQueryPager", form).then(function (d) {
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
                        th.$nextTick(function () {
                            loading_obj.close();
                        });
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
                created: function () {
                    var num = String(Math.round(this.number * 100) / 100);
                    if (num.indexOf('.') > -1) {
                        this.prev = num.substring(0, num.indexOf('.'));
                        this.after = num.substring(num.indexOf('.') + 1);
                    } else {
                        this.prev = num;
                        this.dot = '&nbsp;';
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
