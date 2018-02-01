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
    public partial class TaskWorker_Edit : Extend.CustomPage
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
                if (id < 1) return;
                Song.Entities.Task mm;
                mm = Business.Do<ITask>().GetSingle(id);
                //任务名称
                lbName.Text = mm.Task_Name;
                //优先级
                lbLevel.Text = mm.Task_Level.ToString();
                //指派人信息
                lbWorkerName.Text = mm.Task_WorkerName;
                Song.Entities.Depart dep = Business.Do<IEmployee>().Get4Depart((int)mm.Task_WorkerId);
                if (dep != null)
                {
                    lbDepart.Text = dep.Dep_CnName;
                }
                //任务内容
                lbContext.Text = mm.Task_Context.Replace("\n", "<br/>");
                //开始时间与计划结束时间
                lbStart.Text = ((DateTime)mm.Task_StartTime).ToString("yyyy-MM-dd");
                lbEnd.Text = ((DateTime)mm.Task_EndTime).ToString("yyyy-MM-dd");
                //工作记录
                tbWorkLog.Text = mm.Task_WorkLog;
                //退回的原因
                tbGoBackText.Text = mm.Task_GobackText;
                //完成度
                tbCompletePer.Text = mm.Task_CompletePer.ToString();
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
                if (id < 1) return;
                Song.Entities.Task mm = Business.Do<ITask>().GetSingle(id);
                //工作记录
                mm.Task_WorkLog = tbWorkLog.Text;
                //完成度
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
                Business.Do<ITask>().Save(mm);
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 进入退回任务状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            plGoback.Visible = true;
            plLog.Enabled = false;
            //按钮
            btnGoBack.Visible = false;
            btnGoBackEnter.Visible = true;
            btnGoBackEnterCancel.Visible = true;
            btnEnter.Visible = false;
        }
        /// <summary>
        /// 取消退回任务的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGoBackEnterCancel_Click(object sender, EventArgs e)
        {
            plGoback.Visible = false;
            plLog.Enabled = true;
            //按钮
            btnGoBack.Visible = true;
            btnGoBackEnter.Visible = false;
            btnGoBackEnterCancel.Visible = false;
            btnEnter.Visible = true;
        }
        /// <summary>
        /// 确定退回任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGoBackEnter_Click(object sender, EventArgs e)
        {
            try
            {
                if (id < 1) return;
                Song.Entities.Task mm = Business.Do<ITask>().GetSingle(id);
                //退回
                mm.Task_IsGoback = true;
                //退回的原因
                mm.Task_GobackText = tbGoBackText.Text;
                Business.Do<ITask>().Save(mm);
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        
    }
}
