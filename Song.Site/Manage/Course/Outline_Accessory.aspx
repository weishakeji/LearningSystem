<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Outline_Accessory.aspx.cs" Inherits="Song.Site.Manage.Course.Outline_Accessory"
    Title="章节附件" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
     <fieldset>
            <legend>附件</legend>
            <cc1:FileUpload ID="fuLoad" runat="server" FileAllow="zip|rar|pdf|doc|docx|xls|xlsx|ppt|pptx"
                group="up" nullable="false" Width="500px" />
            <asp:Button verify="true" group="up" ID="btn" runat="server" Text="上传" OnClick="btn_Click" />
            <br />
            <asp:DataList ID="dlAcc" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                <ItemTemplate>
                    <div class="acc-row"><%# Container.ItemIndex   + 1 %>
                    、
                    <asp:HyperLink ID="hl" runat="server" NavigateUrl='<%#Eval("As_FileName") %>' Target="_blank"><%#Eval("As_Name") %></asp:HyperLink>
                    &nbsp; <span class="sizeSpan">(<span class="size"><%#Eval("As_Size") %></span>)</span>&nbsp;
                    &nbsp;<asp:LinkButton ID="lb" runat="server" CommandArgument='<%#Eval("As_Id") %>'
                        ForeColor="Red" OnClick="lb_Click">删</asp:LinkButton>
                    &nbsp;
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </fieldset>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
