$(function () {
    _contentsLoyout();
});
//当窗口大变化时
$(window).resize(
	function () {
	    window.windowResizeTemp = window.windowResizeTemp == null ? 0 : window.windowResizeTemp++;
	    if (window.windowResizeTemp % 2 == 0) _contentsLoyout();
	}
);
//界面始初化布局
function _contentsLoyout() {
    var h = document.documentElement.clientHeight;
    var w = document.documentElement.clientWidth;
    //左侧
    $("#leftBox").height($("#editLeft").height());
    $("#leftBox iframe").height($("#editLeft").height());
    var listBox = $("#leftBox select");
    listBox.height(h - 30);
    //右侧
    var right = $("#rightBox");
    //right.width(w - $("#leftBox").width() - 25);
    var iframe = right.find("iframe");
    iframe.height(h - 40);
    //iframe.width(w - $("#leftBox").width() - 25);
}

