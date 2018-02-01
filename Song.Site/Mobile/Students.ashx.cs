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
    /// 当前课程的学员
    /// </summary>
    public class Students : BasePage
    {       
        protected override void InitPageTemplate(HttpContext context)
        {
           //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                int size = WeiSha.Common.Request.Form["size"].Int32 ?? 10;  //每页多少条
                int index = WeiSha.Common.Request.Form["index"].Int32 ?? 1;  //第几页                

                Song.Entities.Accounts st = Extend.LoginState.Accounts.CurrentUser;   //当前学员
                int stid =st.Ac_ID;
                Song.Entities.Course currCourse = Extend.LoginState.Accounts.Course();

                int sumcount = 0;
                Song.Entities.Accounts[] eas = null;
                eas = Business.Do<ICourse>().Student4Course(currCourse.Cou_ID, null, null, size, index, out sumcount);
                string json = "{\"size\":" + size + ",\"index\":" + index + ",\"sumcount\":" + sumcount + ",";
                json += "\"items\":[";
                for (int i = 0; i < eas.Length; i++)
                {
                    eas[i].Ac_Photo = string.IsNullOrWhiteSpace(eas[i].Ac_Photo) ? null : Upload.Get["Student"].Virtual + eas[i].Ac_Photo;
                    json += eas[i].ToJson() + ",";
                }
                if (json.EndsWith(",")) json = json.Substring(0, json.Length - 1);
                json += "]}";
                Response.Write(json);
                Response.End(); 
            }
        }
    }
}