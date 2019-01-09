$(function () {
    //底部按钮的事件
    mui('body').on('tap', '.cour-footer a', function (event) {
        var target = this.getAttribute("target");
        if (target == null) document.location.href = this.href;
        if (target == "_top") top.location.href = this.href;
    });
    //主内容选项卡
    mui('body').on('tap', '.control .control-item', function (event) {
        $(".control .control-item").removeClass("active");
        $(this).addClass("active");
        var index = $(".control .control-item").index(this);
        $(".control-content>div").hide();
        $(".control-content>div:eq(" + index + ")").show();
    });
    //自动计算图片的高度，以方便显示圆形
	 $(".thPhoto").each(function(index, element) {
		var wd=$(this).innerWidth();
        $(this).height(wd*0.95);
    });

});
window.onload=function(){
      $(".thPhoto").each(function(index, element) {
		var wd=$(this).innerWidth();
        $(this).height(wd*0.95);
    });
}