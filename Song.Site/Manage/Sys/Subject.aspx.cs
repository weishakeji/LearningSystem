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

namespace Song.Site.Manage.Sys
{
    public partial class Subject : Extend.CustomPage
    {
        Song.Entities.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnSear.UniqueID;
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
            ////总记录数
            //int count = 0;
            //bool? isUse = null;
            Song.Entities.Subject[] eas = null;
            //eas = Business.Do<ISubject>().SubjectPager(org.Org_ID, depid, isUse, this.tbSear.Text, Pager1.Size, Pager1.Index, out count);
            eas = Business.Do<ISubject>().SubjectCount(org.Org_ID, tbSear.Text.Trim(), null, -1, 0);
            foreach (Song.Entities.Subject s in eas)
            {
                if (string.IsNullOrEmpty(s.Sbj_Intro) || s.Sbj_Intro.Trim() == "") continue;
                if (s.Sbj_Intro.Length > 20)
                {
                    s.Sbj_Intro = s.Sbj_Intro.Substring(0, 20) + "...";
                }
            }
            DataTable dt = WeiSha.WebControl.Tree.ObjectArrayToDataTable.To(eas);
            if (string.IsNullOrWhiteSpace(tbSear.Text.Trim()))
            {
                WeiSha.WebControl.Tree.DataTableTree tree = new WeiSha.WebControl.Tree.DataTableTree();
                tree.IdKeyName = "Sbj_ID";
                tree.ParentIdKeyName = "Sbj_PID";
                tree.TaxKeyName = "Sbj_Tax";
                tree.Root = 0;
                dt = tree.BuilderTree(dt);
            }
            GridView1.DataSource = dt;
            GridView1.DataKeyNames = new string[] { "Sbj_ID" };
            GridView1.DataBind();

            //Pager1.RecordAmount = count;
        }
        ///// <summary>
        ///// 获取当前专业的试题数量
        ///// </summary>
        ///// <param name="sbjid"></param>
        ///// <returns></returns>
        //protected string GetQuesCount(object sbjid)
        //{
        //    int sbj;
        //    int.TryParse(sbjid.ToString(), out sbj);
        //    int count = Business.Do<ISubject>().QusCountForSubject(org.Org_ID, sbj, -1, null);
        //    return count.ToString();
        //}
        ///// <summary>
        ///// 获取当前专业试题库的数量详情，即每类型各多少道
        ///// </summary>
        ///// <param name="sbjid"></param>
        ///// <returns></returns>
        //protected string GetQuesCountDetails(object sbjid)
        //{
        //    string tm = "";
        //    int sbj = 0;
        //    if (sbjid is System.Int32)
        //    {
        //        sbj = Convert.ToInt32(sbjid);
        //    }
        //    //试题类型
        //    string[] typs = App.Get["QuesType"].Split(',');
        //    for (int i = 0; i < typs.Length; i++)
        //    {
        //        tm += typs[i] + Business.Do<ISubject>().QusCountForSubject(org.Org_ID, sbj, i + 1, null);
        //        tm += "道&nbsp;";
        //    }
        //    return tm;
        //}
        ///// <summary>
        ///// 获取当前专业下的课程数据
        ///// </summary>
        ///// <param name="sbjid"></param>
        ///// <returns></returns>
        //protected string GetCourseCount(object sbjid)
        //{
        //    int sbj;
        //    int.TryParse(sbjid.ToString(), out sbj);
        //    int count = Business.Do<ICourse>().CourseOfCount(org.Org_ID, sbj, -1);
        //    return count.ToString();
        //}
        protected void btnsear_Click(object sender, EventArgs e)
        {
            //Pager1.Index = 1;
            BindData(null, null);
        }
        /// <summary>
        /// 修改是否使用的状态
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
                Song.Entities.Subject entity = Business.Do<ISubject>().SubjectSingle(id);
                entity.Sbj_IsUse = !entity.Sbj_IsUse;
                Business.Do<ISubject>().SubjectSave(entity);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.Alert(ex);
            }
        }
        /// <summary>
        /// 修改是否推荐的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbRec_Click(object sender, EventArgs e)
        {
            try
            {
                StateButton ub = (StateButton)sender;
                int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                //
                Song.Entities.Subject entity = Business.Do<ISubject>().SubjectSingle(id);
                entity.Sbj_IsRec = !entity.Sbj_IsRec;
                Business.Do<ISubject>().SubjectSave(entity);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.Alert(ex);
            }
        }
        /// <summary>
        /// 清空当前专业的试题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbClear_Click(object sender, EventArgs e)
        {            
            LinkButton lb = (LinkButton)sender;
            int id = Convert.ToInt32(lb.CommandArgument);
            Business.Do<ISubject>().SubjectClear(id);
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
                    Business.Do<ISubject>().SubjectDelete(Convert.ToInt32(id));
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
                Business.Do<ISubject>().SubjectDelete(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
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
            if (Business.Do<ISubject>().RemoveUp(id))
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
            if (Business.Do<ISubject>().RemoveDown(id))
            {
                BindData(null, null);
            }                
        }
    }
}
