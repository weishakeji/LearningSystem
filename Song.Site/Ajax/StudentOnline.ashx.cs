using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.WebControl;
using System.Xml;
using System.Runtime.InteropServices;
using WeiSha.Common;
using Song.ServiceInterfaces;
namespace Song.Site.Ajax
{
    /// <summary>
    /// 记录学员在线的时间
    /// </summary>
    public class StudentOnline : IHttpHandler
    {
        //数据提交的间隔时间，也是每次提交的增加的在线时间数
        int interval = WeiSha.Common.Request.QueryString["interval"].Int32 ?? 0;
        //来源
        string plat = WeiSha.Common.Request.QueryString["plat"].String;
        public void ProcessRequest(HttpContext context)
        {
            //更新注册信息
            if (string.IsNullOrWhiteSpace(plat)) plat = "PC";
            //string plat = WeiSha.Common.Browser.IsMobile ? "Mobi" : "PC";
            Business.Do<IStudent>().LogForLoginFresh(interval, plat);
            context.Response.ContentType = "text/plain";
            context.Response.Write("1");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}