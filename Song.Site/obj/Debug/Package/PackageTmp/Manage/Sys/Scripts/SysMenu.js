document.onselectstart = document.ondrag = function(){
    return false;
}
//分类的根节点
var rootid = $().getPara("pcid");
rootid = isNaN(rootid) ? -1 : Number(rootid);
rootid=parseInt(rootid)<0 ? 103 : parseInt(rootid);
//
$(function () {
    initloyout();
    //控制编辑面板，默认为编辑根节点
    setEditPanel("root");
    //载入树形菜单的数据
    $().SoapAjax("ManageMenu", "Order", { result: "", rootid: rootid, type: "sys" }, funcc, loading, unloading);
    //面板的一些事件
    setEvent();
});
//页面初始化布局
function initloyout(){
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
	//保存按钮
	$("input[name$='btnEnter']").click(function(){
			//提交编辑数据
			UpdateNodeEvent();
			return false;
	});	
}
//数据载入完成后的事件
//data:完整数据源，webservice输出
function funcc(data){
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
//编辑根节点
function editroot(){
	var root=$("#RootEditPanel");
	var edit=$("#EditPanel");
	setEditPanel("edit");
	root.show();
	edit.hide();	
	$.get("/manage/soap/ManageMenu.asmx/ManageMenuJson", { id: rootid },
		  function(data)
		  {
				eval($(data).text());
				var panel=$("#MenuEditPanel");
				panel.find("input[name='rootname']").attr("value",node.MM_Name);	
				panel.find("#addNodeParent").text(node.MM_Name);				  
		  })	
	$("span[edit='id']").text(rootid);
}
//进入编辑子节点
function editNode(node)
{
	var root=$("#RootEditPanel");
	var edit=$("#EditPanel");
	setEditPanel("edit");
	root.hide();
	edit.show();
	var panel=$("#MenuEditPanel");
	var id=node.attr("nodeId");
	//获取单个信息
	$.get("/manage/soap/ManageMenu.asmx/ManageMenuJson", { id: id },
	  function(data){
		  //生成node节点对象
		eval($(data).text());
		//当前节点信息
		panel.find("span[edit='id']").text(node.MM_Id);
		panel.find("span[edit='pid']").text(node.MM_PatId);
		panel.find("span[edit='tax']").text(node.MM_Tax);
		panel.find("input[name='name']").attr("value",node.MM_Name);
		panel.find("#addNodeParent").text(node.MM_Name);
		//节点类别
		panel.find("input[name='type']").each(
			function()
			{
				$(this).removeAttr("checked");
				if($(this).val()==node.MM_Type)$(this).attr("checked","checked");
			}
		);
		//如果是打开窗口的类型，则显示窗口宽高
		if(node.MM_Type=="open")
			panel.find("span[id='whArea']").show();
		else
			panel.find("span[id='whArea']").hide();
		//
		panel.find("input[name='width']").attr("value",node.MM_WinWidth);
		panel.find("input[name='height']").attr("value",node.MM_WinHeight);
		panel.find("input[name='link']").attr("value",node.MM_Link);
		panel.find("input[name='marker']").attr("value",node.MM_Marker);
		panel.find("textarea[name='intro']").text(node.MM_Intro);
		//样式
		var cbbold=panel.find("input[type='checkbox'][name='cbIsBold']");
		node.IMM_sBold ? cbbold.attr("checked","checked") : cbbold.removeAttr("checked");
		var cbitalic=panel.find("input[type='checkbox'][name='cbIsItalic']");
		node.MM_IsItalic ? cbitalic.attr("checked","checked") : cbitalic.removeAttr("checked");
		var cbshow=panel.find("input[type='checkbox'][name='cbIsShow']");
		node.MM_IsShow ? cbshow.attr("checked","checked") : cbshow.removeAttr("checked");
		var cbuse=panel.find("input[type='checkbox'][name='cbIsUse']");
		node.MM_IsUse ? cbuse.attr("checked","checked") : cbuse.removeAttr("checked");
		//当前节点父节点信息
		$.get("/manage/soap/ManageMenu.asmx/ManageMenuJson", { id: node.MM_PatId },
				  function(data)
				  {
					  eval($(data).text());
					  var panel=$("#MenuEditPanel");
					  panel.find("#editNodeParent").text(node.MM_Name);					  
				  })
	  });
	
}
//开始载入时的预载
function loading(){
	$("#loadingBar").show();
	var mask=$("#screenMask");
	mask.remove();
	mask=null
	delete mask;
	var html="<div id=\"screenMask\"/>";
	$("body").append(html);
	mask=$("#screenMask");
	mask.css("position","absolute").css("z-index","50000");
	var h=document.documentElement.clientHeight;
	mask.css("width","100%").css("height",h-20);	
	mask.css("top",20).css("left",5);
	mask.css("background-color","#ffffff");
	mask.css("filter","Alpha(Opacity=60)");
	mask.css("display","block");
	mask.css("-moz-opacity",0.6);
	mask.css("opacity",0.6);
	mask.fadeIn("slow");
}
//载入完成后，清除预载效果
function unloading(){
	$("#screenMask").remove();
	$("#loadingBar").hide();
}
//控制编辑面板的事件
//pevent:"edit"为编辑节点，"add"为新增
function setEditPanel(pevent){
	//编辑区面板
	var panel=$("#MenuEditPanel #Panel");
	panel.children().hide();
	var edit=$("#RootEditPanel").parent();
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
//修改节点信息的事件方法
function UpdateNodeEvent(){
	var xml="";
	//如果是编辑状态
	if($("#RootEditPanel").parent().is(":visible")!=false){
		//修改根节点信息
		if($("#RootEditPanel").is(":visible")!=false){
			alert("不允许修改系统菜单根节点！");
			return;
			xml = updateRoot();	
			if($("#rootname").val()=="")
			{
				alert("名称不得为空！");
				$("#rootname").focus();
				return;
			}
		}else{
			//修改子节点信息
			xml = updateNode();	
			if($("#name").val()==""){
				alert("名称不得为空！");
				$("#name").focus();
				return;
			}
		}
	}
	//增加节点信息
	if($("#AddPanel").is(":visible")!=false){
		xml = addNode();		
		if($("#addname").val()==""){
			alert("名称不得为空！");
			$("#name").focus();
			return;
		}
	}
	//alert(xml);
	var res=encodeURIComponent(xml);
	$().SoapAjax("ManageMenu","Update",{result:res,pid:Tree.RootId,type:"sys"},funcc,loading,unloading);
	//getManageMenu("ManageMenu","UpdateTree",{rootId:rootid,nodexml:res},funcc);	
}
//递交根节点信息
function updateRoot(){
	//结果
	var tmp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"; 
	tmp+="<node type=\"root\" id=\""+rootid+"\" rootid=\""+rootid+"\" func=\"sys\">";
	tmp+="<name>"+$("#rootname").val()+"</name>";
	tmp+="<func>sys</func>";
	tmp+="</node>";
	return tmp;
}
//递交子节点信息
function updateNode(){
	var panel=$("#MenuEditPanel");
	//结果
	var tmp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"; 
	tmp+="<node type=\"edit\" id=\""+$("span[edit='id']").text()+"\" rootid=\""+rootid+"\" func=\"sys\">";
	tmp+="<name>"+$("#name").val()+"</name>";
	//判断类型
	var type="";
	panel.find("input[name='type']").each(function(){
			if($(this).attr("checked"))type=$(this).val();
	});	
	tmp+="<type>"+type+"</type>";
	tmp+="<link><![CDATA["+$("#link").val()+"]]></link>";
	tmp+="<marker>"+$("#marker").val()+"</marker>";
	tmp+="<winWidth>"+panel.find("input[name='width']").val()+"</winWidth>";
	tmp+="<winHeight>"+panel.find("input[name='height']").val()+"</winHeight>";
	tmp+="<func>sys</func>";
	tmp+="<IsItalic>"+panel.find("input[type='checkbox'][name='cbIsItalic']").attr("checked")+"</IsItalic>";
	tmp+="<IsBold>"+panel.find("input[type='checkbox'][name='cbIsBold']").attr("checked")+"</IsBold>";
	tmp+="<IsUse>"+panel.find("input[type='checkbox'][name='cbIsUse']").attr("checked")+"</IsUse>";
	tmp+="<IsShow>"+panel.find("input[type='checkbox'][name='cbIsShow']").attr("checked")+"</IsShow>";
	tmp+="<intro><![CDATA["+panel.find("textarea[name='intro']").text()+"]]></intro>";
	tmp+="</node>";
	return tmp;
}
//递交新增子节点信息
function addNode(){
	var panel=$("#AddPanel");
	//结果
	var tmp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"; 
	tmp+="<node type=\"add\" id=\"-1\" rootid=\""+rootid+"\">";
	tmp+="<name>"+$("#addname").val()+"</name>";
	tmp+="<pid>"+$("span[edit='id']").text()+"</pid>";
	//判断类型
	var type="";
	panel.find("input[name='addtype']").each(function(){
			if($(this).attr("checked"))type=$(this).val();
	});	
	tmp+="<type>"+type+"</type>";
	tmp+="<link><![CDATA["+$("#addlink").val()+"]]></link>";
	tmp+="<marker>"+$("#addmarker").val()+"</marker>";
	tmp+="<winWidth>"+panel.find("input[name='addwidth']").val()+"</winWidth>";
	tmp+="<winHeight>"+panel.find("input[name='addheight']").val()+"</winHeight>";
	tmp+="<func>sys</func>";
	tmp+="<IsItalic>"+panel.find("input[type='checkbox'][name='addcbIsItalic']").attr("checked")+"</IsItalic>";
	tmp+="<IsBold>"+panel.find("input[type='checkbox'][name='addcbIsBold']").attr("checked")+"</IsBold>";
	tmp+="<IsShow>"+panel.find("input[type='checkbox'][name='addcbIsShow']").attr("checked")+"</IsShow>";
	tmp+="<IsUse>"+panel.find("input[type='checkbox'][name='addcbIsUse']").attr("checked")+"</IsUse>";
	tmp+="<intro><![CDATA["+panel.find("textarea[name='addintro']").text()+"]]></intro>";
	tmp+="</node>";
	return tmp;
}
//更改顺序
function changeOrder(res){
	$().SoapAjax("ManageMenu","Order",{result:res,rootid:Tree.RootId,type:"sys"},funcc,loading,unloading);	
}
//删除节点
function delNode(res){
	$().SoapAjax("ManageMenu","Del",{result:res,pid:Tree.RootId,type:"sys"},funcc,loading,unloading);
}
//面板上的一些事件
function setEvent(){
	//节点编辑区域中“节点类型”的事件
	$("p[type='nodeType']").find("input[type='radio']").click(function(){
		if($(this).val()=="open"){
			$(this).parents("p[type='nodeType']").find("span").show();
		}else{
			$(this).parents("p[type='nodeType']").find("span").hide();
		}
	});
}