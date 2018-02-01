$("#MenuTreePanel").select(function () {
    //return false;
});
//分类的根节点
var rootid = $().getPara("id");
rootid = parseInt(rootid) < 0 ? 0 : parseInt(rootid);
//默认图标的坐标
var icoX = 90; icoY = 101;
//
$(function () {
    setTimeout("initloyout()", 100);
    setEditPanel("root");
    icoEvent();
    var id = $().getPara("id");
    $().SoapAjax("ManageMenu", "Order", { result: "", rootid: rootid, type: "func" }, funcc, loading, unloading);
});
//页面初始化布局
function initloyout() {
    $("input[type='text']").addClass("TextBox");
    //左侧菜单树区域	//右侧编辑区域
    var h = document.documentElement.clientHeight - 20;
    $("#MenuTreePanel").height(h - 120);
    $("#Panel").height(h - 153);
    //右侧编辑区事件
    $("#EditPanelTitle div").click(function () {
        var type = $(this).attr("type");
        setEditPanel(type);
    });
    //保存按钮
    $("input[name$='btnEnter']").click(function () {
        //提交编辑数据
        UpdateNode();
        return false;
    });
}
//图示事件
function icoEvent() {
    $(".ico").click(function () {
        var panel = $("#icoPanel");
        var off = $(this).offset();
        panel.css({ left: off.left - panel.width() / 2, top: off.top + $(this).height() });
        panel.show();
        panel.mousemove(function (elem, e) {
            var ico = $(".ico:visible");
            var xy = $().Mouse(e);
            var off = $("#icoPanel").offset();
            var x = xy.x - off.left - 8;
            var y = xy.y - off.top - 9;
            ico.attr("left", x).attr("top", y);
            ico.css("background-position", (-x + "px ") + (-y + "px"));
        });
        panel.click(function () {
            $(this).unbind("click");
            $(this).unbind("mousemove");
            $(this).hide();
        });
    });
}
//数据载入完成后的事件
//data:完整数据源，webservice输出
function funcc(data) {
    window.treeDataSource = data;
    var tree = new Tree("#MenuTreePanel");
    Tree.RootClick = editroot;
    Tree.NodeClick = editNode;
    //
    Tree.RootId = rootid;
    Tree.onChangeOrder = changeOrder;
    Tree.onDelNode = delNode;
    //生成菜单
    tree.BuildMenu(data);
    //$("#MenuTreePanel").html(rootHtml); 
    editroot();
}
//编辑根节点
function editroot() {
    var root = $("#RootEditPanel");
    var edit = $("#EditPanel");
    setEditPanel("edit");
    root.show();
    edit.hide();
    $.get("/manage/soap/ManageMenu.asmx/ManageMenuJson", { id: rootid }, function (data) {
        eval($(data).text());
        var panel = $("#MenuEditPanel");
        panel.find("input[name='rootname']").attr("value", node.MM_Name);
        //样式
        var cbbold = panel.find("input[type='checkbox'][name='rtcbIsBold']");
        node.MM_IsBold ? cbbold.attr("checked", "checked") : cbbold.removeAttr("checked");
        var cbitalic = panel.find("input[type='checkbox'][name='rtcbIsItalic']");
        node.MM_IsItalic ? cbitalic.attr("checked", "checked") : cbitalic.removeAttr("checked");
        var cbshow = panel.find("input[type='checkbox'][name='rtcbIsShow']");
        node.MM_IsShow ? cbshow.attr("checked", "checked") : cbshow.removeAttr("checked");
        var cbuse = panel.find("input[type='checkbox'][name='rtcbIsUse']");
        node.MM_IsUse ? cbuse.attr("checked", "checked") : cbuse.removeAttr("checked");
        panel.find("textarea[name='rtintro']").text(node.MM_Intro);
        panel.find("#addNodeParent").text(node.MM_Name);
        //图标
        if (node.MM_IcoX <= 0) {
            node.MM_IcoX = icoX;
            node.MM_IcoY = icoY;
        }
        var ico = $(".ico:visible");
        ico.attr("left", node.MM_IcoX).attr("top", node.MM_IcoY);
        ico.css("background-position", (-node.MM_IcoX + "px ") + (-node.MM_IcoY + "px"));
    })
    $("span[edit='id']").text(rootid);
}
//进入编辑子节点
function editNode(node) {
    //alert(typeof(node));
    //if(node==null)node=$(this);
    var root = $("#RootEditPanel");
    var edit = $("#EditPanel");
    setEditPanel("edit");
    root.hide();
    edit.show();
    var data = window.treeDataSource;
    var panel = $("#MenuEditPanel");
    var id = node.attr("nodeId");
    //获取单个信息
    $.get("/manage/soap/ManageMenu.asmx/ManageMenuJson", { id: id }, function (data) {
        //生成node节点对象
        eval($(data).text());
        //当前节点信息
        panel.find("span[edit='id']").text(node.MM_Id);
        panel.find("span[edit='pid']").text(node.MM_PatId);
        panel.find("span[edit='tax']").text(node.MM_Tax);
        panel.find("input[name='name']").attr("value", node.MM_Name);
        panel.find("#addNodeParent").text(node.MM_Name);
        //菜单项的“移动到”
        var ddlMove = panel.find("select[name$='ddlMove']");
        ddlMove.attr("value", rootid);
        //节点类别
        panel.find("input[name='type']").each(
                    function () {
                        var v = $(this).val();
                        $(this).removeAttr("checked");
                        if (v == node.MM_Type) $(this).attr("checked", "checked");
                    }
                );
        panel.find("input[name='link']").attr("value", node.MM_Link);
        panel.find("input[name='marker']").attr("value", node.MM_Marker);
        panel.find("textarea[name='intro']").text(node.MM_Intro);
        //样式
        var cbbold = panel.find("input[type='checkbox'][name='cbIsBold']");
        node.MM_IsBold ? cbbold.attr("checked", "checked") : cbbold.removeAttr("checked");
        var cbitalic = panel.find("input[type='checkbox'][name='cbIsItalic']");
        node.MM_IsItalic ? cbitalic.attr("checked", "checked") : cbitalic.removeAttr("checked");
        var cbshow = panel.find("input[type='checkbox'][name='cbIsShow']");
        node.MM_IsShow ? cbshow.attr("checked", "checked") : cbshow.removeAttr("checked");
        var cbuse = panel.find("input[type='checkbox'][name='cbIsUse']");
        node.MM_IsUse ? cbuse.attr("checked", "checked") : cbuse.removeAttr("checked");
        //图标
        if (node.MM_IcoX <= 0) {
            node.MM_IcoX = icoX;
            node.MM_IcoY = icoY;
        }
        var ico = $(".ico:visible");
        ico.attr("left", node.MM_IcoX).attr("top", node.MM_IcoY);
        ico.css("background-position", (-node.MM_IcoX + "px ") + (-node.MM_IcoY + "px"));
        //当前节点父节点信息
        $.get("/manage/soap/ManageMenu.asmx/ManageMenuJson", { id: node.MM_PatId }, function (data) {
            eval($(data).text());
            var panel = $("#MenuEditPanel");
            panel.find("#editNodeParent").text(node.MM_Name);
        })
    });
}
//开始载入时的预载
function loading() {
    $("#loadingBar").show();
    var mask = $("#screenMask");
    mask.remove();
    mask = null
    delete mask;
    var html = "<div id=\"screenMask\"/>";
    $("body").append(html);
    mask = $("#screenMask");
    mask.css("position", "absolute");
    mask.css("z-index", "50000");
    var h = document.documentElement.clientHeight;
    mask.css("width", "100%");
    mask.css("height", h - 30);
    mask.css("top", 30);
    mask.css("left", 5);
    mask.css("background-color", "#ffffff");
    mask.css("filter", "Alpha(Opacity=60)");
    mask.css("display", "block");
    mask.css("-moz-opacity", 0.6);
    mask.css("opacity", 0.6);
    mask.fadeIn("slow");
}
//载入完成后，清除预载效果
function unloading() {
    $("#screenMask").remove();
    $("#loadingBar").hide();
}
//控制编辑面板的事件
//pevent:"edit"为编辑节点，"add"为新增
function setEditPanel(pevent) {
    //编辑区面板
    var panel = $("#MenuEditPanel #Panel");
    panel.children().hide();
    var edit = $("#RootEditPanel").parent();
    var add = $("#AddPanel");
    $("#EditPanelTitle").children().removeClass("click");
    switch (pevent) {
        case "edit":
            add.hide();
            edit.show();
            $("#EditPanelTitle div[type='edit']").addClass("click");
            break;
        case "add":
            edit.hide();
            add.show();
            $("#EditPanelTitle div[type='add']").addClass("click");
            var ico = $(".ico:visible");
            ico.attr("left", icoX).attr("top", icoY);
            break;
    }
}
//修改节点信息
function UpdateNode() {
    var xml = "";
    //如果是编辑状态
    if ($("#RootEditPanel").parent().is(":visible") != false) {
        if ($("#RootEditPanel").is(":visible") != false) {
            if ($.trim($("#rootname").val()) == "")
                alert("名称不得为空！");
            else
                xml = updateRoot();
        } else {
            if ($.trim($("#name").val()) == "")
                alert("名称不得为空！");
            else
                xml = updateNode();
        }
    }
    if ($("#AddPanel").is(":visible") != false) {
        if ($.trim($("#addname").val()) == "")
            alert("名称不得为空！");
        else
            xml = addNode();
    }
    if (xml == "") return;
    var res = encodeURIComponent(xml);
    $().SoapAjax("ManageMenu", "Update", { result: res, pid: Tree.RootId, type: "func" }, funcc, loading, unloading);
}
//递交根节点信息
function updateRoot() {
    var panel = $("#MenuEditPanel");
    //结果
    var tmp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
    tmp += "<node type=\"root\" id=\"" + $().getPara("id") + "\" rootid=\"" + $().getPara("id") + "\"  func=\"func\">";
    tmp += "<name>" + $("#rootname").val() + "</name>";
    tmp += "<func>func</func>";
    tmp += "<IsItalic>" + panel.find("input[type='checkbox'][name='rtcbIsItalic']").attr("checked") + "</IsItalic>";
    tmp += "<IsBold>" + panel.find("input[type='checkbox'][name='rtcbIsBold']").attr("checked") + "</IsBold>";
    tmp += "<IsShow>" + panel.find("input[type='checkbox'][name='rtcbIsShow']").attr("checked") + "</IsShow>";
    tmp += "<IsUse>" + panel.find("input[type='checkbox'][name='rtcbIsUse']").attr("checked") + "</IsUse>";
    tmp += "<intro><![CDATA[" + panel.find("textarea[name='rtintro']").text() + "]]></intro>";
    tmp += "<icox>" + panel.find(".ico:visible").attr("left") + "</icox>";
    tmp += "<icoy>" + panel.find(".ico:visible").attr("top") + "</icoy>";
    tmp += "</node>";
    return tmp;
}
//递交子节点信息
function updateNode() {
    var panel = $("#MenuEditPanel");
    //结果
    var tmp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
    //移动到
    var ddlMove = panel.find("select[name$='ddlMove']");
    var mrootid = ddlMove.attr("value");
    //复制到
    var ddlCopy = panel.find("select[name$='ddlCopy']");
    var crootid = ddlCopy.attr("value");
    tmp += "<node type=\"edit\" id=\"" + $("span[edit='id']").text() + "\" rootid=\"" + rootid + "\" moveto=\"" + mrootid + "\" copyto=\"" + crootid + "\" func=\"func\">";
    tmp += "<name>" + $("#name").val() + "</name>";
    //判断类型
    var type = "";
    panel.find("input[name='type']").each(function () {
        if ($(this).attr("checked")) type = $(this).val();
    });
    tmp += "<type>" + type + "</type>";
    tmp += "<link><![CDATA[" + $("#link").val() + "]]></link>";
    tmp += "<marker>" + $("#marker").val() + "</marker>";
    tmp += "<func>func</func>";
    tmp += "<IsItalic>" + panel.find("input[type='checkbox'][name='cbIsItalic']").attr("checked") + "</IsItalic>";
    tmp += "<IsBold>" + panel.find("input[type='checkbox'][name='cbIsBold']").attr("checked") + "</IsBold>";
    tmp += "<IsShow>" + panel.find("input[type='checkbox'][name='cbIsShow']").attr("checked") + "</IsShow>";
    tmp += "<IsUse>" + panel.find("input[type='checkbox'][name='cbIsUse']").attr("checked") + "</IsUse>";
    tmp += "<intro><![CDATA[" + panel.find("textarea[name='intro']").text() + "]]></intro>";
    tmp += "<icox>" + panel.find(".ico:visible").attr("left") + "</icox>";
    tmp += "<icoy>" + panel.find(".ico:visible").attr("top") + "</icoy>";
    tmp += "</node>";
    return tmp;
}
//递交新增子节点信息
function addNode() {
    var panel = $("#AddPanel");
    //结果
    var tmp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
    tmp += "<node type=\"add\" id=\"-1\" rootid=\"" + $().getPara("id") + "\">";
    tmp += "<name>" + $("#addname").val() + "</name>";
    tmp += "<pid>" + $("span[edit='id']").text() + "</pid>";
    //判断类型
    var type = "";
    panel.find("input[name='addtype']").each(function () {
        if ($(this).attr("checked")) type = $(this).val();
    });
    tmp += "<type>" + type + "</type>";
    tmp += "<link><![CDATA[" + $("#addlink").val() + "]]></link>";
    tmp += "<marker>" + $("#addmarker").val() + "</marker>";
    tmp += "<func>func</func>";
    tmp += "<IsItalic>" + panel.find("input[type='checkbox'][name='addcbIsItalic']").attr("checked") + "</IsItalic>";
    tmp += "<IsBold>" + panel.find("input[type='checkbox'][name='addcbIsBold']").attr("checked") + "</IsBold>";
    tmp += "<IsShow>" + panel.find("input[type='checkbox'][name='addcbIsShow']").attr("checked") + "</IsShow>";
    tmp += "<IsUse>" + panel.find("input[type='checkbox'][name='addcbIsUse']").attr("checked") + "</IsUse>";
    tmp += "<intro><![CDATA[" + panel.find("textarea[name='addintro']").text() + "]]></intro>";
    tmp += "<icox>" + panel.find(".ico").attr("left") + "</icox>";
    tmp += "<icoy>" + panel.find(".ico").attr("top") + "</icoy>";
    tmp += "</node>";
    return tmp;
}
//更改顺序
function changeOrder(res) {
    $().SoapAjax("ManageMenu", "Order", { result: res, rootid: Tree.RootId, type: "func" }, funcc, loading, unloading);
}
//删除节点
function delNode(res) {
    var div = $("#NodeDragDiv");
    if (div.length < 1) return;
    var name = div.text();
    var msg = "您是否确定要删除当前菜单项：" + name + " ？\n注：\n1、当前菜单项的所有下属菜单，也会同时删除\;\n2、删除后无法恢复。";
    if (!confirm(msg)) return;
    $().SoapAjax("ManageMenu", "Del", { result: res, pid: Tree.RootId, type: "func" }, funcc, loading, unloading);
}