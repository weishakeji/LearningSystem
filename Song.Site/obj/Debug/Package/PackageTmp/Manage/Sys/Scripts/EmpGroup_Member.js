$(function () {
    initloyout();
    //载入院系
    $().SoapAjax("Depart", "DepartOrder", { result: "" }, funcc, loading, unloading);

});
//页面初始化布局
function initloyout() {
    $("input[type='text']").addClass("TextBox");
    $("textarea").addClass("TextBox");
    //页面可控布局
    var cont = $(".pageWinContext");
    //左侧菜单树区域
    var left = $("#MenuTreePanel");
    var h = document.documentElement.clientHeight;
    left.height(h - 120);
    //右侧编辑区域
    var right = $("#EmplyeeList");
    var rightTit = $("#EmplyeeListTitle");
    right.height(left.height() - rightTit.height() - 10);
    var w = document.documentElement.clientWidth;
    $("#EmplyeePanel").width(w - left.width() - 60);
    //载入当前角色的员工
    var emplyee = $("#Emp4Posi dd");
    emplyee.each(function () {
        AddSingleEmp($(this));
    });
    //禁止选择文本
    $("body").bind("selectstart", function () {
        return false;
    });
    //保存按钮
    $("input[name$='btnEnter']").click(function () {
        //提交编辑数据
        UpdateNode();
        alert("保存修改！");
        new parent.PageBox().CloseAndRefresh();
        return false;
    });
}
//数据载入完成后的事件
//data:完整数据源，webservice输出
function funcc(data) {
    window.treeDataSource = data;
    var tree = new Tree("#MenuTreePanel");
    Tree.onAddNode = AddEmplyee;
    //生成菜单
    tree.BuildMenu(data);
}
//载入员工信息成功后
function emplyee(data) {
    window.emplyeeDataSource = data;
}
//开始载入时的预载
function loading() {
    $("#loadingBar").show();
    var mask = $("#screenMask");
    if (mask.size < 1) {
        $("body").append("<div id=\"screenMask\"/>");
        mask = $("#screenMask");
    }
    var h = document.documentElement.clientHeight;
    mask.css({ "position": "absolute", "z-index": "100", "width": "100%", "height": h, "top": 0, "left": 5,
        "background-color": "#ffffff", "filter": "Alpha(Opacity=60)", "display": "block", "-moz-opacity": 0.6, "opacity": 0.6
    });
    mask.fadeIn("slow");
}
//载入完成后，清除预载效果
function unloading() {
    $("#screenMask").remove();
    $("#loadingBar").hide();
}
//移动员工到角色区域的事件
function AddEmplyee() {
    //被拖动对象
    var drag = $("#NodeDragDiv");
    var type = drag.attr("type");
    var nodeEmpId = drag.attr("nodeEmpId");
    if (type == "emplyee") {
        var node = $("#MenuTreePanel div[nodeEmpId=" + nodeEmpId + "]");
        AddSingleEmp(node);
    }
    if (type == "depart") {
        var name = drag.attr("text");
        var nodeid = drag.attr("nodeId");
        if (!confirm("是否将 " + name + " 的全部下属员工设置为该角色？")) return;
        var depart = $("#MenuTreePanel div[nodeId=" + nodeid + "]");
        depart.parent().parent().find("div[type='emplyee']").each(function () {
            AddSingleEmp($(this));
        });
    }
}
//增加单个员工
function AddSingleEmp(node) {
    //隶属当前角色的员工列表
    var list = $("#EmplyeeList");
    if (list.children("div").size() < 1) list.html("");
    //如果员工已经存在
    var nodeid = node.attr("nodeEmpId");
    var emplyee = $("#EmplyeeList div[nodeEmpId=" + nodeid + "]");
    if (emplyee.length > 0) return;
    var tmp = "<div type=\"toPosi\" nodeEmpId=\"" + nodeid + "\" class=\"toPosi\">";
    tmp += "<div class=\"EmpName\">";
    tmp += node.text();
    tmp += "</div>";
    tmp += "<img alt=\"关闭\" src=\"/manage/Images/close1.gif\"/>";
    tmp += "</div>";
    list.append(tmp);
    $("#EmplyeeList div[nodeEmpId=" + nodeid + "] img").click(function () {
        if ($(this).parent().parent().children().size() <= 1)
            $(this).parent().parent().html("无");
        $(this).parent().remove();
    });
}
//生成提交的数据
function BuildUpdateXML() {
    //隶属当前角色的员工列表
    var list = $("#EmplyeeList");
    //if(list.children("div").size()<1)return "";
    //结果
    var tmp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
    tmp += "<nodes  groupid=\"" + $().getPara("id") + "\">";
    list.children("div").each(function () {
        var empId = $(this).attr("nodeEmpId");
        tmp += "<node empid=\"" + empId + "\"/>";
    });
    tmp += "</nodes>";
    return tmp;
}
//提交信息
function UpdateNode() {
    var xml = BuildUpdateXML();
    xml = encodeURIComponent(xml);
    if (xml == "") {
        if (!confirm("没有选中任何员工！\n您是否确认该组成员为空？\n\n提示：设定组成员\n拖动左侧员工或院系,到右侧区域。"))
            return false;
    }
    $().SoapAjax("Employee", "UpdateEmp4Group", { xml: xml }, function () { alert(1) }, loading, unloading);
}