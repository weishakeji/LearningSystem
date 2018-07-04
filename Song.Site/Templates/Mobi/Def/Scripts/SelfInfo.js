$(function () {
	modify_birthday();	
	modify_name();
	modify_sex();
    modify_link();
	modify_safe();
	modify_sign();
});

//修改年龄的操作
function modify_birthday(){
	$("#birthday_select").click(function(){		
		var picker = new mui.DtPicker({
			type: "date",
			endDate: new Date(),
			beginDate: new Date(1949, 01, 01)
		});
		//默认值
		var def=$(this).attr("default");
		picker.setSelectedValue(def)  
		picker.show(function(rs) {
			$.post(window.location.href, { action: "birthday", date: rs.text },function(){
				 document.location.href = window.location.href;
			});						
			picker.dispose();
		});
	});
}
//修改学员姓名
function modify_name(){
	$("#btn_stname").click(function(){	
		mui('#btn_stname').button('loading');
		var tbname=$("#tbname").val();		
		$.post(window.location.href, { action: "name", stname: tbname },function(){
			$(".stname").text(tbname);			
			mui.toast('修改成功！',{ duration:1000, type:'div' }); 
			mui('#btn_stname').button('reset');
		});	
	});
}
//修改性别
function modify_sex(){
	$("input[name=radiosex]").click(function(){	
		var val=Number($(this).val());		
		$(this).attr("disabled","disabled");
		$.post(window.location.href, { action: "sex", sex: val },function(){
			$("#stsex").text(val==1 ? "男" : "女");
			$("input[name=radiosex]").removeAttr("disabled");
			mui.toast('修改成功！',{ duration:1000, type:'div' }); 
		});
	});
}
//修改联系方式
function modify_link(){
	$("#btn-link").click(function(){	
		mui('#btn-link').button('loading');
		var form=$(this).parents("form");
		var mobi=form.find("#tbmobi").val();
		var qq=form.find("#tbqq").val();
		var email=form.find("#tbemail").val();		
		$.post(window.location.href, { action: "link", mobi: mobi,qq:qq,email:email },function(){
				$("#stmobi").text(mobi);				
				mui.toast('修改成功！',{ duration:1000, type:'div' }); 
				mui('#btn_link').button('reset');
		});
	});
}
//修改安全问题
function modify_safe(){
	$("#btn-Safe").click(function(){	
		mui('#btn-Safe').button('loading');
		var form=$(this).parents("form");
		var ques=form.find("#tbQues").val();
		var answer=form.find("#tbAnswer").val();		
		$.post(window.location.href, { action: "safe", ques: ques,answer:answer},function(){			
			mui.toast('修改成功！',{ duration:1000, type:'div' }); 
			mui('#btn-Safe').button('reset');
		});
	});
}
//修改个人签名
function modify_sign(){
	$("#btn-sign").click(function(){	
		mui('#btn-sign').button('loading');
		var form=$(this).parents("form");
		var sign=form.find("#tbSign").val();		
		$.post(window.location.href, { action: "sign", sign: sign},function(){
			$("#sign").text(sign);			
			mui.toast('修改成功！',{ duration:1000, type:'div' }); 
			mui('#btn-sign').button('reset');
		});
	});
}