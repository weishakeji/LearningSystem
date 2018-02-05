/*!
 * 主  题：《管理界面的控制中心》
 * 说  明：管理界面console.aspx的控制，包括布局，功能设定等；
 * 功能描述：
 * 1、控制管理界面的布局，自动计算宽高，使其满屏；
 *
 * 作  者：宋雷鸣 
 * 开发时间: 2014年3月12日
 */
 
 //一些页面级参数
//浏览器窗体初始宽高
//window.OldWindowWidth=$(window).width();
//window.OldWindowHeight=$(window).height();
// 管理控制台的布局操作
$(function(){		
		//初始化界面
		loyoutInit();
		//showFullMask();
	    //页面右上方，生成系统菜单
		var drop=new DropMenu("#consDropMenu");
		drop.load();
		//页面左侧操作,生成功能菜单
		var tree=new TreePanel();
		tree.load();				
		//页面右侧操作 内容区的td		
		var context=$("#consContextPanel");
		var h=document.documentElement.clientHeight;
		var offset = $("#consBody").offset();
		var height=h-offset.top;		
		context.height(height);
		//打开起始页
		new PagePanel().DefOpen();	
		//页面主要按钮的事件
		btnEvent();
		//时实保持登录状态
		//Keeper();
		//setInterval("Keeper()",6000);		
		//判断链接
		//setInterval("checkConniction()",5000);	
		//隐藏遮罩层
		HideFullMask();		
		//var p=new PageBox("说明","Explain.htm",80,80,"Explain");	
		//p.Open();
});
//当窗口大变化时
$(window).resize(function(){
		window.windowResizeTemp=window.windowResizeTemp==null ? 0 : window.windowResizeTemp++;
		if(window.windowResizeTemp%2==0)reLoyout();
}); 
//初始化布局
function loyoutInit(){
	//窗体的内容宽高
	var winHeight=document.documentElement.clientHeight;
	var winWidth =document.documentElement.clientWidth;	
	//主要对象
	var top=$("#consPageTop");//页面顶部
	var bar=$("#consBoxTitleBar");//选项卡区域
	var cbody=$("#consBody");//内容区域
	var left=$("#consTreePanel");//内容区域的左侧
	var right=$("#consContextPanel");//内容区域的右侧
	//设置选项卡
	bar.width(winWidth-left.width());
	bar.css({left:left.width(),top:25});
	//主内容区高度
	cbody.height(winHeight-top.outerHeight()-Number(top.css("margin-bottom").replace("px","")));
	//右则内容宽度，去除边距
	right.width(winWidth-left.width()-Number(right.css("margin-left").replace("px","")));
}
//当窗体变动时，重新布局
function reLoyout(){
	//窗体的内容宽高
	var winHeight=document.documentElement.clientHeight;
	var winWidth =document.documentElement.clientWidth;	
	//主要对象
	var top=$("#consPageTop");//页面顶部
	var bar=$("#consBoxTitleBar");//选项卡区域
	var cbody=$("#consBody");//内容区域
	var left=$("#consTreePanel");//内容区域的左侧
	var right=$("#consContextPanel");//内容区域的右侧
	//设置选项卡
	bar.width(winWidth-left.width());
	bar.css({left:left.width(),top:25});
	//主内容区高度
	cbody.height(winHeight-top.outerHeight()-Number(top.css("margin-bottom").replace("px","")));
	//右则内容宽度，去除边距
	right.width(winWidth-left.width()-Number(right.css("margin-left").replace("px","")));	
	right.height(cbody.height());
	////左侧树形菜单的面板高度设置
	left.height(cbody.height());
	$(".treepanel").height(cbody.height());
	//管理内容页区的高度设置
	$(".consFramePanel").each(function(){
			var iframe=$(this).find("iframe");
			var topbar=$(this).find(".topBar");
			var state=$(this).attr("state");
			//如果不是全屏显示的，仅作为面板显示时
			if(state=="panel")
			{
				$(this).height(right.height());
				$(this).width(right.width());
			    var h=right.height()-topbar.height();
				iframe.height(h);
				iframe.width(right.width());
			}
			//当全屏显示时
			if(state=="fullscreen")
			{				
				$(this).height(winHeight);
				$(this).width(winWidth);
				var h=winHeight-topbar.height();
				iframe.height(h);
				iframe.width(winWidth);
				//iframe.width(winWidth);
				var span=$(this).find(".topBar .pagePanelMaxHand");	
				span.attr("w",right.width());
	    		span.attr("h",right.height());	
			}
	});	
	//弹出窗体
	new PageBox().OnReSize();
}
//按钮事件
function btnEvent(){
	//当前登录员工的名称，用于打开个人信息查看
	$("#lbName").click(function(){
			var name=$(this).text();
			var link=$(this).attr("href");
			new PageBox("尊敬的 "+name +" 您的个人信息",link,400,300).Open();
			return false;
	});
	//短消息的按钮
	$("#messageMarker").click(function(){
			var menu=$("#consTreePanel");
			var node=menu.find("div[title='短消息']:first");
			var link=node.attr("href");	
			var id=node.attr("nodeId");
			var name=node.text();
			var path=node.attr("path");	
			var p=new PagePanel();
			p.add(name,path,link,id);
			return false;
	});
}
//保持登录状态，时实向服务器提交当前用户的在线时间
function Keeper(){
	var urlPath="/manage/Panel/Keeper.aspx?id="+emplyeeId+"&timestamp=" + new Date().getTime();
	$.ajax({
			  type: "Get",
			  url: urlPath,
			  dataType: "text",
			  data:{},
			  //开始，进行预载
			  beforeSend: function(XMLHttpRequest, textStatus){	
			  		
				},
				//加载出错
			 error: function(XMLHttpRequest, textStatus, errorThrown){			 		
				},
				//加载成功！
			  success: function(data) {
				  //不等于0，表示登录失效
				  if(data!="0")
				  {
					  var p=new PageBox("重新登录","reLogin.aspx",300,225,"reLogin");
					  p.Open();
					  p.HideClose();
				  }				  
			  }
		  }); 	
}
//隐藏遮罩层
function HideFullMask(){	
	var mask=$("#consFullMask");	
	mask.fadeOut(3000);
}
//打开一个pagepanel窗口
//通过menuid检索左侧树形菜单，找到并打开
//menuName:菜单项名称
//menuId:菜单项id
function OpenPagePanel(menuName,menuId){	
	var menu=$("#consTreePanel");
	menuName=$.trim(menuName);
	var node=menu.find("div[name='"+menuName+"'][nodeid='"+menuId+"'][nodetype='item']:first");
	if(node.size()<1)
	{
		node=menu.find("div[name='"+menuName+"']:first");
		if(node.size()<1)
		{
			alert("当前工作项："+menuName+" 不存在。");
			return;
		}
	}
	var link=node.attr("href");	
	var id=node.attr("nodeId");
	var name=node.text();
	var path=node.attr("path");	
	var p=new PagePanel();
	p.add(name,path,link,id);	
}


//保持登录状态，时实向服务器提交当前用户的在线时间
function checkConniction(){
	var urlPath="/manage/Panel/1.htm"+"?timestamp=" + new Date().getTime();
	$.ajax({
			  type: "Get",
			  url: urlPath,
			  dataType: "text",
			  data:{},
			  //开始，进行预载
			  beforeSend: function(XMLHttpRequest, textStatus){	
			  		
				},
				//加载出错
			 error: function(XMLHttpRequest, textStatus, errorThrown){			 	
			 		connMask();
				},
				//加载成功！
			  success: function(data) {
				  removeConnMask();			  
			  }
		  }); 	
}
//去除遮罩
function removeConnMask(){
	var mask=$("#ConnictionscreenMask");
	mask.remove();
	$("#showConnictionscreenBreek").remove();
}
//生成遮罩层
function connMask(){	
	var mask=$("#ConnictionscreenMask");
	if(mask.size()>0)return;	
	var html="<div id=\"ConnictionscreenMask\"/>";
	$("body").append(html);
	mask=$("#ConnictionscreenMask");
	mask.css("position","absolute");
	mask.css("z-index","200000");
	mask.css("width",$("body").width());
	mask.css("height",$("body").height());	
	mask.css("top",0);
	mask.css("left",0);
	mask.css("background-color","#ffffff");
	mask.css("filter","Alpha(Opacity=60)");
	mask.css("display","block");
	mask.css("-moz-opacity",0.6);
	mask.css("opacity",0.6);
	mask.fadeIn("slow");
	showBreek();
}
//显示网络中断
function showBreek(){
	var mask=$("#ConnictionscreenMask");
	var html="<div id=\"showConnictionscreenBreek\"/>";
	$("body").append(html);
	var show=$("#showConnictionscreenBreek");
	show.hide();
	var str="连接中断<br/><br/>";
	str+="连通后将自动取消锁定……";
	show.html(str);
	mask.css("filter","Alpha(Opacity=60)");
	mask.css("display","block");
	mask.css("-moz-opacity",0.6);
	mask.css("opacity",0.6);
	show.fadeIn("slow");
}