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
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using System.Drawing;
using System.Reflection;
using System.Xml.Serialization;
using Spring.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Xml;
using System.Threading;

namespace Song.Site.Manage
{
    public partial class Tester : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
           
            //ThreadStart threadStart = new ThreadStart(Calculate);//通过ThreadStart委托告诉子线程执行什么方法　　
           
            //Thread thread = new Thread(threadStart);
            
            //thread.Start();//启动新线程
            
            
        }
        public static void Calculate()
        {
            //string path = WeiSha.Common.App.Get["Static"].MapPath + "Questions";
            //if (!System.IO.Directory.Exists(path))
            //{
            //    System.IO.Directory.CreateDirectory(path);
            //}
            //Song.Entities.Questions[] ques = Business.Do<IQuestions>().QuesCount(-1, null, -1);
            //foreach (Song.Entities.Questions q in ques)
            //{
            //    System.IO.File.WriteAllText(path + "\\" + q.Qus_ID + ".json", q.ToJson(), Encoding.UTF8);
            //}
        }
        public static string LegalName(string filename)
        {
            string s1 = "\\/:*?<>|\"";
            string s2 = "＼／：★？〖〗｜＂";
            for (int i = 0; i < s1.Length; i++)
            {
                string t = s1.Substring(i, 1);
                string s = s2.Substring(i, 1);
                filename = filename.Replace(t, s);
            }
            return filename;
        }
        
    }
}
