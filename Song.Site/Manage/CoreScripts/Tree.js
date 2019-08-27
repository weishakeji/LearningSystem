
/*!
 * 主  题：《图片分类的树形》
 * 说  明：生成图片分类管理的树形结构；
 * 功能描述：
 * 1、生成图片分类管理的树形结构；
 *
 * 作  者：宋雷鸣 
 * 开发时间: 2012年12月23日
 */

//修改数据的事件
//是否进入拖动
var isDrag=false;



//生成管理树
function Tree(area)
{
	if(area!=null)Tree.Area=$(area);	
	if(Tree.InitNumber>0)return;
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
//是否允许拖动
Tree.IsDrag=true;
//开始生成树形菜单
Tree.prototype.BuildMenu=function(data)
{
	var html=data.find("string").text();
	html=html.replace(/\[News\]/gi,"（资讯）");
	html=html.replace(/\[Product\]/gi,"（产品）");
	html=html.replace(/\[Picture\]/gi,"（图片）");
	html=html.replace(/\[Video\]/gi,"（视频）");
	html=html.replace(/\[Download\]/gi,"（下载）");
	html=html.replace(/\[Article\]/gi,"（单页）");
	Tree.Area.html(html); 
	//去除事件
	$("body").unbind("mousedown");
	$("body").unbind("mousemove");
	$("body").unbind("mouseup");
	//赋加事件
	this.TreeEvent();
	this.DragEvent();
}
//树形菜单事件，如打开折叠等
Tree.prototype.TreeEvent=function()
{
	//当点击节点前的图标时，如加减号文件夹图标	
	$("div[type='nodeIco'][IsChilds='True']").click(
		function()
		{			
			//栏目图标，文件夹或文件图标
			var last=$(this).find("img:last");
			//节点图标，加号或┝号
			var pre=last.prev();
			var tree=new Tree();
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
	if(Tree.RootClick!=null)Tree.Area.find("div[type='rootText']").click(
				function()
				{
					//设置点击的焦点
					$("div[type='text']").css("background-color","transparent");	
					$(this).css("background-color","#cccccc");	
					Tree.RootClick();
				});
	//其余节点的点击事件
	///当鼠标点击节点时
	$("body").mousedown(
		function(e)
		{				
			//鼠标点击的当前菜单节点
			var curr=Tree.getDragObj(e);
			if(curr==null)return;
			Tree.NodeClick(curr);
			//设置点击的焦点
			$("div[type='rootText']").css("background-color","transparent");
			$("div[type='text']").css("background-color","transparent");	
			curr.css("background-color","#efefef");	
			//生成可以拖动的节点，其实是复制点中的“焦点”
			var div=Tree.buildDragDiv(curr);			
			div.hide();	
			//锁定自身节点，以及下属节点
			Tree.lockPanel(curr);		
		}
	)
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
//拖动事件
Tree.prototype.DragEvent=function()
{		
	//当鼠标移动时
	$("body").mousemove(
		function(e)
		{	
			if(!Tree.IsDrag)return;
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
	div.css("width","auto").css("height","auto");
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
		$("div[nodeId="+id+"]").attr("state","lock");
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
	var lockPanel=$("div[state='lock']");
	lockPanel.attr("state","none");
	lockPanel.removeAttr("class");
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
	ico.css("background-image", "url(/manage/Images/tree/add.gif)");
	ico.css("background-repeat","no-repeat");
	if(current!=null)
	{
		//当前节点前的图标，例如文件夹或文件图标
		//var img=current.prev().find("img:last");
		//if($.browser.msie)img=current;
		var offset = current.offset();
		ico.css("top",offset.top);
		ico.css("left",offset.left-18);
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
	ico.css("background-image", "url(/manage/Images/tree/del.png)");
	ico.css("background-repeat","no-repeat");
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
	ico.css("background-image", "url(/manage/Images/tree/reline.gif)");
	ico.css("background-repeat","no-repeat");
	if(current!=null)
	{
		//当前节点前的图标，例如文件夹或文件图标
		//var img=current.prev().find("img:last");
		//if($.browser.msie)img=current;
		var offset = current.offset();
		ico.css("left",offset.left-1 -18);
		if(isTop!=null)
		ico.css("top",isTop ? offset.top-2 : offset.top+current.height()-2);		
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
	tmp+="<nodes >";
	$("div[type='text']").each(
		function()
		{
			var id=$(this).attr("nodeId");
			var pid= id!=divId ? $(this).attr("patId") : div.attr("patId") ;
			var tax= id!=divId ? $(this).attr("tax") : div.attr("tax") ;
			var stat=$(this).prev().attr("state");
			stat=stat!=null ? stat : false;
			tmp+="<node id=\""+id+"\" pid=\""+pid+"\" tax=\""+tax+"\" state=\""+stat+"\" rootid=\""+Tree.RootId+"\" />";
		}
	);	
	tmp+="</nodes>";
	return tmp;
}
//删除某节点
function delTreeNode()
{	
	//结果
	var div=$("#NodeDragDiv");
	var divId=div.attr("nodeId");
	var tmp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"; 
	tmp+="<node type=\"del\" id=\""+divId+"\" rootid=\""+Tree.RootId+"\">";
	tmp+="</node>";
	//提交数据
	var res=encodeURIComponent(tmp);
	Tree.onDelNode(res);			
}

