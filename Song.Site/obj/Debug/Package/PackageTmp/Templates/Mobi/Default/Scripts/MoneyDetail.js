$(function () {	
	modify_Remark();	
});

//修改学员姓名
function modify_Remark(){
	$("#btnRemark").click(function(){	
		mui('#btnRemark').button('loading');
		var remark=$("#tbRemark").val();	
		var id = $().getPara("id");	
		$.post("MoneyDetail.ashx?id="+id, { action: "remark",remark: remark },function(data){	
			if(data=="1"){		
				mui.toast('修改成功！',{ duration:1000, type:'div' }); 
			}else{
				mui.toast('系统错误！',{ duration:1000, type:'div' }); 
			}
			mui('#btnRemark').button('reset');
		});	
	});
}
