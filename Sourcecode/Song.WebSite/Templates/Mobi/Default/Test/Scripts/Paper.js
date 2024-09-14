$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            tpid: $api.dot(),   //试卷id
            couid: $api.querystring("couid"),
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项 
            paper: {},           //试卷对象
            course: {},          //试卷所属课程

            loading: false,
            loading_init: true,

            datas: [],           //成绩列表
            finished: false,
            query: {
                'stid': -1, 'tpid': $api.dot(), 'size': 6, 'index': 0
            },
            total: 0
        },
        mounted: function () {
            var th = this;
            $api.get('TestPaper/ForID', { 'id': th.tpid }).then(function (req) {
                if (req.data.success) {
                    th.paper = req.data.result;
                    if (th.couid == '') th.couid = th.paper.Cou_ID;
                    $api.get('Course/ForID', { 'id': th.couid }).then(function (req) {
                        if (req.data.success) {
                            th.course = req.data.result;
                        } else {
                            throw req.data.message;
                        }
                    }).catch(err => console.error(err))
                        .finally(() => { });
                } else {
                    throw req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: t => !$api.isnull(t.account)
        },
        watch: {
            'account': {
                handler: function (nv, ov) {
                    if ($api.isnull(nv)) return;
                    this.query.stid = nv.Ac_ID;
                }, immediate: true
            },
        },
        methods: {
            onload: function () {
                var th = this;
                if (th.query.stid <= 0) {
                    window.setTimeout(function () {
                        th.onload();
                    }, 100);
                    return;
                }
                th.query.tpid = th.tpid;
                th.query.index++;
                var query = $api.clone(this.query);
                $api.put('TestPaper/ResultsPager', query).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.total = req.data.total;
                        var result = req.data.result;
                        for (var i = 0; i < result.length; i++) {
                            th.datas.push(result[i]);
                        }
                        var totalpages = req.data.totalpages;
                        // 数据全部加载完成
                        if (th.datas.length >= th.total || result.length == 0) {
                            th.finished = true;
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }

                }).catch(function (err) {
                    th.loading = false;
                    th.error = err;
                    console.error(err);
                });
            },
            //删除某个测试成绩
            btnDelete: function (item) {
                var th = this;
                this.$dialog.confirm({
                    title: '删除成绩',
                    message: '您是否确定删除当前成绩（' + item.Tr_Score + '分）？',
                }).then(() => {
                    $api.get('TestPaper/ResultDelete', { 'trid': item.Tr_ID }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            if (result == true) {
                                th.$toast.success('删除成功');
                                th.datas = [];
                                th.query.index = 0;
                                th.finished = false;
                                th.total = false;
                                th.onload();
                            }
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });
                }).catch(() => {
                    // on cancel
                });
            },
            //清空成绩
            btnDeleteAll: function () {
                var th = this;
                this.$dialog.confirm({
                    title: '清空成绩',
                    message: '您是否确定清空所有历史成绩？',
                }).then(() => {
                    $api.delete('TestPaper/ResultClearForStuednt', { 'acid': th.account.Ac_ID, 'tpid': th.paper.Tp_Id })
                        .then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                if (result > 0) {
                                    th.$toast.success('清空成功');
                                    th.datas = [];
                                    th.query.index = 0;
                                    th.finished = false;
                                    th.total = false;
                                    th.onload();
                                }
                            } else {
                                console.error(req.data.exception);
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                            console.error(err);
                        });
                }).catch(() => {
                    // on cancel
                });
            },
            //参加载测试
            btnDoing: function () {
                var file = "doing";
                var url = $api.url.set(file, {
                    'tpid': this.tpid,
                    'couid': this.couid
                });
                window.navigateTo(url);
            },
            //单元格滑动时
            cell_swipe: function (event) {
                var position = event.position;
                if (position == "right") {
                    console.log("显示详情");
                }
            },
            //进入详情页
            godetail: function (item) {
                var file = "Review";
                var url = $api.url.set(file, {
                    'tr': item.Tr_ID,
                    'tp': item.Tp_Id,
                    'couid': this.couid
                });
                window.navigateTo(url);
            },
            //得分样式
            scoreStyle: function (score) {
                //总分和及格分
                var total = this.paper.Tp_Total;
                var passscore = this.paper.Tp_PassScore;
                if (score == total) return "praise";
                if (score < passscore) return "nopass";
                if (score < total * 0.8) return "general";
                if (score >= total * 0.8) return "fine";
                return "";
            }
        }
    });

}, ['Components/TestHeader.js']);
