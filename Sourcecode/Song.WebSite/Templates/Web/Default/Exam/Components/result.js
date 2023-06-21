//成绩得分
$dom.load.css([$dom.pagepath() + 'Components/Styles/result.css']);
Vue.component('result', {
    props: ['state', 'exam', 'paper'],
    data: function () {
        return {}
    },
    computed: {
        //是否成功
        success: function () {
            return !$api.isnull(this.state) && this.state.result.exrid != null;
        }
    },
    mounted: function () { },
    methods: {
        //得分样式
        scoreStyle: function (score) {
            //总分和及格分
            var total = this.exam.Exam_Total;
            var passscore = this.paper.Tp_PassScore;
            if (score == total) return "praise";
            if (score < passscore) return "nopass";
            if (score < total * 0.8) return "general";
            if (score >= total * 0.8) return "fine";
            return "";
        },
        //计算得分
        scoreClac: function (score) {
            if (score == null) return '（错误）';
            return Math.floor(score * 100) / 100;
        },
        //跳转页面
        btnEnter: function () {
            let examid = this.state.result.examid;
            let exrid = this.state.result.exrid;
            if (examid == null || exrid == null) {
                alert('存在错误，请联系管理员！');
                return;
            }
            let url = $api.url.set('/student/exam/Review', {
                'examid': examid,
                'exrid': exrid
            });
            window.location.href = url;
        },
        //返回
        goback: function () {
            window.location.reload();
        }
    },

    template: ` <card v-if="!state.loading">
        <card-title>
            <span v-if="success">成绩递交成功 ！</span>
            <span v-else>提交失败，请联系管理员</span>
        </card-title>
        <template v-if="!state.result.async">
            <row>
            得分：<score :class="scoreStyle(state.result.score)">{{scoreClac(state.result.score)}}</score>
            </row>
            <row>总分：{{exam.Exam_Total}}分（{{paper.Tp_PassScore}}分及格）</row>
            <div class="btnEnter" @click="btnEnter">确 定</div>
        </template>
        <template v-else>
            <row>
            成绩计算需要时间，请稍后在成绩回顾中查看成绩信息。
            </row>               
            <div class="btnEnter" @click="goback">确 定</div>
        </template>
    </card>`
});