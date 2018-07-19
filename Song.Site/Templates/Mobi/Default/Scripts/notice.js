$(function () {    
	//搜索框的提交事件
	 $("#formSearch").submit(function(){
	 	var txt=$("#tbSearch").val();
		 if($.trim(txt)=="")return false;
	 });
	 mui('body').on('tap', '.btnSear', function () {
		 var txt=$("#tbSearch").val();
		 if($.trim(txt)=="")return false;
		 var href=$(this).parent("form").attr("action");
		 window.location.href=$().setPara(href,"sear",txt);
	});
});