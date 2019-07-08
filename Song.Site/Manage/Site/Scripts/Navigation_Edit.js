//初始化方法
$(function(){		
	navLoyoutInit();
	setTreeEvent();
	tmpInit();
	 _tmpPageListInit();
});

//初始化
function navLoyoutInit()
{
	//树形下拉菜单
	var ddlTree=$("select[id$=ddlTree]");
	//下拉菜单的选项
	var option=ddlTree.find("option");
	//当前id
	var id=$().getPara("id");	
	//当前导航项的父级id
	var pid=ddlTree.find("option[selected=selected]").attr("value");
	pid= typeof(pid)=="undefined" ? "0" : pid;
	ddlTree.attr("defPid",pid);	
	//
	_setChild(id,option);	
}
//设置当前导航以及下级导航不可以选择
function _setChild(currid,option)
{
	option.each(function(index, element) {
        if($(this).val()==currid)
		{
			$(this).attr("style","background-color: #cccccc;");
			$(this).attr("value",-1);
			//取子级
			option.each(function(){
				var pid=$(this).attr("pid");
				if(pid==currid){
					_setChild($(this).val(),option);
				}
			});
		}
    });
}
//选择父级导航更改时的事件
function setTreeEvent()
{
	//树形下拉菜单
	var ddlTree=$("select[id$=ddlTree]");
	var pid=ddlTree.attr("defPid");
	ddlTree.change( function() {
	  	var cid=$(this).attr("value");
		if(cid=="-1"){
			alert("请勿选择自身或自身的下级作为父级。");
			ddlTree.find("option").removeAttr("selected");
			ddlTree.find("option[value="+pid+"]").attr("selected","selected");
		}
	}); 
}

//模板选择时的一些操作事件
function tmpInit()
{
	//是否使用模板
	var cbIsTmp=$("input[id$='cbIsTmp']")
	//各行
	var type=$("#trTmpType");
	var col=$("#trColumns");
	var index=$("#trTmpIndex");
	var list=$("#trTmpList");
	var detl=$("#trTmpDetails");
	cbIsTmp.change(function(){
		var isTmp=Boolean($(this).prop("checked"));
		if(!isTmp)
		{
			type.hide();
			col.hide();
			index.hide();
			list.hide();
			detl.hide();
		}else{			
			type.show();
			col.show();
			index.show();
			list.show();
			detl.show();
		}
	});
	cbIsTmp.change();
	//模板类型
	var rbtype=$("input[name$='rblTmpType']");
	rbtype.click(function(){
		type.show();
		col.show();
		index.show();
		list.show();
		detl.show();
		switch($(this).val()){
			case "list":
				index.hide();
				break;
			case "details":
				index.hide();
				list.hide();
				break;
		}
	});
}
//模板页列表展示时的初始化
function _tmpPageListInit()
{
	//去除空白的模板
	$("#TmpBox dl").each(function(index, element) {
        if($(this).find("dd").size()<1)$(this).remove();
    });
	$("#TmpBox .TmpPageItem").each(function(index, element) {
        if($(this).find("dl").size()<1)$(this).remove();
    });
	//输入框事件
	$("input[name$='tbTmpIndex']").focus(_tmpShowPageList);
	$("input[name$='tbTmpList']").focus(_tmpShowPageList);
	$("input[name$='tbTmpDetails']").focus(_tmpShowPageList);
	//当点击模板时
	$(".tmpFile").click(function(){
		var file=$.trim($(this).text());
		window.tmpCurrTextbox.attr("value",file);
		$("#TmpBox").hide();
	});
	//关闭模板面板
	$(".tmpClose").click(function(){
		$("#TmpBox").hide();
	});
}
function _tmpShowPageList()
{
	window.tmpCurrTextbox=$(this);
	var tmpbox=$("#TmpBox");
	var off=$(this).offset();
	tmpbox.css("left",$(this).width()+off.left+5);
	tmpbox.css("top",off.top-303-$(this).height()/2);
	tmpbox.show();
}
