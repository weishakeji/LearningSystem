$(function () {
	default_event();
});
//菜单项的事件
function default_event() {
    //主菜单，即课程学习菜单的事件
    mui('body').on('tap', '.mm-item', function () {
        var canStudy = $(this).parent().attr("canStudy");
		var couid=$("body").attr("couid");
        if (canStudy != "True") {
            var txt = "由此进入<a href='CourseBuy.ashx?couid="+couid+"' type='link' target='_top'>购买当前课程</a>";
            txt += "或<a href='coureses.ashx' type='link' target='_top'>课程中心</a>";
            txt = "当前课程没有购买！<br/>" + txt;
            new MsgBox("提示", txt, 90, 200, "alert").Open();
        } else {
            document.location.href = $(this).attr("href");
        }
        return false;
    });
}