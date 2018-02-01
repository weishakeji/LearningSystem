//全局变量
//总票数
var SumNumber=0;
//得标百分比的进度条颜色，按顺序1-10级
var CompleteColor=["666666","FF9900","009900","FF3300","0033FF","ff0000"];
//实始化方法
$(
	function()
	{		
		SumNumber=GetSumNumber();
		SetPer();
		SetFormat();	
		$("input[id$='btnEnter'").click(
				function()
				{
					var tb=$("input[name$='tbItemName']");
					var num=0;
					tb.each(function(){
						if($(this).val()!="")
						{
							num++;
						}
					});
					if(num<1)
					{
						alert("至少需要一个选择项，建议填写两个选择项。");
						return false;
					}
				}
			);	
	}  
 );

//计算总票数
function GetSumNumber()
{
	var sum=0;
	var vtNum=$(".vtNumber .num");
	vtNum.each(
		function()
		{
			var n=$(this).html();
			if(Number(n))
			{
				sum+=Number(n);
			}
		}
	);
	return sum;
}
//设置百分比
function SetPer()
{	
	var vtNum=$(".vtNumber");
	vtNum.each(
		function()
		{
			//百分比的显示条
			var bar=$(this).prev();
			if(SumNumber==0)
			{
				//如果总票为0；
				bar.hide();
				return;
			}
			var n=$(this).find(".num").html();
			if(Number(n))
			{				
				var num=Number(n);
				var per=Math.round((num/SumNumber*10000))/100;	
				//设置进度条长度		
				bar.animate({
				width: per+"%",						
				},1000);
				//bar.css("width",per+"%");
				var color=CompleteColor[Math.floor(per/20)];				
				bar.css({background: "#"+color});
				var alpha=60;
				bar.css("filter","Alpha(Opacity="+alpha+")");	
				bar.css("-moz-opacity",alpha/100);
				bar.css("opacity",alpha/100);
				//设置百分比数值
				$(this).find(".per").html(per+" %");
			}else
			{
				bar.hide();
				$(this).find(".per").html("0 %");
			}
		}
	);
}
//设置一些格式
function SetFormat()
{
	//如果选择项为空，如空调查项，不显示得票数
	var tbItem=$("input[id$='tbItemName']");
	tbItem.each(
		function()
		{
			var val=$(this).val();
			if(val=="")
			{
				$(this).parent().next().html("");
			}
		}
	);
	//设置得票数信息，显示在百分比条上
	var vtNum=$(".vtNumber");
	vtNum.each(
		function()
		{			
			//进度条
			var bar=$(this).prev();
			var offset = bar.parent().offset();
			$(this).css({ left: offset.left,top:offset.top}); 			
		}
	);
}