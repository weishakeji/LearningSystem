using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 学员的笔记列表
    /// </summary>
    public class QuesNotes : BasePage
    {       
        
        protected override void InitPageTemplate(HttpContext context)
        {
            if (!Extend.LoginState.Accounts.IsLogin)
                this.Response.Redirect("login.ashx");
             //此页面的ajax提交，全部采用了POST方式
             if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
             {
                 string action = WeiSha.Common.Request.Form["action"].String;
                 switch (action)
                 {
                     case "delete":
                         delete_note();
                         break;
                     case "list":
                         list_note();
                         break;
                 }
             }
        }

        /// <summary>
        /// 删除笔记
        /// </summary>
        private void delete_note()
        {
            //笔记id
            int id = WeiSha.Common.Request.Form["id"].Int32 ?? 0;
            try
            {
                Business.Do<IStudent>().NotesDelete(id);
                Response.Write("1");
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
        private void list_note()
        {
            int size = WeiSha.Common.Request.Form["size"].Int32 ?? 10;  //每页多少条
            int index = WeiSha.Common.Request.Form["index"].Int32 ?? 1;  //第几页
            int stid = Extend.LoginState.Accounts.CurrentUser.Ac_ID;   //当前学员

            Song.Entities.Accounts st = Extend.LoginState.Accounts.CurrentUser;
            Song.Entities.Course currCourse = Extend.LoginState.Accounts.Course();

            int sumcount = 0;
            Song.Entities.Student_Notes[] notes = Business.Do<IStudent>().NotesPager(stid, 0, null, size, index, out sumcount);
            string json = "{\"size\":" + size + ",\"index\":" + index + ",\"sumcount\":" + sumcount + ",";
            json += "\"items\":[";
            for (int n = 0; n < notes.Length; n++)
            {
                notes[n].Stn_Context = notes[n].Stn_Context.Replace("\r","").Replace("\n", "").Replace("\t", "");
                Song.Entities.Questions q = Business.Do<IQuestions>().QuesSingle(notes[n].Qus_ID);
                Dictionary<string, object> addParas = new Dictionary<string, object>();
                if (q != null)
                {                    
                    addParas.Add("Qus_Title", q.Qus_Title);                    
                }
                else
                {
                    addParas.Add("Qus_Title", "试题不存在！");    
                }
                json += notes[n].ToJson(null, null, addParas) + ",";
            }
            if (json.EndsWith(",")) json = json.Substring(0, json.Length - 1);
            json += "]}";
            Response.Write(json);
            Response.End();    
        }
    }
}