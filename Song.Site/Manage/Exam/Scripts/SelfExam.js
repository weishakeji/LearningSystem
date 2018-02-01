$(function(){
	//提示信息
	$("div[tag=group]").each(function(){		
		var txt=$.trim($(this).text());
		$(this).attr("title",txt);
	});
	setExamInitEvent();
});

//设置初始化的事件
function setExamInitEvent()
{
	//考试的点击事件，点击后进入考场
	$("#todayExam").find(".examBox").click(function(){
		//当前考试（场次）的id
		var examid=$.trim($(this).attr("examid"));
		//考试主题
		var theme=$.trim($(this).find("div[tag=theme]").text());
		//考试学科
		var subject=$.trim($(this).find("div[tag=subject]").text());
		//
		var winPathUrl="/manage/exam/examing.aspx?id="+examid+"&timestamp=" + new Date().getTime();
		var title="考试主题：《"+theme+"》 　　　课程/专业："+subject;
		var p=new parent.PageBox(title,winPathUrl,100,100,"555");
		p.IsDrag=false;
		p.Open();
		p.HideClose();
		//OpenWin(winPathUrl,title,100,100);
	});
}