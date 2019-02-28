$(function () {
    _ExamEditInit();
    setSelGroupEvent();
    btnEvent();
    subjectEvent();
	//提交判断
    $("form").submit(function () {
		//获取各场次的标题
        var items = new Array();
        $("table[name=examItems] tr").each(function (index, element) {
            var name = $(this).find("input[name$=tbName]").val();
            if (typeof (name) != "undefined" && name != "") {
                items.push({ index: index, name: name });
            }
        });
		//判断各场次标题是否重复
		var isExist=false;
        for (var i = 0; i < items.length; i++) {
            for (var j = 0; j < items.length; j++) {
                if (items[i].index == items[j].index) continue;
				if(items[i].name == items[j].name){
					isExist=true;
					var row=$("table[name=examItems] tr:eq("+items[j].index+")");
					alert("各场次考试标题不可以相同");
					row.find("input[name$=tbName]").focus();
					break;
				}
            }
			if(isExist)break;
        }
		if(isExist)return false;
    });
});
//页面初始化
function _ExamEditInit() {
    var type = Number($("input[name$=rblGroup]:checked").val());
    if (type == 2) $("#selStudentSort").show();
    SelGroupChange(type);
    //场次中的试卷下拉
    $("select[name$=ddlTestPager]").change(function () {
        var tr = $(this).parent().parent();
        var sel = tr.find("input[name$=tbTestPager]");
        //试卷id
        tpid = $(this).val();
        sel.val(tpid);
        //考试场次的标题
        var tpname = $(this).find("option[value=" + tpid + "]").text();
        setExamItem(tr, tpname, $(this).find("option:selected").attr("total"),
            $(this).find("option:selected").attr("passScore"),
            $(this).find("option:selected").attr("span"));
    });
}
//设置参考人员范围的事件
function setSelGroupEvent() {
    //当更改参加考试的人员分类时
    $("input[name$=rblGroup]").click(function () {
        var gtype = Number($(this).val());
        //清除选中的数据
        $("select[name$=lbSelected]").find("option").remove();
        $("input[name$=tbSelected]").val("");
        SelGroupChange(gtype);
    });
}
//当参考人员类型变更时
function SelGroupChange(type) {
    //班组控件
    var sots = $("select[name$=lbSort]");
    sots.unbind("click");
    sots.unbind("dblclick");
    $("#btnAddSort").unbind("click");
    switch (type) {
        case 1:
            $("#selStudentSort").hide();
            break;
        case 2:
            $("#selStudentSort").show();
            sots.bind("dblclick", function () {
                var id = $(this).find("option:selected").val();
                var name = $(this).find("option:selected").text();
                name = name.substring(name.indexOf(" "));
                addSelect(id, name);
            });
            $("#btnAddSort").click(function () {
                var id = sots.find("option:selected").val();
                var name = sots.find("option:selected").text();
                name = name.substring(name.indexOf(" "));
                addSelect(id, name);
            });
            break;
    }
}
//添加选中项
function addSelect(id, name) {
    var selist = $("select[name$=lbSelected]");
    if (selist.find("option[value=" + id + "]").size() < 1) {
        selist.append("<option value='" + id + "'>" + name + "</option>");
        var setb = $("input[name$=tbSortSelected]");
        setb.val(setb.val() + "," + id + "|" + name);
    }
}
//移除选中项
function removeSelect() {
    var selist = $("select[name$=lbSelected]");
    selist.find("option:selected").remove();
    var txt = ""
    selist.find("option").each(function () {
        var id = $(this).attr("value");
        var name = $(this).text();
        txt += "," + id + "|" + name;
    });
    $("input[name$=tbSortSelected]").val(txt);
}
//添加与去除内容的按钮事件
function btnEvent() {
    //清除单项
    $("#btnAddSort").click(function () {
        $("select[name$=lbSelected]").find("option:selected").remove();
    });
    $("#btnSortRemove").click(function () {
        $("select[name$=lbSelected]").find("option:selected").remove();
        removeSelect();
    });
    //清除所有选中的学生分类
    $("#btnSortRemoveAll").click(function () {
        $("select[name$=lbSelected]").find("option").remove();
        $("input[name$=tbSortSelected]").val("");
    });
}




/*
* 以下是考试场次的管理
* */
//当学科选项变更时
function subjectEvent() {
    //学科控件
    var ddl = $("select[name$=ddlSubject]");
    ddl.change(function () {
        //当前选中的课程id
        var sjbid = $(this).val();
		
        //试卷的下拉控件
        var tr = $(this).parent().parent();
        var testpager = tr.find("select[name$=ddlTestPager]");
        var sel = tr.find("input[name$=tbTestPager]");
        //动态获取
        if (Number(sjbid) < 1) {
            testpager.find("option").remove();
            sel.val("");
            setExamItem(tr, null, null, null, null);
            var myDate = new Date()
            tr.find("input[name$=tbDate]").val("");
        } else {
            var urlPath = "/json/TestPaper.aspx?id=" + sjbid + "&timestamp=" + new Date().getTime();
            $.get(urlPath, function (data) {
                var arr = eval(data);
                testpager.find("option").remove();
                for (var i = 0; i < arr.length; i++)
                    testpager.append("<option value='" + arr[i].Tp_Id
                    + "' span='" + arr[i].Tp_Span + "' total='" + arr[i].Tp_Total
                    + "' passScore='" + arr[i].Tp_PassScore + "'>" + unescape(arr[i].Tp_Name) + "</option>");
                if (arr.length > 0) {
                    sel.val(arr[0].Tp_Id);
                    setExamItem(tr, unescape(arr[0].Tp_Name), arr[0].Tp_Total, arr[0].Tp_PassScore, arr[0].Tp_Span);
                    var myDate = new Date()
                    tr.find("input[name$=tbDate]").val(myDate.Format("yyyy-MM-dd hh:mm"));
                } else {
                    sel.val("");
                    setExamItem(tr, null, null, null, null);
                    var myDate = new Date()
                    tr.find("input[name$=tbDate]").val("");
                }
            });
        }
    });
}
//设置考试场次的
//参数说明：行的对象，场次名称，总分，及格分，考试时长
function setExamItem(tr, name, total, passcore, span) {
    //考试场次的标题
    if (name != null && name != undefined && name != "") {
        tr.find("input[name$=tbName]").val(name);
    } else {
        tr.find("input[name$=tbName]").val("");
    }
    //总分
    if (total != null && total != undefined && total != "") {
        tr.find("span.lbTotal").html(total);
        tr.find("input[name$=tbTotal]").val(total);
    } else {
        tr.find("span.lbTotal").html("");
        tr.find("input[name$=tbTotal]").val("");
    }
    //及格分
    if (passcore != null && passcore != undefined && passcore != "") {
        tr.find("span.lbPassScore").html(passcore);
        tr.find("input[name$=tbPassScore]").val(passcore);
    } else {
        tr.find("span.lbPassScore").html("");
        tr.find("input[name$=tbPassScore]").val("");
    }
    //试卷的考试用时
    if (span != null && span != undefined && span != "") {
        tr.find("input[name$=tbSpan]").val(span);
    } else {
        tr.find("input[name$=tbSpan]").val("");
    }
}

