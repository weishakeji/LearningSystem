using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Song.Site.Ajax
{
    /// <summary>
    /// QuesRandom 的摘要说明
    /// </summary>
    public class QuesRandom : IHttpHandler
    {
        //专业id，题型,难度等级
        private int sbjid = WeiSha.Common.Request.QueryString["sbjid"].Int32 ?? 0;
        private int type = WeiSha.Common.Request.QueryString["type"].Int32 ?? 0;
        private int diff = WeiSha.Common.Request.QueryString["diff"].Int32 ?? 0;        
        public void ProcessRequest(HttpContext context)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            context.Response.ContentType = "text/plain";
            Song.Entities.Questions[] ques = Business.Do<IQuestions>().QuesRandom(org.Org_ID, sbjid, -1, -1, type, diff, diff, true, 1);
            string tm = "";
            if (ques.Length > 0)
            {
                Song.Entities.Questions q = Extend.Questions.TranText(ques[0]);
                tm = q.ToJson();
                //如果是单选题，或多选题
                if (q.Qus_Type == 1 || q.Qus_Type == 2 || q.Qus_Type == 5)
                    tm = getAnserJson(q, tm);
            }

            context.Response.Write(tm);
            context.Response.End();
        }
        private string getAnserJson(Song.Entities.Questions q, string json)
        {
            //当前试题的答案
            Song.Entities.QuesAnswer[] ans = Business.Do<IQuestions>().QuestionsAnswer(q, null);
            string ansStr = "[";
            for (int i = 0; i < ans.Length; i++)
            {
                ans[i] = Extend.Questions.TranText(ans[i]);
                ansStr += ans[i].ToJson();
                if (i < ans.Length - 1) ansStr += ",";
            }
            ansStr += "]";
            json = json.Replace("}", ",\"Answer\":" + ansStr + "}");
            return json;
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