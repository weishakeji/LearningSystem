$(function () {
    //添加课程到选择列表
    $("a[href=select]").click(function () {
        var couid = $(this).attr("couid");
        if (couid == "all") {
            $("a[href=select][title]").each(function () {
                var couid = $(this).attr("couid");
                var name = $(this).attr("title");
                addCourse(couid, name);
            });
        } else {
            var name = $(this).attr("title");
            addCourse(couid, name);
        }
        return false;
    });
    //清空
    $("dt span.clear").click(function () {
        $("dl.courses dd").remove();
    });
    //确定按钮事件
    $("input[name$=btnEnter]").click(function () {
        //写入选修的课程到cookies
        var courses = new Array()
        $("dl.courses dd").each(function (index) {
            var obj = { id: $(this).attr("couid"), name: $(this).find(".name").text() };
            courses[index] = obj;
            //courses[index].name = $(this).find(".name").text();
        });
        $.cookie("card_add", courses);
        //关闭窗口
        new top.PageBox().Close(window.name);
    });
});

//添加选中的课程
function addCourse(couid, name) {
    //判断是否有重复
    var isExist = false;
    var dl = $("dl.courses");
    dl.find("dd").each(function () {
        var id = $(this).attr("couid");
        if (id == couid) {
            $(this).find(".name").text(name);
            $(this).fadeOut().fadeIn();
            isExist = true;
            return false;
        }
    });
    if (!isExist) {
        var num = dl.find("dd").size();
        dl.append("<dd couid='" + couid + "'><span>" + (num + 1) + "</span>、<span class='name'>" + name + "</span><span class='remove'>移除</span></dd>");
        //移去课程
        dl.find(".remove:last").click(function () {
            var dd = $(this).parent();
            dd.remove();
            dl.find("dd").each(function (index) {
                $(this).find("span:first").text(index+1);
            });
        });
    }
}