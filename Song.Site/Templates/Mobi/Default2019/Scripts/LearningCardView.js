$(function(){
	//使用按钮事件
    $("#btnUseCard").click(function () {
        var card=$("#card").text();		
        //调用父窗体方法        
        top.window.set_card_use(card); //卡码与密码
        //关闭窗口
        new top.PageBox().Close(window.name);
        return false;
    });
	//暂存按钮事件
    $("#btnGetCard").click(function () {
        var card=$("#card").text();		
        //调用父窗体方法        
        top.window.set_card_get(card); //卡码与密码
        //关闭窗口
        new top.PageBox().Close(window.name);
        return false;
    });
});