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
using System.Data.OleDb;
using System.IO;
using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace Song.Site.Manage.Questions
{
    public partial class List : Extend.CustomPage
    {
        //���������
        int type = WeiSha.Common.Request.QueryString["type"].Int16 ?? -1;
        //���ͷ��຺������
        protected string[] typeStr = App.Get["QuesType"].Split(',');
        Song.Entities.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {            
            this.Form.DefaultButton = this.btnSear.UniqueID;
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!IsPostBack)
            {
                InitBind();
                BindData(null, null);
            }
        }
        #region ��������
        /// <summary>
        /// ����ĳ�ʼ��
        /// </summary>
        private void InitBind()
        {
            //רҵ
            Song.Entities.Subject[] subjects = Business.Do<ISubject>().SubjectCount(org.Org_ID, null, true, -1, -1);
            ddlSubject.DataSource = subjects;
            this.ddlSubject.DataTextField = "Sbj_Name";
            this.ddlSubject.DataValueField = "Sbj_ID";
            this.ddlSubject.Root = 0;
            this.ddlSubject.DataBind();
            ddlSubject.Items.Insert(0, new ListItem("-רҵ-", "0"));  
            //��������
            for (int i = 0; i < typeStr.Length; i++)
                ddlType.Items.Add(new ListItem(typeStr[i], (i + 1).ToString()));
            if (type > 0)
            {
                ListItem liDdlType = ddlType.Items.FindByValue(type.ToString());
                if (liDdlType != null) liDdlType.Selected = true;
            }
            //            
            ddlSubject_SelectedIndexChanged(null, null);
            //this.SearchBind(this.searchBox);
        }
        /// <summary>
        /// רҵѡ�����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCourse.Items.Clear();
            int sbjid;
            int.TryParse(ddlSubject.SelectedValue, out sbjid);
            if (sbjid > 0)
            {
                //�ϼ�
                List<Song.Entities.Course> cous = Business.Do<ICourse>().CourseCount(org.Org_ID, sbjid, null, true, -1);
                ddlCourse.DataSource = cous;
                this.ddlCourse.DataTextField = "Cou_Name";
                this.ddlCourse.DataValueField = "Cou_ID";
                this.ddlCourse.Root = 0;
                this.ddlCourse.DataBind();
            }
            ddlCourse.Items.Insert(0, new ListItem("-�γ�-", "0"));            
            this.SearchBind(this.searchBox);
            ddlCourse_SelectedIndexChanged(null, null);
        }
        /// <summary>
        /// �γ�ѡ�������¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        { 
            BindData(null, null);
        }
        #endregion
        /// <summary>
        /// ���б�
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {            
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //�ܼ�¼��
            int count = 0;
            //ѧ�ƣ��γ̣��½�
            int sbjid = 0, couid = 0, olid = 0;
            int.TryParse(ddlSubject.SelectedValue, out sbjid);
            int.TryParse(ddlCourse.SelectedValue, out couid);
            //int.TryParse(ddlOutline.SelectedValue, out olid);
            //��������
            int.TryParse(ddlType.SelectedValue, out type);
            //�Ƿ����
            bool? isError = ddlErrorState.SelectedItem.Value == "-1" ? null : (bool?)(ddlErrorState.SelectedItem.Value == "2" ? true : false);
            //�Ƿ����˷�������
            bool? isWroing = ddlWrongState.SelectedItem.Value == "-1" ? null : (bool?)(ddlWrongState.SelectedItem.Value == "2" ? true : false);
            //�Ƿ�����
            bool? isUse = ddlUseState.SelectedItem.Value == "-1" ? null : (bool?)(ddlUseState.SelectedItem.Value == "1" ? true : false);
            Song.Entities.Questions[] eas = null;
            eas = Business.Do<IQuestions>().QuesPager(org.Org_ID, type, sbjid, couid, olid, isUse, isError, isWroing, -1, this.tbSear.Text, Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = eas;
            //ȥ������е�html��ǩ
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
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Qurey = this.SearchQuery(this.searchBox);
            Pager1.Index = 1;
            ////BindData(null, null);
            //string tm = this.Search(this.searchBox);
            //this.Response.Write(tm);
        }
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteEvent(object sender, EventArgs e)
        {
            string keys = GridView1.GetKeyValues;            
            Business.Do<IQuestions>().QuesDelete(keys);
            Business.Do<IQuestions>().OnSave(null, EventArgs.Empty);
            BindData(null, null); 
        }
        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, ImageClickEventArgs e)
        {            
            WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
            int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
            int id = 0;
            int.TryParse(this.GridView1.DataKeys[index].Value.ToString(), out  id);
            Business.Do<IQuestions>().QuesDelete(id);
            Business.Do<IQuestions>().OnSave(null, EventArgs.Empty);
            BindData(null, null);            
        }
        /// <summary>
        /// �޸��Ƿ���ʾ��״̬
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

        #region ��������

        protected void Output_Click(object sender, EventArgs e)
        {

                BindData(null, null);
                Song.Entities.Questions[] ques = null ;
                if (GridView1.DataSource is Song.Entities.Questions[])
                {
                    ques = (Song.Entities.Questions[])GridView1.DataSource;
                }
                else
                {
                    Message.ExceptionShow(new Exception("���������ʹ���")); return;
                }
                HSSFWorkbook hssfworkbook = new HSSFWorkbook();
                if (type == 1) buildExcelSql_1(hssfworkbook, ques);
                if (type == 2) buildExcelSql_2(hssfworkbook, ques);
                if (type == 3) buildExcelSql_3(hssfworkbook, ques);
                if (type == 4) buildExcelSql_4(hssfworkbook, ques);
                if (type == 5) buildExcelSql_5(hssfworkbook, ques);
            
                //�����ļ�
                string name = App.Get["QuesType"].Split(',')[type - 1];
                string filename = name.Trim() + "��_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm") + ".xls";
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
        //��ѡ�⵼��
        private void buildExcelSql_1(HSSFWorkbook hssfworkbook,  Song.Entities.Questions[] ques)
        {
            //��������������
            ISheet sheet = hssfworkbook.CreateSheet("��ѡ��");
            //sheet.DefaultColumnWidth = 30;
            //���������ж���
            IRow rowHead = sheet.CreateRow(0);
            //������ͷ
            rowHead.CreateCell(0).SetCellValue("ID");
            rowHead.CreateCell(1).SetCellValue("���");
            rowHead.CreateCell(2).SetCellValue("רҵ");
            rowHead.CreateCell(3).SetCellValue("�Ѷ�");
            rowHead.CreateCell(4).SetCellValue("��ѡ��1");
            rowHead.CreateCell(5).SetCellValue("��ѡ��2");
            rowHead.CreateCell(6).SetCellValue("��ѡ��3");
            rowHead.CreateCell(7).SetCellValue("��ѡ��4");
            rowHead.CreateCell(8).SetCellValue("��ѡ��5");
            rowHead.CreateCell(9).SetCellValue("��ѡ��6");
            rowHead.CreateCell(10).SetCellValue("��ȷ��");
            rowHead.CreateCell(11).SetCellValue("���⽲��");
            //����������
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
         //��ѡ�⵼��
        private void buildExcelSql_2(HSSFWorkbook hssfworkbook, Song.Entities.Questions[] ques)
        {
            //��������������
            ISheet sheet = hssfworkbook.CreateSheet("��ѡ��");
            //sheet.DefaultColumnWidth = 30;
            //���������ж���
            IRow rowHead = sheet.CreateRow(0);
            //������ͷ
            rowHead.CreateCell(0).SetCellValue("ID");
            rowHead.CreateCell(1).SetCellValue("���");
            rowHead.CreateCell(2).SetCellValue("רҵ");
            rowHead.CreateCell(3).SetCellValue("�Ѷ�");
            rowHead.CreateCell(4).SetCellValue("��ѡ��1");
            rowHead.CreateCell(5).SetCellValue("��ѡ��2");
            rowHead.CreateCell(6).SetCellValue("��ѡ��3");
            rowHead.CreateCell(7).SetCellValue("��ѡ��4");
            rowHead.CreateCell(8).SetCellValue("��ѡ��5");
            rowHead.CreateCell(9).SetCellValue("��ѡ��6");
            rowHead.CreateCell(10).SetCellValue("��ȷ��");
            rowHead.CreateCell(11).SetCellValue("���⽲��");
            //����������
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
         //�ж��⵼��
        private void buildExcelSql_3(HSSFWorkbook hssfworkbook, Song.Entities.Questions[] ques)
        {
            //��������������
            ISheet sheet = hssfworkbook.CreateSheet("�ж���");
            //sheet.DefaultColumnWidth = 30;
            //���������ж���
            IRow rowHead = sheet.CreateRow(0);
            //������ͷ
            rowHead.CreateCell(0).SetCellValue("ID");
            rowHead.CreateCell(1).SetCellValue("���");
            rowHead.CreateCell(2).SetCellValue("רҵ");
            rowHead.CreateCell(3).SetCellValue("�Ѷ�");
            rowHead.CreateCell(4).SetCellValue("��");
            rowHead.CreateCell(5).SetCellValue("���⽲��");
            //����������
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;
            int i = 0;
            foreach (Song.Entities.Questions q in ques)
            {
                string ans = "";
                if (Convert.ToString(q.Qus_IsCorrect) == "False") { ans = "����"; } else { ans = "��ȷ"; }
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
         //����⵼��
        private void buildExcelSql_4(HSSFWorkbook hssfworkbook, Song.Entities.Questions[] ques)
        {
            //��������������
            ISheet sheet = hssfworkbook.CreateSheet("�����");
            //sheet.DefaultColumnWidth = 30;
            //���������ж���
            IRow rowHead = sheet.CreateRow(0);
            //������ͷ
            rowHead.CreateCell(0).SetCellValue("ID");
            rowHead.CreateCell(1).SetCellValue("���");
            rowHead.CreateCell(2).SetCellValue("רҵ");
            rowHead.CreateCell(3).SetCellValue("�Ѷ�");
            rowHead.CreateCell(4).SetCellValue("��");
            rowHead.CreateCell(5).SetCellValue("���⽲��");
            //����������
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
         //����⵼��
        private void buildExcelSql_5(HSSFWorkbook hssfworkbook, Song.Entities.Questions[] ques)
        {
            //��������������
            ISheet sheet = hssfworkbook.CreateSheet("�����");
            //sheet.DefaultColumnWidth = 30;
            //���������ж���
            IRow rowHead = sheet.CreateRow(0);
            //������ͷ
            rowHead.CreateCell(0).SetCellValue("ID");
            rowHead.CreateCell(1).SetCellValue("���");
            rowHead.CreateCell(2).SetCellValue("רҵ");
            rowHead.CreateCell(3).SetCellValue("�Ѷ�");
            rowHead.CreateCell(4).SetCellValue("��ѡ��1");
            rowHead.CreateCell(5).SetCellValue("��ѡ��2");
            rowHead.CreateCell(6).SetCellValue("��ѡ��3");
            rowHead.CreateCell(7).SetCellValue("��ѡ��4");
            rowHead.CreateCell(8).SetCellValue("��ѡ��5");
            rowHead.CreateCell(9).SetCellValue("��ѡ��6");
            rowHead.CreateCell(10).SetCellValue("���⽲��");
            //����������
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
