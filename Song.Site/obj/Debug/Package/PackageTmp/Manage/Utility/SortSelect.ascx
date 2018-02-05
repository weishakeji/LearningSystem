<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SortSelect.ascx.cs"
    Inherits="Song.Site.Manage.Utility.SortSelect" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>

        <cc1:DropDownTree ID="ddlSubject" runat="server" IdKeyName="Sbj_ID" ParentIdKeyName="Sbj_PID"
            TaxKeyName="Sbj_Tax" Width="150" AutoPostBack="True" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged">
        </cc1:DropDownTree>
        <cc1:DropDownTree ID="ddlCourse" runat="server" IdKeyName="Cou_ID" ParentIdKeyName="Cou_PID"
            TaxKeyName="Cou_Tax" Width="150" AutoPostBack="True" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
        </cc1:DropDownTree>
        <cc1:DropDownTree ID="ddlOutline" runat="server" IdKeyName="Ol_ID" ParentIdKeyName="Ol_PID"
            TaxKeyName="Ol_Tax" Width="150">
        </cc1:DropDownTree>
   
