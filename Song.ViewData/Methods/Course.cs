using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Common;


namespace Song.ViewData.Methods
{
    /// <summary>
    /// 课程管理
    /// </summary>
    [HttpGet]
    public class Course : IViewAPI
    {
        /// <summary>
        /// 根据课程ID获取课程信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Song.Entities.Course ForID(int id)
        {
            return Business.Do<ICourse>().CourseSingle(id);
        }
        /// <summary>
        /// 记录当前学员的视频学习进度
        /// </summary>
        /// <param name="couid">课程ID</param>
        /// <param name="olid">章节ID</param>
        /// <param name="playTime">观看进度，单位：毫秒</param>
        /// <param name="studyTime">学习时间，单位：秒</param>
        /// <param name="totalTime">视频总时长，单位：秒</param>
        /// <returns></returns>
        [Student]
        [HttpPost]
        public double StudyLog(int couid, int olid, int playTime, int studyTime, int totalTime)
        {
            //当前学员
            Song.Entities.Accounts student = Extend.LoginState.Accounts.CurrentUser;
            if (student == null) return -1;
            double per = Business.Do<IStudent>().LogForStudyUpdate(couid, olid, student, playTime*1000, studyTime, totalTime*1000);
            return per;
        }
    }
}
