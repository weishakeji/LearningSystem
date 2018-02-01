//设置浮动块
//element:浮动窗的id
//position:基准位置，选项left、right或center,窗口左侧还是右侧，还是中心；默认是left
//distance:距离，离基准位置的距离，可以为正负值
//top:离窗口顶的距离，可以设置参照坐标，如{ target: "#pagerTitle", top: 50 }
//initTop:离窗口顶的初始位置
jQuery.fn.FloatBox = function (element, position, distance, top, initTop) {
    var maxWidth = $(document).width() == null || $(document).width() < 1 ? $(window).width() : $(document).width();
    maxWidth = document.documentElement.clientWidth;
    //创建要控制的对象
    if (typeof (element) == "string") element = $(element);
    if (element.size() < 1) return;
    //初始化一些值
    position = position != null && position != "" ? position : "left";
    distance = distance != null ? distance : maxWidth / 2;
    //离窗口上方的初始距离
    if (initTop == null) {
        var off = element.offset();
        initTop = off.top;
    }
    //离窗口顶部最小多少时，开始移动
    var minTop = initTop;
    if (typeof (top) == "object") {
        var off = $(top.target).offset();
        minTop = off.top;
        top = top.top;
    }
    //计算浮动窗体在左侧位置（坐标）
    var left = 0;
    if (position == "left") left = distance;
    if (position == "right") left = maxWidth - distance - element.width();
    if (position == "center") left = maxWidth / 2 + distance;
    //设置浮动窗的属性，主要是设置坐标
    element.css({ position: "absolute",
        "z-index": 1,
        left: left,
        top: initTop
    });
    $(window).scroll(function () {
        var offsetTop = $(window).scrollTop();
        if (offsetTop > minTop)
            element.animate({ top: offsetTop + top }, { duration: 500, queue: false });
        else
            element.animate({ top: initTop }, { duration: 500, queue: false });
    })
}

/*以下为操作实例*/
//$(document).ready(function(){
//	$().FloatBox("#Ols","center",-520,140);
//	$().FloatBox("#qrcode","center",520,140);
//}); 
//$(document).ready(function () {
//    $().FloatBox("#pagerTitle", "center", -490, 2, 370);
//    $().FloatBox("#pagerCard", "center", 230, { target: "#pagerTitle", top: 50 }, 420);

//}); 