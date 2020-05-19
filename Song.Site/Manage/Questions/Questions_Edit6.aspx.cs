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
using System.Text.RegularExpressions;

namespace Song.Site.Manage.Questions
{
    public partial class Questions_Edit6 : Extend.CustomPage
    {
        //试题id
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //试题类型
        private int type = WeiSha.Common.Request.QueryString["type"].Int32 ?? 0;
        //课程id
        private int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //题型分类汉字名称
        protected string[] typeStr = App.Get["QuesType"].Split(',');
        Song.Entities.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            //当前机构
            org = Business.Do<IOrganization>().OrganCurrent();
            this.SortSelect1.CourseChange += CourseChange;
            if (!this.IsPostBack)
            {
                InitBind();
                fill();
                CourseChange(null, null);
            }
            
        }
        /// <summary>
        /// 界面的初始绑定
        /// </summary>
        private void InitBind()
        {
            SortSelect1.InitBind();
            if (couid > 0)
            {
                Song.Entities.Course cour = Business.Do<ICourse>().CourseSingle(couid);
                if (cour != null)
                {
                    SortSelect1.SbjID = cour.Sbj_ID;
                    SortSelect1.CouID = couid;
                }
            }                      
            //题型
            ddlType.DataSource = typeStr;
            ddlType.DataBind();
            if (type > 0) ddlType.SelectedIndex = type - 1;
            
        }
        /// <summary>
        /// 当课程变更时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CourseChange(object sender, EventArgs e)
        {
            ddlKnlSort.Items.Clear();
            //资料分类
            Song.Entities.KnowledgeSort[] nc = Business.Do<IKnowledge>().GetSortAll(org.Org_ID, this.SortSelect1.CouID, true);
            this.ddlKnlSort.DataSource = nc;
            this.ddlKnlSort.DataTextField = "Kns_Name";
            this.ddlKnlSort.DataValueField = "Kns_Id";
            this.ddlKnlSort.DataBind();
            ddlKnlSort.Items.Insert(0, new ListItem("--全部--", "0"));
        }
        void fill()
        {            
            Song.Entities.Questions mm;
            if (id != 0)
            {
                mm = Business.Do<IQuestions>().QuesSingle(id, false);
                cbIsUse.Checked = mm.Qus_IsUse;
                //唯一标识
                ViewState["UID"] = mm.Qus_UID;
                //所属专业、课程、章节
                SortSelect1.SbjID = mm.Sbj_ID;
                SortSelect1.CouID = mm.Cou_ID;
                SortSelect1.OlID = mm.Ol_ID;
                //难度
                ListItem liDiff = ddlDiff.Items.FindByValue(mm.Qus_Diff.ToString());
                if (liDiff != null)
                {
                    ddlDiff.SelectedIndex = -1;
                    liDiff.Selected = true;
                }
                //相关资料
                if (mm.Kn_ID > 0)
                {
                    Song.Entities.Knowledge kn = Business.Do<IKnowledge>().KnowledgeSingle((int)mm.Kn_ID);
                    if (kn != null)
                    {
                        ListItem liKns = ddlKnlSort.Items.FindByValue(kn.Kns_ID.ToString());
                        if (liKns != null) liKns.Selected = true;
                        knTitle.InnerText = kn.Kn_Title;
                        tbKnTit.Text = kn.Kn_Title;
                        tbKnID.Text = kn.Kn_ID.ToString();
                    }
                }
                //错误信息
                ltErrorInfo.Text = mm.Qus_ErrorInfo;
                errorInfo.Visible = mm.Qus_IsError;
                //错误提告
                ltWrongInfo.Text = mm.Qus_WrongInfo;
                wrongInfo.Visible = mm.Qus_IsWrong;
            }
            else
            {
                //如果是新增
                mm = new Song.Entities.Questions();
                ViewState["UID"] = WeiSha.Common.Request.UniqueID();
                tbKnID.Text = "0";
            }
            //题干
            tbTitle.Text = mm.Qus_Title;
            //讲解
            tbExplan.Text = mm.Qus_Explain;
            //排序号
            tbTax.Text = mm.Qus_Tax.ToString();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            //Song.Entities.Questions mm = id > 0 ? Business.Do<IQuestions>().QuesSingle(id) : mm = new Song.Entities.Questions();
            ////题型、学科、题干
            //mm.Qus_Type = this.type == 0 ? ddlType.SelectedIndex + 1 : this.type;
            //mm.Qus_IsUse = cbIsUse.Checked;
            //mm.Sbj_ID = SortSelect1.SbjID;
            //mm.Sbj_Name = SortSelect1.SbjName;
            //mm.Cou_ID = SortSelect1.CouID;
            //mm.Ol_ID = SortSelect1.OlID;
            ////排序号
            //int tax = 0;
            //int.TryParse(tbTax.Text, out tax);
            //mm.Qus_Tax = tax;
            ////
            //mm.Qus_Title = tbTitle.Text.Trim();
            //mm.Qus_Title = tranTxt(mm.Qus_Title);
            ////难度
            //mm.Qus_Diff = Convert.ToInt32(ddlDiff.SelectedItem.Value);
            ////资料、讲解
            //if (tbKnID.Text != string.Empty)
            //{
            //    mm.Kn_ID = Convert.ToInt32(tbKnID.Text);
            //}
            //mm.Qus_Explain = tbExplan.Text.Trim();
            ////UID
            //mm.Qus_UID = getUID();
            ////是否处理报错信息
            //if (cbWrong.Checked) mm.Qus_IsWrong = false;           
            
            ////确定操作
            //try
            //{
            //    if (id == 0)
            //        id = Business.Do<IQuestions>().QuesAdd(mm);
            //    else
            //        Business.Do<IQuestions>().QuesSave(mm);                 
            //    Master.AlertCloseAndRefresh("操作成功！");
            //}
            //catch (Exception ex)
            //{
            //    Master.Alert(ex.Message);
            //}
        }
    }
}
