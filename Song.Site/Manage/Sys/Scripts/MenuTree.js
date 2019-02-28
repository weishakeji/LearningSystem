//默认信息
var defvalue= {
    //根节点id
    rootid: function () {
        var def=0;
        var root = $().getPara("id");
        root = isNaN(root) ? -1 : Number(root);
        root = parseInt(root) <= 0 ? def : parseInt(root);
        return root;
    },
    //菜单类型,func为功能菜单,sys为系统菜单
    type: "func",
    //图标默认位置
    ico: {x: 90, y: 101}
}

//分类的根节点
var rootid = defvalue.rootid();
//
$(function () {
    setTimeout("initloyout()", 100);
    setEditPanel("root");
    icoEvent();
    $().SoapAjax("ManageMenu", "Order", { result: "", rootid: rootid, type: defvalue.type }, funcc, loading, unloading);
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
        if(!Verify.IsPass($("form:visible"))){
            return false;
        };
        update.submit();    //提交编辑数据
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
function funcc(data,msg) {
    if (msg != null)update.alert(msg);
    //重建菜单
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
        var node=eval("("+$(data).text()+")");
        update.setCtl( $("#RootEditPanel"),node);
        update.setCtl( $("#patdata"),node);   //在新增界面的上级
    })
}
//进入编辑子节点
function editNode(node) {
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
        var node=eval("("+$(data).text()+")");
        update.setCtl( $("#EditPanel"),node);   //填充数据
        update.setCtl( $("#patdata"),node);   //在新增界面的上级
       //菜单项的“移动到”
        panel.find("select[name$='moveto']").attr("value", rootid);
        panel.find("select[name$='copyto']").attr("value", rootid);
        //当前节点父节点信息
        $.get("/manage/soap/ManageMenu.asmx/ManageMenuJson", { id: node.MM_PatId }, function (data) {
            var node=eval("("+$(data).text()+")");
            var panel = $("#EditPanel");
            panel.find("#editNodeParent").text(unescape(node.MM_Name));
        })
    });
}
//开始载入时的预载
function loading() {
    $("#loadingBar").show();
    var mask = $("#screenMask");
    if (mask.size() > 0)mask.remove();
    $("body").append("<div id=\"screenMask\"/>");
    mask = $("#screenMask");
    var h = document.documentElement.clientHeight;
    mask.css({
        "width": "100%", "height": h - 30, "top": 30, "left": 5, "display": "block",
        "background-color": "#ffffff", "position": "absolute", "z-index": "50000",
        "filter": "Alpha(Opacity=60)", "-moz-opacity": 0.6, "opacity": 0.6
    }).fadeIn("slow");
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
            ico.attr("left", defvalue.ico.x).attr("top", defvalue.ico.y);
            break;
    }
}

//获取填写的数据
var update= {
    //根节点
    rootxml: function () {
        var panel = $("#RootEditPanel");
        var tmp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
        tmp += "<node type=\"root\" id=\"" + rootid + "\" rootid=\"" + rootid + "\">";
        tmp += update.getCtl(panel);
        tmp += "</node>";
        return tmp;
    },
    //修改子节点
    nodexml: function () {
        var panel = $("#EditPanel");
        var tmp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
        tmp += "<node type=\"edit\" id=\"" + panel.find("span[name='id']").text() + "\" rootid=\"" + rootid + "\">";
        tmp += update.getCtl(panel);
        tmp += "</node>";
        return tmp;
    },
    //添加子菜点
    addxml: function () {
        var panel = $("#AddPanel");
        //结果
        var tmp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
        tmp += "<node type=\"add\" id=\"-1\" rootid=\"" + rootid + "\">";
        tmp += "<patid>" + panel.find("span[name='id']").text() + "</patid>";
        tmp += update.getCtl(panel);
        tmp += "</node>";
        return tmp;
    },
    //获取控件值
    getCtl: function (panel) {
        var ctls = panel.find("*[name]:visible");
        var str = "<{0}>{1}</{0}>";
        var tmp = "<func>"+defvalue.type+"</func>";
        ctls.each(function () {
            var element = $(this).get(0).tagName.toLowerCase(); //控件html标签名
            //var tm= $(this).attr("name");
            //console.log("标签名称："+element+ "    标签内容：" +$(this).html())
            //console.log("name:"+tm+"   是否为空："+(tm==null));
            var name = $.trim($(this).attr("name") != null ? $(this).attr("name").toLowerCase() : "");  //控件名称
            if (element == "input") {
                var type = $(this).attr("type").toLowerCase();  //控件类型
                if (type == "text")tmp += str.format(name, encodeURIComponent($.trim($(this).val())));
                if (type == "checkbox")tmp += str.format(name, $(this).attr('checked'));
            }
            if (element == "span") {
                var read = $(this).attr("read");
                if (read == "no")return true;
                if (name != "")tmp += str.format(name, $(this).text());
            }
            if (element == "select") {
                tmp += str.format(name, panel.find("select[name=" + name + "] option:selected").val());
            }
        });
        //类型
        var type = panel.find("input[name=type][type=radio]:checked").val();
        tmp += str.format("type", type);
        tmp += "<icox>" + panel.find(".ico:visible").attr("left") + "</icox>";
        tmp += "<icoy>" + panel.find(".ico:visible").attr("top") + "</icoy>";
        return tmp;
    },
    //填充控件值
    setCtl: function (panel, node) {
        var ctls = panel.find("*[type!=submit][type!=button][name]");
        ctls.each(function () {
            var element = $(this).get(0).tagName.toLowerCase(); //控件html标签名
            var name = $.trim($(this).attr("name").toLowerCase());   //控件名称
            for (var n in node) {
                var attr = n.substring(n.lastIndexOf("_") + 1).toLowerCase();
				var val=unescape(node[n]);
                if (element == "input") {
                    var type = $(this).attr("type").toLowerCase();  //控件类型
                    if (attr == name && type == "text") $(this).val(val);
                    if (attr == name && type == "checkbox") $(this).attr('checked', val);
                }
                if (element == "span" && attr == name) $(this).text(val);
            }
        });
        //节点类别
        panel.find("input[name='type']").each(function () {
            var v = $(this).val();
            $(this).removeAttr("checked");
            if (v == node.MM_Type) $(this).attr("checked", "checked");
        });
        //图标
        if (node.MM_IcoX <= 0) {
            node.MM_IcoX = defvalue.ico.x;
            node.MM_IcoY = defvalue.ico.y;
        }
        var ico = $(".ico:visible");
        ico.attr("left", node.MM_IcoX).attr("top", node.MM_IcoY);
        ico.css("background-position", (-node.MM_IcoX + "px ") + (-node.MM_IcoY + "px"));
    },
    //提交事件
    submit: function () {
        var xml = "";
        if ($("#RootEditPanel").is(":visible"))xml = update.rootxml(); //修改根节点
        if ($("#EditPanel").is(":visible"))xml = update.nodexml();     //修改子节点
        if ($("#AddPanel").is(":visible")) xml = update.addxml();      //新增子节点
        if (xml == "") return;
        $().SoapAjax("ManageMenu", "Update", {result: xml, pid: Tree.RootId, type: defvalue.type}, function (data) {
            funcc(data,"完成修改操作！");
        }, loading, unloading);
    },
    //提示信息
    alert:function(msg) {
        var alert=$("#alert");
        alert.html(msg).css("font-size","60px").fadeIn(300,function(){
            $(this).animate({"font-size":"14px"},1000,function(){
                $(this).fadeOut(1000);
            });
        });
    }
}

//更改顺序
function changeOrder(res) {
    $().SoapAjax("ManageMenu", "Order", { result: res, rootid: Tree.RootId, type: defvalue.type }, function(data){
        funcc(data,"完成菜单排序更改！");
    }, loading, unloading);
}
//删除节点
function delNode(res) {
    var div = $("#NodeDragDiv");
    if (div.length < 1) return;
    var name = div.text();
    var msg = "您是否确定要删除当前菜单项：" + name + " ？\n注：\n1、当前菜单项的所有下属菜单，也会同时删除\;\n2、删除后无法恢复。";
    if (!confirm(msg)) return;
    $().SoapAjax("ManageMenu", "Del", { result: res, pid: Tree.RootId, type: defvalue.type }, function(data){
        funcc(data,"完成删除操作！");
    }, loading, unloading);
}
