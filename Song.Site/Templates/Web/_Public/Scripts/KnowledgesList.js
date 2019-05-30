$(function(){
   linkEvent();
});

function linkEvent() {
    $(".item a").click(function(){
        var file=$(this).attr("href");
        var name=$(this).text();
		//父窗体名称
		var window_name=$().getPara("window_name");
        new top.PageBox('知识库：'+name,file,980,80,null,window_name).Open();
        return false;
    });
}