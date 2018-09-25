$(function () {
    if (typeof state != "undefined") state.init();
});

/*
保持试题答题状态，包括每一道题的状态
*/
var state = {
    init: function () {    //初始化
        if (this.data.items.length < 1) this.data = this.read();
        var data = this.data;
        if (data.items.length < 1) return;
        //如果有历史数据，则在界面上显示答题状态
        this.rebuild();
        //显示当前试题
        var qid = state.last().qid;
        //设置初始显示的试题
        var firstQitem = qid != 0 ? $(".quesItem[qid=" + qid + "]") : $(".quesItem:first");
        firstQitem = firstQitem.size() > 0 ? firstQitem : $(".quesItem:first");
        if (typeof finger != "undefined") {
            finger.qusmove((Number(firstQitem.attr("index")) - 1) * $(window).width());
        }
        if (typeof card != "undefined")
            card.set("curr");
    },
    //存储项的名称
    name: {
        get: function (file, couid, olid) {      //三个参数串连，文件名+课程ID+章节ID
            if (arguments.length == 3) {
                return (file + "_" + couid + "_" + olid).toLowerCase();
            }
            return this.items.file + "_" + this.items.couid + "_" + this.items.olid;
        },
        items: {    //三个参数,olid,couid,文件名
            olid: Number($().getPara("olid")),
            couid: Number($().getPara("couid")),
            file: $().getFileName().toLowerCase()
        }
    },
    //试题集信息，作为存储在localstorage的数据
    data: {
        items: new Array(),
        //        //注释部分为数据项结构
        //        item: {   //试题id,最后练习时间
        //            qid: 0, last: new time(),
        //            info: { index: 0, olid: 0, couid: 0, type: 0 },
        //            data: { ans: "", correct: "error|succ|null" }
        //        },
        //当前项，为items中的item项
        current: {}
    },
    //创建对象
    create: function (qitem) {
        //存储当前对象
        if (arguments.length == 1) {
            if (!(qitem instanceof jQuery)) return null;
            var qid = Number(qitem.attr("qid"));
            var item = new Object();
            //试题基础信息      
            item.qid = qid;
            item.last = new Date().getTime();     //当前时间，即最后练习的时间
            item.info = { index: Number(qitem.attr("index")),
                olid: state.name.items.olid, couid: state.name.items.couid,
                type: Number(qitem.attr("type"))
            };
            //试题答题信息
            var type = Number(qitem.attr("type"));  //试题类型
            //答题是否正确     
            var card = $("#cardBox dl dd[qid=" + qid + "]");
            var correct = card.hasClass("error") ? "error" : (card.hasClass("succ") ? "succ" : "null");
            //答题信息
            var ans = new Array();
            if (type == 1 || type == 2 || type == 3) {
                qitem.find(".quesItemsBox>div[issel=true]").each(function () {
                    ans.push($(this).attr("ansid"));
                });
            }
            if (type == 4 || type == 5) {
                qitem.find(".quesItemsBox>div.answer").each(function () {
                    ans.push($(this).find("input,textarea").val());
                });
            }
            item.data = { ans: ans, correct: correct };
            return item;
        }
        //创建所有对象
        if (arguments.length == 0) {
            state.data = new Object();
            state.data.items = new Array();
            state.data.current = null;
            $(".quesItem").each(function () {
                var item = state.create($(this));
                if (item != null) {
                    state.data.items.push(item);
                }
            });
            return state.data.items;
        }
    },
    //获取一个item项
    //qid: 试题id
    getdata: function (qid) {
        if (typeof qid != "number") return null;
        if (state.data.items.length < 1) state.data.items = state.create();
        var item = null;
        for (t in state.data.items) {
            if (state.data.items[t].qid == qid) {
                item = state.data.items[t];
                break;
            }
        }
        return item;
    },
    //获取最后一个练习对象
    last: function () {
        if (state.data == null) state.data = this.read();
        if (state.data != null) {
            if (state.data.items.length < 1) state.data = this.read();
            var data = this.data;
            if (data.items.length > 0) return state.data.current;
        }
        return { qid: 0, last: new Date() };
    },
    //更新数据项，如果不存在则添加
    //qitem:试题项的jquery对象，如果为空，则更新所有
    update: function (qitem) {
        if (arguments.length == 1) {
            if (typeof qitem == "number") qitem = $(".quesItem[qid=" + qitem + "]");
            var item = state.create(qitem);
            if (item == null) return;
            if (state.data == null || state.data.items.length < 1)
                state.data.items = state.create();
            var isExist = false;
            for (t in state.data.items) {
                if (state.data.items[t].qid == item.qid) {
                    state.data.items[t] = item;
                    isExist = true;
                    break;
                }
            }
            if (!isExist) state.data.items.push(item);
            state.data.current = item;
        }
        if (arguments.length == 0) {
            state.data.items = state.create();
            if (state.data.items.length > 0) {
                state.data.current = state.data.items[0];
            }
        }
        var name = state.name.get();
        var tm = state.data;
        //alert(JSON.stringify(tm));
        window.storage(name, tm);
    },
    //清除当前页面下的记录
    //name: localstorage的键值
    clear: function (name) {
        if (name == null) name = state.name.get();
        window.storage(name, null);
        state.data = new Object();
        state.data.items = new Array();
        state.data.current = null;
    },
    //读取本地记录
    //name: localstorage的键值
    read: function (name) {
        if (name == null) name = state.name.get();
        var data = window.storage(name);
        if (data == null) {
            state.clear(name);
            data = new Object();
            data.items = new Array();
            data.current = null;
        } 
        if (typeof data == "string") return null;
        if (typeof data == "object") return data;
        return data;
    },
    //将记录的答题信息，还原到界面
    rebuild: function () {
        if (state.data.items.length < 1) return;
        for (i in state.data.items) {
            var d = state.data.items[i];
            //获取试题对象
            var qitem = $(".quesItem[qid=" + d.qid + "]");
            //设置是否答题正确
            if (d.data.correct != "null") {
                qitem.find(".quesBox").addClass(d.data.correct);
                $("#cardBox dl dd[qid=" + d.qid + "]").addClass(d.data.correct);
                //如果答题错误，则显示正确答案与解析
                if (d.data.correct == "error") qitem.find(".quesAnswerBox").show();
            }
            //设置答题项
            if (d.info.type == 1 || d.info.type == 2 || d.info.type == 3) {
                for (j in d.data.ans) {
                    var ans = $.trim(d.data.ans[j]);
                    if (ans == "") continue;
                    var itemsbox = qitem.find(".quesItemsBox>div[ansid=" + ans + "]");
                    itemsbox.attr("issel", true).addClass("selected");
                }
            }
            if (d.info.type == 4 || d.info.type == 5) {     //简答题、填空题
                qitem.find(".quesItemsBox>div.answer").each(function (index) {
                    $(this).find("input,textarea").val(d.data.ans[index]);
                });
            }
        }
        //正确率
        var rate = state.clac(state.data.items);
        $(".correct-rate").text(rate.rate); //正确率
        //正确的答题数，与错误的答题数
        $(".correct-num").text(rate.correct);
        $(".error-num").text(rate.error);
    },
    //计算正确率，正确数，错误数
    //data: 来自state对象的data.items属性
    //return:返回对象，rate（正确率）,correct（正确数）,error（错误数）,sum(总数）,ansnum（答题数)
    clac: function (data) {
        var ret = { rate: 0, correct: 0, error: 0, sum: 0, ansnum: 0 };
        if (data.length < 1) return ret;
        ret.sum = data.length;
        for (i in data) {
            if (data[i].data.correct == "succ") ret.correct++;
            if (data[i].data.correct == "error") ret.error++;
        }
        ret.ansnum = ret.correct + ret.error;
        ret.rate = ret.ansnum > 0 ? Math.floor(ret.correct / ret.ansnum * 10000) / 100 : 0;
        return ret;
    }
};