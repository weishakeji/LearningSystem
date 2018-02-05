<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="Product_Edit.aspx.cs" Inherits="Song.Site.Manage.Content.Product_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">

    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script language="javascript" src="scripts/tab.js" type="text/javascript"></script>

    <div id="tabTitleBox">
        <div class="titItem curr" tab="tab1">
            基本信息</div>
        <div class="titItem" tab="tab2">
            详细信息</div>
        <div class="titItem" tab="tab3">
            产品图片</div>
        <div class="titItem" tab="tab4">
            发布设置</div>
        <div class="titItem" tab="tab5">
            自定义信息</div>
    </div>
    <div id="tabContextBox">
        <div class="contItem" tab="tab1">
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="80px" class="right">
                        <strong>产品名称</strong>：</td>
                    <td>
                        <asp:TextBox nullable="false"  group="ent" ID="tbName" runat="server" MaxLength="255" Width="95%"></asp:TextBox></td>
                </tr>
              
                <tr>
                    <td class="right">
                        栏目分类：</td>
                    <td>
                        <asp:DropDownList ID="ddlColumn" runat="server">
                        </asp:DropDownList> &nbsp;<asp:CheckBox ID="cbIsUse" runat="server" Text="使用" Checked="true" />
                        <asp:CheckBox ID="cbIsNew" runat="server" Text="新产品" />
                        <asp:CheckBox ID="cbIsRec" runat="server" Text="推荐" /></td>
                </tr>
                <tr>
                    <td class="right">
                        规格：</td>
                    <td>
                        <asp:TextBox ID="tbModel" runat="server" MaxLength="100"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="right">
                        编号：</td>
                    <td>
                        <asp:TextBox ID="tbCode" runat="server" MaxLength="100"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="right">
                        上市时间：</td>
                    <td>
                        <asp:TextBox ID="tbSaleTime" runat="server" Width="80" MaxLength="100" onfocus="WdatePicker()"
                            class="Wdate"></asp:TextBox></td>
                </tr>
                  <tr>
                    <td class="right">
                        生产厂家：</td>
                    <td><asp:DropDownList ID="ddlFactory" runat="server" Width="150">
                        </asp:DropDownList>
                        </td>
                </tr>
                  <tr>
                    <td class="right">
                        产地：</td>
                    <td>
                        <asp:DropDownList ID="ddlOrigin" runat="server" Width="150">
                        </asp:DropDownList></td>
                </tr>
                  <tr>
                    <td class="right">
                        材质：</td>
                    <td>
                        <asp:DropDownList ID="ddlMaterial" runat="server" Width="150" state="true">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="right">
                        价格：</td>
                    <td>
                        <asp:TextBox ID="tbPrise" runat="server" datatype="number"  group="ent" MaxLength="100" Width="60px"></asp:TextBox>
                        元
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        重量：</td>
                    <td>
                        <asp:TextBox ID="tbWeight" runat="server" MaxLength="100" Width="60px"></asp:TextBox>
                        千克
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        库存：</td>
                    <td>
                        <asp:TextBox ID="tbStocks" runat="server" MaxLength="100" Width="100px"></asp:TextBox>
                        <asp:DropDownList ID="ddlUnit" runat="server">
                            <asp:ListItem>个</asp:ListItem>
                            <asp:ListItem>台</asp:ListItem>
                            <asp:ListItem>箱</asp:ListItem>
                            <asp:ListItem>部</asp:ListItem>
                            <asp:ListItem>包</asp:ListItem>
                            <asp:ListItem>辆</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="right" valign="top">
                        简要说明：<br>
                        <span id="inputShow"></span>
                    </td>
                    <td>
                        <asp:TextBox ID="tbIntro" runat="server" MaxLength="255" TextMode="MultiLine" lenlimit="250"
                            Width="95%" Height="100px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="right">
                        标签：</td>
                    <td>
                        <asp:TextBox ID="tbLabel" runat="server" MaxLength="255" Width="95%"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="right">
                        编辑人员：</td>
                    <td>
                        <asp:TextBox ID="tbAccName" runat="server" MaxLength="255" Width="80px"></asp:TextBox></td>
                </tr>
            </table>
        </div>
        <div class="contItem" tab="tab2" style="display: none">
            <WebEditor:Editor ID="tbDetails" runat="server" Height="450px" Width="100%"> </WebEditor:Editor>
        </div>
        <!--图片管理-->
        <div class="contItem" tab="tab3" style="display: none">
            <div>
                <cc1:FileUpload ID="fuLoad" runat="server" nullable="false"  fileallow="jpg|bmp|gif|png" group="upfile"
                    Width="400" />
                <asp:Button ID="btnUpFile" runat="server" Text="上传" group="upfile" verify="true"
                    OnClick="btnUpFile_Click" />
            </div>
            <div class="imgList">
                <asp:Repeater ID="rptPict" runat="server">
                    <HeaderTemplate>
                        <dl>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <dd class="imgBox" style="width: 146px">
                            <div class="picShow" style="width: 140px; height: 140px;background-image:url(<%# Eval("Pic_FilePathSmall", "{0}") %>)"
                                imgid="<%# Eval("Pic_Id") %>">
                                <div style="display:<%# Eval("Pic_IsCover","{0}")=="True" ? "block" : "none" %>" class="cover" >封面</div></div>
                            <div class="control">
                                <asp:LinkButton ID="lbCover" runat="server" CommandArgument='<%# Eval("Pic_Id") %>'
                                    OnClick="lbCover_Click">设为封面</asp:LinkButton>
                                <asp:LinkButton ID="lbDel" runat="server" OnClientClick="return confirm('您确定删除吗？')"
                                    CommandArgument='<%# Eval("Pic_Id")%>' OnClick="lbDel_Click">删</asp:LinkButton>
                                <cc1:StateButton ID="sbUse" OnClick="sbShow_Click" runat="server" TrueText="显" FalseText="隐"
                                    State='<%# Eval("Pic_IsShow","{0}")=="True"%>' CommandArgument='<%# Eval("Pic_Id")%>'></cc1:StateButton>
                            </div>
                        </dd>
                    </ItemTemplate>
                    <FooterTemplate>
                        </dl>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
        <div class="contItem" tab="tab4" style="display: none">
            <!--发布设置-->
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="80px" class="right">
                        Keywords：</td>
                    <td>
                        <asp:TextBox ID="tbKeywords" runat="server" MaxLength="255" Width="95%"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="right" valign="top">
                        Description：</td>
                    <td>
                        <asp:TextBox ID="tbDescr" runat="server" MaxLength="255" TextMode="MultiLine" lenlimit="250"
                            Width="95%" Height="100px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="right">
                        上线时间：</td>
                    <td>
                        <asp:TextBox ID="tbPushTime" runat="server" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"
                            MaxLength="255" Width="160"></asp:TextBox></td>
                </tr>
            </table>
        </div>
        <div class="contItem" tab="tab4" style="display: none">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter"  group="ent"/>
    <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
