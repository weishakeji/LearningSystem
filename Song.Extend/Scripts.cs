using System;
using System.Collections.Generic;
using System.Text;

namespace Song.Extend
{
    public class Scripts
    {
        private System.Web.UI.Page _page=null;
        public Scripts(System.Web.UI.Page page)
        {
            this._page = page;
        }
        /// <summary>
        /// 利用JavaScript显示提示,该提示信息处于页面头部，也就是说当显示提示时，页面为空白；
        /// </summary>
        /// <param name="say">要提示的信息</param>
        public void Alert(string say)
        {
            if (say != "")
            {
                say = say.Replace("\\", "\\\\");
                say = say.Replace("\r", "");
                say = say.Replace("\n", "");
                string script = "<script language='JavaScript' type='text/javascript'>alert('{say}');</script>";
                script = script.Replace("{say}", say);
                if (this._page == null) return;
                if (!_page.ClientScript.IsStartupScriptRegistered(_page.GetType(), "alert"))
                {
                    _page.ClientScript.RegisterStartupScript(_page.GetType(), "alert", script);
                }
            }
        }
        public void AlertAndFresh(string say)
        {
            if (say != "")
            {
                say = say.Replace("\\", "\\\\");
                string script = "<script language='JavaScript' type='text/javascript'>alert('{say}');window.location.href=window.location.href;</script>";
                script = script.Replace("{say}", say);
                if (this._page == null) return;
                if (!_page.ClientScript.IsStartupScriptRegistered(_page.GetType(), "alert"))
                {
                    _page.ClientScript.RegisterStartupScript(_page.GetType(), "alert", script);
                }
            }
        }
        /// <summary>
        /// 利用JavaScript显示提示,提示完，关闭窗口，一般用于弹出窗口完成后的提示
        /// </summary>
        /// <param name="say"></param>
        public void AlertAndClose(string say)
        {
            if (say != "")
            {
                say = say.Replace("\\", "\\\\");
                string script = "<script language='JavaScript' type='text/javascript'>alert('{say}');window.close();</script>";
                script = script.Replace("{say}", say);
                if (this._page == null) return;
                if (!_page.ClientScript.IsStartupScriptRegistered(_page.GetType(), "AlertAndClose"))
                {
                    _page.ClientScript.RegisterStartupScript(_page.GetType(), "AlertAndClose", script);
                }               
            }
        }
        /// <summary>
        /// 利用JavaScript关闭窗口，
        /// </summary>
        public void Close()
        {            
            string script = "<script language='JavaScript' type='text/javascript'>window.close();</script>";
            if (this._page == null) return;
            if (!_page.ClientScript.IsStartupScriptRegistered(_page.GetType(), "Close"))
            {
                _page.ClientScript.RegisterStartupScript(_page.GetType(), "Close", script);
            }          
        }
        /// <summary>
        /// 利用JavaScript实现页面转向
        /// </summary>
        public void GoUrl(string url)
        {
            string script = "<script type=\"text/javascript\">window.parent.location.href='" + url + "';</script>";
            if (this._page == null) return;
            if (!_page.ClientScript.IsStartupScriptRegistered(_page.GetType(), "GoUrl"))
            {
                _page.ClientScript.RegisterStartupScript(_page.GetType(), "GoUrl", script);
            }
        }
    }
}
