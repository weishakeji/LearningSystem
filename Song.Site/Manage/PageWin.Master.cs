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

namespace Song.Site.Manage
{
    public partial class PageWin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Buffer = true;
            Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
            Response.Expires = 0;
            Response.CacheControl = "no-cache";
            Page.Response.Cache.SetNoStore();            
        }
        #region 常用脚本输出
        /// <summary>
        /// 利用JavaScript显示提示,该提示信息处于页面头部，也就是说当显示提示时，页面为空白；
        /// </summary>
        /// <param name="say">要提示的信息</param>
        public void Alert(string say)
        {
            if (!string.IsNullOrWhiteSpace(say) && say.Trim() != "")
            {
                say = say.Replace("\r","\n");
                say = "<script type=\"text/javascript\">alert(\"" + say + "\");</script>";
                //ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), "Alert", say, true);                 
                Page.ClientScript.RegisterStartupScript(typeof(string), "alert", say);
                //Page.ClientScript.RegisterClientScriptBlock(typeof(string), "alert", say);
                
            }
        }
        /// <summary>
        /// 利用JavaScript显示提示,提示完，关闭窗口，一般用于弹出窗口完成后的提示
        /// </summary>
        /// <param name="say"></param>
        public  void AlertAndClose(string say)
        {
            if (!string.IsNullOrWhiteSpace(say) && say.Trim() != "")
            {
                say = say.Replace("\r", "\n");
                say = "<script type=\"text/javascript\">alert(\"" + say + "\");new top.PageBox().Close();</script>";              
                //ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), "Alert", say, true);
                Page.ClientScript.RegisterStartupScript(typeof(string), "alert", say);
                //Page.ClientScript.RegisterClientScriptBlock(typeof(string), "alert", say); 
            }
            else
            {
                this.Close();
            }
        }
        /// <summary>
        /// 利用JavaScript显示提示,提示完，关闭窗口，一般用于弹出窗口完成后的提示
        /// </summary>
        /// <param name="say"></param>
        public void AlertCloseAndRefresh(string say)
        {
            if (!string.IsNullOrWhiteSpace(say) && say.Trim() != "")
            {
                say = say.Replace("\r", "\n");
                say = "<script type=\"text/javascript\">alert(\"" + say + "\"); window.top.PageBox.CloseAndRefresh(window.name);</script>";
                //ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), "Alert", say, true);
                Page.ClientScript.RegisterStartupScript(typeof(string), "alert", say);
            }
            else
            {
                this.Close();
            }
        }
        /// <summary>
        /// 利用JavaScript显示提示,提示完，关闭窗口，
        /// </summary>
        /// <param name="say"></param>
        public void Close(string say)
        {
            if (!string.IsNullOrWhiteSpace(say) && say.Trim() != "")
            {
                say = say.Replace("\r", "\n");
                say = "<script type=\"text/javascript\">alert(\"" + say + "\");new top.PageBox().CloseAndRefresh();</script>";  
                //ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), "Alert", say, true);
            }
            else
            {
                say = "<script type=\"text/javascript\">new top.PageBox().CloseAndRefresh();</script>";  
                //ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), "Alert", say, true);
            }
            Page.ClientScript.RegisterStartupScript(typeof(string), "close", say); 
        }
        /// <summary>
        /// 关闭窗口，
        /// </summary>
        public void Close()
        {
            string say = "";
            say = "<script type=\"text/javascript\">new top.PageBox().Close();</script>";  
            //ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), "Alert", say, true);
            Page.ClientScript.RegisterStartupScript(typeof(string), "close", say); 

        }
        /// <summary>
        /// 利用JavaScript显示提示,询问是否转向指定路径
        /// </summary>
        /// <param name="say"></param>
        /// <param name="goUrl"></param>
        public void AlertAndGoUrl(string say,string goUrl)
        {
            if (!string.IsNullOrWhiteSpace(say) && say.Trim() != "")
            {
                say = say.Replace("\r", "\n");
                say = "<script type=\"text/javascript\">if(confirm(\"" + say + "\")){window.parent.location.href=\"" + goUrl + "\";}new parent.PageBox().Close();</script>";
                Page.ClientScript.RegisterStartupScript(typeof(string), "alert", say);
            }
            else
            {
                this.Close();
            }
        }
        #endregion
    }
}
