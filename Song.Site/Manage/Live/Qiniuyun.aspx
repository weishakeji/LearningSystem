<%@ Page Language="C#" MasterPageFile="~/Manage/ElementUI.Master" AutoEventWireup="true"
    CodeBehind="Qiniuyun.aspx.cs" Inherits="Song.Site.Manage.Live.Qiniuyun" Title="��ţ��ֱ������" %>
    
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
 <div id="app-form">
  <el-form ref="form" :model="form"  :rules="rules" label-width="160px" size="small">

  <el-form-item label="AccessKey��" prop="AccessKey">
    <el-input v-model="form.AccessKey" style="width: 400px"></el-input>
  </el-form-item>
 <el-form-item label="SecretKey��" prop="SecretKey">
    <el-input v-model="form.SecretKey" style="width: 400px"></el-input>
  </el-form-item>
  <el-form-item label="ֱ���ռ�����">
    <el-input v-model="form.LiveSpace" style="width: 200px"></el-input>
    <el-button type="info" v-on:click="btnTest"  v-loading="loading" >�� ֤ �� ��</el-button>
  </el-form-item>
   <el-divider></el-divider>
    <el-form-item label="HTTPS���ã�">
   <el-radio-group v-model="form.Protocol">
    <el-radio label="http">��ֹ HTTPS</el-radio>
    <el-radio label="https">���� HTTPS</el-radio>
  </el-radio-group>
  </el-form-item>
   <el-form-item label="ֱ������������">
    <el-input v-model="form.Snapshot" style="width: 400px"></el-input>
  </el-form-item>
   <el-form-item label="�㲥������">
    <el-input v-model="form.Vod" style="width: 400px"></el-input>
  </el-form-item>

  <el-form-item>
    <el-button type="primary" v-on:click="btnEnter"  v-loading="loading" >ȷ ��</el-button>
    
  </el-form-item>
</el-form>
</div>

</asp:Content>
