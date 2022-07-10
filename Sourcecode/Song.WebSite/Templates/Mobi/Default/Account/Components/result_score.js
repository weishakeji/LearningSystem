//综合成绩的显示
Vue.component('result_score', {
    //config:机构的配置项，其实包括了视频完成度的容差值（VideoTolerance）
    props: ['purchase', 'stid', 'config'],
    data: function () {
        return {
            show: false,     //
            tolerance: 10,       //容差，例如容差为10，则完成90%即可认为是100%
            loading: false
        }
    },
    watch: {
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

    },
    mounted: function () { },
    methods: {
        //综合得分 purchase：课程购买记录（记录中包含学习进度等信息）
        resultScore: function () {
            var th = this;
            var purchase = th.purchase;
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

            return Math.round((video + ques + exam) * 100) / 100;
            //获取机构的配置参数
            function orgconfig(para, def) {
                var val = Number(th.config[para]);
                if (isNaN(val)) return def ? def : '';
                return val;
            };
        }
    },
    template: `<div>
        <icon>&#xe6ef</icon>
            <span>{{resultScore()}}</span>          
    </div>`
});