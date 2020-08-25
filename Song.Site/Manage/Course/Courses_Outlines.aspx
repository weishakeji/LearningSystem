<%@ Page Language="C#" MasterPageFile="~/Manage/Course/CourseEdit.Master" Title="课程章节"
    AutoEventWireup="true" CodeBehind="Courses_Outlines.aspx.cs" Inherits="Song.Site.Manage.Course.Courses_Outlines" %>

<%@ MasterType VirtualPath="~/Manage/Course/CourseEdit.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="app-area">
        <template>
  <el-table
    ref="multipleTable"
    :data="tableDatas"
    tooltip-effect="dark"
    style="width: 100%"
    v-on:selection-change="handleSelectionChange">
    <el-table-column
      type="selection"
      width="55">
    </el-table-column>
    <el-table-column
      label="课程章节">
      <template slot-scope="scope"><span class="treeIco">{{ scope.row.Tree }}</span> 
      <l title='直播'  v-show="scope.row.Ol_IsLive"></l>
      
      {{ scope.row.Ol_Name }}</template>
    </el-table-column>
 <el-table-column
      label="试题"
      width="60">
      <template slot-scope="scope">{{ scope.row.Ol_QuesCount }}</template>
    </el-table-column>
    <el-table-column
      label="内容"
      width="120">
      <template slot-scope="scope">视频 直播 附件</template>
    </el-table-column>
     <el-table-column
      label="状态"
      width="195">
      <template slot-scope="scope">
      
      <el-button-group size="mini">
  <el-button type="succes" v-if="scope.row.Ol_IsFree"  size="mini" plain>免费</el-button>
  <el-button type="warning" v-if="!scope.row.Ol_IsFree"  size="mini" plain>收费</el-button>
  <el-button type="primary" v-show="scope.row.Ol_IsUse" size="mini" plain>启用</el-button>
  <el-button type="danger" v-show="!scope.row.Ol_IsUse" size="mini" plain>禁用</el-button>
  <el-button type="info" size="mini" plain>完结</el-button>
</el-button-group></template>
    </el-table-column>
     <el-table-column
      label="编辑"
      width="120">
      <template slot-scope="scope">删除 修改</template>
    </el-table-column>
  </el-table>
  </template>
    </div>
</asp:Content>
