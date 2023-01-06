(function () {
    var state = function () {
        return new state.init();
    }
    state.init = function () {
        //存储项的名称
        this.name = {
            get: function (file, acid, couid, olid) {      //四个参数串连，文件名+学员Id+课程ID+章节ID
                if (arguments.length == 4) {
                    return (file + "_" + acid + "_" + couid + "_" + olid).toLowerCase();
                }
                return this.items.file + "_" + this.items.acid + "_" + this.items.couid + "_" + this.items.olid;
            },
            items: {    //学员id,olid,couid,文件名
                acid: 0,
                olid: $api.querystring("olid", 0),
                couid: $api.querystring("couid", 0),
                file: $dom.file().toLowerCase()
            }
        };
        //试题集信息，作为存储在localstorage的数据
        this.data = {
            items: new Array(),
            /*        //注释部分为数据项结构
            item: {   //试题id,最后练习时间
                qid: 0,
                time: new time(),
                index: 0,
                ans: "",
                correct: "error|succ|null"
            },*/
            //一些数值         
            count: {
                sum: 0,          //总题数
                answer: 0,      //答题数量
                correct: 0,     //正确数
                wrong: 0,           //错误数
                rate: 0         //正确率
            },
            //当前项，为items中的item项
            current: {}
        };
        return this;
    };
    var fn = state.init.prototype;
    //设置学员id
    fn.setaccid = function (acid) {
        this.name.items.acid = acid;
    }
    //获取一个item项
    //qid: 试题id
    fn.getitem = function (qid) {

    };
    //获取最后一个练习对象
    fn.last = function (item) {
        if (item == null) return this.data.current;
        item.time=new Date();
        this.data.current = item;
        this.write();       
        return item;
    };
    //更新
    fn.update = function (ques) {
        this.data.items = [];
        //已经做了多少道题
        var number = 0, correct = 0, wrong = 0;
        var current = ques.length > 0 && ques[0].state ? ques[0].state : {};
        for (var j = 0; j < ques.length; j++) {
            var q = ques[j];
            if (!q.state) continue;
            if (q.state.ans != '') {
                number++;
                if (q.state.correct == 'succ') correct++;
                if (q.state.correct == 'error') wrong++;
            }
            this.data.items.push(q.state);
        }
        var count = {
            sum: ques.length,
            answer: number,
            correct: correct,
            wrong: wrong,
            rate: Math.round(correct / ques.length * 10000) / 100
        }
        this.data.count = count;
        this.write();
        return count;
    };
    //写入
    fn.write = function (name) {
        if (name == null) name = this.name.get();

        $api.storage(name, this.data);
        return this.data;
    };
    //读取本地记录
    //name: localstorage的键值
    fn.read = function (name) {
        if (name == null) name = this.name.get();
        var data = $api.storage(name);
        if (data == null) {
            this.clear(name);
            data = new Object();
            data.items = new Array();
            data.current = null;
            data.count = {
                answer: 0,
                correct: 0,
                rate: 0,
                sum: 0,
                wrong: 0
            };
        }
        if (typeof data == "string") return null;
        if (typeof data == "object") return data;
        return data;
    };
    //清除当前页面下的记录
    //name: localstorage的键值
    fn.clear = function (name) {
        if (name == null) name = this.name.get();
        $api.storage(name, null);
        this.data = new Object();
        this.data.items = new Array();
        this.data.current = null;
    };
    //将记录的答题信息，还原到界面
    fn.restore = function (ques) {
        if (!ques || ques.length < 1) return ques;
        this.data = this.read();
        for (var i = 0; i < ques.length; i++) {
            for (var j = 0; j < this.data.items.length; j++) {
                if (ques[i].Qus_ID == this.data.items[j].qid) {
                    ques[i]['state'] = this.data.items[j];
                    break;
                }
            }
        }
        return ques;
    };
    //计算正确率，正确数，错误数
    //data: 来自state对象的data.items属性
    //return:返回对象，rate（正确率）,correct（正确数）,error（错误数）,sum(总数）,ansnum（答题数)
    fn.calc = function (data) {

    };
    window.$state = new state();
    console.log($state.name.get());
})();