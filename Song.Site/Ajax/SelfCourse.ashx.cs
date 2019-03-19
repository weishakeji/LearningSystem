using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Extend;

namespace Song.Site.Ajax
{
    /// <summary>
    /// 我的课程
    /// </summary>
    public class SelfCourse : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //此页面的ajax提交，全部采用了POST方式
            if (context.Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string action = WeiSha.Common.Request.Form["action"].String;
                //如果未登录
                if (!Extend.LoginState.Accounts.IsLogin)
                {
                    string tm = "\"sum\":{0}";
                    context.Response.Write("{" + string.Format(tm, 0, 0) + "}");
                }
                else
                {
                    //学员登录id
                    int stid = Extend.LoginState.Accounts.UserID;
                    switch (action)
                    {
                        case "buycou":
                            List<Song.Entities.Course> buyCou = Business.Do<ICourse>().CourseForStudent(stid, null, 1, false, -1);
                            context.Response.Write(_toJosn(buyCou, action, stid));
                            break;
                        case "overcou":
                            List<Song.Entities.Course> overCou = Business.Do<ICourse>().CourseForStudent(stid, null, 2, false, -1);
                            context.Response.Write(_toJosn(overCou, action, stid));
                            break;
                        case "trycou":
                            List<Song.Entities.Course> trycou = Business.Do<ICourse>().CourseForStudent(stid, null, -1, true, -1);
                            context.Response.Write(_toJosn(trycou, action, stid));
                            break;
                    }
                }
                context.Response.End();
            }
        }
        /// <summary>
        /// 生成json
        /// </summary>
        protected string _toJosn(List<Song.Entities.Course> courses, string action,int stid)
        {
            string tm = "{\"sum\":" + courses.Count + ",\"action\":\"" + action + "\",\"object\":[";
            for (int i = 0; i < courses.Count; i++)
            {
                Song.Entities.Course c = courses[i];
                c.Cou_LogoSmall = Upload.Get["Course"].Virtual + c.Cou_LogoSmall;
                c.Cou_Logo = Upload.Get["Course"].Virtual + c.Cou_Logo;
                //是否免费，或是限时免费
                if (c.Cou_IsLimitFree)
                {
                    DateTime freeEnd = c.Cou_FreeEnd.AddDays(1).Date;
                    if (!(c.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                        c.Cou_IsLimitFree = false;
                }
                c.Cou_Intro = c.Cou_Target = c.Cou_Content = "";
                c.Cou_Name = c.Cou_Name.Replace("\"", "&quot;");
                //增加输出项
                Dictionary<string, object> addParas = new Dictionary<string, object>();
                addParas.Add("olcount", Business.Do<IOutline>().OutlineOfCount(c.Cou_ID, -1, true));    //当前课程里的章节数
                addParas.Add("quscount", Business.Do<IQuestions>().QuesOfCount(c.Org_ID, c.Sbj_ID, c.Cou_ID, -1, 0, true));   //当前课程的试题    
                //获取课程的购买信息
                Song.Entities.Student_Course sc = Business.Do<ICourse>().StudentCourse(stid, c.Cou_ID);
                if (sc != null)
                {
                    addParas.Add("starttime", sc.Stc_StartTime);
                    addParas.Add("endtime", sc.Stc_EndTime);
                }
                tm += "" + c.ToJson(null, null, addParas);
                if (i < courses.Count - 1) tm += ",";
            }
            tm += "]}";
            return tm;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}