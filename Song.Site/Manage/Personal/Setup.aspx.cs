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

namespace Song.Site.Manage.Personal
{
    public partial class Setup : Extend.CustomPage
    {
        EmpAccount currentUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = Extend.LoginState.Admin.CurrentUser;
            if (!this.IsPostBack)
            {
                fill();
            }
        }
        private void fill()
        {
            try
            {
                Song.Entities.Subject[] sbj = Business.Do<ISubject>().SubjectCount(true, -1);
                cblSubject.DataSource = sbj;
                cblSubject.DataTextField = "sbj_Name";
                cblSubject.DataValueField = "sbj_id";
                cblSubject.DataBind();
                //
                EmpAccount ea = this.currentUser;
                string sel = Business.Do<ISystemPara>()["SubjectForAccout_" + ea.Acc_Id].String;
                if(!string.IsNullOrEmpty(sel)){
                    foreach (string s in sel.Split(','))
                    {
                        if (s == "") continue;
                        ListItem li = cblSubject.Items.FindByValue(s);
                        if (li != null) li.Selected = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// ÐÞ¸Ä
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentUser == null) return;
                EmpAccount ea = currentUser;
                string sel = "";
                foreach (ListItem li in cblSubject.Items)
                {
                    if (li.Selected)
                    {
                        sel += li.Value + ",";
                    }
                }
                Business.Do<ISystemPara>().Save("SubjectForAccout_" + ea.Acc_Id, sel);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
