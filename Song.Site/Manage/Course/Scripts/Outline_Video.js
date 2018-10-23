$(function () {
    _contentsLoyout();
});
//当窗口大变化时
$(window).resize(
	function () {
	}
);
//界面始初化布局
function _contentsLoyout() {
    //弹出窗口
    $("a[btnType=openwin]").click(function () {
        var href = $(this).attr("href");
        var title = $(this).attr("title");
        new top.PageBox(title, href, 640, 480, null, window.name).Open();
        return false;
    });
    $("input[btnType=openwin]").click(function () {
        var a = $(this).parents("tr").find("a[btnType=openwin]");
        var href = a.attr("href");
        var title = a.attr("title");
        new top.PageBox(title, href, 640, 480, null, window.name).Open();
        return false;
    });
}
