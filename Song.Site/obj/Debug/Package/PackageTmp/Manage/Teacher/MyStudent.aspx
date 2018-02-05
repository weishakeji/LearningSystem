<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="MyStudent.aspx.cs" Inherits="Song.Site.Manage.Teacher.MyStudent" Title="我的学员" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">

    <div id="header">
        
         <div class="searchBox">              
            <cc1:DropDownTree ID="ddlCourse" runat="server" IdKeyName="Cou_ID" ParentIdKeyName="Cou_PID"
                TaxKeyName="Cou_Tax" Width="160">
            </cc1:DropDownTree>
            学员名称：<asp:TextBox ID="tbStName" runat="server" Width="80"></asp:TextBox>
            手机号：<asp:TextBox ID="tbStMobi" runat="server" Width="100"></asp:TextBox>&nbsp;<asp:Button
                ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="false">
        <EmptyDataTemplate>
            没有满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemStyle CssClass="center" Width="40" />
                <ItemTemplate>
                    <%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="学员">
                <ItemStyle CssClass="center"  />
                <ItemTemplate>
                     <%# Eval("Ac_Name")%>/<%# Eval("Ac_AccName")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="学生组">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <%# Eval("sts_name", "{0}")%>
                </ItemTemplate>
            </asp:TemplateField>
          <asp:TemplateField HeaderText="性别">
                <ItemStyle CssClass="center"  />
                <ItemTemplate>
                     <%# Eval("Ac_Sex","{0}")=="1" ? "男" : "女"%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="最后登录">
                <ItemStyle CssClass="center" Width="160" />
                <ItemTemplate>
                    <%# Convert.ToDateTime(Eval("Ac_LastTime"))>DateTime.Now.AddYears(-100) ? Eval("Ac_LastTime") : "" %>
                    
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="详情">
                <ItemStyle CssClass="center"  />
                <ItemTemplate>
                    <a href="#" onclick="OpenWin('MyStudent_Online.aspx?stid=<%# Eval("Ac_ID")%>',' <%# Eval("Ac_Name")%>的在线时间',980,80);return false;">在线时间</a>
                    <a href="#" onclick="OpenWin('StudyLog_Details.aspx?couid=<%=couid %>&stid=<%# Eval("Ac_ID")%>',' <%# Eval("Ac_Name")%>的学习情况',980,80);return false;">学习情况</a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
  
</asp:Content>
