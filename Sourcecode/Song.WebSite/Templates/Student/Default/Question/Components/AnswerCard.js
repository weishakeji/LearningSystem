//答题卡
Vue.component('answercard', {
    //试题，类型，vue对象
    props: ['questions', 'types', 'vapp'],
    data: function () {
        return {
            show: false,     //显示面板
            groups: []    //试题分组，按题型
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

    },
    mounted: function () { },
    methods: {
        //移动到指定试题
        moveto: function (q) {
           // this.vapp;
            this.vapp.swipeIndex = q.index;
            this.show = false;
        }
    },
    template: `<el-drawer :visible.sync="show" direction="ltr" class="quesCard" size="100%">
        <div class="cardTit"  slot="title">
            <span><icon>&#xe75e</icon>答题卡</span>
            <span>答题<b>{{vapp.count.answer}}</b>道 / 共<b>{{questions.length}}</b>道</span>
          </div>
        <div class="cardBox">
        <dl v-for="(g,i) in groups" v-if="g.ques.length>0">
            <dt><icon>&#xe6bd</icon> [ {{g.type}}题 ]</dt>
            <dd v-for="(q,n) in g.ques" @click="moveto(q)" :current="q.index==vapp.swipeIndex" :index="q.index"
            :correct="q.state ? q.state.correct : false">
                {{q.index+1}}
            </dd>
        </dl>
        </div>
        </el-drawer>`
});
