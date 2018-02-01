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

namespace Song.Site.Manage.Teacher
{
    public partial class Comments : Extend.CustomPage
    {
 
        protected void Page_Load(object sender, EventArgs e)
        {
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
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.TeacherComment[] eas = null;
            eas = Business.Do<ITeacher>().CommentPager(org.Org_ID, tbSear.Text.Trim(), null, null, Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Thc_id" };
            GridView1.DataBind();
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }
        /// <summary>
        /// 修改是否使用状态，禁用后将不记分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUseClick(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.TeacherComment entity = Business.Do<ITeacher>().CommentSingle(id);
            entity.Thc_IsUse = !entity.Thc_IsUse;
            Business.Do<ITeacher>().CommentSave(entity);
            BindData(null, null);
        }
        /// <summary>
        /// 修改是否显示的状态，隐藏后不再显示，但仍然记分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbShowClick(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.TeacherComment entity = Business.Do<ITeacher>().CommentSingle(id);
            entity.Thc_IsShow = !entity.Thc_IsShow;
            Business.Do<ITeacher>().CommentSave(entity);
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
                    Business.Do<ITeacher>().CommentDelete(Convert.ToInt32(id));
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
                Business.Do<ITeacher>().CommentDelete(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
    }
}
