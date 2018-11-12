using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 添加学员的错误试题
    /// </summary>
    public class AddQues : IHttpHandler
    {
        //试题id,课程id
        protected int qid = WeiSha.Common.Request.QueryString["qid"].Int32 ?? 0;
        protected int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        public void ProcessRequest(HttpContext context)
        {
            if (Extend.LoginState.Accounts.IsLogin)
            {
                //如果不存在收藏，则添加
                Song.Entities.Student_Ques stc = new Entities.Student_Ques();
                stc.Ac_ID = Extend.LoginState.Accounts.CurrentUser.Ac_ID;
                stc.Qus_ID = qid;
                stc.Cou_ID = couid;
                Business.Do<IStudent>().QuesAdd(stc);
            }
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