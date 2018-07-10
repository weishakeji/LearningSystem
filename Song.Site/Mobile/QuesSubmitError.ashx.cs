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
    /// 提交错误试题
    /// </summary>
    public class QuesSubmitError : BasePage
    {
        //试题id
        protected int qid = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        //错误类型
        string errtype = WeiSha.Common.Request.Form["err1"].String;
        //错误信息
        string errinfo = WeiSha.Common.Request.Form["tbErrInfo"].String;
        protected override void InitPageTemplate(HttpContext context)
        {
            Song.Entities.Questions ques = Business.Do<IQuestions>().QuesSingle(qid);
            this.Document.SetValue("ques", ques);

            if (string.IsNullOrWhiteSpace(errtype) && string.IsNullOrWhiteSpace(errinfo))
            {
                return;

            }
            if (string.IsNullOrWhiteSpace(ques.Qus_ErrorInfo))
            {
                ques.Qus_WrongInfo = errtype + "\r" + errinfo;
            }
            else
            {
                ques.Qus_WrongInfo += "\r" + errtype + "\r" + errinfo;
            }

            try
            {
                ques.Qus_IsWrong = true;
                Business.Do<IQuestions>().QuesSave(ques);
                this.Document.SetValue("isSuccess", true);
            }
            catch
            {
                this.Document.SetValue("isSuccess", false);
            }


        }
    }
}