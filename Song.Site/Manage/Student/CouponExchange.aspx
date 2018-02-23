<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CouponExchange.aspx.cs"
    MasterPageFile="~/Manage/PageWin.Master" Inherits="Song.Site.Manage.Student.CouponExchange" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div class="context">
    <div class="topbox">
        <div class="moneyBox">
            <div class="title">
                当前卡券数量：</div>
            <div class="money">
                &spades;<asp:Literal ID="ltCoupon" runat="server"></asp:Literal>个</div>
        </div>
        <div class="moneyBox">
            <div class="title">
                当前可用积分：</div>
            <div class="money point">
                &clubs;<span id="point"><asp:Literal ID="lbPoint" runat="server"></asp:Literal></span>分</div>
        </div></div>
        <!--积分兑换-->
        <fieldset>
            <legend>积分兑换卡券</legend>
            <div class="infoRow">
                积分兑换比率：
                 <span id="pointSum">0</span>&divide;<span id="ratio"><asp:Literal ID="ltPointConvert" runat="server"></asp:Literal></span>=
                 <span id="result">0</span>个
               <span class="alt">（积分除以兑换率，向下取整）</span>
            </div>
            <div class="infoRow">
                输入兑换数量：
               <asp:TextBox ID="tbNumber" runat="server" Width="300px" nullable="false" datatype="uint" place="right"></asp:TextBox>
            </div>
            
            <div class="mrRow">
                <div class="mrTitle">
                    &nbsp;</div>
                <div class="mrInput">
                    <cc1:Button ID="btnCode" runat="server" Text="充值" verify="true" OnClick="btnCode_Click"
                        ValidationGroup="code" />
                    <span class="alert">                        
                        <asp:CustomValidator ID="cv3" runat="server" ControlToValidate="tbNumber" ErrorMessage="不得大于可兑换数量！"
                            ForeColor="Red" ValidationGroup="code"></asp:CustomValidator>
                    </span>
                </div>
        </fieldset>
    </div>
</asp:Content>
