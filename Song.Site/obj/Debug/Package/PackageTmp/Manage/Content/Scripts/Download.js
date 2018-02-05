
$(
 	function()
	{
		setSize();
	}
 );
//设置图片列表中的文件大小，带单位，如kb，mb
function setSize()
{
	$(".diSize").each(
		function()
		{
			var num=Number($(this).text());
			var str=$().getSizeUnit(num);
			$(this).html(str);
		}
	);
}
