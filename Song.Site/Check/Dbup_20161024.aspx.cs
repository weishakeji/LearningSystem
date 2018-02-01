using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using System.Data.SqlClient;
namespace Song.Site.Check
{
    public partial class Dbup_20161024 : System.Web.UI.Page
    {        
        //机构id
        int orgid = WeiSha.Common.Request.QueryString["orgid"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (index > 1)
            //{
            //    btnDataTras_Click(null, null);
            //}
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string action = WeiSha.Common.Request.Form["action"].String;
                switch (action)
                {
                    case "btnStruct":
                        btnStruct_Click(null, null);
                        break;
                    case "btnDataTras":
                        btnDataTras_Click(null, null);
                        break;
                }               
            }
        }
        /// <summary>
        /// 调整试题的数据库结构
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnStruct_Click(object sender, EventArgs e)
        {
            string msg = "";
            try
            {
                string sql = @"alter table [Questions] ALTER COLUMN  [Qus_Explain] [nvarchar](max) NULL
                        alter table [Questions] add Qus_Items [nvarchar](max) NULL
                        alter table [Questions] add Qus_IsTitle [bit] NULL
                        update [Questions] set Qus_IsTitle=1
                        alter table [Questions] ALTER COLUMN Qus_IsTitle [bit]  NOT NULL
                        alter table [QuesAnswer] ALTER COLUMN Qus_ID [int]  NOT NULL                        
                        alter table QuesAnswer DROP COLUMN Org_ID
                        alter table QuesAnswer DROP COLUMN Org_Name
                        alter table [Questions] add [Ol_Name] [nvarchar](500) NULL
                        ";
                foreach (string s in sql.Split('\r'))
                {
                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        Business.Do<ISystemPara>().ExecuteSql(s.Trim());
                    }
                }
                msg = "1";                
            }
            catch (Exception ex)
            {
                string exmsg = ex.InnerException.Message;
                exmsg = exmsg.Replace("'", "\"");
                msg = "错误：" + exmsg + "；      \n原因：可能您已经升级过了。";
            }
            Response.Write(msg);
            Response.End();
            
        }
        /// <summary>
        /// 整理数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDataTras_Click(object sender, EventArgs e)
        {
            int size = WeiSha.Common.Request.Form["size"].Int32 ?? 10;  //每页多少条
            int index = WeiSha.Common.Request.Form["index"].Int32 ?? 1;  //第几页
            int orgid = WeiSha.Common.Request.Form["orgid"].Int32 ?? 0;  //机构id
            int sumcount = 0; //共计多少条记录
            if (orgid < 1)
            {
                Song.Entities.Organization org = null;
                org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            Song.Entities.Questions[] ques = Business.Do<IQuestions>()
                .QuesPager(orgid, -1, null, null, size, index, out sumcount);
            //处理中************
            foreach (Song.Entities.Questions q in ques)
            {
                //读取当前试题的选项
                string sql = string.Format("select * from QuesAnswer where Qus_UID='{0}'", q.Qus_UID);
                List<Song.Entities.QuesAnswer> ans = Business.Do<ISystemPara>().ForSql<Song.Entities.QuesAnswer>(sql);
                //将选项转换为xml字符串，保存到ques_items字段
                string str = Business.Do<IQuestions>().AnswerToItems(ans.ToArray<Song.Entities.QuesAnswer>());
                if (!string.IsNullOrWhiteSpace(str))
                {
                    q.Qus_Items = str;
                    Business.Do<IQuestions>().QuesSave(q);
                    Business.Do<ISystemPara>().ExecuteSql(string.Format("DELETE FROM [QuesAnswer] WHERE Qus_UID='{0}'", q.Qus_UID));
                }
            }
            string json = "{\"size\":" + size + ",\"index\":" + index + ",\"sumcount\":" + sumcount + ",\"orgid\":" + orgid + "}";
            Response.Write(json);
            Response.End();
        }


    }
}