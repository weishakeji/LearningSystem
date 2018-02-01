
$(
  	function()
	{		
		initLayout();
		getLayout();
		$("#btnSaveLoyout").click(
				function()
				{
					upLoad();
				}
			);				
	}
 );
//初始化布局
function initLayout()
{
	//图片的初始位置
	var map=$("img[id$='imgShow'");
	var imgoff=map.offset();
	var initLeft=imgoff.left;
	var initTop=imgoff.top;
	//所有的调查主题
	var box=$(".box");
	var i=0;
	box.each(
		function()
		{
			var id=$(this).attr("id");
			var width=$(this).width();
			var tmId=id+"_boxContent";
			$(this).wrap("<div id='"+tmId+"' style='width:"+width+"'></div>"); 
			var boxCon=$("#"+tmId);
			boxCon.css("position","absolute");
			boxCon.css("z-index","100");			
			boxCon.css("left",initLeft);
			initLeft+=boxCon.width()+5;
			boxCon.css("top",initTop);
			if(initLeft>=map.width())
			{
				initTop+=boxCon.height()+5;
				initLeft=imgoff.left;
			}
			//设置拖动
			boxCon.easydrag();
			boxCon.setHandler(id);			
		}
	);	
}
//获取记录在服务器端的布局信息
function getLayout()
{
	$().SoapAjax("Para","GetPara",{key:"VoteMapLoyout"},setLayout,null,null);
	
}
//设置布局
function setLayout(data)
{
	//获取图片的顶点坐标
	var map=$("img[id$='imgShow'");
	var imgoff=map.offset();
	var pTop=map.css("padding-top").replace("px","");
	var pLeft=map.css("padding-left").replace("px","");	
	//分析获取来的坐标信息
	var txt=$(data).text();
	var array=txt.split("$");	
	for(s in array)
	{
		if(array[s]=="")continue;
		var info=array[s];
		setBox(info,imgoff.left+Number(pLeft),imgoff.top+Number(pTop));
	}
}
//设置元素的位置
//info:元素的信息，包括id与坐标
//left:地图的左顶点坐标
//top:地图的上顶点坐标
function setBox(info,left,top)
{	
	//图片的初始位置
	var map=$("img[id$='imgShow'");
	//图片宽高
	var imgWd=map.width();
	var imgHg=map.height();
	
	var array=info.split(",");	
	var box=$("#"+array[0]+"_boxContent");
	box.css("top",top+imgHg*Number(array[1])-box.height()/2);
	box.css("left",left+imgWd*Number(array[2])-box.width()/2);
	//box.css("left",left+Number(array[2]));
}
//上传布局信息
function upLoad()
{
	var xml=buildXML();
	//xml=encodeURIComponent(xml);
	$().SoapAjax("Para","SetPara",{key:"VoteMapLoyout",value:xml},
				 function()
				 {
					 alert("保存成功！");
				 }
				 ,null,null);
}
//生成配置信息的XML
function buildXML()
{
	//图片的初始位置
	var map=$("img[id$='imgShow'");
	var imgoff=map.offset();
	//图片宽高
	var imgWd=map.width();
	var imgHg=map.height();
	//逐个记录位置
	var box=$(".box");
	//结果
	var tmp = ""; 
	box.each(
		function()
		{
			var id=$(this).attr("id");
			//当前元素所在绝对坐标
			var offset = $(this).parent().offset();
			var xPer=(offset.left-imgoff.left+$(this).parent().width()/2)/imgWd;
			var yPer=(offset.top-imgoff.top+$(this).parent().height()/2)/imgHg;
			//记录
			tmp+=id+",";			
			tmp+=yPer+",";
			tmp+=xPer+"$";
		}
	);
	return tmp;
}