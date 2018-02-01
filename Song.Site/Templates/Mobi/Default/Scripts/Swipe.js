$(function () {
    $("dl dd").each(function (index, element) {
        var n = 100;
        while (--n > 0) {
            $(this).get(0).innerHTML += (index + 1) + " - " + n + "<br/>";
        }
    });
    $("body").css("height", document.body.scrollHeight);
    $("body").css("width", document.documentElement.clientWidth);
    $("dl").css("height", document.body.scrollHeight - $('dl').position().top);
    $("dd").css("height", document.body.scrollHeight - $('dl').position().top);
    $("body").swipe({ fingers: 'all', swipeLeft: swipeFuc, swipeRight: swipeFuc });
    /*
    $("body").swipe({
    swipeStatus: function (event, phase, direction, distance, duration, fingerCount) {
    //if(direction=="up")return;
    $(".txt").text("你用"+fingerCount+"个手指以"+duration+"秒的速度向" + direction + "滑动了" +distance+ "像素 " +"你在"+phase+"中");
    },
    swipeLeft: function (event, direction, distance, duration, fingerCount, fingerData, currentDirection) {
    $(".txt").text("你用" + fingerCount + "个手指以" + duration + "秒的速度向" + direction + "滑动了" + distance + "像素");
    },
    swipeRight: function (event, direction, distance, duration, fingerCount, fingerData, currentDirection) {
    $(".txt").text("你用" + fingerCount + "个手指以" + duration + "秒的速度向" + direction + "滑动了" + distance + "像素");
    }
    });*/

});
var fixLeft = 0;
function swipeFuc(event, direction, distance, duration, fingerCount) {
    fixLeft = Number($("dl").css("left").replace("px", ""));
    fixLeft = isNaN(fixLeft) ? 0 : fixLeft;

    var tm = $("dd").width();
    if (direction == "left") {
        if (Math.abs(fixLeft) < tm * ($("dd").size() - 1)) {
            $("dl").animate({ left: fixLeft - tm });
        }
    }
    if (direction == "right") {

        if (fixLeft < 0)
            $("dl").animate({ left: fixLeft + tm });
    }
    $(".txt").text("你用" + fingerCount + "个手指以" + duration + "秒的速度向" + direction + "滑动了" + distance + "像素");
}
/*
swipeLeft: function(event, direction, distance, duration, fingerCount, fingerData) {              

}*/