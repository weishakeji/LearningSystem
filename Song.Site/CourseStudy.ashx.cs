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
        int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //状态
        int state = WeiSha.Common.Request.QueryString["state"].Int32 ?? 0;
        //课程的所有章节
        Song.Entities.Outline[] outlines;
        //当前章节
        Song.Entities.Outline ol = null;
        //是否选学的当前课程
        bool isStudy = false;

        protected override void InitPageTemplate(HttpContext context)
        {
            //if (!Extend.LoginState.Accounts.IsLogin || this.Account==null)
            //    context.Response.Redirect(WeiSha.Common.Login.Get["Accounts"].NoLoginPath.String);
            //自定义配置项
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);
            //
            //取当前章节
            ol = id < 1 ? Business.Do<IOutline>().OutlineFirst(couid, true)
                       : ol = Business.Do<IOutline>().OutlineSingle(id);
            //当前课程            
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(ol == null ? couid : ol.Cou_ID);
            if (course == null) return;
            #region 创建与学员的关联
            if (this.Account != null)
            {
                int accid = this.Account.Ac_ID;
                bool istudy = Business.Do<ICourse>().Study(course.Cou_ID, accid);
            }
            #endregion
            
            #region 章节输出
            
            //是否免费，或是限时免费
            if (course.Cou_IsLimitFree)
            {
                DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
                if (!(course.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                    course.Cou_IsLimitFree = false;
            }
            this.Document.Variables.SetValue("course", course);
            Extend.LoginState.Accounts.Course(course);
            if (ol == null) return;
            // 
            couid = ol.Cou_ID;
            id = ol.Ol_ID;
            //入写章节id的cookie，当播放视频时会判断此处
            Response.Cookies.Add(new HttpCookie("olid", id.ToString()));
            outlines = Business.Do<IOutline>().OutlineAll(ol.Cou_ID, true);
            //是否学习当前课程
            if (course == null || this.Account==null) isStudy = false;
            else
                isStudy = Business.Do<ICourse>().StudyIsCourse(this.Account.Ac_ID, course.Cou_ID);
            this.Document.Variables.SetValue("isStudy", isStudy);
            //是否可以学习,如果是免费或已经选修便可以学习，否则当前课程允许试用且当前章节是免费的，也可以学习
            bool canStudy = isStudy || course.Cou_IsFree || course.Cou_IsLimitFree ? true : (course.Cou_IsTry && ol.Ol_IsFree);
            canStudy = canStudy && ol.Ol_IsUse && ol.Ol_IsFinish && this.Account != null;
            this.Document.Variables.SetValue("canStudy", canStudy);
            //课程章节列表
            this.Document.Variables.SetValue("outlines", outlines);
            //树形章节输出
            if (outlines.Length > 0)
                this.Document.Variables.SetValue("olTree", Business.Do<IOutline>().OutlineTree(outlines));
            this.Document.Variables.SetValue("outline", ol);
            #endregion
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
                    try
                    {
                        string fileHy = Server.MapPath(videos[0].As_FileName);
                        if (!System.IO.File.Exists(fileHy))
                        {
                            string ext = System.IO.Path.GetExtension(fileHy).ToLower();
                            if (ext == ".mp4") videos[0].As_FileName = Path.ChangeExtension(videos[0].As_FileName, ".flv");
                            if (ext == ".flv") videos[0].As_FileName = Path.ChangeExtension(videos[0].As_FileName, ".mp4");
                        }
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
                    catch
                    {
                    }
                }
                this.Document.Variables.SetValue("IsVideoNoload", config["IsVideoNoload"].Value.Boolean ?? false);
                state = state < 1 ? 1 : state;
            }
            //内容
            if (!string.IsNullOrWhiteSpace(ol.Ol_Intro)) state = state < 1 ? 2 : state;
            //上级章节
            if (ol != null)
            {
                Song.Entities.Outline pat = Business.Do<IOutline>().OutlineSingle(ol.Ol_PID);
                this.Document.Variables.SetValue("pat", pat);
            }
            //附件
            List<Song.Entities.Accessory> access = Business.Do<IAccessory>().GetAll(ol.Ol_UID, "Course");
            if (access.Count > 0)
            {
                foreach (Accessory ac in access)
                    ac.As_FileName = Upload.Get["Course"].Virtual + ac.As_FileName;
                this.Document.Variables.SetValue("access", access);
                state = state < 1 ? 3 : state;
            }
            //当前章节是否有试题
            if (canStudy)
            {
                bool isQues = Business.Do<IOutline>().OutlineIsQues(ol.Ol_ID, true);
                this.Document.Variables.SetValue("isQues", isQues);
                if (isQues) state = state < 1 ? 4 : state;
            }
            else
            {
                state = 0;
            }

            //章节事件
            OutlineEvent[] events = Business.Do<IOutline>().EventAll(-1, ol.Ol_ID, -1, true);
            this.Document.Variables.SetValue("events", events);
            this.Document.RegisterGlobalFunction(this.getEventQues);
            this.Document.RegisterGlobalFunction(this.getEventFeedback);
            this.Document.RegisterGlobalFunction(this.GetOrder);
            //状态
            this.Document.Variables.SetValue("state", state);
            this.Document.Variables.SetValue("olid", id);
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
    }
}