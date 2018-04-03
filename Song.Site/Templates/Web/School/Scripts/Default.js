var colors=["#E08031","#FF6E97","#199475","#0B6E48","#FF534D","#6C890B"];
$(function(){
    //设置快捷链接的背景色
	$(".link-item").each(function(index, element) {
        var i=index%colors.length;
		var c=colors[i];
		$(this).css("background-color",c);
    });
});

