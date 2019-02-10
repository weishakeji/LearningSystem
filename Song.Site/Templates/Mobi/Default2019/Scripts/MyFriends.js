$(function () {
    //查看下级会员详情
    mui('body').on('tap', '.btnFriend', function () {
        new PageBox("我的朋友", "Subordinates.ashx", 100, 100, "url").Open();
    });
    //帮助说明
    mui('body').on('tap', '.btnHelp', function () {
        var txt = $(".help-box .explain").get(0).outerHTML;
        var msg = new MsgBox("<span class='iconfont'>&#xe658;</span>&nbsp;说明", txt, 80, 80, "msg");
        msg.ShowCloseBtn = false;
        msg.OverEvent = function () {            
        };
        msg.Open();
    });
	//分享
    mui('body').on('tap', '.btnShare', function () {
        var txt = $(".share-box .share").get(0).outerHTML;
        var msg = new MsgBox("<span class='iconfont'>&#xe64e;</span>&nbsp;分享", txt, 80, 60, "msg");
        msg.ShowCloseBtn = false;
        msg.OverEvent = function () {            
        };
        msg.Open();
    });
	
});

