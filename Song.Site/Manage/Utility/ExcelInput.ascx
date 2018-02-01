<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ExcelInput.ascx.cs"
    Inherits="Song.Site.Manage.Utility.ExcelInput" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
.
<fieldset id="fdPanel1" runat="server">
    <legend>第一步：上传Excel数据</legend>
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
        <tr>
            <td width="80" class="right">
                模板：</td>
            <td>
                <asp:HyperLink ID="linkDataTmp" Target="_blank" runat="server" CssClass="linkTmp">点击下载{0}模板</asp:HyperLink>
                <span style="color: Red">（数据格式请按照模板整理）</span></td>
        </tr>
        <tr>
            <td class="right">
                上传数据：</td>
            <td>
                <cc1:FileUpload ID="fuLoad" runat="server" Width="350px" group="upload" FileAllow="xls|xlsx" 
                    nullable="false"></cc1:FileUpload>
                <asp:Button ID="btnUp" OnClick="btnUp_Click" runat="server" Text="上传数据" verify="true"
                    group="upload" /></td>
        </tr>
        <tr>
            <td class="right">
                状态：</td>
            <td>
                <asp:Label ID="lbState" runat="server" Text="等待上传……"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
            </td>
            <td>
                <asp:Label ID="lbError1" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                操作说明：
            </td>
            <td>
                1、需要导入的Excel数据请按照模板格式整理；<br />
                2、Excel文档支持Excel97、2003、2007、2010多种版本；
            </td>
        </tr>
        <tr>
            <td class="right">
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnNext1" runat="server" Text="下一步" Visible="false" OnClick="btnNext1_Click" />
            </td>
        </tr>
    </table>
</fieldset>
<fieldset id="fdPanel2" runat="server" visible="false">
    <legend>第二步：选择要导入的工作簿</legend>
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
        <tr>
            <td class="right">
                操作对象：</td>
            <td>
                正在操作文档 《<asp:Label ID="lbFile2" runat="server" Text=""></asp:Label>》
            </td>
        </tr>
        <tr>
            <td width="80" class="right">
                工作簿：</td>
            <td>
                <asp:DataList ID="dlWorkBook" runat="server" RepeatDirection="Horizontal">
                    <ItemTemplate>
                        <asp:Button ID="btnWorkBook" runat="server" CssClass="workbook" Text='<%# Eval("Name","{0}")+"："+Eval("Count","{0}条数据")%>'
                            CommandArgument='<%# Container.ItemIndex  %>' OnClick="btnSheet_Click"></asp:Button>
                    </ItemTemplate>
                </asp:DataList>
            </td>
        </tr>
        <tr>
            <td class="right">
                状态：</td>
            <td>
                请选择要操作的“工作簿”
            </td>
        </tr>
        <tr>
            <td class="right">
            </td>
            <td>
                <asp:Label ID="lbError2" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                操作说明：
            </td>
            <td>
                1、当前Excel文档包含了<asp:Literal ID="ltSheetCount" runat="server"></asp:Literal>个工作簿；<br />
                2、请选择要导入的工作簿（点击上方工作簿按钮进步入一步操作）；
            </td>
        </tr>
        <tr>
            <td class="right">
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnBack2" runat="server" Text="上一步" OnClick="btnBack2_Click" />
            </td>
        </tr>
    </table>
</fieldset>
<fieldset id="fdPanel3" runat="server" visible="false">
    <legend>第三步：匹配Excel中的列名与数据库字段的对应关系</legend>
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
        <tr>
            <td class="right" width="80px">
                操作对象：</td>
            <td>
                正在操作文档 《<asp:Label ID="lbFile3" runat="server" Text=""></asp:Label>》的工作簿<asp:Label
                    ID="lbSheet3" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <asp:DataList ID="dlColumn" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
        <ItemTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td  width="100px" align="right" class="right">
                        <asp:Label ID="lbColumn" Text='<%# Eval("name") %>' runat="server" Font-Bold="True"></asp:Label></td>
                    <td width="20px" style="text-align:center">&rarr;
</td>
                    <td > <asp:DropDownList ID="ddlColumnForField" runat="server"></asp:DropDownList></td>
                </tr>
            </table>
        </ItemTemplate>
        <ItemStyle Width="300px" />
    </asp:DataList>
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
   
            <tr>
                <td class="right">
                </td>
                <td>
                    <asp:Label ID="lbError3" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="right" width="80">
                    操作说明：
                </td>
                <td>
                    1、左侧是Excel表中的数据字段（excel表首行）；右侧是数据库中的字段；<br />
                    2、系统进行了自动匹配，但不保证完全正确，请手工设置对应关系。
                </td>
            </tr>
        <tr>
            <td class="right">
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnBack3" runat="server" Text="上一步" OnClick="btnBack_Click" />
                &nbsp;
                <asp:Button ID="btnInput" runat="server" Text="导入数据" OnClick="btnInput_Click" ForeColor="Red" />
            </td>
        </tr>
    </table>
</fieldset>
<fieldset id="fdPanel4" runat="server" visible="false">
    <legend>数据导入完成！</legend>
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
        <tr>
            <td class="right" width="80px">
                操作对象：</td>
            <td>
                正在操作文档 《<asp:Label ID="lbFile4" runat="server" Text=""></asp:Label>》的工作簿<asp:Label
                    ID="lbSheet4" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                状态：
            </td>
            <td>
                成功导入
                <asp:Label ID="lbSuccCount" runat="server" ForeColor="Green" Font-Bold="True"></asp:Label>
                条数据，失败
                <asp:Label ID="lbErrorCount" runat="server" ForeColor="#C00000" Font-Bold="True"></asp:Label>
                条数据；<br />
            </td>
        </tr>
        <tr>
            <td class="right">
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnBack4" runat="server" Text="继续导入其它工作薄" OnClick="btnBack4_Click" />
                <asp:Button ID="btnBack5" runat="server" Text="继续导入其它Excel数据" OnClick="btnBack5_Click" />
                <asp:Button ID="btnOutpt" runat="server" Text="导出错误数据" Visible="false" OnClick="btnOutpt_Click" />
            </td>
        </tr>
    </table>
</fieldset>
<fieldset id="fdPanel5" runat="server" visible="false">
    <legend>失败导入的结果如下：</legend>
    <asp:GridView ID="gvError" runat="server" AutoGenerateColumns="True">
    </asp:GridView>
</fieldset>
