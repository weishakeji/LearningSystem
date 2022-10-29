//试题的展示
$dom.load.css([$dom.pagepath() + 'Components/Styles/question.css']);
Vue.component('question', {
    //state:答题状态
    //index:索引号，total:试题总数
    //types:试题类型，mode:0为练习模式，1为背题模式
    props: ['ques', 'state', 'index', 'total', 'types', 'mode'],
    data: function () {
        return {
            knowledge: {}        //试题关联的知识点
        }
    },
    watch: {
        'ques': {
            handler(nv, ov) {
                this.ques = this.parseAnswer(nv);
                this.getKnowledge(nv);
            },
            immediate: true
        }
    },
    computed: {
        //是否存在知识点
        existknl: function () {
            return JSON.stringify(this.knowledge) != '{}' && this.knowledge != null;
        }
    },
    mounted: function () { },
    methods: {
        //获取知识点
        getKnowledge: function (ques) {
            if (ques == null || ques.Kn_Uid == '') return;
            var th = this
            $api.get('Knowledge/ForUID', { 'uid': ques.Kn_Uid }).then(function (req) {
                if (req.data.success) {
                    th.knowledge = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                console.error(err);
            });
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
                //从记录中还原
                if (this.state && this.state.ans != '') {
                    if (ques.Qus_Type == 1) {
                        for (var i = 0; i < arr.length; i++) {
                            if (arr[i].Ans_ID == this.state.ans) arr[i].selected = true;
                        }
                    }
                    if (ques.Qus_Type == 2) {
                        var ans = this.state.ans.split(',');
                        for (var i = 0; i < arr.length; i++) {
                            for (var j = 0; j < ans.length; j++) {
                                if (arr[i].Ans_ID == ans[j]) arr[i].selected = true;
                            }
                        }
                    }
                    if (ques.Qus_Type == 5) {
                        var ans = this.state.ans.split(',');
                        for (var i = 0; i < arr.length; i++)
                            arr[i].answer = ans[i];
                    }
                }
                //判断、简答、填空，无需还原处理，直接从this.state.ans获取状态
                ques.Qus_Items = arr;
            }
            return ques;
        },
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
         //ans:某个选项
        //items:当前试题的所有选项
        //judge: 是否立判断对错，true为判断
        ques_doing: function (ans, ques, judge) {
            var type = ques.Qus_Type;
            var func = eval('this.doing_type' + type);
            var correct = func(ans, ques, judge);
            if (!correct) {
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
                });
            }
        },
        //单选题的解答
        //ans:某个选项
        //items:当前试题的所有选项
        doing_type1: function (ans, ques, judge) {
            var items = ques.Qus_Items;
            for (let i = 0; i < items.length; i++) {
                if (items[i].Ans_ID == ans.Ans_ID) continue;
                items[i].selected = false;
            }
            ans.selected = !ans.selected;
            //判断是否正确
            this.state['ans'] = ans.selected ? ans.Ans_ID : 0;
            this.state['correct'] = ans.selected ? (ans.Ans_IsCorrect ? "succ" : "error") : "null";
            this.state['time'] = new Date();
            if (ans.selected && ans.Ans_IsCorrect) vapp.swipeleft();
            this.$emit('answer', this.state, this.ques);
            return this.state['correct'] == 'succ';
        },
        //多选题的选择
        doing_type2: function (ans, ques, judge) {
            var items = ques.Qus_Items;
            if (ans) ans.selected = !ans.selected;
            //选中的选项id,正确的选项
            var ans_ids = [], correct_arr = [];
            for (let i = 0; i < items.length; i++) {
                if (items[i].selected) ans_ids.push(items[i].Ans_ID);
                if (items[i].Ans_IsCorrect) correct_arr.push(items[i]);
            }
            if (judge == null || !judge) return;
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
            this.state['ans'] = ans_ids.join(',');
            this.state['correct'] = ans_ids.length > 0 ? (correct ? "succ" : "error") : "null";
            this.state['time'] = new Date();
            if (correct) vapp.swipeleft();
            this.$emit('answer', this.state, this.ques);
            return correct;
        },
        //判断题的选择,logic为true或false
        doing_type3: function (logic, ques, judge) {
            if (ques.Qus_Answer == String(logic)) {
                ques.Qus_Answer = '';
            } else {
                ques.Qus_Answer = String(logic);
            }
            var correct = ques.Qus_IsCorrect == logic;
            this.state['ans'] = String(logic);
            this.state['correct'] = ques.Qus_Answer != '' ? (correct ? "succ" : "error") : "null";
            this.state['time'] = new Date();
            if (correct && ques.Qus_Answer != '') vapp.swipeleft();
            this.$emit('answer', this.state, this.ques);
            return this.state['correct'] == 'succ';
        },
        //简答题
        doing_type4: function (ans, ques, judge) {
            var correct = this.state.ans == ques.Qus_Answer;
            this.state['ans'] = this.state.ans;
            this.state['correct'] = this.state.ans != '' ? (correct ? "succ" : "error") : "null";
            this.state['time'] = new Date();
            this.$emit('answer', this.state, this.ques);
            return this.state['correct'] == 'succ';
        },
        //填空题
        doing_type5: function (ans, ques, judge) {
            var ansstr = [];
            var correct = true;
            for (let i = 0; i < ques.Qus_Items.length; i++) {
                ansstr.push(ques.Qus_Items[i].answer);
                if (ques.Qus_Items[i].Ans_Context != ques.Qus_Items[i].answer) {
                    correct = false;
                }
            }
            ques.Qus_Answer = ansstr.join(',');
            this.state['ans'] = ansstr.join(',');
            this.state['correct'] = ansstr.length > 0 ? (correct ? "succ" : "error") : "null";
            this.state['time'] = new Date();
            this.$emit('answer', this.state, this.ques);
            return this.state['correct'] == 'succ';
        }
    },
    template: `<dd :qid="ques.Qus_ID">
    <info>
        {{index+1}}/{{total}}
        [ {{this.types[ques.Qus_Type - 1]}}题 ] 
        <slot name="buttons"></slot>   
    </info>
    <card :qid="ques.Qus_ID" :correct="state ? state.correct : ''" :ans="state.ans">   
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
                <div v-for="(ans,i) in ques.Qus_Items" :ansid="ans.Ans_ID" :selected="ans.selected" @click="ques_doing(ans,ques,false)">
                    <i></i>{{showIndex(i)}} .
                    <span v-html="ans.Ans_Context"></span>
                </div>
                <button type="primary" @click="ques_doing(null,ques,true)">提交答案</button>
            </div>
            <div  class="ans_area type2" v-if="ques.Qus_Type==3"  remark="判断题">
                <div  :selected="state.ans=='true'"  @click="ques_doing(true,ques)">
                    <i></i> 正确
                </div>
                <div  :selected="state.ans=='false'"  @click="ques_doing(false,ques)">
                    <i></i> 错误
                </div>
            </div>
            <div v-if="ques.Qus_Type==4" class="type4" remark="简答题">
                <textarea rows="10" placeholder="这里输入文字" v-model.trim="state.ans"></textarea>
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
    <div v-show="mode==1 || (mode==0 && state.ans!='')">
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
        <card class="knowledge" v-if="existknl" >   
            <card-title><icon>&#xe6b0</icon> 相关知识点</card-title>
            <card-context>
                <div>{{knowledge.Kn_Title}}</div>
                <div v-html="knowledge.Kn_Details"></div>
            </card-context>
        </card>
    </div>
</dd>`
});