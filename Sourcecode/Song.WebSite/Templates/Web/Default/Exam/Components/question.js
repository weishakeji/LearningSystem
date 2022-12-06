//试题的展示
$dom.load.css([$dom.pagepath() + 'Components/Styles/question.css']);
Vue.component('question', {
    //groupindex:试题题型的分组，用于排序号
    //total: 试题总数
    props: ['ques', 'index', 'groupindex', 'types','total'],
    data: function () {
        return {}
    },
    watch: {
        'ques': {
            handler(nv, ov) {
                //this.ques = this.parseAnswer(nv);
            },
            immediate: true
        }
    },
    computed: {},
    updated: function () {
        this.$mathjax();
        //没有内容的html元素，不显示
        var qbox = $dom('card[qid="' + this.ques.Qus_ID + '"]');
        this.clearempty(qbox.find('card-title'));
        this.clearempty(qbox.find('.ans_area'));
    },
    mounted: function () { },
    methods: {
        
        //计算序号，整个试卷采用一个序号，跨题型排序
        calcIndex: function (index) {
            var gindex = this.groupindex - 1;
            var initIndex = 0;
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
        //单选题的选择
        type1_select: function (ans, items) {
            for (let index = 0; index < items.length; index++) {
                const element = items[index];
                if (element.Ans_ID == ans.Ans_ID) continue;
                element.selected = false;
            }
            ans.selected = !ans.selected;
            this.$parent.swipeleft();
        },
        //多选题的选择
        type2_select: function (ans) {
            ans.selected = !ans.selected;
        },
        //判断题的选择,logic为true或false
        type3_select: function (logic) {
            this.ques.Qus_Answer = String(logic);
            this.$parent.swipeleft();
        },
        //填空题
        type5_input: function (ques) {
            var ansstr = '';
            for (let index = 0; index < ques.Qus_Items.length; index++) {
                const element = ques.Qus_Items[index];
                ansstr += element.Ans_Context + ",";
            }
            this.ques.Qus_Answer = ansstr;
        },
        //清理空html元素，内容为空的html标签隐藏起来，免得占空间
        clearempty: function (dom) {
            var txt = dom.text();
            if (txt.length < 1) dom.hide();
            var childs = dom.childs();
            var th = this;
            if (childs.length > 0) {
                childs.each(function () {
                    th.clearempty($dom(this));
                });
            }           
        }
    },
    template: `<dd :qid="ques.Qus_ID">
        <info>
            {{calcIndex(index+1)}}/{{total}}
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
                <div class="ans_area type3" v-if="ques.Qus_Type==3"  remark="判断题">
                    <div :selected="ques.Qus_Answer=='true'"  @click="type3_select(true)">
                        <i> 正确</i>
                    </div>
                    <div :selected="ques.Qus_Answer=='false'"  @click="type3_select(false)">
                        <i> 错误</i>
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