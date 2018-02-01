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
    public partial class DailyLog_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
          protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                
                fill();
            }
        }
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {
            try
            {
                //
                Song.Entities.DailyLog mm;
                if (id != 0)
                {
                    mm = Business.Do<IDailyLog>().GetSingle(id);
                    tbTime.Text = ((DateTime)mm.Dlog_WrtTime).ToString("yyyy-MM-dd");
                    //类别
                    ListItem li = rblType.Items.FindByText(mm.Dlog_Type.ToString());
                    if (li != null) li.Selected = true;
                    ltName.Text = mm.Acc_Name;
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.DailyLog();
                    tbTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    divPlan.Visible = false;
                    EmpAccount acc = Extend.LoginState.Admin.CurrentUser;
                    ltName.Text = acc.Acc_Name;
                }
                //工作记录
                tbNote.Text = mm.Dlog_Note;
                //工作计划
                tbPlan.Text = mm.Dlog_Plan;
                //原来的计划
                setPlan();
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
                Song.Entities.DailyLog mm;
                if (id != 0)
                {
                    mm = Business.Do<IDailyLog>().GetSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.DailyLog();
                }
                //工作记录
                mm.Dlog_Note = tbNote.Text;
                //工作计划
                mm.Dlog_Plan = tbPlan.Text;
                //分类
                mm.Dlog_Type = rblType.SelectedItem.Text;
                //写入时间
                mm.Dlog_WrtTime = Convert.ToDateTime(tbTime.Text.Trim());
                //所属员工
                EmpAccount acc = Extend.LoginState.Admin.CurrentUser;
                mm.Acc_Id = acc.Acc_Id;
                mm.Acc_Name = acc.Acc_Name;
                //
                //确定操作
                if (id == 0)
                {
                    Business.Do<IDailyLog>().Add(mm);
                }
                else
                {
                    Business.Do<IDailyLog>().Save(mm);
                }

                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 当更改记录类别时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
        {
            setPlan();
        }

        #region 其它方法
        void setPlan()
        {
            try
            {
                //原计划；
                EmpAccount acc = Extend.LoginState.Admin.CurrentUser;
                //取当前工作记录的上一个记录
                Song.Entities.DailyLog pre = Business.Do<IDailyLog>().GetPrevious(Convert.ToDateTime(tbTime.Text.Trim()), rblType.SelectedItem.Text, acc.Acc_Id);
                //如果不存在
                if (pre == null)
                {
                    divPlan.Visible = false;
                }
                else
                {
                    //如果上一个记录没有填写计划
                    if (pre.Dlog_Plan.Trim() == "")
                    {
                        divPlan.Visible = false;
                    }
                    else
                    {
                        divPlan.Visible = true;
                        ltPlan.Text = pre.Dlog_Plan.Replace("\n", "<br/>");
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        #endregion

        protected void tbTime_TextChanged(object sender, EventArgs e)
        {
            setPlan();
        }
    }
}
