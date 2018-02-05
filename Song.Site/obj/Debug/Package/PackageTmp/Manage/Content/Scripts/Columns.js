//document.onselectstart = document.ondrag = function(){
//    return false;
//}
//分类的根节点
var rootid=$().getPara("pcid");
rootid=parseInt(rootid)<0 ? 0 : parseInt(rootid);
//
$(
	function()
	{
		initloyout();
		//控制编辑面板，默认为编辑根节点
		setEditPanel("root");
		//载入树形菜单的数据
		$().SoapAjax("Columns","Order",{result:""},funcc,loading,unloading);		
	}
);
//页面初始化布局
function initloyout()
{
	$("input[type='text']").addClass("TextBox");
	$("textarea").addClass("TextBox");
	//页面可控布局
	var cont=$(".pageWinContext");
	//左侧菜单树区域
	var left=$("#MenuTreePanel");
	var h=document.documentElement.clientHeight;
	left.height(h-120);
	//右侧编辑区域
	var right=$("#Panel");
	var h=document.documentElement.clientHeight;
	right.height(h-153);
	
	//右侧编辑区事件
	$("#EditPanelTitle div").click(function(){
		var type=$(this).attr("type");				
		setEditPanel(type);
	});
	//保存按钮修改子节点信息
	$("input[name$='btnSave']").click(function(){
		if($.trim($("#name").val())!="")
		{
			xml = encodeURIComponent(updateNode());	
			$().SoapAjax("Columns","Update ",{result:xml},funcc,loading,unloading);
		}			
		return false;
	});
	//新增按钮
	$("input[name$='btnAdd']").click(function()	{		
		if($.trim($("#addname").val())!="")
		{
			var xml = encodeURIComponent(addNode());
			$().SoapAjax("Columns","Add",{result:xml},funcc,loading,unloading);
		}
		return false;
	});	
}
//数据载入完成后的事件
//data:完整数据源，webservice输出
function funcc(data)
{
	var tree=new Tree("#MenuTreePanel");	
	//var html=data.find("string").text();
	//$("#MenuTreePanel").html(html); 
	Tree.RootClick=editroot;
	Tree.NodeClick=editNode;
	//
	Tree.RootId=rootid;
	Tree.onChangeOrder=changeOrder;
	Tree.onDelNode=delNode;	
	
	//生成菜单	
	tree.BuildMenu(data);
	//$("#MenuTreePanel").html(rootHtml); 
	editroot();	
}
//点击根节点
function editroot()
{
	var panel=$("#MenuEditPanel");
	panel.find("#addNodeParent").text("无（顶级）");
	panel.find("span[edit='addpid']").text(rootid);
	var tit=$("#editTitle");
	tit.hide();
	var tit=$("#purviewTitle");
	tit.hide();
	setEditPanel("add");
}
//点击子节点时
function editNode(point)
{	
	var panel=$("#MenuEditPanel");
	var id=point.attr("nodeId");
	//“编辑”选项卡显示
	$("#editTitle").show();	
	$("#purviewTitle").show();	
	$("#purviewTitle").attr("nodeId",id);
	var edit=$("#EditPanel");
	setEditPanel("edit");		
	//获取单个信息
	$.get("/manage/soap/Columns.asmx/ColumnJson", { id: id },
	  function(data){
		//生成node节点对象
		eval($(data).text());
		//新增下级栏目所用的参数
		panel.find("#addNodeParent").text(node.Col_Name);	
		panel.find("span[edit='addpid']").text(node.Col_ID);
		//当前节点信息
		panel.find("span[edit='id']").text(node.Col_ID);
		panel.find("span[edit='pid']").text(node.Col_PID);
		panel.find("span[edit='tax']").text(node.Col_Tax);
		panel.find("input[name='name']").val(node.Col_Name);
		panel.find("input[name='byname']").val(node.Col_ByName);		
		//栏目类型
		panel.find("input[type='radio'][name='type']").each(function(){
			$(this).attr("checked", $(this).val()==node.Col_Type ? "checked" : "");
		});
		panel.find("input[name='title']").val(node.Col_Title);	
		panel.find("input[name='keywords']").val(node.Col_Keywords);	
		panel.find("textarea[name='desc']").text(node.Col_Descr);
		panel.find("textarea[name='intro']").text(node.Col_Intro);
		//是否与使用		
		var cbuse=panel.find("input[type='checkbox'][name='cbIsUse']");
		node.Col_IsUse ? cbuse.attr("checked","checked") : cbuse.removeAttr("checked");
		//是否允许评论与审核
		var cbIsNote=panel.find("input[type='checkbox'][name='cbIsNote']");
		node.Col_IsNote ? cbIsNote.attr("checked","checked") : cbIsNote.removeAttr("checked");			
		//当前节点父节点信息		
		if(node.Col_PID ==0)
		{
			panel.find("#editNodeParent").text("顶级");
		}else
		{
			var pid=point.attr("patId");
			var pnode=$("#MenuTreePanel").find("div[nodeId="+pid+"]");
			var pname=pnode.find("div:last").find("span:first").text();
			panel.find("#editNodeParent").text(pname);			
		}
	  }); 	
}
//控制编辑面板的事件
//pevent:"edit"为编辑节点，"add"为新增
function setEditPanel(pevent)
{
	//编辑区面板
	var panel=$("#MenuEditPanel #Panel");
	panel.children().hide();
	var edit=$("#EditPanel");
	var add=$("#AddPanel");	
	$("#EditPanelTitle").children().removeClass("click");
	switch(pevent)
	{
		case "edit":
			add.hide();
			edit.show();
			$("#EditPanelTitle div[type='edit']").addClass("click");
			break;
		case "add":			
			edit.hide();
			add.show();
			$("#EditPanelTitle div[type='add']").addClass("click");
			break;
	}
}

//递交子节点信息
function updateNode()
{
	var panel=$("#MenuEditPanel");
	//结果
	var tmp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"; 
	tmp+="<node type=\"edit\" id=\""+$("span[edit='id']").text()+"\" >";
	tmp+="<name>"+$("#name").val()+"</name>";
	tmp+="<byname>"+$("#byname").val()+"</byname>";
	tmp+="<pid>"+panel.find("span[edit='pid']").text()+"</pid>";
	tmp+="<type>"+panel.find("input[type='radio'][name='type']:checked").val()+"</type>";	
	tmp+="<title><![CDATA["+panel.find("input[name='title']").val()+"]]></title>";
	tmp+="<keywords><![CDATA["+panel.find("input[name='keywords']").val()+"]]></keywords>";
	tmp+="<desc><![CDATA["+panel.find("textarea[name='desc']").text()+"]]></desc>";
	tmp+="<intro><![CDATA["+panel.find("textarea[name='intro']").text()+"]]></intro>";	
	tmp+="<IsUse>"+panel.find("input[type='checkbox'][name='cbIsUse']").attr("checked")+"</IsUse>";
	tmp+="<IsNote>"+panel.find("input[type='checkbox'][name='cbIsNote']").attr("checked")+"</IsNote>";
	tmp+="<Logo></Logo>";
	tmp+="</node>";
	return tmp;
}
//递交新增子节点信息
function addNode()
{
	var panel=$("#MenuEditPanel");
	//结果
	var tmp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"; 
	tmp+="<node type=\"add\" id=\"-1\" >";
	tmp+="<name>"+$("#addname").val()+"</name>";
	tmp+="<byname>"+$("#addbyname").val()+"</byname>";
	tmp+="<pid>"+panel.find("span[edit='addpid']").text()+"</pid>";
	tmp+="<type>"+panel.find("input[type='radio'][name='addtype']:checked").val()+"</type>";	
	tmp+="<title><![CDATA["+panel.find("input[name='addtitle']").val()+"]]></title>";
	tmp+="<keywords><![CDATA["+panel.find("input[name='addkeywords']").val()+"]]></keywords>";
	tmp+="<desc><![CDATA["+panel.find("textarea[name='adddesc']").text()+"]]></desc>";
	tmp+="<intro><![CDATA["+panel.find("textarea[name='addintro']").text()+"]]></intro>";	
	tmp+="<IsUse>"+panel.find("input[type='checkbox'][name='addcbIsUse']").attr("checked")+"</IsUse>";
	tmp+="<IsNote>"+panel.find("input[type='checkbox'][name='addcbIsNote']").attr("checked")+"</IsNote>";
	tmp+="<Logo></Logo>";
	tmp+="</node>";
	return tmp;
}
//更改顺序
function changeOrder(res)
{
	$().SoapAjax("Columns","Order",{result:res},funcc,loading,unloading);	
}
//删除节点
function delNode(res)
{
	var div=$("#NodeDragDiv");
	if(div.length<1)return;	
	var name=div.text();
	var msg="您是否确定要删除当前栏目："+name+" ？\n注：\n1、当前栏目的所有下属栏目，将同时删除\;\n2、被删除栏目下的内容也会删除;\n3、删除后将无法恢复。";
	if(!confirm(msg))return;

	$().SoapAjax("Columns","Del",{result:res},funcc,loading,unloading);
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