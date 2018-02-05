
/*!
* 主  题：《院系的节点》
* 说  明：生成院系的节点对象；
* 功能描述：
* 1、院系的节点对象，包括院系所有信息，与数据库信息一一对应；
* 2、节点属性包括上级、下级、有否子节点，是否为最后一个节点等；
* 3、如果他对像时为空，则默认生成“院系管理”的根节点
*
* 作  者：宋雷鸣 
* 开发时间: 2012年12月23日
*/
//院系的节点
function Node(object, fulldata) {
    this.object = object;
    this.fulldata = fulldata;
    if (object != null) {
        this.evaluate1();
        this.evaluate2();
    } else {
        this.Name = "全部";
        this.Id = 0;
        this.evaluate2();
    }
}
//取院系的名称，如果为空，则为null
Node.prototype.Name = "null";
//节点的id
Node.prototype.Id = 0;
//院系的父节点id
Node.prototype.PatId = 0;
//排序号
Node.prototype.Tax = 0;
//院系是否启用，默认为true
Node.prototype.IsUse = true;
//该院系是否显示，默认为true
Node.prototype.IsShow = true;
//院系的介绍或说明
Node.prototype.Intro = ""; //
//节点的状态，如果为true则，在页面显示时为展开状态
Node.prototype.State = true;
//节点的业务属性
//院系代码
Node.prototype.Code = "";
//中文简称
Node.prototype.CnAbbr = "";
//英文名称
Node.prototype.EnName = "";
//英文简称
Node.prototype.EnAbbr = "";
//联系方式
Node.prototype.Phone = "";
Node.prototype.Fax = "";
Node.prototype.Email = "";
Node.prototype.Msn = "";
Node.prototype.WorkAddr = "";

//以下为系统属性
//该节点的子节点
Node.prototype.Childs = null;
//该节点的兄弟节点
Node.prototype.Siblings = null;
//该节点，是否有子级
Node.prototype.IsChilds = false;
//该节点，是否是第一个节点
Node.prototype.IsFirst = false;
//当前节点的上一个
Node.prototype.Pre = null;
//该节点，是否是最后一个节点
Node.prototype.IsLast = false;
//当前节点的下一个；
Node.prototype.Next = null;
//当前节点的上一级
Node.prototype.Parent = null;
//节点相对于根节点的路，路径名为中文，分隔符为逗号
Node.prototype.Path = "";
//给基本属性赋值
Node.prototype.evaluate1 = function () {
    //属性参数
    this.Name = this.getValue("Dep_CnName");
    this.Id = Number(this.getValue("Dep_Id"));
    this.PatId = Number(this.getValue("Dep_PatId"));
    this.Tax = Number(this.getValue("Dep_Tax"));
    this.IsUse = this.getValue("Dep_IsUse") == "false" ? false : true; ;
    this.IsShow = this.getValue("Dep_IsShow") == "false" ? false : true;
    this.Intro = this.getValue("Dep_Func");
    //this.Path=this.GetPath();
    this.State = this.getValue("Dep_State") == "false" ? false : true;
    this.Code = this.getValue("Dep_Code");
    this.CnAbbr = this.getValue("Dep_CnAbbr");
    this.EnName = this.getValue("Dep_EnName");
    this.EnAbbr = this.getValue("Dep_EnAbbr");
    this.Phone = this.getValue("Dep_Phone");
    this.Fax = this.getValue("Dep_Fax");
    this.Email = this.getValue("Dep_Email");
    this.Msn = this.getValue("Dep_Msn");
    this.WorkAddr = this.getValue("Dep_WorkAddr");
};
//特性参数
Node.prototype.evaluate2 = function () {
    //特性参数
    this.State = this.Id == 0 ? true : this.State;
    this.Childs = this.GetChilds();
    this.Siblings = this.GetSiblings();
    this.Parent = this.GetParent();
    this.IsChilds = this.Childs.length > 0 ? true : false;
    this.IsFirst = this.SetIsFirst();
    this.IsLast = this.SetIsLast();
    this.Pre = this.GetPre();
    this.Next = this.GetNext();
}
//获取节点的相关属性
//keyName:属性名
Node.prototype.getValue = function (keyName) {
    var node = this.object.find(">" + keyName);
    if (node == null || node.length < 1) {
        return "";
    }
    return node.text().toLowerCase();
}
//获取子级
Node.prototype.GetChilds = function () {
    var fulldata = this.fulldata;
    var nodes = this.fulldata.find("Depart");
    var id = this.Id;
    //alert(id);
    var arr = new Array();
    nodes.each(
		function () {
		    var n = Number($(this).find("Dep_PatId").text().toLowerCase());
		    if (n == id) {
		        arr.push($(this));
		    }
		}
	);
    arr = this.SetOrder(arr);
    return arr;
}
//获取上级,xml对象，非node对象
Node.prototype.GetParent = function () {
    var fulldata = this.fulldata;
    var nodes = this.fulldata.find("Depart");
    var pid = this.PatId;
    //alert(pid);
    var arr = new Array();
    nodes.each(
		function () {
		    var n = Number($(this).find("Dep_Id").text().toLowerCase());
		    if (n == pid) {
		        arr.push($(this));
		    }
		}
	);
    if (arr.length > 0) return arr[0];
    return null;
}
//获取兄弟子级
Node.prototype.GetSiblings = function () {
    var nodes = this.fulldata.find("Depart");
    var pid = this.PatId;
    //alert(id);
    var arr = new Array();
    nodes.each(
		function () {
		    var n = Number($(this).find("Dep_PatId").text().toLowerCase());
		    if (n == pid) {
		        arr.push($(this));
		    }
		}
	);
    arr = this.SetOrder(arr);
    return arr;
}
//将数组类的列排序
Node.prototype.SetOrder = function (array) {
    for (var i = 0; i <= array.length - 1; i++) {
        for (var j = array.length - 1; j > i; j--) {
            var jj = Number(array[j].find("Dep_Tax").text().toLowerCase());
            var jn = Number(array[j - 1].find("Dep_Tax").text().toLowerCase());
            if (jj < jn) {
                var temp = array[j];
                array[j] = array[j - 1];
                array[j - 1] = temp;
            }
        }
    }
    for (var i = 0; i < array.length; i++) {
        array[i].find("Dep_Tax").text((i + 1) * 10);
    }
    return array;
}
//是不是第一个节点
Node.prototype.SetIsFirst = function () {
    var arr = this.Siblings;
    var len = arr.length;
    if (arr.length > 0) {
        var id = Number(arr[0].find("Dep_Id").text().toLowerCase());
        if (id == this.Id) {
            return true;
        }
    }
    return false;
}
//是不是最后一个节点
Node.prototype.SetIsLast = function () {
    var arr = this.Siblings;
    var len = arr.length;
    if (arr.length > 0) {
        var id = Number(arr[len - 1].find("Dep_Id").text().toLowerCase());
        if (id == this.Id) {
            return true;
        }
    }
    return false;
}
//当前节点的上一个节点
Node.prototype.GetPre = function () {
    //如果自身是第一个节点，则返回Null
    if (this.IsFirst) return null;
    var arr = this.Siblings;
    var len = arr.length;
    for (var i = 0; i < len; i++) {
        var id = Number(arr[i].find("Dep_Id").text().toLowerCase());
        if (id == this.Id) {
            return arr[i - 1];
        }
    }
    return null;
}
//当前节点的下一个节点
Node.prototype.GetNext = function () {
    //如果自身是第一个节点，则返回Null
    if (this.IsLast) return null;
    var arr = this.Siblings;
    var len = arr.length;
    for (var i = 0; i < len; i++) {
        var id = Number(arr[i].find("Dep_Id").text().toLowerCase());
        if (id == this.Id) {
            return arr[i + 1];
        }
    }
    return null;
}
//获取当前节点从根节点的路径，路径名为中文，分隔符为,号；
Node.prototype.GetPath = function () {
    var tm = "";
    var p = this.GetParent(this.Id);
    if (p == null) return tm;
    var pid = p.find("Dep_PatId").text();
    while (Number(pid) != 0) {
        tm = p.find("Dep_CnName").text() + "," + tm;
        var pid = p.find("Dep_PatId").text();
        pid = Number(pid);
        p = this.GetParent(pid);
    }
    tm = tm.substring(0, tm.lastIndexOf(","));
    return tm;
}