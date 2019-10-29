<%@ Page Language="C#" MasterPageFile="~/Manage/Course/CourseEdit.Master" AutoEventWireup="true"
    CodeBehind="Courses_Message.aspx.cs" Inherits="Song.Site.Manage.Course.Courses_Message" %>

<%@ MasterType VirtualPath="~/Manage/Course/CourseEdit.Master" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div id="app">
        <dl class="outlineArea">
        <dt v-on:click="outlineClick(0,$event)" :class="(olid==0 ? 'current' : '')">所有留言 (<msgcount :olid="0"></msgcount>)</dt>
            <template v-for="d in outlines" remark="章节列表">
              <dd :class="'olitem '+(d.Ol_IsVideo ? 'li-video ' : '')+(d.Ol_ID==olid ? 'current ' : '')+(d.Ol_IsLive ? 'li-live ' : '')"
             :isvideo="d.Ol_IsVideo" :olid="d.Ol_ID" :title="d.Ol_Name"
             :style="'padding-left:'+(d.Ol_Level*15+5)+'px'"  v-on:click="outlineClick(d.Ol_ID,$event)">
            {{d.Ol_XPath}}{{d.Ol_Name}}<msgcount :olid="d.Ol_ID"></msgcount>
              </dd>
   </template>
        </dl>
        <div class="rightArea">
        <el-table :data="messages"  :max-height="tableHeight"  stripe border style="width: 100%"
         v-on:row-click="rowClick" >
            <el-table-column type="index" width="70">
                <template scope="scope">
                    <div style="text-align:center;">{{scope.$index+(index - 1) * size + 1}}</div>
                </template>
            </el-table-column>
            <el-table-column prop="Msg_Context" label="留言">
                <template scope="scope">                   
                    <span v-html="scope.row.Msg_Context"></span>
                </template>
            </el-table-column>
            <el-table-column prop="Msg_CrtTime" label="时间" width="160">
                <template scope="scope">
                   {{scope.row.Msg_CrtTime|date('yyyy-MM-dd hh:mm')}}
                </template>
            </el-table-column>

            
        </el-table>
        <div id="pager-box">
        <el-pagination v-on:current-change="handleCurrentChange" :current-page="index"
            :page-sizes="[15, 30, 50, 100]" :page-size="size" :pager-count="12"
            layout="total, prev, pager, next, jumper" :total="total">
        </el-pagination>
    </div>
        </div>
        <el-dialog title="留言管理" :visible.sync="dialogFormVisible">
  <el-form :model="msgobj">
    <el-form-item label="留言：" label-width="60">
  
     <el-input  type="textarea" v-model="msgobj.Msg_Context" autocomplete="off"></el-input>
    </el-form-item>
    <el-form-item label="时间：">
      {{msgobj.Msg_CrtTime|date('yyyy-MM-dd hh:mm')}}    
    </el-form-item>
    <el-form-item label="学员：">
      {{msgobj.Ac_Name}}      {{msgobj.Msg_Phone}} 
    </el-form-item>
    <el-form-item label="IP：">
      {{msgobj.Msg_IP}}
    </el-form-item>
  </el-form>
  <div slot="footer" class="dialog-footer">

    <el-button @click="dialogFormVisible = false">取 消</el-button>
    <el-button type="danger" @click="msgDel">删除</el-button>
    <el-button type="primary" v-on:click="msgEdit">确 定</el-button>
  </div>
</el-dialog>
    </div>
</asp:Content>
