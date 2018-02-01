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



namespace Song.Site.Manage.Student
{
    public partial class Ques_View : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //题型分类汉字名称
        protected string[] typeStr = App.Get["QuesType"].Split(',');
        protected Song.Entities.Questions mm;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {              
                fill();
            }
            
        }

        private void fill()
        {
            if (id < 1) return;
            mm = Business.Do<IQuestions>().QuesSingle(id);
            ltTitle.Text = Extend.Html.ClearHTML(mm.Qus_Title, "pre", "p");
            //知识点解析
            ltExplan.Text = mm.Qus_Explain == string.Empty ? "无" : mm.Qus_Explain;
            ltExplan.Text = Extend.Html.ClearHTML(ltExplan.Text, "pre", "p");
            if (mm.Qus_Type == 1) getAnswer1(mm);
            if (mm.Qus_Type == 2) getAnswer2(mm);
            if (mm.Qus_Type == 3) getAnswer3(mm);
            if (mm.Qus_Type == 4) getAnswer4(mm);
            if (mm.Qus_Type == 5) getAnswer5(mm);
        }
        #region 获取答案
        /// <summary>
        /// 获取单选题答案
        /// </summary>
        /// <param name="qus"></param>
        private void getAnswer1(Song.Entities.Questions qus)
        {
            string ansStr = "";
            //当前试题的答案
            Song.Entities.QuesAnswer[] ans = Business.Do<IQuestions>().QuestionsAnswer(qus, null);
            for (int i = 0; i < ans.Length; i++)
            {
                if (ans[i].Ans_IsCorrect)
                    ansStr += (char)(65 + i);
            }
            ltAnswerWord.Text = ansStr;
            divAnswerWord.Visible = true;
            divAnswerText.Visible = false;
            //
            rptItem.DataSource = ans;
            rptItem.DataBind();
        }
        /// <summary>
        /// 获取多选题答案
        /// </summary>
        /// <param name="qus"></param>
        private void getAnswer2(Song.Entities.Questions qus)
        {
            string ansStr = "";
            //当前试题的答案
            Song.Entities.QuesAnswer[] ans = Business.Do<IQuestions>().QuestionsAnswer(qus, null);
            for (int i = 0; i < ans.Length; i++)
            {
                if (ans[i].Ans_IsCorrect)
                    ansStr += (char)(65 + i) + "、";
            }
            ansStr = ansStr.Substring(0, ansStr.LastIndexOf("、"));
            ltAnswerWord.Text = ansStr;
            divAnswerWord.Visible = true;
            divAnswerText.Visible = false;
            rptItem.DataSource = ans;
            rptItem.DataBind();
        }
        /// <summary>
        /// 获取判断题答案
        /// </summary>
        /// <param name="qus"></param>
        private void getAnswer3(Song.Entities.Questions qus)
        {
            string ansStr = qus.Qus_IsCorrect ? "正确" : "错误";
            ltAnswerWord.Text = ansStr;
            divAnswerWord.Visible = true;
            divAnswerText.Visible = false;           
            
        }
        /// <summary>
        /// 获取简答题答案
        /// </summary>
        /// <param name="qus"></param>
        private void getAnswer4(Song.Entities.Questions qus)
        {
            if (qus != null && !string.IsNullOrEmpty(qus.Qus_Answer))
            {
                ltAnswerText.Text = qus.Qus_Answer;
            }
            divAnswerWord.Visible = false;
            divAnswerText.Visible = true;
        }
        /// <summary>
        /// 获取填空题答案
        /// </summary>
        /// <param name="qusUid"></param>
        private void getAnswer5(Song.Entities.Questions qus)
        {
            string ansStr = "";
            //当前试题的答案
            Song.Entities.QuesAnswer[] ans = Business.Do<IQuestions>().QuestionsAnswer(qus, null);
            for (int i = 0; i < ans.Length; i++)
            {
                ansStr += ans[i].Ans_Context + "<br/>";
            }
            ltAnswerWord.Text = ansStr;
            divAnswerWord.Visible = true;
            divAnswerText.Visible = false;
        }
        #endregion
    }
}
