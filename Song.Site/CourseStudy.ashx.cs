using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using System.Data;
using System.IO;

namespace Song.Site
{
    /// <summary>
    /// 课程学习（web端）
    /// </summary>
    public class CourseStudy : BasePage
    {
        //章节id,课程id
        int id = WeiSha.Common.Request.QueryString["olid"].Int32 ?? 0;
        int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //状态值（来自地址栏），1为视频，2为内容，3为附件，4为试题
        int stateVal = WeiSha.Common.Request.QueryString["state"].Int32 ?? 0;
        //是否选学的当前课程，是否购买
        bool isStudy = false, isBuy = false;
        protected override void InitPageTemplate(HttpContext context)
        {            
            //当前章节，如果章节id，则取课程第一个章节
            Song.Entities.Outline ol = id < 1 ? 
                Business.Do<IOutline>().OutlineFirst(couid, true)
                : Business.Do<IOutline>().OutlineSingle(id);
            couid = couid > 0 ? couid : (ol != null ? ol.Cou_ID : 0);
            //当前课程            
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
            if (course == null || !course.Cou_IsUse) return;
            this.Document.Variables.SetValue("course", course);
            //如果章节为空，则不再后面的了
            if (ol == null) return;
            this.Document.Variables.SetValue("outline", ol);
            this.Document.Variables.SetValue("olid", ol.Ol_ID.ToString());
            ////上级章节
            //this.Document.Variables.SetValue("pat", Business.Do<IOutline>().OutlineSingle(ol.Ol_PID));            
            //Response.Cookies.Add(new HttpCookie("olid", ol.Ol_ID.ToString()));
            //当前课程            
            //Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid >0 ? couid : ol.Cou_ID);
            //if (course == null || !course.Cou_IsUse) return;           
            //是否免费，或是限时免费
            if (course.Cou_IsLimitFree)
            {
                DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
                if (!(course.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                    course.Cou_IsLimitFree = false;
            }
            this.Document.Variables.SetValue("course", course);
            //判断是否允许在桌面应用中学习
            this.Document.Variables.SetValue("StudyForDeskapp", getForDeskapp(course, ol));
            //是否学习当前课程，如果没有学习且课程处于免费，则创建关联
            if (this.Account != null)
            {
                isStudy = Business.Do<ICourse>().Study(course.Cou_ID, this.Account.Ac_ID);
                isBuy = course.Cou_IsFree || course.Cou_IsLimitFree ? true : Business.Do<ICourse>().IsBuy(course.Cou_ID, this.Account.Ac_ID);
            }
            this.Document.Variables.SetValue("isStudy", isStudy);
            this.Document.Variables.SetValue("isBuy", isBuy);
            //是否可以学习,如果是免费或已经选修便可以学习，否则当前课程允许试用且当前章节是免费的，也可以学习
            bool canStudy = isBuy || (isStudy && ol.Ol_IsUse && ol.Ol_IsFinish && course.Cou_IsTry && ol.Ol_IsFree);
            this.Document.Variables.SetValue("canStudy", canStudy);
            //记录学员当前学习的课程
            if (isStudy) Extend.LoginState.Accounts.Course(course);  
            

            #region 内容输出
            CourseContext_State state = new CourseContext_State();            
            //视频
            Song.Entities.Accessory video = getVideo(ol.Ol_UID);
            this.Document.Variables.SetValue("video", video);
            if (video != null) state.Video = canStudy ? true : false;
            if (Extend.LoginState.Accounts.IsLogin)
            {
                Song.Entities.LogForStudentStudy studyLog = Business.Do<IStudent>().LogForStudySingle(this.Account.Ac_ID, ol.Ol_ID);
                if (studyLog != null)
                {
                    this.Document.Variables.SetValue("studyLog", studyLog.Lss_StudyTime);
                    double historyPlay = (double)studyLog.Lss_PlayTime / 1000;
                    this.Document.Variables.SetValue("historyPlay", historyPlay);
                }
            }
            //内容
            if (!string.IsNullOrWhiteSpace(ol.Ol_Intro)) state.Context = canStudy ? true : false;             
            //附件
            List<Song.Entities.Accessory> access = Business.Do<IAccessory>().GetAll(ol.Ol_UID, "Course");
            if (access.Count > 0)
            {
                foreach (Accessory ac in access)
                    ac.As_FileName = Upload.Get["Course"].Virtual + ac.As_FileName;
                this.Document.Variables.SetValue("access", access);
                state.Attachment = canStudy ? true : false; 
            }
            //当前章节是否有试题
            if (canStudy)
            {
                bool isQues = Business.Do<IOutline>().OutlineIsQues(ol.Ol_ID, true);
                if (isQues) state.Questions = canStudy ? true : false; ;
            }
            state.JudgeNull(stateVal);
            this.Document.Variables.SetValue("state", state);
            #endregion

            //章节事件
            OutlineEvent[] events = Business.Do<IOutline>().EventAll(-1, ol.Ol_ID, -1, true);
            this.Document.Variables.SetValue("events", events);
            this.Document.RegisterGlobalFunction(this.getEventQues);
            this.Document.RegisterGlobalFunction(this.getEventFeedback);
            this.Document.RegisterGlobalFunction(this.GetOrder);
        }
        /// <summary>
        /// 判断是否必须在桌面应用中学习
        /// </summary>
        /// <returns>如果为true，则必须在课面应用中学习</returns>
        private bool getForDeskapp(Song.Entities.Course course, Song.Entities.Outline ol)
        {
            //自定义配置项
            WeiSha.Common.CustomConfig config = CustomConfig.Load(this.Organ.Org_Config);
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
        #region 章节事件用到的方法
        /// <summary>
        /// 获取视频的弹出试题选项内容
        /// </summary>
        /// <returns></returns>
        private DataTable getEventQues(object[] paras)
        {
            int oeid = 0;
            if (paras.Length > 0 && paras[0] is int)
                int.TryParse(paras[0].ToString(), out oeid);
            DataTable dt = Business.Do<IOutline>().EventQues(oeid);
            return dt;
        }
        /// <summary>
        /// 设置实时反馈的选项内容
        /// </summary>
        /// <returns></returns>
        private DataTable getEventFeedback(object[] paras)
        {
            int oeid = 0;
            if (paras.Length > 0 && paras[0] is int)
                int.TryParse(paras[0].ToString(), out oeid);
            DataTable dt = Business.Do<IOutline>().EventFeedback(oeid);
            return dt;
        }
        /// <summary>
        /// 获取序号
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected object GetOrder(object[] index)
        {
            int tax = 0;
            if (index.Length > 0)
                tax = Convert.ToInt32(index[0]);
            return (char)(tax - 1 + 65);
        }
        #endregion

        /// <summary>
        /// 获取视频
        /// </summary>
        /// <param name="couUID"></param>
        /// <returns></returns>
        public static Song.Entities.Accessory getVideo(string couUID)
        {            
            //视频
            List<Song.Entities.Accessory> videos = Business.Do<IAccessory>().GetAll(couUID, "CourseVideo");
            if (videos.Count <= 0) return null;
            Song.Entities.Accessory video = null;
            //如果是外部链接
            if (videos[0].As_IsOuter) video = videos[0];
            //如果是内部链接
            if (!videos[0].As_IsOuter)
            {
                videos[0].As_FileName = Upload.Get[videos[0].As_Type].Virtual + videos[0].As_FileName;
                string fileHy = WeiSha.Common.Server.MapPath(videos[0].As_FileName);
                if (!System.IO.File.Exists(fileHy))
                {
                    string ext = System.IO.Path.GetExtension(fileHy).ToLower();
                    if (ext == ".mp4") videos[0].As_FileName = Path.ChangeExtension(videos[0].As_FileName, ".flv");
                    if (ext == ".flv") videos[0].As_FileName = Path.ChangeExtension(videos[0].As_FileName, ".mp4");
                }
                video = videos[0];
            }
            return video;
        }
    }
    /// <summary>
    /// 课程内容状态，例如有没有视频，有没有试题
    /// </summary>
    public class CourseContext_State
    {
        //视频,第一个指是否有内容，第二为状态是否该显示
        public bool Video { get; set; }
        public bool VideoState { get; set; }
        //内容
        public bool Context { get; set; }
        public bool ContextState { get; set; }
        //附件
        public bool Attachment { get; set; }
        public bool AttachmentState { get; set; }
        //试题
        public bool Questions { get; set; }
        public bool QuestionsState { get; set; }
        //没有任何内容
        public bool Null { get; set; }
        /// <summary>
        /// 判断是否是全没有内容
        /// </summary>
        /// <returns></returns>
        public bool JudgeNull(int stateVal)
        {
            this.VideoState = this.Video && stateVal == 1;
            this.ContextState = this.Context && stateVal == 2;
            this.AttachmentState = this.Attachment && stateVal == 3;
            this.QuestionsState = this.Questions && stateVal == 4;
            if (stateVal == 0)
            {
                if (this.Video) this.VideoState = true;
                if (this.Context) this.ContextState = true;
                if (this.Attachment) this.AttachmentState = true;
                if (this.Questions) this.QuestionsState = true;
            }
            //如果有一个不空，则不空
            this.Null = !(this.Video || this.Context || this.Attachment || this.Questions);
            return this.Null;
        }
    };

}