$(function(){
    setImg();
});
function setImg(){
	$("#artContext img").each(function(index, element) {
		//if($(this).width()<100)return true;
        $(this).wrap("<div class='img-box'></div>");
		$(this).parentsUntil("#artContext").css("text-indent","0px");
    });
}