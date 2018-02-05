/*!
* 主  题：《院系员工树形》
* 说  明：用树形结构，展现院系下的员工；
* 功能描述：
* 1、院系树形展现，并直接显示其下属员工信息；
* 2、员工可以拖动，院系拖动后，等同拖动其下属所有员工。
*
* 作  者：宋雷鸣 
* 开发时间: 2012年12月23日
*/


//修改数据的事件
//是否进入拖动
var isDrag = false;



//生成管理树
function Tree(area) {
    if (area != null) Tree.Area = $(area);
    if (Tree.InitNumber > 0) return;
    //重新定义ico
    for (var t in this.ico) {
        this.ico[t] = "<img src=\"" + this.icoPath + this.ico[t] + "\" />";
    }
    Tree.InitNumber++;
}
//初始变量，每实例化一次，加一
Tree.InitNumber = 0;
//树形菜单所在区域
Tree.Area = "";
//树形菜单的根节点id;
Tree.RootId = 0;
//增加员工到角色的事件
Tree.onAddNode = null
//图标的路径
Tree.prototype.icoPath = "/Manage/Images/tree/";
//图标
Tree.prototype.ico = {
    root: 'root.gif',
    folder: 'folder.gif',
    folderOpen: 'folderopen.gif',
    page: 'page.gif',
    empty: 'empty.gif',
    line: 'line.gif',
    join: 'join.gif',
    joinBottom: 'joinbottom.gif',
    plus: 'plus.gif',
    plusBottom: 'plusbottom.gif',
    minus: 'minus.gif',
    minusBottom: 'minusbottom.gif',
    nlPlus: 'nlPlus.gif',
    nlMinus: 'nlMinus.gif',
    add: 'add.gif',
    reline: 'reline.gif'
};
//开始生成树形菜单
Tree.prototype.BuildMenu = function (data) {
    //var rootHtml=BuildTree(data);
    var rootHtml = this.buildItem(null, data);
    Tree.Area.html(rootHtml);
    //去除事件
    $("body").unbind("mousedown");
    $("body").unbind("mousemove");
    $("body").unbind("mouseup");
    //处理每个院系，最后一个员工的链接线
    this.setLastEmpLink();
    //赋加事件
    this.TreeEvent();
    this.DragEvent();

}
//生成菜单
//node:数据源的节点；
//data:完整的数据源，基于soap
Tree.prototype.buildItem = function (node, data) {
    //当前节点对象
    var n = new Node(node, data);
    //开始生成
    var temp = "<div style=\"float: none;width:100%\" type=\"nodePanel\" panelId=\"" + n.Id + "\">";
    temp += "<div type=\"nodeline\" style=\"float: none;width:100%;display: table;\" >";
    //节点前的图标区域//树的连线与图标
    temp += "<div style='width:auto;float:left;' ";
    temp += "type='nodeIco' ";
    temp += "IsChilds='" + (n.IsChilds ? "True" : "False") + "'>";
    temp += this.nodeLine(n, data) + this.nodeIco(n, data);
    temp += "</div>";
    //菜单项文本
    temp += this.BuildNode(n, data);
    temp += "</div>";
    //当前院系的员工
    var emplyee = $("#emplyee dd[depid=" + n.Id + "]");
    for (var i = 0; i < emplyee.length; i++) {
        var emp = emplyee.eq(i);
        temp += "<div type=\"nodeline\" style=\"float: none;width:100%;display: table;\" >";
        //节点前的图标区域//树的连线与图标
        temp += "<div style='width:auto;float:left;' ";
        temp += "type='nodeIco' >";
        temp += this.empLine(n, data) + this.ico.page;
        temp += "</div>";
        temp += "<div style=\"width:auto;line-height: 18px;display: table; font-size: 12px;cursor: default;\"";
        temp += " type='emplyee' nodeType='text'";
        temp += " nodeEmpId='" + emp.attr("empid") + "' patId=\"" + emp.attr("depid") + "\" >";
        temp += emp.text();
        temp += "</div>";
        temp += "</div>";
    }

    if (n.IsChilds) {
        temp += "<div type='childPanel'>";
        //temp+="<div type='childPanel' >";
        for (var i = 0; i < n.Childs.length; i++) temp += this.buildItem(n.Childs[i], data);
        temp += "</div>";
    }
    temp += "</div>";
    return temp;
}
//生成节点项文本
Tree.prototype.BuildNode = function (node, data) {
    var temp = "<div style=\"width:auto;line-height: 18px;display: table;	font-weight: bold;font-size: 12px;cursor: default;\"";
    temp += "title='" + node.Intro + "'";
    temp += " nodeId='" + node.Id + "' text=\"" + node.Name + "\" type='depart' ";
    temp += " tax='" + node.Tax + "' patId=\"" + node.PatId + "\" ";
    temp += " nodeType='" + (node.Id == 0 ? "rootText" : "text") + "'>";
    //菜单节点的自定义样式
    var style = "";
    //如果当前节点显示状态为false
    //if(node.IsShow)
    temp += "<span style=\"" + style + "\">" + node.Name + "</span>";
    temp += "</div>";
    return temp;
}
//生成菜单项前的第一个节点，即链接
//node:当前节点
Tree.prototype.nodeLine = function (node, data) {
    var temp = "";
    if (node.Id != 0) {
        var p = new Node(node.Parent, data);
        while (p.Id != 0) {
            //如果是最后一个节点
            if (p.IsLast) temp = this.ico.empty + temp;
            else temp = this.ico.line + temp;
            //当前节点的上级节点		
            p = new Node(p.Parent, data);
        }
    }
    return temp;
}
//员工前面的线
Tree.prototype.empLine = function (node, data) {
    var temp = "";
    if (node.Id != 0) {
        var p = new Node(node.Parent, data);
        while (p.Id != 0) {
            //如果是最后一个节点
            if (p.IsLast) temp = this.ico.empty + temp;
            else
                temp = this.ico.line + temp;
            //当前节点的上级节点		
            p = new Node(p.Parent, data);
        }
    }
    //当前节点如果为最后一个节点，则增加空与连接符；否quot;
    //该连线是在员工前面的
    if (node.IsLast) temp += this.ico.empty + this.ico.join;
    else
        temp += this.ico.line + this.ico.join;
    return temp;
}
//菜单项第二个子节点，用于显示加减号与菜单项图标
//n:当前节点
Tree.prototype.nodeIco = function (n, data) {
    var temp = "";
    //如果是顶级，则返回根图标
    if (n.Id == 0) return this.ico.root;
    //最后一个节点,且为打开状态
    if (n.IsLast) temp += this.ico.minusBottom + this.ico.folderOpen;
    //不是最后一个节点，状态为打开
    if (!n.IsLast) temp += this.ico.minus + this.ico.folderOpen;
    return temp;
}
//处理点前图片的替换效果
//element:节点的元素
//img1:原图片
//img2:新图片
Tree.prototype.tranImg = function (el, img1, img2) {
    if (el.attr("src").indexOf(img1) > 0)
        el.attr("src", el.attr("src").replace(img1, img2));
    else
        if (el.attr("src").indexOf(img2) > 0)
            el.attr("src", el.attr("src").replace(img2, img1));
}
//设置当前院系，最后一个员工前面的链接线
Tree.prototype.setLastEmpLink = function () {
    $("div[type='emplyee']").each(function () {
        var tree = new Tree()
        var p = $(this).parent();
        //如果下一个节点不存在，则意味着当前节点是最后一个
        if (p.next().length < 1) {
            var img = p.find("div:first-child").find("img:last-child").prev();
            tree.tranImg(img, "join.gif", "joinBottom.gif");
        }
    });
}
//树形菜单事件，如打开折叠等
Tree.prototype.TreeEvent = function () {
    var tree = new Tree();
    //当点击节点前的图标时，如加减号文件夹图标
    $("div[nodeType='text']").prev("div[IsChilds='True']").click(function () {
        //栏目图标，文件夹或文件图标
        var last = $(this).find("img:last");
        //节点图标，加号或┝号
        var pre = last.prev();
        tree.tranImg(pre, "plus.gif", "minus.gif");
        tree.tranImg(pre, "minusbottom.gif", "plusbottom.gif");
        tree.tranImg(last, "folderopen.gif", "folder.gif");
        $(this).parent().next().slideToggle();
        var state = $(this).attr("state");
        $(this).attr("state", state == "true" ? "false" : "true");
    });
    //节点文本双击事件，展开与折叠
    $("div[nodeType='text']").dblclick(function () {
        //其实就是，当前节点前面图标的事件
        $(this).prev("div[IsChilds='True']").click();
    });
}
//拖动事件
Tree.prototype.DragEvent = function () {
    //当鼠标点击节点时
    $("body").mousedown(function (e) {
        //鼠标点击的当前菜单节点
        var curr = Tree.getDragObj(e);
        if (curr == null) return;
        //设置点击的焦点
        $("div[nodeType='text']").css("background-color", "transparent");
        curr.css("background-color", "#eeeeee");
        //生成可以拖动的节点，其实是复制点中的“焦点”
        var div = Tree.buildDragDiv(curr);
        div.hide();
    })
    //当鼠标移动时
    $("body").mousemove(function (e) {
        //清除原有鼠标事件效果
        addIco(null, false);
        var div = $("#NodeDragDiv");
        if (div.length < 1) return;
        div.show();
        //进入拖动状态
        isDrag = true;
        //拖动对象，跟随鼠标
        var mouse = $().Mouse(e);
        div.css("top", mouse.y).css("left", mouse.x);
        //判断员工是否在角色区域，如果是，则显示增加箭头
        var isTree = $().isMouseArea($("#EmplyeeList"), e);
        if (isTree) addIco(div, true);
    })
    //当鼠标松开点击事件时
    $("body").mouseup(function (e) {
        if (isDrag) {
            var div = $("#NodeDragDiv");
            var tax = Number(div.attr("tax"));
            var pid = Number(div.attr("patId"));
            var oldtax = Number(div.attr("oldTax"));
            var oldPid = Number(div.attr("oldPatId"));
            var oldNodeId = Number(div.attr("nodeId"));
            var isTree = $().isMouseArea($("#EmplyeeList"), e);
            if (isTree) Tree.onAddNode();
        }
        addIco(null, false);
        //清除拖动对象
        $("#NodeDragDiv").remove();
        isDrag = false;
    })
}
//生成要拖动的节点
Tree.buildDragDiv = function (curr) {
    var div = $("#NodeDragDiv");
    if (div.length < 1) {
        $("body").append("<div id=\"NodeDragDiv\"></div>");
        div = $("#NodeDragDiv");
    }
    div.html(curr.text());
    div.attr("text", curr.text());
    div.attr("nodeEmpId", curr.attr("nodeEmpId"));
    div.attr("patId", curr.attr("patId"));
    div.attr("type", curr.attr("type"));
    div.attr("nodeEmpId", curr.attr("nodeEmpId"));
    div.attr("nodeId", curr.attr("nodeId"));
    div.css({ "position": "absolute", "z-index": "1000", "width": "auto", "height": "auto",
        "border": "1px dashed #FFCC66", "background-color": "#ffffff"
    });
    return div;
}
//获取当前坐标下的节点
//return:返回树形节点的页面元素
Tree.getDragObj = function (e) {
    var mouse = $().Mouse(e);
    var x = mouse.x;
    var y = mouse.y
    var tmp = null;
    $("div[nodeType='text']").each(function () {
        var offset = $(this).offset();
        var xt = x > offset.left && x < (offset.left + $(this).width());
        var yt = y > offset.top && y < (offset.top + $(this).height());
        if (xt && yt && $(this).attr("state") != "lock") tmp = $(this);
    });
    return tmp;
}
//添加添头图标
function addIco(current, isShow) {
    var ico = $("#TreeaddIco");
    if (ico.length < 1) {
        $("body").append("<div id=\"TreeaddIco\"></div>");
        ico = $("#TreeaddIco");
    }
    ico.css("position", "absolute").css("z-index", "2000");
    ico.css("width", 13).css("height", 10);
    ico.css("background-image", "url(../Images/arrow1.gif)");
    if (current != null) {
        var offset = current.offset();
        ico.css("top", offset.top + current.height() - ico.height() / 3);
        ico.css("left", offset.left + current.width() - ico.width() / 3);
    }
    isShow ? ico.show() : ico.hide();
}
