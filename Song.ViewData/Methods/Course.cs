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
    public class Course : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 根据课程ID获取课程信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Cache(Expires = 60)]
        public Song.Entities.Course ForID(int id)
        {
            Song.Entities.Course cur = Business.Do<ICourse>().CourseSingle(id);
            return _tran(cur);
        }
        /// <summary>
        /// 分页获取课程
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjids">章节id，可以为多个，以逗号分隔</param>
        /// <param name="search">检索字符，按课程名称</param>
        /// <param name="size">每页几条</param>
        /// <param name="index">第几页</param>
        /// <returns></returns>
        public ListResult Pager(int orgid, string sbjids, string search, int size, int index)
        {
            int count = 0;
            List<Song.Entities.Course> eas = null;
            eas = Business.Do<ICourse>().CoursePager(orgid, sbjids, true, search, "", size, index, out count);
            for (int i = 0; i < eas.Count; i++)
            {
                eas[i] = _tran(eas[i]);
            }
            ListResult result = new ListResult(eas);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 分页获取课程
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjids">章节id，可以为多个，以逗号分隔</param>
        /// <param name="search">检索字符，按课程名称</param>
        /// <param name="size">每页几条</param>
        /// <param name="index">第几页</param>
        /// <returns></returns>
        public ListResult ShowPager(int orgid, string sbjids, string search, int size, int index)
        {
            int count = 0;
            List<Song.Entities.Course> eas = null;
            eas = Business.Do<ICourse>().CoursePager(orgid, sbjids, true, search, "", size, index, out count);
            string vpath = WeiSha.Common.Upload.Get["Course"].Virtual;
            for (int i = 0; i < eas.Count; i++)
            {
                Song.Entities.Course c = eas[i];
                //图片和缩略图
                c.Cou_Logo = vpath + c.Cou_Logo;
                c.Cou_LogoSmall = vpath + c.Cou_LogoSmall;
                if (!System.IO.File.Exists(c.Cou_Logo)) c.Cou_Logo = "";
                if (!System.IO.File.Exists(c.Cou_LogoSmall)) c.Cou_LogoSmall = "";
                //是否免费，或是限时免费
                if (c.Cou_IsLimitFree)
                {
                    DateTime freeEnd = c.Cou_FreeEnd.AddDays(1).Date;
                    if (!(c.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                        c.Cou_IsLimitFree = false;
                }
                c.Cou_Intro = c.Cou_Target = c.Cou_Content = "";
                c.Cou_Name = c.Cou_Name.Replace("\"", "&quot;");
            }
            ListResult result = new ListResult(eas);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
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

        #region 私有方法，处理对象的关联信息
        /// <summary>
        /// 处理课程信息，图片转为全路径，并生成clone对象
        /// </summary>
        /// <param name="cour">课程对象的clone</param>
        /// <returns></returns>
        private Song.Entities.Course _tran(Song.Entities.Course cour)
        {
            if (cour == null) return cour;
            string vpath = WeiSha.Common.Upload.Get["Course"].Virtual;
            Song.Entities.Course curr = cour.Clone<Song.Entities.Course>();
            curr.Cou_Logo = vpath + curr.Cou_Logo;
            curr.Cou_LogoSmall = vpath + curr.Cou_LogoSmall;
            return curr;
        }
        #endregion
    }
}
