<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Organization_Edit.aspx.cs" Inherits="Song.Site.Manage.Sys.Organization_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <script type="text/javascript" src="../CoreScripts/iColorPicker.js"></script>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
     <tr>
            <td class="right" width="80">
                平台名称：
            </td>
            <td>
            <asp:TextBox nullable="false" ID="Org_PlatformName" group="para" runat="server" MaxLength="200"
                        Width="90%"></asp:TextBox>
            </td>
            </tr>

        <tr>
            <td class="right">
                机构全称：
            </td>
            <td>
                <asp:TextBox ID="tbName" nullable="false" runat="server" MaxLength="200" Width="330px"></asp:TextBox>
                中文简称：
                <asp:TextBox ID="tbAbbrName" nullable="false" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                英文全称：
            </td>
            <td>
                <asp:TextBox ID="tbEnName" runat="server" MaxLength="255" Width="330px"></asp:TextBox>
                英文简称：
                <asp:TextBox ID="tbAbbrEnName" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td valign="top" class="right">
                公司地址：
            </td>
            <td>
                <div>
                    <asp:TextBox ID="tbAddress" runat="server" Width="560px" MaxLength="255"></asp:TextBox>
                </div>
                <div class="gpsRow">
                    <div id="btnViewmap" title="查询地理信息">
                        &nbsp;</div>
                    <div class="gpsBox">
                        经度：
                        <asp:TextBox ID="tbLng" runat="server" MaxLength="200" Width="160"></asp:TextBox>
                        纬度：
                        <asp:TextBox ID="tbLat" runat="server" MaxLength="200" Width="160"></asp:TextBox>
                        <span class="showTxt">点击地图获取坐标</span>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td class="right">
                &nbsp;
            </td>
            <td valign="top">
                <div id="map" style="height: 210px; width: 650px;">
                </div>
            </td>
        </tr>
        <tr>
            <td class="right">
                电 话：
            </td>
            <td>
                <asp:TextBox ID="tbPhone" runat="server" MaxLength="200" Width="160"></asp:TextBox>
                传 真：
                <asp:TextBox ID="tbFax" runat="server" MaxLength="100" Width="160"></asp:TextBox>
                邮 编：
                <asp:TextBox ID="tbZip" runat="server" MaxLength="6" Width="160"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                电子信箱：
            </td>
            <td>
                <asp:TextBox ID="tbMail" runat="server" MaxLength="255" Width="160"></asp:TextBox>
                企业微信：<asp:TextBox ID="tbWeixin" runat="server" MaxLength="255" Width="160"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                联系人：
            </td>
            <td>
                <asp:TextBox ID="tbLinkman" runat="server" MaxLength="255" Width="160"></asp:TextBox>
                联系电话：<asp:TextBox ID="tbLinkmanPhone" runat="server" MaxLength="255" Width="160"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                &nbsp;
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="right">
                机构等级：
            </td>
            <td>
                <asp:DropDownList ID="ddlLevel" runat="server" Width="160">
                </asp:DropDownList>
                &nbsp;是否启用：
                <asp:CheckBox ID="cbIsUse" runat="server" Checked="true" />
            </td>
        </tr>
        <tr>
            <td class="right">
                域名：
            </td>
            <td>
                <asp:TextBox ID="tbDomain" runat="server" MaxLength="255" Width="160" nullable="false"></asp:TextBox>.<asp:Label
                    ID="lbDomain" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                模板风格：
            </td>
            <td>
                <asp:TextBox ID="tbTemplate" runat="server" MaxLength="255" Width="160"></asp:TextBox>
            </td>
        </tr>
    </table>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=MBUg6BlQ9sowj5824cX1IKIG"></script>
    <script type="text/javascript" src="../CoreScripts/baiduMap/convertor.js"></script>
    <script type="text/javascript">
        var lng = Number($("input[id$='tbLng']").val());
        var lat = Number($("input[id$='tbLat']").val());
        lng = lng == 0 ? 116.404 : lng;
        lat = lat == 0 ? 39.915 : lat;
        //创建地图
        var map = new BMap.Map("map");
        map.enableScrollWheelZoom();
        map.enableKeyboard();
        map.addControl(new BMap.NavigationControl());
        map.centerAndZoom(new BMap.Point(lng, lat), 16);
        var marker = new BMap.Marker(new BMap.Point(lng, lat));  // 创建标注
        map.addOverlay(marker);
        //鼠标点击事件
        function showInfo(e) {
            var lng = e.point.lng;
            var lat = e.point.lat;
            map.clearOverlays();
            map.closeInfoWindow();
            var zoom = map.getZoom();
            //alert(zoom);
            map.centerAndZoom(new BMap.Point(lng, lat), zoom);
            var point = new BMap.Point(lng, lat);
            var marker = new BMap.Marker(new BMap.Point(lng, lat));  // 创建标注
            map.addOverlay(marker);
            //填充经纬度
            $("input[id$='tbLng']").val(lng);
            $("input[id$='tbLat']").val(lat);
        }
        map.addEventListener("click", showInfo);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="BtnEnter_Click"
        ValidationGroup="enter" />
    <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
