
/*!
* 主  题：《内容页的扩展方法》
* 说  明：内容页中一些常用的JavaScript方法。
* 功能描述：
* 1、服务器控件toolsBar中用到的方法,如新增事件、修改事件等；
* 2、页面公共的初化事件，如清除输入框内容的按钮
*
* 作  者：宋雷鸣 
* 开发时间: 2016年12月29日
*/

//初始化事件
$(initEvent);
$(function () {
    var t = $("input[type='text'][class!='Wdate'][state!='noFocus']:first");
    //alert(t.attr("onfocus"));
    t.focus();
});
//GridView中行的双击事件
//id:参数id
function OnRowDbClick(keyname, keyvalue, title, winname) {
    //子页路径
    if (ChildPage == null || ChildPage == "") return false;
    if (GridViewId == "") return false;
    var keys = GetKeyValues(GridViewId);
    //如果允许打开弹出窗口
    if (isWinOpen) {
        keyvalue = escape(keyvalue == null ? keys : keyvalue);
        OpenSysWin(keyname, keyvalue, title, winname);
    } else {
        //如果不允许弹出，则页面转向
        var url = AddPara(ChildPage, keyname, keyvalue);
        url = AddPara(ChildPage, "from", SelfPage);
        this.location.href = url;
    }
    return false;
}
//新增按钮事件
function OnAdd(winname) {
    //子页路径
    if (ChildPage == "") return false;
    if (isWinOpen) {
        OpenSysWin(null,null,null,winname);
    } else {
        var url = AddPara(ChildPage, "from", SelfPage);
        window.location.href = url;
    }
    return false;
}
//修改按钮事件
//id:参数id
function OnEdit(id, winname) {
    //子页路径
    if (ChildPage == "") return false;
    if (GridViewId == "") return false;
    var keys = GetKeyValues(GridViewId);
    if (keys == "" && id == null) {
        alert("请选择要操作的行");
        return false;
    }
    if (keys.split(",").length > 1 && id == null) {
        alert("无法同时修改多行，请选择单行数据");
        return false;
    }
    //如果允许打开弹出窗口
    if (isWinOpen) {
        id = escape(id == null ? keys : id);
        OpenSysWin(id, null,null,winname);
    } else {
        //如果不允许弹出，则页面转向
        var url = AddPara(ChildPage, "id", id);
        url = AddPara(ChildPage, "from", SelfPage);
        this.location.href = url;
    }
    return false;
}

//删除按钮事件
function OnDelete() {
    if (GridViewId == "") return false;
    var keys = GetKeyValues(GridViewId);
    if (keys == "") {
        alert("请选择要操作的行");
        return false;
    }
    var n = keys.split(",").length;
    if (!confirm("你是否真的要删除这 " + n + " 项内容？" + DelShow)) {
        return false;
    }
    return true;
}
//查看按钮的事件
function OnView() {
    //子页路径
    if (ChildPage == "") return false;
    if (GridViewId == "") return false;
    var keys = GetKeyValues(GridViewId);
    if (keys == "" && id == null) {
        alert("请选择要操作的行");
        return false;
    }
    if (keys.split(",").length > 1 && id == null) {
        alert("无法同时多个项目，请选择单行数据");
        return false;
    }
    //如果允许打开弹出窗口
    if (isWinOpen) {
        id = escape(id == null ? keys : id);
        OpenSysWin(id,null,null,window.name);
    } else {
        //如果不允许弹出，则页面转向
        var url = AddPara(ChildPage, "id", id);
        url = AddPara(ChildPage, "from", SelfPage);
        this.location.href = url;
    }
    return false;
}
//审核按钮的事件
function OnVerify() {
    if (GridViewId == "") return false;
    var keys = GetKeyValues(GridViewId);
    if (keys == "") {
        alert("请选择要操作的行");
        return false;
    }
    var n = keys.split(",").length;
    if (!confirm("您是否确定将  " + n + " 项内容通过审核？")) {
        return false;
    }
    return true;
}
//还原按钮的事件
function OnRecover() {
    if (GridViewId == "") return false;
    var keys = GetKeyValues(GridViewId);
    if (keys == "") {
        alert("请选择要操作的行");
        return false;
    }
    var n = keys.split(",").length;
    if (!IsBatchReco) {
        if (n > 1) {
            alert("无法同时还原多行，请选择单行数据");
            return false;
        }
    }
    if (!confirm("您是否确定将  " + n + " 项内容项还原？")) {
        return false;
    }
    return true;
}
//回复按钮事件
function OnAnswer() {
    //子页路径
    if (ChildPage == "") return false;
    if (GridViewId == "") return false;
    var keys = GetKeyValues(GridViewId);
    if (keys == "" && id == null) {
        alert("请选择要操作的行");
        return false;
    }
    if (keys.split(",").length > 1 && id == null) {
        alert("无法同时修改多行，请选择单行数据");
        return false;
    }
    //如果允许打开弹出窗口
    if (isWinOpen) {
        id = escape(id == null ? keys : id);
        OpenSysWin(id,null,null,window.name);
    } else {
        //如果不允许弹出，则页面转向
        var url = AddPara(ChildPage, "id", id);
        url = AddPara(ChildPage, "from", SelfPage);
        this.location.href = url;
    }
    return false;
}

//打开窗口的方法
function OpenSysWin(keyname, keyvalue, title, winname) {    
    var pid = $().getPara("pid");
    //当前焦点页面标题	
    var name = "";
    try {
        name = new top.PagePanel().focusName();
    } catch (err) {
        try {
            name = top.getAdminTitle();
        } catch (err) {
            name = $("title").text();
        }
    }
    //当前页面的上级路径，因为子页面没有写路径，默认与本页面同路径
    var url = String(window.document.location.href);

    url = url.substring(0, url.lastIndexOf("/") + 1);
    //子页页要打开的路径
    var path = ChildPage;
    if (path.substring(0, 1) != "/") {
        path = url + ChildPage;
    }

    path = keyvalue != null ? AddPara(path, keyname, keyvalue) : path;
    path = pid != "-1" || pid!="" ? AddPara(path, "pid", pid) : path;
    //
    var btnMod = $("input[name$='btnModify']");
    var ext = title;
    if (title == null) {
        ext = keyvalue != null ? "修改" : "新增";
        if (ext == "修改" && btnMod.size() < 1) ext = "编辑";
    }
    winname = winname == null ? window.name : winname;
    new top.PageBox(name + "---" + ext, path, ChildPageWd, ChildPageHg, null, winname).Open();
}
//打开窗口的方法，供其它方法调用
function OpenWin(file, title, wd, hg,winname) {
    //当前页面的上级路径，因为子页面没有写路径，默认与本页面同路径
    var url = String(window.document.location.href);
    url = url.substring(0, url.lastIndexOf("/") + 1);
    var path = file;
    if (path.substring(0, 1) != "/") {
        path = url + file;
    }
    //当前焦点页面标题	
    var name = "";
    try {
        name = new top.PagePanel().focusName();
    } catch (err) {
        try {
            name = top.getAdminTitle();
        } catch (err) {
            name = $("title").text();
        }
    }
    //
    winname = winname == null ? window.name : winname;
    new top.PageBox(name + "---" + title, path, wd, hg, null,winname).Open();
}
//为地址增加参数
function AddPara(url, key, value) {
    //当前地址不包括冒号，说明还没有任何参数
    if (url.indexOf("?") < 0) {
        url = url + "?" + key + "=" + value;
    } else {
        //如果有冒号说明已经有参数
        url = url + "&" + key + "=" + value;
    }
    return url;
}


//以下非按钮条事件
//在页面中清除自身前面，第一个输入框的内容
function initEvent() {
    $("input[type='image'][class^='Row']").hover(function () {
        $(this).addClass("RowButtonOn");
    }, function () {
        $(this).removeClass("RowButtonOn");
    });
    //当图片加载出错时
    $("img").error(
			function () {
			    $(this).attr("src", "/manage/images/nophoto.gif");
			    return true;
			}
	);
    $("a").each(
		function () {
		    if ($(this).attr("href") == null || $(this).attr("href") == "") {
		        $(this).css("color", "#999999");
		    }
		}
	);
}