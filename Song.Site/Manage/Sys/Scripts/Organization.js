$(function(){
	 _init();
});

function _init(){
    $(".adminState").each(function(){
        var txt=$(this).text();
        var orgid=$(this).attr("orgid");
        var orgname=$(this).attr("orgname");
        if(txt=="【设置管理员】")        {
           $(this).attr("title","当前分厂已经存在员工，但没有管理员");
        }else {

        }

    });
    $(".adminState").click(function () {
        var txt = $(this).text();
        var orgid = encodeURIComponent($(this).parents("tr").attr("EncryptKey"));
        var orgname = $(this).attr("orgname");
        if (txt == "【设置管理员】") {
            OpenWin("Organization_Admin.aspx?id=" + orgid, "设置“" + orgname + "”管理员", 600, 450);
        } else {
            OpenWin("Admin_Edit.aspx?id=" + orgid, "设置“" + orgname + "”管理员", 600, 450);
        }
        return false;
    });
}
