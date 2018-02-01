//管理后台登录页面的js

$(
  function()
  {	  
		//Loyout();
		Event();
		$("#codeImg").click(
			function()
			{
				var src=$(this).attr("src");
				src=src+"&s=e";
				$(this).attr({src:src});
				//$(this).attr("src","Utility/CodeImg.aspx?len=4&name=login");
			}
		);
  }
 );
$(document).ready(function(){
  //$("#tbAccName").focus();
  $("input[name$='tbAccName']").focus();
}); 
//初始化布布局
function Loyout()
{
	var h=document.documentElement.clientHeight;
	var w=document.documentElement.clientWidth;	
	var panel=$("#loginPanel");
	panel.css("top",(h-panel.height())/2);
	panel.css("left",(w-panel.width())/2);
	//设置拖动
	$("#loginPanel").easydrag();
	$("#loginPanel").setHandler("loginTitle");
	//
	panel.fadeIn(1000);
};
//提交的事件
function Event()
{
	var form=$("#form1");		
	form.submit(
		function()
		{
			var show=$("#showtext");
			show.html("");
			var acc=$("#tbAccName");
			if($.trim(acc.val())=="")
			{
				show.html("帐号不得为空！");
				acc.focus();
				acc.val("");
				return false;
			}
			var pw=$("#tbPw1");
			if($.trim(pw.val())=="")
			{
				show.html("请输入密码！");
				pw.focus();
				pw.val("");
				return false;
			}
			var code=$("#tbCode");
			if($.trim(code.val())=="")
			{
				show.html("请输入验证码！");
				code.focus();
				code.val("");
				return false;
			}
		}
	);
}