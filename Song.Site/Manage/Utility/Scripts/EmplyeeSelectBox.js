$(
	  function()
	  {
		  setTree();
		  TitleBarEvent();
		  TreeEvent();
		  emplistEvent();
	  }
  
);
//头部标题选择事件
function TitleBarEvent()
{
	//员工选择列表的总区域
	var box=$("#EmplyeeSelectBox");
	box.find("div[type='zone']").hide();
	//内容区高度
	box.find("div[type='zone']").height(box.height-20);
	//标题区域
	var title=$("#EmplyeeSelectBoxTitle");
	title.find("dd").attr("class","out");
	var fisrt=title.find("dd:first");
	fisrt.attr("class","click");
	var clicktype=fisrt.attr("type");
	box.find("div[paneltype='"+clicktype+"']").show();
	title.find("dd").click(
			function()
			{
				$(this).siblings().attr("class","out");
				$(this).attr("class","click");
				var box=$("#EmplyeeSelectBox");
				box.find("div[type='zone']").hide();
				var clicktype=$(this).attr("type");
				box.find("div[paneltype='"+clicktype+"']").show();
			}
		);
}
//设置所在区域的高度
function setHeight(hg)
{
	
	var box=$("#EmplyeeSelectBox");
	box.height(hg);
	//内容区高度
	box.find("div[type='zone']").height(hg-20);
}
//设置树形
function setTree()
{
	//员工选择列表的总区域
	var box=$("#EmplyeeSelectBox");
	var ctext=box.find("div[class='ctext']");
	//设置最后一个员工前的连接线
	ctext.each(
		function()
		{
			var line=$(this).parent();
			//如果是第后一个员工节点
			
			if(line.nextAll("div[type='empnode']").size()<1 && line.parent().next().attr("class")!="panel")
			{
				tranImg($(this).prev().find("img:last-child").prev(),"join.gif","joinBottom.gif");
			}
			nodeLine($(this).parent());
		}
	);
	//设置最后一个院系前的连接线
	var ptext=box.find("div[class='ptext']");
	ptext.each(
		function()
		{
			var line=$(this).parent();
			//如果是第后一个员工节点
			if(line.nextAll("div[type='departnode']").size()<1)
			{
				tranImg($(this).prev().find("img:last-child").prev(),"minus.gif","minusbottom.gif");
			}
			nodeLine($(this).parent());
		}
	);
	//
}
//处理点前图片的替换效果
//element:节点的元素
//img1:原图片
//img2:新图片
function tranImg(el,img1,img2)
{
	if(el.attr("src").indexOf(img1)>0)	
		el.attr("src",el.attr("src").replace(img1,img2));
	else	
		if(el.attr("src").indexOf(img2)>0)	
			el.attr("src",el.attr("src").replace(img2,img1));		
}
//节点前的链接线
function nodeLine(node,data)
{
	var temp="";
	var p=node.parent();
	while(p.attr("class") =="panel" && p.attr("patId")!="0")
	{		
		//如果是最后一个节点
		if(p.nextAll("div[class='nodeline']").size()<1)
		{
			temp="<img src=\"/Manage/Images/tree/empty.gif\"/>"+temp;
		}else
		{
			temp="<img src=\"/Manage/Images/tree/line.gif\"/>"+temp;
		}
		p=p.parent();
	}
	var img=node.find("div:first-child");
	var imghtml=img.html();
	//alert(temp);
	img.html(temp+imghtml);
	return temp;
}
//树形菜单事件，如打开折叠等
function TreeEvent()
{
	//当点击节点前的图标时，如加减号文件夹图标
	$("div[type='departnode'] div[nodetype='ico']").click(
		function()
		{			
			var next=$(this).parent().nextAll("div[type='emplyeepanel']:first");
			if(next.size()<1)return;			
			//栏目图标，文件夹或文件图标
			var last=$(this).find("img:last");
			//节点图标，加号或┝号
			var pre=last.prev();
			tranImg(pre,"plus.gif","minus.gif");
			tranImg(pre,"minusbottom.gif","plusbottom.gif");
			tranImg(last,"folderopen.gif","folder.gif");
			next.slideToggle();
			var state=$(this).attr("state");			
			$(this).attr("state", state=="true" ? "false" : "true");
		}
	);
	//当点击员工名称时，增加到指定区域
	$("#EmplyeeSelectBox div[class='ctext']").click(
			function()
			{
				if(emplyeeselBoxTarget=="")return;
				var target=$("div[id$='"+emplyeeselBoxTarget+"']");
				var id=$(this).attr("nodeid");
				var name=$(this).text();
				//target.append(name);
				AddSingleEmp($(this));
			}
		);
}
//员工列表的关闭事件
function emplistEvent()
{
	if(emplyeeselBoxTarget=="")return;
	var target=$("div[id$='"+emplyeeselBoxTarget+"']");
	target.find("div[type='toPosi'] img").click(
			function()
			{
				var nodeid=$(this).parent().attr("nodeId");
				var target=$("input[name$='"+emplyeeselTextBoxTarget+"']");				
				target.val(target.val().replace(nodeid+",",""));
				$(this).parent().remove();
				if($(this).parent().parent().children().size()<1)
				{
					$(this).parent().parent().html("无");
				}
			}
		);
}
//增加单个员工
function AddSingleEmp(node)
{
	//员工选择后的所在目标区
	if(emplyeeselBoxTarget=="")return;
	var target=$("div[id$='"+emplyeeselBoxTarget+"']");
	if(target.children("div").size()<1)
		target.html("");
	//如果员工已经存在
	var nodeid=node.attr("nodeid");
	var name=node.text();
	var emplyee=target.find("div[nodeId="+nodeid+"]");
	if(emplyee.length>0)return;	
	var tmp="<div type=\"toPosi\" nodeId=\""+nodeid+"\" class=\"toPosi\">";
	tmp+="<div class=\"EmpName\">";
	tmp+=node.text();
	tmp+="</div>";
	tmp+="<img alt=\"关闭\" src=\"/manage/Images/close1.gif\"/>";
	tmp+="</div>";
	target.append(tmp);
	target.find("div[nodeId="+nodeid+"] img").click(
			function()
			{
				var nodeid=$(this).parent().attr("nodeId");
				var target=$("input[name$='"+emplyeeselTextBoxTarget+"']");				
				target.val(target.val().replace(nodeid+",",""));
				$(this).parent().remove();
				if($(this).parent().parent().children().size()<1)
				{
					$(this).parent().parent().html("无");
				}
			}
		);
	//员工选择后的隐形textbox所在
	if(emplyeeselTextBoxTarget=="")return;
	var target=$("input[name$='"+emplyeeselTextBoxTarget+"']");
	var val=target.val();
	if(val!="")
	{
		var vals=val.split(",");
		for(var n in vals)
		{
			if(val[n]!=nodeid)
			{
				target.val(val+nodeid+",");
			}
		}
	}else
	{
		target.val(nodeid+",");
	}
}
