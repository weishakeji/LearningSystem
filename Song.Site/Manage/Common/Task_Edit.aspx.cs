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
using WeiSha.WebControl;

namespace Song.Site.Manage.Common
{
    public partial class Task_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
          protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ddlDepartBind();
                setLoyout();
                fill();
            }
        }
        #region 初始数据
        /// <summary>
        /// 院系下拉绑定
        /// </summary>
        private void ddlDepartBind()
        {
            try
            {
                int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
                Song.Entities.Depart[] nc = Business.Do<IDepart>().GetAll(orgid, true, true);
                this.ddlDepart.DataSource = nc;
                this.ddlDepart.DataTextField = "dep_cnName";
                this.ddlDepart.DataValueField = "dep_id";
                this.ddlDepart.DataBind();
                this.ddlDepart.Items.Insert(0, new ListItem(" -- 所有院系 -- ", "-1"));
                ddlDepart_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 院系下拉列表的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDepart_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                WeiSha.WebControl.DropDownTree ddt = (WeiSha.WebControl.DropDownTree)sender;
                if (ddt == null) ddt = this.ddlDepart;
                int depId = Convert.ToInt32(ddt.SelectedValue);
                int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
                Song.Entities.EmpAccount[] eas = Business.Do<IEmployee>().GetAll(orgid,depId, true,null);
                this.ddlEmployee.DataSource = eas;
                this.ddlEmployee.DataTextField = "Acc_Name";
                this.ddlEmployee.DataValueField = "Acc_Id";
                this.ddlEmployee.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 如果是修改模式，设置指派给哪个员工，且该员工所处的院系
        /// </summary>
        /// <param name="accid">员工Id</param>
        private void setDepart(int accid)
        {
            try
            {
                ddlDepart.SelectedIndex = -1;
                //设置院系
                Song.Entities.Depart dep = Business.Do<IEmployee>().Get4Depart(accid);
                if (dep == null)
                {
                    ddlDepart.Items.FindByValue("-1").Selected = true;
                    return;
                }
                ListItem liDepart = ddlDepart.Items.FindByValue(dep.Dep_Id.ToString());
                if (liDepart != null)
                {
                    liDepart.Selected = true;
                    ddlDepart_SelectedIndexChanged(null, null);
                }
                //设定员工
                ListItem li = ddlEmployee.Items.FindByValue(accid.ToString());
                if (li != null)
                {
                    ddlEmployee.SelectedIndex = -1;
                    li.Selected = true;
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }
        /// <summary>
        /// 设置初始化布局
        /// </summary>
        private void setLoyout()
        {
            //是否关闭与是否完成等，在新增时不显示
            plPara.Visible = id != 0;
        }
        #endregion
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {
            try
            {
                //
                Song.Entities.Task mm;
                if (id != 0)
                {
                    mm = Business.Do<ITask>().GetSingle(id);
                    //优先级
                    ListItem li = ddlLevel.Items.FindByText(mm.Task_Level.ToString());
                    if (li != null) li.Selected = true;
                    tbStart.Text = ((DateTime)mm.Task_StartTime).ToString("yyyy-MM-dd");
                    tbEnd.Text = ((DateTime)mm.Task_EndTime).ToString("yyyy-MM-dd");
                    //是否完成
                    cbIsComplete.Checked = mm.Task_IsComplete;
                    if (mm.Task_IsComplete)
                    {
                        tbComplete.Text = ((DateTime)mm.Task_CompleteTime).ToString("yyyy-MM-dd");
                    }
                    //设置指派给谁
                    setDepart((int)mm.Task_WorkerId);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.Task();
                    tbStart.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    tbEnd.Text = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
                    mm.Task_IsUse = true;
                }
                //任务名称
                tbName.Text = mm.Task_Name;
                //任务内容
                tbContext.Text = mm.Task_Context;
                //是否使用
                cbIsUse.Checked = !mm.Task_IsUse;
                //完成度
                tbCompletePer.Text = mm.Task_CompletePer.ToString();

                //完成时间所在的区域
                divCompleteBox.Visible = mm.Task_IsComplete;
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
            try
            {
                Song.Entities.Task mm;
                if (id != 0)
                {
                    mm = Business.Do<ITask>().GetSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.Task();
                    //创建时间
                    mm.Task_CrtTime = DateTime.Now;
                }
                //任务名称
                mm.Task_Name = tbName.Text.Trim();
                //任务内容
                mm.Task_Context = tbContext.Text.Trim();
                //任务等级
                mm.Task_Level = Convert.ToInt16(ddlLevel.SelectedItem.Text);
                //计划开始时间，计划结束时间，实际结束时间
                mm.Task_StartTime = Convert.ToDateTime(tbStart.Text);
                mm.Task_EndTime = Convert.ToDateTime(tbEnd.Text);
                if (tbComplete.Text.Trim() != "")
                {
                    mm.Task_CompleteTime = Convert.ToDateTime(tbComplete.Text);
                }
                //所属员工
                EmpAccount acc = Extend.LoginState.Admin.CurrentUser;
                mm.Acc_Id = acc.Acc_Id;
                mm.Acc_Name = acc.Acc_Name;
                //指派给谁
                mm.Task_WorkerId = Convert.ToInt32(ddlEmployee.SelectedValue);
                //是否使用
                mm.Task_IsUse = !cbIsUse.Checked;
                //是否完成
                mm.Task_IsComplete = cbIsComplete.Checked;
                //完成度
                if (tbCompletePer.Text.Trim() != "")
                {
                    int per = Convert.ToInt16(tbCompletePer.Text);
                    per = per < 0 ? 0 : per;
                    mm.Task_CompletePer = per > 100 ? per : per;
                    if (mm.Task_CompletePer >= 100)
                    {
                        mm.Task_IsComplete = true;
                        mm.Task_CompleteTime = DateTime.Now;
                    }
                }
                else
                {
                    mm.Task_CompletePer = 0;
                }
                //
                //确定操作
                if (id == 0)
                {
                    Business.Do<ITask>().Add(mm);
                }
                else
                {
                    Business.Do<ITask>().Save(mm);
                }

                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 是否完成任务的复选框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbIsComplete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox cb = (CheckBox)sender;
                divCompleteBox.Visible = cb.Checked;
                if (cb.Checked)
                {
                    tbComplete.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    tbCompletePer.Text = "100";
                }
                else
                {
                    Song.Entities.Task mm = Business.Do<ITask>().GetSingle(id);
                    tbCompletePer.Text = mm.Task_CompletePer.ToString();
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        
    }
}
