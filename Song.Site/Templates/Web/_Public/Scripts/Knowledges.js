$(function(){
	initloyout();
    TreeEvent();
	//设置iframe链接，加上窗体名称
	var src=$("iframe").attr("targetFile");
	$("iframe").attr("src",src+"?window_name="+window.name);
});
//页面初始化布局
function initloyout(){


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
        var frame=$("#frame");
        var file=frame.attr("targetFile");
        frame.attr("src",file+"?window_name="+window.name);
    });
    //其余节点的点击事件
    $("div[type='text']").click(function(){
        var id=$(this).attr("nodeId");
        var frame=$("#frame");
        var file=frame.attr("targetFile")+"?sort="+id;
        frame.attr("src",file+"?window_name="+window.name);
    });
}