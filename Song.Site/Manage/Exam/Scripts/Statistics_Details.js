$(function(){
    _initLoyout();
    $(".examAvg").text(_clacAvg());
});
//计算平均分
function _clacAvg(){
    var tm=0;
    var span=$(".examItem span");
    span.each(function(){
        var num=Number($(this).text());
        tm+=num;
    });
    var avg=tm/span.size();
    return Math.round(avg*100)/100;
}
//初始部署
function _initLoyout(){
    var gv=$(".GridView");
    gv.find("tr").find("th:first").hide();
    gv.find("tr").find("td:first").remove();
    gv.find("tr").find("td:first").width(80);
    gv.find("td").addClass("center");
}