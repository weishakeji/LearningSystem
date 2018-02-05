/*一些基本参数*/
//当前分页,每页取多少
window.knSize = 10;
//当前分页索引
window.knIndex = 1;
//获取的记录
window.knSumCount = 0;
//上下级翻页时的当前索引
var DateIndex = -1;
//所有的数据行
var DataCount = 0;
$(function () {
    _KnowledgeAjax_int();
    //监听输入框
    SetSuggestEvent();
    //监控输入变更
    setInterval("InputChange()", 200);
    if ($("#suggestListBox").size() > 0) {
        $("#suggestListBox").dbclick(function () {
            $(this).hide();
        })
    }
});
//初始化
function _KnowledgeAjax_int() {
    var knTit = $.trim($("input[name$=tbKnTit]").val());
    if (knTit == "") {
        $("span[id$=knTitle]").text("关联：无");
    }
    else {
        $("span[id$=knTitle]").html("关联：<a href=\"/Knowledge.ashx?id=" + $("input[name$=tbKnID]").val() + "\" target=\"_blank\">" + knTit + "</a>");
        // $("span[id$=knTitle]").text("关联：" + knTit);
    }
    //资料输入框
    var tbKns = $("*[name$=tbKnsName]");
    tbKns.focus(function () {
        $(this).attr("isFocus", "true");
        _tbKnsNameFocus();
    });
    tbKns.blur(function () {
        $(this).attr("isFocus", "false");
    });
    $("*[name$=ddlKnlSort]").change(function () {
        window.knIndex = 1;
        $("*[name$=tbKnsName]").focus();
        var val = $("*[name$=tbKnsName]").val();
        var knsid = $(this).val();
        var couid = $("*[name$=ddlCourse]").val();
        _getList(knsid, couid, window.knIndex, window.knSize, val, successFunc, loadfunc, unloadfunc, errfunc);
    });
}

//资料输入框焦点事件
function _tbKnsNameFocus() {
    var val = $("*[name$=tbKnsName]").val();
    var knsid = $("*[name$=ddlKnlSort]").val();
    var couid= $("*[name$=ddlCourse]").val();
    var box = $("#suggestListBox:visible");
    if (box.size() < 1)
        _getList(knsid,couid, window.knIndex, window.knSize, val, successFunc, loadfunc, unloadfunc, errfunc);
}

//监控输入变更
function InputChange() {
    var tb = $("*[name$=tbKnsName]");
    var val = tb.val();
    //是否拥有焦点
    var isFocus = tb.attr("isFocus");
    if (isFocus && isFocus == "true") {
        if (val == window.InputChangeText) return;
    } else {
        if (typeof (val) == "undefined" || $.trim(val) == "" || val == window.InputChangeText) return;
    }
    window.InputChangeText = val;
    window.knIndex = 1;
    //内容变更则执行
    //知识库分类Id、课程id
    var knsid = $("*[name$=ddlKnlSort]").val();
    var couid= $("*[name$=ddlCourse]").val();
    _getList(knsid,couid, window.knIndex, window.knSize, val, successFunc, loadfunc, unloadfunc, errfunc);
}
//获取信息列表
function _getList(knsid, couid, index, size, sear, successFunc, loadfunc, unloadfunc, errfunc) {
    sear = encodeURIComponent(sear);
    var urlPath = "Knowledge.ashx?knsid=" + knsid + "&couid="+couid+"&index=" + index + "&size=" + size + "&sear=" + sear + "&timestamp=" + new Date().getTime();
    $.ajax({
        type: "POST", url: urlPath, dataType: "text", data: null,
        //开始，进行预载
        beforeSend: function (XMLHttpRequest, textStatus) {
            if (loadfunc != null) loadfunc(XMLHttpRequest, textStatus);
        },
        //加载出错
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (errfunc != null) errfunc(XMLHttpRequest, textStatus, errorThrown);
            if (unloadfunc != null) unloadfunc();
        },
        //加载成功！
        success: function (data) {
            if (successFunc != null) successFunc(data);
            if (unloadfunc != null) unloadfunc();
        }
    });
}
//预载
function loadfunc() {
    var tb = $("*[name$=tbKnsName]");
    var offset = tb.offset();
    var box = $("#suggestLoadingBox");
    if (box.size() < 1) {
        var html = "<div id=\"suggestLoadingBox\" class=\"suggestLoadingBox\">";
        html += "<div>正在加载……<br/>";
        html += "<img src=\"../Images/loading/load_016.gif\"></div>";
        html += "</div>";
        $("body").append(html);
        box = $("#suggestLoadingBox");
    }
    box.css({
        position: "absolute",
        top: offset.top + tb.height() + 1,
        left: offset.left - box.width() + tb.width() - 10
    });
}
function unloadfunc() {
    var box = $("#suggestLoadingBox");
    box.hide();
}
function errfunc() {
    $("#suggestListBox").hide();
    var tb = $("*[name$=tbKnsName]");
    var offset = tb.offset();
    var box = $("#suggestErrBox");
    if (box.size() < 1) {
        var html = "<div id=\"suggestErrBox\">";
        html += "<div>加载错误，请与系统管理员</div>";
        $("body").append(html);
        box = $("#suggestErrBox");
    }
    box.css({
        position: "absolute",
        top: offset.top + tb.height() + 1,
        left: offset.left - box.width() + tb.width() - 10
    });
}
function successFunc(data) {
    //json转换
    var data = eval(data);
    //生成面板
    var box = buildBox(data);
    var SumCount = data[data.length - 1].SumCount;
    if (SumCount > 0) setListData(data, box);
}
//构建下拉面板
function buildBox(data) {
    //搜索输入框
    var tb = $("*[name$=tbKnsName]");
    var offset = tb.offset();
    //构建下拉面板
    if ($("#suggestListBox").size() < 1)
        $("body").append("<div id=\"suggestListBox\"></div>");
    var box = $("#suggestListBox");
    box.css({
        position: "absolute",
        top: offset.top + tb.height() + 1,
        left: offset.left
    });
    box.show();
    //设置初始面板效果
    DataCount = data.length - 1;
    var SumCount = data[data.length - 1].SumCount;
    window.knSumCount = SumCount;
    if (SumCount < 1) {
        box.html("<dl><dd>没有相关资料！</dd></dl>");
    } else {
        var html = "<div id=\"suggestNavBox\">";
        html += "<div id=\"suggestCloseBtn\">&nbsp;</div>";
        html += "<div id=\"suggestNavBtn\"><span id=\"prev\">上一页</span><span id=\"next\">下一页</span></div>";
        html += "</div>";
        box.html("<dl></dl>" + html);
    }
    box.find("#suggestCloseBtn").click(function () {
        $("#suggestListBox").hide();
    });
    //上一页
    box.find("#prev").click(function () {
        var knsid = $("select[name$=ddlKnlSort]").val();
        var couid= $("*[name$=ddlCourse]").val();
        var val = $("input[name$=tbKnsName]").val();
        if (window.knIndex > 1)
            _getList(knsid,couid, --window.knIndex, window.knSize, val, successFunc, loadfunc, unloadfunc, errfunc);
    });
    //下页
    box.find("#next").click(function () {
        var knsid = $("select[name$=ddlKnlSort]").val();
        var couid= $("*[name$=ddlCourse]").val();
        var val = $("input[name$=tbKnsName]").val();
        var pageNum = window.knSumCount / window.knSize;
        if (window.knIndex < pageNum)
            _getList(knsid,couid, ++window.knIndex, window.knSize, val, successFunc, loadfunc, unloadfunc, errfunc);
    });
    return box;
}
//设置获取的数据
function setListData(data, box) {
    //生成数据列菜单	
    var html = "";
    for (var i = 0; i < data.length - 1; i++) {
        var n = data[i];
        var index = i + window.knSize * (window.knIndex - 1) + 1;
        html += "<dd knid=\"" + n.Kn_ID + "\" index=\"" + index + "\">" + index + "、<span>" + n.Kn_Title + "</span></dd>";
    }
    box.find("dl").html(html);
    //事件
    var dd = box.find("dd");
    dd.hover(function () {
        $(this).parent().find("dd").removeClass("over");
        $(this).addClass("over");
        var index = $(this).attr("index");
        DateIndex = Number(index);
        //SugTarget.attr("value",$(this).text());
    }, function () { });
    dd.click(function () {
        SetSuggestResrult($(this));
    });
}
//设置上下翻的事件
function SetSuggestEvent() {
    $(document).keyup(function (e) {//监听上下翻 
        if (/tbKnsName/i.test(e.target.id)) {//只代理特定元素,提高性能 
            if (e.which === 38) { //up 8	
                DateIndex--;
                if (DateIndex < 0)
                    DateIndex = DataCount - 1;

            } else if (e.which === 40) {//down 2 				
                DateIndex++;
                if (DateIndex >= DataCount)
                    DateIndex = 0;
                $("#suggestListBox").show();
            } else if (e.which === 39 || e.which === 13) { //向右键或回车选中
                var curr = $("#suggestListBox:visible").find("dd.over");
                if (curr.size() < 1) return;
                SetSuggestResrult(curr);
            }
        }
        var index = DateIndex >= 0 && DateIndex <= DataCount ? DateIndex : 0;
        $("#suggestListBox").find("dd").removeClass("over");
        var curr = $("#suggestListBox").find("dd").eq(index);
        curr.addClass("over");
    });
}
//当点击选项或按向右键时，选中的操作
function SetSuggestResrult(dd) {
    //当前资料的id与名称
    var id = dd.attr("knid");
    var title = dd.find("span").text();
    //设置
    $("input[name$=tbKnID]").val(id);
    $("input[name$=tbKnTit]").val(title);
    $("span[id$=knTitle]").html("关联：<a href=\"/Knowledge.ashx?id="+id+"\" target=\"_blank\">"+title+"</a>");
    //隐藏面板
    $("#suggestListBox").hide();
}