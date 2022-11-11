//试题的展示
Vue.component('question', {
    //groupindex:试题题型的分组，用于排序号
    props: ['ques', 'index', 'groupindex', 'types'],
    data: function () {
        return {}
    },
    watch: {
        'ques': {
            handler(nv, ov) {
                this.ques = this.parseAnswer(nv);
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
        }
    },
    computed: {},
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
        //将试题对象中的Qus_Items，解析为json
        parseAnswer: function (ques) {
            if (ques.Qus_Type == 1 || ques.Qus_Type == 2 || ques.Qus_Type == 5) {
                if ($api.getType(ques.Qus_Items) != 'String') return ques;
                var xml = $api.loadxml(ques.Qus_Items);
                var arr = [];
                var items = xml.getElementsByTagName("item");
                for (var i = 0; i < items.length; i++) {
                    var item = $dom(items[i]);
                    var ansid = Number(item.find("Ans_ID").html());
                    var uid = item.find("Qus_UID").text();
                    var context = item.find("Ans_Context").text();
                    var isCorrect = item.find("Ans_IsCorrect").text() == "True";
                    arr.push({
                        "Ans_ID": ansid,
                        "Qus_ID": ques.Qus_ID,
                        "Qus_UID": uid,
                        "Ans_Context": context,
                        "Ans_IsCorrect": isCorrect,
                        "selected": false,
                        "answer": ''        //答题内容，用于填空题
                    });
                }
                ques.Qus_Items = arr;
            }
            return ques;
        },
        //试题解答
        ques_doing: function (ans, ques) {
            var type = ques.Qus_Type;
            var func = eval('this.doing_type' + type);
            var correct = func(ans, ques);
            ques.state['sucess'] = correct;
            ques.state['score'] = correct ? ques.Qus_Number : 0;
            if (!correct) {
                /*
                $api.post('Question/ErrorAdd', { 'acid': 0, 'qid': ques.Qus_ID, 'couid': ques.Cou_ID }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        //...
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {

                    console.error(err);
                });*/
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
            vapp.swipeleft();
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
            if (ques.Qus_Answer == String(logic)) {
                ques.Qus_Answer = '';
            } else {
                ques.Qus_Answer = String(logic);
            }
            var correct = ques.Qus_IsCorrect == logic;
            ques.state['ans'] = String(logic);
            ques.state['correct'] = ques.Qus_Answer != '' ? (correct ? "succ" : "error") : "null";
            ques.state['time'] = new Date();
            vapp.swipeleft();
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
        {{calcIndex(index+1)}}/{{vapp.questotal}}
        [ {{this.types[ques.Qus_Type - 1]}}题 ] 
        <span>（{{ques.Qus_Number}} 分）</span>
    </info>
    <card :qid="ques.Qus_ID" :correct="ques.state ? ques.state.correct : ''" :ans="ques.state.ans">   
        <card-title v-if="ques.Qus_Title!=''" v-html="ques.Qus_Title"></card-title>  
        <card-title v-else>(试题不存在）</card-title>          
        <card-context>
            <div class="ans_area type1" v-if="ques.Qus_Type==1"  remark="单选题">
                <div v-for="(ans,i) in ques.Qus_Items" :ansid="ans.Ans_ID" 
                :selected="ans.selected" @click="ques_doing(ans,ques)">
                    <i>{{showIndex(i)}} .</i>
                    <span v-html="ans.Ans_Context"></span>
                </div>
            </div>
            <div  class="ans_area type2" v-if="ques.Qus_Type==2"  remark="多选题">
                <div v-for="(ans,i) in ques.Qus_Items" :ansid="ans.Ans_ID" :selected="ans.selected" @click="ques_doing(ans,ques)">
                    <i>{{showIndex(i)}} .</i>
                    <span v-html="ans.Ans_Context"></span>
                </div>
            </div>
            <div  class="ans_area type2" v-if="ques.Qus_Type==3"  remark="判断题">
            {{ques.state.ans}}
                <div :selected="ques.Qus_Answer=='true'"  @click="ques_doing(true,ques)">
                    <i></i> 正确
                </div>
                <div :selected="ques.Qus_Answer=='false'"  @click="ques_doing(false,ques)">
                    <i></i> 错误
                </div>
            </div>
            <div v-if="ques.Qus_Type==4" class="type4" remark="简答题">
                <textarea rows="10" placeholder="这里输入文字" v-model.trim="ques.state.ans"></textarea>
                 <button type="primary" @click="ques_doing(null,ques)">提交答案</button>
                </div>
            <div class="ans_area type5" v-if="ques.Qus_Type==5" remark="填空题">
                <div v-for="(ans,i) in ques.Qus_Items">
                <i></i>
                <input type="text" v-model="ans.answer"></input>                
                </div>
                <button type="primary" @click="ques_doing(null,ques)">提交答案</button>
            </div>    
        </card-context>
    </card>
</dd>`
});