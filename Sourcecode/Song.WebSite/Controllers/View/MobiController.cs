using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Song.WebSite.Controllers
{
    public class MobiController : Controller
    {
        /// <summary>
        /// 没有对应的action时，默认对应指定页面
        /// </summary>
        /// <param name="actionName">方法名，对应视图文件夹的cshtml页</param>
        protected override void HandleUnknownAction(string actionName)
        {
            // 获取控制器、方法的名称，以及id值
            string ctr = this.RouteData.Values["controller"].ToString();
            string action = this.RouteData.Values["action"].ToString();
            string id = this.RouteData.Values["id"]!=null ? this.RouteData.Values["id"].ToString() : string.Empty;

            ////如果是web端，则跳转
            //if (WeiSha.Core.Skip.Enable && !WeiSha.Core.Browser.IsMobile)
            //{
            //    string url = this.Request.Url.ToString();
            //    url = WeiSha.Core.Skip.MobiToWeb(url);
            //    this.Response.Redirect(url);
            //    this.Response.Close();
            //    return;
            //}
            try
            {
                //返回方法名为视图的对象，只要Views文件夹存在actionName.cshtml
                this.View(actionName).ExecuteResult(this.ControllerContext);
            }
            catch (InvalidOperationException ieox)
            {
                //如果Views文件夹不存在actionName.cshtml,则返回到错误页
                ViewData["error"] = "Unknown Action: \"" + Server.HtmlEncode(actionName) + "\"";
                ViewData["exMessage"] = ieox.Message;
                //this.View("Error_404").ExecuteResult(this.ControllerContext);
            }
        }

    }
}
