<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CouponRecharge.aspx.cs"
    MasterPageFile="~/Manage/PageWin.Master" Inherits="Song.Site.Manage.Student.CouponRecharge" %>
<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
<div class="context">
    <div class="moneyBox">
        <div class="title">
            当前卡券数量：</div>
        <div class="money">
            &spades;
<asp:Literal ID="ltCoupon" runat="server"></asp:Literal>个</div>
    </div>
    <!--充值码充值-->
    <div>
        <fieldset>
            <legend>充值码充值</legend>
            <div class="infoRow">
                请输入充值码与密码：
                <asp:TextBox ID="tbCode" runat="server" Width="300px" nullable="false" place="right"></asp:TextBox>
            </div>
            <div class="infoRow">
                <span class="msg">（注：格式为“充值码-密码”，破折号不可缺少） </span>
            </div>
            <div class="mrRow">
                <div class="mrTitle">
                    &nbsp;</div>
                <div class="mrInput">
                    <cc1:Button ID="btnCode" runat="server" Text="充值" verify="true" OnClick="btnCode_Click"
                        ValidationGroup="code" />
                    <span class="alert">
                        <asp:RequiredFieldValidator ID="rv1" runat="server" ControlToValidate="tbCode" Display="Dynamic"
                            ErrorMessage="*不得为空" ForeColor="Red" ValidationGroup="code"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="rv2" runat="server" ControlToValidate="tbCode"
                            Display="Dynamic" ErrorMessage="*格式不对" ForeColor="Red" ValidationExpression="^\w+([\s-]+)\w+$"
                            ValidationGroup="code"></asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="cv3" runat="server" ControlToValidate="tbCode" ErrorMessage="充值码不存在或已经过期！"
                            ForeColor="Red" ValidationGroup="code"></asp:CustomValidator>
                    </span>
                </div>
        </fieldset>
       
    </div>
    </div>
</asp:Content>
