$(function () {
	$(".version").text(Verify.version);
    //添加菜单序号
    $(".menubox dd").each(function (index, element) {
        //次菜单项索引
        var num = $(this).parent().find("dd").index($(this)) + 1;
        $(this).html(num + "、" + $(this).html());
    });
    //菜单项事件
    $(".menubox dd").click(function () {
        //主菜单项索引
        var index = $(this).parent().parent().find("dl").index($(this).parent()) + 1;
        //次菜单项索引
        var num = $(this).parent().find("dd").index($(this)) + 1;
        //调取页面
        var file = "Demo/" + index + "-" + num + ".htm";
        history.pushState("", "", $().setPara(window.location.href, "file", file));
        setFile(file);
    });
    //默认显示第一个菜单
    var file = $().getPara("file");
    if (file == "") $(".menubox dd:first").click();
    if (file != "") setFile(file);
});
//设置右侧内容
function setFile(file) {
    $(".context").attr("src", file);
    var name = file.substring(file.indexOf("/") + 1, file.indexOf("."));
    var arr = name.split("-");
    //设置样式
    $(".menubox dd").removeClass("curr");
    var curr = $(".menubox:eq(" + (Number(arr[0]) - 1) + ")").find("dd:eq(" + (Number(arr[1]) - 1) + ")");
    curr.addClass("curr");
}

/****** 扩展方法，给地址栏添加参数以及取参数
*/
//获取QueryString参数
jQuery.fn.getPara = function (url, key) {
    if (arguments.length == 1) {
        key = arguments[0];
        url = String(window.document.location.href);
    }
    var value = "";
    if (url.indexOf("?") > -1) {
        var ques = url.substring(url.lastIndexOf("?") + 1);
        var tm = ques.split('&');
        for (var i = 0; i < tm.length; i++) {
            var arr = tm[i].split('=');
            if (arr.length < 2) continue;
            if (key.toLowerCase() == arr[0].toLowerCase()) {
                value = arr[1];
                break;
            }
        }
    }
    return value;
}
//添加链接地址的参数
//url:超链接
//key:参数的名称
//value:参数的值
jQuery.fn.setPara = function (url, key, value) {
    if (isNull(url) || isNull(key)) return url; //如果网址或参数名为空，则返回
    //如果没有参数，直接添加
    if (url.indexOf("?") < 0) return url + "?" + key + "=" + value;
    //如果已经有参数
    var paras = getParas(url);
    paras = setPara(paras, key, value);
    url = url.substring(0, url.lastIndexOf("?"));
    return url + "?" + stringPara(paras);
    function isNull(data) {
        return (data == "" || data == undefined || data == null) ? true : false;
    }
    //将参数转换成数组格式
    function getParas(url) {
        if (url.indexOf("?") > -1) url = url.substring(url.lastIndexOf("?") + 1);
        var tm = url.split('&');
        var paras = new Array();    //要返回的数组
        for (var i = 0; i < tm.length; i++) {
            var arr = tm[i].split('=');
            if (arr.length < 2) continue;
            paras.push({ key: arr[0].toLowerCase(), value: arr[1] });
        }
        return paras;
    }
    //设置参数，如果不存在则添加
    function setPara(paras, key, value) {
        var isexist = false;
        for (var i = 0; i < paras.length; i++) {
            if (paras[i].key == key.toLowerCase()) {
                isexist = true;
                paras[i].value = value;
                break;
            }
        }
        if (!isexist) paras.push({ key: key, value: value });
        return paras;
    }
    //将数组的参数转换为url参数格式
    function stringPara(paras) {
        var str = "";
        for (var i = 0; i < paras.length; i++) {
            if (paras[i].value == null) continue;
            str += paras[i].key + "=" + paras[i].value + "&";
        }
        if (str.length > 0) {
            if (str.charAt(str.length - 1) == "&") {
                str = str.substring(0, str.length - 1)
            }
        }
        return str;
    }
}