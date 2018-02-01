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
    public partial class Team_Edit : Extend.CustomPage
    {
        //要操作的对象主键
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                departBind();
                subjectBind();
                fill();
            }
        }
        /// <summary>
        /// 绑定院系
        /// </summary>
        private void departBind()
        {
            try
            {
                int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
                Song.Entities.Depart[] nc = Business.Do<IDepart>().GetAll(orgid,true,true);
                this.ddlDepart.DataSource = nc;
                this.ddlDepart.DataTextField = "dep_cnName";
                this.ddlDepart.DataValueField = "dep_id";
                this.ddlDepart.DataBind();
                this.ddlDepart.Items.Insert(0, new ListItem("---- 请选择院系 ----", "-1"));
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 绑定专业
        /// </summary>
        private void subjectBind()
        {
            try
            {
                Song.Entities.Subject[] nc = Business.Do<ISubject>().SubjectCount(true, 0);
                this.ddlSubject.DataSource = nc;
                this.ddlSubject.DataTextField = "Sbj_Name";
                this.ddlSubject.DataValueField = "Sbj_ID";
                this.ddlSubject.DataBind();
                this.ddlSubject.Items.Insert(0, new ListItem("---- 请选择专业 ----", "-1"));
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {
            try
            {
                Song.Entities.Team mm;
                if (id != 0)
                {
                    mm = Business.Do<ITeam>().TeamSingle(id);
                    //是否显示
                    cbIsUse.Checked = mm.Team_IsUse;
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.Team();
                }
                //组名称
                tbName.Text = mm.Team_Name;
                tbByName.Text = mm.Team_ByName;
                //所在院系
                ListItem liDepart = ddlDepart.Items.FindByValue(mm.Dep_ID.ToString());
                if (liDepart != null)
                {
                    ddlDepart.SelectedIndex = -1;
                    liDepart.Selected = true;
                }
                //所属学科
                ListItem liSubj = ddlSubject.Items.FindByValue(mm.Sbj_ID.ToString());
                if (liSubj != null)
                {
                    ddlSubject.SelectedIndex = -1;
                    liSubj.Selected = true;
                }
                //说明
                tbIntro.Text = mm.Team_Intro;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.Team mm=null;
            try
            {
                if (id != 0)
                {
                    mm = Business.Do<ITeam>().TeamSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.Team();
                }
                //属性
                mm.Team_Name = tbName.Text.Trim();
                mm.Team_ByName = tbByName.Text.Trim();
                //院系
                mm.Dep_ID = Convert.ToInt32(ddlDepart.SelectedValue);
                //学科
                mm.Sbj_ID = Convert.ToInt32(ddlSubject.SelectedValue);
                mm.Sbj_Name = ddlSubject.SelectedItem.Text;
                //是否允许
                mm.Team_IsUse = cbIsUse.Checked;
                //说明
                mm.Team_Intro = tbIntro.Text;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
            //确定操作
            try
            {
                if (id == 0)
                      Business.Do<ITeam>().TeamAdd(mm);
                else
                    Business.Do<ITeam>().TeamSave(mm);
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }
    }
}
