$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //考试id和成绩id
            tpid: $api.querystring('tp', 0),
            trid: $api.querystring('tr', 0),
            account: {},     //当前登录账号
            student: {},     //当前参考的学员，有可能不是当前学员        
            result: {},         //考试成绩的籹据实体对象
            exrxml: {},          //答题信息，xml
            paper: {},           //试卷信息
            config: {},      //当前机构配置项        
            types: {},          //题型

            scoreFinal: 0,       //考试得分

            tabactive: '',      //选项卡的状态
            error: '',           //错误提示信息，例如不能查看考虑成绩时
            loading: false
        },
        mounted: function () {
            window.addEventListener('scroll', this.handleScroll, true);
            this.loading = true;
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Question/Types:9999'),
                $api.cache('TestPaper/ForID', { 'id': this.tpid }),
                $api.get('TestPaper/ResultForID', { 'id': this.trid }),
            ).then(axios.spread(function (account, types, paper,result) {
                vapp.loading = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                    }
                }
                //获取结果
                vapp.account = account.data.result;
                vapp.types = types.data.result;
                vapp.paper = paper.data.result;
                vapp.result = result.data.result;
                vapp.exrxml = $api.loadxml(vapp.result.Tr_Results);
              
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            },
            //试卷中的答题信息
            //返回结构：先按试题分类，分类下是答题信息
            questions: function () {
                var exrxml = this.exrxml;
                var arr = [];
                if (JSON.stringify(exrxml) === '{}') return arr;
                var elements = exrxml.getElementsByTagName("ques");
                for (var i = 0; i < elements.length; i++) {
                    var gruop = $dom(elements[i]);
                    //题型,题量，总分
                    var type = Number(gruop.attr('type'));
                    var count = Number(gruop.attr('count'));
                    var number = Number(gruop.attr('number'));
                    //试题
                    var qarr = [];
                    var list = gruop.find('q');
                    for (var j = 0; j < list.length; j++) {
                        var q = $dom(list[j]);
                        var qid = Number(q.attr('id'));
                        var ans = q.attr('ans');
                        var sucess = q.attr('sucess') == 'true';
                        var score = Number(q.attr('score'));
                        qarr.push({
                            'id': qid, 'type': type,
                            'ans': ans, 'success': sucess, 'score': score
                        });
                    }
                    arr.push({
                        'type': type, 'count': count, 'number': number, 'ques': qarr
                    });
                }
                return arr;
            },
            //总题数
            ques_all_count: function () {
                var count = 0;
                for (var i = 0; i < this.questions.length; i++)
                    count += this.questions[i].ques.length;
                return count;
            },
            //答对的题数
            ques_success_count: function () {
                var count = 0;
                for (var i = 0; i < this.questions.length; i++) {
                    var ques = this.questions[i].ques;
                    for (var j = 0; j < ques.length; j++) {
                        if (ques[j].success) count++;
                    }
                }
                return count;
            },
            //答错的题数
            ques_error_count: function () {
                var count = 0;
                for (var i = 0; i < this.questions.length; i++) {
                    var ques = this.questions[i].ques;
                    for (var j = 0; j < ques.length; j++) {
                        if (!ques[j].success) count++;
                    }
                }
                return count;
            },
            //未做的题数
            ques_unanswerd_count: function () {
                var count = 0;
                for (var i = 0; i < this.questions.length; i++) {
                    var ques = this.questions[i].ques;
                    for (var j = 0; j < ques.length; j++) {
                        if (ques[j].ans == '') count++;
                    }
                }
                return count;
            },
        },
        watch: {
            'tabactive': function (nv, ov) {
                //console.log(nv);
            }
        },
        methods: {
            //得分样式
            scoreStyle: function (score) {
                //总分和及格分
                var total = this.paper.Tp_Total;
                var passscore = this.paper.Tp_PassScore;
                if (score == total) return "praise";
                if (score < passscore) return "nopass";
                if (score < total * 0.8) return "general";
                if (score >= total * 0.8) return "fine";
                return "";
            }
        }
    });
    //试题大项分类
    Vue.component('group', {
        //item,试题分类项
        //state:状态，如正确、错误、未做； 
        props: ['item', 'index', 'types', 'state'],
        data: function () {
            return {
                loading: false
            }
        },
        watch: {
            'state': function (nv, ov) {
                //console.log(nv);
            }
        },
        computed: {},
        mounted: function () {
        },
        methods: {
            //显示分类的信息栏
            showIndex: function () {
                let changeNum = ['零', '一', '二', '三', '四', '五', '六', '七', '八', '九'];
                return changeNum[this.index + 1];
            },
            //显示题型
            showType: function () {
                return this.types[this.item.type - 1];
            },
            //计算得分
            score: function () {
                var num = 0;
                for (var i = 0; i < this.item.ques.length; i++) {
                    var q = this.item.ques[i];
                    if (q.success) num += q.score;
                }
                return Math.floor(num * 100) / 100;
            }
        },
        template: `<div> 
        <div class="type_title">{{showIndex()}}、 {{showType()}}   
            <div class="type_info">               
                <el-tag type="warning">{{item.count}}道题，共{{item.number}}分</el-tag>    
                <el-tag type="success">得分{{score()}}</el-tag>           
            </div>
        </div>
        <slot></slot>
        </div>`
    });
    //试题的展示
    Vue.component('question', {
        //groupindex:试题题型的分组，用于排序号
        props: ['qans', 'index', 'state', 'groupindex', 'questions'],
        data: function () {
            return {
                ques: {},       //试题              
                loading: false
            }
        },
        watch: {},
        computed: {},
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
            showIndex: function (index) {
                return String.fromCharCode(65 + index);
            },
            //正确答案
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
                        ansStr.push((i + 1) + "." + this.ques.Qus_Items[i].Ans_Context);
                    return ansStr.join("；");
                }
            },
            //实际答题
            actualAnswer: function () {
                if (this.ques.Qus_Type == 1 || this.ques.Qus_Type == 2) {
                    //学员答题信息
                    var answer = this.qans.ans.split(',');
                    var ansstr = '';
                    for (var j = 0; j < this.ques.Qus_Items.length; j++) {
                        var ishav = false;
                        for (var i = 0; i < answer.length; i++) {
                            if (answer[i] == '') continue;
                            if (answer[i] == this.ques.Qus_Items[j].Ans_ID) {
                                ansstr += this.showIndex(j) + "、";
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
                <i></i>{{showIndex(i)}} .
                <span v-html="ans.Ans_Context"></span>
             </div>
        </div>
        <div  class="ans_area type2" v-if="ques.Qus_Type==2">
            <div v-for="(ans,i) in ques.Qus_Items" :correct="ans.Ans_IsCorrect" :selected="ans.selected">
                <i></i>{{showIndex(i)}} .
                <span v-html="ans.Ans_Context"></span>
            </div>
        </div>
        <div  class="ans_area type2" v-if="ques.Qus_Type==3">
            <div :correct="ques.Qus_IsCorrect" :selected="qans.ans=='0'">
                <i></i> 正确
            </div>
            <div :correct="!ques.Qus_IsCorrect" :selected="qans.ans=='1'">
                <i></i> 错误
            </div>
        </div>
        <div v-if="ques.Qus_Type==4">
        
        </div>
        <div v-if="ques.Qus_Type==5">
        
        </div>
        <div class="resultBox">
            <div>正确答案：<span v-html="sucessAnswer()"></span></div>
            <div>实际答题：<span v-html="actualAnswer()"></span></div>
            <div class="result_score" :success="qans.success">答题得分：{{qans.score}} 分</div>
            <div>试题解析：<span v-if="ques.Qus_Explain!=''" v-html="ques.Qus_Explain"></span>
                <span v-else>无</span>
            </div>
        </div>
        </card-context>
      </card>`
    });
});
