//试题的展示,用于回顾
$dom.load.css(['/Utilities/Components/Question/Styles/review.css']);
Vue.component('question', {
    //groupindex:试题题型的分组，用于排序号
    props: ['qans', 'index', 'state', 'groupindex', 'questions', 'org'],
    data: function () {
        return {
            ques: {},       //试题        
            ismobi: $api.ismobi(),       //是否是手机端      
            loading: false
        }
    },
    watch: {
        'ques': {
            handler(nv, ov) {
                if (!this.existques) return;
                this.$nextTick(function () {
                    var dom = $dom("card[qid='" + this.qans.id + "']");
                    //清理空元素                
                    window.ques.clearempty(dom.find('card-title'));
                    window.ques.clearempty(dom.find('.ans_area'));
                    //公式渲染
                    this.$mathjax([dom[0]]);
                });
            },
            immediate: true
        },
    },
    computed: {
        //是否试题加载完成
        existques: function () {
            return JSON.stringify(this.ques) != '{}' && this.ques != null;
        }
    },
    mounted: function () {
        var th = this;
        th.loading = true;
        $api.cache('Question/ForID:60', { 'id': this.qans.id }).then(function (req) {
            th.loading = false;
            if (req.data.success) {
                th.ques = req.data.result;
                if (th.ques.Qus_Type == 1 || th.ques.Qus_Type == 2 || th.ques.Qus_Type == 5)
                    th.ques = th.parseAnswer(th.ques);
            } else {
                console.error(req.data.exception);
                throw req.data.message;
            }
        }).catch(function (err) {
            th.loading = false;
            console.error(err);
        });
    },
    methods: {
        //是否显示试题，根据选项卡状态
        showQues: function () {
            if (this.state == "success") {
                return this.qans.success;
            }
            if (this.state == "error") {
                return !this.qans.success;
            }
            if (this.state == "unasnwered") {
                return this.qans.ans == '';
            }
            return true;
        },
        //计算序号，整个试卷采用一个序号，跨题型排序
        calcIndex: function (index) {
            var gindex = this.groupindex - 1;
            var initIndex = 0;
            while (gindex >= 0) {
                initIndex += this.questions[gindex].ques.length;
                gindex--;
            };
            return initIndex + index;
        },
        //将试题对象中的Qus_Items，解析为json
        parseAnswer: function (ques) {
            var xml = $api.loadxml(ques.Qus_Items);
            if ($api.getType(xml) == "Array") {
                ques.Qus_Items = xml;
                return ques;
            }
            //学员答题信息
            var answer = this.qans.ans == null ? [] : this.qans.ans.split(',');
            var arr = [];
            var items = xml.getElementsByTagName("item");
            for (var i = 0; i < items.length; i++) {
                var item = $dom(items[i]);
                var ansid = Number(item.find("Ans_ID").html());
                var uid = item.find("Qus_UID").text();
                var context = item.find("Ans_Context").text();
                var correct = item.find("Ans_IsCorrect").text() == "True";
                //学员是否选择某个选项
                var selected = false;
                for (var s in answer) {
                    if (answer[s] == '') continue;
                    if (ansid == Number(answer[s])) {
                        selected = true;
                        break;
                    }
                }
                arr.push({
                    "Ans_ID": ansid,
                    "Qus_ID": ques.Qus_ID,
                    "Qus_UID": uid,
                    "Ans_Context": context,
                    "Ans_IsCorrect": correct,
                    "selected": selected
                });
            }
            ques.Qus_Items = arr;
            return ques;
        },
        //选项的序号转字母
        toletter: function (index) {
            return String.fromCharCode(65 + index);
        },
        //正确答案
        sucessAnswer: function () {
            if (this.ques.Qus_Type == 1 || this.ques.Qus_Type == 2) {
                var ansstr = '';
                for (var j = 0; j < this.ques.Qus_Items.length; j++) {
                    if (this.ques.Qus_Items[j].Ans_IsCorrect) {
                        ansstr += this.toletter(j) + "、";
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
                    ansStr.push((i + 1) + "." + this.ques.Qus_Items[i].Ans_Context);
                return ansStr.join("；");
            }
        },
        //实际答题
        actualAnswer: function () {
            if (this.qans.ans == '') return '未做';
            if (this.ques.Qus_Type == 1 || this.ques.Qus_Type == 2) {
                //学员答题信息
                var answer = this.qans.ans.split(',');
                var ansstr = '';
                for (var j = 0; j < this.ques.Qus_Items.length; j++) {
                    var ishav = false;
                    for (var i = 0; i < answer.length; i++) {
                        if (answer[i] == '') continue;
                        if (answer[i] == this.ques.Qus_Items[j].Ans_ID) {
                            ansstr += this.toletter(j) + "、";
                        }
                    }
                }
                if (ansstr.indexOf("、") > -1)
                    ansstr = ansstr.substring(0, ansstr.length - 1);
                return ansstr;
            }
            if (this.ques.Qus_Type == 3) {
                return this.qans.ans == '0' ? "正确" : "错误";
            }
            if (this.ques.Qus_Type == 4) {
                return this.qans.ans;
            }
            if (this.ques.Qus_Type == 5) {
                return this.qans.ans;
            }
        }
    },
    template: `<card :qid="qans.id" v-if="showQues()">
        <card-title :index="calcIndex(index+1)" v-if="loading">
            <loading type="spinner" size="24px" > 加载中...</loading>
        </card-title>
        <card-title :index="calcIndex(index+1)" v-else-if="ques.Qus_Title" v-html="ques.Qus_Title"></card-title>
        <card-title :index="calcIndex(index+1)" v-else><span class="null">(试题不存在)</span></card-title>
        <card-context>
        <div class="ans_area type1" v-if="ques.Qus_Type==1">
            <div v-for="(ans,i) in ques.Qus_Items" :correct="ans.Ans_IsCorrect" :selected="ans.selected">
                <i>{{toletter(i)}} .</i>
                <span v-html="ans.Ans_Context"></span>
             </div>
        </div>
        <div  class="ans_area type2" v-if="ques.Qus_Type==2">
            <div v-for="(ans,i) in ques.Qus_Items" :correct="ans.Ans_IsCorrect" :selected="ans.selected">
                <i>{{toletter(i)}} .</i>
                <span v-html="ans.Ans_Context"></span>
            </div>
        </div>
        <div  class="ans_area type3" v-if="ques.Qus_Type==3">
            <div :correct="ques.Qus_IsCorrect" :selected="qans.ans=='0'">
                <i>正确</i> 
            </div>
            <div :correct="!ques.Qus_IsCorrect" :selected="qans.ans=='1'">
                <i>错误</i>
            </div>
        </div>
        <div v-if="ques.Qus_Type==4">
        
        </div>
        <div v-if="ques.Qus_Type==5">
        
        </div>
        <div class="resultBox">
            <div :mobi="ismobi">
                <div>正确答案：<span v-html="sucessAnswer()"></span></div>
                <div>实际答题：<span v-html="actualAnswer()"></span></div>
                <div class="result_score" :success="qans.success">得分：{{qans.score}} 分</div>
            </div>
            <div v-if="ques.Qus_Explain!=''">试题解析：<span v-if="ques.Qus_Explain!=''" v-html="ques.Qus_Explain"></span>
                <span v-else>无</span>
            </div>
        </div>
        </card-context>
        <div class="orgname noview" v-if="org">{{org.Org_Name}}</div>
      </card>`
});