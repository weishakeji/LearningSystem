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
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace Song.Site.Manage.Course
{
    public partial class Outline_Export : Extend.CustomPage
    {
        //来自get参数的课程id
        int couid_get = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Song.Entities.Course cur = Business.Do<ICourse>().CourseSingle(couid_get);
            if (cur != null) lbCourName.Text = cur.Cou_Name;
        }
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.Course cur = Business.Do<ICourse>().CourseSingle(couid_get);
            //导出
            string filename = cur.Cou_Name + "(章节导出)_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm") + ".xls";   //创建文件
            string filePath = Upload.Get["Temp"].Physics + filename;
            filePath = Business.Do<IOutline>().OutlineExport4Excel(filePath, couid_get);
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
    }
}
