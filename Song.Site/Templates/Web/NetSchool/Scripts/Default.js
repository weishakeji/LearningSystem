$(function () {
    SubjectPanelInit();   
	courseTabs(); 
	//
	//var ua = window.navigator.userAgent.toLowerCase();
	//document.write(ua);
});
/*
	首页专业展示面板
	写于 2017-01-05
	作者： 宋雷鸣 qq10522779
*/
//专业展示面板的初始化
function SubjectPanelInit() {	
    //当鼠标滑过主菜单
    $(".sbj-top-item").hover(function () {
        $(this).parent().children().removeClass("sbjOver");
        $(this).addClass("sbjOver");
        hideSbjpanel(-1); //隐藏所有下拉菜单        	
        showSbjpanel($(this));//显示下拉菜单	
        window._sbjHover = true;		
    }, function () {        
        setTimeout('hideSbjpanel()', 500);
		window._sbjHover = false;		
    });   
	 $(".sbj-two-panel").hover(function () {
		 window._sbjHover = true;	
	 }, function () {        
        setTimeout('hideSbjpanel()', 500);
		window._sbjHover = false;		
    });
}
//显示下拉菜单
//root:根菜单的Html节点对象
function showSbjpanel(root) {    
    var pid = root.attr("sid");//上级id
    var menuBox = $(".sbj-two-panel[pid=" + pid + "]");
    if (menuBox.size() < 1) return;
	//设置高度
	var border=parseInt(menuBox.css("border-width"));	//窗体边线宽度
	menuBox.height(root.parents().height()-border*2);
	menuBox.show();	
}
//隐藏下级菜单，包括下级菜单的所有下级，递规查找
function hideSbjpanel(pid) {
    if (pid==-1)$(".sbj-two-panel").hide();
	if ((pid == null || typeof (pid) == "undefined") && !window._sbjHover) {
        $(".sbj-two-panel").hide();
		$(".sbj-top-item").removeClass("sbjOver");
    }
}

//课程切换
function courseTabs() {
    $("div[type='tab']").each(function (index) {
        $(this).find("*[type='area']:first").show();
        $(this).find("*[type='title']:first").addClass("titleOver");
        //选项卡
        $(this).find("*[type='title']").click(function () {
            //隐藏所有内容区
            $(this).parents("*[type='tab']").find("*[type='area']").hide();
            $(this).parents("*[type='tab']").find("*[type='title']").removeClass("titleOver");
            //设置当前效果
            var index = $(this).attr("index");
            $(this).parents("*[type='tab']").find("*[type='area'][index=" + index + "]").show();
            $(this).addClass("titleOver");
            return false;
        });
    });
}