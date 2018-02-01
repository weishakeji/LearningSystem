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
using System.Xml;
using System.IO;

namespace Song.Site.Manage.Teacher
{
    public partial class Correct : Extend.CustomPage
    {
        //考试成绩记录id
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        //学生Id
        private int stid = WeiSha.Common.Request.QueryString["stid"].Int32 ?? 0;
        //当前考试
        Song.Entities.Examination exam;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                bindStudent();
                fill();
            }
        }
        /// <summary>
        /// 当前考试所有考生
        /// </summary>
        private void bindStudent()
        {
            Song.Entities.ExamResults[] res = Business.Do<IExamination>().ResultCount(id, -1);
            ddlStudent.DataSource = res;
            ddlStudent.DataTextField = "Ac_Name";
            ddlStudent.DataValueField = "Ac_ID";
            ddlStudent.DataBind();
            //
            foreach (ListItem li in ddlStudent.Items)
            {
                int stid = Convert.ToInt32(li.Value);
                foreach (Song.Entities.ExamResults ex in res)
                {
                    if (stid == ex.Ac_ID)
                    {
                        if (ex.Exr_Score < ex.Exr_ScoreFinal)
                        {
                            li.Attributes.Add("class","correct");
                        }
                        break;
                    }
                }
            }
        }
        private void fill()
        {
            //当前考试
            exam = Business.Do<IExamination>().ExamSingle(id);
            Song.Entities.ExamResults er = Business.Do<IExamination>().ResultSingle(id, 0, stid);
            if (er == null) return;
            stid = er.Ac_ID;
            //当前学生
            ListItem li = ddlStudent.Items.FindByValue(stid.ToString());
            if (li != null) li.Selected = true;
            //
            //学生名称
            lbStudent.Text = er.Ac_Name;
            //考试主题
            lbExamTheme.Text = exam.Exam_Title;
            //场次
            lbExamName.Text = exam.Exam_Name;
            //应试时间
            lbExamTime.Text = ((DateTime)er.Exr_CrtTime).ToString("yyyy月MM月dd日 hh:mm");
            //最终得分
            lbScoreFinal.Text = er.Exr_ScoreFinal.ToString();
            //展示简答题
            plJianda.Visible = bindShortQues(er);
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
                XmlNodeList nodes = nodeList[i].ChildNodes;
                for (int j = 0; j < nodes.Count; j++)
                {
                    //试题的Id
                    int id = Convert.ToInt32(nodes[j].Attributes["id"].Value);
                    //试题的分数
                    double num = Convert.ToDouble(nodes[j].Attributes["num"].Value);
                    //试题得分
                    double score = 0;
                    if (nodes[j].Attributes["score"] != null)
                        score = Convert.ToDouble(nodes[j].Attributes["score"].Value);
                    //回答
                    string reply = nodes[j].InnerText;
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
                    dr["score"] = score.ToString();
                    dr["reply"] = reply;
                    dt.Rows.Add(dr);
                }
            }
            rptQues.DataSource = dt;
            rptQues.DataBind();
            return dt.Rows.Count > 0;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            //判断分值是否超出最大分值
            for (int i = 0; i < rptQues.Items.Count; i++)
            {
                //当前试题的分数
                HiddenField hfnumField = (HiddenField)rptQues.Items[i].FindControl("hfnum");
                double hfnum = 0;
                double.TryParse(hfnumField.Value, out hfnum);
                //实际得分
                TextBox tbNumber = (TextBox)rptQues.Items[i].FindControl("tbNumber");
                double number = 0;
                double.TryParse(tbNumber.Text, out number);
                if (string.IsNullOrEmpty(tbNumber.Text))
                {
                    Master.Alert("第" + (i + 1) + "题得分不能为空！");
                    return;
                }
                if (number > hfnum)
                {
                    Master.Alert("第" + (i + 1) + "题得分超出该题最高分值！");
                    return;
                }
                if (number < 0)
                {
                    Master.Alert("第" + (i + 1) + "题得分不得为负值！");
                    return;
                }
            }
            Song.Entities.ExamResults mm = Business.Do<IExamination>().ResultSingle(id, 0, stid);
            //简答题得分
            double sQusNum = calcShort(mm);
            mm.Exr_ScoreFinal = (float)mm.Exr_Score + (float)sQusNum;
            //最终得分
            try
            {
                Business.Do<IExamination>().ResultSave(mm);
                Song.Entities.ExamResults next = Business.Do<IExamination>().ResultSingleNext(id, stid, false);
                if (next != null)
                {
                    this.Response.Redirect(string.Format("Correct.aspx?id={0}&stid={1}", next.Exam_ID.ToString(), next.Ac_ID.ToString()));
                }
                else
                {
                    fill();
                    Master.Alert("操作成功！");
                }
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
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
            XmlNodeList nodes = null;
            for (int i = 0; i < nodeList.Count; i++)
            {
                //试题的类型
                int type = Convert.ToInt32(nodeList[i].Attributes["type"].Value);
                //如果是不是简答题，跳过
                if (type != 4) continue;
                nodes = nodeList[i].ChildNodes;
            }
            if (nodes == null) return 0;
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
                for (int i = 0; i < nodes.Count; i++)
                {
                    XmlNode node = nodes[i];
                    if (node.Attributes["id"].Value == lbId.Text)
                    {
                        XmlElement el = (XmlElement)node;
                        el.SetAttribute("score", score.ToString());
                        //试题的分数
                        double num = Convert.ToDouble(node.Attributes["num"].Value);
                        if (score >= num) el.SetAttribute("sucess", "true");
                    }
                }
            }
            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);
            resXml.WriteTo(xw);
            exr.Exr_Results = sw.ToString();
            return scoreSum;
        }

        protected void ddlStudent_SelectedIndexChanged(object sender, EventArgs e)
        {            
            this.Response.Redirect(string.Format("Correct.aspx?id={0}&stid={1}", id.ToString(), ddlStudent.SelectedValue));
        }
        /// <summary>
        /// 当试题绑定时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptQues_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //获取当前试题id与附件控件
            Label lbID = (Label)e.Item.FindControl("lbID");
            HyperLink link=(HyperLink)e.Item.FindControl("linkAcc");
            //获取附件
            string path = Upload.Get["Exam"].Physics + exam.Exam_ID + "/" + stid + "/";
            string file = "";
            if (System.IO.Directory.Exists(path))
            {                
                foreach (FileInfo f in new DirectoryInfo(path).GetFiles())
                {
                    if (f.Name.IndexOf("_") > 0)
                    {
                        string idtm = f.Name.Substring(0, f.Name.IndexOf("_"));
                        if (idtm == lbID.Text.ToString())
                        {
                            file = f.Name;
                            break;
                        }
                    }
                }
            }
            if (file.Trim() != "")
            {
                link.Text = file;
                link.NavigateUrl = Upload.Get["Exam"].Virtual + exam.Exam_ID + "/" + stid + "/" + file;
            }

        }
    }
}
