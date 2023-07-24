//试题的展示
Vue.component('question', {
    //groupindex:试题题型的分组，用于排序号
    props: ['ques', 'index', 'groupindex', 'types'],
    data: function () {
        return {}
    },
    watch: {
        //答题状态变动时
        'ques': {
            handler(nv, ov) {
                if ($api.isnull(nv)) return;              
                this.$nextTick(function () {
                    var dom = $dom("card[qid='" + this.ques.Qus_ID + "']");
                    //清理空元素                
                    window.ques.clearempty(dom.find('card-title'));
                    window.ques.clearempty(dom.find('.ans_area'));
                    //公式渲染
                    this.$mathjax([dom[0]]);                  
                });              
            }, immediate: true
        }
    },
    computed: {},
    mounted: function () { },
    methods: {
        //计算序号，整个试卷采用一个序号，跨题型排序
        calcIndex: function (index) {
            let gindex = this.groupindex - 1;
            let initIndex = 0;
            while (gindex >= 0) {
                initIndex += vapp.paperQues[gindex].ques.length;
                gindex--;
            };
            return initIndex + index;
        },
        //选项的序号转字母
        showIndex: function (index) {
            return String.fromCharCode(65 + index);
        },
        /*  */
        //单选题的选择
        type1_select: function (ans, items) {
            for (let index = 0; index < items.length; index++) {
                const element = items[index];
                if (element.Ans_ID == ans.Ans_ID) continue;
                element.selected = false;
            }
            ans.selected = !ans.selected;
            if (ans.selected) this.$parent.swipeleft();
        },
        //多选题的选择
        type2_select: function (ans) {
            ans.selected = !ans.selected;
        },
        //判断题的选择,logic为true或false
        type3_select: function (logic) {
            let answer = String(logic);
            if (this.ques.Qus_Answer == answer) this.ques.Qus_Answer = '';
            else {
                this.ques.Qus_Answer = answer;
                this.$parent.swipeleft();
            }
        },
        //填空题
        type5_input: function (ques) {
            var ansstr = '';
            for (let index = 0; index < ques.Qus_Items.length; index++) {
                const element = ques.Qus_Items[index];
                ansstr += element.Ans_Context + ",";
            }
            this.ques.Qus_Answer = ansstr;
        }
    },
    template: `<dd :qid="ques.Qus_ID">
    <info>
        {{calcIndex(index+1)}}/{{vapp.questotal}}
        [ {{this.types[ques.Qus_Type - 1]}}题 ] 
        <span>（{{ques.Qus_Number}} 分）</span>
    </info>
    <card :qid="ques.Qus_ID">   
        <card-title v-html="ques.Qus_Title"></card-title>
        <card-context>
            <div class="ans_area type1" v-if="ques.Qus_Type==1"  remark="单选题">
                <div v-for="(ans,i) in ques.Qus_Items" :ansid="ans.Ans_ID" 
                :selected="ans.selected" @click="type1_select(ans,ques.Qus_Items)">
                    <i>{{showIndex(i)}} .</i>
                    <span v-html="ans.Ans_Context"></span>
                </div>
            </div>
            <div  class="ans_area type2" v-if="ques.Qus_Type==2"  remark="多选题">
                <div v-for="(ans,i) in ques.Qus_Items" :ansid="ans.Ans_ID" :selected="ans.selected" @click="type2_select(ans)">
                    <i>{{showIndex(i)}} .</i>
                    <span v-html="ans.Ans_Context"></span>
                </div>
            </div>
            <div  class="ans_area type2" v-if="ques.Qus_Type==3"  remark="判断题">
                <div :selected="ques.Qus_Answer=='true'" ansid="true"  @click="type3_select(true)">
                    <i></i> 正确
                </div>
                <div :selected="ques.Qus_Answer=='false'" ansid="false" @click="type3_select(false)">
                    <i></i> 错误
                </div>
            </div>
            <div v-if="ques.Qus_Type==4" remark="答题题">
                <textarea rows="10" placeholder="这里输入文字" v-model.trim="ques.Qus_Answer"></textarea>
            </div>
            <div  class="ans_area" v-if="ques.Qus_Type==5" remark="填空题">
                <div v-for="(ans,i) in ques.Qus_Items">
                    <i></i>{{i+1}}.
                    <input type="text" v-model="ans.Ans_Context"  @input="type5_input(ques)"></input>                
                </div>
            </div>    
        </card-context>
    </card>
</dd>`
});