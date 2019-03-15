using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
namespace Song.Site.Ajax
{
    /// <summary>
    /// 记录学员学习视频的时间
    /// </summary>
    public class StudentStudy : IHttpHandler
    {
        //课程id，章节id
        private int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        private int olid = WeiSha.Common.Request.QueryString["olid"].Int32 ?? 0;
        //播放进度，单位毫秒
        private int playTime = WeiSha.Common.Request.QueryString["playTime"].Int32 ?? 0;
        //学习时间，单位秒
        private int studyTime = WeiSha.Common.Request.QueryString["studyTime"].Int32 ?? 0;
        //视频总时长，单位毫秒
        private int totalTime = WeiSha.Common.Request.QueryString["totalTime"].Int32 ?? 0;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //当前学员
            Song.Entities.Accounts student = Extend.LoginState.Accounts.CurrentUser;
            if (student == null)
            {
                context.Response.Write("-1");
                return;
            }
            else
            {
                if (totalTime <= 0) throw new Exception("视频总时长为零");
                //记录学习进度，返回完成度的百分比
                double per = Business.Do<IStudent>().LogForStudyUpdate(couid, olid, student, playTime, studyTime, totalTime);
                context.Response.Write(per);
            }
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