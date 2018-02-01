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

namespace Song.Site.Manage.Site
{
    public partial class Notice_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
        }
        void fill()
        {
            try
            {
                Song.Entities.Notice mm;
                if (id != 0)
                {
                    mm = Business.Do<INotice>().NoticeSingle(id);
                    tbStarTime.Text = ((DateTime)mm.No_StartTime).ToString("yyyy-MM-d HH:mm:ss");
                    cbIsShow.Checked = mm.No_IsShow;
                    tbName.Text = mm.Acc_Name;
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.Notice();
                    tbStarTime.Text = DateTime.Now.ToString("yyyy-MM-d HH:mm:ss");
                    //当前登录用户的名称
                    tbName.Text = Extend.LoginState.Admin.CurrentUser.Acc_Name;
                }
                tbTtl.Text = mm.No_Ttl;
                tbContent.Text = mm.No_Context;
                tbName.Text = mm.Acc_Name;
                //发布单位
                tbOrg.Text = mm.No_Organ;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                Song.Entities.Notice mm;
                if (id != 0)
                {
                    mm = Business.Do<INotice>().NoticeSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.Notice();
                }
                //公告标题
                mm.No_Ttl = tbTtl.Text;
                mm.No_IsShow = cbIsShow.Checked;
                mm.No_Context = tbContent.Text;
                //发布时间
                mm.No_StartTime = Convert.ToDateTime(tbStarTime.Text);
                //
                mm.No_Organ = tbOrg.Text;
                //确定操作
                if (id == 0)
                {
                    EmpAccount acc = Extend.LoginState.Admin.CurrentUser;
                    mm.Acc_Id = acc.Acc_Id;
                    mm.Acc_Name = acc.Acc_Name;
                    Business.Do<INotice>().Add(mm);
                }
                else
                {
                    Business.Do<INotice>().Save(mm);
                }
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }
    }
}
