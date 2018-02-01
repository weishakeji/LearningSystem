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
using System.Collections.Generic;

namespace Song.Site.Manage.Exam
{
    public partial class SelfExam : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData(null, null);
            }
        }

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            Song.Entities.Accounts st = Extend.LoginState.Accounts.CurrentUser;
            //准备开始的考试
            DateTime start = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            DateTime end = start.AddDays(1);
            List<Song.Entities.Examination> eas = Business.Do<IExamination>().GetSelfExam(st.Ac_ID, start, end);
            rptExamStart.DataSource = eas;
            rptExamStart.DataBind();
            //近期要开展的考试
            start = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddDays(1);
            rtpExamList.DataSource = Business.Do<IExamination>().GetSelfExam(st.Ac_ID, start, null);
            rtpExamList.DataBind();
           
        }    
      
     
        /// <summary>
        /// 获取参考人员类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected string getGroupType(string gtype,string uid)
        {
            //try
            //{
            //    int type = Convert.ToInt32(gtype);
            //    if (type == 1) return "全体员工";
            //    if (type == 2)
            //    {
            //        Song.Entities.Depart[] deps = Business.Do<IExamination>().GroupForDepart(uid);
            //        string strDep = "（院系）";
            //        for (int i = 0; i < deps.Length; i++)
            //        {
            //            strDep += deps[i].Dep_CnName;
            //            if (i < deps.Length - 1) strDep += ",";
            //        }
            //        return strDep;
            //    }
            //    if (type == 3)
            //    {
            //        Song.Entities.Team[] teams = Business.Do<IExamination>().GroupForTeam(uid);
            //        string strTeam = "（班组）";
            //        for (int i = 0; i < teams.Length; i++)
            //        {
            //            strTeam += teams[i].Team_Name;
            //            if (i < teams.Length - 1) strTeam += ",";
            //        }
            //        return strTeam;
            //    }
            //    return "";
            //}
            //catch (Exception ex)
            //{
            //    Message.ExceptionShow(ex);
            //    return "";
            //}
            return "";
        }
    }
}
