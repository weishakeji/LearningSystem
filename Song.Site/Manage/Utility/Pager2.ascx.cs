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
using System.IO;
using System.Text.RegularExpressions;

namespace Song.Site.Manage.Utility
{
    public partial class Pager2 : System.Web.UI.UserControl
    {
        #region 属性

        /// <summary>
        /// 每页显示多少条
        /// </summary>
        public int Size
        {
            get
            {
                object obj = ViewState["Size"];
                return (obj == null) ? 0 : (int)obj;
            }
            set
            {
                ViewState["Size"] = value;
                //设置界面效果
                faceSetup();
            }
        }
        /// <summary>
        /// 显示几个分页码
        /// </summary>
        public int Display 
        {
            get
            {
                object obj = ViewState["Display"];
                return (obj == null) ? 3 : (int)obj;
            }
            set
            {
                ViewState["Display"] = value;
            }
        }
        /// <summary>
        /// 当前页索引
        /// </summary>
        public int Index
        {
            get
            {
                int pindex = WeiSha.Common.Request.QueryString["index"].Int32 ?? 1;
                if (pindex >= PageAmount && PageAmount > 0) return PageAmount;
                if (pindex < 1) return 1;
                return pindex;
            }
            set
            {
                string query = this.Qurey;
                string fileName = Path.GetFileName(this.Request.FilePath);
                if (string.IsNullOrWhiteSpace(query))
                {
                    fileName += "?index=" + 1;
                }
                else
                {
                    fileName += "?" + getQuery("index", 1);
                }
                Response.Redirect(fileName);
            }
        }
        private int _recordAmount;
        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordAmount
        {
            get
            {
                //return WeiSha.Common.Request.QueryString["Recordcount"].Int32 ?? 1;
                return _recordAmount;
            }
            set
            {
                _recordAmount = value;
            }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageAmount
        {
            get { return (int)Math.Ceiling((double)RecordAmount / (double)Size); }
        }

        private string _query;
        /// <summary>
        /// 查询（Get方式）字符串
        /// </summary>
        public string Qurey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_query))
                    return this.Page.ClientQueryString;
                else
                    return _query;
            }
            set
            {
                _query = value;
            }
        }
        #endregion

        #region 导航
        public string First
        {
            get
            {
                int index = this.Index;
                int display = this.Display;
                string url = "";
                if (index - 1 <= display) return url;
                string fileName = Path.GetFileName(this.Request.FilePath);
                url = fileName + "?" + getQuery("index", 1);
                url = "<a href=\"" + url + "\">|&lt;</a>";
                return url;
            }
        }
        public string Prev
        {
            get
            {
                int index = this.Index;
                int display = this.Display;
                //
                string url = "";
                if (index - 1 <= display) return url;
                string fileName = Path.GetFileName(this.Request.FilePath);
                if (index - 1 <= 0) url = fileName + "?" + getQuery("index", index); 
                if (index - 1 > 0) url = fileName + "?" + getQuery("index", index - 1 - display * 2);                
                url = "<a href=\"" + url + "\">...</a>";
                return url;
            }
        }
        public string Next
        {
            get
            {
                int index = this.Index;
                int amount = this.PageAmount;
                int display = this.Display;
                //
                string url = "";
                if (index >= amount - display) return url;
                string fileName = Path.GetFileName(this.Request.FilePath);
                if (index + 1 < amount - display * 2)
                    url = fileName + "?" + getQuery("index", index + 1 + this.Display * 2);
                else
                    url = fileName + "?" + getQuery("index", amount);
                return url = "<a href=\"" + url + "\">...</a>";
            }
        }
        public string Last
        {
            get
            {
                int index = this.Index;
                int amount = this.PageAmount;
                int display = this.Display;
                string url = "";
                if (index >= amount || index >= amount - display) return url;
                string fileName = Path.GetFileName(this.Request.FilePath);
                url = fileName + "?" + getQuery("index", amount);
                return url = "<a href=\"" + url + "\">&gt;|</a>";
            }
        }
        /// <summary>
        /// 用于拼接页面的查询字串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string getQuery(string key, int value)
        {
            string query = this.Qurey;
            if (string.IsNullOrWhiteSpace(query)) return key + "=" + value;
            //
            Regex regex = new Regex(@"(?<key>" + key + @")=(?<value>(|.)[^&]*)", RegexOptions.IgnoreCase);
            if (regex.Match(query).Success)
                query = regex.Replace(query, "$2=" + value);
            else
                query += "&" + key + "=" + value;
            return query;
        }
        /// <summary>
        /// 生成数字导航
        /// </summary>
        /// <param name="index">当前面索引</param>
        /// <param name="tag">html标识</param>
        /// <returns></returns>
        public string NumberNav(int index, string tag)
        {
            string nav = "";
            string fileName = Path.GetFileName(this.Request.FilePath);
            int amount = this.PageAmount;
            int display = this.Display;
            //添加新项
            for (int i = index - display; i <= index + display; i++)
            {
                if (i > 0 && i <= amount)
                {

                    if (i == index)
                    {
                        nav += "<" + tag + " class=\"curr\">" + i + "</" + tag + ">";
                    }
                    else
                    {
                        nav += "<a href=\"" + fileName + "?" + getQuery("index", i) + "\">" + i + "</a>";
                    }
                   
                }
            }
            return nav;
        }
        #endregion       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                faceSetup();
                tbGoPagenum.Attributes.Add("numlimit", "1-" + this.PageAmount);
            }
        }
        /// <summary>
        /// 设置界面效果
        /// </summary>
        private void faceSetup()
        {
            string name = WeiSha.Common.Request.Page.Name;
        }
        /// <summary>
        /// 跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGoPagenum_Click(object sender, EventArgs e)
        {
            int goIndex = 1;
            int.TryParse(tbGoPagenum.Text, out goIndex);
            string fileName = Path.GetFileName(this.Request.FilePath);
            string url = fileName + "?" + getQuery("index", goIndex);
            this.Response.Redirect(url);
        }
  
    }
}