$(function () {
    //点击进入课程编辑
    $("#cour_add").click(function () {
        var href = $(this).attr("href");
        var width = Number($(this).attr("wd"));
        var height = Number($(this).attr("hg"));
        var box = new top.PageBox("编辑学习卡的课程", href, width, height, null, window.name);
        box.CloseEvent = function () {
            //var courses = $.cookie("card_add");
            //alert(courses[0].id);
        }
        box.Open();
        return false;
    });
    //初始化课程列表
    var courses = new Array();
    $("b[couid]").each(function (index) {
        var obj = { id: $(this).attr("couid"), name: $(this).text() };
        courses[index] = obj;
    });
    setCourse(courses);
    //确定按钮
    $("input[name$=btnEnter]").click(function () {
        if ($("dl.courses dd").size() < 1) {
            window.Verify.ShowBox($(".noCourse"), "至少要有一个课程关联");
            return false;
        }
    });
});


//设置选中的课程
//courses:课程数组，对象属性（id、name）
function setCourse(courses) {
    var dl = $("dl.courses");
    dl.find("dd").remove();
    //判断是否有重复
    var isExist = false;    
    for (var i = 0; i < courses.length; i++) {
        var isExist = false;
        dl.find("dd").each(function () {
            var couid = $(this).attr("couid");
            if (courses[i].id == Number(couid)) {
                set($(this), courses[i].name);
                isExist = true;
                return false;
            }
        });
        if (!isExist) add(dl, courses[i].id, courses[i].name);
    }
    var couids="";
    dl.find("dd").each(function (index) {
        $(this).find("span:first").text(index + 1);
        couids += $(this).attr("couid")+",";
    });
    dl.find("dd").size() < 1 ? $(".noCourse").show() : $(".noCourse").hide();
    $("input[name$=tbCourses]").val(couids);
    //添加课程
    function add(dl, couid, name) {
        dl.append("<dd couid='" + couid + "'><span>0</span>、<span class='name'>" + name + "</span></dd>");
    }
    function set(dd, name) {
        $(this).find(".name").text(name);
    }
}
//获取课程
//return: 课程数组，对象属性（id、name）
function getCourses() {
    //写入选修的课程到cookies
    var courses = new Array()
    $("dl.courses dd").each(function (index) {
        var obj = { id: $(this).attr("couid"), name: $(this).find(".name").text() };
        courses[index] = obj;
    });
    return courses;
}