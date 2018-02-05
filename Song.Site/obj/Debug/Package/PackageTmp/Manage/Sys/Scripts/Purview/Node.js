//菜单的节点
function Node(object,fulldata)
{	
	this.object=object;	
	this.fulldata=fulldata;
	if(object!=null)
	{			
		this.evaluate1();
		this.evaluate2();
	}else
	{		
		this.Name="院系管理";
		this.Id=0;
		this.evaluate2();
	}
}
//取菜单项的名称，如果为空，则为null
Node.prototype.Name="null";
//节点的id
Node.prototype.Id=0;
//菜单的类型，item为菜单项，link为超链接，line为分隔符
Node.prototype.Type="item";
//导航地址
Node.prototype.Link="#";
//颜色
Node.prototype.Color="#000000";
//字体
Node.prototype.Font ="宋体";
//是否粗体
Node.prototype.IsBold=false;
//是否斜体
Node.prototype.IsItalic=false;
//节点的状态，如果为true则，在页面显示时为展开状态
Node.prototype.State=true;
//菜单项的父节点id
Node.prototype.PatId=0;
//菜单树的id
Node.prototype.Root=0;
//排序号
Node.prototype.Tax=0;
//菜单项是否启用，默认为true
Node.prototype.IsUse=true;
//该菜单是否显示，默认为true
Node.prototype.IsShow=true;
//该菜单是为根节点
Node.prototype.IsRoot=true;
//小图标
Node.prototype.IcoS="";
//大图标
Node.prototype.IcoB="";
//菜单项的介绍或说明
Node.prototype.Intro="";
//
//该节点的子节点
Node.prototype.Childs=null;
//该节点的兄弟节点
Node.prototype.Siblings=null;
//该节点，是否有子级
Node.prototype.IsChilds=false;
//该节点，是否是第一个节点
Node.prototype.IsFirst=false;
//当前节点的上一个
Node.prototype.Pre=null;
//该节点，是否是最后一个节点
Node.prototype.IsLast=false;
//当前节点的下一个；
Node.prototype.Next=null;
//当前节点的上一级
Node.prototype.Parent=null;
//节点相对于根节点的路，路径名为中文，分隔符为逗号
Node.prototype.Path="";
//给各个属性赋值
Node.prototype.evaluate1=function()
{
	//属性参数
	this.Name=this.getValue("MM_Name");
	this.Id=Number(this.getValue("MM_Id"));
	this.Type=this.getValue("MM_Type");
	this.Link=this.getValue("MM_Link");
	this.Color=this.getValue("MM_Color");
	this.Font=this.getValue("MM_Font ");
	this.IsBold=this.getValue("MM_IsBold")=="false" ? false : true;
	this.IsItalic=this.getValue("MM_IsItalic")=="false" ? false : true;		
	this.PatId=Number(this.getValue("MM_PatId"));
	this.IsRoot=this.PatId==0 ? true : false;
	this.State=this.getValue("MM_State")=="false" ? false : true;	
	this.Root=Number(this.getValue("MM_Root"));
	this.Tax=Number(this.getValue("MM_Tax"));
	this.IsUse=this.getValue("MM_IsUse")=="false" ? false : true;;
	this.IsShow=this.getValue("MM_IsShow")=="false" ? false : true;
	this.IcoS=this.getValue("IcoS");
	this.IcoB=this.getValue("IcoB");
	this.Intro=this.getValue("MM_Intro");
};
Node.prototype.evaluate2=function()
{
	//特性参数
	this.State=this.PatId==0 ? true : this.State;
	this.IsBold=this.PatId==0 ? true : this.IsBold;
	this.Childs=this.GetChilds();
	this.Siblings=this.GetSiblings();
	this.Parent=this.GetParent();
	this.IsChilds=this.Childs.length>0 ? true : false;	
	this.IsFirst=this.SetIsFirst();	
	this.IsLast=this.SetIsLast();
	this.Pre=this.GetPre();
	this.Next=this.GetNext();
}
//获取节点的相关属性
//keyName:属性名
Node.prototype.getValue=function(keyName)
{
	var node=this.object.find(">"+keyName);
	if(node==null||node.length<1)
	{
		return "";
	}
	return node.text().toLowerCase();
}
//获取子级
Node.prototype.GetChilds=function()
{
	var fulldata=this.fulldata;
	var nodes=this.fulldata.find("ManageMenu");	
	var id=this.Id;
	//alert(id);
	var arr=new Array();
	nodes.each(
		function()
		{
			var n=Number($(this).find("MM_PatId").text().toLowerCase());
			if(n==id)
			{
				arr.push($(this));
			}
		}
	);
	arr=this.SetOrder(arr);
	return arr;
}
//获取上级,xml对象，非node对象
Node.prototype.GetParent=function()
{
	var fulldata=this.fulldata;
	var nodes=this.fulldata.find("ManageMenu");	
	var pid=this.PatId;
	//alert(pid);
	var arr=new Array();
	nodes.each(
		function()
		{
			var n=Number($(this).find("MM_Id").text().toLowerCase());
			if(n==pid)
			{			
				arr.push($(this));
			}
		}
	);
	if(arr.length>0)return arr[0];
	return null;
}
//获取兄弟子级
Node.prototype.GetSiblings=function()
{
	var nodes=this.fulldata.find("ManageMenu");	
	var pid=this.PatId;
	//alert(id);
	var arr=new Array();
	nodes.each(
		function()
		{
			var n=Number($(this).find("MM_PatId").text().toLowerCase());
			if(n==pid)
			{
				arr.push($(this));
			}
		}
	);
	arr=this.SetOrder(arr);
	return arr;
}
//将数组类的列排序
Node.prototype.SetOrder=function(array)
{
	for (var i = 0; i <= array.length - 1; i++)
	{
		for (var j = array.length - 1; j > i; j--)
		{
			var jj=Number(array[j].find("MM_Tax").text().toLowerCase());
			var jn=Number(array[j-1].find("MM_Tax").text().toLowerCase());
			if (jj < jn )
			{
				var temp = array[j];
				array[j] = array[j - 1]; 
				array[j - 1] = temp; 
			}
		}
	} 
	for (var i = 0; i < array.length; i++)
	{
		array[i].find("MM_Tax").text((i+1)*10);		
	}
	return array;
}
//是不是第一个节点
Node.prototype.SetIsFirst=function()
{
	var arr=this.Siblings;
	var len=arr.length;
	if(arr.length>0)
	{
		var id=Number(arr[0].find("MM_Id").text().toLowerCase());
		if(id==this.Id)
		{
			return true;
		}
	}
	return false;
}
//是不是最后一个节点
Node.prototype.SetIsLast=function()
{
	var arr=this.Siblings;
	var len=arr.length;
	if(arr.length>0)
	{
		var id=Number(arr[len-1].find("MM_Id").text().toLowerCase());
		if(id==this.Id)
		{
			return true;
		}
	}
	return false;
}
//当前节点的上一个节点
Node.prototype.GetPre=function()
{
	//如果自身是第一个节点，则返回Null
	if(this.IsFirst)return null;
	var arr=this.Siblings;
	var len=arr.length;	
	for(var i=0;i<len;i++)
	{
		var id=Number(arr[i].find("MM_Id").text().toLowerCase());
		if(id==this.Id)
		{
			return arr[i-1];
		}
	}
	return null;
}	
//当前节点的下一个节点
Node.prototype.GetNext=function()
{
	//如果自身是第一个节点，则返回Null
	if(this.IsLast)return null;
	var arr=this.Siblings;
	var len=arr.length;	
	for(var i=0;i<len;i++)
	{
		var id=Number(arr[i].find("MM_Id").text().toLowerCase());
		if(id==this.Id)
		{
			return arr[i+1];
		}
	}
	return null;
}
//获取当前节点从根节点的路径，路径名为中文，分隔符为,号；
Node.prototype.GetPath=function()
{
	var tm="";
	var p=this.GetParent(this.Id);
	if(p==null)return tm;
	var pid=p.find("MM_PatId").text();
	while(Number(pid)!=0)
	{
		tm=p.find("MM_Name").text()+","+tm;
		var pid=p.find("MM_PatId").text();
		pid=Number(pid);
		p=this.GetParent(pid);
	}
	tm=tm.substring(0,tm.lastIndexOf(","));
	return tm;
}