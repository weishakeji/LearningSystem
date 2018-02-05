$(
 	function()
	{
		_openPicEditEvent();
	}
 );
//设置图片列表中的文件大小，带单位，如kb，mb
function setSize()
{
	$(".piSize").each(
		function()
		{
			var num=Number($(this).text());
			var str=$().getSizeUnit(num);
			$(this).html(str);
		}
	);
}

function _openPicEditEvent()
{
	$(".picShow").click(function(){
		var id=$(this).attr("imgid");
		var para=location.href.substring(location.href.indexOf("?"));
		var path="Content/Picture_Edit.aspx"+para+"&id="+id;
		new top.PageBox("内容管理---图片编辑",path,800,600).Open();
	});
}