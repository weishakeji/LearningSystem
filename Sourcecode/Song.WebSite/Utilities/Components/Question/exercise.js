﻿//试题的练习
$dom.load.css(['/Utilities/Components/Question/Styles/exercise.css']);
//事件：
//answer:当答题状态变化时触发，返回答题状态与试题
Vue.component('question', {
    //qid:当前试题的id
    //state:答题状态
    //index:索引号，
    //total:试题总数
    //types:试题类型，
    //mode:0为练习模式，1为背题模式
    //current:当前显示的试题，即滑动到这个试题
    props: ['qid', 'state', 'index', 'total', 'types', 'mode', 'current', 'account', 'fontsize'],
    data: function () {
        return {
            init: false,         //初始化完成     
            ques: {},                //当前试题   
            knowledge: {},        //试题关联的知识点

            error: '',           //错误信息
            loading: false,       //试题加载中

            //强制渲染，当答题时加一，不知道为什么有些题答题后页面没有渲染，只好采这种变态的方法
            forced_rendering: 0,

            ai_show: false, //AI解析的面板显示
            loading_ai: false, //加载Ai解析的状态
            loading_score: false,   //加载分数的状态
        }
    },
    watch: {
        'qid': {
            handler(nv, ov) { },
            immediate: true
        },
        //试题总数变化时（例如删除错题），重新处理当前试题
        'total': function (nv, ov) {
            if (nv && this.current) {
                this.initialization();
            }
        },
        //是否是当前显示的试题
        'current': {
            handler(nv, ov) {
                if (!ov && nv && !this.init)
                    this.initialization();
            },
            immediate: true
        },
        'fontsize': function (nv, ov) {
            if ($api.isnull(nv)) return;
            if (this.init) this.setfontsize(nv - ov);
        }
    },
    computed: {
        //是否存在知识点
        existknl: t => !$api.isnull(t.knowledge),
    },
    updated: function () {
        //this.$mathjax();
    },
    mounted: function () { },
    methods: {
        //始始化的方法
        initialization: function () {
            if (this.qid == null) return;
            var th = this;
            th.loading = true;
            //缓存一个月
            $api.cache('Question/ForID:43200', { 'id': th.qid }).then(function (req) {
                if (req.data.success) {
                    th.ques = req.data.result;
                    th.getKnowledge(th.ques);
                    th.ques = th.parseAnswer(th.ques);
                    //console.error(th.ques)
                    th.$nextTick(function () {
                        var dom = $dom("dd[qid='" + th.ques.Qus_ID + "']");
                        let title = dom.find('card-title');
                        //清理空元素                
                        window.ques.clearempty(dom.find('card-title'));
                        window.ques.clearempty(dom.find('.ans_area'));
                        //公式渲染
                        th.$mathjax([dom[0]]);
                        window.setTimeout(function () {
                            th.setfontsize(th.fontsize);
                            th.$mathjax([dom[0]]);
                        }, 200);

                    });
                } else {
                    console.error(req);
                    throw req.data.message;
                }
            }).catch(err => th.error = err)
                .finally(() => {
                    th.loading = false;
                    th.init = true;
                });
        },
        //设置字体大小
        setfontsize: function (num) {
            //var num = this.fontsize;
            var size = 16, min_size = 12, max_size = 30;
            ergodic($dom("dl.quesArea dd[qid='" + this.qid + "']"), num);
            function ergodic(dom, num) {
                //如果是公式，则不处理
                let domname = dom[0].tagName.toLowerCase();
                if (domname.toLowerCase() == 'mjx-container') return;
                //
                var fontsize = parseInt(dom.css("font-size"));
                fontsize = isNaN(fontsize) ? size : fontsize;
                if (num < 0 && fontsize + num > min_size) fontsize += num;
                if (num > 0 && fontsize + num < max_size) fontsize += num;
                dom.css("font-size", fontsize + "px", true);
                var child = dom.childs();
                if (child.length < 1) return;
                child.each(function (node) {
                    var n = $dom(this);
                    if (n.attr('no-font-size') != null) return true;
                    ergodic(n, num);
                });
            }
        },
        //获取知识点
        getKnowledge: function (ques) {
            if (ques == null || ques.Kn_Uid == '') return;
            var th = this
            $api.get('Knowledge/ForUID', { 'uid': ques.Kn_Uid }).then(function (req) {
                if (req.data.success) {
                    th.knowledge = req.data.result;
                } else {
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
            });
        },
        //AI解析答案
        getAiexplain: function () {
            var th = this;
            if (th.loading_ai) return;
            let type = this.types[th.ques.Qus_Type - 1];    //试题类型
            //试题选项
            let items = '';
            if (th.ques.Qus_Type == 1 || th.ques.Qus_Type == 2) {
                for (var i = 0; i < th.ques.Qus_Items.length; i++)
                    items += th.toletter(i) + "、" + th.ques.Qus_Items[i].Ans_Context + "\n";
            }
            if (th.ques.Qus_Type == 3) items += "A、正确；B、错误\n";
            if (th.ques.Qus_Type == 4) items += "";
            if (th.ques.Qus_Type == 5) {
                for (var i = 0; i < th.ques.Qus_Items.length; i++)
                    items += (i + 1) + "、" + th.ques.Qus_Items[i].Ans_Context + "\n";
            }
            //答案
            let answer = this.sucessAnswer(this.ques);

            th.ai_show = true;
            th.loading_ai = true;
            $api.post('Question/AIExplain', { 'qid': th.qid, 'type': type, 'items': items, 'answer': answer }).then(req => {
                if (req.data.success) {
                    var result = req.data.result;
                    let aiexplain = typeof marked === 'undefined' ? result : marked.parse(result);
                    th.$set(th.ques, 'AI_Explain', aiexplain);
                    //th.$set(th.ques, 'Qus_Explain', aiexplain);
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading_ai = false);
        },
        //AI计算简答题得分
        getAiscore: async function (qid, answer) {
            var th = this;
            if (th.loading_score) return;
            th.loading_score = true;
            await $api.post('Question/AICalcScore', { 'qid': qid, 'answer': answer, 'num': 10 }).then(req => {
                if (req.data.success) {
                    let score = req.data.result;  
                    th.$set(th.state, 'score', score);
                    th.$set(th.state, 'correct', score >= 10 ? "succ" : "error");
                    //触发答题事件
                    th.$emit('answer', th.state, th.ques);
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading_score = false);
        },
        //将试题对象中的Qus_Items，解析为json，并还原答题状态
        parseAnswer: function (ques) {
            ques = window.ques.parseAnswer(ques);
            var arr = ques.Qus_Items;
            //从记录中还原
            if (this.state && this.state.ans != '') {
                if (ques.Qus_Type == 1) {
                    for (var i = 0; i < arr.length; i++) {
                        if (arr[i].Ans_ID == this.state.ans) arr[i].selected = true;
                    }
                }
                if (ques.Qus_Type == 2) {
                    let ans = this.state.ans ? this.state.ans.split(',') : [];
                    for (var i = 0; i < arr.length; i++) {
                        for (var j = 0; j < ans.length; j++) {
                            if (arr[i].Ans_ID == ans[j]) arr[i].selected = true;
                        }
                    }
                }
                if (ques.Qus_Type == 5) {
                    let ans = this.state.ans ? this.state.ans.split(',') : [];
                    for (var i = 0; i < arr.length; i++)
                        arr[i].answer = ans[i];
                }
            }
            //判断、简答、填空，无需还原处理，直接从this.state.ans获取状态
            ques.Qus_Items = arr;
            return ques;
        },
        //选项的序号转字母
        toletter: index => String.fromCharCode(65 + index),
        //试题的正确答案
        sucessAnswer: function (ques) {
            if (ques.Qus_Type == 1 || ques.Qus_Type == 2) {
                var ansstr = '';
                for (var j = 0; j < ques.Qus_Items.length; j++) {
                    if (ques.Qus_Items[j].Ans_IsCorrect)
                        ansstr += this.toletter(j) + "、";
                }
                if (ansstr.indexOf("、") > -1)
                    ansstr = ansstr.substring(0, ansstr.length - 1);
                return ansstr;
            }
            if (ques.Qus_Type == 3) return ques.Qus_IsCorrect ? "正确" : "错误";
            if (ques.Qus_Type == 4) return ques.Qus_Answer;
            if (ques.Qus_Type == 5) {
                var ansStr = [];
                for (var i = 0; i < ques.Qus_Items.length; i++)
                    ansStr.push((i + 1) + "、" + ques.Qus_Items[i].Ans_Context);
                return ansStr.join("<br/>");
            }
        },
        //试题解答
        //ans:某个选项
        //items:当前试题的所有选项
        //judge: 是否立判断对错，true为判断
        ques_doing: function (ans, ques, judge) {
            if (this.mode % 2 == 1) {
                alert('背题模式下不可以答题，请切换到“答题模式”');
                return;
            }
            this.forced_rendering++;        //强制渲染的冗余值
            var type = ques.Qus_Type;
            var func = eval('this.doing_type' + type);
            var correct = func(ans, ques, judge);
            if (correct == null) return;
            //只有点击时，才添加记录的时间
            let state = $api.clone(this.state);
            state['time'] = new Date();
            //触发答题事件
            this.$emit('answer', state, this.ques);
            if (!correct) {
                let acid = $api.isnull(this.account) ? 0 : this.account.Ac_ID;
                if (acid <= 0) return;
                $api.post('Question/ErrorAdd', { 'acid': acid, 'qid': ques.Qus_ID, 'couid': ques.Cou_ID })
                    .then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
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
        //judge: 是否立判断对错，true为判断
        doing_type1: function (ans, ques, judge) {
            if (this.state['ans'] == String(ans.selected ? ans.Ans_ID : 0)) return null;
            let items = ques.Qus_Items;
            for (let i = 0; i < items.length; i++) {
                if (items[i].Ans_ID == ans.Ans_ID) continue;
                items[i].selected = false;
            }
            ans.selected = !ans.selected;
            //判断是否正确
            this.state['ans'] = String(ans.selected ? ans.Ans_ID : 0);
            this.state['correct'] = ans.selected ? (ans.Ans_IsCorrect ? "succ" : "error") : "null";
            if (ans.selected && ans.Ans_IsCorrect) this.$parent.swipe({ 'direction': 2 });
            return this.state['correct'] == 'succ';
        },
        //多选题的选择
        //ans:某个选项
        //items:当前试题的所有选项
        //judge: 是否立判断对错，true为判断
        doing_type2: function (ans, ques, judge) {
            var items = ques.Qus_Items;
            if (ans) ans.selected = !ans.selected;
            //选中的选项id,正确的选项
            var ans_ids = [], correct_arr = [];
            for (let i = 0; i < items.length; i++) {
                if (items[i].selected) ans_ids.push(items[i].Ans_ID);
                if (items[i].Ans_IsCorrect) correct_arr.push(items[i]);
            }
            if (this.state['ans'] == ans_ids.join(',')) return;
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
            if (correct) this.$parent.swipe({ 'direction': 2 });
            return correct;
        },
        //判断题的选择,logic为true或false
        doing_type3: function (logic, ques, judge) {
            let ans = String(logic).toLowerCase();
            if (this.state['ans'] == ans) return null;
            ques.Qus_Answer = ques.Qus_Answer == ans ? '' : ans;
            let correct = ques.Qus_IsCorrect == logic;
            this.state['ans'] = ans;
            this.state['correct'] = ques.Qus_Answer != '' ? (correct ? "succ" : "error") : "null";
            if (correct && ques.Qus_Answer != '') this.$parent.swipe({ 'direction': 2 });
            return this.state['correct'] == 'succ';
        },
        //简答题
        doing_type4: function (ans, ques, judge) {
            let answer = ques.Qus_Answer.replace(/<[^>]+>/g, '').trim();
            let correct = this.state.ans == answer || this.state.score >= 10;
            this.state['ans'] = ans;
            this.state['correct'] = this.state.ans != '' ? (correct ? "succ" : "error") : "null";
            //console.error(this.state.ans);
            this.getAiscore(ques.Qus_ID, this.state.ans);
            return this.state['correct'] == 'succ';
        },
        //填空题
        doing_type5: function (ans, ques, judge) {
            let ansstr = [];
            let correct = true;
            for (let i = 0; i < ques.Qus_Items.length; i++) {
                let answer = ques.Qus_Items[i].answer;            //答题信息
                ansstr.push(answer);
                let items = ques.Qus_Items[i].Ans_Context.split(","); //正确答案
                if (!items.includes(answer)) {
                    correct = false;
                    break;
                }
            }
            ques.Qus_Answer = ansstr.join(',');
            this.state['ans'] = ansstr.join(',');
            this.state['correct'] = ansstr.length > 0 ? (correct ? "succ" : "error") : "null";
            if (correct) this.$parent.swipe({ 'direction': 2 });
            return correct;
        }
    },
    template: `<dd :qid="qid" :current="current" :render="init">
    <div loading="p1" v-if="loading"></div>  
    <div v-else-if="error!=''" class="error"> 
        <div>{{index+1}}/{{total}} 试题加载错误！</div>
        <alert v-html="error"></alert>
    </div>
    <template v-else-if="init">
        <info no-font-size>
            <span>
                <i>{{index+1}}/{{total}}</i>
                [ {{types[ques.Qus_Type - 1]}}题 ] 
            </span>
            <slot name="buttons" :ques="ques"></slot>          
        </info>
        <section>
            <card :correct="state ? state.correct : ''" :ans="state.ans" :forced_rendering="forced_rendering">   
                <card-title v-html="ques.Qus_Title"></card-title>          
                <card-content>
                    <div class="ans_area type1" v-if="ques.Qus_Type==1"  remark="单选题">
                        <div v-for="(ans,i) in ques.Qus_Items" :ansid="ans.Ans_ID" 
                        :selected="ans.selected" @click="ques_doing(ans,ques)">
                            <i>{{toletter(i)}} .</i>
                            <span v-html="ans.Ans_Context"></span>
                        </div>
                    </div>
                    <div  class="ans_area type2" v-if="ques.Qus_Type==2"  remark="多选题">
                        <div v-for="(ans,i) in ques.Qus_Items" :ansid="ans.Ans_ID" :selected="ans.selected" @click="ques_doing(ans,ques,false)">
                            <i>{{toletter(i)}} .</i>
                            <span v-html="ans.Ans_Context"></span>
                        </div>
                        <button type="primary" @click="ques_doing(null,ques,true)">提交答案</button>
                    </div>
                    <div  class="ans_area type3" v-if="ques.Qus_Type==3" remark="判断题">
                        <div :selected="state.ans=='true'"  @click="ques_doing(true,ques)">
                            <i>正确</i> 
                        </div>
                        <div :selected="state.ans=='false'" @click="ques_doing(false,ques)">
                            <i>错误</i> 
                        </div>
                    </div>
                    <div v-if="ques.Qus_Type==4" class="type4" remark="简答题">
                        <textarea rows="5" placeholder="这里输入文字" v-model.trim="state.ans"></textarea>
                        <button type="primary" @click="ques_doing(state.ans,ques)" :disabled="loading_score">
                            <loading star v-if="loading_score">提交中...</loading>
                            <span v-else>提交答案</span>
                        </button>
                        <span v-if="state.score!=null" class="aiscore">AI评判：{{state.score}} 分</span>
                    </div>
                    <div class="ans_area type5" v-if="ques.Qus_Type==5" remark="填空题">
                        <div v-for="(ans,i) in ques.Qus_Items">                   
                            <input type="text" v-model="ans.answer"></input>                
                        </div>
                        <button type="primary" @click="ques_doing(null,ques)">提交答案</button>
                    </div>    
                </card-content>
            </card>
            <div v-show="mode==1 || (mode==0 && (state.ans && state.ans!=''))">
                <card class="answer">   
                    <card-title><icon>&#xe816</icon> 正确答案</card-title>
                    <card-content v-html="sucessAnswer(ques)"></card-content>
                </card>
                <card class="explain">   
                    <card-title>
                        <span><icon>&#xe85a</icon> 试题解析</span>                      
                        <div class="ai_btn" v-if="ques.Qus_Explain=='' && !loading_ai" @click="getAiexplain">
                           AI解析
                        </div>
                    </card-title>
                    <card-content>
                        <span v-if="ques.Qus_Explain!=''" v-html="ques.Qus_Explain"></span>
                        <span v-else-if="ai_show==false">（无解析），请尝试“AI解析”</span> 
                        <div class="ai_panel" v-if="ai_show">
                            <template  v-if="loading_ai">
                                <loading star>正在加载中...</loading>
                            </template>
                            <template v-else> 
                                <h1>以下是AI解析:</h1>
                                <div v-html="ques.AI_Explain"></div>
                            </template>
                        </div>
                    </card-content>
                </card>
                <card class="knowledge" v-if="existknl" >   
                    <card-title><icon>&#xe6b0</icon> 相关知识点</card-title>
                    <card-content>
                        <div>{{knowledge.Kn_Title}}</div>
                        <div v-html="knowledge.Kn_Details"></div>
                    </card-content>
                </card>
            </div>
        </section>
       
    </template>
</dd>`
});