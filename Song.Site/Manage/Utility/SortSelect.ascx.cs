using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using System.Text.RegularExpressions;

namespace Song.Site.Manage.Utility
{
    public partial class SortSelect : System.Web.UI.UserControl
    {
        #region 属性
        /// <summary>
        /// 专业ID
        /// </summary>
        public int SbjID
        {
            get
            {
                int sbjid;
                int.TryParse(ddlSubject.SelectedValue, out sbjid);
                return sbjid;
            }
            set
            {
                ListItem li = ddlSubject.Items.FindByValue(value.ToString());
                if (li != null)
                {
                    ddlSubject.SelectedIndex = -1;
                    li.Selected = true;
                    ddlSubject_SelectedIndexChanged(null, null);
                }
            }
        }
        public string SbjName
        {
            get { return ddlSubject.SelectedText; }
        }
        /// <summary>
        /// 课程ID
        /// </summary>
        public int CouID
        {
            get
            {
                int couid;
                int.TryParse(ddlCourse.SelectedValue, out couid);
                return couid;
            }
            set
            {
                ListItem li = ddlCourse.Items.FindByValue(value.ToString());
                if (li != null)
                {
                    ddlCourse.SelectedIndex = -1;
                    li.Selected = true;
                    ddlCourse_SelectedIndexChanged(null, null);                    
                }
            }
        }
        public string CouName
        {
            get { return ddlCourse.SelectedText; }
        }
        /// <summary>
        /// 章节ID
        /// </summary>
        public int OlID
        {
            get
            {
                int olid;
                int.TryParse(ddlOutline.SelectedValue, out olid);
                return olid;
            }
            set
            {
                ListItem li = ddlOutline.Items.FindByValue(value.ToString());
                if (li != null)
                {
                    ddlOutline.SelectedIndex = -1;
                    li.Selected = true;
                }
            }
        }
        public string OlName
        {
            get { return ddlOutline.SelectedText; }
        }
        #endregion

        #region 事件

        #endregion

        Song.Entities.Organization org = null;
        //当课程变更时
        public event EventHandler CourseChange;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// 界面的初始绑定
        /// </summary>
        public void InitBind()
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            //专业
            Song.Entities.Subject[] subjects = Business.Do<ISubject>().SubjectCount(org.Org_ID, null, true, -1, -1);
            ddlSubject.DataSource = subjects;
            this.ddlSubject.DataTextField = "Sbj_Name";
            this.ddlSubject.DataValueField = "Sbj_ID";
            this.ddlSubject.Root = 0;
            this.ddlSubject.DataBind();
            ddlSubject.Items.Insert(0, new ListItem("-专业-", "0"));
            ddlSubject_SelectedIndexChanged(null, null);
        }
        /// <summary>
        /// 专业选择变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCourse.Items.Clear();
            int sbjid;
            int.TryParse(ddlSubject.SelectedValue, out sbjid);
            if (sbjid > 0)
            {
                org = Business.Do<IOrganization>().OrganCurrent();
                //上级
                List<Song.Entities.Course> cous = Business.Do<ICourse>().CourseCount(org.Org_ID, sbjid, null, null, -1);
                ddlCourse.DataSource = cous;
                this.ddlCourse.DataTextField = "Cou_Name";
                this.ddlCourse.DataValueField = "Cou_ID";
                this.ddlCourse.Root = 0;
                this.ddlCourse.DataBind();
            }
            ddlCourse.Items.Insert(0, new ListItem("-课程-", "0"));
            ddlCourse_SelectedIndexChanged(null, null);
        }
        /// <summary>
        /// 课程选择项变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlOutline.Items.Clear();
            //课程id
            int cou;
            int.TryParse(ddlCourse.SelectedValue, out cou);
            if (cou > 0)
            {
                org = Business.Do<IOrganization>().OrganCurrent();
                Song.Entities.Outline[] outline = Business.Do<IOutline>().OutlineAll(cou, null);
                ddlOutline.DataSource = outline;
                this.ddlOutline.DataTextField = "Ol_Name";
                this.ddlOutline.DataValueField = "Ol_ID";
                ddlOutline.DataBind();
                
            }
            this.ddlOutline.Items.Insert(0, new ListItem("-章节-", "0"));
            //执行事件
            if (this.CourseChange != null)
            {
                this.CourseChange(null, null);
            }
        }
    }
}