$(function(){
	//参加考试
	$(".btnStart").click(function(){
		var examid=$(this).attr("examid");
		 document.location.href = $().setPara("examing.ashx", "id", examid);
	});
});