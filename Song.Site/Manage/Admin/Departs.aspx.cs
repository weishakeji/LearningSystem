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

namespace Song.Site.Manage.Admin
{
    public partial class Departs : Extend.CustomPage
    {
        Song.Entities.Organization org = null;       
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
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
            Song.Entities.Depart[] eas = null;
            eas = Business.Do<IDepart>().GetAll(org.Org_ID, null,null);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Dep_ID" };
            GridView1.DataBind();
            
        }
        /// <summary>
        /// 获取当前专业下的课程数据
        /// </summary>
        /// <param name="sbjid"></param>
        /// <returns></returns>
        protected string GetCourseCount(object sbjid)
        {
            int sbj;
            int.TryParse(sbjid.ToString(), out sbj);
            int count = Business.Do<ICourse>().CourseOfCount(org.Org_ID, sbj, -1);
            return count.ToString();
        }
        
        /// <summary>
        /// 修改是否展示的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbShow_Click(object sender, EventArgs e)
        {            
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.Depart entity = Business.Do<IDepart>().GetSingle(id);
            entity.Dep_IsShow = !entity.Dep_IsShow;
            Business.Do<IDepart>().Save(entity);
            BindData(null, null);           
        }
        protected void sbUse_Click(object sender, EventArgs e)
        {            
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.Depart entity = Business.Do<IDepart>().GetSingle(id);
            entity.Dep_IsUse = !entity.Dep_IsUse;
            Business.Do<IDepart>().Save(entity);
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
                Business.Do<IDepart>().Delete(Convert.ToInt32(id));
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
            Business.Do<IDepart>().Delete(id);
            BindData(null, null);

        }
        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUp_Click(object sender, EventArgs e)
        {            
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IDepart>().RemoveUp(id))
            {
                BindData(null, null);
            }
        }
        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDown_Click(object sender, EventArgs e)
        {            
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IDepart>().RemoveDown(id))
            {
                BindData(null, null);
            }
        }
    }
}
