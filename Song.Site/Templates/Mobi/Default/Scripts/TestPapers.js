
$(function () {
    _initLoyout();
});

function _initLoyout() {
    var row = $(".row");
    //设置行的事件与样式
    row.each(function () {
        var width = $(this).width();
        width = width - $(this).find(".rowlogo").outerWidth() - $(this).find(".rightbox").outerWidth()-22;
        $(this).find(".rowtxt").width(width);
    });
	mui('body').on('tap', '.row', function () {
        var href = $(this).attr("href");
         new PageBox("试卷详情", href, 100, 100, "url").Open();
    });  
	$(".rowlogo img").error(function(){
		$(this).hide();
		$(this).parent().find(".ico").show();
	});  
}
