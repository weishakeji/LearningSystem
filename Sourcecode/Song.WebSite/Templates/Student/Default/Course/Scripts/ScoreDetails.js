$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.dot(),
            stid: $api.querystring('stid'),
            account: {},     //当前登录账号       

            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项     

            purchase: {},        //课程购买的记录
            purchase_query: { 'couid': -1, 'stid': -1 },
            //得分信息
            score: {},

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
            ).then(axios.spread(function (account, platinfo, organ) {
                //获取结果
                th.account = account.data.result;
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.getpurchase();

            })).catch(err => console.error(err))
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
        },
        methods: {
            //获取购买课程的记录
            getpurchase: function () {
                var th = this;
                th.loading = true;
                $api.get('Course/Purchaselog:5', { 'couid': th.couid, 'stid': th.stid }).then(function (req) {
                    if (req.data.success) {
                        th.purchase = req.data.result;
                        //计算得分
                        th.score = th.resultScore(th.purchase, th.config);
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
            //综合得分 purchase：课程购买记录（记录中包含学习进度等信息）
            resultScore: function (purchase, config) {
                if (JSON.stringify(purchase) == '{}' || purchase == null) return 0;
                //获取机构的配置参数
                let orgconfig = this.orgconfig;
                //视频得分
                let weight_video = orgconfig('finaltest_weight_video', 33.3);
                //加上容差
                let video = purchase.Stc_StudyScore > 0 ? purchase.Stc_StudyScore + orgconfig('VideoTolerance', 0) : 0;
                video = video >= 100 ? 100 : video;
                video = weight_video * video / 100;
                //试题得分
                let weight_ques = orgconfig('finaltest_weight_ques', 33.3);
                let ques = weight_ques * purchase.Stc_QuesScore / 100;
                //结考课试分
                let weight_exam = orgconfig('finaltest_weight_exam', 33.3);
                let exam = weight_exam * purchase.Stc_ExamScore / 100;
                //最终得分
                let score = Math.round((video + ques + exam) * 100) / 100;
                score = score >= 100 ? 100 : score;
                return {
                    'video': video,
                    'ques': ques,
                    'exam': exam,
                    'score': score
                }
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
                                        th.score = th.resultScore(th.purchase, th.config);
                                        th.$notify({ type: 'success', message: '更新视频学习进度成功' });
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
                        th.score = th.resultScore(th.purchase, th.config);
                        th.$notify({ type: 'success', message: '更新结课考试成绩成功' });
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading_fresh--);
            }
        }
    });

}, ['Components/outline_progress.js']);
