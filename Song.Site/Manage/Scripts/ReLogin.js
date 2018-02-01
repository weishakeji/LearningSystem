$(document).ready(function(){
  $("input[name$='tbAccName']").focus();
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
}); 

//提交的事件
function Event()
{
	var form=$("form");		
	form.submit(
		function()
		{
			var show=$("#showtext");
			show.html("");
			var acc=$("input[name$='tbAccName']");
			if($.trim(acc.val())=="")
			{
				show.html("工号不得为空！");
				acc.focus();
				acc.val("");
				return false;
			}
			var pw=$("input[name$='tbPw1']");
			if($.trim(pw.val())=="")
			{
				show.html("请输入密码！");
				pw.focus();
				pw.val("");
				return false;
			}
			var code=$("input[name$='tbCode']");
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