<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Learningcard_Courses.aspx.cs" Inherits="Song.Site.Manage.Card.Learningcard_Courses"
    Title="学习卡关联课程" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="Kind" %>
<%@ Register Src="../Utility/SortSelect.ascx" TagName="SortSelect" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
<div class="topbox">
 
 <el-select v-model="form.orgid" placeholder="请选择"  style="width:32%">
    <el-option
      v-for="item in orgs"
      :key="item.Org_ID"
      :label="item.Org_PlatformName"
      :value="item.Org_ID">
    </el-option>
  </el-select>

  <el-cascader
    :options="sbjs" v-model="sbjids" placeholder="请选择专业" :show-all-levels="true" style="width:32%"
    :props="{ checkStrictly: true }"
    clearable></el-cascader>
    <el-input v-model="form.search" placeholder="课程检索" style="width:32%"></el-input>
</div>
<div>
<el-table
    :data="courses"
    tooltip-effect="dark"
    style="width: 46%; float:left" :max-height="tableHeight">
     <el-table-column
      type="index"
      width="50">
    </el-table-column>
    <el-table-column
      label="供选择的课程">
      <template slot-scope="scope">{{ scope.row.Cou_Name }}</template>
    </el-table-column>
   <el-table-column
      fixed="right"
      label="选择"
      width="60">
      <template slot="header" slot-scope="scope">
       <el-button type="text"  v-on:click="handleSelectCourse()">全选</el-button>
      </template>
      <template slot-scope="scope">        
        <el-button type="text" size="small" v-on:click="handleSelectCourse(scope.row)" >选择</el-button>
      </template>
    </el-table-column>
  </el-table>
  <el-table
    :data="selects"
    tooltip-effect="dark"
    style="width: 46%; float:right" :max-height="tableHeight">
     <el-table-column
      type="index"
      width="50">
    </el-table-column>
    <el-table-column
      label="已选择课程">
      <template slot-scope="scope">{{ scope.row.name }}</template>
    </el-table-column>
   <el-table-column
      fixed="right"
      label="删除"
      width="60">
      <template slot="header" slot-scope="scope">
       <el-button type="text"  v-on:click="handleRemoveourse()" >清空</el-button>
      </template>
      <template slot-scope="scope">
        
       <el-button type="danger" icon="el-icon-delete" size="mini" circle  v-on:click="handleRemoveourse(scope.row)" ></el-button>
      </template>
    </el-table-column>
  </el-table>

</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定"
        ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
