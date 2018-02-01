$(function(){
	_contentsLoyout();
	_contentsInit();
	_contentsInitSelected();	
});
//当窗口大变化时
$(window).resize(
	function(){
		window.windowResizeTemp=window.windowResizeTemp==null ? 0 : window.windowResizeTemp++;
		if(window.windowResizeTemp%2==0)_contentsLoyout();
	}
); 
//界面始初化布局
function _contentsLoyout(){
	var h=document.documentElement.clientHeight;
    var w=$("form").width();
	//左侧
	var listBox=$("#leftBox select");	
	listBox.height(h-5);
	//右侧
	var right=$("#rightBox");
	right.width(w-$("#leftBox").width()-10);
	var iframe=right.find("iframe");
	iframe.height(h-10);
	iframe.width(w-$("#leftBox").width()-10);
}

//界面初始设置
function _contentsInit(){
	var node=$("#leftBox select option");	
	node.each(function(){
		//$(this).text(_getTypeName(type)+" "+$(this).text());
	});
	//事件
	$("#leftBox select").change(function(){
		var curr=$(this).find("option:selected");
		var id=$(this).attr("value");
		$().pagecookie("curr",id);
		_goIframeFile(curr);
	});
	$("#leftBox select").dblclick(function(){
		var curr=$(this).find("option:selected");
		var id=$(this).attr("value");
		//当前页面的上级路径，因为子页面没有写路径，默认与本页面同路径
		var url=String(window.document.location.href);		
		url=url.substring(0,url.lastIndexOf("/")+1); 
		var boxUrl=url+"List_Edit.aspx?id="+id;
		new top.PageBox("课程信息编辑",boxUrl,800,600).Open();
	});
}
function _contentsInitSelected(){
	//获取上次的记录
	var currid=$().pagecookie("curr");
	if(currid!=null )
	{
		var curr=$("#leftBox select option[value="+currid+"]");
		if(curr.size()>0)
		{
			_goIframeFile(curr);
			return;
		}
	}
	var sel=$("#leftBox select");
	var node=sel.find("option");	
	node.each(function(){
		var id=$(this).val();		
		var isChild=false;
		node.each(function(){
			var pid=$(this).attr("pid");
			if(id==pid)
			{
				isChild=true;
				return false;
			}
		});
		if(!isChild)
		{			
			_goIframeFile($(this));
			return false;
		}
	});
}
//在Iframe中打开
function _goIframeFile(option){
	option.attr("selected","selected");
	var couid=option.attr("value");
	var type=option.attr("type");
	var action=$().getPara("action");
	//var url=type+".aspx?colid="+colid+"&type="+type+"&action="+action+"&timestamp=" + new Date().getTime();
	var url="Outline.aspx?couid="+couid+"&timestamp=" + new Date().getTime();
	//
	var iframe=$("#rightBox iframe");
	iframe.attr("src",url);
}
