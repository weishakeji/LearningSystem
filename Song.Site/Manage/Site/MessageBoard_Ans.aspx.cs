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
using System.Collections.Generic;

namespace Song.Site.Manage.Site
{
    public partial class MessageBoard_Ans : Extend.CustomPage
    {
        //要操作的对象主键
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //当前学生id
        protected int stid = 0;
        //是否为超管
        protected bool isAdmin = false;
        protected Song.Entities.MessageBoard mb;
        protected void Page_Load(object sender, EventArgs e)
        {
            mb = Business.Do<IMessageBoard>().ThemeSingle(id);
            if (!this.IsPostBack)
            {
                fill();               
            }
        }
        private void fill()
        {
            Song.Entities.Teacher th = Extend.LoginState.Accounts.Teacher;
            lbCurrName.Text = th.Th_Name;
            imbCurrPhoto.ImageUrl = Upload.Get["Teacher"].Virtual + th.Th_Photo;
            //回复信息
            tbAns.Text = mb.Mb_Answer;
            cbIshow.Checked = mb.Mb_IsShow;

        }


        protected void btnEnter_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                mb.Mb_AnsTime = DateTime.Now;
                mb.Mb_Answer = tbAns.Text.Trim();
                mb.Mb_IsShow = cbIshow.Checked;
                mb.Mb_IsAns = true;
                //当前教师
                Song.Entities.Teacher teacher = Extend.LoginState.Accounts.Teacher;
                if (teacher != null)
                {
                    mb.Th_ID = teacher.Th_ID;
                }
                Business.Do<IMessageBoard>().ThemeSave(mb);
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }


    }
}
