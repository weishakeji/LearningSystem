$(function () {
    //自动计算图片的高度，以方便显示圆形
    $(".thPhoto").each(function (index, element) {
        var wd = $(this).innerWidth();
        $(this).height(wd * 0.95);
    });
    setStar();
	setEvent();
	//点击查看教师详情
	mui('.mui-card').on('tap', '.teacher', function(){
		var name=this.getAttribute("thname");		//教师名称
		var thid=this.getAttribute("thid");		//教师id
		new PageBox(name, "Teacher.ashx?id="+thid, 100, 100, "url").Open();
	});
	mui('.mui-scroll-wrapper').scroll({
		deceleration: 0.0005 //flick 减速系数，系数越大，滚动速度越慢，滚动距离越小，默认值0.0006
	});
});

//设置星标
function setStar() {
    $("span.star").each(function (index, element) {
        //显示多少个星标
        var size = Number($(this).attr("size"));
        if (isNaN(size)) size = 5;
        //当前得分
        var score = Number($(this).attr("score"));
        if (isNaN(score)) return true;
        score = score / (10 / size);
        //生成空的星标
        while (size-- > 0) $(this).append("<i></i>");
        //生成实的星标
        var tm = Math.floor(score);
        $(this).find("i:lt(" + tm + ")").addClass("s1");
        //生成半实的星标
        if (score > tm) $(this).find("i:eq(" + tm + ")").addClass("s0");
		 $(this).append("<b>"+$(this).attr("score")+"分</b>");
    });
}
//设置相关事件
function setEvent(){
	mui('.mui-card').on('tap', 'span.star', function(){
		var name=this.getAttribute("name");		//教师名称
		var thid=this.getAttribute("thid");		//教师id
		new PageBox("请对“"+name+"”进行评价", "TeacherComment.ashx?thid="+thid, 100, 100, "url").Open();
	});
}