$(function(){
	$(".item img").click(function(){
		var thisEle=$(this).parent();
		var id=thisEle.attr("couid");
		var width=parseInt(thisEle.attr("wd"));
		var height=parseInt(thisEle.attr("hg"));
		//
		//当前页面的上级路径，因为子页面没有写路径，默认与本页面同路径
		var url=String(window.document.location.href);		
		url=url.substring(0,url.lastIndexOf("/")+1); 
		var boxUrl=url+"List_Edit.aspx?id="+id;
		new top.PageBox("课程信息编辑",boxUrl,width,height).Open();
	});
});