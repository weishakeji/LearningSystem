//事件组，当试题加载完成后要执行的事件
window.loadEvent = new Array();
$(window).load(function () {
    //加载试题
    var loadbox = new MsgBox("正在加载试题...", "", 70, 101, "loading");
    //从本地数据库读取试题数据
    indexDatabase().then(text => {
        if (text == null) {
            //如果本地没有，则从服务器读取
            $.ajax({
                url: "QuesExercisesItems.ashx",
                data: { couid: $().getPara("couid"), olid: $().getPara("olid") },
                type: "get", cache: true,
                beforeSend: function (result) {
                    loadbox.Open();
                },
                error: function (msg) { },
                complete: function (msg) {
                    loadbox.Close(false);
                },
                success: function (data) {
                    indexDatabase(data).then(text => {
                        $("#quesArea").html(text);
                        for (s in window.loadEvent) {
                            window.loadEvent[s]();
                        }
                    });

                }
            });
        } else {
            //如果本地有数据，则直接使用
            $("#quesArea").html(text);
            for (s in window.loadEvent) {
                window.loadEvent[s]();
            }
        }
    });

});

//页面实始化
window.loadEvent.push(function () {
    //总题数
    var count = $("#quesArea .quesItem").size();
    //设置试题宽度
    var wd = $(window).width();
    var hg = document.querySelector(".context").clientHeight;
    $("#quesArea").width(wd * (count == 0 ? 1 : count + 10))
        .css("height", hg == 0 ? "auto" : hg);
    //设置题型
    var quesTypes = $("body").attr("questype").split(",");
    //设置宽高，试题类型
    $(".quesItem").width(wd).height(hg).each(function (index, element) {
        var type = Number($(this).attr("type"));
        $(this).find(".ques-type").text("【" + $.trim(quesTypes[type - 1]) + "题】");
        if (type == 1 || type == 3) {
            $(this).find(".btnSubmit").hide();
        }
        //收藏图标的状态
        var isCollect = $(this).attr("IsCollect") == "True" ? true : false;
        if (isCollect) {
            $(this).find(".btnFav").addClass("IsCollect");
        }
    });
    //选项的序号，数字转字母
    $(".quesItemsBox").each(function () {
        $(this).find(">div").each(function (index, element) {
            var char = String.fromCharCode(0x41 + index);
            $(this).find("b").after(char + "、");
        });
    });
    //左右滑动切换试题
    finger.init();
});
//更新本地试题的按钮事件
window.loadEvent.push(function () {
    $(".btnRefresh").click(function () {
        var msg = new MsgBox("更新试题", "将当前练习的试题与服务器端保持同步。", 80, 220, "confirm");
        //msg.href = this.href;
        msg.EnterEvent = function () {
            var loadbox = new MsgBox("正在加载试题...", "", 70, 101, "loading");
            $.ajax({
                url: "QuesExercisesItems.ashx",
                data: { couid: $().getPara("couid"), olid: $().getPara("olid") },
                type: "get", cache: true,
                beforeSend: function (result) {
                    loadbox.Open();
                },
                error: function (msg) { },
                complete: function (msg) {
                    loadbox.Close(false);
                },
                success: function (data) {
                    indexDatabase(data).then(text => {
                        $("#quesArea").html(text);
                        for (s in window.loadEvent) {
                            window.loadEvent[s]();
                        }
                    });

                }
            });
            msg.Close(msg.WinId);
        }
        msg.Open();
    });
});
//本地数据库读写
function indexDatabase(text) {
    var couid = $().getPara("couid");
    var olid = $().getPara("olid");
    var dbname = "QuesExercises_" + couid + "_" + olid;
    return new Promise(((resolve, reject) => {
        Dexie.exists(dbname).then(function (exists) {
            var db = new Dexie(dbname);
            db.version(1).stores({
                questions: "++id,html"
            });
            db.open();
            if (!exists) {
                db.questions.put({ html: text }).then(function (d) {
                    resolve(text);
                }).catch(function (error) {
                    alert("error: " + error);
                });
            }
            if (text == null) {
                db.questions.where({ id: 1 }).first(d => {
                    resolve(d.html);
                });
            } else {
                db.questions.update(1, { html: text }).then(function (d) {
                    resolve(text);
                });
            }
            //db.close();
        })
    }));
    //
};

