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
using System.Collections.Generic;
using System.IO;
using WeiSha.Common;
using NPOI.HSSF.UserModel;
using NPOI.SS.Converter;


namespace Song.Site.Manage
{
    public partial class Tester : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            HSSFWorkbook workbook;
            //the excel file to convert
            string fileName = "学员导出.xlsx";
            //fileName = WeiSha.Common.Server.MapPath("~/");
            fileName = Path.Combine(WeiSha.Common.Server.MapPath("~/"), fileName);
            workbook = ExcelToHtmlUtils.LoadXls(fileName);

            ExcelToHtmlConverter excelToHtmlConverter = new ExcelToHtmlConverter();

            //set output parameter
            excelToHtmlConverter.OutputColumnHeaders = false;
            excelToHtmlConverter.OutputHiddenColumns = true;
            excelToHtmlConverter.OutputHiddenRows = true;
            excelToHtmlConverter.OutputLeadingSpacesAsNonBreaking = false;
            excelToHtmlConverter.OutputRowNumbers = true;
            excelToHtmlConverter.UseDivsToSpan = true;

            //process the excel file
            excelToHtmlConverter.ProcessWorkbook(workbook);

            //output the html file
            excelToHtmlConverter.Document.Save(Path.ChangeExtension(fileName, "html"));
        //    string path = this.Request.Url.PathAndQuery;

        //    WeiSha.Common.PageCache cache = WeiSha.Common.PageCache.Get[path];

        //    string ansStr = "";
        //    if (ansStr.EndsWith("、")) 
        //        ansStr = ansStr.Substring(0, ansStr.Length - 1);   
        //    //txt = _replacePath(txt, "body|link|script|img");
        //    //string a = "false";
        //    //bool b=true;
        //    //bool.TryParse(a,out b);
           


        }

    }
}
