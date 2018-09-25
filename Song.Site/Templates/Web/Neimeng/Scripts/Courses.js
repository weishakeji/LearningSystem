$(function(){
    selectedInit();
    subjectEvent();
    courseOver();
	//搜索以及分页导航
	$("input[name=search]").val(decodeURI($().getPara("search")));
	var paras=$().getPara();
	if(paras!=null && paras.length>0){
		$("#pagerBar a").each(function(index, element) {
			var href=$(this).attr("href");
            for(var i=0;i<paras.length;i++){
				href=$().setPara(href,paras[i].key,paras[i].value);
			}
			$(this).attr("href",href);
        });
	}
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
    url+="?sbjid="+sbjid+"&index="+index;
    return url;
}

//初始化
function selectedInit(){
    var sbjid=parseInt($().getPara("sbjid"));
    if(sbjid<1 || isNaN(sbjid)){
        $("#SelectedBar").hide();
        return;
    }
    var sbjName=$("#Selected").attr("sbjname");
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
//当课程信息，鼠标滑过时
function courseOver(){
    $(".courseList  .item").hover(function(){
        $(this).addClass("courseOver");
        $(this).find(".itemMark").animate({top:0,height:123});
        $(this).find(".itemInfo").animate({top:0,height:123});
    },function(){
        $(this).removeClass("courseOver");
        $(this).find(".itemMark").animate({top:83,height:40});
        $(this).find(".itemInfo").animate({top:123-40,height:40});
    });
}