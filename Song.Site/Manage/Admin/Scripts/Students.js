$(function () {
    
});
//导入
function OnInput() {
    //$("input[name$=btnInput]").click(function () {
        OpenWin('Student_Input.aspx', '数据导入', 800, 600);
        return false;
    //});
}
//导出事件
function OnOutput() {
    var LocString = String(window.document.location.href);
    if (LocString.indexOf("?") > -1) {
        var para = LocString.substring(LocString.lastIndexOf("?"));
        OpenWin('Student_Export.aspx' + para, '学员导出', 800, 600);
    } else {
        OpenWin('Student_Export.aspx', '学员导出', 800, 600);
    }
    return false;
}