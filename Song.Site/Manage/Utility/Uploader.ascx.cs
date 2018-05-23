using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Song.Site.Manage.Utility
{
    public partial class Uploader : System.Web.UI.UserControl
    {
        //上传控件的资源文件夹
        protected string uploaderPath = "/Manage/Utility/UploadPath/";
        /// <summary>
        /// 上传文件夹
        /// </summary>
        public string UploadPath
        {
            get
            {
                object obj = ViewState["UploadPath"];
                return (obj == null) ? "" : (string)obj;
            }
            set { ViewState["UploadPath"] = value; }
        }
        /// <summary>
        /// 全局唯一值
        /// </summary>
        public string UID
        {
            get
            {
                object obj = ViewState["UID"];
                return (obj == null) ? "" : (string)obj;
            }
            set { ViewState["UID"] = value; }
        }
        /// <summary>
        /// 限制的上传个数
        /// </summary>
        public int LimitCount
        {
            get
            {
                object obj = ViewState["LimitCount"];
                return (obj == null) ? 0 : Convert.ToInt32(obj);
            }
            set { ViewState["LimitCount"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //uploaderPath = WeiSha.Common.Server.DomainPath + uploaderPath;
            Page.Header.Controls.Add(new System.Web.UI.LiteralControl("<link href=\"" + uploaderPath + "css.css\" type=\"text/css\" rel=\"stylesheet\" />\r\n"));
        }
    }
}