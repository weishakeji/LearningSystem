$(function() {
	  loyoutint();
	  _jishuanSize();
});

function loyoutint()
{
	//常用的来源项
	var oftenSource=$("#oftenSource");
	var str="";
	var arrSource=oftenSource.text().split(",");
	for(var n in arrSource)
	{
		str+=" &nbsp; <a href=\"#\" type=\"source\">"+arrSource[n]+"</a>";
	}
	oftenSource.html(str);
	oftenSource.find("a[type='source']").click(
		function()
		{
			$("input[id$='tbSource']").val($(this).text());
			return false;
		}
	);
}

function _jishuanSize()
{
	$(".size").each(function(){
		var n=Number($(this).text());
		var tm=$().getSizeUnit(n);
		$(this).text(tm);
	});
}