/*!
 * 主  题：《下拉菜单生成》
 * 说  明：用于管理界面右上方的下拉菜单。
 * 功能描述：
 * 1、模仿桌面应用软件下拉菜单效果；
 * 2、能够应用屏幕，当右侧空间不够时，下拉菜单向左展开
 * 3、数源来源自/Static/SysMenuTree.html静态化数据
 *
 * 作  者：宋雷鸣 
 * 开发时间: 2012年2月13日
 */
 
 
//页面左侧树形菜单的面板
//area:下拉菜单所处在页面div区域
function DropMenu(area)
{
	if(area!="" && area!=null)DropMenu.Area=area;
	DropMenu.ZIndex=4000;
}
//菜单停留时间
DropMenu.MenuWaitTime=100;
//判断鼠标是否处在菜单之上
window.IsOnDropMenu=true;    
function closeDropMenu()
{
	if(!window.IsOnDropMenu)
	{
		var d=new DropMenu();
		d.PanelHide();		
	}	
}
//载入数据
DropMenu.prototype.load=function()
{		
	//$().SoapAjax("managemenu","DropMenu",{},BuildDropMenu,DropMenu.Loading,DropMenu.UnLoading);
	$.get("panel/SysMenuTree.aspx", function(data){
	  var area=$(DropMenu.Area);
		if(area.size()<1)return;
		var drop=new DropMenu();
		var tmp="";
		//
		area.html(data);
		drop.MenuStyleInit();
		drop.MenuStyleReset();
		drop.Event();
	}); 
}

//菜单样式初始化
DropMenu.prototype.MenuStyleInit=function()
{
	var area=$(DropMenu.Area);
	area.find("[type='menuPanel']").each(
			function()
			{
				var level=Number($(this).attr("level"));
				//顶部菜单
				if(level==0)
				{
					$(this).attr("class","rootPanel").attr("type","rootPanel");
					$(this).children().attr("class","rootMenu");
					$(this).show();	
				}else
				{
					$(this).attr("class","menuPanel");
					//alert($(this).attr("class"));
					$(this).children().attr("class","MenuNode");
					$(this).children().each(
						function()
						{
							//如果子菜单项有下拉菜单，显示黑色的小三角
							var isChild=$(this).attr("isChild");
							var style="background-image: url(/manage/images/tree/triangle.gif);";
							style+="background-repeat: no-repeat;";
							style+="background-position: "+($(this).parent().width()-8)+"px 5px;";
							if(isChild=="True")$(this).attr("style",style);
						}
					);	
					$(this).hide();
				}
			}
		);
}
//菜单样式的重新赋值，主要是ie6下,ajax载入入不能完全刷新css
DropMenu.prototype.MenuStyleReset=function()
{
}
//下拉菜单相关事件
DropMenu.prototype.Event=function()
{
	var area=$(DropMenu.Area);	
	//菜单事件
	area.find("div[type='MenuNode']").hover(
			function()
			{
				var d=new DropMenu();
				d.PanelShow($(this));	
				d.MouseOverShow($(this));
			},function(){}
		);
	//当鼠标移动时
	$("body").mousemove(
		function(e)
		{	
			//判断鼠标是否处于菜单之上
			var tmp=$().isMouseArea($("div[type$='Panel']"),e);			
			window.IsOnDropMenu=tmp;
			if(!tmp)setTimeout("closeDropMenu()",DropMenu.MenuWaitTime); 
		}
	);
	//菜单项,点击事件
	area.find("div[type='MenuNode']").click(
			function()
			{		
			    var node=$(this).find("span");
				//如果当前节点有子节点，则不响应事件
				if(node.attr("IsChild")=="True")return false;
				//如果该节点禁用，则不响应事件
				if(node.attr("IsUse")=="false")return false;
				var type=node.parent().attr("nodetype");
				var link=node.attr("href");	
				var id=node.parent().attr("nodeId");
				var name=node.text();
				var path=node.attr("path");	
				
				if(path!=null && path.indexOf(",")>-1)
				{
					//path=path.substring(path.indexOf(",")+1);
				}				
				switch(type)
				{
					case "link"://如果是外部链接，直接打开
						return true;
					case "open"://如果是弹出窗口
						var width=node.attr("width");	
						width=width ? width : 400;
						var height=node.attr("height");	
						height=height ? height : 300;
						var title=node.parent().attr("title");
						new PageBox(title,link,Number(width),Number(height)).Open();
						return false;
					case "event"://如果是javascript事件
						eval(link);
						return false;
					case "item":
						var p=new PagePanel();
						p.add(name,path,link,id);
						return false;
				}				
			}
		);
}
//鼠标滑过事件
//current:鼠标正在滑过的菜单项
DropMenu.prototype.MouseOverShow=function(current)
{
	var parent=current.parent();
	//面板级别
	var level=Number(parent.attr("level"));
	//如果是根菜单
	if(level==0)
	{
		current.siblings().attr("class","rootMenu");
		current.attr("class","rootMenu rootMenuOver");
	}else
	{
		current.siblings().attr("class","MenuNode");
		current.attr("class","MenuNode MenuNodeOver");
	}
}
//下拉菜单的显示
//pmenu:面板的上级菜单，即正在点中的菜单项
DropMenu.prototype.PanelShow=function(pmenu)
{
	this.PanelHide(pmenu.parent());
	var area=$(DropMenu.Area);
	//面板级别
	var level=Number(pmenu.parent().attr("level"));
	var nodeid=pmenu.attr("nodeId");
	//自身坐标对象
	var off=pmenu.offset();
	//子菜单面板
	var panel=area.find("div[patId="+nodeid+"]");
	if(panel.size()<1)return;
	var w=document.documentElement.clientWidth;
	//panel.css("background-color","#FFFFCC");
	//panel.css("border","1px solid #666666");
	panel.css("width","100px");
	panel.children().css("width","100px");
	//如果是根菜单
	if(level==0)
	{		
		panel.css("top",off.top+pmenu.height());		
		if((off.left+panel.width())<w)
		{
			panel.css("left",off.left);
		}else
		{
			panel.css("left",off.left+pmenu.width()-panel.width()+5);
		}
		panel.show();
		return;
	}
	//如果是其它菜单
	panel.css("top",off.top+pmenu.height()/2);	
	if((off.left+pmenu.width()+panel.width())>w)
	{
		panel.css("left",off.left-panel.width()+5);
	}else
	{
		panel.css("left",off.left+pmenu.width()-5);
	}	
	panel.show();
}
//下拉菜单面板的隐藏
//panel:上级菜单
DropMenu.prototype.PanelHide=function(panel)
{
	//如果参数为空，则隐藏所有下拉菜单面板
	if(panel==null)
	{
		$(DropMenu.Area).find("div[type$='Panel']").each(
				function()
				{
					var level=Number($(this).attr("level"));
					$(this).children().attr("class",(level==0) ? "rootMenu" : "MenuNode");
					if(level!=0)$(this).hide();
				}
			);	
		return;
	}
	//上级菜单的同级菜单
	panel.find("div[isChild=True]").each(
			function()
			{
				var nodeid=$(this).attr("nodeId");
				//同级子面板隐藏
				var child=$(DropMenu.Area).find("div[patId="+nodeid+"]");	
				child.children().attr("class","MenuNode");
				child.hide();				
				var d=new DropMenu();
				d.PanelHide(child);				
			}
		);
}
//获取当前坐标下的节点
//return:返回树形节点的页面元素
DropMenu.getDragObj=function(e)
{
	var mouse=$().Mouse(e);
	var x=mouse.x;
	var y=mouse.y
	var tmp=null;	
	$("div[type='menuPanel']").each(
			function()
			{
				var offset = $(this).offset();
				var xt = x > offset.left && x < (offset.left + $(this).width());
				var yt = y > offset.top && y < (offset.top +$(this).height());				
				if(xt && yt)
				{
					if($(this).attr("state")!="lock")tmp=$(this);
				}
			}
		);
	return tmp;
}