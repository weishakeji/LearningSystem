/*!
 * 主  题：《保持页面录入状态》
 * 说  明：页面中的指定控件状态，维持前一次操作的状态。
 * 功能描述：
 * 1、当新增数据时，页面状态状态维持前一次录入的状态，如果是修改数据，则不维持；
 * 2、通过配置控件属性sate="true"，使该控件保持状态。
 *
 * 作  者：宋雷鸣 
 * 开发时间: 2011年8月3日
 */

$(
  function()
  {
	  	//记录状态
		record();
		//是否带有参数,一般新增信息页面不带参数
		var p="";
		var LocString=String(window.document.location.href);
		var rs=new RegExp("(^|).=([^\&]*)(\&|$)","gi").exec(LocString),tmp;
		if(tmp=rs)p= tmp[2];
		//没有参数，页面一般是用于新增的操作，则读取状态
		if(p==null || p=="")
		{
			read();
		}
  }
  
 );
function record()
{
	var form=$("form");
	form.submit(
		function()
		{			
			inputwrite();
			checkwrite();
			selectkwrite();
			textareawrite();
		}
	);
}
//单行文本框写入
function inputwrite()
{
	$("input[state='true']").each(
		function()
		{
			var id=$(this).attr("id");
			if(id.indexOf("_")>-1)
			{
				id=id.substring(id.lastIndexOf("_")+1);
			}
			var vl=$(this).val();
			$().pagecookie(id,vl);
		}
	);
}
//多行文本框
function textareawrite()
{
	$("textarea[state='true']").each(
		function()
		{
			var id=$(this).attr("id");
			if(id.indexOf("_")>-1)
			{
				id=id.substring(id.lastIndexOf("_")+1);
			}
			var vl=$(this).text();
			$().pagecookie(id,vl);
		}
	);
}
//选择框录入
function checkwrite()
{
	$("span[state='true'] input[type='checkbox']").each(
		function()
		{
			var id=$(this).attr("id");
			if(id.indexOf("_")>-1)
			{
				id=id.substring(id.lastIndexOf("_")+1);
			}
			var vl=$(this).prop("checked");
			$().pagecookie(id,vl);
		}
	);
}
//下拉选择框写入
function selectkwrite()
{
	$("select[state='true']").each(
		function()
		{
			var id=$(this).attr("id");
			if(id.indexOf("_")>-1)
			{
				id=id.substring(id.lastIndexOf("_")+1);
			}
			var vl=$(this).val();
			$().pagecookie(id,vl);
		}
	);
}
/*
以下是写入部分

*/

function read()
{
	inputread();
	checkread();
	selectread();
	textarearead();
}
//录入框写入
function inputread()
{
	var btn=$("input[state='true']");
	btn.each(
		function()
		{
			var id=$(this).attr("id");
			if(id.indexOf("_")>-1)
			{
				id=id.substring(id.lastIndexOf("_")+1);
			}
			var vl=$().pagecookie(id);
			if(vl!=null)$(this).val(vl);
		}
	);
}
//复选框读取
function checkread()
{
	$("span[state='true'] input[type='checkbox']").each(
		function()
		{
			var id=$(this).attr("id");
			if(id.indexOf("_")>-1)
			{
				id=id.substring(id.lastIndexOf("_")+1);
			}
			var vl=$().pagecookie(id);
			//alert(typeof(vl));
			if(vl!=null)
			{
				if(vl=="true")$(this).prop("checked","checked");
				if(vl=="false")$(this).removeAttr("checked");
			}
		}
	);
}
//下拉框的读取
function selectread()
{
	$("select[state='true']").each(
		function()
		{
			var id=$(this).attr("id");
			if(id.indexOf("_")>-1)
			{
				id=id.substring(id.lastIndexOf("_")+1);
			}
			var vl=$().pagecookie(id);
			if(vl!=null)$(this).val(vl);
		}
	);
}
//多行文本框
function textarearead()
{
	$("textarea[state='true']").each(
		function()
		{
			var id=$(this).attr("id");
			if(id.indexOf("_")>-1)
			{
				id=id.substring(id.lastIndexOf("_")+1);
			}
			var vl=$().pagecookie(id);
			if(vl!=null)$(this).text(vl);
		}
	);
}