using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
namespace Song.Site.Ajax
{
    /// <summary>
    /// 获取当前登录账户信息
    /// </summary>
    public class Self : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //当前登录账号
            Song.Entities.Accounts acc = Extend.LoginState.Accounts.CurrentUser;
            string tm = "{\"login\":{islogin}";
            if (acc == null) tm = tm.Replace("{islogin}", false.ToString().ToLower());
            if (acc != null)
            {
                tm = tm.Replace("{islogin}", true.ToString().ToLower());
                //资源的路径
                //string resPath = Upload.Get["Accounts"].Virtual;
                //acc.Ac_Photo = resPath + acc.Ac_Photo;
                tm += ",\"object\":" + acc.ToJson(null, "Ac_Pw,Ac_Qus,Ac_Ans,Ac_IDCardNumber");
            }
            tm += "}";
            context.Response.Write(tm);
            context.Response.End();
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