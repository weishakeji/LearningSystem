//答题卡
$dom.load.css([$dom.pagepath() + 'Components/Styles/AnswerSheet.css']);
Vue.component('answersheet', {
    //ansstate:答题的状态
    props: ['questions', 'types', 'ansstate', 'data'],
    data: function () {
        return {
            groups: [],    //试题分组，按题型
            showsheet: false,     //是否显示
            total: 0
        }
    },
    watch: {
        'questions': {
            handler(nv, ov) {
                if (!(this.types && this.types.length > 0)) return;
                for (let k in this.questions) {
                    if (k.indexOf('_') < 0) continue;
                    let type = Number(k.substring(k.indexOf('_') + 1));
                    const group = {
                        'type': this.types[type - 1],
                        'ques': this.questions[k]
                    }
                    this.groups.push(group);
                }
            },
            immediate: true
        }
    },
    computed: {

    },
    mounted: function () { },
    methods: {
        //判断答题是否正确
        judge: function (q) {
            return false;
            //if (!this.ansstate) return false;
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
        //显示面板
        show: function () {
            this.showsheet = true;
        },
        //当点击试题标识时
        clickEvent: function (qid, index) {
            this.showsheet = false;
            this.$emit('click', qid, index);
        },
        //试题索引号
        index: function (i, j) {
            let init = 0;
            while (--i >= 0) init += this.groups[i].ques.length;
            return init + j + 1;
        }
    },
    template: `<div :class="{'answerSheet':true,'sheet_show':showsheet}">
            <div class="sheet_title">
                <span><icon>&#xe75e</icon>答题卡</span>
                <span>答题<b>{{data.answer}}</b>道 / 共<b>{{data.total}}</b>道</span>
                <icon class="sheet_close" @click="showsheet=false">&#xe606</icon>
            </div>
            <div class="sheet_area">
                <dl v-for="(g,i) in groups" v-if="g.ques.length>0">
                    <dt><icon>&#xe6bd</icon> [ {{g.type}}题 ]</dt>
                    <dd v-for="(q,j) in g.ques" @click="clickEvent(q,index(i,j))" :current="false" :index="q.index"
                    :correct="judge(q)">
                        {{index(i,j)}}
                    </dd>
                </dl>
            </div>
        </div>`
});
