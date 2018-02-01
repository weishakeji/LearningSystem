$(function(){
    var tr=$(".gvList tr");
    tr.each(function(){
        var left=$(this).find(".mLeft");
        var right=$(this).find(".mRight");
        var hg=left.height()>right.height() ? left.height() : right.height();
        left.height(hg);
        left.css({"min-height":"160px"});
        right.height(hg);
    });
});
