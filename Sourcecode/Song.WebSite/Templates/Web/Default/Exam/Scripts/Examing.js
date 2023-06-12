$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            examid: $api.querystring('id', 0),      //考试场次的id
            account: {},     //当前登录账号          
            org: {},            //当前机构
            config: {},      //当前机构配置项   

            theme: {},               //考试主题
            exam: {},             //考试 
            
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
            //加载中的状态
            loading: {
                init: true,             //初始化主要参数
                exam: true,               //加载考试信息中
                submit: false,           //成绩提交中
                paper: false             //试卷生成中
            }
        },
        mounted: function () {

        },
        created: function () {

        },
        computed: {
            //学员是否登录
            islogin: t => { return !$api.isnull(t.account); },
            //是否存在考试
            isexam: t => { return !$api.isnull(t.exam); },
            //试题总数
            questotal: function () {
                let total = 0;
                for (let i = 0; i < this.paperQues.length; i++)
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
            //学员登录
            'account': {
                handler(nv, ov) {

                },
                immediate: true
            }
        },
        methods: {
            //获取基础信息
            getbaseinfo: function () {
                var th = this;
                $api.bat(
                    $api.cache('Question/Types:9999'),      //试题类型
                    $api.post('Platform/ServerTime'),       //服务器端的时间                 
                    $api.get('Exam/ForID', { 'id': th.examid })             //考试场次的对象实体
                ).then(axios.spread(function (type, time, exam) {                  
                    //考试相关
                    th.types = type.data.result;
                    th.exam = exam.data.result;
                    //时间信息
                    th.time.server = eval('new ' + eval('/Date(' + time.data.result + ')/').source);
                    th.time.client = new Date();
                    window.setInterval(function () {
                        th.time.now = new Date().getTime();                       
                    }, 1000);                   
                    //获取考试主题和专业、试卷
                    $api.bat(
                        $api.get('Exam/ThemeForUID', { 'uid': th.examstate.uid }),
                        $api.get('Subject/ForID', { 'id': th.examstate.subject }),
                        $api.get('TestPaper/ForID', { 'id': th.examstate.paper })
                    ).then(axios.spread(function (theme, sbj, paper) {
                        th.loading.exam = false;
                        th.time.span = th.exam.Exam_Span;
                        th.theme = theme.data.result;
                        th.subject = sbj.data.result;
                        th.paper = paper.data.result;
                    })).catch(function (err) {
                        alert(err)
                        console.error(err);
                    });

                })).catch(err => alert(err))
                    .finally(() => th.loading.init = false);
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
    '/Utilities/Components/upload-file.js',
    '/Utilities/Components/question/function.js',
    'Components/question.js',
    'Components/result.js']);

