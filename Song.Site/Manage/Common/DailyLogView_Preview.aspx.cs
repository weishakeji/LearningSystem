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
    public partial class DailyLogView_Preview : Extend.CustomPage
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
                if (id == 0) return;
                Song.Entities.DailyLog mm;
                mm = Business.Do<IDailyLog>().GetSingle(id);
                this.ltWrtTime.Text = ((DateTime)mm.Dlog_WrtTime).ToString("yyyy-MM-dd");
                //类别
                ListItem li = rblType.Items.FindByText(mm.Dlog_Type.ToString());
                if (li != null) li.Selected = true;

                //工作记录
                tbNote.Text = mm.Dlog_Note;
                //工作计划
                tbPlan.Text = mm.Dlog_Plan;
                //员工
                ltName.Text = mm.Acc_Name;
                //原来的计划
                setPlan();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }    
            
        }
        void setPlan()
        {
            try
            {
                //
                //原计划；
                //Extend.ManageSession ms = new Song.Extend.ManageSession();
                Song.Entities.DailyLog dl = Business.Do<IDailyLog>().GetSingle(id);
                //取当前工作记录的上一个记录
                Song.Entities.DailyLog pre = Business.Do<IDailyLog>().GetPrevious(Convert.ToDateTime(this.ltWrtTime.Text.Trim()), rblType.SelectedItem.Text, (int)dl.Acc_Id);
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
    }
}
