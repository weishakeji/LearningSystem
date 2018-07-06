$(function () {
	default_event();
});
//菜单项的事件
function default_event() {
    //主菜单，即课程学习菜单的事件
    mui('body').on('tap', '.mm-item', function () {
        var isBuy = $(this).parent().attr("isBuy");
        if (isBuy != "True") {
            var txt = "由此进入<a href='courses.ashx' type='link' target='_top'>课程中心</a>";
            txt += "或<a href='selfcourse.ashx' type='link' target='_top'>我的课程</a>";
            txt = "请选择当前要学习的课程！<br/>" + txt;
            new MsgBox("提示", txt, 90, 200, "alert").Open();
        } else {
            document.location.href = $(this).attr("href");
        }
        return false;
    });
}