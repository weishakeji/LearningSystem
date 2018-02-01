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

namespace Song.Site.Manage.Exam
{
    public partial class Test_Review : Extend.CustomPage
    {
        int trid = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        int tpid = WeiSha.Common.Request.QueryString["tpid"].Int32 ?? 0;  
        //当前试卷
        protected Song.Entities.TestPaper pager = null;
        //当前考生
        protected Song.Entities.Accounts st = null;      
        //考试成绩
        protected Song.Entities.TestResults exr = null;
        //唯一值
        protected string uid = WeiSha.Common.Request.UniqueID();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                exr = Business.Do<ITestPaper>().ResultsSingle(trid);
                if (exr == null) return;
                st = Business.Do<IAccounts>().AccountsSingle(exr.Ac_ID);
                string resPath = Upload.Get["Student"].Virtual;
                st.Ac_Photo = resPath + st.Ac_Photo;                
                //当前试卷
                pager = Business.Do<ITestPaper>().PagerSingle((int)exr.Tp_Id);                
            }
        } 
    }
}
