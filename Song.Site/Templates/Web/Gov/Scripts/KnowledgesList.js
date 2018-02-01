$(function(){
   // linkEvent();
});

function linkEvent() {
    $(".item a").click(function(){
        var file=$(this).attr("href");
        var name=$(this).text();
        new top.PageBox('知识库：'+name,file,1200,80).Open();
        return false;
    });
}