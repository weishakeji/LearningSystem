<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Correct.aspx.cs" Inherits="Song.Site.Manage.Teacher.Correct"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <fieldset>
        <legend>
            <asp:Label ID="lbStudent" runat="server" Text=""></asp:Label>-《<asp:Label ID="lbExamTheme"
                runat="server" Text=""></asp:Label>》</legend>
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td class="right" width="100px">
                    场次：</td>
                <td>
                    <asp:Label ID="lbExamName" runat="server" Text=""></asp:Label>
                </td>
              
                <td width="100px" class="right">
                    应试时间：</td>
                <td>
                    <asp:Label ID="lbExamTime" runat="server" Text=""></asp:Label></td>
               
            </tr>
            <tr>
                
                <td class="right">
                    最终得分：</td>
                <td>
                    <asp:Label ID="lbScoreFinal" runat="server" ForeColor="Red"></asp:Label>
                    分
                </td>
                <td class="right">切换考生：</td>
                 <td>
                     <asp:DropDownList ID="ddlStudent" runat="server" AutoPostBack="True" 
                         onselectedindexchanged="ddlStudent_SelectedIndexChanged">
                     </asp:DropDownList> （背景为蓝色的考生，已经判过卷）
                 </td>
            </tr>
        </table>
    </fieldset>
    <asp:Panel ID="plJianda" runat="server">
        <fieldset>
            <legend>人工判卷：简答题</legend>
            <asp:Repeater ID="rptQues" runat="server" 
                onitemdatabound="rptQues_ItemDataBound">
                <ItemTemplate>
                    <div class="shortQues">
                        <div class="quesTitle">
                            <span style="display: none">
                                <asp:Label ID="lbID" runat="server" Text='<%# Eval("qid")%>'></asp:Label></span>
                            <%# Container.ItemIndex + 1%>
                            、<asp:Label ID="lbQuesTitle" runat="server" Text='<%# Eval("qtitle")%>'></asp:Label>[
                            <%# Eval("num")%><asp:HiddenField ID="hfnum" runat="server" Value='<%# Eval("num")%>' />
                            分]
                        </div>
                        <div class="quText">
                            <div class="quitem">考生答题内容：</div>
                            <asp:Label ID="lbReply" runat="server" Text='<%# Eval("reply")=="" ? "(未答题)" : Eval("reply")%>'></asp:Label>
                            <div class="accfile">答题附件：<asp:HyperLink ID="linkAcc" runat="server" Target=_blank>无</asp:HyperLink></div>
                        </div>
                        <div class="quText">
                            <div class="quitem">试题参考答案：</div>
                            <asp:Label ID="lbAnswer" runat="server" Text='<%# Eval("answer")%>'></asp:Label>
                        </div>
                        <div class="quesScore">
                            该题得分：<asp:TextBox ID="tbNumber" datatype="number" Width="80px" Text='<%# Eval("score")%>'
                                runat="server"></asp:TextBox>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </fieldset>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
