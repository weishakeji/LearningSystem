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
using System.Text.RegularExpressions;

namespace Song.Site.Manage.Card
{
    public partial class Learningcard_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitBind();
                fill();
            }
        }
        /// <summary>
        /// 界面的初始绑定
        /// </summary>
        private void InitBind()
        {
            Lcs_SecretKey.Enabled = id < 1;
        }
        void fill()
        {            
            Song.Entities.LearningCardSet set = id == 0 ? new Song.Entities.LearningCardSet() : Business.Do<ILearningCard>().SetSingle(id);
            if (id > 0)
            {
                this.EntityBind(set);
                //当前学习卡关联的课程
                Song.Entities.Course[] courses = Business.Do<ILearningCard>().CoursesGet(set);
                if (courses != null)
                {
                    rtpCourses.DataSource = courses;
                    rtpCourses.DataBind();
                    foreach (Song.Entities.Course c in courses)
                    {
                        tbCourses.Text += c.Cou_ID + ",";
                    }
                }
            }
            if (id <1)
            {
                //如果是新增   
                Lcs_LimitStart.Text = DateTime.Now.ToString("yyyy-MM-dd");
                Lcs_LimitEnd.Text = DateTime.Now.AddMonths(12).ToString("yyyy-MM-dd");               
                Lcs_SecretKey.Text = getUID();
            }            
            
        }       
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.LearningCardSet set = id != 0 ? Business.Do<ILearningCard>().SetSingle(id) : new Song.Entities.LearningCardSet();
            set = this.EntityFill(set) as Song.Entities.LearningCardSet;
            //关联的课程
            set = Business.Do<ILearningCard>().CoursesSet(set, tbCourses.Text.Trim());
            try
            {
                if (id == 0)
                {
                    Business.Do<ILearningCard>().SetAdd(set);
                }
                else
                {
                    Business.Do<ILearningCard>().SetSave(set);
                }

                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            } 
        }
       
    }
}
