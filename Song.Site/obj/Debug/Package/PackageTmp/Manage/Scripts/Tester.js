//获取QueryString参数
jQuery.fn.getPara=function(para)
{
	var LocString=String(window.document.location.href);
	var rs=new RegExp("(^|)"+para+"=([^\&]*)(\&|$)","gi").exec(LocString),tmp;
	if(tmp=rs)return tmp[2];
	return "-1";
}
$(setTreeNodeState);
//设参数中id与节点id一致时，当前节点特殊显示
function setTreeNodeState()
{
	var id=$().getPara("id");
	if(id=="-1")return;
	var panel=$("div[type='MenuTree']");
	var node=panel.find("div[type='text'][nodeId='"+id+"']");
	node.attr("class","currentNode");
}