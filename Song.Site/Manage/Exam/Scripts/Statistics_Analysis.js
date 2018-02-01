$(function(){
    _initLoyout();
 });

//初始部署
function _initLoyout(){
    var gv=$(".GridView");
   if(gv.size()<1) {
        $(".pageWinContext div").text("当前考试没有任何人参加！");
   }else{
       gv.find("tr").find("th:first").hide();
       gv.find("tr").find("td:first").remove();
       gv.find("tr").find("td:first").width(80);
       gv.find("td").addClass("center");
   }
}