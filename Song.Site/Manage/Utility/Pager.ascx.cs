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

namespace Song.Site.Manage.Utility
{
    public partial class Pager : System.Web.UI.UserControl
    {
        #region 属性

        /// <summary>
        /// 每页显示多少条
        /// </summary>
        public int Size
        {
            get
            {
                //object obj = ViewState["Size"];
                //return (obj == null) ? 15 : (int)obj;  
                //如果是第一次读取
                if (!this.Page.IsPostBack)
                {
                    string name = WeiSha.Common.Request.Page.Name;
                    if (this.Page.Request.Cookies[name + "_pagerSize"] == null)
                    {
                        return Convert.ToInt32(tbSize.Text);
                    }
                    else
                    {
                        string size = this.Page.Request.Cookies[name + "_pagerSize"].Value;
                        tbSize.Text = size;
                        return Convert.ToInt32(tbSize.Text);
                    }
                }
                else
                {
                    return Convert.ToInt32(tbSize.Text);
                }
            }
            set
            {
                //ViewState["Size"] = value;
                tbSize.Text = value.ToString();
                string name = WeiSha.Common.Request.Page.Name;
                this.Page.Response.Cookies[name + "_pagerSize"].Value = tbSize.Text;
                //设置界面效果
                //faceSetup();
            }
        }
        /// <summary>
        /// 当前页索引
        /// </summary>
        public int Index
        {
            get
            {
                object cpage = ViewState["Index"];
                int pindex = (cpage == null) ? 1 : (int)cpage;
                if (pindex > PageAmount && PageAmount > 0)
                {
                    return PageAmount;
                }
                else if (pindex < 1)
                {
                    return 1;
                }
                return pindex;
            }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                else
                {
                    if (value > PageAmount)
                    {
                        value = PageAmount;
                    }
                }
                ViewState["Index"] = value;
                //设置界面效果
                faceSetup();
            }
        }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordAmount
        {
            get
            {
                object obj = ViewState["Recordcount"];
                return (obj == null) ? 0 : (int)obj;
            }
            set
            {
                ViewState["Recordcount"] = value;
                //设置界面效果
                faceSetup();
            }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageAmount
        {
            get { return (int)Math.Ceiling((double)RecordAmount / (double)Size); }
        }

        #endregion

        #region 事件
        public event EventHandler PageChanged;
        /// <summary>
        /// 转到第一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbnFirst_Click(object sender, EventArgs e)
        {
            Index = 1;
            //this.Size = Convert.ToInt32(tbSize.Text);
            PageChanged(sender, e);
            //设置界面效果
            faceSetup();
        }
        /// <summary>
        /// 转到前一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbnPrevious_Click(object sender, EventArgs e)
        {
            Index--;
            //this.Size = Convert.ToInt32(tbSize.Text);
            PageChanged(sender, e);
            //设置界面效果
            faceSetup();
        }
        /// <summary>
        /// 转到下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbnNext_Click(object sender, EventArgs e)
        {
            Index++;
            //this.Size = Convert.ToInt32(tbSize.Text);
            PageChanged(sender, e);
            //设置界面效果
            faceSetup();
        }
        /// <summary>
        /// 转到最后一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbnLast_Click(object sender, EventArgs e)
        {
            Index = PageAmount;
            //this.Size = Convert.ToInt32(tbSize.Text);
            PageChanged(sender, e);
            //设置界面效果
            faceSetup();
        }
        /// <summary>
        /// 跳转到任一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbnGoto_Click(object sender, EventArgs e)
        {
            //string strIndex = txtGoto.Text.Trim();
            //if (strIndex.Length == 0) { return; }

            //try
            //{
            //    PageIndex = Convert.ToInt32(strIndex);
            //}
            //catch
            //{
            //    return;
            //}

            //PageChanged(sender, e);
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tbSize.Text = this.Size.ToString();
                faceSetup();
            }
        }
        /// <summary>
        /// 设置界面效果
        /// </summary>
        private void faceSetup()
        {
            if (Index == 1)
            {
                //如果是第一页
                this.lbnFirst.Enabled = false;
                this.lbnPrevious.Enabled = false;
            }
            else
            {
                this.lbnFirst.Enabled = true;
                this.lbnPrevious.Enabled = true;
            }
            if (Index == PageAmount || PageAmount == 0)
            {
                //如果是最后一页
                this.lbnNext.Enabled = false;
                this.lbnLast.Enabled = false;
            }
            else
            {
                this.lbnNext.Enabled = true;
                this.lbnLast.Enabled = true;
            }
            if (this.RecordAmount < 1)
            {
                //this.Visible = false;
            }
            string name = WeiSha.Common.Request.Page.Name;
            this.Page.Response.Cookies[name + "_pagerSize"].Value = tbSize.Text;
            ddlGoPageBind();
        }
        /// <summary>
        /// 绑定下拉框
        /// </summary>
        private void ddlGoPageBind()
        {
            ddlGoPage.Items.Clear();
            //添加新项
            for (int i = this.Index - 10; i < this.Index + 10; i++)
            {
                if (i > 0 && i <= PageAmount)
                {
                    ddlGoPage.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
            }
            if (PageAmount != 0)
            {
                //定位到当前页索引
                ddlGoPage.Items.FindByValue(this.Index.ToString()).Selected = true;
            }
        }
        /// <summary>
        /// 下接框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlGoPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            int value = Convert.ToInt32(ddlGoPage.SelectedItem.Value);
            Index = value;
            //设置界面效果
            faceSetup();
            PageChanged(sender, e);

        }
    }
}