using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace Song.Site.Json
{
    public partial class QuesRandomForEmp : System.Web.UI.Page
    {       
        //试卷id，考试id,员工id
        private int sbjid = WeiSha.Common.Request.Form["sbjid"].Int32 ?? 0;
        private int diff = WeiSha.Common.Request.Form["diff"].Int32 ?? 0;
        private int type = WeiSha.Common.Request.Form["type"].Int32 ?? 0;
        //获取当前学科下的所有试卷
        protected void Page_Load(object sender, EventArgs e)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            EmpAccount currentUser = Extend.LoginState.Admin.CurrentUser;
            if (currentUser == null)
            {
                Response.Write("");
                Response.End();
                return;
            }
            //当前员所处的学科
            Song.Entities.Team team = Business.Do<ITeam>().TeamSingle((int)currentUser.Team_ID);
            if (team != null) sbjid = (int)team.Sbj_ID;
            //员工选择的每日一练的学科范围
            string sel = Business.Do<ISystemPara>()["SubjectForAccout_" + currentUser.Acc_Id].String;
            string[] arr = sel.Split(',');
            if (arr.Length > 0) arr[arr.Length - 1] = sbjid.ToString();
            //随机抽取学科
            if (arr.Length > 1)
            {
                Random rand = new Random();
                int index = rand.Next(0, arr.Length - 1);
                sbjid = Convert.ToInt32(arr[index]);
            }
            Song.Entities.Questions[] ques = Business.Do<IQuestions>().QuesRandom(org.Org_ID, sbjid, -1, -1, type, diff, diff, true, 1);
            string tm = "";
            if (ques.Length > 0)
            {
                Song.Entities.Questions q = ques[0];
                q = replaceText(q);
                tm = q.ToJson();
                //如果是单选题，或多选题
                if (q.Qus_Type == 1 || q.Qus_Type == 2 || q.Qus_Type == 5)
                    tm = getAnserJson(q, tm);
            }
            
            Response.Write(tm);
            Response.End();
        }
        private string getAnserJson(Song.Entities.Questions q, string json)
        {
            //当前试题的答案
            Song.Entities.QuesAnswer[] ans = Business.Do<IQuestions>().QuestionsAnswer(q, null);
            string ansStr = "[";
            for (int i = 0; i < ans.Length; i++)
            {
                ansStr += ans[i].ToJson();
                if (i < ans.Length - 1) ansStr += ",";
            }
            ansStr += "]";
            json = json.Replace("}", ",\"Answer\":" + ansStr + "}");
            return json;
        }
        /// <summary>
        /// 处理试题中的文本内容
        /// </summary>
        /// <param name="qs"></param>
        /// <returns></returns>
        private Song.Entities.Questions replaceText(Song.Entities.Questions qs)
        {
            qs.Qus_Title = qs.Qus_Title == null ? "" : qs.Qus_Title;
            qs.Qus_Title = qs.Qus_Title.Replace("\r","");
            qs.Qus_Title = qs.Qus_Title.Replace("\n", "");
            qs.Qus_Title = qs.Qus_Title.Replace("\"", "＂");
            qs.Qus_Title = qs.Qus_Title.Replace("\t", "");
            //
            qs.Qus_Explain = qs.Qus_Explain == null ? "" : qs.Qus_Explain;
            if (qs.Qus_Explain != string.Empty)
            {
                qs.Qus_Explain = qs.Qus_Explain.Replace("\r", "");
                qs.Qus_Explain = qs.Qus_Explain.Replace("\n", "");
                qs.Qus_Explain = qs.Qus_Explain.Replace("\"", "＂");
                qs.Qus_Explain = qs.Qus_Title.Replace("\t", "");
            }
            //
            if (qs.Qus_Answer != string.Empty)
            {
                qs.Qus_Answer = qs.Qus_Answer == null ? "" : qs.Qus_Answer;
                qs.Qus_Answer = qs.Qus_Answer.Replace("\r", "");
                qs.Qus_Answer = qs.Qus_Answer.Replace("\n", "");
                qs.Qus_Answer = qs.Qus_Answer.Replace("\"", "＂");
                qs.Qus_Answer = qs.Qus_Title.Replace("\t", "");
            }
            return qs;
        }
    }
}
