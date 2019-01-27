$(function () {
	//顶部主菜单切换
	$(".menu-box>div").click(function(){
		var type=$(this).attr("class");
		$("div[area][area!="+type+"]").slideUp(300,function(){
			$("div[area][area="+type+"]").slideDown(300);
		});
	});
    //参加考试
    $(".btnStart").click(function () {
        var examid = $(this).attr("examid");
        document.location.href = $().setPara("examing.ashx", "id", examid);
    });
    //查看成绩
    mui('body').on('tap', 'a.btnReview', function (event) {
        var href = $(this).attr("href");
        var name = $(this).attr("title");
        new PageBox("成绩：《" + name+ "》", href, 100, 100, window.name, "url").Open();
    });
	//得分计算是否极格
	$(".score").each(function () {
        var total = Number($(this).attr("total"));	//总分
		var pass = Number($(this).attr("pass"));	//及格分
		var score = Number($(this).attr("score"));	//实际得分
		//不及格
        if(score<pass)$(this).addClass("nopass");
		//及格，但还挂不上优秀
		if(score>=pass && score<((total-pass)*0.7+pass))$(this).addClass("yespass");
		//及格，且优秀
		if(score>=((total-pass)*0.7+pass))$(this).addClass("excellent");
    });
});