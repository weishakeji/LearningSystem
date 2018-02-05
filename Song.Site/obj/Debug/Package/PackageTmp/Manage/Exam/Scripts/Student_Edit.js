$(function(){				
	setEvent();
});

function setEvent()
{
	var tbName=$("input[id$='tbName']");
	var tbNamePinyin=$("input[id$='tbNamePinjin']");
	tbName.focusout(tranHanzi);
	tbNamePinyin.focus(tranHanzi);
	$("input[id$='btnEnter']").click(tranHanzi);
}
//转换汉字
function tranHanzi()
{
	var tbName=$("input[id$='tbName']");
	var tbNamePinyin=$("input[id$='tbNamePinjin']");
	var duoyinBox=$("#namePinjin");
	var duoyin=$("#duoyin");
	duoyin.html("");
	//转换为拼音缩写，返回为数组
	var arrRslt = makePy(tbName.val().trim());
	//如果缩写已经存在
	var isHav=false;	
	for(r in arrRslt)
		if(arrRslt[r]==tbNamePinyin.val().trim())isHav=true;
	//如果不存在，则赋值
	if(!isHav)tbNamePinyin.val(arrRslt[0]);
	//如果只有一个选择，结束操作
	if(arrRslt.length<=1)
	{
		duoyinBox.hide();
		return;
	}
	//如果是多音字
	for(r in arrRslt)
	{
		duoyin.append("<a href=\"#\">"+arrRslt[r]+"</a>&nbsp;&nbsp;");
	}
	duoyin.find("a").click(
		function()
		{
			tbNamePinyin.val($(this).text());
			return;
		}
	);
	duoyinBox.show();
}