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
    /// 试题收藏
    /// </summary>
    public class Collects : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string action = WeiSha.Common.Request.Form["action"].String;
                switch (action)
                {
                    case "delete":
                        delete();
                        break;
                    case "list":
                        list();
                        break;
                }
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        private void delete()
        {
            //收藏记录的id
            int qid = WeiSha.Common.Request.Form["qid"].Int32 ?? 0;
            try
            {
                if (Extend.LoginState.Accounts.IsLogin)
                {
                    Business.Do<IStudent>().CollectDelete(qid, this.Account.Ac_ID);
                    Response.Write("1");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            Response.End();
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        private void list()
        {
            int size = WeiSha.Common.Request.Form["size"].Int32 ?? 10;  //每页多少条
            int index = WeiSha.Common.Request.Form["index"].Int32 ?? 1;  //第几页
            int stid = Extend.LoginState.Accounts.CurrentUser.Ac_ID;   //当前学员

            Song.Entities.Accounts st = Extend.LoginState.Accounts.CurrentUser;
            Song.Entities.Course currCourse = Extend.LoginState.Accounts.Course();

            int sumcount = 0;
            //收藏的试题列表
            Song.Entities.Questions[] ques = Business.Do<IStudent>().CollectPager(st.Ac_ID, 0, currCourse.Cou_ID, -1, -1, size, index, out sumcount);
            string json = "{\"size\":" + size + ",\"index\":" + index + ",\"sumcount\":" + sumcount + ",";
            json += "\"items\":[";
            for (int n = 0; n < ques.Length; n++)
            {
                ques[n].Qus_Title = ques[n].Qus_Title.Replace("\r", "").Replace("\n", "").Replace("\t", "");
                ques[n].Qus_Title = Extend.Html.ClearHTML(ques[n].Qus_Title, "p", "img", "div", "span", "font");
                json += ques[n].ToJson() + ",";
            }
            if (json.EndsWith(",")) json = json.Substring(0, json.Length - 1);
            json += "]}";
            Response.Write(json);
            Response.End();
        }
    }
}