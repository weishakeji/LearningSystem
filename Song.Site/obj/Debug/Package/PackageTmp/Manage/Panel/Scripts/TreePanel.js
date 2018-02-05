/*!
* 主  题：《菜单树生成》
* 说  明：用于管理页面左侧树形菜单。
* 功能描述：
* 1、首先生成最左侧的选项卡；
* 2、再生成相关的菜单树
*
*
* 作  者：宋雷鸣 
* 开发时间: 2011年4月4日
*/


//页面左侧树形菜单的面板
function TreePanel() {
    //this.load();	
}
//三级菜单（树形菜单），是否全部展开
//yes：全部展开；no:全部关闭；def:默认，打开第一个
TreePanel.TreeIsShow = "def";
//菜单条的打开方式，1：同时打开多个；2：同一时刻只打开一个
TreePanel.TreeBarOpen = "2";
//数据源
TreePanel.DataSource = null;
//载入数据
TreePanel.prototype.load = function () {
    $.ajax({
        type: "get",
        url: "Panel/TreePanel.aspx?timestamp=" + new Date().getTime(),
        dataType: "html",
        data: {},
        //开始，进行预载
        beforeSend: function (XMLHttpRequest, textStatus) {
            treePanleLoading();
        },
        //加载出错
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (unloadfunc != null) {
                alert(textStatus);
                unloadfunc();
            }
        },
        //加载成功！
        success: function (data) {
            //alert(data);
            SetManageMenu(data);
            unTreePanleLoading();
        }
    });
    //生成左侧的根菜单树
    function SetManageMenu(data) {
        var tree = new TreePanel();
        //左侧区域
        var pl = $("#consTreePanel");
        pl.html(data);
        TreeInit("#consTreePanel");
        TreeEvent("#consTreePanel");
        tree.focus();
        //设置节点前的图标与连线样式
        pl.find(".nodeIco").each(
				function () {
				    var num = Number($(this).find("img").size());
				    $(this).width(num * 18);
				    $(this).css("float", "left");
				}
			);
        $(".treepanel").height($("#consContextPanel").height());
        //如果不是超管，则进行权限判断；
        //emplyeeIsAdmin与emplyeeId参数，来自于console.aspx页面初始化
        if (emplyeeIsAdmin != 0) {
            //$().SoapAjax("Purview","GetAll4Array",{empId:emplyeeId},RemoveOnPur,treePanleLoading,unTreePanleLoading);
        }
    }
    //管理界面左侧,功能菜单区的预载框
    function treePanleLoading() {
        var treepanel = $("#consTreePanel");
        var offset = treepanel.offset();
        var html = "<div id=\"treePanleLoadingMask\">";
        html += "<div>正在加载……<br/>";
        html += "<img src=\"Images/loading/load_016.gif\"></div>";
        html += "</div>";
        $("body").append(html);
        var mask = $("#treePanleLoadingMask");
        mask.css("position", "absolute");
        mask.css("z-index", "2000");
        var h = document.documentElement.clientHeight;
        var offset = $("#consBody").offset();
        var height = h - offset.top - $("#consFoot").height();
        mask.css("width", treepanel.width());
        mask.css("height", height);
        mask.css("top", offset.top);
        mask.css("left", offset.left);
        mask.css("background-color", "#ffffff");
        var alpha = 100;
        mask.css("filter", "Alpha(Opacity=" + alpha + ")");
        mask.css("display", "block");
        mask.css("-moz-opacity", alpha / 100);
        mask.css("opacity", alpha / 100);
        mask.fadeIn("slow");
    }

    //去除，管理界面左侧,功能菜单区的预载界面
    function unTreePanleLoading() {
        $("#treePanleLoadingMask").fadeOut();
    }
}

//设置当前面板为焦点
TreePanel.prototype.focus = function (id) {
    //左侧区域
    var pl = $("#consTreePanel");
    //标题的焦点
    pl.find(".titlePanel div").attr("class", "out");
    pl.find("div[class='treepanel']").hide();
    if (id != null) {
        pl.find(".titlePanel div[id='consTreeTitle_" + id + "']").attr("class", "current");
        pl.find("div[id='consTreePanel_" + id + "']").show();
    } else {
        pl.find(".titlePanel div:first").attr("class", "current");
        pl.find("div[class='treepanel']:first").show();
    }
    //取出所有的tree面板
    var panel = pl.find("div[class='treepanel']");
}

//树形的初始化，例如图标设置
function TreeInit(panel) {
    //菜单条,有下级节点的
    var bar = $(panel + " div[type='menuBar'][IsChilds='True']");
    //设置所有为关闭状态
    //bar.attr("class","menuBar menuBarClose");
    //如果全部展开
    if (TreePanel.TreeIsShow == "yes") {
        bar.attr("openstate", "true");
        bar.attr("class", "menuBar menuBarOpen");
        bar.next().show();
    }
    //如果全部关闭
    if (TreePanel.TreeIsShow == "no") {
        bar.attr("openstate", "false");
        bar.attr("class", "menuBar menuBarClose");
        bar.next().hide();
    }
    //默认状态，打开第一个菜单，其余关闭
    if (TreePanel.TreeIsShow == "def") {
        bar.attr("openstate", "false");
        bar.attr("class", "menuBar menuBarClose");
        bar.next().hide();
        var first = $("div[class='treepanel']").find("div[type='menuBar']:first");
        first.attr("openstate", "true");
        first.attr("class", "menuBar menuBarOpen");
        first.next().show();
    }
}
//树形菜单事件，如打开折叠等
function TreeEvent(panel) {
    //左侧区域标题项的事件
    $(panel + " .titlePanel div").click(function () {
        var thisId = $(this).attr("id");
        thisId = thisId.substring(thisId.indexOf("_") + 1);
        var tree = new TreePanel();
        tree.focus(thisId);
    });
    //菜单条的事件
    $(panel + " div[IsChilds='True'][type='menuBar']").click(function () {
        var n = $(this).next();
        var state = $(this).attr("openstate");
        if (TreePanel.TreeBarOpen == "1") {
            if (state == "true") {
                n.slideUp();
                $(this).attr("openstate", "false");
                $(this).attr("class", "menuBar menuBarClose");
            } else {
                n.slideDown();
                $(this).attr("openstate", "true");
                $(this).attr("class", "menuBar menuBarOpen");
            }
        }
        //打开当前的，其它的则关闭				
        if (TreePanel.TreeBarOpen == "2") {
            //兄弟节点
            var sibl = $(this).siblings("div[IsChilds='True'][type='menuBar']");
            //自身节点的处理
            if (state == "true") {
                n.slideUp();
                $(this).attr("openstate", "false");
                $(this).attr("class", "menuBar menuBarClose");
            } else {
                n.slideDown();
                $(this).attr("openstate", "true");
                $(this).attr("class", "menuBar menuBarOpen");
            }
            //兄弟节点的处理
            sibl.attr("openstate", "false");
            sibl.attr("class", "menuBar menuBarClose");
            sibl.next().slideUp();
        }
    });
    //当点击节点前的图标时，如加减号文件夹图标
    $(panel + " div[IsChilds='True'][type='nodeIco']").click(function () {			//alert(1);
        //栏目图标，文件夹或文件图标
        var last = $(this).find("img:last");
        //节点图标，加号或┝号
        var pre = last.prev();
        tranImg(pre, "plus.gif", "minus.gif");
        tranImg(pre, "minusbottom.gif", "plusbottom.gif");
        tranImg(last, "folderopen.gif", "folder.gif");
        $(this).parent().next().slideToggle();
        var state = $(this).attr("state");
        $(this).attr("state", state == "true" ? "false" : "true");
    });
    //当点击节点文本时
    $(panel + " div[type='text'][IsChilds='True']").click(
		function () {			//alert($(this).parent().html());
		    //栏目图标，文件夹或文件图标
		    var last = $(this).parent().find("div:first").find("img:last");
		    //节点图标，加号或┝号
		    var pre = last.prev();
		    tranImg(pre, "plus.gif", "minus.gif");
		    tranImg(pre, "minusbottom.gif", "plusbottom.gif");
		    tranImg(last, "folderopen.gif", "folder.gif");
		    $(this).parent().next().slideToggle();
		    var state = $(this).attr("state");
		    $(this).attr("state", state == "true" ? "false" : "true");
		}
	);
    //根节点事件
    $(panel).find("a").click(function () {
        var type = $(this).parent().attr("nodetype");
        var link = $(this).attr("href");
        var id = $(this).parent().attr("nodeId");
        var name = $(this).text();
        var path = $(this).parent().attr("path");
        switch (type) {
            case "link": //如果是外部链接，直接打开
                return true;
            case "open": //如果是弹出窗口
                new PageBox(name, link, 50, 50).Open();
                return false;
            case "item":
            default:
                var p = new PagePanel();
                p.add(name, path, link, id);
                return false;
        }
    });
}
//处理点前图片的替换效果
//element:节点的元素
//img1:原图片
//img2:新图片
function tranImg(el, img1, img2) {
    if (el.attr("src").indexOf(img1) > 0)
        el.attr("src", el.attr("src").replace(img1, img2));
    else
        if (el.attr("src").indexOf(img2) > 0)
            el.attr("src", el.attr("src").replace(img2, img1));
}