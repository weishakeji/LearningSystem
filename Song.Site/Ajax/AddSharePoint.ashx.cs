using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
namespace Song.Site.Ajax
{
    /// <summary>
    /// 添加分享积分
    /// </summary>
    public class AddSharePoint : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //分享人的id
            int sharekeyid = WeiSha.Common.Request.QueryString["sharekeyid"].Int32 ?? 1;
            //如果有登录的账户，则表示对方已经在访问了，不增加积分；
            if (Extend.LoginState.Accounts.IsLogin) return;

            //增加分享积分
            //获取推荐人
            Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsSingle(sharekeyid);
            if (acc == null) return;
            Business.Do<IAccounts>().PointAdd4Share(acc);    //增加分享积分
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