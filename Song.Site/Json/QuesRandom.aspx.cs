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

using Song.ServiceInterfaces;
using Song.Entities;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using WeiSha.Common;
namespace Song.Site.Json
{
    public partial class QuesRandom : System.Web.UI.Page
    {       
        //试卷id，考试id,学生id
        private int tpid = WeiSha.Common.Request.Form["tpid"].Int32 ?? 0;
        private int examid = WeiSha.Common.Request.Form["examid"].Int32 ?? 0;
        private int stid = WeiSha.Common.Request.Form["stid"].Int32 ?? 0;
        //获取当前学科下的所有试卷
        protected void Page_Load(object sender, EventArgs e)
        {
            //获取答题信息
            Song.Entities.ExamResults exr = Business.Do<IExamination>().ResultSingle(examid, tpid, stid);
            //取试题
            List<Song.Entities.Questions> quesList = new List<Questions>();
            if (exr == null)
            {
                //取果是第一次打开，则随机生成试题，此为获取试卷
                Song.Entities.TestPaper tp = Business.Do<ITestPaper>().PagerSingle(tpid);
                //难度区间
                int diff1 = tp.Tp_Diff > tp.Tp_Diff2 ? (int)tp.Tp_Diff2 : (int)tp.Tp_Diff;
                int diff2 = tp.Tp_Diff > tp.Tp_Diff2 ? (int)tp.Tp_Diff : (int)tp.Tp_Diff2;
                //获取试题项
                Song.Entities.TestPaperItem[] tpi = Business.Do<ITestPaper>().GetItemForAll(tp);
                foreach (Song.Entities.TestPaperItem pi in tpi)
                {
                    //类型，试题数目，该类型占多少分，
                    int type = (int)pi.TPI_Type;
                    int count = (int)pi.TPI_Count;
                    float num = (float)pi.TPI_Number;
                    int per = (int)pi.TPI_Percent;
                    if (count < 1) continue;
                    Song.Entities.Questions[] ques = Business.Do<IQuestions>().QuesRandom(tp.Org_ID, (int)tp.Sbj_ID, -1, -1, type, diff1, diff2, true, count);
                    float surplus = num;
                    for (int i = 0; i < ques.Length; i++)
                    {
                        //当前试题的分数
                        float curr = ((float)pi.TPI_Number) / ques.Length;
                        curr = ((float)Math.Round(curr * 10)) / 10;
                        if (i < ques.Length - 1)
                        {
                            ques[i].Qus_Number = curr;
                            surplus = surplus - curr;
                        }
                        else
                        {
                            ques[i].Qus_Number = surplus;
                        }
                    }
                    foreach (Song.Entities.Questions q in ques)
                        quesList.Add(replaceText(q));
                }

            }
            else
            {
                //如果已经做过试题
                if (exr.Exr_IsSubmit)
                {
                    //如果已经交过卷，则不允许再做题
                    Response.Write("2");
                    Response.End();
                }
                else
                {
                    quesList = Business.Do<IExamination>().QuesForResults(exr.Exr_Results);
                }
            }
            
            string tm = "[";
            for (int i = 0; i < quesList.Count; i++)
            {
                string json = quesList[i].ToJson();
                //如果是单选题，或多选题，或填空题
                if (quesList[i].Qus_Type == 1 || quesList[i].Qus_Type == 2 || quesList[i].Qus_Type == 5)
                    tm += getAnserJson(quesList[i], json);
                else
                    tm += json;
                if (i < quesList.Count - 1) tm += ",";
            }
            tm += "]";
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
