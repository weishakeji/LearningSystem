using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.Entities;
using Song.ServiceInterfaces;
using VTemplate.Engine;
using System.Data;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 章节学习的界面，用于视频、图文资料学习
    /// </summary>
    public class CourseStudy : BasePage
    {
        //课程ID，章节id
        protected int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        protected int olid = WeiSha.Common.Request.QueryString["olid"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            this.Document.SetValue("couid", couid);
            //当前课程
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
            if (course == null) return;
            if (course != null)
            {
                //是否免费，或是限时免费
                if (course.Cou_IsLimitFree)
                {
                    DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
                    if (!(course.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                        course.Cou_IsLimitFree = false;
                }
                this.Document.SetValue("course", course);                 
                //当前课程下的章节
                Song.Entities.Outline[] outlines = Business.Do<IOutline>().OutlineAll(couid, true);
                //foreach (Song.Entities.Outline c in outlines)
                //    c.Ol_Intro = Extend.Html.ClearHTML(c.Ol_Intro);
                this.Document.SetValue("outlines", outlines);
                if (outlines != null && outlines.Length > 0)
                {
                    this.Document.Variables.SetValue("dtOutlines", Business.Do<IOutline>().OutlineTree(outlines));
                }
            }
            //是否学习当前课程
            int accid = 0;
            if (Extend.LoginState.Accounts.IsLogin)
            {
                accid = this.Account.Ac_ID;
                bool isStudy = Business.Do<ICourse>().StudyIsCourse(accid, couid);
                this.Document.Variables.SetValue("isStudy", isStudy);
                //当前章节
                Song.Entities.Outline ol = null;
                ol = olid < 1 ? Business.Do<IOutline>().OutlineFirst(couid, true)
                           : ol = Business.Do<IOutline>().OutlineSingle(olid);
                if (ol == null) return;
                //是否可以学习,如果是免费或已经选修便可以学习，否则当前课程允许试用且当前章节是免费的，也可以学习
                bool canStudy = isStudy || course.Cou_IsFree || course.Cou_IsLimitFree ? true : (course.Cou_IsTry && ol.Ol_IsFree);
                canStudy = canStudy && ol.Ol_IsUse && ol.Ol_IsFinish && this.Account != null;
                this.Document.Variables.SetValue("canStudy", canStudy);
                this.Document.Variables.SetValue("outline", ol);
                if (!canStudy) return;
                if (ol == null) return;
                //入写章节id的cookie，当播放视频时会判断此处
                Response.Cookies.Add(new HttpCookie("olid", ol.Ol_ID.ToString()));
                //自定义配置项
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);
                //视频
                List<Song.Entities.Accessory> videos = Business.Do<IAccessory>().GetAll(ol.Ol_UID, "CourseVideo");
                if (videos.Count > 0)
                {
                    if (videos[0].As_IsOuter)
                    {
                        //如果是外部链接
                        this.Document.Variables.SetValue("video", videos[0]);
                    }
                    else
                    {
                        //如果是内部链接
                        videos[0].As_FileName = Upload.Get[videos[0].As_Type].Virtual + videos[0].As_FileName;
                        this.Document.Variables.SetValue("video", videos[0]);
                        if (Extend.LoginState.Accounts.IsLogin)
                        {
                            Song.Entities.LogForStudentStudy studyLog = Business.Do<IStudent>().LogForStudySingle(this.Account.Ac_ID, ol.Ol_ID);
                            if (studyLog != null)
                            {
                                this.Document.Variables.SetValue("studyLog", studyLog);
                                double historyPlay = (double)studyLog.Lss_PlayTime / 1000;
                                this.Document.Variables.SetValue("historyPlay", historyPlay);
                            }
                        }
                    }
                    this.Document.Variables.SetValue("vpath", Upload.Get["CourseVideo"].Virtual);
                    this.Document.Variables.SetValue("IsVideoNoload", config["IsVideoNoload"].Value.Boolean ?? false);
                }
                //附件
                List<Song.Entities.Accessory> access = Business.Do<IAccessory>().GetAll(ol.Ol_UID, "Course");
                if (access.Count > 0)
                {
                    foreach (Accessory ac in access)
                        ac.As_FileName = Upload.Get["Course"].Virtual + ac.As_FileName;
                    this.Document.Variables.SetValue("access", access);
                }
                //章节事件
                OutlineEvent[] events = Business.Do<IOutline>().EventAll(-1, ol.Ol_ID, -1, true);
                this.Document.Variables.SetValue("events", events);
                this.Document.RegisterGlobalFunction(this.getEventQues);
                this.Document.RegisterGlobalFunction(this.getEventFeedback);
                this.Document.RegisterGlobalFunction(this.GetOrder);
            }
        }
        /// <summary>
        /// 获取试题的选项内容
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
    }
}