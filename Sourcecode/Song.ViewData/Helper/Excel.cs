using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Song.ViewData.Helper
{
    public class Excel
    {
        #region 读取或解析Excel文件
        /// <summary>
        /// 从Excel中读取一个工作薄，生成Datatable对象。
        /// </summary>
        /// <param name="xlsFile">excel文件的物理地址</param>
        /// <param name="sheetIndex">工作簿索引</param>
        /// <param name="cfgFile">配置文件</param>
        /// <returns></returns>
        public static DataTable SheetToDatatable(string xlsFile, int sheetIndex, string cfgFile)
        {
            //创建工作薄对象
            IWorkbook workbook = createWorkbook(xlsFile);
            //判断是xls还是xlsx
            string ext = Path.GetExtension(xlsFile).ToLower();

            ISheet sheet = workbook.GetSheetAt(sheetIndex);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            DataTable dt = new DataTable();
            //配置信息
            DataTable dtConfig = Config(cfgFile);
            try
            {
                rows.MoveNext();
                //创建Datatable结构
                IRow firsRow = createRow(ext, rows.Current);
                for (int i = 0; i < firsRow.LastCellNum; i++)
                {
                    ICell cell = firsRow.GetCell(i);
                    if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                    dt.Columns.Add(new DataColumn(cell.ToString(), getColumnType(cell.ToString(), dtConfig)));
                }
                //导入工作薄的数据
                while (rows.MoveNext())
                {
                    IRow row = createRow(ext, rows.Current);
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        ICell cell = row.GetCell(i);
                        if (cell == null) continue;
                        string value = string.Empty;
                        //读取Excel格式，根据格式读取数据类型
                        switch (dt.Columns[i].DataType.FullName)
                        {
                            case "System.DateTime": //日期类型
                                try
                                {
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
                                }
                                catch
                                {
                                    value = cell.ToString();
                                }
                                object obj = WeiSha.Core.Param.Method.ConvertToAnyValue.Get(value).ChangeType(dt.Columns[i].DataType);
                                dr[i] = obj == null ? DateTime.Now : (System.DateTime)obj;
                                break;

                            default:
                                value = getCellValue(cell, ext, workbook);
                                dr[i] = WeiSha.Core.Param.Method.ConvertToAnyValue.Get(value).ChangeType(dt.Columns[i].DataType);
                                break;
                        }

                    }
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                return dt;
            }
            return dt;
        }
        /// <summary>
        /// 获取Excel单元格的值
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="ext">excel的扩展名，用于判断是xls还是xlsx</param>
        /// <param name="workbook">文档对象</param>
        /// <returns></returns>
        private static string getCellValue(ICell cell, string ext, IWorkbook workbook)
        {
            string val = string.Empty;
            switch (cell.CellType)
            {
                case CellType.Numeric:
                    if (HSSFDateUtil.IsCellDateFormatted(cell))//日期类型
                    {
                        val = cell.DateCellValue.ToString("yyyy-MM-dd");
                    }
                    else//其他数字类型
                    {
                        //val = cell.NumericCellValue.ToString();
                        val = cell.ToString();
                    }
                    break;
                case CellType.Blank:
                    val = string.Empty;
                    break;
                case CellType.Formula:   //此处是处理公式数据，获取公式执行后的值
                    if (ext == ".xlsx")
                    {
                        XSSFFormulaEvaluator eva = new XSSFFormulaEvaluator(workbook);
                        if (eva.Evaluate(cell).CellType == CellType.Numeric)
                        {
                            val = eva.Evaluate(cell).NumberValue.ToString();
                        }
                        else
                        {
                            val = eva.Evaluate(cell).StringValue;
                        }
                    }
                    else
                    {
                        HSSFFormulaEvaluator eva = new HSSFFormulaEvaluator(workbook);
                        if (eva.Evaluate(cell).CellType == CellType.Numeric)
                        {
                            val = eva.Evaluate(cell).NumberValue.ToString();
                        }
                        else
                        {
                            val = eva.Evaluate(cell).StringValue;
                        }
                    }
                    break;
                default:
                    val = cell.StringCellValue;
                    break;
            }
            return string.IsNullOrWhiteSpace(val) ? val : val.Trim();
        }
        /// <summary>
        /// 获取文档中的所有工作薄
        /// </summary>
        /// <param name="xlsFile"></param>
        /// <returns>name:工作簿名称; count:记录数</returns>
        public static JArray Sheets(string xlsFile)
        {
            JArray arr = new JArray();
            //创建工作薄对象
            IWorkbook workbook = createWorkbook(xlsFile);
            int sheetNum = workbook.NumberOfSheets;
            for (int i = 0; i < sheetNum; i++)
            {
                JObject jo = new JObject();
                jo.Add("name", workbook.GetSheetAt(i).SheetName);
                jo.Add("count", workbook.GetSheetAt(i).LastRowNum);
                arr.Add(jo);
            }
            return arr;
        }
        /// <summary>
        /// 获取工作薄的列表，即第一行的标题
        /// </summary>
        /// <param name="xlsFile">excel的物理地址</param>
        /// <param name="sheetIndex">工作簿的索引</param>
        /// <returns>name:工作簿名称;index:工作簿索引;count:记录数;columns:列名 </returns>
        public static JObject Columns(string xlsFile, int sheetIndex)
        {
            //excel文档对象
            IWorkbook workbook = createWorkbook(xlsFile);
            //工作簿对象
            ISheet sheet = workbook.GetSheetAt(sheetIndex);
            JObject jo = new JObject();
            jo.Add("name", sheet.SheetName);
            jo.Add("index", sheetIndex);
            jo.Add("count", sheet.LastRowNum);

            JArray arr = new JArray();
            //获取列
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            rows.MoveNext();
            IRow firsRow = (IRow)rows.Current;
            for (int i = 0; i < firsRow.LastCellNum; i++)
            {
                ICell cell = firsRow.GetCell(i);
                JObject o = new JObject();
                o.Add("Name", cell == null ? "(null)" + i : cell.ToString());
                arr.Add(o);
            }
            jo.Add("columns", arr);
            return jo;
        }
        /// <summary>
        /// 通过Excel文档，创建对应的处理对象
        /// </summary>
        /// <param name="xlsFile"></param>
        /// <returns></returns>
        public static IWorkbook createWorkbook(string xlsFile)
        {
            //创建工作薄对象
            IWorkbook workbook = null;
            using (FileStream file = new FileStream(xlsFile, FileMode.Open, FileAccess.Read))
            {
                //根据扩展名判断excel版本
                string ext = Path.GetExtension(xlsFile).ToLower();
                if (ext == ".xls") workbook = new HSSFWorkbook(file);
                if (ext == ".xlsx") workbook = new XSSFWorkbook(file);
            }
            return workbook;
        }
        /// <summary>
        /// 创建行对象
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static IRow createRow(string ext, object obj)
        {
            IRow row = null;
            if (ext == ".xls") row = (HSSFRow)obj;
            if (ext == ".xlsx") row = (XSSFRow)obj;
            return row;
        }
        /// <summary>
        /// 获取列名与字段名的对应关系的设置
        /// </summary>
        /// <param name="file">配置文件的路径(绝对路径）</param>
        public static DataTable Config(string file)
        {
            //构造表
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Column", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Field", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("DataType", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Format", Type.GetType("System.String")));
            //配置文件的路径
            //string path = App.Get["ExcelInputConfig"].VirtualPath;
            string config = WeiSha.Core.Server.MapPath(file);
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
        /// 获取列的数据类型
        /// </summary>
        /// <param name="colname"></param>
        /// <param name="dtConfig">配置信息</param>
        /// <returns></returns>
        private static System.Type getColumnType(string colname, DataTable dtConfig)
        {
            System.Type type = null;
            foreach (DataRow dr in dtConfig.Rows)
            {
                if (colname.ToLower().Trim() == dr["Column"].ToString().ToLower().Trim())
                {
                    if (dr["DataType"].ToString().ToLower().Trim() == "date") type = Type.GetType("System.DateTime");
                }
            }
            if (type == null) type = Type.GetType("System.String");
            return type;
        }
        #endregion

        #region 删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="folder">文件所在的文件夹</param>
        /// <param name="uploadkey">上传文件所在的根文件夹，此处为配置项的Key值，配置项来自web.config的Upload节点</param>
        public static bool DeleteFile(string file, string folder, string uploadkey)
        {
            //校验文件
            if (string.IsNullOrWhiteSpace(file)) return false;
            file = WeiSha.Core.Server.LegalName(file);
            if (string.IsNullOrWhiteSpace(file)) return false;
            //上传的根文件夹
            string rootPhy = WeiSha.Core.Upload.Get[uploadkey].Physics;
            //校验文件夹
            if (!string.IsNullOrWhiteSpace(folder)) rootPhy += WeiSha.Core.Server.LegalPath(folder);
            if (!rootPhy.EndsWith("\\")) rootPhy += "\\";
            if (!Directory.Exists(rootPhy)) return false;
            //删除文件
            string filePath = rootPhy + file;
            if (!File.Exists(filePath)) return false;
            File.Delete(filePath);
            return true;
        }
        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="folder">文件所在的文件夹</param>
        /// <param name="uploadkey">上传文件所在的根文件夹，此处为配置项的Key值，配置项来自web.config的Upload节点</param>
        /// <param name="rule">按文件名查询时的规则</param>
        /// <returns></returns>
        public static JArray Files(string folder, string uploadkey, string rule)
        {
            //上传的根文件夹
            string rootPhy = WeiSha.Core.Upload.Get[uploadkey].Physics;     //物理路径
            string rootVir = WeiSha.Core.Upload.Get[uploadkey].Virtual;     //虚拟路径
            //校验文件夹
            if (!string.IsNullOrWhiteSpace(folder))
            {
                folder = WeiSha.Core.Server.LegalPath(folder);
                rootPhy += folder;
                rootVir += folder;
                if (!rootPhy.EndsWith("\\")) rootPhy += "\\";
                if (!rootVir.EndsWith("/")) rootVir += "/";
            }
            //
            JArray jarr = new JArray();
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(rootPhy);
            if (!dir.Exists) return jarr;
            //
            if (string.IsNullOrWhiteSpace(rule)) rule = "*.xls";
            FileInfo[] files = dir.GetFiles(rule).OrderByDescending(f => f.CreationTime).ToArray();
            foreach (FileInfo f in files)
            {
                string name = Path.GetFileNameWithoutExtension(f.Name);
                JObject jo = new JObject();
                jo.Add("name", name);
                jo.Add("file", f.Name);
                jo.Add("url", rootVir + f.Name);
                jo.Add("date", f.CreationTime);
                jo.Add("size", f.Length);
                jarr.Add(jo);
            }
            return jarr;
        }
        #endregion
    }
}
