<%@ Control Language="C#" AutoEventWireup="true" Codebehind="QrBuilder.ascx.cs" Inherits="Song.Site.Manage.Utility.QrBuilder" %>

<script type="text/javascript" src="/manage/CoreScripts/iColorPicker.js"></script>

<table width="100" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td valign="top">
            二维码内容：<asp:RequiredFieldValidator ID="rfv1" runat="server" ErrorMessage="不得为空"
                ValidationGroup="qrcode" ControlToValidate="tbContent" SetFocusOnError="True"></asp:RequiredFieldValidator><br />
            <asp:TextBox ID="tbContent" runat="server" TextMode="MultiLine" Width="200px" Height="150px" ></asp:TextBox><br />           </td>
        <td valign="top">
            <div style="margin-left:10px">二维码：<br />
            <asp:Image ID="imgQr" Height="150px" runat="server" /></div></td>
    </tr>
    <tr>
        <td colspan="2">
             前景：<asp:TextBox ID="tbColor" runat="server" Width="60px" CssClass="iColorPicker"
                SkinID="不存在皮肤主题"></asp:TextBox> &nbsp;
            背景：<asp:TextBox ID="tbBgcolor" runat="server" Width="60px"
                    CssClass="iColorPicker" SkinID="不存在皮肤主题"></asp:TextBox><br />
            宽高：<asp:TextBox ID="tbWh" runat="server" Width="60px" Text="150"></asp:TextBox>像素  &nbsp;
            <asp:CheckBox ID="cbIsLogo" runat="server" Text="是否采用企业Logo" Checked="True" />            </td>
    </tr>
</table>
<asp:Button ID="btnBuilerQr" runat="server" Text="生成二维码" ValidationGroup="qrcode" OnClick="btnBuilerQr_Click" /> <span id="qrcodeEnterShow" style="display:none">正在生成...</span>
<script type="text/javascript">
$(function(){
	$("input[id$='btnBuilerQr']").click(function(){
		//$(this).attr({"disabled":"disabled"});
		var context=$("textarea[id$='tbContent']").val();
		if($.trim(context)!="")
		{
		    $("#qrcodeEnterShow").show();
		}
	});
})
</script>

