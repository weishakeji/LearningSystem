//课程视频的学习值
Vue.component('progress_value', {
    //config:机构的配置项，其实包括了视频完成度的容差值（VideoTolerance）
    props: ['course', 'stid', 'config'],
    data: function () {
        return {
            data: {},        //进度信息         
            percent: 0,     //完成的真实百分比
            tolerance: 10,       //容差，例如容差为10，则完成90%即可认为是100%
            purchase: {},        //购买记录，其中包含学习进度等
            score: 0,        //最终成绩
            loading: false
        }
    },
    watch: {
        'course': {
            handler: function (nv, ov) {
                this.onload();
            }, immediate: true, deep: true
        },
        'config': {
            handler: function (nv, ov) {
                if (nv && nv.VideoTolerance) {
                    this.tolerance = Number(nv.VideoTolerance);
                    this.tolerance = isNaN(this.tolerance) ? 0 : this.tolerance;
                    this.tolerance = this.tolerance <= 0 ? 0 : this.tolerance;
                }
            }, immediate: true, deep: true
        },
    },
    computed: {
        //完成度，加了容差之后的
        'progress': function () {
            return this.percent + this.tolerance >= 100 ? 100 : this.percent;
        },
    },
    mounted: function () { },
    methods: {
        onload: function () {
            var th = this;
            th.loading = true;
            $api.bat(
                $api.cache('Course/LogForVideo:5', { 'couid': th.course.Cou_ID, 'stid': th.stid }),
                $api.get('Course/Purchaselog', { 'stid': th.stid, 'couid': th.course.Cou_ID }),
            ).then(axios.spread(function (cou, purchase) {
                var result = cou.data.result;
                if (result != null && result.length > 0) {
                    th.data = result[0];
                    th.data.lastTime = new Date(th.data.lastTime);
                    th.percent = th.data.complete;
                    console.log(th.data);
                } else {
                    th.data = null;
                    th.percent = 0;
                }
                //购买记录
                th.purchase = purchase.data.result;
                th.score = th.resultScore(purchase.data.result).score;
            })).catch(err => console.error(err))
                .finally(() => th.loading = false);
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
            score = score >= 100 ? 100 : score;
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
    },
    template: `<dd>
            <loading v-if="loading"></loading>
            <slot v-else :value='progress' :score="score" :course='data' :purchase="purchase"></slot>
    </dd>`
});