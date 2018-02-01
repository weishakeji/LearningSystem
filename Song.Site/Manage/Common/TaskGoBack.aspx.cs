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
    public partial class TaskGoBack : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //设置查询的起止时间
                //tbStart.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                //tbEnd.Text = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd");
                BindData(null, null);
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            try
            {
                //总记录数
                int count = 0;
                //等级
                int level = Convert.ToInt16(ddlLevel.SelectedItem.Value);
                //查询字符
                string sear = tbSear.Text.Trim();
                DateTime startTime = DateTime.Now.AddYears(-100);
                //开始时间与结束时间
                if (tbStart.Text.Trim() != "")
                {
                    startTime = Convert.ToDateTime(tbStart.Text.Trim());
                }
                DateTime endTime = DateTime.Now.AddYears(100);
                if (this.tbEnd.Text.Trim() != "")
                {
                    endTime = Convert.ToDateTime(tbEnd.Text.Trim());
                }
                endTime = endTime.AddDays(1).AddSeconds(-1);
                //当前登录用户id
                EmpAccount acc = Extend.LoginState.Admin.CurrentUser;
                Song.Entities.Task[] task = null;
                task = Business.Do<ITask>().GetMyPager(acc.Acc_Id, true, startTime, endTime, "-1", level, sear, Pager1.Size, Pager1.Index, out count);

                GridView1.DataSource = task;
                GridView1.DataKeyNames = new string[] { "Task_Id" };
                GridView1.DataBind();

                Pager1.RecordAmount = count;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
            
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteEvent(object sender, EventArgs e)
        {
            try
            {
                string keys = GridView1.GetKeyValues;
                foreach (string mid in keys.Split(','))
                {
                    Business.Do<ITask>().Delete(Convert.ToInt16(mid));
                }
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 获取任务的剩余时间
        /// </summary>
        /// <param name="endTime"></param>
        /// <returns></returns>
        protected double getRemainingTime(string endTime)
        {
            try
            {
                DateTime end = Convert.ToDateTime(endTime);
                TimeSpan span = end - System.DateTime.Now;
                return Math.Round(span.TotalHours * 100) / 100;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
                return 0;
            }
        }
    }
}
