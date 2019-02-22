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

namespace Song.Site.Manage.Exam
{
    public partial class TestPaper_Edit2 : Extend.CustomPage
    {
        //试卷id
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //试卷类型，1为静态试卷，2为动态试卷
        private int type = WeiSha.Common.Request.QueryString["type"].Int32 ?? 0;
        //课程id
        private int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //题型分类汉字名称
        protected string[] typeStr = App.Get["QuesType"].Split(',');
        private string _uppath = "TestPaper";
        Song.Entities.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            ddlCourse.Enabled = ddlSubject.Enabled = couid <= 0;
            if (!this.IsPostBack)
            {
                InitBind();
                fill();
            }
        }
        #region 初始化界面
        /// <summary>
        /// 界面的初始绑定
        /// </summary>
        private void InitBind()
        {           
             //学科/专业
            Song.Entities.Subject[] subs = Business.Do<ISubject>().SubjectCount(org.Org_ID, null,true,-1, 0);
            this.ddlSubject.DataSource = subs;
            this.ddlSubject.DataTextField = "Sbj_Name";
            this.ddlSubject.DataValueField = "Sbj_ID";
            this.ddlSubject.DataBind();
            //当前课程（当处在课程管理中时），id小于1表示新建试卷，couid大于0表示处在课程管理中
            Song.Entities.Course course = id < 1 && couid > 0 ? Business.Do<ICourse>().CourseSingle(couid) : null;
            if (course != null)
            {                
                ListItem lisubject = ddlSubject.Items.FindByValue(course.Sbj_ID.ToString());
                if (lisubject != null)
                {
                    ddlSubject.SelectedIndex = -1;
                    lisubject.Selected = true;                   
                }
            }
            ddlSubject_SelectedIndexChanged(null, null);
            //当针对所有章节时，绑定试题类型
            rptItemForAll.DataSource = typeStr;
            rptItemForAll.DataBind();
            rptOutlineScore.DataSource = typeStr;
            rptOutlineScore.DataBind();
        }
        /// <summary>
        /// 当专业下拉菜单更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            //课程
            int sbjid;
            int.TryParse(ddlSubject.SelectedValue, out sbjid);
            if (sbjid > 0)
            {
                ddlCourse.Items.Clear();
                List<Song.Entities.Course> cous = Business.Do<ICourse>().CourseCount(org.Org_ID, sbjid, null, null, -1);
                ddlCourse.DataSource = cous;
                this.ddlCourse.DataTextField = "Cou_Name";
                this.ddlCourse.DataValueField = "Cou_ID";
                this.ddlCourse.Root = 0;
                this.ddlCourse.DataBind();
            }
            //
            if (ddlCourse.Items.Count < 1)          
                ddlCourse.Items.Add(new ListItem("--没有课程--","-1"));           
            //选定当前课程
            ListItem licourse = this.ddlCourse.Items.FindByValue(couid.ToString());
            if (licourse != null)
            {
                ddlCourse.SelectedIndex = -1;
                licourse.Selected = true;
            }            
        }
        /// <summary>
        /// 当课程下拉菜单更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            int couid = 0;
            int.TryParse(ddlCourse.SelectedValue, out couid);
            rptOutline.ItemDataBound += rptOutline_ItemDataBound;
            Song.Entities.Outline[] outlines = Business.Do<IOutline>().OutlineCount(couid, 0, true, -1);
            rptOutline.DataSource = outlines;
            rptOutline.DataBind();
            ltNoOutline.Visible = outlines.Length < 1;
            //按章节选择试题的区域，进行刷新
            UpdatePanel_ItemForOutline.Update();
        }
        /// <summary>
        /// 当试题按章节出题时，选择区域的章节列表绑定时，触发该事件
        /// 并试题类型显示出来，供填写试题数量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptOutline_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rtp = (Repeater)e.Item.FindControl("rtpOutlineItem");
                rtp.DataSource = typeStr;
                rtp.DataBind();
            }
        }
        #endregion

        #region 数据填充
        /// <summary>
        /// 填充数据
        /// </summary>
        private void fill()
        {
            Song.Entities.TestPaper mm;
            if (id != 0)
            {
                mm = Business.Do<ITestPaper>().PagerSingle(id);
                cbIsUse.Checked = mm.Tp_IsUse;  //是否使用
                cbIsRec.Checked = mm.Tp_IsRec;  //是否推荐
                ViewState["UID"] = mm.Tp_UID;   //唯一标识
                //所属专业                
                ListItem liSubj = ddlSubject.Items.FindByValue(mm.Sbj_ID.ToString());
                if (liSubj != null)
                {
                    ddlSubject.SelectedIndex = -1;
                    liSubj.Selected = true;
                    ddlSubject_SelectedIndexChanged(null, null);
                }
                //所属课程
                ListItem liCous = this.ddlCourse.Items.FindByValue(mm.Cou_ID.ToString());
                if (liCous != null)
                {
                    ddlCourse.SelectedIndex = -1;
                    liCous.Selected = true;
                    ddlCourse_SelectedIndexChanged(null, null);
                }                
                //难度
                ListItem liDiff = ddlDiff.Items.FindByValue(mm.Tp_Diff2.ToString());
                if (liDiff != null)
                {
                    ddlDiff.SelectedIndex = -1;
                    liDiff.Selected = true;
                }
                //时长，总分，及格分
                tbSpan.Text = mm.Tp_Span.ToString();
                tbTotal.Text = mm.Tp_Total.ToString();
                tbPassScore.Text = mm.Tp_PassScore.ToString(); 
            }
            else
            {
                //如果是新增
                mm = new Song.Entities.TestPaper();
                ViewState["UID"] = WeiSha.Common.Request.UniqueID();
                //当处在课程管理中时
                if (couid > 0)
                {
                    Song.Entities.Course cour = Business.Do<ICourse>().CourseSingle(couid);
                    if (cour != null)
                    {
                        //所属专业
                        ListItem liSubj = ddlSubject.Items.FindByValue(cour.Sbj_ID.ToString());
                        if (liSubj != null)
                        {
                            ddlSubject.SelectedIndex = -1;
                            liSubj.Selected = true;
                            ddlSubject_SelectedIndexChanged(null, null);
                        }
                        //所属课程
                        ListItem liCous = this.ddlCourse.Items.FindByValue(cour.Cou_ID.ToString());
                        if (liCous != null)
                        {
                            ddlCourse.SelectedIndex = -1;
                            liCous.Selected = true;
                        }                        
                    }
                }
                ddlCourse_SelectedIndexChanged(null, null);
            }
            tbName.Text = mm.Tp_Name;   //标题            
            tbSubName.Text = mm.Tp_SubName; //副标题
            tbAuthor.Text = mm.Tp_Author;   //出卷人
            tbIntro.Text = mm.Tp_Intro; //试卷简介
            tbRemind.Text = mm.Tp_Remind;   //注意事项
            //试卷Logo图片
            if (!string.IsNullOrEmpty(mm.Tp_Logo) && mm.Tp_Logo.Trim() != "")
            {
                this.imgShow.Src = Upload.Get[_uppath].Virtual + mm.Tp_Logo;
            }
            //试题范围
            ListItem liFrom = this.rblFromType.Items.FindByValue(mm.Tp_FromType.ToString());
            if (liFrom != null)
            {
                rblFromType.SelectedIndex = -1;
                liFrom.Selected = true;
            }            
            //各项题型分值
            _fillItemForAll(mm); //按课程出题
            _fileItemForOutline(mm);    //按章节出题
        }
        /// <summary>
        /// 填充“当按课程出题时”的各项题题分值
        /// </summary>
        /// <param name="uid"></param>
        private void _fillItemForAll(Song.Entities.TestPaper tp)
        {
            Song.Entities.TestPaperItem[] tpi = Business.Do<ITestPaper>().GetItemForAll(tp);
            for (int i = 0; i < this.rptItemForAll.Items.Count; i++)
            {
                //当前索引行的试题类型
                Song.Entities.TestPaperItem pi = null;
                foreach (Song.Entities.TestPaperItem p in tpi)
                    if (p.TPI_Type == i + 1) pi = p;
                if (pi == null) continue;
                //几道题
                TextBox tbCount = (TextBox)this.rptItemForAll.Items[i].FindControl("tbItemCount");
                //占多少分数比
                TextBox tbScore = (TextBox)this.rptItemForAll.Items[i].FindControl("tbItemScore");
                //占多少分
                Label lbNumber = (Label)this.rptItemForAll.Items[i].FindControl("lbItemNumber");
                TextBox tbNumber = (TextBox)this.rptItemForAll.Items[i].FindControl("tbItemNumber");
                tbCount.Text = pi.TPI_Count.ToString();
                lbNumber.Text = tbNumber.Text = pi.TPI_Number.ToString();
                tbScore.Text = pi.TPI_Percent.ToString();
            } 
        }
        /// <summary>
        ///  填充“当按章节出题时”的各项题题分值
        /// </summary>
        /// <param name="tp"></param>
        private void _fileItemForOutline(Song.Entities.TestPaper tp)
        {
            //各题型的占比
            Song.Entities.TestPaperItem[] tpi = Business.Do<ITestPaper>().GetItemForOlPercent(tp);
            if (tpi == null) return;
            for (int i = 0; i < this.rptOutlineScore.Items.Count; i++)
            {
                //当前索引行的试题类型
                Song.Entities.TestPaperItem pi = null;
                foreach (Song.Entities.TestPaperItem p in tpi)
                    if (p.TPI_Type == i + 1) pi = p;
                if (pi == null) continue;
                //占多少分数比
                TextBox tbScore = (TextBox)this.rptOutlineScore.Items[i].FindControl("tbQuesScore");
                tbScore.Text = pi.TPI_Percent > 0 ? pi.TPI_Percent.ToString() : "";
            }
            //各章节的各题型试题数量
             for (int i = 0; i < this.rptOutline.Items.Count; i++)
            {
                Label lbOlid = (Label)this.rptOutline.Items[i].FindControl("lbOlid");   //章节id控件   
                int olid = Convert.ToInt32(lbOlid.Text);    //章节id 
                //内嵌repeat，用于显示题型
                Repeater rptItems = (Repeater)this.rptOutline.Items[i].FindControl("rtpOutlineItem");
                Song.Entities.TestPaperItem[] tpiol = Business.Do<ITestPaper>().GetItemForOlCount(tp, olid);    //当前章节的各试题抽取数量
                for (int j = 0; j < rptItems.Items.Count; j++)
                {
                    //当前索引行的试题类型
                    Song.Entities.TestPaperItem pi = null;
                    foreach (Song.Entities.TestPaperItem p in tpiol)
                        if (p.TPI_Type == j + 1) pi = p;
                    if (pi == null) continue;
                    //几道题
                    TextBox tbCount = (TextBox)rptItems.Items[j].FindControl("tbQuesCount");
                    tbCount.Text = pi.TPI_Count > 0 ? pi.TPI_Count.ToString() : "";
                }
            }
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.TestPaper mm = id != 0 ? Business.Do<ITestPaper>().PagerSingle(id) : new Song.Entities.TestPaper();
            //试卷类型，为2时表示：随机抽题
            mm.Tp_Type = 2;
            mm.Tp_UID = getUID();   //UID
            mm.Tp_Name = tbName.Text.Trim();    //试卷名称
            mm.Tp_SubName = tbSubName.Text.Trim();  //试卷副标题
            mm.Tp_IsUse = cbIsUse.Checked;  //是否使用
            mm.Tp_IsRec = cbIsRec.Checked;  //是否推荐            
            mm.Sbj_ID = Convert.ToInt32(ddlSubject.SelectedValue);//专业id
            mm.Sbj_Name = ddlSubject.SelectedText;//专业名称
            int couid = 0;  //课程ID
            int.TryParse(ddlCourse.SelectedValue, out couid);
            mm.Cou_ID = couid;  //设置课程id            
            mm.Tp_Diff = mm.Tp_Diff2 = Convert.ToInt32(ddlDiff.SelectedValue);    //难易度
            mm.Tp_Span = Convert.ToInt32(tbSpan.Text);  //考试限时时长
            mm.Tp_Total = Convert.ToInt32(tbTotal.Text);    //考试总分
            mm.Tp_PassScore = Convert.ToInt32(tbPassScore.Text);   //及极分            
            mm.Tp_Intro = tbIntro.Text.Trim();      //简介
            mm.Tp_Remind = tbRemind.Text.Trim();    //注意事项       
            mm.Tp_FromType = Convert.ToInt16(rblFromType.SelectedValue);    //抽题范围，0为当前课程，1为按章节
            //图片上传
            if (fuLoad.PostedFile.FileName != "")
            {
                try
                {
                    fuLoad.UpPath = _uppath;    //上传路径
                    fuLoad.IsMakeSmall = false; //是否生成缩略图，此为不生成
                    fuLoad.IsConvertJpg = true; //是否转换为Jpg格式，此为转换
                    fuLoad.SaveAndDeleteOld(mm.Tp_Logo);    //上传图片，并同步删除旧图片
                    fuLoad.File.Server.ChangeSize(200, 200, false);     //更改图片大小
                    mm.Tp_Logo = fuLoad.File.Server.FileName;
                    imgShow.Src = fuLoad.File.Server.VirtualPath;
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message);
                }
            }
            int tpcount = 0;
            //出题范围的配置项
            mm.Tp_FromConfig = _buildItemXML(mm.Tp_FromType, out tpcount);
            mm.Tp_Count = tpcount;
            //确定操作
            try
            {
                if (id == 0)
                    id = Business.Do<ITestPaper>().PagerAdd(mm);
                else
                    Business.Do<ITestPaper>().PagerSave(mm);
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }

        /// <summary>
        /// 构建试题选择范围的配置信息
        /// </summary>
        /// <param name="fromtype">出卷类型，0为当前课程，1为按章节</param>
        /// <param name="quscount">输出总题量</param>
        /// <returns></returns>
        private string _buildItemXML(int fromtype,out int quscount)
        {
            quscount = 0;
            int qcount0 = 0, qcount1 = 0;
            string xml = "";
            xml += "<Config>";
            //按课程出题
            xml += "<All><Items>";
            xml += _buildXMLForAll(out qcount0);   
              xml += "</Items></All>";
            //按章节出题
            xml += "<Outline>";
            xml += "<Percent>";
            xml += _buildXMLForOutlinePercent();
            xml += "</Percent>";
            xml += "<Items>";
            xml += _buildXMLForOutlineItem(out qcount1);
            xml += "</Items>";
            xml += "</Outline>";
            xml += "</Config>";
            if (fromtype == 0) quscount = qcount0;
            if (fromtype == 1) quscount = qcount1;
            return xml;
        }
        /// <summary>
        /// 返回按课程出题的配置信息
        /// </summary>
        /// <returns></returns>
        private string _buildXMLForAll(out int quscount)
        {
            quscount = 0;
            string xml = "";
            for (int i = 0; i < this.rptItemForAll.Items.Count; i++)
            {
                Song.Entities.TestPaperItem pi = new TestPaperItem();
                //几道题
                TextBox tbCount = (TextBox)this.rptItemForAll.Items[i].FindControl("tbItemCount");
                //占多少分数比
                TextBox tbScore = (TextBox)this.rptItemForAll.Items[i].FindControl("tbItemScore");
                //占多少分               
                TextBox tbNumber = (TextBox)this.rptItemForAll.Items[i].FindControl("tbItemNumber");
                pi.TPI_Count = Convert.ToInt32(tbCount.Text.Trim() == "" ? "0" : tbCount.Text);
                quscount += pi.TPI_Count;
                pi.TPI_Percent = Convert.ToInt32(tbScore.Text.Trim() == "" ? "0" : tbScore.Text);
                pi.TPI_Number = Convert.ToInt32(tbNumber.Text.Trim() == "" ? "0" : tbNumber.Text);
                pi.TPI_Type = i + 1;
                xml += pi.ToXML();
            }
            return xml;
        }
        /// <summary>
        /// 按章节出题的型占比信息
        /// </summary>
        /// <returns></returns>
        private string _buildXMLForOutlinePercent()
        {
            string xml = "";
            double total = Convert.ToDouble(tbTotal.Text);  //试卷总分
            double surplus = total;            //剩余分数，用于计算每项得分，因为百分乘于总分，总会有余数，最后一项为前几项的剩余分
            for (int i = 0; i < this.rptOutlineScore.Items.Count; i++)
            {
                Song.Entities.TestPaperItem pi = new TestPaperItem();
                //占多少分数比
                TextBox tbScore = (TextBox)this.rptOutlineScore.Items[i].FindControl("tbQuesScore");                           
                pi.TPI_Percent = Convert.ToInt32(tbScore.Text.Trim() == "" ? "0" : tbScore.Text);
                //占多少分
                if (i < this.rptOutlineScore.Items.Count - 1)
                {
                    pi.TPI_Number = (int)Math.Floor(pi.TPI_Percent * total / 100);
                    surplus -= pi.TPI_Number;
                }
                else
                {
                    pi.TPI_Number = (int)surplus;
                }                
                pi.TPI_Type = i + 1;
                xml += pi.ToXML();
            }
            return xml;
        }
        /// <summary>
        /// 按章节出题的配置信息
        /// </summary>
        /// <returns></returns>
        private string _buildXMLForOutlineItem(out int quscount)
        {
            quscount = 0;
            string xml = "";
            for (int i = 0; i < this.rptOutline.Items.Count; i++)
            {               
                Label lbOlid = (Label)this.rptOutline.Items[i].FindControl("lbOlid");   //章节id               
                //内嵌repeat，用于显示题型
                Repeater rptItems = (Repeater)this.rptOutline.Items[i].FindControl("rtpOutlineItem");
                for (int j = 0; j < rptItems.Items.Count; j++)
                {
                    Song.Entities.TestPaperItem pi = new TestPaperItem();
                    pi.Ol_ID = Convert.ToInt32(lbOlid.Text);
                    pi.TPI_Type = j + 1;
                    TextBox tbCount = (TextBox)rptItems.Items[j].FindControl("tbQuesCount");    //当前题型占几道题
                    pi.TPI_Count = Convert.ToInt32(tbCount.Text.Trim() == "" ? "0" : tbCount.Text);
                    quscount += pi.TPI_Count;
                    xml += pi.ToXML();
                }
            }
            return xml;
        }
        #endregion



    }
}
