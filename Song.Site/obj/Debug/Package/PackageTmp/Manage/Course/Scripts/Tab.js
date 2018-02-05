$(function(){
		_tabInit();
		_setInitTab();
});
//
function _tabInit()
{
	//初始化布局
	var context=$("#tabContextBox");
	var hg=context.parent().parent().height();
	var wd=context.parent().parent().width();
	var mag = context.parent().css("margin-top");
	if (mag != null) mag = mag.replace("px", "");
	//alert(mag);
	hg=hg-$("#tabTitleBox").height()-Number(mag)*2-28;
	//context.height(hg);	
	context.width($("#tabTitleBox").width()-5);
    //$("#tabTitleBox").width(context.width());
	//设置当前选中选项卡
	$("#tabTitleBox .titItem").click(function(){
		_setTab($(this));
	});
}
//设置默认的选项卡
function _setInitTab()
{
	var curr=$().pagecookie("currTab");
	var currTab=$("#tabTitleBox .titItem[tab='"+curr+"']");
	if(currTab.size()>0)
	{
		_setTab(currTab);
	}else{
		//选项卡标签
		var first=$("#tabTitleBox .titItem:first");
		_setTab(first);
	}
}
//设置当前选项目产
function _setTab(currTab)
{
	//当前选项，保存到cookies
	$().pagecookie("currTab",currTab.attr("tab"));
	//
	$("#tabTitleBox .titItem").removeClass("curr");
	$("#tabContextBox .contItem").hide();
	currTab.addClass("curr");
	var tag=currTab.attr("tab");
	$("#tabContextBox .contItem[tab='"+tag+"']").show();
}
