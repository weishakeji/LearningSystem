//答题卡
Vue.component('answercard', {
    props: ['questions', 'types'],
    data: function () {
        return {
        }
    },
    watch: {
        'questions': {
            handler(nv, ov) {
                if (!(this.types && this.types.length > 0)) return;
                //生成试题的序号
                for (var i = 0; i < this.questions.length; i++) {
                    var gindex = i - 1;
                    var initIndex = 0;
                    while (gindex >= 0) {
                        initIndex += this.questions[gindex].q.length;
                        gindex--;
                    };
                    var group = this.questions[i];
                    for (var j = 0; j < group.q.length; j++) {
                        group.q[j]['index'] = initIndex + j;
                    }
                }
            },
            immediate: true
        }
    },
    computed: {
        //试题总数
        questotal: function () {
            var total = 0;
            if (this.questions == null) return 0;
            for (var i = 0; i < this.questions.length; i++) {
                total += this.questions[i].count;
            }
            return total;
        },
        //已经做的题数
        answertotal: function () {
            if (!this.questions) return 0;
            var total = 0;
            for (var i = 0; i < this.questions.length; i++) {
                for (let j = 0; j < this.questions[i].q.length; j++) {
                    const q = this.questions[i].q[j];
                    if (q.ans != '') total++;
                }
            }
            return total;
        },
    },
    mounted: function () { },
    methods: {},
    template: `<div>
        <div class="cardTit">
            <icon>&#xe75e</icon>答题卡</span>
            <span>完成<b>{{answertotal}}</b>道 / 共<b>{{questotal}}</b>道</span>
        </div>
        <div class="cardBox">
            <dl v-for="(group,i) in questions">
                <dt>
                    <icon>&#xe6bd</icon>
                    [ {{types[group.type - 1]}}题 ]
                    <span>每题{{Math.floor(group.number/group.count*100)/100}}分/共{{group.number}}分</span>
                </dt>
                <dd v-for="q in group.q" @click="vapp.swipe(q.index)" :ans="q.ans!=''"
                :current="q.index==vapp.swipeIndex" :index="q.index"
                :correct="q.state ? q.state.correct : false">
                    {{q.index+1}}
                </dd>
            </dl>
        </div>
    </div>`
});
