using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Song.WebSite.Controllers
{
    /// <summary>
    /// 平板电脑的页面
    /// </summary>
    public class PadController : Controller
    {
        /// <summary>
        /// 没有对应的action时，默认对应到templates中的指定页面
        /// </summary>
        /// <param name="actionName">方法名，对应视图文件夹的html页</param>
        protected override void HandleUnknownAction(string actionName)
        {
            //返回方法名为视图的对象，只要Views文件夹存在actionName.html
            ViewResult vr = this.View(actionName);
            vr.ExecuteResult(this.ControllerContext);
        }
    }
}
