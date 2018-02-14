
//生成管理树
function Tree(area) {
    if (area != null) Tree.Area = $(area);
    if (Tree.InitNumber > 0) return;
    //重新定义ico
    for (var t in Tree.ico) Tree.ico[t] = "<img src=\"" + Tree.icoPath + Tree.ico[t] + "\" />";
    Tree.InitNumber++;
}
//初始变量，每实例化一次，加一
Tree.InitNumber = 0;
//树形菜单所在区域
Tree.Area = "";
//图标的路径
Tree.icoPath = "/Manage/Images/tree/";
//图标
Tree.ico = {
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
    sel: 'Select.gif',
    nosel: 'noSel.gif',
    halfsel: 'halfSel.gif'
};

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
//树形菜单事件，如打开折叠等
Tree.prototype.TreeEvent = function () {
    var tree = new Tree();
    //当点击节点前的图标时，如加减号文件夹图标
    $("div[type='select']").click(function () {
        Tree.SetSelectBox($(this));
    });
    $("div[type='text']").click(function () {
        Tree.SetSelectBox($(this).prev());
    });
}
//设置复选框
Tree.SetSelectBox = function (node) {
    //复选框状态
    var state = node.attr("state");
    //是否有子结点
    var ischild = node.attr("IsChilds") == "True" ? true : false;
    var tree = new Tree();
    if (state == "nosel") Tree.ToSelect(node);
    if (state == "sel") Tree.ToNoSelect(node);
    if (state == "half") Tree.ToSelect(node);
    //如果有子节点，则设置所有下级本节点相同
    if (ischild) Tree.SearchChild(node);
    Tree.SearchParent(node);
}
//查找当前节点像的所有子级
Tree.SearchChild = function (box) {
    var state = box.attr("state");
    var panel = box.parent().next();
    var sels = panel.find("div[type='select']");
    var tree = new Tree();
    if (state == "nosel") Tree.ToNoSelect(sels);
    if (state == "sel") Tree.ToSelect(sels);
}
//查找当前节点项的所有父级
Tree.SearchParent = function (box) {
    //取兄弟节点复选框状态
    var panel = box.parent().parent();
    //兄弟个数
    var sels = panel.children("[type='nodeline']");
    //alert(sels.size());
    var selNum = 0;
    var halfNum = 0;
    sels.each(function () {
        var thisBox = $(this).find("div[type='select']");
        if (thisBox.attr("state") == "sel") selNum++;
        if (thisBox.attr("state") == "half") halfNum++;
    });
    //取父节点复选框
    var pbox = panel.prev("[type='nodeline']").find("div[type='select']");
    if (pbox.length < 1) return;
    var tree = new Tree();
    //子节点全部选中
    if (selNum == sels.size()) Tree.ToSelect(pbox);
    //子节点全部没选中
    if (selNum == 0 && halfNum == 0) {
        Tree.ToNoSelect(pbox);
    }
    //子节点存在半选
    if ((selNum > 0 && selNum < sels.size()) || halfNum != 0) {
        Tree.ToHalfSelect(pbox);
    }
    Tree.SearchParent(pbox);
}

//设置为选中状态
Tree.ToSelect = function (box) {
    box.find("img:first").attr("src", Tree.icoPath + "Select.gif");
    box.attr("state", "sel");
}
//设置为非选中状态
Tree.ToNoSelect = function (box) {
    box.find("img:first").attr("src", Tree.icoPath + "noSel.gif");
    box.attr("state", "nosel");
}
//设置为半选中状态
Tree.ToHalfSelect = function (box) {
    box.find("img:first").attr("src", Tree.icoPath + "halfsel.gif");
    box.attr("state", "half");
}
//获取树的状态
function getTreeState() {
    var sels = Tree.Area.find("div[type='select']");
    //结果
    var tmp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
    var id = $().getPara("id") == "" ? -1 : Number($().getPara("id"));
    tmp += "<nodes type=\"" + $().getPara("type") + "\" memberId=\"" + id + "\">";
    sels.each(function () {
        var stat = $(this).attr("state");
        if (stat != "nosel") {
            var mmid = $(this).attr("nodeId");
            tmp += "<node MM_Id=\"" + mmid + "\" state=\"" + stat + "\"/>";
        }
    });
    tmp += "</nodes>";
    //alert(tmp);
    return tmp;
}
