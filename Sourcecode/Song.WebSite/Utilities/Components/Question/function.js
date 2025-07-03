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
            for (let i = 0; i < items.length; i++) {
                let item = $dom(items[i]);
                let ansid = item.find("Ans_ID").html();
                let uid = item.find("Qus_UID").text();
                let context = item.find("Ans_Context").text();
                let isCorrect = item.find("Ans_IsCorrect").text() == "True";
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
        var excludes = "INPUT,IMG,BUTTON,BR,TEXTAREA".split(',');
        if (excludes.includes(dom[0].tagName)) return;
        //
        var childs = dom.childs();
        if (childs.length < 1 && dom.text().length < 1) dom.hide();
        //if (dom.text().length < 1) dom.hide();
        var th = this;
        if (childs.length > 0) {
            childs.each(function () {
                th.clearempty($dom(this));
            });
        }
    };
    //获取试题缓存数据
    fn.get_cache_data = function () {
        var couid = $api.querystring('couid');
        if (couid == null || couid == '') return;
        $api.cache('Outline/Tree', { 'couid': couid, 'isuse': true }).then(function (req) {
            if (req.data.success) {
                var result = req.data.result;
                for (let i = 0; i < result.length; i++) {
                    if (result[i].Ol_QuesCount > 0) {
                        let para = { 'couid': couid, 'olid': result[i].Ol_ID, 'type': -1, 'diff': '-1', 'count': 0 };
                        $api.cache('Question/Simplify:' + (60 * 24 * 30), para)
                            .then((req) => { });
                    }
                }
                //console.log('加载章节:'+result.length);
            }
        }).catch(function (err) {
            //alert(err);
            console.error(err);
        });

    };
    window.ques = new method();
})();

//window.ques.alert();