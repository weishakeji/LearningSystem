using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;


namespace Song.Site.Check
{
    public partial class Dbup_20170323 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                int size = WeiSha.Common.Request.Form["size"].Int32 ?? 10;  //每页多少条
                int index = WeiSha.Common.Request.Form["index"].Int32 ?? 1;  //第几页

                int sumcount = 0;
                Song.Entities.TestPaper[] enties;
                enties = Business.Do<ITestPaper>().PaperPager(0, 0, 0, 0, null, null, size, index, out sumcount);
                foreach (Song.Entities.TestPaper c in enties)
                {
                    int fromtype = c.Tp_FromType;
                    TestPaperItem[] tpi = null;
                    if (fromtype == 0)                   
                        tpi = Business.Do<ITestPaper>().GetItemForAll(c);
                    if (fromtype == 1)
                        tpi = Business.Do<ITestPaper>().GetItemForOlPercent(c);
                    //
                    int qcount = 0;
                    foreach (TestPaperItem t in tpi)                    
                        qcount += t.TPI_Count;
                    c.Tp_Count = qcount;
                    //
                    Business.Do<ITestPaper>().PagerSave(c);
                }
                string json = "{'size':'" + size + "','index':'" + index + "','sumcount':'" + sumcount + "'}";
                Response.Write(json);
                Response.End();
            }
        }
    }
}