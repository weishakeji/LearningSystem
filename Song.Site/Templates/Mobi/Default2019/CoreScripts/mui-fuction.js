function plusReady() {
    var self = plus.webview.currentWebview();
}
if (window.plus) {
    plusReady();
} else {
    document.addEventListener("plusready", plusReady, false);
}
$(function () {
    verifyCode();
    APIClound_Event();
    mui('body').on('tap', 'a', a_click);
});

//当点击验证码时，刷新验证码
function verifyCode() {
    mui("body").on('tap', '.verifyCode', function () {
        var src = $(this).attr("src");
        if (src.indexOf("&") < 0) {
            src += "&timestamp=" + new Date().getTime();
        } else {
            src = src.substring(0, src.lastIndexOf("&"));
            src += "&timestamp=" + new Date().getTime();
        }
        $(this).attr("src", src);
    })
}
//apicloud的事件添加
function APIClound_Event() {
    //apicloud事件必须写在apiready里面
    apiready = function () {
        //添加手机上的返回键事件
        api.addEventListener({ name: 'keyback' }, function (ret, err) {
            if (ret) {
                //ret内有两个属性：keyCode（值为4），longPress(是否长按)
                if (!ret.longPress) {
                    event_goback(ret);
                } else {
                    api.closeWin(); //长按返回键，关闭软件
                }
            } else {
                //alert(JSON.stringify(err));
            }
        });
        //主菜单事件
        api.addEventListener({ name: 'keymenu' }, function (ret, err) {
            if (ret) {
                //ret内有两个属性：keyCode（值为82），longPress(是否长按)
                if (!ret.longPress) {
                    //侧滑容器父节点
                    var offCanvasWrapper = mui('#offCanvasWrapper');
                    offCanvasWrapper.offCanvas('show');
                } else {
                    api.closeWin();
                }
            } else {
                //alert(JSON.stringify(err));
            }
        });
        //长按返回键，关闭软件
        api.addEventListener({ name: 'longpress' }, function (ret, err) {
            //api.closeWin();
        });
        /*
        //监听音量调节
        api.addEventListener({ name: 'volumedown' }, function (ret, err) {
        if (ret) {
        //alert(JSON.stringify(ret));
        } else {
        //alert(JSON.stringify(err));
        }
        });
        */
        //网络断开
        api.addEventListener({ name: 'offline' }, function (ret, err) {
            if (ret) {
                //断开后，该值为none;
                var connectionType = ret.connectionType;
                alert("网络中断，您当前为离线状态。本软件不支持离线学习。");
            } else {
                alert(JSON.stringify(err));
            }
        });
        //网络链接
        api.addEventListener({ name: 'online' }, function (ret, err) {
            if (ret) {
                var type = ret.connectionType;
                //alert("网络联通（" + type + "）,可以继续学习。");
            } else {
                alert(JSON.stringify(err));
            }
        });
    }
}


//当点击手机上返回键时
function event_goback(ret) {
    //如果有弹出窗口，则选关闭窗口
    if ($(".PageBox").size() > 0) {
        var winid = $(".PageBox").attr("winid");
        window.PageBox.Close(winid);
        return;
    }
    //如果有弹出窗口，则选关闭窗口
    if ($(".mui-popup").size() > 0) {
        $(".mui-popup").remove();
        $(".mui-popup-backdrop").remove();
        return;
    }
    //判断侧滑菜单是否打开
    var offCanvasWrapper = mui('#offCanvasWrapper');
    if (offCanvasWrapper.length > 0) {
        var isShow = offCanvasWrapper.offCanvas().isShown();
        //如果打开，则选先关闭菜单，再点时返回上一页
        if (isShow) {
            offCanvasWrapper.offCanvas().close();
            return;
        }
    }
    //
    window.history.go(-1);

   
    //如果快速双击返回键，关闭软件
    window.appback_num = typeof (window.appback_num) == "undefined" ? 0 : window.appback_num;
    window.appback_time = typeof (window.appback_time) == "undefined" ? new Date() : window.appback_time;
    if ((new Date().getTime() - window.appback_time.getTime()) < 300) { window.appback_num++; }
    else { window.appback_num = 0; }
    window.appback_time = new Date();
    if (window.appback_num % 2 == 1) api.closeWin();    

}

