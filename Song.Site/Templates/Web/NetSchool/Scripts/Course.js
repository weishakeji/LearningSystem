$(function(){
    tabEvent();
});
//选项卡切换
function tabEvent(){	
	$(".tabArea").each(function(index, element) {
		$(this).find(".tab-bar>div").click(function(){
			$(this).parent().find(">div").removeClass("titOver");
			$(this).parent().parent().find(".tab-context>div").hide();
			$(this).addClass("titOver");
			var index = $(this).parent().find(">div").index(this); 
			$(this).parent().parent().find(".tab-context>div:eq("+index+")").show();	
		});
		//默认第一个显示
		//$(this).find(".tab-bar>div:eq(0)").click();
    });
}

$(function(){
    initEvent();
    TreeEvent();
});
//页面初始事件
function initEvent(){
    //点击选择学习的按钮
    var btn=$("a[state=noSelected]");
    $("a[state=noSelected]").click(function(){
        var url=$(this).attr("href");
		window.msgUrl=url;
        //当前学员是否通过审核
        var isPass=$(this).attr("isPass")=="False" ?  false : true;
        if(isPass) {
            var txt = "您是否确认学习当前课程？";
            var msg = new MsgBox("选择课程", txt, 400, 300, "confirm");
            msg.EnterEvent = function () {
                window.location.href = window.msgUrl;                
            };
            msg.Open();
        }else {
            var txt = "您还没有通过审核，无法进行课程学习。<br/>如果您确认已经通过审核，请重新登录。";
            var msg = new MsgBox("学员未通过审核", txt, 400, 200, "alert");
            msg.Open();
        }
        return false;
    });
}
//树形菜单事件，如打开折叠等
function TreeEvent(){
    var tree=new Tree();
    //当点击节点前的图标时，如加减号文件夹图标
    $("div[type='text']").prev("div[IsChilds='True']").click(
        function()
        {
            //栏目图标，文件夹或文件图标
            var last=$(this).find("img:last");
            //节点图标，加号或┝号
            var pre=last.prev();
            tree.tranImg(pre,"plus.gif","minus.gif");
            tree.tranImg(pre,"minusbottom.gif","plusbottom.gif");
            tree.tranImg(last,"folderopen.gif","folder.gif");
            $(this).parent().next().slideToggle();
            var state=$(this).attr("state");
            $(this).attr("state", state=="true" ? "false" : "true");
        }
    );
    //节点文本双击事件，展开与折叠
    $("div[type='text']").dblclick(
        function()
        {
            //其实就是，当前节点前面图标的事件
            $(this).prev("div[IsChilds='True']").click();
        }
    );
    //根节点的点击事件
    $("div[type='rootText']").click(function(){
        var frame=$("#knlframe");
        var file=frame.attr("targetFile");
        frame.attr("src",file);
    });
    //其余节点的点击事件
    $("div[type='text']").click(function(){
        var id=$(this).attr("nodeId");
        var frame=$("#knlframe");
        var file=frame.attr("targetFile")+"&sort="+id;
        frame.attr("src",file);
    });
}