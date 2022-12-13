//试题处理中的一些常用方法
(function () {
    var method = function () {

    };
    var fn = method.prototype;
    fn.alert = function () {
        alert('来自qeustion method');
    };
    //将试题对象中的Qus_Items，解析为json
    fn.parseAnswer = function (ques) {
        if (ques.Qus_Type == 1 || ques.Qus_Type == 2 || ques.Qus_Type == 5) {
            if ($api.getType(ques.Qus_Items) != 'String') return ques;
            var xml = $api.loadxml(ques.Qus_Items);
            var arr = [];
            var items = xml.getElementsByTagName("item");
            for (var i = 0; i < items.length; i++) {
                var item = $dom(items[i]);
                var ansid = Number(item.find("Ans_ID").html());
                var uid = item.find("Qus_UID").text();
                var context = item.find("Ans_Context").text();
                var isCorrect = item.find("Ans_IsCorrect").text() == "True";
                arr.push({
                    "Ans_ID": ansid,
                    "Qus_ID": ques.Qus_ID,
                    "Qus_UID": uid,
                    "Ans_Context": context,
                    "Ans_IsCorrect": isCorrect,
                    "selected": false,
                    "answer": ''        //答题内容，用于填空题
                });
            }
            ques.Qus_Items = arr;
        }
        return ques;
    };
    //清理空html元素，内容为空的html标签隐藏起来，免得占空间
    fn.clearempty = function (dom) {
        if (dom.length < 1) return;
        var exclude = "INPUT,IMG,BUTTON,BR,TEXTAREA".split(',');
        if (exclude.includes(dom[0].tagName)) return;

        var childs = dom.childs();
        if (childs.length < 1 && dom.text().length < 1) dom.hide();
        var th = this;
        if (childs.length > 0) {
            childs.each(function () {
                th.clearempty($dom(this));
            });
        }
    }
    window.ques = new method();
})();

//window.ques.alert();