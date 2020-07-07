<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Students_Details.aspx.cs" Inherits="Song.Site.Manage.Student.Students_Details"
    Title="学员信息详情" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
<script type="text/javascript" src="/Utility/CoreScripts/jquery.qrcode.min.js"></script>
<script type="text/javascript" src="/Manage/CoreScripts/jquery.jqprint-0.3.js"></script>
    <div id="app-area" v-clock>
    <div id="loading" v-show="loading"><img src="../Images/loading/load_016.gif" />
    <div>加载中... 学员{{students.length}}个，完成 {{finishcount<=2 ? 0 : finishcount-2}} </div>
    </div>
 <template  v-for="d in students" v-if="!loading">
        <div class="page"  v-show="!loading">       
        <img id="imgStamp" :src="stamp.path"  :class="'stamp '+stamp.positon" remark="公章"/>
        <div class="qrcode" :acid="stid"></div>
         <div class="page-title">学习证明</div>
         <table width="100%" class="first" border="1" cellspacing="0" cellpadding="0">
                <tr>
                    <td class="right" width="120px">
                        姓名：
                    </td>
                    <td class="left" width="100px">
                        {{d.Ac_Name}}
                    </td>
                    <td class="right" width="100px">
                        性别：
                    </td>
                    <td class="left">
                        {{d.Ac_Sex== 0 ? "未知" : (d.Ac_Sex== 1 ? "男" : "女")}}
                    </td>
                    <td rowspan="5" valign="middle" class="photo">
                  
                        <img :src='d.Ac_Photo' 
                                width="150px" height="200px" onerror="this.style.setProperty('display','none')" />
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        年龄：
                    </td>
                    <td class="left">                    
                        {{d.Ac_Age > 200 || d.Ac_Age<=0 ? "" : d.Ac_Age}}
                    </td>
                    <td class="right">
                        出生年月：
                    </td>
                    <td class="left">
                    {{birthday('yyyy年M月',d.Ac_Birthday)}}
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        籍贯：
                    </td>
                    <td class="left">
                        {{d.Ac_Native}}
                    </td>
                    <td class="right">
                        学号：
                    </td>
                    <td class="left">
                        {{d.Ac_CodeNumber}}
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        民 族：
                    </td>
                    <td class="left">
                        {{d.Ac_Nation}}
                    </td>
                    <td class="right">
                        身份证：
                    </td>
                    <td class="left">
                        <span class="txtrow">
                            {{d.Ac_IDCardNumber}}</span>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        学历：
                    </td>
                    <td class="left">
                        {{getedu(d.Ac_Education)}}
                    </td>
                    <td class="right">
                        专业：
                    </td>
                    <td class="left">
                        {{d.Ac_Major}}
                    </td>
                </tr>
            </table>
         <table width="100%" class="second" border="1" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="right" width="120px">
                            毕业院校：
                        </td>
                        <td class="left" colspan="4">
                            {{d.Ac_School}}
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                            通讯地址：
                        </td>
                        <td class="left" colspan="4">
                            {{d.Ac_AddrContact}}
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                            邮编：
                        </td>
                        <td class="left" width="100px">
                            {{d.Ac_Zip}}
                        </td>
                        <td class="right" width="100px">
                            电话：
                        </td>
                        <td class="left" colspan="2">
                            {{d.Ac_MobiTel1}}
                            &nbsp;
                            {{d.Ac_MobiTel2==d.Ac_MobiTel1 ? "" : d.Ac_MobiTel2}}
                            &nbsp;
                            {{d.Ac_Tel}}
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                            网路通讯：
                        </td>
                        <td class="left" colspan="4">
                            <span class="txtrow">
                                {{d.Ac_Email != null ? "Email：" + d.Ac_Email : ""}}
                                &nbsp;
                                {{d.Ac_Weixin  != null ? "微信：" + d.Ac_Weixin : ""}}
                                &nbsp;
                                {{d.Ac_Qq  != null ? "QQ：" + d.Ac_Qq : ""}}
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                            紧急联系人：
                        </td>
                        <td class="left" colspan="4">
                            {{d.Ac_LinkMan}}
                            &nbsp;
                            {{d.Ac_LinkManPhone != null ? "联系电话：" +d.Ac_LinkManPhone  : ""}}
                        </td>
                    </tr>
                    </table>
         <table width="100%" class="three" border="1" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="center">
                            学习情况 <span style="font-weight:normal">（{{ d.courses ? d.courses.length : 0}} 门课程
                            {{d.pager ? '，分'+ (d.pager+1)+'页打印': ''}}
                            ）</span>
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
                                <dd v-for="(c,index) in d.courses" v-if="index<coursepager.first">
                                <div class="cou">
                                              {{index+1}}.  《
                                                {{c.Cou_Name}}
                                                》
                                            </div>
                                            <div class="date">
                                            
                                                {{ format('yyyy-MM-dd',new Date(c.lastTime))}}
                                            </div>
                                               <div class="complete">
                                               {{getPercent(c.complete)}}
                                               </div>
                                </dd>
                            </dl>
                            <%--机构信息--%>
                            <div class="info-foot">
                                <div class="plate-name">
                                    {{org.Org_Name}}
                                </div>
                                <div class="output-date">
                                   
                                    {{format('yyyy年M月d日',new Date())}}
                                </div>
                            </div>
                            
                        </td>
                    </tr>
                </table>
        </div>
        <template v-if="d.courses && d.courses.length>10" v-for="p in d.pager">
         <div class="page"> 
          <img id="img1" :src="stamp.path"  :class="'stamp '+stamp.positon" remark="公章"/>
        <div class="qrcode"></div>
         <div class="pagerinfo"><span>学员： {{d.Ac_Name}}</span>  &nbsp;&nbsp;
         <span v-show="d.Ac_IDCardNumber!=null">身份证号：{{d.Ac_IDCardNumber}}</span>
         <span v-show="d.Ac_CodeNumber!=null">学号：{{d.Ac_CodeNumber}}</span>
          </div>
          <table width="100%" class="four" border="1" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="center">
                            学习情况 <span style="font-weight:normal">（累计学习 {{ d.courses ? d.courses.length : 0}} 门课程，<span>第{{p+1}}/{{d.pager+1}}页</span>）</span>
                        </td>
                    </tr>
                     <tr>
                        <td class="info-area">
                            <dl class="rtpLearnInfo" style="min-height: 20.7cm">
                                <dt>
                                    <div class="cou">
                                        课程</div>
                                    <div class="date">
                                        学习时间</div>
                                    <div class="complete">
                                        完成度</div>
                                </dt>
                                <dd v-for="(c,index) in d.courses" v-if="index>=coursepager.first+(p-1)*coursepager.size && index<coursepager.first+p*coursepager.size">
                                <div class="cou">
                                              {{index+1}}.  《
                                                {{c.Cou_Name}}
                                                》
                                            </div>
                                            <div class="date">
                                            
                                                {{ format('yyyy-MM-dd',new Date(c.lastTime))}}
                                            </div>
                                               <div class="complete">
                                               {{getPercent(c.complete)}}
                                               </div>
                                </dd>
                            </dl>
                            <%--机构信息--%>
                            <div class="info-foot">
                                <div class="plate-name">
                                    {{org.Org_Name}}
                                </div>
                                <div class="output-date">
                                   
                                    {{format('yyyy年M月d日',new Date())}}
                                </div>
                            </div>
                            
                        </td>
                    </tr>
                    </table>
         </div>
        </template>
</template>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
<input type="button" name="btnPrint" value="打印" id="btnPrint"/>
</asp:Content>
