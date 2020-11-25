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
using System.Data;
using System.IO;
using pili_sdk;
using Newtonsoft.Json;
using System.Xml.Serialization;


namespace Song.ViewData.Methods
{
    /// <summary>
    /// 章节信息
    /// </summary>
    [HttpGet, HttpPut]
    public class Outline : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 获取章节
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Cache(Expires = 20)]
        public Song.Entities.Outline ForID(int id)
        {
            return Business.Do<IOutline>().OutlineSingle(id);
        }
        /// <summary>
        /// 章节列表
        /// </summary>
        /// <param name="couid">所属课程的id</param>
        /// <param name="pid">上级id</param>
        /// <returns></returns>
        [HttpGet(Ignore = true)]
        [Cache(Expires = 20)]
        public Song.Entities.Outline[] List(int couid, int pid)
        {
            return Business.Do<IOutline>().OutlineCount(couid, pid, true, 0);
        }

        /// <summary>
        /// 树形章节列表
        /// </summary>
        /// <param name="couid">所属课程的id</param>
        /// <returns></returns>
        [HttpGet,HttpPost,HttpPut]
        [Cache(Expires=120)]
        public DataTable Tree(int couid)
        {
            // 当前课程的所有章节
            Song.Entities.Outline[] outlines = Business.Do<IOutline>().OutlineAll(couid, true);
            if (outlines.Length > 0)
            {
                foreach (Song.Entities.Outline ol in outlines) ol.Ol_Intro = string.Empty;                
                //树形章节输出
                DataTable dt = Business.Do<IOutline>().OutlineTree(outlines);
                return dt;
            }
            return null;
        }
        /// <summary>
        /// 章节附件
        /// </summary>
        /// <param name="oluid">章节的uid</param>
        /// <returns></returns>
        //[Cache(Expires = 20)]
        public Song.Entities.Accessory[] Accessory(string uid)
        {
            //先判断是否购买课程
            Song.Entities.Accounts acc = Extend.LoginState.Accounts.CurrentUser;
            if (acc == null) throw new ExceptionForNoLogin();
            Song.Entities.Outline outline = Business.Do<IOutline>().OutlineSingle(uid);
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(outline.Cou_ID);
            //是否免费，或是限时免费
            if (course.Cou_IsLimitFree)
            {
                DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
                if (!(course.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                    course.Cou_IsLimitFree = false;
            }
            //是否购买 （必须是免费或购买后才能查看附件，仅试学时，不可以查看附件）
            bool isBuy = course.Cou_IsFree || course.Cou_IsLimitFree ? true : Business.Do<ICourse>().IsBuy(course.Cou_ID, acc.Ac_ID);
            //if (!isBuy) throw new Exception("未购买课程，无法提供附件信息");
            //获取附件
            List<Song.Entities.Accessory> access = Business.Do<IAccessory>().GetAll(uid, "Course");
            if (isBuy)
            {
                foreach (Accessory ac in access)
                    ac.As_FileName = Upload.Get["Course"].Virtual + ac.As_FileName;
            }
            return access.ToArray<Song.Entities.Accessory>();
        }
        /// <summary>
        /// 视频事件
        /// </summary>
        /// <param name="olid"></param>
        /// <returns></returns>
        [Cache(Expires = 20)]
        public OutlineEvent[] VideoEvents(int olid)
        {
            OutlineEvent[] events = Business.Do<IOutline>().EventAll(-1, olid, -1, true);
            foreach (OutlineEvent ev in events)
            {
                if (ev.Oe_EventType == 1 || ev.Oe_EventType == 2) continue;
                if (string.IsNullOrWhiteSpace(ev.Oe_Datatable) || ev.Oe_Datatable.Trim() == "") continue;
                //如果是问答
                XmlSerializer xmlSerial = new XmlSerializer(typeof(DataTable));
                DataTable dt = (DataTable)xmlSerial.Deserialize(new System.IO.StringReader(ev.Oe_Datatable));
                ev.Oe_Datatable = JsonConvert.SerializeObject(dt);
            }
            return events;
        }
        /// <summary>
        /// 章节的状态
        /// </summary>
        /// <param name="olid"></param>
        /// <returns></returns>        
        public Dictionary<string, object> State(int olid)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            Song.Entities.Accounts acc = Extend.LoginState.Accounts.CurrentUser;
            dic.Add("isLogin", acc != null);    //学员是否登录
            //
            Song.Entities.Outline outline = Business.Do<IOutline>().OutlineSingle(olid);
            if (outline == null) throw new Exception("章节不存在");
            dic.Add("Name", outline.Ol_Name);
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(outline.Cou_ID);
            if (course == null) throw new Exception("课程不存在");
            dic.Add("Course", course.Cou_Name);
            Song.Entities.Organization orgin;
            //是否限制在桌面应用中打开
            dic.Add("DeskAllow", this.getDeskallow(course,outline,out orgin));
            //是否在切换浏览器时继续播放
            dic.Add("SwitchPlay", this.getSwitchPlay(course, acc, orgin));            
            //是否免费，或是限时免费
            if (course.Cou_IsLimitFree)
            {
                DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
                if (!(course.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                    course.Cou_IsLimitFree = false;
            }            
            //是否可以学习，是否购买
            bool isStudy = false, isBuy = false, canStudy = false;
            if (acc != null)
            {
                isStudy = Business.Do<ICourse>().Study(course.Cou_ID, acc.Ac_ID);
                isBuy = course.Cou_IsFree || course.Cou_IsLimitFree ? true : Business.Do<ICourse>().IsBuy(course.Cou_ID, acc.Ac_ID);
                //学习记录
                Song.Entities.LogForStudentStudy studyLog = Business.Do<IStudent>().LogForStudySingle(acc.Ac_ID, outline.Ol_ID);
                dic.Add("StudyTime", studyLog != null ? studyLog.Lss_StudyTime : 0);
                dic.Add("PlayTime", studyLog != null ? studyLog.Lss_PlayTime : 0);
            }
            dic.Add("isStudy", isStudy);
            dic.Add("isBuy", isBuy);
            //是否可以学习,如果是免费或已经选修便可以学习，否则当前课程允许试用且当前章节是免费的，也可以学习
            canStudy = isBuy || (isStudy && outline.Ol_IsUse && outline.Ol_IsFinish && course.Cou_IsTry && outline.Ol_IsFree);
            dic.Add("canStudy", canStudy);
            //是否有知识库
            int knlCount = Business.Do<IKnowledge>().KnowledgeOfCount(-1, course.Cou_ID, -1, true);
            dic.Add("isKnl", knlCount > 0 && canStudy);

            //是否有视频，是否为外链视频

            List<Song.Entities.Accessory> videos = Business.Do<IAccessory>().GetAll(outline.Ol_UID, "CourseVideo");
            bool existVideo = videos.Count > 0;
            dic.Add("outerVideo", existVideo && (videos.Count > 0 && videos[0].As_IsOuter));    //站外视频,包括其它网站的视频，或是视频播放链接
            dic.Add("otherVideo", existVideo && (videos.Count > 0 && videos[0].As_IsOther));    //其它视频平台的链接
            if (videos.Count > 0)
            {
                string videoUrl = existVideo ? videos[0].As_FileName : string.Empty; //视频地址
                //如果是内部链接
                if (existVideo && !videos[0].As_IsOuter)
                {
                    videoUrl = Upload.Get[videos[0].As_Type].Virtual + videoUrl;
                    string ext = System.IO.Path.GetExtension(videoUrl).ToLower();
                    if (ext == ".flv") videoUrl = Path.ChangeExtension(videoUrl, ".mp4");
                }
                dic.Add("urlVideo", canStudy ? videoUrl : string.Empty);
                outline.Ol_IsLive = false;
            }
            //直播  
            bool isLive = outline.Ol_IsLive, isLiving = false;
            if (outline.Ol_IsLive)
            {
                string urlVideo = string.Empty;
                if (canStudy)
                {
                    //查询直播状态
                    pili_sdk.pili.StreamStatus status = Pili.API<IStream>().Status(outline.Ol_LiveID);
                    if (status != null)
                    {
                        pili_sdk.pili.Stream stream = status.Stream;
                        string proto = Business.Do<ILive>().GetProtocol;    //协议，http还是https
                        urlVideo = string.Format("{0}://{1}/{2}/{3}.m3u8", proto, stream.LiveHlsHost, stream.HubName, stream.Title);
                        isLiving = status.Status == "connected";  //正在直播
                        existVideo = isLiving ? false : existVideo;
                    }
                }
                //直播播放地址
                if(!dic.ContainsKey("urlVideo"))
                    dic.Add("urlVideo", urlVideo);
                //直播开始或结束
                dic.Add("LiveStart", DateTime.Now > outline.Ol_LiveTime);
                dic.Add("LiveOver", outline.Ol_LiveTime.AddMinutes(outline.Ol_LiveSpan) < DateTime.Now);
            }
            dic.Add("isLive", outline.Ol_IsLive);   //是否为直播章节
            dic.Add("isLiving", isLiving && canStudy);  //是否在直播中
            dic.Add("existVideo", existVideo && canStudy);

            //是否有课程内容
            bool isContext = !string.IsNullOrWhiteSpace(outline.Ol_Intro);
            dic.Add("isContext", isContext && canStudy);
            //是否有试题
            bool isQues = Business.Do<IOutline>().OutlineIsQues(outline.Ol_ID, true);
            dic.Add("isQues", isQues && canStudy);
            //是否有附件
            int accessCount = Business.Do<IAccessory>().OfCount(outline.Ol_UID, "Course");
            dic.Add("isAccess", accessCount > 0 && canStudy);
            //啥都没有（视频，内容，附件，试题，都没有）
            bool isNull = !(existVideo || isLive || isContext || isQues || isQues || accessCount > 0);
            dic.Add("isNull",isNull || !canStudy);
            return dic;
        }
        /// <summary>
        /// 判断是否必须在桌面应用中学习
        /// </summary>
        /// <returns>如果为true，则必须在课面应用中学习</returns>
        private bool getDeskallow(Song.Entities.Course course, Song.Entities.Outline ol,out Song.Entities.Organization organ)
        {
            //当前机构
            organ = Business.Do<IOrganization>().OrganSingle(course.Org_ID);

            //自定义配置项
            WeiSha.Common.CustomConfig config = CustomConfig.Load(organ.Org_Config);
            //是否限制在桌面应用中学习
            bool studyFordesk = config["StudyForDeskapp"].Value.Boolean ?? false;   //课程学习需要在桌面应用打开
            bool freeFordesk = config["FreeForDeskapp"].Value.Boolean ?? false;     //免费课程和试用章节除外
            if (!WeiSha.Common.Browser.IsDestopApp)
            {
                if (!freeFordesk)
                {
                    return studyFordesk && !WeiSha.Common.Browser.IsDestopApp;
                }
                else
                {
                    if (course.Cou_IsFree || course.Cou_IsLimitFree) return false;
                    if (ol.Ol_IsFree) return false;
                }
            }
            return true && !WeiSha.Common.Browser.IsDestopApp;
        }
        /// <summary>
        /// 判断当前课程是否允许切换浏览器时视频暂停
        /// </summary>
        /// <param name="course"></param>
        /// <param name="acc"></param>
        /// <param name="organ"></param>
        /// <returns>true，则允许浏览器失去焦点时，视频仍然播放</returns>
        private bool getSwitchPlay(Song.Entities.Course course, Song.Entities.Accounts acc, Song.Entities.Organization organ)
        {
            if (acc == null) return false;
            //自定义配置项
            WeiSha.Common.CustomConfig config = CustomConfig.Load(organ.Org_Config);
            bool isstop = config["IsSwitchPlay"].Value.Boolean ?? false;
            if (isstop) return true;
            //如果机构设置中为false，继续判断学员组的设置
            Song.Entities.StudentSort sort = Business.Do<IStudent>().SortSingle(acc.Sts_ID);
            if (sort == null || !sort.Sts_IsUse) return isstop;
            return sort.Sts_SwitchPlay;
        }
    }
}
