using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;



namespace Song.Site.Manage.Sys
{
    public partial class Employee_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //员工上传资料的所有路径
        private string _uppath = "Employee";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitBind();
                fill();
            }   
            //密码输入框的显示与否
            this.trPw1.Visible = id == 0;
        }
        private void fill()
        {

            EmpAccount ea;
            if (id != 0)
            {
                ea = Business.Do<IEmployee>().GetSingle(id);
                if (ea == null) return;
                //所属角色
                //ddlPosi.Enabled = !Business.Do<IEmployee>().IsAdmin(id);
                ListItem liPosi = ddlPosi.Items.FindByValue(ea.Posi_Id.ToString());
                if (liPosi != null)
                {
                    ddlPosi.SelectedIndex = -1;
                    liPosi.Selected = true;
                }
                //所属的组
                Song.Entities.EmpGroup[] egs = Business.Do<IEmpGroup>().GetAll4Emp(id);
                foreach (Song.Entities.EmpGroup eg in egs)
                {
                    ListItem li = this.cblGroup.Items.FindByValue(eg.EGrp_Id.ToString());
                    if (li != null) li.Selected = true;
                }
                //性别
                string sex = ea.Acc_Sex.ToString().ToLower();
                ListItem liSex = rbSex.Items.FindByValue(sex);
                if (liSex != null)
                {
                    rbSex.SelectedIndex = -1;
                    liSex.Selected = true;
                }
                //年龄
                if (ea.Acc_Age > DateTime.Now.AddYears(-100).Year)
                {
                    tbAge.Text = (DateTime.Now.Year - ea.Acc_Age).ToString();
                }
                //是否在职
                cbIsUse.Checked = ea.Acc_IsUse;
                //是否全职
                cbIsPartTime.Checked = ea.Acc_IsPartTime;
                //个人照片
                if (!string.IsNullOrEmpty(ea.Acc_Photo) && ea.Acc_Photo.Trim() != "")
                {
                    this.imgShow.Src = Upload.Get[_uppath].Virtual + ea.Acc_Photo;
                }
            }
            else
            {
                ea = new EmpAccount();
                ea.Acc_RegTime = DateTime.Now;
            }
            //员工帐号
            this.tbAccName.Text = ea.Acc_AccName;
            //身份证
            this.tbIdCard.Text = ea.Acc_IDCardNumber;
            //员工名称与拼音缩写
            this.tbName.Text = ea.Acc_Name;
            tbNamePinjin.Text = ea.Acc_NamePinyin;
            //员工登录密码，为空
            //邮箱
            this.tbEmail.Text = ea.Acc_Email;
            //员工工号
            this.tbEmpCode.Text = ea.Acc_EmpCode;
            //入职时间
            DateTime regTime = ea.Acc_RegTime;
            this.tbRegTime.Text = regTime.ToString("yyyy-MM-dd");
            //所在院系
            ListItem liDepart = ddlDepart.Items.FindByValue(ea.Dep_Id.ToString());
            if (liDepart != null)
            {
                liDepart.Selected = true;
                ddlDepart_SelectedIndexChanged(null, null);
            }
            //职务（头衔）
            ListItem liTitle = ddlTitle.Items.FindByValue(ea.Title_Id.ToString());
            if (liTitle != null) liTitle.Selected = true;
            //联系方式
            tbTel.Text = ea.Acc_Tel;
            cbIsOpenTel.Checked = ea.Acc_IsOpenTel;
            tbMobleTel.Text = ea.Acc_MobileTel;
            cbIsOpenMobile.Checked = ea.Acc_IsOpenMobile;
            tbQQ.Text = ea.Acc_QQ;
            tbWeixin.Text = ea.Acc_Weixin;
            //离职时间
            cbIsAutoOut.Checked = ea.Acc_IsAutoOut;
            DateTime outTime = ea.Acc_OutTime;
            if (outTime.AddYears(100) > DateTime.Now)
            {
                this.tbOutTime.Text = outTime.ToString("yyyy-MM-dd");
            }
            cbIsAutoOut.Enabled = ea.Acc_IsUse;
            //如果需要设置自动离职，则显示离职时间
            cbIsAutoOut_CheckedChanged(null, null);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            EmpAccount obj = null;
            Song.Entities.Organization org;
            if (Extend.LoginState.Admin.IsSuperAdmin)
                org = Business.Do<IOrganization>().OrganSingle(Extend.LoginState.Admin.CurrentUser.Org_ID);
            else
                org = Business.Do<IOrganization>().OrganCurrent();
            if (org == null) throw new WeiSha.Common.ExceptionForAlert("当前机构不存在");
            try
            {
                if (id == 0)
                {
                    obj = new EmpAccount();                   
                    obj.Org_ID = org.Org_ID;
                    obj.Org_Name = org.Org_Name;
                }
                else
                {
                    obj = Business.Do<IEmployee>().GetSingle(id);
                    //先清除与用户组的关联
                    Business.Do<IEmpGroup>().DelRelation4Emplyee(id);
                }
                //帐号
                obj.Acc_AccName = this.tbAccName.Text.Trim();
                //员工名称
                obj.Acc_Name = this.tbName.Text.Trim();
                obj.Acc_NamePinyin = tbNamePinjin.Text.Trim();
                //员工登录密码，为空
                if (tbPw1.Text != "")
                {
                    //string md5 = WeiSha.Common.Request.Controls[tbPw1].MD5;
                    obj.Acc_Pw = tbPw1.Text.Trim();
                }
                //身份证
                obj.Acc_IDCardNumber = tbIdCard.Text.Trim();
                //邮箱
                obj.Acc_Email = this.tbEmail.Text.Trim();
                //是否在职，是否全职
                obj.Acc_IsUse = this.cbIsUse.Checked;
                obj.Acc_IsPartTime = cbIsPartTime.Checked;
                //员工工号
                obj.Acc_EmpCode = this.tbEmpCode.Text.Trim();
                //性别
                obj.Acc_Sex = Convert.ToInt16(rbSex.SelectedValue);
                //年龄
                if (tbAge.Text.Trim() != "")
                {
                    obj.Acc_Age = DateTime.Now.Year - Convert.ToInt32(tbAge.Text.Trim());
                }
                //入职时间
                obj.Acc_RegTime = Convert.ToDateTime(this.tbRegTime.Text);
                //所在院系
                obj.Dep_Id = Convert.ToInt16(ddlDepart.SelectedValue);
                if (spanTeam.Visible)
                {
                    obj.Team_ID = Convert.ToInt32(ddlTeam.SelectedItem.Value);
                    obj.Team_Name = ddlTeam.SelectedItem.Text;
                }
                //所属角色
                obj.Posi_Id = Convert.ToInt16(ddlPosi.SelectedValue);
                //职务（头衔）
                obj.Title_Id = Convert.ToInt32(ddlTitle.SelectedValue);
                obj.Title_Name = ddlTitle.SelectedItem.Text;
                //联系方式
                obj.Acc_Tel = tbTel.Text;
                obj.Acc_IsOpenTel = cbIsOpenTel.Checked;
                obj.Acc_MobileTel = tbMobleTel.Text;
                obj.Acc_IsOpenMobile = cbIsOpenMobile.Checked;
                obj.Acc_QQ = tbQQ.Text;
                obj.Acc_Weixin = tbWeixin.Text.Trim();
                //离职时间
                obj.Acc_IsAutoOut = cbIsAutoOut.Checked;
                //图片
                if (fuLoad.PostedFile.FileName != "")
                {
                    try
                    {
                        fuLoad.UpPath = _uppath;
                        fuLoad.IsMakeSmall = false;
                        fuLoad.IsConvertJpg = true;
                        fuLoad.SaveAndDeleteOld(obj.Acc_Photo);
                        fuLoad.File.Server.ChangeSize(150, 200, false);
                        obj.Acc_Photo = fuLoad.File.Server.FileName;
                        //
                        imgShow.Src = fuLoad.File.Server.VirtualPath;
                    }
                    catch (Exception ex)
                    {
                        this.Alert(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
            try
            {
                if (tbOutTime.Text.Trim() != "")
                {
                    obj.Acc_OutTime = Convert.ToDateTime(this.tbOutTime.Text);
                }
                bool isExists = Business.Do<IEmployee>().IsExists(org.Org_ID, obj);
                if (isExists)
                {
                    Master.Alert("当前帐号已经存在！");
                    return;
                }
                if (id == 0)
                {
                    id = Business.Do<IEmployee>().Add(obj);
                    this.AddRel(id);
                }
                else
                {
                    Business.Do<IEmployee>().Save(obj);
                    this.AddRel(id);
                }

                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }
        /// <summary>
        /// 添加与用户组的关联
        /// </summary>
        /// <param name="id">员工id</param>
        private void AddRel(int id)
        {
            try
            {
                foreach (ListItem li in this.cblGroup.Items)
                {
                    if (li.Selected)
                    {
                        int gid = Convert.ToInt32(li.Value);
                        Business.Do<IEmpGroup>().AddRelation(id, gid);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 界面的初始绑定
        /// </summary>
        private void InitBind()
        {
            try
            {
                int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
                Song.Entities.Depart[] nc = Business.Do<IDepart>().GetAll(orgid,true,true);
                this.ddlDepart.DataSource = nc;
                this.ddlDepart.DataTextField = "dep_cnName";
                this.ddlDepart.DataValueField = "dep_id";
                this.ddlDepart.DataBind();
                //ddlDepart.Items.Insert(0, new ListItem("", "-1"));
                //角色
                Song.Entities.Position[] posi = Business.Do<IPosition>().GetAll(orgid,true);
                foreach (Song.Entities.Position p in posi)
                {
                    if (p.Posi_IsAdmin)
                    {
                        p.Posi_Name = p.Posi_Name + "*";
                    }
                }
                ddlPosi.DataSource = posi;
                ddlPosi.DataTextField = "Posi_Name";
                ddlPosi.DataValueField = "Posi_Id";
                ddlPosi.DataBind();
                //ddlPosi.Items.Insert(0,new ListItem("", "-1"));
                //用户组
                Song.Entities.EmpGroup[] group = Business.Do<IEmpGroup>().GetAll(orgid,true);
                cblGroup.DataSource = group;
                cblGroup.DataTextField = "EGrp_Name";
                cblGroup.DataValueField = "EGrp_Id";
                cblGroup.DataBind();
                //职务（头衔）
                Song.Entities.EmpTitle[] title = Business.Do<IEmployee>().TitleAll(orgid,true);
                ddlTitle.DataSource = title;
                ddlTitle.DataTextField = "Title_Name";
                ddlTitle.DataValueField = "Title_Id";
                ddlTitle.DataBind();
                ddlTitle.Items.Insert(0, new ListItem("", "-1"));
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 当是否在职选项，改变时。
        /// 如果在职，则可以设定什么时间自动离职
        /// 如果已经不在职，则不必再设置自动离职
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbIsUse_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            //如果不在职，显示离职时间
            cbIsAutoOut_CheckedChanged(null, null);
            //是否预设自动离职
            cbIsAutoOut.Enabled = cb.Checked;
            cbIsAutoOut.Checked = false;
            //if (!cb.Checked) cbIsAutoOut.Enabled = false;
            
        }
        /// <summary>
        /// 如果设置自动离职，则显示离职时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbIsAutoOut_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //是否辞职
                bool isUse = cbIsUse.Checked;
                //
                spanOutTime.Visible = (cbIsAutoOut.Checked && cbIsAutoOut.Enabled && isUse) || !isUse;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 当选中院系时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDepart_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int depId = Convert.ToInt32(ddlDepart.SelectedValue);
                Song.Entities.Team[] nc = Business.Do<ITeam>().GetTeam(true, depId, 0);
                if (nc != null && nc.Length > 0)
                {
                    spanTeam.Visible = true;
                    this.ddlTeam.DataSource = nc;
                    this.ddlTeam.DataTextField = "Team_Name";
                    this.ddlTeam.DataValueField = "Team_ID";
                    this.ddlTeam.DataBind();
                }
                else
                {
                    spanTeam.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
