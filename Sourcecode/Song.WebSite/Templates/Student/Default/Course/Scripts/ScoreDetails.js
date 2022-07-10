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
                th.account = account.data.result;
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.getpurchase();

            })).catch(function (err) {
                th.loading_init = false;
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
            getpurchase: function () {
                var th = this;
                th.loading = true;
                $api.get('Course/Purchaselog:5', { 'couid': th.couid, 'stid': th.stid }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.purchase = req.data.result;
                        //计算得分
                        th.score = th.resultScore(th.purchase);
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
            //获取机构的配置参数
            orgconfig: function (para, def) {
                var val = Number(this.config[para]);
                if (isNaN(val)) return def ? def : '';                    
                return val;
            },
            //综合得分 purchase：课程购买记录（记录中包含学习进度等信息）
            resultScore: function (purchase) {
                var th = this;
                //视频得分
                var weight_video = orgconfig('finaltest_weight_video', 33.3);
                //加上容差
                var video = purchase.Stc_StudyScore > 0 ? purchase.Stc_StudyScore + orgconfig('VideoTolerance', 0) : 0;
                video = video >= 100 ? 100 : video;
                video = weight_video * video / 100;
                //试题得分
                var weight_ques = orgconfig('finaltest_weight_ques', 33.3);
                var ques = weight_ques * purchase.Stc_QuesScore / 100;
                //结考课试分
                var weight_exam = orgconfig('finaltest_weight_exam', 33.3);
                var exam = weight_exam * purchase.Stc_ExamScore / 100;
                //最终得分
                var score = Math.round((video + ques + exam) * 100) / 100;
                return {
                    'video': video,
                    'ques': ques,
                    'exam': exam,
                    'score': score
                }
                //获取机构的配置参数
                function orgconfig(para, def) {
                    var val = Number(th.config[para]);
                    if (isNaN(val)) return def ? def : '';                    
                    return val;
                };
            },
        }
    });

}, ['Components/outline_progress.js']);
