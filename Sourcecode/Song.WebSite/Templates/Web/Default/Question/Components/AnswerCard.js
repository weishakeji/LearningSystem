//答题卡
$dom.load.css([$dom.pagepath() + 'Components/Styles/answercard.css']);
Vue.component('answercard', {
    //ansstate:答题的状态
    props: ['questions', 'types', 'ansstate', 'width'],
    data: function () {
        return {
            groups: [],    //试题分组，按题型
            show: false
        }
    },
    watch: {
        'questions': {
            handler(nv, ov) {
                if (!(this.types && this.types.length > 0)) return;
                for (var i = 0; i < this.types.length; i++) {
                    var group = {
                        'type': this.types[i],
                        'ques': []
                    }
                    for (var j = 0; j < this.questions.length; j++) {
                        var q = this.questions[j];
                        q['index'] = j;
                        if (q.Qus_Type == (i + 1))
                            group.ques.push(q);
                    }
                    this.groups.push(group);
                }
            },
            immediate: true
        }
    },
    computed: {
        'width_percent': function () {
            if (!this.width) return 60;
            return this.width;
        }
    },
    mounted: function () { },
    methods: {
        //判断答题是否正确
        judge: function (q) {
            var items = this.ansstate.data.items;
            var item = null;
            for (var j = 0; j < items.length; j++) {
                if (q.Qus_ID == items[j].qid) {
                    item = items[j];
                    break;
                }
            }
            if (item == null) return false;
            if (item.correct == null) return false;
            return item.correct;
        },
        //当点击试题标识时
        clickEvent: function (index, q) {
            this.show = false;
            this.$emit('click', index, q);
        }
    },
    template: `<el-drawer :visible.sync="show" direction="ltr" class="quesCard" append-to-body :size="width_percent+'%'">
        <div class="cardTit" slot="title">
            <span><icon>&#xe75e</icon>答题卡</span>
            <span>答题<b>{{vapp.count.answer}}</b>道 / 共<b>{{questions.length}}</b>道</span>
          </div>
        <div class="cardBox">
            <dl  v-for="(g,i) in groups" v-if="g.ques.length>0">
                <dt><icon>&#xe6bd</icon> [ {{g.type}}题 ]</dt>
                <dd v-for="(q,n) in g.ques" @click="clickEvent(q.index,q)" :current="q.index==vapp.swipeIndex" :index="q.index"
                :correct="judge(q)">
                    {{q.index+1}}
                </dd>
            </dl>
        </div>
    </el-drawer>`
});
