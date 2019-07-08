var sysrootid=103;
//
$(function(){
    initloyout();
    //控制编辑面板，默认为编辑根节点
    setEditPanel("root");
    //载入树形菜单的数据
    $().SoapAjax("Depart","DepartOrder",{result:""},funcc,loading,unloading);
});
//页面初始化布局
function initloyout(){
	$("input[type='text']").addClass("TextBox");
	//$("textarea").addClass("TextBox");
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
        //设置权限
        if(type=="purview"){
            var target=$(this).attr("target");
            var id=$(this).attr("id");
            if(id=="purviewTitle"){
                OpenWin(target+"?id="+$(this).attr("nodeId")+"&type=depart","设置院系管理权限",1024,600);
                return;
            }
            if(id=="artPurTitle"){
                OpenWin(target+"?id="+$(this).attr("nodeId")+"&type=depart","设置院系文章编辑权限",90,90);
                return;
            }
        }
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
	window.treeDataSource=data;
	var tree=new Tree("#MenuTreePanel");
	Tree.RootClick=editroot;
	Tree.NodeClick=editNode;
	//
	Tree.RootId=sysrootid;
	Tree.onChangeOrder=changeOrder;
	Tree.onDelNode=delNode;	
	//生成菜单
	tree.BuildMenu(data);
	//$("#MenuTreePanel").html(rootHtml); 
	editroot();	
}
//点击根节点
function editroot(){
	$("#MenuEditPanel").find("#addNodeParent").text("无（顶级）");
	var tit=$("#editTitle");
	tit.hide();
	var tit=$("#purviewTitle");
	tit.hide();
	$("#artPurTitle").hide();	
	setEditPanel("add");
}
//点击子节点时
function editNode(node){
	var data=window.treeDataSource;
	var panel=$("#MenuEditPanel");
	var id=node.attr("nodeId");
	//“编辑”选项卡显示
	$("#editTitle").show();	
	$("#purviewTitle").show();	
	$("#artPurTitle").show();	
	$("#purviewTitle").attr("nodeId",id);
	$("#artPurTitle").attr("nodeId",id);
	var edit=$("#EditPanel");
	setEditPanel("edit");		
	var nodes=data.find("Depart");		
	nodes.each(function(){
		var n=Number($(this).find("Dep_Id").text().toLowerCase());
		//生成当前菜单节点的节点对象
		if(n==id)node=new Node($(this),data);
	});
	//当前节点信息
	panel.find("span[edit='id']").text(node.Id);
	panel.find("span[edit='pid']").text(node.PatId);
	panel.find("span[edit='tax']").text(node.Tax);
	panel.find("span[edit='addpid']").text(node.Id);
	panel.find("input[name='name']").val(node.Name);
	panel.find("#addNodeParent").text(node.Name);
	panel.find("input[name='CnAbbr']").val(node.CnAbbr);
	panel.find("input[name='enname']").val(node.EnName);
	panel.find("input[name='EnAbbr']").val(node.EnAbbr);
	panel.find("input[name='code']").val(node.Code);
	panel.find("textarea[name='func']").text(node.Intro);	
	var cbshow=panel.find("input[type='checkbox'][name='cbIsShow']");
	node.IsShow ? cbshow.prop("checked","checked") : cbshow.removeAttr("checked");
	var cbuse=panel.find("input[type='checkbox'][name='cbIsUse']");
	node.IsUse ? cbuse.prop("checked","checked") : cbuse.removeAttr("checked");
	panel.find("input[name='phone']").val(node.Phone);
	panel.find("input[name='fax']").val(node.Fax);
	panel.find("input[name='email']").val(node.Email);
	panel.find("input[name='msn']").val(node.Msn);
	panel.find("input[name='WorkAddr']").val(node.WorkAddr);
	//当前节点父节点信息
	var p=new Node(node.Parent,data);
	panel.find("#editNodeParent").text(p.Id==0 ? "无（顶级）" : p.Name);
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
	var edit=$("#EditPanel");
	var add=$("#AddPanel");	
	$("#EditPanelTitle").children().removeClass("click");
	switch(pevent){
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
	if($("#EditPanel").is(":visible")!=false){		
		//修改子节点信息		
		if($("#name").val()==""){
			alert("名称不得为空！");
			$("#name").focus();
			return;
		}
		xml = encodeURIComponent(updateNode());	
		$().SoapAjax("Depart","DepartUpdate ",{result:xml},funcc,loading,unloading);
	}
	//增加节点信息
	if($("#AddPanel").is(":visible")!=false){	
		if($("#addname").val()==""){
			alert("名称不得为空！");
			$("#name").focus();
			return;
		}
		xml = encodeURIComponent(addNode());
		$().SoapAjax("Depart","DepartAdd",{result:xml},funcc,loading,unloading);
	}
	return false;
}
//递交子节点信息
function updateNode(){
	var panel=$("#MenuEditPanel");
	//结果
	var tmp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"; 
	tmp+="<node type=\"edit\" id=\""+$("span[edit='id']").text()+"\" >";
	tmp+="<name>"+$("#name").val()+"</name>";	
	tmp+="<CnAbbr>"+$("#CnAbbr").val()+"</CnAbbr>";
	tmp+="<enname>"+$("#enname").val()+"</enname>";
	tmp+="<EnAbbr>"+$("#EnAbbr").val()+"</EnAbbr>";
	tmp+="<code>"+$("#code").val()+"</code>";	
	tmp+="<IsShow>"+panel.find("input[type='checkbox'][name='cbIsShow']").prop("checked")+"</IsShow>";
	tmp+="<IsUse>"+panel.find("input[type='checkbox'][name='cbIsUse']").prop("checked")+"</IsUse>";
	tmp+="<func><![CDATA["+panel.find("textarea[name='func']").text()+"]]></func>";
	tmp+="<phone>"+$("#phone").val()+"</phone>";	
	tmp+="<fax>"+$("#fax").val()+"</fax>";	
	tmp+="<email>"+$("#email").val()+"</email>";	
	tmp+="<msn>"+$("#msn").val()+"</msn>";	
	tmp+="<WorkAddr>"+$("#WorkAddr").val()+"</WorkAddr>";	
	tmp+="</node>";
	return tmp;
}
//递交新增子节点信息
function addNode(){
	var panel=$("#MenuEditPanel");
	//结果
	var tmp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"; 
	tmp+="<node type=\"add\" id=\"-1\" >";
	tmp+="<name>"+$("#addname").val()+"</name>";	
	tmp+="<pid>"+panel.find("span[edit='addpid']").text()+"</pid>";
	tmp+="<CnAbbr>"+$("#addCnAbbr").val()+"</CnAbbr>";
	tmp+="<enname>"+$("#addenname").val()+"</enname>";
	tmp+="<EnAbbr>"+$("#addEnAbbr").val()+"</EnAbbr>";
	tmp+="<code>"+$("#addcode").val()+"</code>";	
	tmp+="<IsShow>"+panel.find("input[type='checkbox'][name='addcbIsShow']").prop("checked")+"</IsShow>";
	tmp+="<IsUse>"+panel.find("input[type='checkbox'][name='addcbIsUse']").prop("checked")+"</IsUse>";
	tmp+="<func><![CDATA["+panel.find("textarea[name='addfunc']").text()+"]]></func>";
	tmp+="<phone>"+$("#addphone").val()+"</phone>";	
	tmp+="<fax>"+$("#addfax").val()+"</fax>";	
	tmp+="<email>"+$("#addemail").val()+"</email>";	
	tmp+="<msn>"+$("#addmsn").val()+"</msn>";	
	tmp+="<WorkAddr>"+$("#addWorkAddr").val()+"</WorkAddr>";	
	tmp+="</node>";
	return tmp;
}
//更改顺序
function changeOrder(res){
	$().SoapAjax("Depart","DepartOrder",{result:res},funcc,loading,unloading);	
}
//删除节点
function delNode(res)
{
	$().SoapAjax("Depart","DepartDel",{result:res},funcc,loading,unloading);
}