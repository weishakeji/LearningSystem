<%@ Page Language="C#" MasterPageFile="~/Manage/ElementUI.Master" AutoEventWireup="true"
    CodeBehind="Qiniuyun.aspx.cs" Inherits="Song.Site.Manage.Live.Qiniuyun" Title="七牛云直播设置" %>
    
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
 <div id="app-form">
  <el-form ref="form" :model="form"  :rules="rules" label-width="160px" size="small">

  <el-form-item label="AccessKey：" prop="AccessKey">
    <el-input v-model="form.AccessKey" style="width: 400px"></el-input>
  </el-form-item>
 <el-form-item label="SecretKey：" prop="SecretKey">
    <el-input v-model="form.SecretKey" style="width: 400px"></el-input>
  </el-form-item>
  <el-form-item label="直播空间名：">
    <el-input v-model="form.LiveSpace" style="width: 300px"></el-input>
  </el-form-item>
   <el-divider></el-divider>
   
   <el-form-item label="直播封面域名：">
    <el-input v-model="form.Snapshot" style="width: 400px"></el-input>
  </el-form-item>
   <el-form-item label="点播域名：">
    <el-input v-model="form.Vod" style="width: 400px"></el-input>
  </el-form-item>

  <el-form-item>
    <el-button type="primary" v-on:click="btnEnter"  v-loading="loading" >确 定</el-button>
   
  </el-form-item>
</el-form>
</div>

</asp:Content>
