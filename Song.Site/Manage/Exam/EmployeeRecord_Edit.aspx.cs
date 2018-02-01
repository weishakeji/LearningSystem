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
using WeiSha.WebControl;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace Song.Site.Manage.Site
{
    public partial class EmployeeRecord_Edit : Extend.CustomPage
    {
        //成绩id
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
        }

        void fill()
        {
            try
            {
                if (id < 1) return;
                Song.Entities.ExamResults mm = Business.Do<IExamination>().ResultSingle(id);
                if (mm == null) return;
                //考生姓名，考试主题，学科
                lbAccName.Text = mm.Ac_Name;
                lbExamTheme.Text = mm.Exam_Title;
                lbExamSbj.Text = mm.Sbj_Name;
                //自动判卷得分
                lbScore.Text = mm.Exr_Score.ToString();
                //最终得分
                lbScoreFinal.Text = mm.Exr_ScoreFinal.ToString();
                //人工判卷
                tbDraw.Text = mm.Exr_Draw.ToString();
                tbColligate.Text = mm.Exr_Colligate.ToString();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
            try
            {
                if (id < 1) return;
                Song.Entities.ExamResults mm = Business.Do<IExamination>().ResultSingle(id);
                if (mm == null) return;
                //考生姓名，考试主题，学科
                lbAccName.Text = mm.Ac_Name;
                lbExamTheme.Text = mm.Exam_Title;
                lbExamSbj.Text = mm.Sbj_Name;
                //应试时间
                lbExamTime.Text = ((DateTime)mm.Exr_CrtTime).ToString("yyyy月MM月dd日 hh:mm");
                //采用试卷
                Song.Entities.TestPaper tp = Business.Do<ITestPaper>().PagerSingle((int)mm.Tp_Id);
                lbTestPager.Text = tp != null ? tp.Tp_Name : "";
                //自动判卷得分
                lbScore.Text = mm.Exr_Score.ToString();
                //最终得分
                lbScoreFinal.Text = mm.Exr_ScoreFinal.ToString();
                //人工判卷
                tbDraw.Text = mm.Exr_Draw.ToString();
                tbColligate.Text = mm.Exr_Colligate.ToString();
                //展示简答题
                plJianda.Visible = bindShortQues(mm);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }

        protected void btnEnter_Click(object sender, ImageClickEventArgs e)
        {
            if (id < 1) return;
            Song.Entities.ExamResults mm = Business.Do<IExamination>().ResultSingle(id);
            //画图得分
            float draw = (float)Convert.ToDouble(tbDraw.Text.Trim() == "" ? "0" : tbDraw.Text);
            mm.Exr_Draw = draw;
            //综合分
            float coli = (float)Convert.ToDouble(tbColligate.Text.Trim() == "" ? "0" : tbColligate.Text);
            mm.Exr_Colligate = coli;
            //简答题得分
            double sQusNum = calcShort(mm);
            //最终得分
            try
            {
                mm = Business.Do<IExamination>().ResultClacScore(mm);
                Business.Do<IExamination>().ResultSave(mm);
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }

        }

        /// <summary>
        /// 绑定简答题
        /// </summary>
        private bool bindShortQues(Song.Entities.ExamResults exr)
        {
            if (string.IsNullOrEmpty(exr.Exr_Results)) return false;
            DataTable dt = new DataTable("DataBase");
            //试题的id，题干，答案，类型
            dt.Columns.Add(new DataColumn("qid", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("qtitle", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("answer", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("type", Type.GetType("System.String")));
            //试题分数，得分，考生回答内容
            dt.Columns.Add(new DataColumn("num", Type.GetType("System.String")));            
            dt.Columns.Add(new DataColumn("score", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("reply", Type.GetType("System.String")));
            //
            XmlDocument resXml = new XmlDocument();
            resXml.LoadXml(exr.Exr_Results, false);
            XmlNodeList nodeList = resXml.SelectSingleNode("results").ChildNodes;            
            for (int i = 0; i < nodeList.Count; i++)
            {
                //试题的类型
                int type = Convert.ToInt32(nodeList[i].Attributes["type"].Value);
                //如果是不是简答题，跳过
                if (type != 4) continue;
                //试题的Id
                int id = Convert.ToInt32(nodeList[i].Attributes["id"].Value);                
                //试题的分数
                double num = Convert.ToDouble(nodeList[i].Attributes["num"].Value);
                //试题得分
                double score=0;
                if (nodeList[i].Attributes["score"] != null)
                    score = Convert.ToDouble(nodeList[i].Attributes["score"].Value);               
                //回答
                string reply = nodeList[i].InnerText;
                //生成Datatable
                DataRow dr = dt.NewRow();
                dr["qid"] = id.ToString();
                Song.Entities.Questions qus = Business.Do<IQuestions>().QuesSingle(id);
                if (qus != null)
                {
                    dr["qtitle"] = qus.Qus_Title;
                    dr["answer"] = qus.Qus_Answer;
                }
                dr["type"] = type.ToString();
                dr["num"] = num.ToString();
                dr["score"] = score < 1 ? "" : score.ToString();
                dr["reply"] = reply;
                dt.Rows.Add(dr);                
            }
            rptQues.DataSource = dt;
            rptQues.DataBind();
            return dt.Rows.Count > 0;
        }
        /// <summary>
        /// 计算简答题得分
        /// </summary>
        /// <returns></returns>
        private double calcShort(Song.Entities.ExamResults exr)
        {
            if (string.IsNullOrEmpty(exr.Exr_Results)) return 0;
            XmlDocument resXml = new XmlDocument();
            resXml.LoadXml(exr.Exr_Results, false);
            XmlNodeList nodeList = resXml.SelectSingleNode("results").ChildNodes;
            //得分记录
            double scoreSum = 0;
            foreach (RepeaterItem pi in rptQues.Items)
            {
                //id
                Label lbId = (Label)pi.FindControl("lbID");
                //得分
                TextBox tb = (TextBox)pi.FindControl("tbNumber");
                double score = tb.Text.Trim() == "" ? 0 : Convert.ToDouble(tb.Text);
                scoreSum += score;
                for (int i = 0; i < nodeList.Count; i++)
                {
                    XmlNode node = nodeList[i];
                    if (node.Attributes["id"].Value == lbId.Text)
                    {
                        XmlElement el = (XmlElement)node;
                        el.SetAttribute("score", score.ToString());                        
                    }
                }
            }
            StringWriter sw = new StringWriter(); 
            XmlTextWriter xw = new XmlTextWriter(sw); 
            resXml.WriteTo(xw);
            exr.Exr_Results = sw.ToString();
            return scoreSum;
        }
    }
}
