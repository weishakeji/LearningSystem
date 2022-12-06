$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            examid: $api.querystring('id', 0),
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
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

            //++一些状态信息
            swipeIndex: 0,           //试题滑动时的索引，用于记录当前显示的试题索引号    
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
            /*
            //考试中的状态
            state: {
                loading: false, //是否正在加载试卷               
            },*/

            //加载中的状态
            loading: {
                init: true,             //初始化主要参数
                exam: true,               //加载考试信息中
                submit: false,           //成绩提交中
                paper: false             //试卷生成中
            }
        },
        updated: function () {
            this.$mathjax();
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.cache('Question/Types:9999'),
                $api.get('Exam/State', { 'examid': this.examid }),
                $api.post('Platform/ServerTime')
            ).then(axios.spread(function (type, state, time) {
                th.loading.init = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                    }
                }
                //考试相关
                th.types = type.data.result;
                th.examstate = state.data.result;   //考试的状态
                th.paperAnswer = th.examstate.result;     //答题详情，也许不存在
                //时间信息
                th.time.server = eval('new ' + eval('/Date(' + time.data.result + ')/').source);
                th.time.client = new Date();
                window.setInterval(function () {
                    th.time.now = new Date().getTime();
                    th.paperAnswer.now = th.nowtime.getTime();
                }, 1000);
                if (!th.islogin || !th.examstate.exist) return;
                //获取考试主题和专业、试卷
                $api.bat(
                    $api.cache('Exam/ForID', { 'id': th.examid }),
                    $api.cache('Exam/ThemeForUID', { 'uid': th.examstate.uid }),
                    $api.cache('Subject/ForID', { 'id': th.examstate.subject }),
                    $api.get('TestPaper/ForID', { 'id': th.examstate.paper })
                ).then(axios.spread(function (exam, theme, sbj, paper) {
                    th.loading.exam = false;
                    //判断结果是否正常
                    for (var i = 0; i < arguments.length; i++) {
                        if (arguments[i].status != 200)
                            console.error(arguments[i]);
                        var data = arguments[i].data;
                        if (!data.success && data.exception != null) {
                            console.error(data.exception);
                            throw data.message;
                        }
                    }
                    th.exam = exam.data.result;
                    th.time.span = th.exam.Exam_Span;
                    th.theme = theme.data.result;
                    th.subject = sbj.data.result;
                    th.paper = paper.data.result;
                })).catch(function (err) {
                    console.error(err);
                });

            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () { },
        computed: {
            //学员是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            },
            //是否存在考试
            isexam: function () {
                return JSON.stringify(this.exam) != '{}' && this.exam != null;
            },
            //试题总数
            questotal: function () {
                var total = 0;
                for (var i = 0; i < this.paperQues.length; i++)
                    total += Number(this.paperQues[i].count);
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
            },
            //考试开始时间
            starttime: function () {
                try {
                    return new Date(this.examstate.startTime);
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
            }
        },
        watch: {
            //当前时间
            'nowtime': function (nv, ov) {
                //离考试还有多久
                this.time.wait = this.starttime.getTime() - this.nowtime.getTime();
                this.time.wait = this.time.wait <= 0 ? 0 : Math.floor(this.time.wait / 1000);
                if (!this.examstate.isover) {
                    if (this.time.wait < this.time.requestlimit * 60 && JSON.stringify(this.exam) != '{}') {
                        //this.loading.paper = true;
                        this.generatePaper();
                    }
                    if (this.time.wait == 0 && !this.examstate.issubmit) {
                        this.examstate.doing = true;
                    }
                }
            },
            //考试剩余时间
            'surplustime': {
                handler(nv, ov) {
                    if (nv <= 0) {
                        var th = this;
                        window.setTimeout(function () {
                            if (th.examstate.isover) return;
                            if (!th.loading.paper && th.surplustime == 0 && !th.examstate.issubmit) {
                                th.submit(2);
                            }
                        }, 2000);
                    }
                },
                immediate: true
            },
            'paperQues': {
                handler(nv, ov) {
                    //if (JSON.stringify(nv) == JSON.stringify(ov)) return;                   
                    //生成答题信息（Json格式）
                    this.paperAnswer = this.generateAnswerJson(nv);
                    console.log(this.paperAnswer);
                }, immediate: false, deep: true
            },
            //答题信息变更时
            'paperAnswer': {
                handler(nv, ov) {
                    if (!this.loading.paper && !this.examstate.issubmit)
                        this.submit(1);
                },
                deep: true
            }
        },
        methods: {
            //生成试卷内容
            generatePaper: function () {
                if (JSON.stringify(this.paper) == '{}' && this.paper == null) return;
                if (this.paperQues.length > 0) return;
                if (this.loading.paper) return;
                var th = this;
                th.loading.paper = true;
                $api.put('Exam/MakeoutPaper', { 'examid': th.exam.Exam_ID, 'tpid': th.paper.Tp_Id, 'stid': th.account.Ac_ID })
                    .then(function (req) {
                        if (req.data.success) {
                            var paper = req.data.result;
                            //将试题对象中的Qus_Items，解析为json
                            for (let i = 0; i < paper.length; i++) {
                                for (let j = 0; j < paper[i].ques.length; j++)
                                    paper[i].ques[j] = th.parseAnswer(paper[i].ques[j]);
                            }
                            th.calcTime();
                            //将本地记录的答题信息还原到界面
                            window.setTimeout(function () {
                                th.loading.paper = false;
                            }, 1000);
                            th.paperQues = paper;
                            th.$nextTick(function () {
                                //th.submit(1);
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
            //将试题对象中的Qus_Items，解析为json
            parseAnswer: function (ques) {
                if (!(ques.Qus_Type == 1 || ques.Qus_Type == 2 || ques.Qus_Type == 5))
                    return ques;
                if (typeof (ques.Qus_Items) != 'string') return ques;
                var xml = $api.loadxml(ques.Qus_Items);
                var arr = [];
                var items = xml.getElementsByTagName("item");
                for (var i = 0; i < items.length; i++) {
                    var item = $dom(items[i]);
                    var ansid = Number(item.find("Ans_ID").html());
                    var uid = item.find("Qus_UID").text();
                    var context = item.find("Ans_Context").text();
                    arr.push({
                        "Ans_ID": ansid,
                        "Qus_ID": ques.Qus_ID,
                        "Qus_UID": uid,
                        "Ans_Context": ques.Qus_Type == 5 ? "" : context,
                        "selected": false
                    });
                }
                ques.Qus_Items = arr;
                return ques;
            },
            //计算序号，整个试卷采用一个序号，跨题型排序
            calcIndex: function (index, groupindex) {
                var gindex = groupindex - 1;
                var initIndex = 0;
                while (gindex >= 0) {
                    if (this.paperQues && this.paperQues.length > 0)
                        initIndex += this.paperQues[gindex].ques.length;
                    gindex--;
                };
                return initIndex + index;
            },
            //跳转到查看成绩
            goreview: function () {
                var url = $api.url.set("/student/exam/review", {
                    "examid": this.exam.Exam_ID,
                    "exrid": this.result.Exr_ID
                });
                //var url = "Review?examid=" + this.exam.Exam_ID + "&exrid=" + this.result.Exr_ID;
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
            //patter:提交方式，1为自动提交，2为交卷
            submit: function (patter) {
                var th = this;

                if (!th.islogin || !th.isexam) return;
                if (JSON.stringify(th.paperAnswer) == '{}') return;
                if (th.examstate.issubmit) return;
                //th.submitState.show = true;
                th.submitState.loading = true;
                //设置为交卷
                th.paperAnswer.patter = patter;
                var xml = this.generateAnswerXml(th.paperAnswer);
                //提交答题信息，async为异步，成绩计算在后台执行
                $api.put('Exam/SubmitResult', { 'xml': xml, 'async': false }).then(function (req) {
                    th.submitState.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.submitState.result = result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
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
                    dangerouslyUseHTMLString: true,
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
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
                    var ques = { "type": group.type, "count": group.count, "number": group.number, "q": [] }
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
                var record = this.paperAnswer;
                if (record == null || JSON.stringify(record) == '{}' || !record.ques) {
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
                if (this.nowtime > over) {
                    return paper;
                } else {
                    this.time.begin = begin;
                    this.time.over = over;
                }
                this.time.start = new Date(record.starttime);
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
                                if (reclist[n].ans == '') continue;
                                //单选
                                if (q.Qus_Type == 1) {
                                    for (let index = 0; index < q.Qus_Items.length; index++) {
                                        if (q.Qus_Items[index].Ans_ID == Number(reclist[n].ans)) {
                                            q.Qus_Items[index]["selected"] = true;
                                        }
                                    }
                                }
                                //多选
                                if (q.Qus_Type == 2) {
                                    var arr = reclist[n].ans.split(',');
                                    if (arr.length <= 0) continue;
                                    for (var a in arr) {
                                        if (arr[a] == '') continue;
                                        for (let index = 0; index < q.Qus_Items.length; index++) {
                                            if (q.Qus_Items[index].Ans_ID == Number(arr[a])) {
                                                q.Qus_Items[index]["selected"] = true;
                                            }
                                        }
                                    }
                                }
                                //判断
                                if (q.Qus_Type == 3) {
                                    q.Qus_Answer = reclist[n].ans == "0" ? 'true' : 'false';
                                }
                                //简答
                                if (q.Qus_Type == 4) {
                                    q.Qus_Answer = reclist[n].ans;
                                }
                                //填空
                                if (q.Qus_Type == 5) {
                                    var arr = reclist[n].ans.split(',');
                                    if (arr.length <= 0) continue;
                                    for (var a = 0; a < arr.length; a++) {
                                        if (arr[a] == '') continue;
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

}, ['/Utilities/Components/avatar.js',
    'Components/question.js',
    'Components/result.js']);
