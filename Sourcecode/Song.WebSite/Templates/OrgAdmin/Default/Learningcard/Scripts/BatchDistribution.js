$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            lsid: $api.querystring('id'),
            cardset: {},     //学习卡的设置项
            courses: [],
            cards: [],           //学习卡卡号

            loading: false,
            loadingcard: false,     //加载学习卡卡号的预载

            num: {
                total: 0,
                used: 0,     //已经使用的个数
                rollbak: 0,      //回滚的个数
                disable: 0,      //禁用个数
                usable: 0            //可用的数量
            },
            accounts: [],     //要处理的学员       
            distribution_tatol: 0   //要处理的学员数，默认等于accounts.length       
        },
        watch: {
            distribution_tatol: function (ov, nv) {
                if (ov <= 0) {
                    this.getdatainfo();
                    this.operateSuccess();
                }
            }
        },
        computed: {

        },
        created: function () {
            var th = this;
            this.lsid = $api.querystring('id');
            th.loading = true;
            $api.get('Learningcard/SetForID', { 'id': th.lsid }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    th.cardset = req.data.result;
                    th.cardset.courses = [];
                    $api.get('Learningcard/SetCourses', { 'id': th.lsid }).then(function (req) {
                        if (req.data.success) {
                            th.cardset.courses = [];
                            th.courses = req.data.result;
                        }
                    }).catch(function (err) {
                        th.$alert(err);
                        console.error(err);
                    });
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                th.$alert(err);
                th.loading = false;
                console.error(err);
            });
            this.getdatainfo();
            this.getCards();
        },
        methods: {
            //获取学习卡数据统计信息
            getdatainfo: function () {
                var th = this;
                $api.get('Learningcard/SetDataInfo', { 'id': this.lsid }).then(function (req) {
                    if (req.data.success) {
                        th.num = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err);
                    console.error(err);
                });
            },
            //加载学习卡的卡号
            getCards: function () {
                var th = this;
                th.loadingcard = true;
                $api.get('Learningcard/cards', { 'lsid': th.lsid, 'enable': true, 'used': false }).then(function (req) {
                    th.loadingcard = false;
                    if (req.data.success) {
                        th.cards = req.data.result;
                        th.num.usable = th.cards.length;
                        console.log(th.cards);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loadingcard = false;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            //向学员派发学习卡           
            distribution: function (arr) {
                if (this.loadingcard) {
                    this.$alert('学习卡卡号信息未加载完成，请联系管理员', '数加载异常', {
                        confirmButtonText: '确定',
                        dangerouslyUseHTMLString: true
                    });
                    return;
                }
                if (arr.length < 1) {
                    this.$alert('没有可供派发学习卡的学员，请正确选择', '数量为空', {
                        confirmButtonText: '确定',
                        dangerouslyUseHTMLString: true
                    });
                    return;
                }
                //可用的卡号数量
                var usable = this.num.usable;
                if (arr.length > usable) {
                    this.$alert('可用学习卡数量 <b>' + usable + '</b> 张，要派发的学员数量为 <b>' + arr.length + '</b> 位。<br/>' +
                        '请减少要派发的学员数量，或返回学习卡主题管理，增加学习卡数量。', '操作出数量', {
                        confirmButtonText: '确定',
                        dangerouslyUseHTMLString: true
                    });
                    return;
                }
                this.$confirm('是否批量派发 '+arr.length+' 张学习卡?', '确认', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    this.accounts = [];
                    this.accounts = arr;
                    this.distribution_tatol = arr.length;
                    this.distribution_func(0);
                }).catch(() => { });
            },
            //派发学习卡的具体方法
            distribution_func: function (index) {
                if (this.accounts.length < 1 || index > this.accounts.length - 1) return;
                var th = this;
                var card = th.cards[index];
                var acc = th.accounts[index];
                $api.post('Learningcard/UseCode', { 'card': card, 'account': acc }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        if (result) {
                            th.distribution_tatol--;
                            var index = th.cards.findIndex(x => x.Lc_Code == card.Lc_Code);
                            th.$delete(th.cards[index]);
                            th.distribution_func(++index);
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.distribution_tatol--;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            //操作成功
            operateSuccess: function () {
                if (window.top.$pagebox)
                    window.top.$pagebox.source.tab(window.name, 'vue.handleCurrentChange', true);
            }
        }
    });

}, ['/Utilities/Components/student_batch.js']);