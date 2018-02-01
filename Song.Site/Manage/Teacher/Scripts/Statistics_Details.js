$(function(){
    //_initLoyout();
    _hideColumn("ID");
    _hideColumn("姓名");
    _hideColumn("身份证");
    $(".GridView").find("td").addClass("center");
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

//隐藏某一行
function _hideColumn(clName) {
    var gv=$(".GridView");
    var index;
    gv.find("th").each(function(i){
        if($.trim($(this).text())==clName) {
            index = i;
            //return false;
        }
    });
    gv.find("tr").each(function(){
        $(this).children().each(function(i){
            if(i==index) {
                $(this).hide();
                return false;
            }
        })
    })
}