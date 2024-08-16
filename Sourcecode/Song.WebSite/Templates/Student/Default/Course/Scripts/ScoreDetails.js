$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.dot(),
            stid: $api.querystring('stid'), //学员ID
            stcid: $api.querystring('stcid'),    //学习记录的ID，即Student_Course的主键ID
            account: {},     //当前登录账号       

            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项     

            purchase: {},        //课程购买的记录
            purchase_query: { 'couid': -1, 'stid': -1 },

            loading_init: true,
            loading_fresh: 0,        //刷新的预载
            loading: false
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Account/ForID', { 'id': th.stid }),
                $api.cache('Platform/PlatInfo'),
                $api.get('Organization/Current')
            ).then(([account, platinfo, organ]) => {
                //获取结果
                th.account = account.data.result;
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.getpurchase();

            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: t => !$api.isnull(t.account),
            //是否有购买记录
            isnull: t => $api.isnull(t.purchase)
        },
        watch: {
            //当数据更新完，重新计算综合成绩
            'loading_fresh': function (n, o) {
                if (n <= 0) this.calcResultScore();
            }
        },
        methods: {
            //获取购买课程的记录
            getpurchase: function () {
                var th = this;
                th.loading = true;
                $api.get('Course/Purchaselog:5', { 'stcid': th.stcid }).then(function (req) {
                    if (req.data.success) {
                        th.purchase = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //获取机构的配置参数
            orgconfig: function (para, def) {
                var val = Number(this.config[para]);
                if (isNaN(val)) return def ? def : '';
                return val;
            },
            //更新学习记录的相关数据
            refresh_data: function () {
                this.loading_fresh = 2;
                var th = this;
                //更新视频学习进度
                $api.cache('Course/LogForVideo:update', { 'couid': th.couid, 'stid': th.stid })
                    .then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            var videonum = result != null && result.length > 0 ? result[0].complete : 0;
                            $api.post('Course/LogForVideoRecord', { 'acid': th.stid, 'couid': th.couid, 'rate': videonum })
                                .then(function (req) {
                                    if (req.data.success) {
                                        th.purchase.Stc_StudyScore = req.data.result;
                                        th.$notify({ message: '更新视频学习进度成功' });
                                    } else {
                                        console.error(req.data.exception);
                                        throw req.config.way + ' ' + req.data.message;
                                    }
                                }).catch(function (err) {
                                    console.error(err);
                                });
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(err => console.error(err))
                    .finally(() => th.loading_fresh--);
                //更新结课考试成绩
                var form = { 'acid': th.stid, 'couid': th.couid, 'score': 0 }
                $api.post('TestPaper/ResultLogRecord', form).then(function (req) {
                    if (req.data.success) {
                        th.purchase.Stc_ExamScore = req.data.result;
                        th.$notify({  message: '更新结课考试成绩成功' });
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading_fresh--);
            },
            //重新计算综合成绩
            calcResultScore: function () {
                var th = this;              
                $api.get('Course/ResultScoreCalc', { 'stcid': th.stcid }).then(req => {
                    if (req.data.success) {
                        var result = req.data.result;                      
                        th.purchase.Stc_ResultScore = result;
                        th.$notify({ type: 'success', message: '更新计算综合成绩完成' });
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadingid = -1);
            },
        }
    });

}, ['Components/outline_progress.js']);
