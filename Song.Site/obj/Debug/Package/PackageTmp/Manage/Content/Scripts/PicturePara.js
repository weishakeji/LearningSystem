$(
  function()
  {
	  setLoyout();
	  setEvent();
  }
);

//设置初始部局
function setLoyout()
{
	var local=getLocal();
	$(".place td").each(
		function()
		{			
			if($.trim($(this).text())==local)
				$(this).attr("class","current");
		}
	);
	$("#spanLocal").text(local);
}
//获取设置的水印图片所处位置，并转为中文
function getLocal()
{
	var local=$("input[name$='tbLocal']");
	local.hide();
	var txt=local.val();
	txt=txt.replace("left","左");
	txt=txt.replace("right","右");
	txt=txt.replace("top","上");
	txt=txt.replace("down","下");
	txt=txt.replace("center","中");	
	return $.trim(txt);
}
function setEvent()
{
	$(".place td").click(
		function()
		{
			var txt=$(this).text();
			txt=txt.replace("左","left");
			txt=txt.replace("右","right");
			txt=txt.replace("上","top");
			txt=txt.replace("下","down");
			txt=txt.replace("中","center");
			$("input[name$='tbLocal']").val(txt);
			//
			$("#spanLocal").text($(this).text());
			$(".place td").removeClass("hover");
			$(this).addClass("hover");
		}
	);
}
function setLocal()
{
	var local=$("input[name$='tbLocal']");
	alert(local.val());
}