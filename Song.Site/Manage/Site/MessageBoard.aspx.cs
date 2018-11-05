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
    public partial class MessageBoard : Extend.CustomPage
    {
        Song.Entities.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                init();
                //BindData(null, null);
            }
        }

        #region 初始化
        private void init()
        {
            ddlDepart_SelectedIndexChanged(null, null);
        }
        /// <summary>
        /// 当院系的选择变动时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDepart_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSubject.Items.Clear();
            Song.Entities.Subject[] sbjs = Business.Do<ISubject>().SubjectCount(org.Org_ID, -1, "", null, 0, 0);
            ddlSubject.DataSource = sbjs;
            ddlSubject.DataTextField = "Sbj_Name";
            ddlSubject.DataValueField = "Sbj_ID";
            ddlSubject.DataBind();
            this.ddlSubject.Items.Insert(0, new ListItem(" -- 专业 -- ", "-1"));
            ddlSubject_SelectedIndexChanged(null, null);
        }
        protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCourse.Items.Clear();
            //总记录数
            bool? isUse = null;
            Song.Entities.Teacher th = Extend.LoginState.Accounts.Teacher;
            List<Song.Entities.Course> eas = null;
            int sbjid = Convert.ToInt32(ddlSubject.SelectedValue);
            eas = Business.Do<ICourse>().CourseCount(org.Org_ID, sbjid, -1, -1, null, isUse, -1);
            ddlCourse.DataSource = eas;
            ddlCourse.DataTextField = "Cou_Name";
            ddlCourse.DataValueField = "Cou_ID";
            ddlCourse.DataBind();
            if (ddlCourse.Items.Count < 1)
            {
                ddlCourse.Items.Insert(0, new ListItem(" -- 没有课程 -- ", "0"));
            }
            ddlCourse.Items.Insert(0, new ListItem(" -- 课程 -- ", "-1"));
            ddlCourse_SelectedIndexChanged(null, null);
        }

        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            //当前课程id
            BindData(null, null);
        }
        #endregion
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            try
            {
                //总记录数
                int count = 0;
                Song.Entities.MessageBoard[] eas = null;
                int couid = 0;
                int.TryParse(ddlCourse.SelectedValue, out couid);
                eas = Business.Do<IMessageBoard>().ThemePager(org.Org_ID, couid, null, null, tbSear.Text.Trim(), Pager1.Size, Pager1.Index, out count);
                foreach (Song.Entities.MessageBoard mb in eas)
                {
                    if (!string.IsNullOrWhiteSpace(mb.Mb_Content))
                    {
                        if (mb.Mb_Content.Length >= 20)
                        {
                            mb.Mb_Content = mb.Mb_Content.Substring(0, 20);
                        }
                    }
                }
                GridView1.DataSource = eas;
                GridView1.DataKeyNames = new string[] { "Mb_id" };
                GridView1.DataBind();

                Pager1.RecordAmount = count;
            }
            catch (Exception ex)
            {
                Message.Alert(ex);
            } 
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }

        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
                int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                Business.Do<IMessageBoard>().ThemeDelete(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.Alert(ex);
            } 
        }
        #region 列表中的事件
        /// <summary>
        /// 修改是否显示的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbShow_Click(object sender, EventArgs e)
        {
            try
            {
                StateButton ub = (StateButton)sender;
                int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                //
                Song.Entities.MessageBoard entity = Business.Do<IMessageBoard>().ThemeSingle(id);
                entity.Mb_IsShow = !entity.Mb_IsShow;
                Business.Do<IMessageBoard>().ThemeSave(entity);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }       
        #endregion
    }
}
