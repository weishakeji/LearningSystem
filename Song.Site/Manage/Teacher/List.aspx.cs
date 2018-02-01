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
using System.IO;

namespace Song.Site.Manage.Teacher
{
    public partial class List : Extend.CustomPage
    {
        Song.Entities.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnSear.UniqueID;
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                init();
                BindData(null, null);
            }
        }
        private void init()
        {            
            Song.Entities.TeacherSort[] sort = Business.Do<ITeacher>().SortAll(org.Org_ID, true);
            ddlSort.DataSource = sort;
            ddlSort.DataBind();

            ddlSort.Items.Insert(0, new ListItem(" -- 所有 -- ", "-1"));
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //总记录数
            int count = 0;
            int sortid = Convert.ToInt32(ddlSort.SelectedValue);
            Song.Entities.Teacher[] eas = null;
            eas = Business.Do<ITeacher>().TeacherPager(org.Org_ID, sortid, null, null, this.tbSear.Text, Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Th_id" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;
           
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }
        /// <summary>
        /// 修改是否使用的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUse_Click(object sender, EventArgs e)
        {            
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.Teacher entity = Business.Do<ITeacher>().TeacherSingle(id);
            entity.Th_IsUse = !entity.Th_IsUse;
            Business.Do<ITeacher>().TeacherSave(entity);
            BindData(null, null);          
        }
        /// <summary>
        /// 注册审核是否通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbPass_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.Teacher entity = Business.Do<ITeacher>().TeacherSingle(id);
            entity.Th_IsPass = !entity.Th_IsPass;
            Business.Do<ITeacher>().TeacherSave(entity);
            BindData(null, null);
        }
        /// <summary>
        /// 修改是否显示，flase时教师不在前端网页展示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbShow_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.Teacher entity = Business.Do<ITeacher>().TeacherSingle(id);
            entity.Th_IsShow = !entity.Th_IsShow;
            Business.Do<ITeacher>().TeacherSave(entity);
            BindData(null, null);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteEvent(object sender, EventArgs e)
        {
            string keys = GridView1.GetKeyValues;
            foreach (string id in keys.Split(','))
            {
                Business.Do<ITeacher>().TeacherDelete(Convert.ToInt32(id));
            }
            BindData(null, null);            
        }
        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, ImageClickEventArgs e)
        {            
            WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
            int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            Business.Do<ITeacher>().TeacherDelete(id);
            BindData(null, null);           
        }
        /// <summary>
        /// 导出教师信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void outputEvent(object sender, EventArgs e)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //创建文件
            string name = "教师信息导出" + "_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm") + ".xls";
            string filePath = Upload.Get["Temp"].Physics + name;
            filePath = Business.Do<ITeacher>().TeacherExport4Excel(filePath, org.Org_ID, -1);
            if (System.IO.File.Exists(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileInfo.Name));
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.AddHeader("Content-Transfer-Encoding", "binary");
                Response.ContentType = "application/-excel";
                Response.ContentEncoding = System.Text.Encoding.Default;
                Response.WriteFile(fileInfo.FullName);
                Response.Flush();
                Response.End();
                File.Delete(filePath);
            }
        }
    }
}
