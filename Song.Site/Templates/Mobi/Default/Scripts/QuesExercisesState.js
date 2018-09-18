$(function () {
    //    $("body").click(function () {
    //        //答题状态初始化
    if (typeof state != "undefined") state.init();
    //        var t = state.read();
    //        //alert(JSON.stringify(t));
    //    });

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
        this.reconsitution();
    },
    //存储项的名称
    name: {
        get: function () {      //三个参数串连，文件名+课程ID+章节ID
            return this.items.file + "_" + this.items.couid + "_" + this.items.olid;
        },
        items: {    //三个参数,olid,couid,文件名
            olid: Number($().getPara("olid")),
            couid: Number($().getPara("couid")),
            file: $().getFileName()
        }
    },
    //试题集
    data: {
        items: new Array(),
        //            item: {
        //                qid: 0,
        //                info: { index: 0, olid: 0, couid: 0, file: null, name: null },
        //                data: { ans: "", correct: "error|succ|null" }
        //            }

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
            item.info = { index: Number(qitem.attr("index")),
                olid: state.name.items.olid, coiid: state.name.items.couid,
                file: state.name.items.file, name: state.name.get()
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
    get: function (qid) {
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
    //更新数据项，如果不存在则添加
    //qitem:试题项的jquery对象，如果为空，则更新所有
    update: function (qitem) {
        if (arguments.length == 1) {
            if (typeof qitem == "number") qitem = $(".quesItem[qid=" + qitem + "]");
            var item = state.create(qitem);
            if (item == null) return;
            if (state.data.items.length < 1)
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
    clear: function () {
        var name = state.name.get();
        window.storage(name, null);
        state.data = new Object();
        state.data.items = new Array();
        state.data.current = null;
    },
    //读取本地记录
    read: function () {
        var name = state.name.get();
        state.data = window.storage(name);
        if (state.data == null) {
            state.clear();
        }
        if (typeof state.data == "string") return null;
        if (typeof state.data == "object") return state.data;
        return state.data;
    },
    //重构
    reconsitution: function () {
    }
};