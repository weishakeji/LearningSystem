$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            examid: $api.querystring('id', 0),
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项   

        },
        mounted: function () {
            
        },
        created: function () {
           
        },
        computed: {
            //学员是否登录
            islogin: t => { return !$api.isnull(t.account); },
            //是否存在考试
            isexam:  t => { return !$api.isnull(t.exam); },          
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
           
        },
        methods: {
           
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

