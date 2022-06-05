$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            tpid: $api.querystring("id"),   //试卷id
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项 
            paper: {},           //试卷对象
            course: {},          //试卷所属课程

            loading: false,
            loading_init: true,

            datas: [],           //成绩列表
            finished: false,
            query: {
                'stid': -1, 'tpid': this.tpid, 'size': 6, 'index': 0
            },
            total: 0
        },
        mounted: function () {
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current'),
                $api.get('TestPaper/ForID', { 'id': this.tpid }),
                $api.get('Course/ForID', { 'id': $api.querystring("couid") })
            ).then(axios.spread(function (account, platinfo, organ, paper, course) {
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
                vapp.account = account.data.result;
                if (vapp.islogin)
                    vapp.query.stid = vapp.account.Ac_ID;
                vapp.platinfo = platinfo.data.result;
                vapp.paper = paper.data.result;
                vapp.organ = organ.data.result;
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;
                vapp.course = course.data.result;

            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        watch: {
        },
        methods: {
            onload: function () {
                var th = this;
                if (th.query.stid <= 0) {
                    window.setTimeout(function () {
                        vapp.onload();
                    }, 100);
                    return;
                }
                th.query.tpid = $api.querystring("id", 0);
                th.query.index++;
                var query = $api.clone(this.query);
                $api.get('TestPaper/ResultsPager', query).then(function (req) {
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
                this.$dialog.confirm({
                    title: '删除成绩',
                    message: '您是否确定删除当前成绩（' + item.Tr_Score + '分）？',
                }).then(() => {
                    $api.get('TestPaper/ResultDelete', { 'trid': item.Tr_ID }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            if (result == true) {
                                vapp.$toast.success('删除成功');
                                vapp.datas = [];
                                vapp.query.index = 0;
                                vapp.finished = false;
                                vapp.total = false;
                                vapp.onload();
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
                this.$dialog.confirm({
                    title: '清空成绩',
                    message: '您是否确定清空所有历史成绩？',
                }).then(() => {
                    $api.delete('TestPaper/ResultClear', { 'acid': vapp.account.Ac_ID, 'tpid': vapp.paper.Tp_Id })
                        .then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                if (result > 0) {
                                    vapp.$toast.success('清空成功');
                                    vapp.datas = [];
                                    vapp.query.index = 0;
                                    vapp.finished = false;
                                    vapp.total = false;
                                    vapp.onload();
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
                    'couid': $api.querystring("couid")
                });
                window.location.href = url;
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
                    'couid': $api.querystring("couid")
                });
                window.location.href = url;
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
