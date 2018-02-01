using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;


namespace Song.Site.Check
{
    public partial class Dbup_20170301 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                int size = WeiSha.Common.Request.Form["size"].Int32 ?? 10;  //每页多少条
                int index = WeiSha.Common.Request.Form["index"].Int32 ?? 1;  //第几页

                int sumcount = 0;
                List<Song.Entities.Course> courses;
                courses = Business.Do<ICourse>().CoursePager(0, 0, true, null, size, index, out sumcount);
                foreach (Song.Entities.Course c in courses)
                {
                    Business.Do<ICourse>().PriceSetCourse(c.Cou_UID);
                }
                string json = "{'size':'" + size + "','index':'" + index + "','sumcount':'" + sumcount + "'}";
                Response.Write(json);
                Response.End();
            }
        }
    }
}