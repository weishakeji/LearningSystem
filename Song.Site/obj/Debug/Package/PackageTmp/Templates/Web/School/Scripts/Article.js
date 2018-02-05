$(function(){
    setImg();
});
function setImg(){
	$("#artContext img").each(function(index, element) {
		//if($(this).width()<100)return true;
        $(this).wrap("<div class='img-box'></div>");
    });
}