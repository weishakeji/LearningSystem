//修改数据的事件
//是否进入拖动
var isDrag=false;



//生成管理树
function Tree(area)
{
	if(area!=null)Tree.Area=$(area);
	
	if(Tree.InitNumber>0)return;
	//重新定义ico
	for(var t in this.ico)
	{
		this.ico[t]="<img src=\""+this.icoPath+this.ico[t]+"\" />";
	}
	Tree.InitNumber++;
}
//初始变量，每实例化一次，加一
Tree.InitNumber=0;
//树形菜单所在区域
Tree.Area="";
//树形菜单的根节点id;
Tree.RootId=0;
//根节点，点击事件
Tree.RootClick=null;
//其余节点的点击事件
Tree.NodeClick=null;
//排序事件，当更改节点顺序后
Tree.onChangeOrder=null;
//删除事件，当删除节点后；
Tree.onDelNode=null
//图标的路径
Tree.prototype.icoPath="/Manage/Images/tree/";
//图标
Tree.prototype.ico = {
		root				: 'root.gif',
		folder			    : 'folder.gif',
		folderOpen	        : 'folderopen.gif',
		page				: 'page.gif',
		empty				: 'empty.gif',
		line				: 'line.gif',
		join				: 'join.gif',
		joinBottom	        : 'joinbottom.gif',
		plus				: 'plus.gif',
		plusBottom	        : 'plusbottom.gif',
		minus				: 'minus.gif',
		minusBottom	        : 'minusbottom.gif',
		nlPlus			    : 'nlPlus.gif',
		nlMinus			    : 'nlMinus.gif',
		add					: 'add.gif',
		reline				: 'reline.gif'
};
//开始生成树形菜单
Tree.prototype.BuildMenu=function(data)
{
	//var rootHtml=BuildTree(data);
	var rootHtml=this.buildItem(null,data);
	Tree.Area.html(rootHtml); 
	//去除事件
	$("body").unbind("mousedown");
	$("body").unbind("mousemove");
	$("body").unbind("mouseup");
	//赋加事件
	this.TreeEvent();	
	this.DragEvent();

}
//生成菜单
//node:数据源的节点；
//data:完整的数据源，基于soap
Tree.prototype.buildItem=function(node,data)
{
	//当前节点对象
	var n=new Node(node,data);
	//开始生成
	var temp="<div style=\"float: none;width:100%\" type=\"nodePanel\" panelId=\""+n.Id+"\">";
	temp+="<div type=\"nodeline\" style=\"float: none;width:100%;display: table;\" >";
	//节点前的图标区域//树的连线与图标
	temp+="<div style='width:auto;float:left;' state='"+n.State+"' ";
	temp+="type='nodeIco' ";
	temp+="IsChilds='"+(n.IsChilds ? "True" : "False")+"'>";
	temp+=this.nodeLine(n,data)+this.nodeIco(n,data);
	temp+="</div>";
	//菜单项文本
	temp+=this.BuildNode(n,data);
	temp+="</div>";	
	if(n.IsChilds)
	{
		temp+="<div type='childPanel' style='display:"+(n.State ? "" : "none")+"'>";
		for(var i=0;i<n.Childs.length;i++) temp+=this.buildItem(n.Childs[i],data);
		temp+="</div>";
	}
	temp+="</div>";
	return temp;
}
//生成节点项文本
Tree.prototype.BuildNode=function(node,data)
{
	var temp="<div style=\"width:auto;line-height: 18px;display: table;float:left;	font-size: 12px;cursor: default;\"";
	temp+="title='"+node.Intro+"'";
	temp+=" nodeId='"+node.Id+"' text=\""+node.Name+"\"";
	temp+=" tax='"+node.Tax+"' patId=\""+node.PatId+"\" ";
	temp+=" type='"+(node.Parent==null ? "rootText" : "text")+"'>";
	//菜单节点的自定义样式
	var style="";
	if(node.Color!="") style+="color: "+node.Color+";";
	if(node.IsBold) style+="font-weight: bold;";
	if(node.IsItalic) style+="font-style: italic;";
	if(node.Font!="") style+="font-family: '"+node.Font+"';";	
	temp+="<span style=\""+style+"\">"+node.Name+"</span>";	
	//如果当前节点显示状态为false
	if(!node.IsShow)
	{
		temp+="  <span style=\"color:red\" title=\"该节点项实际使用中将不显示\">[隐]</span>";	
	}
	if(!node.IsUse)
	{
		temp+="  <span style=\"color:red\" title=\"该节点项在使用中将不响应任何事件\">[禁]</span>";	
	}
	temp+="</div>";
	return temp;
}
//生成菜单项前的第一个节点，即链接
//node:当前节点
Tree.prototype.nodeLine=function(node,data)
{
	var temp="";	
	if(node.Parent !=null)
	{
		var p=new Node(node.Parent,data);	
		while(p.Parent !=null)
		{
			//如果是最后一个节点
			if(p.IsLast) temp=this.ico.empty+temp;
			else temp=this.ico.line+temp;
			//当前节点的上级节点		
			p=new Node(p.Parent,data);
		}
	}
	return temp;
}
//菜单项第二个子节点，用于显示加减号与菜单项图标
//n:当前节点
Tree.prototype.nodeIco=function(n,data)
{
	var temp="";
	//如果是顶级，则返回根图标
	if(n.Parent==null)return this.ico.root;
	//如果有子节点
	if(n.IsChilds)
	{
		//最后一个节点,且为打开状态
		if(n.IsLast && n.State)temp+=this.ico.minusBottom+this.ico.folderOpen;
		//最后一个节点，状态为关闭
		if(n.IsLast && !n.State)temp+=this.ico.plusBottom+this.ico.folder;
		//不是最后一个节点，状态为打开
		if(!n.IsLast && n.State)temp+=this.ico.minus+this.ico.folderOpen;	
		//不是最后一个节点，状态为关闭
		if(!n.IsLast && !n.State)temp+=this.ico.plus+this.ico.folder;
	}else
	{
		//如果没有子节点，且为当前级别最后一个节点
		if(n.IsLast)temp+=this.ico.joinBottom+this.ico.page;
		//没有子节点，不是当前级别，最后一个
		if(!n.IsLast)temp+=this.ico.join+this.ico.page;
	}
	return temp;
}
//处理点前图片的替换效果
//element:节点的元素
//img1:原图片
//img2:新图片
Tree.prototype.tranImg=function(el,img1,img2)
{
	if(el.attr("src").indexOf(img1)>0)	
		el.attr("src",el.attr("src").replace(img1,img2));
	else	
		if(el.attr("src").indexOf(img2)>0)	
			el.attr("src",el.attr("src").replace(img2,img1));		
}
//树形菜单事件，如打开折叠等
Tree.prototype.TreeEvent=function()
{
	var tree=new Tree();
	//当点击节点前的图标时，如加减号文件夹图标
	$("div[type='text']").prev("div[IsChilds='True']").click(
		function()
		{			
			//栏目图标，文件夹或文件图标
			var last=$(this).find("img:last");
			//节点图标，加号或┝号
			var pre=last.prev();
			tree.tranImg(pre,"plus.gif","minus.gif");
			tree.tranImg(pre,"minusbottom.gif","plusbottom.gif");
			tree.tranImg(last,"folderopen.gif","folder.gif");
			$(this).parent().next().slideToggle();
			var state=$(this).attr("state");			
			$(this).attr("state", state=="true" ? "false" : "true");
		}
	);
	//节点文本双击事件，展开与折叠
	$("div[type='text']").dblclick(
		function()
		{
			//其实就是，当前节点前面图标的事件
			$(this).prev("div[IsChilds='True']").click();
		}
	);
	//根节点的点击事件
	if(Tree.RootClick!=null)Tree.Area.find("div[type='rootText']").click(Tree.RootClick);
	//其余节点的点击事件
	//if(Tree.NodeClick!=null)Tree.Area.find("div[type='text']").click(Tree.NodeClick);
}
//拖动事件
Tree.prototype.DragEvent=function()
{	
	//当鼠标点击节点时
	$("body").mousedown(
		function(e)
		{				
			//鼠标点击的当前菜单节点
			var curr=Tree.getDragObj(e);
			if(curr==null)return;
			Tree.NodeClick(curr);
			//设置点击的焦点
			$("div[type='text']").css("background-color","transparent");	
			curr.css("background-color","#efefef");	
			//生成可以拖动的节点，其实是复制点中的“焦点”
			var div=Tree.buildDragDiv(curr);			
			div.hide();	
			//锁定自身节点，以及下属节点
			Tree.lockPanel(curr);		
		}
	)
	//当鼠标移动时
	$("body").mousemove(
		function(e)
		{			
			//清除原有鼠标事件效果
			addIco(null,false);
			delIco(null,false);
			moveIco(null,false);
			var div=$("#NodeDragDiv");
			if(div.length<1)return;			
			div.show();	
			//进入拖动状态
			isDrag=true;
			Tree.lockPanel();
			//拖动对象，跟随鼠标
			var mouse=$().Mouse(e);
			div.css("top",mouse.y).css("left",mouse.x);
			//判断是否拖出左侧区域,如果拖出区域，则显示删除图标
			var isTree=$().isMouseArea(Tree.Area,e);
			if(!isTree)delIco(div,true);
			//鼠标拖动过程中，滑过的元素
			var curr=Tree.getDragObj(e);
			if(curr==null)return;			
			//判断是否处于自身节点
			if($().isMouseArea($(".lockPanel"),e))return;
			//鼠标处在节点的位置，如上半部分，下半部分
			var b=Tree.getGragBehavior(curr,mouse.x,mouse.y);	
			if(b==1 || b ==2)
			{
				moveIco(curr,true,true);
				div.attr("tax",Number(curr.attr("tax"))-5);
				div.attr("patId",curr.attr("patId"));				
			}
			if(b==3)
			{
				moveIco(curr,true,false);
				div.attr("tax",Number(curr.attr("tax"))+5);
				div.attr("patId",curr.attr("patId"));				
			}
			if(b==4)
			{
				div.attr("tax",5);
				addIco(curr,true);				
				if(div.attr("nodeId")!=curr.attr("nodeId"))div.attr("patId",curr.attr("nodeId"));				
			}
			
		}
	)
	//当鼠标松开点击事件时
	$("body").mouseup(
		function(e)
		{			
			if(isDrag)
			{	
				var div=$("#NodeDragDiv");
				var tax=Number(div.attr("tax"));
				var pid=Number(div.attr("patId"));
				var oldtax=Number(div.attr("oldTax"));
				var oldPid=Number(div.attr("oldPatId"));
				//是否临近自身
				var isSelf=false;
				if(pid==oldPid)isSelf=Math.abs(tax-oldtax)<10;
				//鼠标是否处在树形区域
				var isTree=$().isMouseArea(Tree.Area,e);
				//如果处在树形区域，则更改排放顺序
				if(isTree && !isSelf)updateTree(e);
				//如果不处在树形区域，则为删除
				if(!isTree)delTreeNode();				
			}
			//将一些拖动显示效果还原
			addIco(null,false);
			delIco(null,false);
			moveIco(null,false);
			//清除拖动对象
			$("#NodeDragDiv").remove();			
			//解决锁定，并退出拖动状态
			Tree.unlockPanel();	
		}
	)
}
//生成要拖动的节点
Tree.buildDragDiv=function(curr)
{
	var div=$("#NodeDragDiv");
	if(div.length<1)
	{
		$("body").append("<div id=\"NodeDragDiv\"></div>");
		div=$("#NodeDragDiv");
	}
	div.html(curr.text());
	div.attr("text",curr.text());
	div.attr("nodeId",curr.attr("nodeId"));
	div.attr("oldPatId",curr.attr("patId"));
	div.attr("oldTax",curr.attr("tax"));
	div.css("position","absolute");
	div.css("z-index","1000");
	div.css("width","auto");
	div.css("height","auto");
	div.css("border","1px dashed #FFCC66");
	div.css("background-color","#ffffff");	
	return div;
}
//获取当前坐标下的节点
//return:返回树形节点的页面元素
Tree.getDragObj=function(e)
{
	var mouse=$().Mouse(e);
	var x=mouse.x;
	var y=mouse.y
	var tmp=null;	
	$("div[type='text']").each(
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
//当鼠标处在某节点元素时，探知其要做的行为
//return:
//		1:鼠标处在元素左上半部分，
//		2:鼠标处在元素右上半部分，
//		3:鼠标处在元素左下半部分
//		4:鼠标处在元素右下半部分
Tree.getGragBehavior=function(obj,x,y)
{
	var offset = obj.offset();
	var l=offset.left;
	var t=offset.top;
	var w=obj.width();
	var h=obj.height();		
	//判断处于左半部分
	if(x > l && x < (l + w/2))
	{
		if(y > t && y < (t + h/2)) return 1;//左上
		if(y > (t + h/2) && y <(t+h)) return 3;//左下
	}
	//判断处于右半部分
	if(x > (l + w/2) && x < (l+ w))
	{
		if(y > t && y < (t + h/2)) return 2;//右上
		if(y > (t + h/2) && y <(t+h)) return 4;//右下
	}
	return 0;	
}
//锁定面板，当拖动某菜单节点时，该节点及下属节点锁定
//element:页面中当前菜单节点对象
Tree.lockPanel=function(element)
{	
	if(element!=null)
	{
		var id=element.attr("nodeId");
		var panel=$("div[panelId="+id+"]");
		panel.attr("state","lock");
	}else
	{
		var panel=$("div[state='lock']");
		panel.attr("class","lockPanel");
		panel.find("div[type='text']").attr("state","lock");
		panel.find("div[type='text']").attr("class","lockNode");
	}	
}
//解除所定的面板
Tree.unlockPanel=function()
{
	$("div[type='nodePanel']").removeClass("lockPanel");
	$("div[type='nodePanel']").attr("state","none");
	$("div[type='text']").attr("state","none");
	$("div[type='text']").removeClass("lockNode");
	isDrag=false;
}
//增加子级的图标
//current:当前所处的节点
//isShow:是否要显示图标
function addIco(current,isShow)
{
	var ico=$("#TreeAddIco");
	if(ico.length<1)
	{
		$("body").append("<div id=\"TreeAddIco\"></div>");
		ico=$("#TreeAddIco");
	}	
	ico.css("position","absolute").css("z-index","100");
	ico.css("width",18).css("height",18);
	ico.css("background-image", "url(../Images/tree/add.gif)");
	if(current!=null)
	{
		//当前节点前的图标，例如文件夹或文件图标
		var img=current.prev().find("img:last");
		var offset = img.offset();
		ico.css("top",offset.top);
		ico.css("left",offset.left);
	}
	isShow ? ico.show() : ico.hide();
}
//删除的图标
function delIco(current,isShow)
{
	var ico=$("#TreeDelIco");
	if(ico.length<1)
	{
		$("body").append("<div id=\"TreeDelIco\"></div>");
		ico=$("#TreeDelIco");
	}	
	ico.css("position","absolute").css("z-index","2000");
	ico.css("width",16).css("height",16);
	ico.css("background-image", "url(../Images/tree/del.png)");
	if(current!=null)
	{
		var offset = current.offset();
		ico.css("top",offset.top+current.height()-ico.height()/2);
		ico.css("left",offset.left+current.width()-ico.width()/2);
	}
	isShow ? ico.show() : ico.hide();
}
function moveIco(current,isShow,isTop)
{
	var ico=$("#TreeMoveIco");
	if(ico.length<1)
	{
		$("body").append("<div id=\"TreeMoveIco\"></div>");
		ico=$("#TreeMoveIco");
	}	
	ico.css("position","absolute").css("z-index","300");
	ico.css("width",60).css("height",5);
	ico.css("background-image", "url(../Images/tree/reline.gif)");
	if(current!=null)
	{
		//当前节点前的图标，例如文件夹或文件图标
		var img=current.prev().find("img:last");
		var offset = img.offset();
		ico.css("left",offset.left-1);
		if(isTop!=null)
		ico.css("top",isTop ? offset.top-2 : offset.top+img.height()-2);		
	}
	isShow ? ico.show() : ico.hide();
}
//上传排序菜单树排序状态数据
function updateTree(e)
{	
	//如果不存在任何节点之上
	var curr=Tree.getDragObj(e);
	if(curr==null)return;
	//判断是否处于自身节点
	var lock=$(".lockPanel");
	var treepanel=Tree.Area.find(":first-child");
	var isSelf=$().isMouseArea(lock,e);
	if(isSelf)return;	
	//提交菜单排序数据
	var res=encodeURIComponent(orderResult());
	var id=$().getPara("id");
	Tree.onChangeOrder(res);
}
//递交排列顺序的最终结果
//return:返回xml数据;记录节点id,父id,序号,节点状态
function orderResult()
{
	var div=$("#NodeDragDiv");
	if(div.length<1)return;
	var divId=div.attr("nodeId");
	//结果
	var tmp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"; 
	tmp+="<nodes rootid=\""+Tree.RootId+"\">";
	$("div[type='text']").each(
		function()
		{
			var id=$(this).attr("nodeId");
			var pid= id!=divId ? $(this).attr("patId") : div.attr("patId") ;
			var tax= id!=divId ? $(this).attr("tax") : div.attr("tax") ;
			var stat=$(this).prev().attr("state");
			tmp+="<node id=\""+id+"\" pid=\""+pid+"\" tax=\""+tax+"\" state=\""+stat+"\" rootid=\""+Tree.RootId+"\" />";
		}
	);	
	tmp+="</nodes>";
	return tmp;
}
//删除某节点
function delTreeNode()
{		
	var div=$("#NodeDragDiv");
	if(div.length<1)return;	
	var name=div.text();
	var msg="您是否确定要删除当前菜单项："+name+" ？\n注：\n1、当前菜单项的下级所有子菜单，也会同时删除。\n2、删除后无法恢复。";
	if(!confirm(msg))return;
	//结果
	var divId=div.attr("nodeId");
	var tmp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"; 
	tmp+="<node type=\"del\" id=\""+divId+"\" rootid=\""+Tree.RootId+"\">";
	tmp+="</node>";
	//提交数据
	var res=encodeURIComponent(tmp);
	Tree.onDelNode(res);
			
}
