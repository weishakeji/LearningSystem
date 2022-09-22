$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            tpid: $api.querystring('tpid', 0),      //试卷id
            couid: $api.querystring('couid', 0),

            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项        

            purchase: {},        //课程购买的记录           
            results: [],        //测试成绩

            paper: {},           //试卷信息           
            types: [],              //试题类型 
            paperQues: [],           //试卷内容（即试题信息）
            paperAnswer: {},          //答题信息
            paperAnswerXml: '',          //答题信息的xml格式数据
            //++一些状态信息
            swipeIndex: 0,           //试题滑动时的索引，用于记录当前显示的试题索引号
            showCard: false,          //是否显示答题卡           

            submitState: {
                show: false,       //成绩提交的面板提示
                loading: false,         //考试成绩提交中
                result: {},
                submited: false          //是否交卷
            },
            time: {
                now: new Date().getTime(),             //当前时间的毫秒数            
                client: {},            //客户端时间
                span: 0,            //考试时长（单位分钟）  
                begin: new Date(), //开始考试时间          
                over: new Date(),    //考试结束时间 
            },
            result: {},                  //答题成绩信息
            //加载中的状态
            loading: {
                init: true,             //初始化主要参数           
                submit: false,           //成绩提交中
                paper: false             //试卷生成中
            },
            //成绩得分
            resultTotal: 0
        },
        mounted: function () {

        },
        created: function () { },
        computed: {
            //学员是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            },
            //是否过期，过期返回true
            isoverdue: function () {
                var isbuy = JSON.stringify(this.purchase) != '{}' && this.purchase != null;
                if (!isbuy) return true;
                if (this.purchase.Stc_IsFree) return false;
                if (this.purchase.Stc_EndTime > new Date()) return false;
                return true;
            },
            //是否可以进行模拟测试
            istest: function () {

            },
            //试题总数
            questotal: function () {
                var total = 0;
                for (var i = 0; i < this.paperQues.length; i++) {
                    total += this.paperQues[i].count;
                }
                return total;
            },
            //已经做的题数
            answertotal: function () {
                if (!this.paperAnswer.ques) return 0;
                var total = 0;
                for (var i = 0; i < this.paperAnswer.ques.length; i++) {
                    for (let j = 0; j < this.paperAnswer.ques[i].q.length; j++) {
                        const q = this.paperAnswer.ques[i].q[j];
                        if (q.ans != '') total++;
                    }
                }
                return total;
            },
            //考试剩余时间
            surplustime: function () {
                var surplus = Math.floor((this.time.over.getTime() - this.time.now) / 1000 - 1);
                if (this.submitState.submited) return 0;
                return surplus > 0 ? surplus : 0;
            }
        },
        watch: {
            //当学员登录后
            'account': {
                handler: function (nv, ov) {
                    this.initialize();
                },
                immediate: true
            },
            //试卷内容，试题列表，按题型分开
            'paperQues': {
                handler(nv, ov) {
                    //if (JSON.stringify(nv) == JSON.stringify(ov)) return;                   
                    //生成答题信息（Json格式）
                    this.paperAnswer = this.generateAnswerJson(nv);
                },
                deep: true
            },
            //答题信息变更时
            'paperAnswer': {
                handler(nv, ov) {
                    //生成xml，用于提交到数据库
                    this.paperAnswerXml = this.generateAnswerXml(nv);
                    //console.log(this.paperAnswerXml);
                    //计算成绩
                    this.calcReslutScore();
                },
                deep: true
            },
            //剩余时间
            'surplustime': function (nv, ov) {
                if (nv <= 0) {
                    console.log('交卷');
                    this.submit(1);
                }
                //console.log(nv);
            },
            //滑动试题，滑动到指定试题索引
            'swipeIndex': {
                handler: function (nv, ov) {
                    console.log(nv);
                    if (nv > this.questotal || nv < 0) return;
                    var qeus_width = 800;
                    window.setTimeout(function () {
                        $dom("section[remark]").css('left', -(qeus_width * nv) + 'px');
                    }, 100);

                }, immediate: true
            }
        },
        methods: {
            //初始化
            initialize: function () {
                var th = this;
                $api.bat(
                    $api.cache('Question/Types:9999'),
                    $api.get('TestPaper/ForID', { 'id': this.tpid })
                ).then(axios.spread(function (type, paper) {
                    vapp.loading.init = false;
                    //判断结果是否正常
                    for (var i = 0; i < arguments.length; i++) {
                        if (arguments[i].status != 200)
                            console.error(arguments[i]);
                        var data = arguments[i].data;
                        if (!data.success && data.exception != null) {
                            console.error(data.message);
                        }
                    }
                    //试题类型
                    th.types = type.data.result;
                    //试卷
                    th.paper = paper.data.result;
                    //获取购买记录
                    if (th.islogin) {
                        th.getpurchase(th.account.Ac_ID, th.paper.Cou_ID);
                        th.getresults(th.account.Ac_ID);
                    }
                    if (!th.final_disable())
                        th.generatePaper();
                })).catch(function (err) {
                    console.error(err);
                });
            },
            //获取购买课程的记录
            getpurchase: function (stid, couid) {
                var th = this;
                if (th.couid <= 0 || stid <= 0) return;
                th.loading = true;
                $api.get('Course/Purchaselog:5', { 'stid': stid, 'couid': couid }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.purchase = req.data.result;
                        console.log(th.purchase);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            //获取历史成绩
            getresults: function (stid) {
                var th = this;
                $api.get('TestPaper/ResultsAll', { 'stid': stid, 'tpid': th.tpid }).then(function (req) {
                    th.loading_result = false;
                    if (req.data.success) {
                        th.results = req.data.result;
                        console.log(th.results);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }

                }).catch(function (err) {
                    th.loading_result = false;
                    th.results = [];
                    console.error(err);
                });
            },
            //结果考试的按钮是否通过,为true时表示不通过
            final_disable: function () {
                if (!this.paper.Tp_IsFinal) return false;
                var final_condition = this.$refs["final_condition"];
                return final_condition.final_disable();
            },
            //生成试卷内容
            generatePaper: function () {
                if (JSON.stringify(this.paper) == '{}' && this.paper == null) return;
                if (this.paperQues.length > 0) return;
                var th = this;
                th.loading.paper = true;
                $api.get('TestPaper/GenerateRandom', { 'tpid': this.tpid }).then(function (req) {
                    window.setTimeout(function () {
                        th.loading.paper = false;
                    }, 1000);
                    if (req.data.success) {
                        var paper = req.data.result;
                        th.paperQues = paper;
                        window.setInterval(function () {
                            vapp.time.now = new Date().getTime();
                            //var surplus = Math.floor((vapp.time.over.getTime() - vapp.time.now) / 1000 - 1);
                            //vapp.surplustime = surplus > 0 ? surplus : 0;
                        }, 1000);
                        th.$nextTick(function () {
                            vapp.calcTime();
                        });

                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //跳转到试卷页
            goback: function () {
                var file = "/web/test/paper";
                var url = $api.url.dot(this.tpid, file);
                window.location.href = url;
            },
            //计算时间，参数：初始时间、考试时长
            calcTime: function () {
                this.time.client = new Date();
                this.time.span = vapp.paper.Tp_Span;
                this.time.begin = new Date();
                this.time.over = new Date(this.time.begin.getTime() + this.time.span * 60 * 1000);

            },
            //交卷
            // patter:交卷方式，1为自动提交，2为交卷
            submit: function (patter) {
                if (JSON.stringify(this.paperAnswer) == '{}') return;
                if (this.submitState.submited) return;
                this.submitState.show = true;
                this.submitState.loading = true;
                this.submitState.submited = true;
                //设置为交卷
                this.paperAnswer.patter = patter;
                this.paperAnswer.score = this.calcReslutScore();
                var xml = this.generateAnswerXml(this.paperAnswer);
                //提交答题信息，async为异步，成绩计算在后台执行
                var th = this;
                $api.put('TestPaper/InResult', { 'result': xml }).then(function (req) {
                    vapp.submitState.loading = false;
                    if (req.data.success) {
                        th.submitState.result = req.data.result;
                        console.log('成绩递交成功');
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //手动交卷
            submitManual: function () {
                //没有答的题数
                var surplus = this.questotal - this.answertotal;
                var msg = '';
                if (surplus <= 0) {
                    msg = "当前考试" + this.questotal + "道题，您已经全部做完！";
                } else {
                    msg = "当前考试" + this.questotal + "道题，您还有" + surplus + " 没有做！";
                }
                var th = this;
                this.$confirm(msg + '<br/>是否确认交卷？', '交卷', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    dangerouslyUseHTMLString: true,
                    type: 'warning'
                }).then(() => {
                    th.submit(2);
                }).catch(() => { });

            },
            //计算成绩得分
            calcReslutScore: function () {
                var answer = this.paperAnswer;
                if (!answer.ques) return 0;
                var total = 0;
                for (var i = 0; i < answer.ques.length; i++) {
                    for (let j = 0; j < answer.ques[i].q.length; j++) {
                        const q = answer.ques[i].q[j];
                        if (q.sucess) total += q.score;
                    }
                }
                return total;               
            },
            //试题向右滑动 
            swiperight: function (e) {
                if (e && e.preventDefault) e.preventDefault();
                if (this.swipeIndex > 0) this.swipeIndex--;
            },
            //试题向左滑动
            swipeleft: function (e) {
                if (e && e.preventDefault) e.preventDefault();
                if (this.swipeIndex < this.questotal - 1) this.swipeIndex++;
            },
            //生成答题信息
            generateAnswerJson: function (paper) {
                var results = {
                    "examid": 0,
                    "stid": this.account.Ac_ID,
                    "stname": this.account.Ac_Name,
                    "stsex": this.account.Ac_Sex,
                    "stcardid": this.account.Ac_IDCardNumber,
                    "stsid": this.account.Sts_ID,
                    "stsname": this.account.Sts_Name,
                    //课程
                    "couid": this.paper.Cou_ID,                    
                    //试卷
                    "tpid": this.paper.Tp_Id,
                    "tpname": this.paper.Tp_Name,
                    //学科
                    "sbjid": this.paper.Sbj_ID,
                    "sbjname": this.paper.Sbj_Name,
                    //考试开始时间与结束时间
                    "begin": this.time.begin.getTime(),
                    "overtime": new Date().getTime(),

                    "patter": 1,    //提交方式，1为自动提交，2为交卷
                    "score": this.calcReslutScore(),
                    "isclac": true,
                    "ques": []
                }
                //实际答题内容
                var questionAnswer = function (ques) {
                    if (ques.Qus_Type == 1 || ques.Qus_Type == 2) {
                        var ansstr = '';
                        for (var j = 0; j < ques.Qus_Items.length; j++) {
                            if (ques.Qus_Items[j].selected)
                                ansstr += ques.Qus_Items[j].Ans_ID + ",";
                        }
                        if (ansstr.indexOf(",") > -1)
                            ansstr = ansstr.substring(0, ansstr.length - 1);
                        return ansstr;
                    }
                    if (ques.Qus_Type == 3) {
                        if (ques.Qus_Answer == '') return '';
                        return ques.Qus_Answer == "true" ? '0' : '1';
                    }
                    if (ques.Qus_Type == 4 || ques.Qus_Type == 5)
                        return ques.Qus_Answer;
                };
                //记录答题信息
                for (let i = 0; i < paper.length; i++) {
                    const group = paper[i];
                    var ques = {
                        "type": group.type,
                        "count": group.count,
                        "number": group.number,
                        "q": []
                    }
                    for (let j = 0; j < group.ques.length; j++) {
                        const qus = group.ques[j];
                        ques.q.push({
                            "id": qus.Qus_ID,
                            "num": qus.Qus_Number,
                            "ans": questionAnswer(qus),
                            'sucess': qus.state ? qus.state['sucess'] : false,
                            'score': qus.state ? qus.state['score'] : 0
                        });
                    }
                    results.ques.push(ques);
                }
                return results;
            },
            //生成答题的状态记录
            generateAnswerXml: function (quesAnswer) {
                var results = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
                results += "<results ";
                //生成主参数
                var queslist = {};
                for (var att in quesAnswer) {
                    if (att == 'ques') {
                        queslist = quesAnswer[att];
                        continue;
                    }
                    results += att + '="' + quesAnswer[att] + '" ';
                }
                results += ">";
                //生成试题
                for (var i = 0; i < queslist.length; i++) {
                    var quesgroup = queslist[i];
                    var ques = "<ques ";
                    var q = [];
                    for (var att in quesgroup) {
                        if (att == 'q') {
                            q = quesgroup[att];
                            continue;
                        }
                        ques += att + '="' + quesgroup[att] + '" ';
                    }
                    ques += ">";
                    for (var j = 0; j < q.length; j++) {
                        ques += '<q ';
                        for (var att in q[j]) {
                            if ((quesgroup.type == 4 || quesgroup.type == 5) && att == "ans") continue;
                            ques += att + '="' + q[j][att] + '" ';
                        }
                        ques += ">";
                        if (quesgroup.type == 4 || quesgroup.type == 5) {
                            ques += "<![CDATA[" + q[j]['ans'] + "]]>"
                        }
                        ques += "</q>";
                    }
                    ques += "</ques> ";
                    results += ques;
                }
                results += "</results> ";
                //console.log(results);
                return results
            },
            //进入回顾
            goreview: function () {
                var file = "/web/test/paper";
                var url = $api.url.dot(this.tpid, file);
                window.location.href = url;
            },
        },
        filters: {
            //考试剩余时间的格式
            'surplus': function (value) {
                var mm = parseInt(value / 60);
                var ss = parseInt(value % 60);
                return mm + ":" + ss;
            }
        }
    });
}, ['Components/Question.js',
    'Components/Quesbuttons.js',
    'Components/AnswerCard.js',
    "Components/final_condition.js"]);
