$(function () {
    setStar();
});

//设置星标
function setStar() {    
    $("span.star").each(function (index, element) {
        //显示多少个星标
        var size = Number($(this).attr("size"));
        if (isNaN(size)) size = 5;
        //当前得分
        var score = Number($(this).prev().text());
        if (isNaN(score)) return true;
        score = score / (10 / size);
        //生成空的星标
        while (size-- > 0) $(this).append("<i></i>");
        //生成实的星标
        var tm = Math.floor(score);
        $(this).find("i:lt(" + tm + ")").addClass("s1");
        //生成半实的星标
        if (score > tm) $(this).find("i:eq(" + tm + ")").addClass("s0");
        $(this).append("<b>" + $(this).prev().text() + "分</b>");
    });
}