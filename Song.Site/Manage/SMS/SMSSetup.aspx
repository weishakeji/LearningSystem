<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="SMSSetup.aspx.cs" Inherits="Song.Site.Manage.SMS.SMSSetup" Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="SMSSetup_Edit.aspx" WinWidth="600"
            WinHeight="400" GvName="GridView1" DelButtonVisible="false" AddButtonVisible="false" />
            &nbsp;&nbsp;说明：注册成功后，需手工在账号列进行设置账号与密码。
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowFooter="False"
        SelectBoxKeyName="SelectBox" ShowSelectBox="True">
        <EmptyDataTemplate>
            没有满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemStyle CssClass="center" Width="40" />
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <cc1:RowEdit ID="btnEdit" runat="server"></cc1:RowEdit>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="25px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="短信平台">
                <ItemStyle CssClass="center" Width="180" />
                <ItemTemplate>
                    <%# Eval("Name")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="帐号">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                   <a href="SMSSetup_Edit.aspx?id=" title='<%# Eval("Remarks")%>' class="smsedit"> 
                   <%# Eval("User", "{0}") == "" ? "未设置，点击设置" : Eval("User", "{0}")%></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="余额（条）">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <span class="smsCount" remarks='<%# Eval("Remarks", "{0}")%>'>0</span>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="选择短信平台">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Visible='<%# Eval("IsCurrent")%>' ForeColor="Red"
                        Text="当前采用"></asp:Label>
                    <asp:LinkButton ID="linkBtn" CommandArgument='<%# Eval("Remarks")%>' OnClick="linkBtn_Click"
                        runat="server"><%# Eval("IsCurrent","{0}")=="True" ? "" : "设置为当前"%></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="短信模板">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                <%# Eval("User")%>
                    <a href="SMSSetup_Msg.aspx?remarks=<%# Eval("Remarks", "{0}")%>" type="open" width="600" height="400" title="短信模板-<%# Eval("Remarks")%>">编辑</a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="注册/充值">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <a href="<%# Eval("PayUrl")%>?username=<%# Eval("User", "{0}")%>" style='display: <%# Eval("User", "{0}") != "" ? "" : "none"%>'    type="open" width="80" height="80" title="充值-<%# Eval("Remarks")%>" target="_blank">充值</a>
                    <a href="<%# Eval("RegisterUrl")%>" type="open" width="600" height="80" title="注册-<%# Eval("Remarks")%>短信平台" target="_blank">注册</a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
  
</asp:Content>
