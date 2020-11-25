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
using pili_sdk;


namespace Song.ViewData.Methods
{
    
    /// <summary>
    /// 教师账号的相关操作
    /// </summary> 
    [HttpGet]
    public class Teacher : ViewMethod, IViewAPI
    {
        
        /// <summary>
        /// 教师登录
        /// </summary>
        /// <param name="acc">账号</param>
        /// <param name="pw">密码，明文字符</param>
        /// <returns></returns>
        public Song.Entities.Teacher Login(string acc,string pw)
        {
            Song.Entities.Accounts account = Business.Do<IAccounts>().AccountsLogin(acc, pw, true);
            if (account == null) return null;
            if (!account.Ac_IsTeacher) return null;
            //
            Song.Entities.Teacher teacher = Business.Do<ITeacher>().TeacherSingle(account.Ac_AccName, -1);
            if (teacher != null)
            {
                teacher = teacher.Clone<Song.Entities.Teacher>();
                if (teacher != null) teacher.Th_Pw = string.Empty;
                teacher.Th_Photo = WeiSha.Common.Upload.Get["Accounts"].Virtual + account.Ac_Photo;
            }
            return teacher;
        }
        /// <summary>
        /// 教师的课程
        /// </summary>
        /// <param name="thid">教师id</param>
        /// <returns></returns>
        public Song.Entities.Course[] Courses(int thid)
        {
            List<Song.Entities.Course> cours = Business.Do<ICourse>().CourseAll(-1, -1, thid, true);
            if (cours == null || cours.Count < 1) return null;
            for (int i = 0; i < cours.Count; i++)
            {
                cours[i].Cou_Logo = WeiSha.Common.Upload.Get["Course"].Virtual + cours[i].Cou_Logo;
                cours[i].Cou_LogoSmall = WeiSha.Common.Upload.Get["Course"].Virtual + cours[i].Cou_LogoSmall;
            }
            return cours.ToArray<Song.Entities.Course>();
        }
        /// <summary>
        /// 教师的直播课
        /// </summary>
        /// <param name="thid">教师id</param>
        /// <param name="count">取多少条记录</param>
        /// <returns></returns>
        public Song.Entities.Course[] LiveCourses(int thid, int count)
        {
            List<Song.Entities.Course> cours = Business.Do<ICourse>().CourseCount(-1, -1, thid, true, null, true, count);
            if (cours == null || cours.Count < 1) return null;
            for (int i = 0; i < cours.Count; i++)
            {
                cours[i].Cou_Logo = WeiSha.Common.Upload.Get["Course"].Virtual + cours[i].Cou_Logo;
                cours[i].Cou_LogoSmall = WeiSha.Common.Upload.Get["Course"].Virtual + cours[i].Cou_LogoSmall;
            }
            return cours.ToArray<Song.Entities.Course>();
        }
        /// <summary>
        /// 教师的直播章节
        /// </summary>
        /// <param name="thid">教师id</param>
        /// <param name="couid">课程id</param>
        /// <returns>返回内容包括：带直播的章节的名称、ID、UID、课程名称、直播相关信息；
        /// LivePublish：直播推流地址，示例 rtmp://pili-publish.zhibo.weisha100.cn/ceshi5/132_267_548f3efe842b564c74babb4c70fc
        /// LiveHLS：播放地址，示例 http://pili-live-hls.zhibo.weisha100.cn/ceshi5/132_267_548f3efe842b564c74babb4c70fc
        /// LiveCover：直播封面，每五秒更新，示例http://pili-snapshot.zhibo.weisha100.cn/ceshi5/132_267_548f3efe842b564c74babb4c70fc
        /// LiveTime：直播开始时间，这个时间仅供于通知学员何时开始，从技术来说直播可以随时开始
        /// LiveSpan：直播课的时长，仅供告知学员课时长度，从技术上来说直播不受时长限制
        /// </returns>
        /// <remarks></remarks>
        public Outline_LiveInfo[] Lives(int thid, int couid)
        {
            Song.Entities.Course cour = Business.Do<ICourse>().CourseSingle(couid);
            if (cour == null) return null;
            if (cour.Th_ID != thid) return null;
            //
            Song.Entities.Outline[] outls = Business.Do<IOutline>().OutlineCount(couid, true, null, true, 0);
            List<Outline_LiveInfo> list = new List<Outline_LiveInfo>();
            //直播截图的域名
            string snapshot = Business.Do<ILive>().GetSnapshot;
            string proto = Business.Do<ILive>().GetProtocol;    //协议，http还是https
            foreach (Song.Entities.Outline o in outls)
            {
                pili_sdk.pili.Stream stream = Pili.API<IStream>().GetForTitle(o.Ol_LiveID);
                if (stream == null) continue;
                //推流地址
                string publist = string.Format("rtmp://{0}/{1}/{2}", stream.PublishRtmpHost, stream.HubName, stream.Title);
                //播放地址               
                string play = string.Format("{0}://{1}/{2}/{3}.m3u8", proto, stream.LiveHlsHost, stream.HubName, stream.Title);
                //封面地址
                string cover = string.Format("http://{0}/{1}/{2}.jpg", snapshot, stream.HubName, stream.Title);
                list.Add(new Outline_LiveInfo()
                {
                    Name = o.Ol_Name,
                    ID = o.Ol_ID,
                    UID = o.Ol_UID,
                    Course = cour.Cou_Name,
                    LiveID = stream.StreamID,
                    LiveTitle = stream.Title,
                    LiveTime = o.Ol_LiveTime,
                    LiveSpan = o.Ol_LiveSpan,
                    LivePublish = publist,
                    LiveHLS = play,
                    LiveCover = cover
                });

            }
            return list.ToArray<Outline_LiveInfo>();
        }

    }
    //章节直播信息
    public class Outline_LiveInfo
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public string UID { get; set; }
        public string Course { get; set; }      //课程名称
        public string LiveID { get; set; }      //直播ID，由z1.空间名.id三部分组成
        public string LiveTitle { get; set; }       //直播ID,这才是直播id
        public DateTime LiveTime { get; set; }      //直播开始时间
        public int LiveSpan { get; set; }           //直播时长，单位分钟
        public string LivePublish { get; set; }     //直播推送地址
        public string LiveHLS { get; set; }         //直播播放地址
        public string LiveCover { get; set; }       //直播封面地址
    }
}
