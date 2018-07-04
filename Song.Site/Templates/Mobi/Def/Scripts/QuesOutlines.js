$(function () {
    _clacTax(0,"");

});

//计算序号
function _clacTax(pid,prefix){
	$(".outline[pid="+pid+"]").each(function(index, element) {
        var tax=$(this).find(".tax");
		tax.html(prefix+(index+1)+".");
		var olid=$(this).attr("olid");
		_clacTax(olid,tax.text());
    });
}

