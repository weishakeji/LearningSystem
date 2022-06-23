$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring("couid"),
            tpid: 0,         //试卷id
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项 

            paper: {},           //试卷对象
            course: {},          //试卷所属课程
            purchase: {},           //课程购买记录

            loading: false,
            loading_init: true,

            datas: [],           //成绩列表
            finished: false,
            query: {
                'stid': -1, 'tpid': 0, 'size': 6, 'index': 0
            },
            total: 0
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current'),
                $api.get('Course/ForID', { 'id': th.couid })
            ).then(axios.spread(function (account, platinfo, organ, course) {
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
                th.account = account.data.result;
                if (th.islogin) {
                    th.query.stid = th.account.Ac_ID;
                    th.getpurchase(th.account.Ac_ID, th.couid);
                }
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.course = course.data.result;
                if (JSON.stringify(th.course) != '{}' && th.course != null) {
                    $api.get('TestPaper/FinalPaper', { 'couid': th.course.Cou_ID, 'use': true }).then(function (req) {
                        if (req.data.success) {
                            th.paper = req.data.result;
                            th.tpid = th.paper.Tp_Id;
                            th.query.tpid = th.tpid;
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        //alert(err);
                        Vue.prototype.$alert(err);
                        console.error(err);
                    });
                }
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
            //获取购买课程的记录
            getpurchase: function (stid, couid) {
                var th = this;
                th.loading = true;
                $api.get('Course/Purchaselog:5', { 'stid': stid, 'couid': couid }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.purchase = req.data.result;
                        console.log(th.purchase);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            //加载考试成绩
            onload: function () {
                var th = this;
                if (th.query.stid <= 0 || th.query.tpid <= 0) {
                    window.setTimeout(function () {
                        th.onload();
                    }, 100);
                    return;
                }
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
            //结果考试的按钮是否通过,为true时表示不通过
            final_disable: function () {
                var final_condition = this.$refs["final_condition"];
                if (!final_condition) return false;
                return final_condition.final_disable();
            },
            //参加载测试
            btnDoing: function () {
                var disabled = this.final_disable();
                if (disabled) {
                    this.$dialog.alert({
                        title: '未满足结课条件',
                        message: '未满足结课条件，不能参加结课考试',
                        theme: 'round-button',
                    }).then(() => {
                        // on close
                    });
                    return;
                }

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

}, ['Components/TestHeader.js',
    'Components/final_condition.js']);
