<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Students_Details.aspx.cs" Inherits="Song.Site.Manage.Student.Students_Details"
    Title="学员信息详情" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <asp:DropDownList ID="ddlEducation" runat="server">
        <asp:ListItem Value="81">小学</asp:ListItem>
        <asp:ListItem Value="71">初中</asp:ListItem>
        <asp:ListItem Value="61">高中</asp:ListItem>
        <asp:ListItem Value="41">中等职业教育</asp:ListItem>
        <asp:ListItem Value="31">大学（专科）</asp:ListItem>
        <asp:ListItem Value="21">大学（本科）</asp:ListItem>
        <asp:ListItem Value="14">硕士</asp:ListItem>
        <asp:ListItem Value="11">博士</asp:ListItem>
        <asp:ListItem Value="90">其它</asp:ListItem>
    </asp:DropDownList>
    <asp:Repeater ID="rptAccounts" runat="server" OnItemDataBound="rptAccounts_ItemDataBound">
        <ItemTemplate>
            <div class="page" style="page-break-after: always"><asp:Image ID="imgStamp" runat="server" />
            <table width="100%" class="first" border="1" cellspacing="0" cellpadding="0">
                <tr>
                    <td class="right" width="120px">
                        姓名：
                    </td>
                    <td class="left" width="100px">
                        <%# Eval("Ac_name") %>
                    </td>
                    <td class="right" width="100px">
                        性别：
                    </td>
                    <td class="left">
                        <%# Eval("Ac_sex","{0}")=="0" ? "女" : "男" %>
                    </td>
                    <td rowspan="5" valign="middle" class="photo">
                        <img src='<%# Eval("Ac_photo", "{0}") =="" ? "/manage/images/nophoto.gif" : Eval("Ac_photo", "{0}")%>' width="150px" height="200px" />
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        年龄：
                    </td>
                    <td class="left">
                        <%# Convert.ToInt16(Eval("Ac_Age")) > 200 ? "未知" : Eval("Ac_Age")%>
                    </td>
                    <td class="right">
                        出生年月：
                    </td>
                    <td class="left">
                        <%# DateTime.Parse(Eval("Ac_Birthday", "{0}")).AddYears(200) < DateTime.Now ? "" : Eval("Ac_Birthday", "{0:yyyy年M月}")%>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        籍贯：
                    </td>
                    <td class="left">
                        <%# Eval("Ac_Native")%>
                    </td>
                    <td class="right">
                         学号：
                    </td>
                    <td class="left">
                        <%# Eval("Ac_CodeNumber")%>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                      民 族： 
                    </td>
                    <td class="left"> <%# Eval("Ac_Nation")%>
                       
                    </td>
                    <td class="right">
                        身份证：
                    </td>
                    <td class="left">
                       <span class="txtrow"> <%# Eval("Ac_IDCardNumber")%></span>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        学历：
                    </td>
                    <td class="left">
                        <%# getEdu(Eval("Ac_Education"))%>
                    </td>
                    <td class="right">
                        学号：
                    </td>
                    <td class="left">
                        <span class="txtrow"> <%# Eval("Ac_accname")%></span>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        毕业院校：
                    </td>
                    <td class="left" colspan="4">
                        <%# Eval("Ac_School")%>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        专业：
                    </td>
                    <td class="left" colspan="4">
                         <%# Eval("Ac_Major")%>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        通讯地址：
                    </td>
                    <td class="left" colspan="4">
                        <%# Eval("Ac_AddrContact")%>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        邮编：
                    </td>
                    <td class="left">
                        <%# Eval("Ac_Zip")%>
                    </td>
                    <td class="right">
                        电话：
                    </td>
                    <td class="left" colspan="2">
                        <%# Eval("Ac_MobiTel1")%>
                        &nbsp;
                        <%# Eval("Ac_MobiTel2", "{0}") == Eval("Ac_MobiTel1", "{0}") ? "" : Eval("Ac_MobiTel2", "{0}")%>
                        &nbsp;
                        <%# Eval("Ac_Tel")%>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        网路通讯：
                    </td>
                    <td class="left" colspan="4">
                      <span class="txtrow">  <%# Eval("Ac_Email", "{0}") != "" ? "Email：" + Eval("Ac_Email", "{0}") : ""%>
              &nbsp;
                        <%# Eval("Ac_Weixin", "{0}") != "" ? "微信：" + Eval("Ac_Weixin", "{0}") : ""%>
                        &nbsp;
                        <%# Eval("Ac_Qq", "{0}") != "" ? "QQ：" + Eval("Ac_Qq", "{0}") : ""%>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        紧急联系人：
                    </td>
                    <td class="left" colspan="4">
                        <%# Eval("Ac_LinkMan")%>
                         &nbsp;
                         <%# Eval("Ac_LinkManPhone", "{0}") != "" ? "联系电话：" + Eval("Ac_LinkManPhone", "{0}") : ""%>              
                    
                    </td>
                </tr>
                <tr>
                    <td class="center" colspan="5">
                        学习情况
                    </td>
                </tr>
                <tr>
                    <td colspan="5" class="info-area">
                        <dl class="rtpLearnInfo">
                            <dt>
                                <div class="cou">
                                    课程</div>
                                <div class="date">
                                    学习时间</div>
                                <div class="complete">
                                    完成度</div>
                            </dt>
                            <asp:Repeater ID="rtpLearnInfo" runat="server">
                                <ItemTemplate>
                                    <dd>
                                        <div class="cou">
                                            <%# Container.ItemIndex + 1%>.《
                                            <%# Eval("Cou_Name")%>
                                            》
                                        </div>
                                        <div class="date">
                                            <%# Eval("LastTime", "{0:yyyy-MM-dd}")%>
                                        </div>
                                        <div class="complete">
                                            <%# Convert.ToDouble(Eval("complete", "{0}") == "" ? "0" : Eval("complete", "{0}")) >= 95 ? "100%" : Eval("complete", "{0:0.00}%")%></div>
                                    </dd>
                                </ItemTemplate>
                            </asp:Repeater>
                        </dl>
                        <%--机构信息--%>
                        <div class="info-foot">
                        <div class="plate-name">
                        <%= org.Org_Name%>
                        </div>
                        <div class="output-date">
                        <%= System.DateTime.Now.ToString("yyyy年M月d日") %>
                        </div></div>
                    </td>
                </tr>
            </table>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
</asp:Content>
