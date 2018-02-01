$(
 	function()
	{
		setEvent();
	}
 );
//设置图片列表中的文件大小，带单位，如kb，mb
function setEvent()
{
	$("input[id$='btnBack']").click(
		function()
		{
			window.parent.closeFrame();
		}
	);
}
