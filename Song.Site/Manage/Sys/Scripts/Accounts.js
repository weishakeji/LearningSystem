$(function () {


});
//导出事件
function OnOutput() {
    var LocString = String(window.document.location.href);
    if (LocString.indexOf("?") > -1) {
        var para = LocString.substring(LocString.lastIndexOf("?"));
        OpenWin('Accounts_Export.aspx' + para, '学员导出', 800, 600);
    } else {
        OpenWin('Accounts_Export.aspx', '学员导出', 800, 600);
    }
    return false;
}