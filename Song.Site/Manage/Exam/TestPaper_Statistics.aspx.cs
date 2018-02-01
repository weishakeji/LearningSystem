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

namespace Song.Site.Manage.Exam
{
    public partial class TestPaper_Statistics : Extend.CustomPage
    {
        //试卷id
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        private int type = WeiSha.Common.Request.QueryString["type"].Int32 ?? 0;
        //题型分类汉字名称
        protected string[] typeStr = App.Get["QuesType"].Split(',');
        Song.Entities.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                fill();
                BindData(null, null);                
            }
        }

        void fill()
        {
            Song.Entities.TestPaper mm = Business.Do<ITestPaper>().PagerSingle(id);
            if (mm == null) return;
            //试卷名称
            lbPagerName.Text = mm.Tp_Name;
            //及格分，满分
            lbPassnum.Text = mm.Tp_PassScore.ToString();
            lbScore.Text = mm.Tp_Total.ToString();
            //及格率
            double pass = Business.Do<ITestPaper>().ResultsPassrate(id);
            pass = Math.Round(pass * 10000) / 100;
            lbPassPer.Text = pass.ToString();
            //参考人次
            lbPersontime.Text = Business.Do<ITestPaper>().ResultsPersontime(id).ToString();
            //平均分
            double avg = Business.Do<ITestPaper>().ResultsAverage(id);
            avg = Math.Round(avg * 100) / 100;
            lbAvg.Text = avg.ToString();
            //最高分
            Song.Entities.TestResults trHighest = Business.Do<ITestPaper>().ResultsHighest(id);
            if (trHighest != null)
            {
                lbHeight.Text = trHighest.Tr_Score.ToString();
                lbHeightStudent.Text = "（" + trHighest.Ac_Name + "）";
            }
            //最低分
            Song.Entities.TestResults trLowest = Business.Do<ITestPaper>().ResultsLowest(id);
            if (trLowest != null)
            {
                lbLower.Text = trLowest.Tr_Score.ToString();
                lbLowerStudent.Text = "（" + trLowest.Ac_Name + "）";
            }            
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //总记录数
            int count = 0;
            //
            Song.Entities.TestResults[] eas = null;
            eas = Business.Do<ITestPaper>().ResultsPager(id, Pager1.Size, Pager1.Index, out count);
            foreach (Song.Entities.TestResults r in eas)
            {
                double score = r.Tr_Score ?? 0;
                r.Tr_Score = (float)Math.Round(score * 100) / 100;
            }
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Tr_Id" };
            GridView1.DataBind();
            Pager1.RecordAmount = count;
        }
        /// <summary>
        /// 删除考试成绩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int trid = 0;   //测试成绩记录id
            int.TryParse(btn.CommandArgument, out trid);
            Business.Do<ITestPaper>().ResultsDelete(trid);
            BindData(null, null);
        }
    }
}
