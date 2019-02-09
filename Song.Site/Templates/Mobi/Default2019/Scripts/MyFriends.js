$(function(){
   //查看下级会员详情
    mui('body').on('tap', '.btnFriend', function () {
        new PageBox("我的朋友", "Subordinates.ashx", 100, 100, "url").Open();
    });
});
