using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using WeiSha.WebControl;
using Song.ServiceInterfaces;
using Song.Entities;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;

namespace Song.Site.Manage.Course
{
    public partial class Courses_Questions : Extend.CustomPage
    {
        //课程id
        private int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //试题的类型
        int type = WeiSha.Common.Request.QueryString["type"].Int16 ?? -1;
        //题型分类汉字名称
        protected string[] typeStr = App.Get["QuesType"].Split(',');
        //章节列表
        protected Song.Entities.Outline[] outlines = null;
        Song.Entities.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnSear.UniqueID;
            this.Title = "课程试题管理";
            org = Business.Do<IOrganization>().OrganCurrent();
            outlines = Business.Do<IOutline>().OutlineAll(couid, null); //当前课程所有章节
            if (!IsPostBack)
            {
                InitBind();
                BindData(null, null);
            }
        }
        #region 搜索区域
        /// <summary>
        /// 界面的初始绑定
        /// </summary>
        private void InitBind()
        {
            //章节
            Song.Entities.Outline[] outline = Business.Do<IOutline>().OutlineAll(couid, null);
            DataTable dt = WeiSha.WebControl.Tree.ObjectArrayToDataTable.To(outline);
            WeiSha.WebControl.Tree.DataTableTree tree = new WeiSha.WebControl.Tree.DataTableTree();
            tree.IdKeyName = "OL_ID";
            tree.ParentIdKeyName = "OL_PID";
            tree.TaxKeyName = "Ol_Tax";
            tree.Root = 0;
            dt = tree.BuilderTree(dt);
            ddlOutline.DataSource = dt;
            ddlOutline.DataTextField = "Ol_Name";
            ddlOutline.DataValueField = "OL_ID";
            ddlOutline.DataBind();
            ddlOutline.Items.Insert(0,new ListItem(" -- 章节 -- ", "-1"));
            //试题类型
            for (int i = 0; i < typeStr.Length; i++)
                ddlType.Items.Add(new ListItem(typeStr[i], (i + 1).ToString()));
            if (type > 0)
            {
                ListItem liDdlType = ddlType.Items.FindByValue(type.ToString());
                if (liDdlType != null) liDdlType.Selected = true;
            }
            this.SearchBind(this.searchBox);
        }
        #endregion
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //总记录数
            int count = 0;
            //学科，课程，章节
            int olid = 0;           
            int.TryParse(ddlOutline.SelectedValue, out olid);
            //难易度
            int diff = Convert.ToInt32(ddlDiff.SelectedItem.Value);
            //试题类型
            int.TryParse(ddlType.SelectedValue, out type);
            Song.Entities.Questions[] eas = null;
            eas = Business.Do<IQuestions>().QuesPager(org.Org_ID, type, -1, couid, olid, null, null, null, diff, this.tbSear.Text, Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = eas;
            //去除题干中的html标签
            string regexstr = @"(<[^>]*>)|\r|\n|\s";
            foreach (Song.Entities.Questions q in eas)
            {
                if (string.IsNullOrWhiteSpace(q.Qus_Title)) continue;
                q.Qus_Title = Regex.Replace(q.Qus_Title, regexstr, string.Empty, RegexOptions.IgnoreCase);
            }
            GridView1.DataKeyNames = new string[] { "Qus_ID" };
            GridView1.DataBind();
            Pager1.RecordAmount = count;
        }
        /// <summary>
        /// 获取章节名称
        /// </summary>
        /// <param name="olid"></param>
        /// <returns></returns>
        protected string getOutlineName(int olid)
        {
            string olname = "";
            foreach (Song.Entities.Outline o in outlines)
            {
                if (o.Ol_ID == olid)
                {
                    olname = o.Ol_Name;
                    break;
                }
            }
            return olname;
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Qurey = this.SearchQuery(this.searchBox);
            Pager1.Index = 1;
            ////BindData(null, null);
            //string tm = this.Search(this.searchBox);
            //this.Response.Write(tm);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteEvent(object sender, EventArgs e)
        {
            string keys = GridView1.GetKeyValues;
            Business.Do<IQuestions>().QuesDelete(keys);
            BindData(null, null);
        }
        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, ImageClickEventArgs e)
        {
            WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
            int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            Business.Do<IQuestions>().QuesDelete(id);
            BindData(null, null);
        }
        /// <summary>
        /// 修改是否显示的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUse_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.Questions entity = Business.Do<IQuestions>().QuesSingle(id);
            entity.Qus_IsUse = !entity.Qus_IsUse;
            Business.Do<IQuestions>().QuesSave(entity);
            BindData(null, null);

        }

        protected void lbUse_Click(object sender, EventArgs e)
        {
            string keys = GridView1.GetKeyValues;
            foreach (string id in keys.Split(','))
            {
                if (string.IsNullOrEmpty(id)) continue;
                int tmid = Convert.ToInt16(id);
                Song.Entities.Questions entity = Business.Do<IQuestions>().QuesSingle(tmid);
                entity.Qus_IsUse = true;
                Business.Do<IQuestions>().QuesSave(entity);
            }
            BindData(null, null);

        }
        protected void lbNoUse_Click(object sender, EventArgs e)
        {
            try
            {
                string keys = GridView1.GetKeyValues;
                foreach (string id in keys.Split(','))
                {
                    if (string.IsNullOrEmpty(id)) continue;
                    int tmid = Convert.ToInt16(id);
                    Song.Entities.Questions entity = Business.Do<IQuestions>().QuesSingle(tmid);
                    entity.Qus_IsUse = false;
                    Business.Do<IQuestions>().QuesSave(entity);
                }
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }

        #region 导出数据

        protected void Output_Click(object sender, EventArgs e)
        {

            BindData(null, null);
            Song.Entities.Questions[] ques = null;
            if (GridView1.DataSource is Song.Entities.Questions[])
            {
                ques = (Song.Entities.Questions[])GridView1.DataSource;
            }
            else
            {
                Message.ExceptionShow(new Exception("绑定数据类型错误")); return;
            }
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            if (type == 1) buildExcelSql_1(hssfworkbook, ques);
            if (type == 2) buildExcelSql_2(hssfworkbook, ques);
            if (type == 3) buildExcelSql_3(hssfworkbook, ques);
            if (type == 4) buildExcelSql_4(hssfworkbook, ques);
            if (type == 5) buildExcelSql_5(hssfworkbook, ques);

            //创建文件
            string name = App.Get["QuesType"].Split(',')[type - 1];
            string filename = name.Trim() + "题_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm") + ".xls";
            string filePath = Upload.Get["Temp"].Physics + WeiSha.Common.Server.LegalName(filename);
            FileStream file = new FileStream(filePath, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            if (System.IO.File.Exists(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileInfo.Name));
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.AddHeader("Content-Transfer-Encoding", "binary");
                Response.ContentType = "application/-excel";
                Response.ContentEncoding = System.Text.Encoding.Default;
                Response.WriteFile(fileInfo.FullName);
                Response.Flush();
                Response.End();
                File.Delete(filePath);
            }
        }
        //单选题导出
        private void buildExcelSql_1(HSSFWorkbook hssfworkbook, Song.Entities.Questions[] ques)
        {
            //创建工作簿对象
            ISheet sheet = hssfworkbook.CreateSheet("单选题");
            //sheet.DefaultColumnWidth = 30;
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            //创建表头
            rowHead.CreateCell(0).SetCellValue("ID");
            rowHead.CreateCell(1).SetCellValue("题干");
            rowHead.CreateCell(2).SetCellValue("专业");
            rowHead.CreateCell(3).SetCellValue("难度");
            rowHead.CreateCell(4).SetCellValue("答案选项1");
            rowHead.CreateCell(5).SetCellValue("答案选项2");
            rowHead.CreateCell(6).SetCellValue("答案选项3");
            rowHead.CreateCell(7).SetCellValue("答案选项4");
            rowHead.CreateCell(8).SetCellValue("答案选项5");
            rowHead.CreateCell(9).SetCellValue("答案选项6");
            rowHead.CreateCell(10).SetCellValue("正确答案");
            rowHead.CreateCell(11).SetCellValue("试题讲解");
            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;
            int i = 0;
            foreach (Song.Entities.Questions q in ques)
            {
                IRow row = sheet.CreateRow(i + 1);
                string tmpVal = "";
                QuesAnswer[] qas = Business.Do<IQuestions>().QuestionsAnswer(q, null);
                int ansIndex = 0;
                for (int j = 0; j < qas.Length; j++)
                {
                    QuesAnswer c = qas[j];
                    tmpVal = c.Ans_Context;
                    row.CreateCell(4 + j).SetCellValue(tmpVal);
                    if (c.Ans_IsCorrect)
                        ansIndex = j + 1;
                }
                row.CreateCell(0).SetCellValue(q.Qus_ID);
                row.CreateCell(1).SetCellValue(q.Qus_Title);
                row.CreateCell(2).SetCellValue(q.Sbj_Name);
                row.CreateCell(3).SetCellValue((int)q.Qus_Diff);
                row.CreateCell(10).SetCellValue(ansIndex.ToString());
                row.CreateCell(11).SetCellValue(q.Qus_Explain);
                i++;
            }
        }
        //多选题导出
        private void buildExcelSql_2(HSSFWorkbook hssfworkbook, Song.Entities.Questions[] ques)
        {
            //创建工作簿对象
            ISheet sheet = hssfworkbook.CreateSheet("多选题");
            //sheet.DefaultColumnWidth = 30;
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            //创建表头
            rowHead.CreateCell(0).SetCellValue("ID");
            rowHead.CreateCell(1).SetCellValue("题干");
            rowHead.CreateCell(2).SetCellValue("专业");
            rowHead.CreateCell(3).SetCellValue("难度");
            rowHead.CreateCell(4).SetCellValue("答案选项1");
            rowHead.CreateCell(5).SetCellValue("答案选项2");
            rowHead.CreateCell(6).SetCellValue("答案选项3");
            rowHead.CreateCell(7).SetCellValue("答案选项4");
            rowHead.CreateCell(8).SetCellValue("答案选项5");
            rowHead.CreateCell(9).SetCellValue("答案选项6");
            rowHead.CreateCell(10).SetCellValue("正确答案");
            rowHead.CreateCell(11).SetCellValue("试题讲解");
            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;
            int i = 0;
            foreach (Song.Entities.Questions q in ques)
            {
                IRow row = sheet.CreateRow(i + 1);
                string tmpVal = "";
                QuesAnswer[] qas = Business.Do<IQuestions>().QuestionsAnswer(q, null);
                string ansIndex = "";
                for (int j = 0; j < qas.Length; j++)
                {
                    QuesAnswer c = qas[j];
                    tmpVal = c.Ans_Context;
                    row.CreateCell(4 + j).SetCellValue(tmpVal);
                    if (c.Ans_IsCorrect)
                        ansIndex += Convert.ToString(j + 1) + ",";
                }
                if (ansIndex.Length > 0)
                {
                    if (ansIndex.Substring(ansIndex.Length - 1) == ",")
                        ansIndex = ansIndex.Substring(0, ansIndex.Length - 1);
                }

                row.CreateCell(0).SetCellValue(q.Qus_ID);
                row.CreateCell(1).SetCellValue(q.Qus_Title);
                row.CreateCell(2).SetCellValue(q.Sbj_Name);
                row.CreateCell(3).SetCellValue((int)q.Qus_Diff);
                row.CreateCell(10).SetCellValue(ansIndex.ToString());
                row.CreateCell(11).SetCellValue(q.Qus_Explain);
                i++;
            }
        }
        //判断题导出
        private void buildExcelSql_3(HSSFWorkbook hssfworkbook, Song.Entities.Questions[] ques)
        {
            //创建工作簿对象
            ISheet sheet = hssfworkbook.CreateSheet("判断题");
            //sheet.DefaultColumnWidth = 30;
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            //创建表头
            rowHead.CreateCell(0).SetCellValue("ID");
            rowHead.CreateCell(1).SetCellValue("题干");
            rowHead.CreateCell(2).SetCellValue("专业");
            rowHead.CreateCell(3).SetCellValue("难度");
            rowHead.CreateCell(4).SetCellValue("答案");
            rowHead.CreateCell(5).SetCellValue("试题讲解");
            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;
            int i = 0;
            foreach (Song.Entities.Questions q in ques)
            {
                string ans = "";
                if (Convert.ToString(q.Qus_IsCorrect) == "False") { ans = "错误"; } else { ans = "正确"; }
                IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(q.Qus_ID);
                row.CreateCell(1).SetCellValue(q.Qus_Title);
                row.CreateCell(2).SetCellValue(q.Sbj_Name);
                row.CreateCell(3).SetCellValue((int)q.Qus_Diff);
                row.CreateCell(4).SetCellValue(ans);
                row.CreateCell(5).SetCellValue(q.Qus_Explain);
                i++;
            }
        }
        //简答题导出
        private void buildExcelSql_4(HSSFWorkbook hssfworkbook, Song.Entities.Questions[] ques)
        {
            //创建工作簿对象
            ISheet sheet = hssfworkbook.CreateSheet("简答题");
            //sheet.DefaultColumnWidth = 30;
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            //创建表头
            rowHead.CreateCell(0).SetCellValue("ID");
            rowHead.CreateCell(1).SetCellValue("题干");
            rowHead.CreateCell(2).SetCellValue("专业");
            rowHead.CreateCell(3).SetCellValue("难度");
            rowHead.CreateCell(4).SetCellValue("答案");
            rowHead.CreateCell(5).SetCellValue("试题讲解");
            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;
            int i = 0;
            foreach (Song.Entities.Questions q in ques)
            {
                IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(q.Qus_ID);
                row.CreateCell(1).SetCellValue(q.Qus_Title);
                row.CreateCell(2).SetCellValue(q.Sbj_Name);
                row.CreateCell(3).SetCellValue((int)q.Qus_Diff);
                row.CreateCell(4).SetCellValue(q.Qus_Answer);
                row.CreateCell(5).SetCellValue(q.Qus_Explain);
                i++;
            }
        }
        //填空题导出
        private void buildExcelSql_5(HSSFWorkbook hssfworkbook, Song.Entities.Questions[] ques)
        {
            //创建工作簿对象
            ISheet sheet = hssfworkbook.CreateSheet("填空题");
            //sheet.DefaultColumnWidth = 30;
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            //创建表头
            rowHead.CreateCell(0).SetCellValue("ID");
            rowHead.CreateCell(1).SetCellValue("题干");
            rowHead.CreateCell(2).SetCellValue("专业");
            rowHead.CreateCell(3).SetCellValue("难度");
            rowHead.CreateCell(4).SetCellValue("答案选项1");
            rowHead.CreateCell(5).SetCellValue("答案选项2");
            rowHead.CreateCell(6).SetCellValue("答案选项3");
            rowHead.CreateCell(7).SetCellValue("答案选项4");
            rowHead.CreateCell(8).SetCellValue("答案选项5");
            rowHead.CreateCell(9).SetCellValue("答案选项6");
            rowHead.CreateCell(10).SetCellValue("试题讲解");
            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;
            int i = 0;
            foreach (Song.Entities.Questions q in ques)
            {
                IRow row = sheet.CreateRow(i + 1);
                string tmpVal = "";
                QuesAnswer[] qas = Business.Do<IQuestions>().QuestionsAnswer(q, null);
                for (int j = 0; j < qas.Length; j++)
                {
                    QuesAnswer c = qas[j];
                    tmpVal = c.Ans_Context;
                    row.CreateCell(4 + j).SetCellValue(tmpVal);
                }

                row.CreateCell(0).SetCellValue(q.Qus_ID);
                row.CreateCell(1).SetCellValue(q.Qus_Title);
                row.CreateCell(2).SetCellValue(q.Sbj_Name);
                row.CreateCell(3).SetCellValue((int)q.Qus_Diff);
                row.CreateCell(10).SetCellValue(q.Qus_Explain);
                i++;
            }
        }
        #endregion

       

        

    }
}