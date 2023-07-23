$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            examid: $api.querystring('id', 0),
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项   

            theme: {},               //考试主题
            exam: {},             //考试 
            examstate: {
                islogin: true
            },           //考试状态
            paper: {},           //试卷信息   
            subject: {},             //试卷所属专业
            types: [],              //试题类型 
            paperQues: [],           //试卷内容（即试题信息）
            paperAnswer: {},          //答题信息
            paperAnswerXml: '',          //答题信息的xml格式数据
            //++一些状态信息
            swipeIndex: 0,           //试题滑动时的索引，用于记录当前显示的试题索引号
            showCard: false,          //是否显示答题卡   
            showTime: false,             //显示时间信息
            showExam: false,             //显示考试信息

            submitState: {
                show: false,       //成绩提交的面板提示
                loading: false,         //考试成绩提交中
                result: {}
            },
            time: {
                now: new Date().getTime(),             //当前时间的毫秒数              
                server: {},          //服务器端时间
                client: {},            //客户端时间
                span: 0,            //考试时长（单位分钟）
                wait: 0,           //离开始考试还有多久，单位秒
                begin: new Date(),  //考试开始时间,如果固定时间考试，此时间来自系统设置
                over: new Date(),    //考试结束时间               
                start: new Date(),        //学员真正开始考试的时间，例如9:00 begin开始，学员9:10进场
                requestlimit: 10,    //离开考多久的时候，开始预加载试题，单位：分钟
            },
            //考试中的状态
            state: {
                loading: false, //是否正在加载试卷               
            },
            result: {},                  //答题成绩信息
            //加载中的状态
            loading: {
                init: false,             //初始化主要参数
                exam: true,               //加载考试信息中
                paper: false,             //试卷加载中
                ques: true,              //加载试题中
                submit: false,           //成绩提交中

            }
        },
        mounted: function () {
            var th = this;
            //初始化
            th.loading.init = true;
            $api.bat(
                $api.cache('Question/Types:9999'),
                $api.post('Platform/ServerTime')
            ).then(axios.spread(function (type, time) {
                th.types = type.data.result;        //试题类型
                //时间信息
                th.time.server = eval('new ' + eval('/Date(' + time.data.result + ')/').source);
                th.time.client = new Date();
                window.setInterval(function () {
                    th.time.now = new Date().getTime();
                    th.paperAnswer.now = th.nowtime.getTime();
                }, 1000);
            })).catch(err => console.error(err))
                .finally(() => th.loading.init = false);
        },
        created: function () { },
        computed: {
            //是否登录
            islogin: (t) => { return !$api.isnull(t.account); },
            //试题总数
            questotal: function () {
                let total = 0;
                for (var i = 0; i < this.paperQues.length; i++)
                    total += this.paperQues[i].count;
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
            //当前时间，经过服务器时间校正过的
            nowtime: function () {
                try {
                    var curr = new Date(this.time.server.getTime() + (this.time.now - this.time.client.getTime()));
                    return curr;
                } catch { }
            },
            //考试剩余时间
            surplustime: function () {
                var surplus = Math.floor((this.time.over.getTime() - this.time.now) / 1000);
                return surplus > 0 ? surplus : 0;
                return surplus;
            },
            //考试开始时间
            starttime: function () {
                try {
                    var curr = new Date(this.examstate.startTime);
                    return curr;
                } catch { }
            },
            //离开始考试还有多少时间
            howtime: function () {
                var how = this.starttime.getTime() - this.nowtime.getTime();
                this.time.wait = how;
                if (how <= 0) return '';
                how = Math.floor(how / 1000);
                var mm = Math.floor(how / 60);
                var ss = how - mm * 60;
                var hh = Math.floor(mm / 60);
                if (hh > 0) mm = mm - hh * 60;
                if (hh > 0) {
                    return hh + "小时 " + mm + "分 " + ss + "秒";
                } else {
                    return mm + "分 " + ss + "秒";
                }
            },
            //答题记录存储时的名称
            recordname: function () {
                var acid = this.account.Ac_ID;
                var examid = this.exam.Exam_ID;
                return "exam_answer_record:[stid=" + acid + "],[examid=" + examid + "]";
            },
            
        },
        watch: {
            //是否登录的状态变化，如果登录，则加载考试信息
            'islogin': function (nv, ov) {
                if (!nv) return;    //如果未登录，则退出
                var th = this;
                th.loading.exam = true;
                $api.bat(
                    $api.get('Exam/State', { 'examid': th.examid }),
                    $api.get('Exam/ForID', { 'id': th.examid })
                ).then(axios.spread(function (state, exam) {
                    th.examstate = state.data.result;
                    console.error(th.examstate);
                    th.exam = exam.data.result;
                })).catch(err => console.error(err))
                    .finally(() => th.loading.exam = false);
            },
            //考试对象加载后，加载试卷
            'exam': function (nv, ov) {
                if ($api.isnull(nv)) return;
                var th = this;
                th.time.span = nv.Exam_Span;
                th.loading.paper = true;
                $api.bat(
                    $api.cache('Exam/ThemeForUID', { 'uid': th.examstate.uid }),
                    $api.cache('Subject/ForID', { 'id': th.examstate.subject }),
                    $api.get('TestPaper/ForID', { 'id': th.examstate.paper }),
                    $api.get('Exam/Result', { 'examid': th.examid, 'tpid': th.examstate.paper, 'stid': th.account.Ac_ID })
                ).then(axios.spread(function (theme, sbj, paper, result) {
                    th.theme = theme.data.result;
                    th.subject = sbj.data.result;
                    th.paper = paper.data.result;
                    //是否已经交过卷
                    th.result = result.data.result;
                    //生成试卷
                    if (th.result == null || !th.result.Exr_IsSubmit)
                        th.generatePaper();
                })).catch(err => console.error(err))
                    .finally(() => th.loading.paper = false);
            },
            //当前时间
            'nowtime': function (nv, ov) {
                //离考试还有多久
                this.time.wait = this.starttime.getTime() - this.nowtime.getTime();
                this.time.wait = this.time.wait <= 0 ? 0 : Math.floor(this.time.wait / 1000);
                if (!this.examstate.isover) {
                    if (this.time.wait < this.time.requestlimit * 60 && JSON.stringify(this.exam) != '{}' && !this.loading.ques) {
                        this.loading.ques = true;
                        this.generatePaper();
                    }
                    if (this.time.wait == 0 && !this.examstate.issubmit) {
                        this.examstate.doing = true;
                    }
                }
            },
            //考试剩余时间
            'surplustime': {
                handler: function (nv, ov) {
                    if (nv <= 0) {
                        var th = this;
                        window.setTimeout(function () {
                            if (th.examstate.isover) return;
                            if (th.surplustime == 0 && !th.examstate.issubmit) {
                                th.submit(2);
                            }
                        }, 2000);
                    }
                }, immediate: true
            },
            'paperQues': {
                handler: function (nv, ov) {
                    //if (JSON.stringify(nv) == JSON.stringify(ov)) return;         
                    if ($api.isnull(this.exam) || $api.isnull(this.paper)) return;
                    //生成答题信息（Json格式）
                    this.paperAnswer = this.generateAnswerJson(nv);
                }, deep: true
            },
            //答题信息变更时
            'paperAnswer': {
                handler: function (nv, ov) {
                    //记录到本地
                    if (!this.examstate.issubmit)
                        $api.storage(this.recordname, nv);
                    //生成xml，用于提交到数据库
                    this.paperAnswerXml = this.generateAnswerXml(nv);
                }, deep: true
            }
        },
        methods: {
            //生成试卷内容
            generatePaper: function () {
                if ($api.isnull(this.paper)) return;
                if (this.paperQues.length > 0) return;
                var th = this;
                th.loading.ques = true;
                //试卷缓存过期时间
                var span = th.exam.Exam_Span;
                $api.cache('Exam/MakeoutPaper:+' + span * 2,
                    { 'examid': th.exam.Exam_ID, 'tpid': th.paper.Tp_Id, 'stid': th.account.Ac_ID })
                    .then(function (req) {
                        window.setTimeout(function () {
                            th.loading.ques = false;
                            th.submit(1);
                        }, 1000);
                        if (req.data.success) {
                            var paper = req.data.result;
                            //将试题对象中的Qus_Items，解析为json
                            for (let i = 0; i < paper.length; i++) {
                                const group = paper[i];
                                for (let key in group) {
                                    if (key == 'ques') {
                                        for (let j = 0; j < group.ques.length; j++) {
                                            group.ques[j] = window.ques.parseAnswer(group.ques[j]);
                                            if (group.ques[j].Qus_Type == 5) {
                                                for (let b = 0; b < group.ques[j].Qus_Items.length; b++)
                                                    group.ques[j].Qus_Items[b]["Ans_Context"] = '';
                                            }
                                        }
                                        continue;
                                    }
                                    group[key] = Number(group[key]);
                                }
                            }
                            th.calcTime();
                            //将本地记录的答题信息还原到界面
                            paper = th.restoreAnswer(paper);
                            th.paperQues = paper;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    }).finally(() => th.loading.ques = false);
            },
            //是否处于考试中
            isexaming: function () {
                if ($api.isnull(this.exam) || $api.isnull(this.paper)) return false;
                //如果已经交卷
                if ($api.isnull(this.examstate) || this.examstate.issubmit) return false;
                //如果不在考试人群中
                if (!this.examstate.allow) return false;
                //考试时间已过或还未开始
                if (this.examstate.isover || !this.examstate.isstart) return false;
                return this.examstate.doing;
            },
            //计算序号，整个试卷采用一个序号，跨题型排序
            calcIndex: function (index, groupindex) {
                var gindex = groupindex - 1;
                var initIndex = 0;
                while (gindex >= 0) {
                    initIndex += vapp.paperQues[gindex].ques.length;
                    gindex--;
                };
                return initIndex + index;
            },
            //跳转到查看成绩
            goreviwe: function () {
                var url = "Review?examid=" + this.exam.Exam_ID + "&exrid=" + this.result.Exr_ID;
                window.location.href = url;
            },
            calcTime: function () {
                //固定时间开始
                if (this.examstate.type == 1) {
                    this.time.begin = new Date(this.examstate.startTime);
                    this.time.over = new Date(this.time.begin.getTime() + this.time.span * 60 * 1000);
                    if (this.time.begin > this.nowtime) this.time.start = this.nowtime;
                    else
                        this.time.start = this.time.begin;
                }
                //限定时间段
                if (this.examstate.type == 2) {
                    this.time.begin = this.nowtime;
                    this.time.over = new Date(this.nowtime.getTime() + this.time.span * 60 * 1000);
                    this.time.start = this.nowtime;
                }
            },
            //交卷
            submit: function (patter) {
                if (!this.isexaming()) return;    //没有处于考试中，则不提交
                if ($api.isnull(this.paperAnswer)) return;
                if (this.examstate.issubmit) return;

                if (patter == null) patter = 1;
                var th = this;
                if (patter == 2) {
                    th.submitState.show = true;
                    $api.storage('exam_blur_num_' + th.examid, null);
                }
                th.submitState.loading = true;
                th.paperAnswer = th.generateAnswerJson(th.paperQues);
                //设置为交卷
                th.paperAnswer.patter = patter;
                var xml = th.generateAnswerXml(th.paperAnswer);
                //提交答题信息，async为异步，成绩计算在后台执行
                $api.put('Exam/SubmitResult', { 'xml': xml, 'async': false }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        if (patter == 2) {
                            th.submitState.result = result;
                            $api.storage(th.recordname, null);
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.submitState.loading = false);
            },
            //手动交卷
            submitManual: function () {
                //没有答的题数
                var surplus = this.questotal - this.answertotal;
                var msg = '';
                var th = this;
                if (surplus <= 0) {
                    msg = "当前考试" + this.questotal + "道题，您已经全部做完！";
                } else {
                    msg = "当前考试" + this.questotal + "道题，您还有" + surplus + " 没有做！";
                }
                th.$dialog.confirm({
                    title: '交卷',
                    message: msg + '<br/>是否确认交卷？',
                }).then(() => {
                    th.submit(2);
                }).catch(() => {
                    // on cancel
                });
            },
            //自动交卷
            submitAuto: function () {

            },
            //试题向右滑动 
            swiperight: function (e) {
                if (e && e.preventDefault) e.preventDefault();
                if (this.swipeIndex > 0) this.swipeIndex--;
                this.swipe(this.swipeIndex);
            },
            //试题向左滑动
            swipeleft: function (e) {
                if (e && e.preventDefault) e.preventDefault();
                if (this.swipeIndex < this.questotal - 1) this.swipeIndex++;
                this.swipe(this.swipeIndex);
            },
            //滑动试题，滑动到指定试题索引
            swipe: function (index) {
                this.swipeIndex = index;
                $dom("section").css('left', -($dom("section dd").width() * this.swipeIndex) + 'px');
                this.showCard = false;
            },
            //生成答题信息
            generateAnswerJson: function (paper) {
                var results = {
                    "examid": this.exam.Exam_ID,
                    "tpid": this.paper.Tp_Id,
                    //"now": this.nowtime.getTime(),
                    "begin": this.time.begin.getTime(),
                    "overtime": this.time.over.getTime(),
                    "starttime": this.time.start.getTime(),
                    "stid": this.account.Ac_ID,
                    "stname": this.account.Ac_Name,
                    "stsid": this.account.Sts_ID,
                    "stsex": this.account.Ac_Sex,
                    "stcardid": this.account.Ac_IDCardNumber,
                    "uid": this.exam.Exam_UID,
                    "theme": this.theme.Exam_Title,
                    "sbjid": this.subject.Sbj_ID,
                    "sbjname": this.subject.Sbj_Name,
                    "patter": 1,    //提交方式，1为自动提交，2为交卷
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
                            "class": "level1",
                            "num": qus.Qus_Number,
                            "ans": questionAnswer(qus)
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
                            q[j]['ans'] = q[j]['ans'].replace(/<[^>]*>/g, '');
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
            //将本地记录本的答题信息还原到试卷，用于应对学员刷新页面或重新打开试卷时
            restoreAnswer: function (paper) {
                var record = $api.storage(this.recordname);
                if (record == null || record == "") {
                    //固定时间开始
                    if (this.examstate.type == 1) {
                        this.time.begin = new Date(this.examstate.startTime);
                        this.time.over = new Date(this.time.begin.getTime() + this.time.span * 60 * 1000);
                    }
                    //限定时间段
                    if (this.examstate.type == 2) {
                        if (this.nowtime > this.examstate.startTime) {
                            this.time.begin = this.nowtime;
                            this.time.over = new Date(this.nowtime.getTime() + this.time.span * 60 * 1000);
                        } else {
                            this.time.begin = new Date(this.examstate.startTime);
                            this.time.over = new Date(this.examstate.startTime + this.time.span * 60 * 1000);
                        }
                    }
                    return paper;
                }
                console.info("此处应该做考试过期的判断，利用overtime属性");
                //开始时间与剩余时间
                var begin = new Date(record.begin);
                var over = new Date(record.overtime);
                if (vapp.nowtime > over) {
                    $api.storage(this.recordname, null);
                    return paper;
                } else {
                    vapp.time.begin = begin;
                    vapp.time.over = over;
                }
                vapp.time.start = new Date(record.starttime);
                //console.log(begin);
                this.paperAnswer = record;
                //答题记录，转成一层数组，方便遍历
                var reclist = []
                for (var i = 0; i < record.ques.length; i++) {
                    for (let j = 0; j < record.ques[i].q.length; j++) {
                        const q = record.ques[i].q[j];
                        reclist.push(q);
                    }
                }
                //遍历试卷试题，进行还原
                for (var i = 0; i < paper.length; i++) {
                    var group = paper[i];
                    for (let j = 0; j < group.ques.length; j++) {
                        const q = group.ques[j];
                        //通过答题记录还原
                        for (var n = 0; n < reclist.length; n++) {
                            if (q.Qus_ID == reclist[n].id) {
                                //单选
                                if (q.Qus_Type == 1) {
                                    for (let index = 0; index < q.Qus_Items.length; index++) {
                                        if (q.Qus_Items[index].Ans_ID == reclist[n].ans) {
                                            q.Qus_Items[index]["selected"] = true;
                                        }
                                    }
                                }
                                //多选
                                if (q.Qus_Type == 2) {
                                    let arr = reclist[n].ans.split(',');
                                    if (arr.length <= 0) continue;
                                    for (let a = 0; a < arr.length; a++) {
                                        if (arr[a] == '') continue;
                                        for (let index = 0; index < q.Qus_Items.length; index++) {
                                            if (q.Qus_Items[index].Ans_ID == arr[a]) {
                                                q.Qus_Items[index]["selected"] = true;
                                            }
                                        }
                                    }
                                }
                                //判断
                                if (q.Qus_Type == 3) {
                                    if (reclist[n].ans == '') continue;
                                    q.Qus_Answer = reclist[n].ans == "0" ? 'true' : 'false';
                                }
                                //简答
                                if (q.Qus_Type == 4) {
                                    q.Qus_Answer = reclist[n].ans;
                                }
                                //填空
                                if (q.Qus_Type == 5) {
                                    for (let b = 0; b < q.Qus_Items.length; b++)
                                        q.Qus_Items[b]["Ans_Context"] = '';
                                    var arr = reclist[n].ans.split(',');
                                    if (arr.length < 1) continue;
                                    for (var a = 0; a < arr.length && a < q.Qus_Items.length; a++) {
                                        //if (arr[a] == '') continue;
                                        q.Qus_Items[a]["Ans_Context"] = arr[a];
                                    }
                                    q.Qus_Answer = reclist[n].ans;
                                }
                            }
                        }
                    }
                }
                //console.log(reclist);
                return paper;
            }
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


}, ['/Utilities/Components/question/function.js',
    'Components/result.js',
    'Components/question.js']);
