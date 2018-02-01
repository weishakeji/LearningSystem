$(
 	function()
	{
		Init();
		//如果不允许修改
		//alert(typeof(allowModify) + " _ " + allowModify);
		if(!allowModify)
		{
			
			$("#addEmplyee").hide(); 
			$("#inputShow").hide();
			$(".EmplyeeList img").hide();
			//$(".EmplyeeList .toPosi").css("width","auto");
		}else
		{
			$("#addEmplyee").show(); 
			$("#inputShow").show();
		}
	}
 );


function Init()
{
	//显示员工列表的按钮
	$("#addEmplyee").click(
			function()
			{
				var left=$("#leftPanel");
				left.width("65%");
				//收件人列表（填写区）
				var emp=$("div[id$='EmplyeeList']");
				//消息录入框
				var msgbox=$("textarea[name$='tbMsg']");
				//
				var tree=$("#MenuTreePanel");
				var offset = emp.offset();
				tree.css("top",offset.top);
				tree.css("left",offset.left+emp.width()+10);
				var msgset = msgbox.offset();
				var hg=msgset.top+msgbox.height()-offset.top;
				tree.height(hg);
				//员工列区域的高度
				setHeight(hg);
				$("#MenuTreePanel").show();
				return false;
			}
	);
	setInterval("inchange()",50);
	//发送按钮
	$("input[id$='btnSaveAndTo']").click(
			function()
			{				
				var tt=$("input[id$='tbEmpId']").val();
				if(tt=="")
				{
					alert("请选择收件人。");
					return false;
				}
			}
		);
	//保存到草稿箱的按钮
	$("input[id$='btnSave']").click(
			function()
			{				
				var tt=$("input[id$='tbEmpId']").val();
				if(tt=="")
				{
					alert("您未选择任何收件人，虽然不影响保存到草稿箱，但请注意添加。");
					return true;
				}
			}
		);
	
}
//录入文字数
function inchange()
{
	var show=$("#inputShow");
	var msg=$("textarea[name$='tbMsg']").val();
	var len=msg.length;
	if(len<=255)
	{
		show.html("您还可以录入<span class=\"showGreen\"> "+(255-len)+"</span> 个字符。");
	}else
	{
		show.html("您的录入超出 <span class=\"showRed\"> "+(len-255)+"</span> 个字符。");
	}
}

//数据载入完成后的事件
//data:完整数据源，webservice输出
function funcc(data)
{
	window.treeDataSource=data;
	var tree=new Tree("#MenuTreePanel");
	Tree.onAddNode=AddEmplyee;	
	//生成菜单
	tree.BuildMenu(data);	
}
//载入员工信息成功后
function emplyee(data)
{
	window.emplyeeDataSource=data;
}
//开始载入时的预载
function loading()
{
	$("#loadingBar").show();
	var mask=$("#screenMask");
	mask.remove();
	mask=null
	delete mask;
	var html="<div id=\"screenMask\"/>";
	$("body").append(html);
	mask=$("#screenMask");
	mask.css("position","absolute").css("z-index","100");
	var h=document.documentElement.clientHeight;
	mask.css("width","100%").css("height",h-0);	
	mask.css("top",0).css("left",5);
	mask.css("background-color","#ffffff");
	mask.css("filter","Alpha(Opacity=60)");
	mask.css("display","block");
	mask.css("-moz-opacity",0.6);
	mask.css("opacity",0.6);
	mask.fadeIn("slow");
}
//载入完成后，清除预载效果
function unloading()
{
	$("#screenMask").remove();
	$("#loadingBar").hide();
}
//移动员工到角色区域的事件
function AddEmplyee()
{
	//被拖动对象
	var drag=$("#NodeDragDiv");	
	var type=drag.attr("type");
	var nodeid=drag.attr("nodeId");
	if(type=="emplyee")
	{
		var node=$("#MenuTreePanel div[nodeId="+nodeid+"]");
		AddSingleEmp(node);
	}
	if(type=="depart")
	{
		var name=drag.attr("text");
		if(!confirm("是否将 "+name+" 的全部下属员工设置为该角色？"))return;
		var depart=$("#MenuTreePanel div[nodeId="+nodeid+"]");
		depart.parent().parent().find("div[type='emplyee']").each(
				function()
				{
					AddSingleEmp($(this));
				}
			);		
	}
}
