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
        //是否选学的当前课程，是否购买
        bool isStudy = false, isBuy = false;
        protected override void InitPageTemplate(HttpContext context)
        {
            //当前课程
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
            if (course == null || !course.Cou_IsUse) return;
            //是否免费，或是限时免费
            if (course.Cou_IsLimitFree)
            {
                DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
                if (!(course.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                    course.Cou_IsLimitFree = false;
            }
            this.Document.Variables.SetValue("course", course);
            //是否学习当前课程，如果没有学习且课程处于免费，则创建关联
            if (this.Account != null)
            {
                isStudy = Business.Do<ICourse>().Study(course.Cou_ID, this.Account.Ac_ID);
                isBuy = course.Cou_IsFree || course.Cou_IsLimitFree ? true : Business.Do<ICourse>().IsBuy(course.Cou_ID, this.Account.Ac_ID);
            }
            this.Document.Variables.SetValue("isStudy", isStudy);
            this.Document.Variables.SetValue("isBuy", isBuy);
            //记录学员当前学习的课程
            if (isStudy) Extend.LoginState.Accounts.Course(course);
            //当前章节
            Song.Entities.Outline ol = olid < 1 ?
                Business.Do<IOutline>().OutlineFirst(couid, true)
                : Business.Do<IOutline>().OutlineSingle(olid);
            if (ol == null) return;
            this.Document.Variables.SetValue("outline", ol);
            this.Document.Variables.SetValue("olid", ol.Ol_ID.ToString());
            //入写章节id的cookie，当播放视频时会判断此处
            Response.Cookies.Add(new HttpCookie("olid", ol.Ol_ID.ToString()));
            //是否可以学习,如果是免费或已经选修便可以学习，否则当前课程允许试用且当前章节是免费的，也可以学习
            bool canStudy = isBuy || (isStudy && ol.Ol_IsUse && ol.Ol_IsFinish && course.Cou_IsTry && ol.Ol_IsFree);
            this.Document.Variables.SetValue("canStudy", canStudy);

            #region 章节输出
            // 当前课程的所有章节            
            Song.Entities.Outline[] outlines = Business.Do<IOutline>().OutlineAll(ol.Cou_ID, true);
            //课程章节列表
            this.Document.Variables.SetValue("outlines", outlines);
            //树形章节输出
            if (outlines.Length > 0)
                this.Document.Variables.SetValue("olTree", Business.Do<IOutline>().OutlineTree(outlines));
            #endregion

            //视频
            Song.Entities.Accessory video = Song.Site.CourseStudy.getVideo(ol.Ol_UID);
            this.Document.Variables.SetValue("video", video);            
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
            //附件
            List<Song.Entities.Accessory> access = Business.Do<IAccessory>().GetAll(ol.Ol_UID, "Course");
            if (access.Count > 0)
            {
                foreach (Accessory ac in access)
                    ac.As_FileName = Upload.Get["Course"].Virtual + ac.As_FileName;
                this.Document.Variables.SetValue("access", access);
            }
        }      
    }
}