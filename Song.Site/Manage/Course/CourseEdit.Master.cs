using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Song.Site.Manage.Course
{
    public partial class CourseEdit : System.Web.UI.MasterPage
    {       
        //课程id
        private int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        /// <summary>
        /// 课程id
        /// </summary>
        public int Couid
        {
            get { return couid; }
            set { couid = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {            
            this.Page.Form.DefaultButton = this.btnEnter.UniqueID;
            //_setLeftMenu();
            //上一步、下一步按钮的显示与否
            btnPrev.Visible = this.PrevHref() != null;
            btnNext.Visible = this.NextHref() != null;
            //绑定数据
            rptLeftMenu.DataSource = _leftMenu;
            rptLeftMenu.DataBind();
            //如果是新增，则只显示第一个按钮
            if (this.Couid == 0)
            {
                for (int i = 0; i < rptLeftMenu.Items.Count; i++)
                {
                    HyperLink link=(HyperLink)rptLeftMenu.Items[i].FindControl("link");
                    if (i != 0)
                    {
                        link.NavigateUrl = "";
                        link.ToolTip = "点击下一步，新建课程后，才能编辑相关内容";
                    }
                }
            }
        }
        #region 事件
        public event ImageClickEventHandler Prev_Click;
        public event ImageClickEventHandler Next_Click;
        public event ImageClickEventHandler Enter_Click;
        /// <summary>
        /// 上一步的按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrev_Click(object sender, ImageClickEventArgs e)
        {
            if (Prev_Click != null)
            {
                this.Prev_Click(sender, e);                
            }
            string prevHref = this.PrevHref();
            this.Page.Response.Redirect(prevHref);
        }
        /// <summary>
        /// 下一步的按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNext_Click(object sender, ImageClickEventArgs e)
        {
            if (Next_Click != null)
            {
                this.Next_Click(sender, e);                
            }
            if (couid > 0)
            {
                string nextHref = this.NextHref();
                this.Page.Response.Redirect(nextHref);
            }
        }
        /// <summary>
        /// 确定按钮的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, ImageClickEventArgs e)
        {
            if (Enter_Click != null)
            {
                this.Enter_Click(sender, e);
            }
        }
        #endregion

        #region 选项卡管理
        private DataTable _leftMenu = null;
        public DataTable LeftMenu
        {
            get { return _leftMenu; }
        }
        private void _setLeftMenu()
        {
            string xml = "LeftMenu.xml";
            string xmlPath = this.Page.Request.Url.AbsolutePath;
            if (xmlPath.IndexOf("/") > -1)
            {
                xmlPath = xmlPath.Substring(0, xmlPath.LastIndexOf("/")+1);
            }
            xmlPath = WeiSha.Common.Server.MapPath(xmlPath + xml);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);
            XmlNodeList xnl = xmlDoc.GetElementsByTagName("item");
            DataTable dt = new DataTable("LeftMenu");
            //链接名称
            dt.Columns.Add(new DataColumn("name", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("href", Type.GetType("System.String")));
            foreach (XmlNode n in xnl)
            {
                DataRow dr = dt.NewRow();
                dr["name"] = n.Attributes["name"].Value;
                dr["href"] = n.Attributes["href"].Value;
                dt.Rows.Add(dr);
            }
            _leftMenu = dt;
        }
        /// <summary>
        /// 当前菜单项
        /// </summary>
        public string CurrName
        {
            get
            {
                if (_leftMenu == null) _setLeftMenu();
                string name = string.Empty;
                string filename = WeiSha.Common.Request.Page.FileName;
                foreach (DataRow dr in _leftMenu.Rows)
                {
                    if (filename.ToLower() == dr["href"].ToString().ToLower())
                    {
                        name = dr["name"].ToString();
                        break;
                    }
                }
                return name;
            }
        }
        /// <summary>
        /// 上一个链接
        /// </summary>
        /// <returns></returns>
        public string PrevHref()
        {
            if (_leftMenu == null) _setLeftMenu();
            string name = string.Empty;
            string filename = WeiSha.Common.Request.Page.FileName;
            int index = 0;
            for (int i = 0; i < _leftMenu.Rows.Count;i++ )
            {
                DataRow dr = _leftMenu.Rows[i];
                if (filename.ToLower() == dr["href"].ToString().ToLower())
                {
                    index = i;
                    break;
                }
            }
            if (index == 0) return null;
            string href = _leftMenu.Rows[index - 1]["href"].ToString();
            return href += "?couid=" + couid;
        }
        /// <summary>
        /// 下一个链接
        /// </summary>
        /// <returns></returns>
        public string NextHref()
        {
            if (_leftMenu == null) _setLeftMenu();
            string name = string.Empty;
            string filename = WeiSha.Common.Request.Page.FileName;
            int index = 0;
            for (int i = 0; i < _leftMenu.Rows.Count; i++)
            {
                DataRow dr = _leftMenu.Rows[i];
                if (filename.ToLower() == dr["href"].ToString().ToLower())
                {
                    index = i;
                    break;
                }
            }
            if (index >= _leftMenu.Rows.Count - 1) return null;
            string href = _leftMenu.Rows[index + 1]["href"].ToString();
            return href += "?couid=" + couid;
        }
        #endregion
    }
}