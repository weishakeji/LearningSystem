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
    /// 修改学员密码
    /// </summary>
    public class SelfPw : BasePage
    {
        //学员原密码，新密码
        protected string oldPw = WeiSha.Common.Request.Form["tbPw"].String;
        protected string newPw = WeiSha.Common.Request.Form["tbNewPw"].String;
        //状态代码
        int state = WeiSha.Common.Request.QueryString["state"].Int16 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {            
            if (!string.IsNullOrWhiteSpace(oldPw))
            {
                Song.Entities.Accounts st = Extend.LoginState.Accounts.CurrentUser;
                //通过验证，进入登录状态            
                Song.Entities.Accounts emp = Business.Do<IAccounts>().AccountsSingle(st.Ac_AccName, oldPw, this.Organ.Org_ID);
                if (emp == null)
                {
                    //密码不正确
                    this.Response.Redirect(addPara(context.Request.Url.AbsolutePath, "state=-3"));
                }
                else
                {
                    st.Ac_Pw = newPw;
                    st.Ac_Pw = new WeiSha.Common.Param.Method.ConvertToAnyValue(st.Ac_Pw).MD5;
                    Business.Do<IAccounts>().AccountsSave(st);
                    //将账号信息写入                    
                    this.Document.SetValue("accid", emp.Ac_ID);
                    this.Document.SetValue("accpw", emp.Ac_Pw);
                    this.Response.Redirect(addPara(context.Request.Url.AbsolutePath, "state=1"));
                }  
            }
            //状态
            this.Document.SetValue("state", state);
        }
        /// <summary>
        /// 增加地址的参数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        private string addPara(string url, params string[] para)
        {
            string p = "";
            for (int i = 0; i < para.Length; i++)
            {
                p += para[i];
                if (i < para.Length - 1) p += "&";
            }
            url += (url.IndexOf("?") > -1 ? "&" : "?") + p;
            return url;
        }
    }
}