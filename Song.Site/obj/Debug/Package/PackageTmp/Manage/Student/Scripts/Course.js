$(function () {
    $(".item img").click(function () {
        var thisEle = $(this).parent();
        var id = thisEle.attr("couid");
        var width = parseInt(thisEle.attr("wd"));
        var height = parseInt(thisEle.attr("hg"));
        //
        //当前页面的上级路径，因为子页面没有写路径，默认与本页面同路径
        var url = String(window.document.location.href);
        url = url.substring(0, url.lastIndexOf("/") + 1);
        var boxUrl = url + "Course_View.aspx?id=" + id;
        new top.PageBox("课程信息预览", boxUrl, width, height).Open();
    });
    //取消课程学习的按钮事件
    $(".box a.selected").click(function () {
        if (confirm("是否确定中止该课程的学习？")) {
            return true;
        } else {
            return false;
        }
    });
});