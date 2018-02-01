using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using WeiSha.WebControl;
using Song.ServiceInterfaces;
using Song.Entities;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;

namespace Song.Site.Manage.Course
{
    public partial class Courses_TestPaper : Extend.CustomPage
    {
        //课程id
        private int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        Song.Entities.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            this.Title = "试卷管理";
            if (!this.IsPostBack)
            {
                BindData(null, null);
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //总记录数
            int count = 0;
            int sbj = 0;
            //Song.Entities.Course cour = Business.Do<ICourse>().CourseSingle(couid);
            //if (cour != null) sbj = cour.Sbj_ID;           
            string sear = tbSear.Text.Trim();           
            //
            Song.Entities.TestPaper[] eas = null;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            eas = Business.Do<ITestPaper>().PaperPager(org.Org_ID, sbj, couid, -1, null, sear, Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Tp_Id" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;

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
                foreach (string id in keys.Split(','))
                {
                    Business.Do<ITestPaper>().PagerDelete(Convert.ToInt16(id));
                }
                BindData(null, null);
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
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
                Business.Do<ITestPaper>().PagerDelete(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
        /// <summary>
        /// 修改是否显示的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUse_Click(object sender, EventArgs e)
        {
            try
            {
                StateButton ub = (StateButton)sender;
                int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                //
                Song.Entities.TestPaper entity = Business.Do<ITestPaper>().PagerSingle(id);
                entity.Tp_IsUse = !entity.Tp_IsUse;
                Business.Do<ITestPaper>().PagerSave(entity);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Alert(ex.Message);
            }
        }
    }
}