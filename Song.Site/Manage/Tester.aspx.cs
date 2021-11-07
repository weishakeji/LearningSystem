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
using Song.ServiceInterfaces;
using System.Text.RegularExpressions;
using pili_sdk.pili;


namespace Song.Site.Manage
{
    public partial class Tester : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            List<Song.Entities.Course> cour = Business.Do<ICourse>().CourseAll(-1, -1, -1, true);

            Song.Entities.Course[] arr = cour.ToArray();


            object value = cour;
            //Type type = value.GetType();
            //Array array = (Array)value;
            //for (int i = 0; i < array.Length; i++)
            //{
            //    int ii = i;
            //    object o = array.GetValue(i);

            //    Song.Entities.Course c = (Song.Entities.Course)o;
            //}

            System.Collections.IList list = (System.Collections.IList)value;
            for (int i = 0; i < list.Count; i++)
            {
                object o = list[i];
                Song.Entities.Course c = (Song.Entities.Course)o;
            }
        }

    }
}
