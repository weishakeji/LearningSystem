//试题练习状态的管理
(function () {
    var state = function () {
        //return new state.init();
    }
    //创建练习状态的对象
    state.create = function (acid, couid, olid, file) {
        var s = new state();
        s.acid = acid;
        s.couid = couid;
        s.olid = olid;
        s.file = file ? file : $dom.file().toLowerCase();
        //用于记录在storage中的名字
        s.keyname = (s.file + "_" + acid + "_" + couid + "_" + olid).toLowerCase();
        return s;
    };
    var fn = state.prototype;
    fn.loading = false;
    //试题集信息，作为存储在localstorage的数据
    fn.data = {
        items: new Array(),
        /*        //注释部分为数据项结构,勿删除
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
    //数据初始化
    fn.data_init = function () {
        this.data = new Object();
        this.data.items = new Array();
        this.data.current = null;
        this.data.count = { answer: 0, correct: 0, rate: 0, sum: 0, wrong: 0 };
        return this.data;
    };
    //获取一个item项
    //qid: 试题id
    fn.getitem = function (qid, index) {
        if (!this.data) return {};
        var item = null;
        for (var j = 0; j < this.data.items.length; j++) {
            if (qid == this.data.items[j].qid) {
                item = this.data.items[j];
                break;
            }
        }
        if (item == null) {
            item = {
                qid: qid, time: new Date(),
                index: index, ans: '',
                correct: "null"     //是否答题正确，状态为succ,error,null
            }
            this.data.items.push(item);
        }
        return item;
    };
    //获取最后一个练习对象
    fn.last = function (qid, index) {
        if (qid == null) return this.data.current;
        var item = this.getitem(qid, index);
        item.time = new Date();
        this.data.current = item;
        this.update(false);
        return item;
    };
    //更新
    //toserver:是否同步到服务器端
    fn.update = function (toserver) {
        var arr = [];
        if (this.data == null) this.data = this.data_init();
        var items = this.data.items;
        //已经做了多少道题
        var number = 0, correct = 0, wrong = 0;
        if (this.data.current == null) {
            this.data.current = items.length > 0 && items[0] ? items[0] : {};
        }
        for (var j = 0; j < items.length; j++) {
            var state = items[j];
            if (!state) continue;
            if (state.ans != '') {
                number++;
                if (state.correct == 'succ') correct++;
                if (state.correct == 'error') wrong++;
            }
            arr.push(state);
        }
        var count = {
            sum: items.length,
            answer: number,
            correct: correct,
            wrong: wrong,
            rate: Math.round(correct / items.length * 10000) / 100
        }
        this.data.count = count;
        this.data.items = arr;
        this.write(toserver);
        return this.data;
    };
    //写入
    //toserver:是否同步到服务器端
    fn.write = function (toserver) {
        var th = this;
        return new Promise(function (resolve, reject) {
            $api.storage(th.keyname, th.data);
            if (!toserver) return resolve(th.data);
            //保存到服务器 
            var para = { 'acid': th.acid, 'couid': th.couid, 'olid': th.olid, 'json': th.data };
            $api.post('Question/ExerciseLogSave', para).then(function (req) {
                if (req.data.success) {
                    resolve(req.data.result);
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                reject(err);
            });
        });
    };
    //清除当前页面下的记录
    //delserver:是否删除服务端
    fn.clear = function (delserver) {
        var th = this;
        return new Promise(function (resolve, reject) {
            $api.storage(th.keyname, null);
            th.data = th.data_init();
            if (!delserver) return resolve(th.data);
            //删除服务端的数据
            var para = { 'acid': th.acid, 'couid': th.couid, 'olid': th.olid };
            $api.delete('Question/ExerciseLogDel', para).then(function (req) {
                if (req.data.success) {
                    resolve(req.data.result);
                    window.location.reload();
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                reject(err);
            });
        });
    };
    //将记录的答题信息，还原到界面
    fn.restore = function () {
        var th = this;
        var localdata = th.gettolocal();
        //获取服务端的数据
        if (th.acid <= 0) return;
        var para = { 'acid': th.acid, 'couid': th.couid, 'olid': th.olid };
        return new Promise(function (resolve, reject) {
            $api.get('Question/ExerciseLogGet', para).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    var json = $api.parseJson(result.Lse_JsonData);
                    var statedate = null;
                    if (localdata.current == null || localdata.current.time == null) {
                        statedate = json;
                    } else {
                        statedate = json.current.time > localdata.current.time ? json : localdata;
                    }
                    th.data = statedate;
                    resolve(statedate);
                } else {
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                th.data = localdata;
                reject(localdata);
            });
        });
    };
    //读取本地记录
    fn.gettolocal = function () {
        var data = $api.storage(this.keyname);
        if (data == null) data = this.data_init();
        if (typeof data == "string") return null;
        if (typeof data == "object") return data;
        return data;
    };
    //从服务端获取练习记录
    fn.gettoserver = function () {
        var para = { 'acid': this.acid, 'couid': this.couid, 'olid': this.olid };
        return new Promise(function (resolve, reject) {
            $api.get('Question/ExerciseLogGet', para).then(function (req) {
                if (req.data.success) {
                    resolve(req.data.result);
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                reject(err);
            });
        });
    };
    window.$state = state;
})();