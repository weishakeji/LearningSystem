using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Song.Site.Ajax
{
    /// <summary>
    /// 获取试卷的信息
    /// </summary>
    public class TestPaper : IHttpHandler
    {
        //试卷id，考试id,学生id
        private int tpid = WeiSha.Common.Request.Form["tpid"].Int32 ?? 0;            
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //Song.Entities.TestPaper tp = Business.Do<ITestPaper>().PagerSingle(tpid);
            context.Response.Write(randomJson());
            context.Response.End();
        }
        /// <summary>
        /// 随机出题
        /// </summary>
        /// <returns></returns>
        private string randomJson()
        {
            //取果是第一次打开，则随机生成试题，此为获取试卷
            Song.Entities.TestPaper tp = Business.Do<ITestPaper>().PagerSingle(tpid);
            //难度区间
            int diff1 = tp.Tp_Diff > tp.Tp_Diff2 ? (int)tp.Tp_Diff2 : (int)tp.Tp_Diff;
            int diff2 = tp.Tp_Diff > tp.Tp_Diff2 ? (int)tp.Tp_Diff : (int)tp.Tp_Diff2;
            //获取试题项
            Song.Entities.TestPaperItem[] tpi = Business.Do<ITestPaper>().GetItemForAll(tp);
            string json = "[";
            for (int i = 0; i < tpi.Length; i++)
            {
                Song.Entities.TestPaperItem pi = tpi[i];
                //类型，试题数目，该类型占多少分，
                int type = (int)pi.TPI_Type;
                int count = (int)pi.TPI_Count;
                float num = (float)pi.TPI_Number;
                if (count < 1) continue;
                string quesObj = "{";
                quesObj += "'type':" + type + ",'count':" + count + ",'number':" + num + ",";
                //当前类型的试题
                Song.Entities.Questions[] ques = Business.Do<IQuestions>().QuesRandom(tp.Org_ID, (int)tp.Sbj_ID, -1, -1, type, diff1, diff2, true, count);
                ques = clacScore(ques, num);
                quesObj += "'ques':[";
                for (int n = 0; n < ques.Length; n++)
                {
                    ques[n].Qus_Explain = "";
                    ques[n].Qus_Answer = "";
                    ques[n] = Extend.Questions.TranText(ques[n]);
                    string js = ques[n].ToJson();
                    //如果是单选题，或多选题，或填空题
                    if (ques[n].Qus_Type == 1 || ques[n].Qus_Type == 2 || ques[n].Qus_Type == 5)
                        quesObj += getAnserJson(ques[n], js);
                    else
                        quesObj += js;
                    if (n < ques.Length - 1) quesObj += ",";
                }
                quesObj += "]";
                quesObj += "}";
                if (i < tpi.Length - 1) quesObj += ",";
                json += quesObj;
            }
            json += "]";
            return json.Replace("'", "\"");
        }
        /// <summary>
        /// 计算每道试题的分数
        /// </summary>
        /// <param name="ques">试题</param>
        /// <param name="total">试题的总分</param>
        /// <returns></returns>
        private Song.Entities.Questions[] clacScore(Song.Entities.Questions[] ques, float total)
        {
            float surplus = total;
            for (int j = 0; j < ques.Length; j++)
            {
                ques[j].Qus_Explain = ques[j].Qus_Answer = ques[j].Qus_ErrorInfo = "";
                ques[j] = Extend.Questions.TranText(ques[j]);
                //当前试题的分数
                float curr = total / ques.Length;
                curr = ((float)Math.Round(curr * 10)) / 10;
                if (j < ques.Length - 1)
                {
                    ques[j].Qus_Number = curr;
                    surplus = surplus - curr;
                }
                else
                {
                    ques[j].Qus_Number = surplus;
                }
            }
            return ques;
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