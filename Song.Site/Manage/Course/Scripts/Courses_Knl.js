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
    var h = document.documentElement.clientHeight;
    var w = document.documentElement.clientWidth;
    //左侧
    $("#leftBox").height($("#editLeft").height());
    $("#leftBox iframe").height($("#editLeft").height());
    var listBox = $("#leftBox select");
    listBox.height(h - 30);
    //右侧
    var right = $("#rightBox");
    //right.width(w - $("#leftBox").width() - 25);
    var iframe = right.find("iframe");
    iframe.height(h - 40);
    //iframe.width(w - $("#leftBox").width() - 25);
}

//界面初始设置
function _contentsInit(){
	//事件
	$("#leftBox select").change(function(){
		var curr=$(this).find("option:selected");
		var id=$(this).attr("value");
		$().pagecookie("curr",id);
		_goIframeFile(curr);
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
	var knsid=option.attr("value");
	var type=option.attr("type");
	var action=$().getPara("action");
	//var url=type+".aspx?colid="+colid+"&type="+type+"&action="+action+"&timestamp=" + new Date().getTime();
	var url = "KnlContent.aspx?knsid=" + knsid + "&timestamp=" + new Date().getTime();
	//
	var iframe=$("#rightBox iframe");
	iframe.attr("src",url);
}
