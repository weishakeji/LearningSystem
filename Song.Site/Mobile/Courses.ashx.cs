using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
namespace Song.Site.Mobile
{
    /// <summary>
    /// 课程列表
    /// </summary>
    public class Courses : BasePage
    {
        //专业ID,课程id
        protected int sbjid = WeiSha.Common.Request.QueryString["sbjid"].Int32 ?? 0;
        protected int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //搜索字符
        protected string search = WeiSha.Common.Request.Form["search"].String;
        protected override void InitPageTemplate(HttpContext context)
        {
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                //专业id，多个id用逗号分隔
                string sbjids = WeiSha.Common.Request.Form["sbjids"].String;
                string sear = WeiSha.Common.Request.Form["sear"].UrlDecode;
                int size = WeiSha.Common.Request.Form["size"].Int32 ?? 10;  //每页多少条
                int index = WeiSha.Common.Request.Form["index"].Int32 ?? 1;  //第几页
                //仅限的输出字段
                string onlyFeild = WeiSha.Common.Request.Form["only"].String;   //输出哪些字段
                string wipeFeild = WeiSha.Common.Request.Form["wipe"].String;   //哪些字段不输出
                //查询排序方式，
                //def:默认，先推荐，然后按访问量倒序
                //flux：按访问量倒序
                //tax：按自定义排序要求
                //new:按创建时间，最新发布在前面
                //rec:按推荐，先推荐，然后按tax排序
                string order = WeiSha.Common.Request.Form["order"].String;
                int sumcount = 0;
                //当前专业下的子专业，如果是顶级，则显示所有顶级专业
                List<Song.Entities.Course> courses;
                courses = Business.Do<ICourse>().CoursePager(this.Organ.Org_ID, sbjids, true, sear, order, size, index, out sumcount);
                string json = "{\"size\":" + size + ",\"index\":" + index + ",\"sumcount\":" + sumcount + ",";
                json += "\"items\":[";
                for (int i = 0; i < courses.Count; i++)
                {
                    Song.Entities.Course c = courses[i];
                    c.Cou_LogoSmall = string.IsNullOrWhiteSpace(c.Cou_LogoSmall) ? null : Upload.Get["Course"].Virtual + c.Cou_LogoSmall;
                    c.Cou_Logo = string.IsNullOrWhiteSpace(c.Cou_Logo) ? null : Upload.Get["Course"].Virtual + c.Cou_Logo;
                    c.Cou_Intro = c.Cou_Content = string.Empty;
                    //是否免费，或是限时免费
                    if (c.Cou_IsLimitFree)
                    {
                        DateTime freeEnd = c.Cou_FreeEnd.AddDays(1).Date;
                        if (!(c.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                            c.Cou_IsLimitFree = false;
                    }
                    //增加输出项
                    Dictionary<string, object> addParas = new Dictionary<string, object>();
                    addParas.Add("olcount", Business.Do<IOutline>().OutlineOfCount(c.Cou_ID, -1, true));    //当前课程里的章节数
                    addParas.Add("quscount", Business.Do<IQuestions>().QuesOfCount(c.Org_ID, c.Sbj_ID, c.Cou_ID, -1, 0, true));   //当前课程的试题                   
                    json += c.ToJson(onlyFeild, wipeFeild, addParas) + ",";
                }
                if (json.EndsWith(",")) json = json.Substring(0, json.Length - 1);
                json += "]}";
                Response.Write(json);
                Response.End();
            }
        }
    }
}