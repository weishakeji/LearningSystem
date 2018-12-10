<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MoneyRecharge.aspx.cs"
    MasterPageFile="~/Manage/Student/Parents.Master" Inherits="Song.Site.Manage.Student.MoneyRecharge" %>

<%@ MasterType VirtualPath="~/Manage/Student/Parents.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div class="moneyBox">
        <div class="title">
            当前资金余额：</div>
        <div class="money">
            ￥<asp:Literal ID="ltMoney" runat="server"></asp:Literal>元</div>
    </div>
    <!--充值码充值-->
    <!--在线充值-->
    <fieldset id="onlinePay" runat="server">
        <legend>在线充值</legend>
        <div class="payInterFaceTit">
            选择支付平台</div>
        <div class="payInterface">
            <asp:Repeater ID="rptPi" runat="server">
                <ItemTemplate>
                    <div class="pi" select="false" pattern="<%#Eval("Pai_Pattern")%>" paiid="<%#Eval("Pai_ID")%>">
                        <div class="ico">
                            <img src="/pay/images/<%#Eval("Pai_Pattern")%>.png" /></div>
                        <div class="piName">
                            <%#Eval("Pai_Name")%></div>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:Label ID="lblEmpty" Text="没有设置支付接口，请与管理员联系！" runat="server" Visible='<%#bool.Parse((rptPi.Items.Count==0).ToString())%>'></asp:Label>
                </FooterTemplate>
            </asp:Repeater>
            <input name="paiid" type="hidden" id="paiid" value="" />
        </div>
        <div class="mrRow">
            <div class="mrTitle">
                充值金额：</div>
            <div class="mrInput">
                <asp:TextBox ID="tbMoney" CssClass="tbMoney" nullable="false" datatype="number" runat="server" Width="180px"></asp:TextBox><span
                    class="alert"><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                        ControlToValidate="tbMoney" Display="Dynamic" ErrorMessage="金额不得为空" ForeColor="Red"
                        ValidationGroup="o"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator1" runat="server" ControlToValidate="tbMoney" Display="Dynamic"
                            ErrorMessage="输入正数" ForeColor="Red" ValidationExpression="^\d+(\.\d+)?$" ValidationGroup="o"></asp:RegularExpressionValidator></span></div>
            <input name="money" type="hidden" id="money" value="" />
        </div>
        <div class="mrRow">
            <div class="mrTitle">
                验证码：
            </div>
            <div class="mrInput">
                <asp:TextBox ID="tbVerCode" CssClass="tbMoney" runat="server"  nullable="false" datatype="uint" lenlimit="4" Width="80px"></asp:TextBox>
                <input name="vpaycode" type="hidden" id="vpaycode" value="" />
            </div>
            <div class="mrVirfy">
                <img src="../Utility/codeimg.aspx?name=vpaycode" class="verifyCode" />
            </div>
            <div class="mrVirfy">
                <span class="alert">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbVerCode"
                        Display="Dynamic" ErrorMessage="验证码不得为空" ForeColor="Red" ValidationGroup="o"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="tbVerCode"
                        Display="Dynamic" ErrorMessage="输入4位数字" ForeColor="Red" ValidationExpression="^\d{4}$"
                        ValidationGroup="o"></asp:RegularExpressionValidator>
                    <asp:CustomValidator ID="cvCode" runat="server" ControlToValidate="tbVerCode" ErrorMessage="验证码不正确！"
                        ForeColor="Red" ValidationGroup="o"></asp:CustomValidator>
                </span>
            </div>
        </div>
        <div class="mrRow">
            <div class="mrTitle">
                &nbsp;</div>
            <div class="mrInput">
                <cc1:Button ID="btnOnlinePay" runat="server" Text="充值" verify="true" ValidationGroup="o"
                    OnClick="btnOnlinePay_Click" />
                <span class="alert"></span>
            </div>
        </div>
    </fieldset>
</asp:Content>
