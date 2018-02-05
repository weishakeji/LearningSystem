$(function(){
    selectedInit();
    subjectEvent();
    btnEvent();
});
//专业的选择按钮
function subjectEvent(){
    $(".subjectItem a").click(function(){
        var sbjid=$(this).attr("sbjid");
        var url=buildUrl(sbjid,1);
        window.document.location.href=url;
        return false;
    });
}
//生成地址
function buildUrl(sbjid,index){
    //当前页面的上级路径，因为子页面没有写路径，默认与本页面同路径
    var url=String(window.document.location.href);
    if(url.indexOf("?")>-1)
        url=url.substring(0,url.lastIndexOf("?"));
    url+="?sbj="+sbjid+"&index="+index;
    return url;
}

//初始化
function selectedInit(){
    var sbjid=parseInt($().getPara("sbj"));
    if (sbjid < 1 || isNaN(sbjid)) {
        $("#SelectedBar").hide();
        return;
    }
    var sbjName=$(".subjectItem a[sbjid="+sbjid+"]").text();
    var tm="<div class=\"selectedItem\"><div class='name'>"+sbjName+"</div><div class='close'>&nbsp;</div></div> ";
    $("#Selected").html(tm);
    //选中的专业在鼠标经过时
    $("#Selected .selectedItem").hover(function(){
        $(this).addClass("selectedItemOver");
        $(this).find(".close").addClass("closeOver");
    },function(){
        $(this).removeClass("selectedItemOver");
        $(this).find(".close").removeClass("closeOver");
    });
    $("#Selected .close").click(function(){
        var url=buildUrl(0,1);
        window.document.location.href=url;
    });
}

//点击进入测试的按钮事件
function btnEvent() {
    $("a.inTestScene").click(function () {
        var url = $(this).attr("href");
        //当前学员是否通过审核
        var isPass = $(this).attr("isPass") == "False" ? false : true;
        if (!isPass) {
            var txt = "您还没有通过审核，无法进行模拟测试。<br/>如果您确认已经通过审核，请重新登录。";
            var msg = new MsgBox("学员未通过审核", txt, 400, 200, "alert");
            msg.Open();
            return false;
        }
    });
}