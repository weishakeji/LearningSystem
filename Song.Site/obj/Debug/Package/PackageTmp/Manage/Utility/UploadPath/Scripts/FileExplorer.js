$(function(){
	fileSelectEvent();
});
//选择文件的事件
function fileSelectEvent()
{
	$(".item").click(function(){
		var file=$.trim($(this).text());
		var path=$.trim($("#ltCuurentPath").text());
		//调用上级页面方法
		top.setSelectFile(file,path);
	});	
}
