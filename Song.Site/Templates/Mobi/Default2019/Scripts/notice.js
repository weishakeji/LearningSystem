$(function () {    
	//搜索框的提交事件
	 $("#formSearch").submit(function(){
	 	var txt=$("#tbSearch").val();
		 if($.trim(txt)=="")return false;
	 });
	 mui('body').on('tap', '.btnSearch', function () {
		 var txt=$("#tbSearch").val();
		 if($.trim(txt)=="")return false;
		 var href=$(this).prev("form").attr("action");
		 window.location.href=$().setPara(href,"sear",txt);
	});
});