! function () {
    /* 使 apiready 更优雅. */
    window.apiready = function(){
        var evt = document.createEvent("HTMLEvents");
        evt.initEvent("apiready", false, false);

        document.dispatchEvent(evt);
        window.dispatchEvent(evt);
    }

    /* 使事件机制更加优雅. */
    window.addEventListener("apiready", function(){
        var apicloudEvents = ["pause", "resume", "online", "offline", "batterylow", "batterystatus", "scrolltobottom",
            "viewappear", "viewdisappear", "keyback", "keymenu", "tap", "swipeleft", "swiperight", "swipeup",
            "swipedown", "shake"];

        for(var idx in apicloudEvents){
            var apicloudEventName = apicloudEvents[idx];

            ! function(eventName){
                api.addEventListener({
                    name: eventName
                }, function(ret, err){
                    var evt = document.createEvent("HTMLEvents");
                    evt.initEvent(eventName, false, false);

                    for(var prop in ret){
                        evt[prop] = ret[prop];
                    }

                    document.dispatchEvent(evt);
                    window.dispatchEvent(evt);
                });
            }(apicloudEventName);
        }
    });


    window.addEventListener("apiready", function(){
        /* 重写 alert 方法. */
        window.alert = function(msg){
            api.alert({
                title: "提示",
                msg: msg,
                buttons: ["确定"]
            });
        }

        /* 一个获取导航栏高度的方法. */
        window.getNavHeight = function () {
            var h = 44;

            if("ios" == api.systemType && parseInt(api.systemVersion) >= 7){
                h = 64;
            }

            return h;
        }

        /* 一个关闭窗口,返回上一级页面的优雅实现. */
        window.closeWin= function(winName) {
           var winName = winName || api.winName

            var animationType = "ripple"; // iOS 使用 滴水效果动画
            var subType = "none";
            var duration = 1000;

            if("ios" != api.systemType){
                animationType = "movein"; // Android
                subType = "from_left";
                duration = 300;
            }

            api.closeWin({
                name: winName,
                animation: {
                    type: animationType,
                    subType: subType,
                    duration: duration
                }
            });
        }
    });

}()