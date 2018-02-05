
$(
	function()
	{				
		/*//载入树形菜单的数据
		$.ajax({
			  type: "get",
			  url: "/Static/DepartArtCol.html",
			  dataType: "html",
			  data:{},
			  //开始，进行预载
			  beforeSend: function(XMLHttpRequest, textStatus){	
			  		//if(loadfunc!=null)loadfunc();
				},
				//加载出错
			 error: function(XMLHttpRequest, textStatus, errorThrown){	
			 		//if(unloadfunc!=null)unloadfunc();
				},
				//加载成功！
			  success: function(data) {	
				var area=$("#TreeAreaPanel");
				area.html(data);
				initLoadSuccess();			
			  }
		  }); */
	    var type = $().getPara("type");
		$().SoapAjax("ArticleColumn","GetPurViewTree",{type:type},funcc,loading,unloading);
	}
);
//数据载入完成后的事件
//data:完整数据源，webservice输出
function funcc(data)
{
	var area=$("#TreeAreaPanel");
	var html=data.find("string").text();
	area.html(html);
	initLoadSuccess();	
}
//初始载入成功
function initLoadSuccess()
{
	//树形总区域
	var area=$("#TreeAreaPanel");
	var h=document.documentElement.clientHeight;
	area.height(h-$("#contTop").height()-$(".pageWinBtn").height()-50);
	var w=document.documentElement.clientWidth;
	//单个树形区域
	var treebox=$(".TreeBox");
	treebox.width(w/treebox.size()*.9);	
	treebox.height(area.height()*.95);
	//界面初始化
	initloyout();
	//获取已经拥有的权限
	var id=$().getPara("id");
	var type=$().getPara("type");	
	$().SoapAjax("ArticleColumn","GetPurView",{depid:id},setPur,null,unloading);	
}
//页面初始化布局
function initloyout()
{
	var type=$().getPara("type");
	var typename=$("#typeName");
	if(type=="posi")typename.text("角色");
	if(type=="group")typename.text("工作组");
	if(type=="depart")typename.text("院系");
	//保存按钮
	$("input[name$='btnEnter']").click(
		function()
		{
			//提交编辑数据
			var state=getTreeState();
			state=encodeURIComponent(state);      
			$().SoapAjax("ArticleColumn","AcceptPurView",{xml:state},
					function()
					{
						alert("保存权限成功！");
					},
					loading,unloading);
			return false;
		}
	);	
	$("input[name$='CloseButton']").click(
		function()
		{
			new parent.PageBox().Close();
			return false;
		}
	);	
}
//设置权限
function setPur(data)
{
	var pur=data.find("Depart_ArtColumn");	
	pur.each(
		function()
		{
			var mmid=$(this).find("Ac_Id").text();
			var state=$(this).find("Dac_State").text();
			var node=$("div[type='select'][nodeId='"+mmid+"']");
			if(state=="sel")Tree.ToSelect(node);
			if(state=="half")Tree.ToHalfSelect(node);
		}
	);
	var tree=new Tree("#TreeAreaPanel");
	tree.TreeEvent();
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
function unloading()
{
	$("#screenMask").remove();
	$("#loadingBar").hide();
}