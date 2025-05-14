//试题的在测试与考试中的展现
$dom.load.css(['/Utilities/Components/Question/Styles/test.css']);
Vue.component('question', {
    //ques:当前试题
    //groups:试题按题型分类的试题组
    //groupindex:试题题型的分组，用于排序号
    props: ['ques', 'groups', 'index', 'swipeindex', 'groupindex', 'total', 'types'],
    data: function () {
        return {
            init: false,         //初始化完成    
            current: false       //是否是当前试题，即滑动到当前
        }
    },
    watch: {
        'ques': {
            handler(nv, ov) {
                this.ques = window.ques.parseAnswer(this.ques);
                //记录答题状态
                if (!this.ques.state) {
                    this.ques['state'] = {
                        qid: nv.Qus_ID,
                        time: new Date(),
                        index: this.index,
                        ans: '',
                        sucess: false,      //是否正确，只能是true或false
                        score: 0,            //得分
                        correct: "null"     //是否答题正确，状态为succ,error,null
                    }
                }                
            },
            immediate: true
        },
        //滑动到哪个试题的索引
        'swipeindex': {
            handler: function (nv, ov) {
                if (nv == null) return;
                var index = this.calcIndex(this.index);
                this.current = index == nv;
            }, immediate: true
        },
        //是否是当前显示的试题
        'current': {
            handler(nv, ov) {
                if (!ov && nv && !this.init) {
                    this.ques = window.ques.parseAnswer(this.ques);
                    this.init = true;
                    this.$nextTick(function () {
                        var dom = $dom("card[qid='" + this.ques.Qus_ID + "']");
                        //清理空元素                
                        window.ques.clearempty(dom.find('card-title'));
                        window.ques.clearempty(dom.find('.ans_area'));
                        //公式渲染
                        this.$mathjax([dom[0]]);
                    });
                }
                //console.log('当前试题:' + nv);
            },
            immediate: true
        }
    },
    computed: {
        //是否试题加载完成
        existques: function () {
            return JSON.stringify(this.ques) != '{}' && this.ques != null;
        }
    },
    mounted: function () { },
    methods: {
        //计算序号，整个试卷采用一个序号，跨题型排序
        calcIndex: function (index) {
            var gindex = this.groupindex - 1;
            var initIndex = 0;
            while (gindex >= 0) {
                initIndex += this.groups[gindex].ques.length;
                gindex--;
            };
            return initIndex + index;
        },
        //选项的序号转字母
        toletter: function (index) {
            return String.fromCharCode(65 + index);
        },
        //试题解答
        ques_doing: function (ans, ques) {
            var type = ques.Qus_Type;
            var func = eval('this.doing_type' + type);
            var correct = func(ans, ques);
            ques.state['sucess'] = correct;
            ques.state['score'] = correct ? Number(ques.Qus_Number) : 0;
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
            this.$parent.swipeleft();
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
            return correct;
        },
        //判断题的选择,logic为true或false
        doing_type3: function (logic, ques) {
            if (ques.state.ans == String(logic)) {
                ques.state.ans = '';
            } else ques.state.ans = String(logic);

            var correct = ques.Qus_IsCorrect == logic;
            ques.state['ans'] = String(logic);
            ques.state['correct'] = ques.state.ans != '' ? (correct ? "succ" : "error") : "null";
            ques.state['time'] = new Date();
            this.$parent.swipeleft();
            return ques.state['correct'] == 'succ';
        },
        //简答题
        doing_type4: function (ans, ques) {
            let answer = ques.Qus_Answer.replace(/<[^>]+>/g, '').trim();
            let correct = ques.state.ans == answer;
            ques.state['ans'] = ques.state.ans;
            ques.state['correct'] = ques.state.ans != '' ? (correct ? "succ" : "error") : "null";
            ques.state['time'] = new Date();
            return ques.state['correct'] == 'succ';
        },
        //填空题
        doing_type5: function (ans, ques) {
            let ansstr = [];
            let correct = true; //是否答题正确
            for (let i = 0; i < ques.Qus_Items.length; i++) {
                let answer = ques.Qus_Items[i].answer;            //答题信息
                ansstr.push(answer);
                let items = ques.Qus_Items[i].Ans_Context.split(","); //正确答案
                if (!items.includes(answer)) correct = false;
            }
            ques.state['ans'] = ansstr.join(',');
            ques.state['correct'] = ansstr.length > 0 ? (correct ? "succ" : "error") : "null";
            ques.state['time'] = new Date();
            return ques.state['correct'] == 'succ';
        }
    },
    template: `<dd :qid="ques.Qus_ID" :render="init">
    <template v-if="init">
        <info>
            {{calcIndex(index+1)}}/{{total}}
            [ {{this.types[ques.Qus_Type - 1]}}题 ] 
            <span>（{{ques.Qus_Number}} 分）</span>       
        </info>
        <card  shadow="hover" :qid="ques.Qus_ID" :correct="ques.state ? ques.state.correct : ''" :ans="ques.state.ans">   
            <card-title v-html="ques.Qus_Title"></card-title>          
            <card-context>
                <div class="ans_area type1" v-if="ques.Qus_Type==1"  remark="单选题">               
                    <div v-for="(ans,i) in ques.Qus_Items" :ansid="ans.Ans_ID" 
                    :selected="ans.selected" @click="ques_doing(ans,ques)">
                        <i>{{toletter(i)}} .</i>
                        <span v-html="ans.Ans_Context"></span>
                    </div>
                </div>
                <div  class="ans_area type2" v-if="ques.Qus_Type==2"  remark="多选题">
                    <div v-for="(ans,i) in ques.Qus_Items" :ansid="ans.Ans_ID" :selected="ans.selected" @click="ques_doing(ans,ques)">
                        <i>{{toletter(i)}} .</i>
                        <span v-html="ans.Ans_Context"></span>
                    </div>
                </div>
                <div  class="ans_area type3" v-if="ques.Qus_Type==3"  remark="判断题">
                    <div :selected="ques.state.ans=='true'"  @click="ques_doing(true,ques)">
                        <i>正确</i> 
                    </div>
                    <div :selected="ques.state.ans=='false'"  @click="ques_doing(false,ques)">
                        <i>错误</i> 
                    </div>
                </div>
                <div v-if="ques.Qus_Type==4" class="type4" remark="简答题">
                    <textarea rows="10" placeholder="这里输入文字" @blur="ques_doing(null,ques)" v-model.trim="ques.state.ans"></textarea>                
                </div>
                <div class="ans_area type5" v-if="ques.Qus_Type==5" remark="填空题">
                    <div v-for="(ans,i) in ques.Qus_Items">
                        <i>{{i+1}}.</i>
                        <input type="text" v-model.trim="ans.answer" @blur="ques_doing(null,ques)"></input>                
                    </div>                   
                </div>    
            </card-context>
        </card>
    </template>
</dd>`
});