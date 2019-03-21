using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using System.Data;
using System.Text.RegularExpressions;


namespace Song.Site.Check
{
    public partial class Dbup_20190321 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //取结束时间
            Regex rxEnd = new Regex(@"\d{4}-\d{2}-\d{2}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex rxName = new Regex(@"(?<=:)(.[^；]*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //
            List<Song.Entities.MoneyAccount> mas = Business.Do<ISystemPara>().ForSql<Song.Entities.MoneyAccount>("select * from moneyaccount where ma_type=1");
            foreach (Song.Entities.MoneyAccount m in mas)
            {
                string info = m.Ma_Info;
                //取课程名称与结束时间
                string endstr = string.Empty, cour = string.Empty;
                int acid = m.Ac_ID; //学员Id
                MatchCollection matcheEnd = rxEnd.Matches(info);
                if (matcheEnd.Count > 0) endstr = matcheEnd[0].Value;
                MatchCollection matcheName = rxName.Matches(info);
                if (matcheName.Count > 0) cour = matcheName[0].Value;
                //取当前课程
                Song.Entities.Course course = Business.Do<ISystemPara>().ScalarSql<Song.Entities.Course>("select * from Course where cou_name='"+cour+"'");
                if (course!=null)
                {
                    Student_Course sc = Business.Do<ISystemPara>()
                        .ScalarSql<Student_Course>(string.Format("select * from student_course where ac_id={0} and cou_id={1}", acid, course.Cou_ID));
                    if (sc!=null)
                    {
                        DateTime end = DateTime.Now;    //资金流水中记录的过期时间
                        DateTime.TryParse(endstr, out end);
                        if (end > sc.Stc_EndTime)
                        {
                            sc.Stc_EndTime = end;
                            Business.Do<ISystemPara>().ExecuteSql(string.Format("update student_course set stc_endtime='{0}' where ac_id={1} and cou_id={2}",
                                endstr, acid, course.Cou_ID));
                        }

                    }
                }
            }
            //
            this.Response.Write(this.Button1.Text + "---操作完成！");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            List<Song.Entities.LearningCard> lcs = Business.Do<ISystemPara>().ForSql<Song.Entities.LearningCard>("select * from LearningCard where Lc_State=1");
            foreach (LearningCard lc in lcs)
            {
                int acid =lc.Ac_ID; //学员Id
                int span = lc.Lc_Span; //要增加的学习时间
                //获取学习卡关联的课程
                Song.Entities.LearningCardSet lset = Business.Do<ILearningCard>().SetSingle(lc.Lcs_ID);
                Song.Entities.Course[] cours = Business.Do<ILearningCard>().CoursesGet(lset);
                foreach (Song.Entities.Course c in cours)
                {
                    Student_Course sc = Business.Do<ISystemPara>()
                        .ScalarSql<Student_Course>(string.Format("select * from student_course where ac_id={0} and cou_id={1}", acid, c.Cou_ID));
                    if (sc != null)
                    {
                        DateTime end = sc.Stc_StartTime.AddDays(span);  //应该结束的学习时效
                        if (end > sc.Stc_EndTime)
                        {
                            sc.Stc_EndTime = end;
                            Business.Do<ISystemPara>().ExecuteSql(string.Format("update student_course set stc_endtime='{0}' where ac_id={1} and cou_id={2}",
                                end.ToString("yyyy-MM-dd"), acid, c.Cou_ID));
                        }
                    }
                }
            }
            this.Response.Write(this.Button2.Text + "---操作完成！");
        }        
    }
}