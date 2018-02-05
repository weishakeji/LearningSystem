<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true" CodeBehind="Vote_Edit.aspx.cs" Inherits="Song.Site.Manage.Site.Vote_Edit" Title="无标题页" %>


<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
<p>
        名称：<asp:TextBox nullable="false" ID="tbName" runat="server" MaxLength="100" Width="90%"></asp:TextBox></p>
        <p>类型：<asp:RadioButtonList ID="rbSelType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" >
        <asp:ListItem Selected="True" Value="1">单选</asp:ListItem>
        <asp:ListItem Value="2">多选</asp:ListItem>
    </asp:RadioButtonList>
     </p>
    <p>
        状态：<asp:CheckBox ID="cbIsUse" runat="server" Checked="True" Text="是否启用" /><asp:CheckBox
            ID="cbIsShow" runat="server" Checked="True" Text="是否显示" />
        <asp:CheckBox
            ID="cbIsAllowSee" runat="server" Checked="True" Text="是否允许查看结果" />
        <asp:CheckBox
            ID="cbIsImg" runat="server" Text="图片调查" AutoPostBack="True" OnCheckedChanged="cbIsImg_CheckedChanged" /></p>
        <p><table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td width="310" valign="top" runat="server" id="tdImageArea"> <div id="picShowBox"> <img src="../Images/nophoto.gif" name="imgShow" width="300" height="200" id="imgShow" runat=server /></div>
  选择：
  <cc1:FileUpload ID="fuLoad" runat="server" fileallow="jpg|bmp|gif|png"  Width="260px"/></td>
    <td valign="top">调查内容：
        <br />
      <asp:TextBox ID="tbIntro" runat="server" Height="180px" MaxLength="255" TextMode="MultiLine"
            Width="95%"></asp:TextBox></td>
  </tr>
</table>

      调查选择项：<br />

            <asp:gridview id="GridView1" runat="server" autogeneratecolumns="False" 
        selectboxkeyname="SelectBox" showselectbox="True">
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <itemstyle cssclass="center" width="40px" />
                <itemtemplate>
<%# Container.DataItemIndex   + 1 %>
</itemtemplate>
            </asp:TemplateField>
           
            <asp:TemplateField HeaderText="选项">
                <itemstyle cssclass="center" />
                <itemtemplate>

                    <asp:TextBox ID="tbItemName" runat="server" MaxLength="200" Width="99%" Text='<%# Eval("Vt_Name")%>'></asp:TextBox>
</itemtemplate>
                <FooterTemplate>
                    <asp:LinkButton ID="lbAdd" runat="server" OnClick="lbAdd_Click">+增加行</asp:LinkButton>
                    <asp:LinkButton ID="lbDel" runat="server" OnClick="lbDel_Click">-去除行</asp:LinkButton>
                    
                </FooterTemplate>
            </asp:TemplateField>     
               <asp:TemplateField HeaderText="得票数">
                <itemstyle cssclass="center"  width="200px" />
                <itemtemplate>
 <div class="compBar" tag="<%# Eval("Vt_Id","{0}")%>"> &nbsp; </div>
                            <div class="vtNumber" tag="<%# Eval("Vt_Id","{0}")%>" ><span class="num">
                                <asp:Literal ID="ltItemNumber" runat="server" Text='<%# Eval("Vt_Number", "{0}")%>'></asp:Literal>
                            </span>票 <span class="per"></span></div>
                    
</itemtemplate>
            </asp:TemplateField>    
           
        </Columns>
    </asp:gridview>
    <br />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
<cc1:EnterButton verify=true  ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click" ValidationGroup="enter" />
    <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
