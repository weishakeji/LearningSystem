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
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Xml;
using System.Collections.Generic;
using System.Data.OleDb;
using WeiSha.Common;


namespace Song.Site.Manage.Utility
{
    public partial class ExcelInput : System.Web.UI.UserControl
    {
        #region 属性，事件
        public event EventHandler Input;
        //文档上传后的临时存放路径
        private string _tempPathConfig = "Temp";
        //Excel链接字符串
        string connStr = "Provider=Microsoft.Ace.OleDb.12.0;data source={0};Extended Properties='Excel 12.0; HDR=NO; IMEX=1'";
        /// <summary>
        /// 数据模板的名称
        /// </summary>
        public string TemplateName
        {
            get
            {
                object obj = ViewState["TemplateName"];
                return (obj == null) ? "" : obj.ToString();
            }
            set
            {
                ViewState["TemplateName"] = value;
            }
        }
        /// <summary>
        /// 数据模板的路径
        /// </summary>
        public string TemplatePath
        {
            get
            {
                object obj = ViewState["TemplatePath"];
                return (obj == null) ? "" : obj.ToString();
            }
            set
            {
                ViewState["TemplatePath"] = value;
            }
        }
        /// <summary>
        /// 记录Excel列名与数据库字段对应关系的配置文件的文件名
        /// </summary>
        public string Config
        {
            get
            {
                object obj = ViewState["Config"];
                return (obj == null) ? "" : obj.ToString();
            }
            set
            {
                ViewState["Config"] = value;
            }
        }
        public List<string> Keys
        {
            get
            {
                return GetKeys();
            }
        }
        private DataTable _sheetData;
        /// <summary>
        /// 当前需要导入的工作薄的数据集
        /// </summary>
        public DataTable SheetDataTable
        {
            get { return _sheetData; }
            set { _sheetData = value; }
        }
        private List<DataRow> _errorDataRow = new List<DataRow>();
        /// <summary>
        /// 导入失败的数据集
        /// </summary>
        public List<DataRow> ErrorDataRows
        {
            get { return _errorDataRow; }
        }
        private Dictionary<String, String> _dataRelation;
        /// <summary>
        /// Excel列名与数据库字段对应关系，前者为Excel列名(key值），后者为数据库字段名（value值）
        /// </summary>
        public Dictionary<String, String> DataRelation
        {
            get { return _dataRelation; }
            set { _dataRelation = value; }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //下载模板
                linkDataTmp.NavigateUrl = this.ResolveUrl(this.TemplatePath);
                linkDataTmp.Text = linkDataTmp.Text.Replace("{0}", this.TemplateName);
            }
        }

        /// <summary>
        /// 添加导入失败的行数
        /// </summary>
        /// <param name="dr"></param>
        public void AddError(DataRow dr)
        {
            _errorDataRow.Add(dr);
        }
        /// <summary>
        /// 第一步：上传数据文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (fuLoad.PostedFile.FileName != "")
                {
                    fuLoad.UpPath = _tempPathConfig;
                    fuLoad.IsMakeSmall = false;
                    fuLoad.IsConvertJpg = false;
                    fuLoad.SaveAs();
                    ViewState["dataFilePath"] = fuLoad.File.Server.FileFullName;
                    //工作簿
                    DataTable table = this.GetSheets(fuLoad.File.Server.FileFullName);
                    dlWorkBook.DataSource = table;
                    dlWorkBook.DataBind();
                    //状态                    
                    ltSheetCount.Text = table.Rows.Count.ToString();
                    //
                    lbState.Text = "正在操作文档 《" + fuLoad.File.Client.FileName + "》";
                    btnNext1.Visible = true;
                    lbFile2.Text = fuLoad.File.Client.FileName;
                    //
                    fdPanel1.Visible = false;
                    fdPanel2.Visible = true;
                    lbError2.Text = "";
                    //如果只有一个工作簿，直接跳到下一步
                    if (table.Rows.Count == 1)
                    {
                        foreach (DataListItem dli in dlWorkBook.Items)
                        {
                            Button lb = (Button)dli.FindControl("btnWorkBook");
                            btnSheet_Click(lb, null);
                            break;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                lbError1.Text = ex.Message;
            }
        }
        /// <summary>
        /// 第二步：工作薄选择的按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSheet_Click(object sender, EventArgs e)
        {
            lbFile3.Text = lbFile2.Text;
            //当前操作的工作簿
            string sheetText = ((Button)sender).Text;
            lbSheet3.Text = "【" + sheetText.Substring(0, sheetText.IndexOf("：")) + "】共" + sheetText.Substring(sheetText.IndexOf("：") + 1);
            //要操作的Excel文件名与工作表名
            string xlsFile = ViewState["dataFilePath"].ToString();
            //工作簿的索引
            int sheetIndex = Convert.ToInt32(((Button)sender).CommandArgument);
            ViewState["sheetIndex"] = sheetIndex;
            try
            {
                //DataTable dtRows = SheetToDatatable(xlsFile, sheetIndex);
                //if (dtRows.Rows.Count < 1) throw new Exception("当前工作簿没有数据");
                //获取工作薄的列
                DataTable dtColumn = this.GetSheetColumn(xlsFile, sheetIndex);
                dlColumn.DataSource = dtColumn;
                dlColumn.DataBind();
                //预置的列与字段的对应关系
                DataTable dtConfig = getConfig();
                //自动识别excel中字段与数据库中字符匹配
                for (int i = 0; i < dlColumn.Items.Count; i++)
                {
                    DataListItem dli = dlColumn.Items[i];
                    //列名
                    Label lb = (Label)dli.FindControl("lbColumn");
                    //绑定对应关系的配置
                    DropDownList ddl = (DropDownList)dli.FindControl("ddlColumnForField");
                    ddl.DataSource = dtConfig;
                    ddl.DataTextField = "Column";
                    ddl.DataValueField = "Field";
                    ddl.DataBind();
                    ddl.Items.Insert(0, new ListItem("", ""));
                    //自动设置对应关系
                    setDataList(lb.Text, ddl);
                    if (i > 30) break;
                }
                fdPanel2.Visible = false;
                fdPanel3.Visible = true;
                lbError3.Text = "";
            }
            catch (Exception ex)
            {
                lbError2.Text = ex.Message;
            }
        }
        /// <summary>
        /// 第三步：导入数据的按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInput_Click(object sender, EventArgs e)
        {
            fdPanel5.Visible = false;
            btnOutpt.Visible = false;
            try
            {
                //实际的列与字段的关系
                this._dataRelation = getColumnForField();
                //工作薄的数据
                string file = ViewState["dataFilePath"].ToString();
                int sheetIndex = Convert.ToInt32(ViewState["sheetIndex"].ToString());
                this._sheetData = this.SheetToDatatable(file, sheetIndex);
                //*****************
                //执行导入数据的事件
                if (Input != null)
                    this.Input(sender, e);
                //
                lbFile4.Text = lbFile3.Text;
                lbSheet4.Text = lbSheet3.Text;
                fdPanel3.Visible = false;
                fdPanel4.Visible = true;
                //导入结果的处理
                lbErrorCount.Text = ErrorDataRows.Count.ToString();
                lbSuccCount.Text = (SheetDataTable.Rows.Count - ErrorDataRows.Count).ToString();
                if (ErrorDataRows.Count > 0)
                {
                    fdPanel5.Visible = true;
                    btnOutpt.Visible = true;
                    //错误的数据
                    DataTable dtErr = SheetDataTable.Clone();
                    foreach (DataRow dr in ErrorDataRows)
                    {
                        dtErr.ImportRow(dr);
                    }
                    gvError.DataSource = dtErr;
                    gvError.DataBind();
                }
            }
            catch (Exception ex)
            {
                lbError3.Text = ex.Message;
            }
        }

        #region 操作Excel的方法
        /// <summary>
        /// 通过Excel文档，创建对应的处理对象
        /// </summary>
        /// <param name="xlsFile"></param>
        /// <returns></returns>
        private IWorkbook createWorkbook(string xlsFile)
        {
            //创建工作薄对象
            IWorkbook workbook = null;
            using (FileStream file = new FileStream(xlsFile, FileMode.Open, FileAccess.Read))
            {               
                //根据扩展名判断excel版本
                string ext = xlsFile.Substring(xlsFile.LastIndexOf(".") + 1);
                //WorkbookFactory.Create(file);
                if (ext.ToLower() == "xls") workbook = new HSSFWorkbook(file);
                if (ext.ToLower() == "xlsx") workbook = new XSSFWorkbook(file);
            }
            return workbook;
        }
        /// <summary>
        /// 从Excel中读取一个工作薄，生成Datatable对象。
        /// </summary>
        /// <param name="xlsFile"></param>
        /// <param name="sheetIndex"></param>
        /// <returns></returns>
        private DataTable SheetToDatatable(string xlsFile, int sheetIndex)
        {
            DataTable dt = new DataTable();
            //创建工作薄对象
            IWorkbook workbook = createWorkbook(xlsFile);
            ISheet sheet = workbook.GetSheetAt(sheetIndex);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            try
            {
                rows.MoveNext();
                //创建Datatable结构
                HSSFRow firsRow = (HSSFRow)rows.Current;
                for (int i = 0; i < firsRow.LastCellNum; i++)
                {
                    ICell cell = firsRow.GetCell(i);
                    if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                    dt.Columns.Add(new DataColumn(cell.ToString(), getColumnType(cell.ToString())));
                }
                //导入工作薄的数据
                while (rows.MoveNext())
                {
                    HSSFRow row = (HSSFRow)rows.Current;
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        ICell cell = row.GetCell(i);
                        if (cell == null) continue;
                        string value = cell.ToString();
                        //读取Excel格式，根据格式读取数据类型
                        switch (dt.Columns[i].DataType.FullName)
                        {
                            case "System.DateTime": //日期类型                                   
                                if (DateUtil.IsValidExcelDate(cell.NumericCellValue))
                                {
                                    try
                                    {
                                        value = cell.DateCellValue.ToString();
                                    }
                                    catch
                                    {
                                        value = cell.ToString();
                                    }
                                }
                                else
                                {
                                    value = cell.NumericCellValue.ToString();
                                }
                                break;

                            default:
                                value = cell.ToString();
                                break;
                        }
                        dr[i] = WeiSha.Common.Param.Method.ConvertToAnyValue.Get(value).ChangeType(dt.Columns[i].DataType);

                    }
                    dt.Rows.Add(dr);
                }
            }
            catch
            {
                return dt;
            }
            return dt;
        }
        /// <summary>
        /// 获取列的数据类型
        /// </summary>
        /// <param name="colname"></param>
        /// <returns></returns>
        private System.Type getColumnType(string colname)
        {
            DataTable dtConfing = getConfig();
            System.Type type=null;
            foreach (DataRow dr in dtConfing.Rows)
            {
                if (colname.ToLower().Trim() == dr["Column"].ToString().ToLower().Trim())
                {
                    if (dr["DataType"].ToString().ToLower().Trim() == "date") type = Type.GetType("System.DateTime");
                }
            }
            if (type == null) type = Type.GetType("System.String");
            return type;
        }
        /// <summary>
        /// 获取文档中的所有工作薄
        /// </summary>
        /// <param name="xlsFile"></param>
        /// <returns></returns>
        private DataTable GetSheets(string xlsFile)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Name"));
            dt.Columns.Add(new DataColumn("Count"));
            //创建工作薄对象
            IWorkbook workbook = createWorkbook(xlsFile);
            int sheetNum = workbook.NumberOfSheets;
            for (int i = 0; i < sheetNum; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Name"] = workbook.GetSheetAt(i).SheetName;
                dr["Count"] = workbook.GetSheetAt(i).LastRowNum;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// 获取工作薄的列表，即第一行的标题
        /// </summary>
        /// <param name="xlsFile"></param>
        /// <param name="sheetIndex"></param>
        /// <returns></returns>
        private DataTable GetSheetColumn(string xlsFile, int sheetIndex)
        {
            DataTable dtSch = new DataTable("SheetStructure");
            dtSch.Columns.Add(new DataColumn("Name", Type.GetType("System.String")));
            //创建工作薄对象
            IWorkbook workbook = createWorkbook(xlsFile);   
            ISheet sheet = workbook.GetSheetAt(sheetIndex);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            rows.MoveNext();
            //创建Datatable结构
            IRow firsRow = (IRow)rows.Current;
            for (int i = 0; i < firsRow.LastCellNum; i++)
            {
                DataRow dr = dtSch.NewRow();
                ICell cell = firsRow.GetCell(i);
                dr["Name"] = cell == null ? "(null)" + i : cell.ToString();
                dtSch.Rows.Add(dr);
            }
            return dtSch;
        }
        #endregion

        #region 处理字段对比的方法
        /// <summary>
        /// 通过字段，获取对应的列名
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public string GetColumnForField(string field)
        {
            Dictionary<String, String> dic = this._dataRelation;
            if (dic.Count < 1) return "";
            foreach (KeyValuePair<String, String> kvp in dic)
            {
                if (kvp.Value == field) return kvp.Key;
            }
            return "";
        }
        /// <summary>
        /// 获取主键
        /// </summary>
        /// <returns></returns>
        public List<string> GetKeys()
        {
            List<string> keys = new List<string>();
            //配置文件的路径
            string path = App.Get["ExcelInputConfig"].VirtualPath;
            string config = this.Server.MapPath(path + this.Config);
            if (!System.IO.File.Exists(config)) return keys;
            //填充表
            XmlDocument resXml = new XmlDocument();
            resXml.Load(config);
            //获取主键
            XmlNode ele = resXml.LastChild;
            if (ele.Attributes["Keys"] == null) return keys;
            string[] keyscol = ele.Attributes["Keys"].Value.Split(',');
            //匹配项的配置数据
            XmlNodeList nodes = resXml.GetElementsByTagName("item");
            //           
            foreach (string k in keyscol)
            {
                foreach (XmlNode n in nodes)
                {
                    string column = ((XmlElement)n).Attributes["Column"].Value;
                    string field = ((XmlElement)n).Attributes["Field"].Value;
                    if (k.Trim().ToLower() == column.Trim().ToLower())
                    {
                        keys.Add(field.Trim());
                    }
                }
            }
            return keys;
        }
        /// <summary>
        /// 通过列名，获取对应的字段
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public string GetFieldForColumn(string column)
        {
            Dictionary<String, String> dic = this._dataRelation;
            if (dic.Count < 1) return "";
            foreach (KeyValuePair<String, String> kvp in dic)
            {
                if (kvp.Key == column) return kvp.Value;
            }
            return "";
        }
        /// <summary>
        /// 获取列名与字段名的对应关系的设置
        /// </summary>
        private DataTable getConfig()
        {
            //构造表
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Column", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Field", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("DataType", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Format", Type.GetType("System.String")));
            //配置文件的路径
            string path = App.Get["ExcelInputConfig"].VirtualPath;
            string config = this.Server.MapPath(path + this.Config);
            if (!System.IO.File.Exists(config)) return dt;
            //填充表
            XmlDocument resXml = new XmlDocument();
            resXml.Load(config);
            XmlNodeList nodes = resXml.GetElementsByTagName("item");
            foreach (XmlNode n in nodes)
            {
                XmlElement el = (XmlElement)n;
                DataRow dr = dt.NewRow();
                dr["Column"] = el.Attributes["Column"].Value;
                dr["Field"] = el.Attributes["Field"].Value;
                dr["DataType"] = el.Attributes["DataType"] != null ? el.Attributes["DataType"].Value : null;
                dr["Format"] = el.Attributes["Format"] != null ? el.Attributes["Format"].Value : null;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// 获取实际的列与字段的关系,Dictionary<String, String> 前者为列名，后者为字段名
        /// </summary>
        /// <returns></returns>
        private Dictionary<String, String> getColumnForField()
        {
            Dictionary<String, String> dic = new Dictionary<string, string>();
            for (int i = 0; i < dlColumn.Items.Count; i++)
            {
                DataListItem dli = dlColumn.Items[i];
                //列名、对应的字段
                Label lb = (Label)dli.FindControl("lbColumn");
                DropDownList ddl = (DropDownList)dli.FindControl("ddlColumnForField");
                if (!dic.ContainsKey(lb.Text))
                    dic.Add(lb.Text, ddl.SelectedValue);
            }
            return dic;
        }
        /// <summary>
        /// 设置下拉选框中的哪个项，为选种状态
        /// </summary>
        /// <param name="label">字段名称（对应数据库说明）</param>
        /// <param name="ddl"></param>
        private void setDataList(string label, DropDownList ddl)
        {
            for (int i = 0; i < ddl.Items.Count; i++)
            {
                if (label == ddl.Items[i].Text)
                {
                    ddl.Items[i].Selected = true;
                    return;
                }

            }
            for (int i = 0; i < ddl.Items.Count; i++)
            {
                if (isExts(label, ddl.Items[i].Text))
                {
                    ddl.Items[i].Selected = true;
                    break;
                }

            }
        }
        /// <summary>
        /// 判断前者字符，是否存在于后者
        /// </summary>
        /// <param name="ea"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool isExts(string ea, string str)
        {
            bool isExt = false;
            if (ea.Trim() == str.Trim()) return true;
            if (str.IndexOf(ea) > -1) isExt = true;
            if (ea.Length > 2)
            {
                for (int i = 0; i <= (ea.Length - 2); i++)
                {
                    string t = ea.Substring(i, 2);
                    if (str.IndexOf(t) > -1)
                    {
                        isExt = true;
                        break;
                    }
                }
            }
            return isExt;
        }
        #endregion

        #region 导航按钮
        protected void btnBack2_Click(object sender, EventArgs e)
        {
            fdPanel1.Visible = true;
            fdPanel2.Visible = false;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            fdPanel2.Visible = true;
            fdPanel3.Visible = false;
        }
        /// <summary>
        /// 继续导入其它的工作簿
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack4_Click(object sender, EventArgs e)
        {
            fdPanel2.Visible = true;
            fdPanel4.Visible = false;
            lbError2.Text = "";
            fdPanel5.Visible = false;
        }
        /// <summary>
        /// 继续导入Excel文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack5_Click(object sender, EventArgs e)
        {
            fdPanel1.Visible = true;
            fdPanel4.Visible = false;
            lbState.Text = "等待上传……";
            btnNext1.Visible = false;
            lbError1.Text = "";
            fdPanel5.Visible = false;
        }


        protected void btnNext1_Click(object sender, EventArgs e)
        {
            fdPanel1.Visible = false;
            fdPanel2.Visible = true;
        }
        #endregion

        #region 导出错误数据
        /// <summary>
        /// 导出错误数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOutpt_Click(object sender, EventArgs e)
        {
            //创建Excel对象
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            //创建工作簿对象
            ISheet sheet = hssfworkbook.CreateSheet(this.TemplateName);
            //sheet.DefaultColumnWidth = 30;
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            //生成表头
            for (int i = 0; i < gvError.HeaderRow.Cells.Count; i++)
            {
                string txt = gvError.HeaderRow.Cells[i].Text;
                txt = txt.Replace("&nbsp;", "");
                rowHead.CreateCell(i).SetCellValue(txt);
            }
            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;
            for (int i = 0; i < gvError.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                GridViewRow gvr = gvError.Rows[i];
                for (int j = 0; j < gvr.Cells.Count; j++)
                {
                    string txt = gvr.Cells[j].Text;
                    if (string.IsNullOrEmpty(txt)) continue;
                    txt = txt.Replace("&nbsp;", "");
                    txt = txt.Replace("\n", "");
                    txt = txt.Replace("\r", "");
                    ICell cell = row.CreateCell(j);
                    cell.SetCellValue(txt.Trim());
                    cell.CellStyle = style_size;
                    if (txt.Length > 20)
                    {
                        cell.Sheet.SetColumnWidth(j, 100 * 256);
                    }
                }

            }
            //创建文件
            string filePath = Upload.Get["Temp"].Physics + WeiSha.Common.Server.LegalName(this.TemplateName + "-导入错误-" + DateTime.Now.ToLongDateString()) + ".xls";
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
            }
        }
        #endregion
    }
}