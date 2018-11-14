$(function () {
    $("a[type=review]").click(function () {
        var href = $(this).attr("href");
        var title = $(this).attr("title");
        new top.PageBox(title, href, 80, 80, null, window.name).Open();
        return false;
    });
});
