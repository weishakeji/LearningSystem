//试题的展示
Vue.component('question', {
    //index:索引号，total:试题总数
    //types:试题类型，mode:0为练习模式，1为背题模式
    props: ['ques', 'index', 'total', 'types', 'mode'],
    data: function () {
        return {}
    },
    watch: {
        'ques': {
            handler(nv, ov) {
                //将试题对象中的Qus_Items，解析为json
                this.ques = window.ques.parseAnswer(nv);
                //记录答题状态
                if (!this.ques.state) {
                    this.ques['state'] = {
                        qid: nv.Qus_ID,
                        time: new Date(),
                        index: this.index,
                        ans: '',
                        correct: "null"     //是否答题正确，状态为succ,error,null
                    }
                }
            },
            immediate: true
        }
    },
    computed: {},
    mounted: function () { },
    methods: {
        //选项的序号转字母
        showIndex: function (index) {
            return String.fromCharCode(65 + index);
        },
        //试题的正确答案
        sucessAnswer: function () {
            if (this.ques.Qus_Type == 1 || this.ques.Qus_Type == 2) {
                var ansstr = '';
                for (var j = 0; j < this.ques.Qus_Items.length; j++) {
                    if (this.ques.Qus_Items[j].Ans_IsCorrect) {
                        ansstr += this.showIndex(j) + "、";
                    }
                }
                if (ansstr.indexOf("、") > -1)
                    ansstr = ansstr.substring(0, ansstr.length - 1);
                return ansstr;
            }
            if (this.ques.Qus_Type == 3) {
                return this.ques.Qus_IsCorrect ? "正确" : "错误";
            }
            if (this.ques.Qus_Type == 4) {
                return this.ques.Qus_Answer;
            }
            if (this.ques.Qus_Type == 5) {
                var ansStr = [];
                for (var i = 0; i < this.ques.Qus_Items.length; i++)
                    ansStr.push((i + 1) + "、" + this.ques.Qus_Items[i].Ans_Context);
                return ansStr.join("<br/>");
            }
        },
        //试题解答
        ques_doing: function (ans, ques) {
            var type = ques.Qus_Type;
            var func = eval('this.doing_type' + type);
            var correct = func(ans, ques);
            if (!correct) {
                $api.post('Question/ErrorAdd', { 'acid': 0, 'qid': ques.Qus_ID, 'couid': ques.Cou_ID }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        //...
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err));
            }
        },
        //单选题的解答
        //ans:某个选项
        //items:当前试题的所有选项
        doing_type1: function (ans, ques) {
            var items = ques.Qus_Items;
            for (let i = 0; i < items.length; i++) {
                if (items[i].Ans_ID == ans.Ans_ID) continue;
                items[i].selected = false;
            }
            ans.selected = !ans.selected;
            //判断是否正确
            ques.state['ans'] = ans.Ans_ID;
            ques.state['correct'] = ans.selected ? (ans.Ans_IsCorrect ? "succ" : "error") : "null";
            ques.state['time'] = new Date();
            if (ans.selected && ans.Ans_IsCorrect) vapp.swipeleft();
            return ques.state['correct'] == 'succ';
        },
        //多选题的选择
        doing_type2: function (ans, ques) {
            var items = ques.Qus_Items;
            ans.selected = !ans.selected;
            //选中的选项id,正确的选项
            var ans_ids = [], correct_arr = [];
            for (let i = 0; i < items.length; i++) {
                if (items[i].selected) ans_ids.push(items[i].Ans_ID);
                if (items[i].Ans_IsCorrect) correct_arr.push(items[i]);
            }
            //判断是否正确
            var correct = true;
            if (ans_ids.length == correct_arr.length) {
                for (let i = 0; i < correct_arr.length; i++) {
                    var exist = false;
                    for (let j = 0; j < ans_ids.length; j++) {
                        if (correct_arr[i].Ans_ID == ans_ids[j]) {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist) {
                        correct = false;
                        break;
                    }
                }
            } else {
                correct = false;
            }
            //判断是否正确
            ques.state['ans'] = ans_ids.join(',');
            ques.state['correct'] = ans_ids.length > 0 ? (correct ? "succ" : "error") : "null";
            ques.state['time'] = new Date();
            if (correct) vapp.swipeleft();
            return correct;
        },
        //判断题的选择,logic为true或false
        doing_type3: function (logic, ques) {
            if (ques.Qus_Answer == String(logic)) {
                ques.Qus_Answer = '';
            } else {
                ques.Qus_Answer = String(logic);
            }
            var correct = ques.Qus_IsCorrect == logic;
            ques.state['ans'] = String(logic);
            ques.state['correct'] = ques.Qus_Answer != '' ? (correct ? "succ" : "error") : "null";
            ques.state['time'] = new Date();
            if (correct && ques.Qus_Answer != '') vapp.swipeleft();
            return ques.state['correct'] == 'succ';
        },
        //简答题
        doing_type4: function (ans, ques) {
            var correct = ques.state.ans == ques.Qus_Answer;
            ques.state['ans'] = ques.state.ans;
            ques.state['correct'] = ques.state.ans != '' ? (correct ? "succ" : "error") : "null";
            ques.state['time'] = new Date();
            return ques.state['correct'] == 'succ';
        },
        //填空题
        doing_type5: function (ans, ques) {
            var ansstr = [];
            var correct = true;
            for (let i = 0; i < ques.Qus_Items.length; i++) {
                ansstr.push(ques.Qus_Items[i].answer);
                if (ques.Qus_Items[i].Ans_Context != ques.Qus_Items[i].answer) {
                    correct = false;
                }
            }
            ques.Qus_Answer = ansstr.join(',');
            ques.state['ans'] = ansstr.join(',');
            ques.state['correct'] = ansstr.length > 0 ? (correct ? "succ" : "error") : "null";
            ques.state['time'] = new Date();
            return ques.state['correct'] == 'succ';
        }
    },
    template: `<dd :qid="ques.Qus_ID">
    <info>
        {{index+1}}/{{total}}
        [ {{this.types[ques.Qus_Type - 1]}}题 ] 
        <slot name="buttons"></slot>   
    </info>
    <card :qid="ques.Qus_ID" :correct="ques.state ? ques.state.correct : ''" :ans="ques.state.ans">   
        <card-title v-html="ques.Qus_Title"></card-title>          
        <card-context>
            <div class="ans_area type1" v-if="ques.Qus_Type==1"  remark="单选题">
                <div v-for="(ans,i) in ques.Qus_Items" :ansid="ans.Ans_ID" 
                :selected="ans.selected" @click="ques_doing(ans,ques)">
                    <i></i>{{showIndex(i)}} .
                    <span v-html="ans.Ans_Context"></span>
                </div>
            </div>
            <div  class="ans_area type2" v-if="ques.Qus_Type==2"  remark="多选题">
                <div v-for="(ans,i) in ques.Qus_Items" :ansid="ans.Ans_ID" :selected="ans.selected" @click="ques_doing(ans,ques)">
                    <i></i>{{showIndex(i)}} .
                    <span v-html="ans.Ans_Context"></span>
                </div>
            </div>
            <div  class="ans_area type2" v-if="ques.Qus_Type==3"  remark="判断题">
                <div  :selected="ques.state.ans=='true'"  @click="ques_doing(true,ques)">
                    <i></i> 正确
                </div>
                <div  :selected="ques.state.ans=='false'"  @click="ques_doing(false,ques)">
                    <i></i> 错误
                </div>
            </div>
            <div v-if="ques.Qus_Type==4" class="type4" remark="简答题">
                <textarea rows="10" placeholder="这里输入文字" v-model.trim="ques.state.ans"></textarea>
                 <el-button type="primary" size="mini" @click="ques_doing(null,ques)">
                    <icon>&#xe84c</icon>
                    提交答案</el-button>
                </div>
            <div class="ans_area type5" v-if="ques.Qus_Type==5" remark="填空题">
                <div v-for="(ans,i) in ques.Qus_Items">
                <i></i>
                <input type="text" v-model="ans.answer"></input>                
                </div>
                <el-button type="primary" size="mini" @click="ques_doing(null,ques)">
                    <icon>&#xe84c</icon>
                    提交答案</el-button>
            </div>    
        </card-context>
    </card>
    <div v-show="mode==1 || (mode==0 && ques.state.ans!='')">
    <card class="answer">   
    <card-title><icon>&#xe816</icon> 正确答案</card-title>
    <card-context> <span v-html="sucessAnswer()"></span> </card-context>
    </card>
    <card v-if="ques.Qus_Explain!=''" class="explain">   
        <card-title><icon>&#xe85a</icon> 试题解析</card-title>
        <card-context>
            <span v-if="ques.Qus_Explain!=''" v-html="ques.Qus_Explain"></span>
            <span v-else>无</span> 
        </card-context>
    </card>
    </div>
</dd>`
});