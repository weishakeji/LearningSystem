$(taskInit);
//常规参数
//等级颜色,按顺序1-5级；
var levelColor=["333333","333333","6600FF","ff0000","ff0000"];
//完成度的进度条颜色，按顺序1-10级；红、橙、黄、灰、绿、蓝
var CompleteColor=["ff0000","FF3300","FF9900","333333","009900","0066FF"];

//页面初始化
function taskInit()
{
	setRemainingTime();
	setLevelColor();
	setTitleLoyout();
	setCompleteBarColor();
}
//设置剩余时间是否显示为红色
function setRemainingTime()
{
	var span=$("span[type='RemainingTime']");
	span.each(
		function()
		{
			//获取时间值，并转换成数字
			var s=$(this).find("span:first");
			var num=Number(s.text());
			if(num<0)
			{				
				$(this).attr("class","redTxt");
			}
		}
	);
}
//设置等级颜色
function setLevelColor()
{
	//levelColor=levelColor.reverse();
	var span=$("span[type='level']");
	span.each(
		function()
		{
			//获取当前任务的等级值
			var num=Number($(this).text());
			var color=levelColor[num-1];			
			$(this).css({ color: "#"+color}); 
		}
	);
}
//设置完成度的进度条颜色
function setCompleteBarColor()
{
	//设置完成度进度条颜色
	var bar=$(".compBar");
	bar.each(
		function()
		{
			//获取当前任务的等级值
			var complete=Number($(this).attr("complete"));
			if(complete<1)complete=1;
			if(complete>=100)complete=100;
			var num=Math.floor(complete/20);			
			var color=CompleteColor[num];			
			//$(this).css({ background: "#"+color,width:complete+"%"}); 
			$(this).css({ background: "#"+color,width:"1px"});
			$(this).animate({
				width: complete+"%",						
				},2000);
			var alpha=70;
			$(this).css("filter","Alpha(Opacity="+alpha+")");	
			$(this).css("-moz-opacity",alpha/100);
			$(this).css("opacity",alpha/100);
		}
	);
	//完成度数值的颜色
	var span=$(".CompletePer");
	span.each(
		function()
		{
			//获取时间值，并转换成数字
			var s=$(this).find("span:first");
			var complete=Number(s.text());
			var num=Math.floor(complete/20);
			var color=CompleteColor[num];
			$(this).css({ color: "#"+color});
		}
	);
}
//设置任务标题的位置
function setTitleLoyout()
{
	var tit=$(".taskTitle");
	tit.each(
		function()
		{
			//设置任务标题的位置
			var id=$(this).attr("tag");
			var bar=$(".compBar[tag='"+id+"']");	
			var offset = bar.parent().offset();
			$(this).css({ left: offset.left,top:offset.top}); 
			//设置任务的颜色，按level等级
			var num=Number($(this).attr("level"));
			var color=levelColor[num-1];			
			$(this).css({ color: "#"+color}); 
		}
	);
}