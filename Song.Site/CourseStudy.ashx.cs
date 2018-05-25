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
            if (!Extend.LoginState.Accounts.IsLogin || this.Account==null)
                context.Response.Redirect(WeiSha.Common.Login.Get["Accounts"].NoLoginPath.String);
            //自定义配置项
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);
            
            #region 章节输出
            //取当前章节
            ol = id < 1 ? Business.Do<IOutline>().OutlineFirst(couid, true)
                       : ol = Business.Do<IOutline>().OutlineSingle(id);
            //当前课程            
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(ol == null ? couid : ol.Cou_ID);
            if (course == null) return;
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
            if (course == null) isStudy = false;
            else
                isStudy = Business.Do<ICourse>().StudyIsCourse(this.Account.Ac_ID, course.Cou_ID);
            this.Document.Variables.SetValue("isStudy", isStudy);
            //课程章节列表
            this.Document.Variables.SetValue("outlines", outlines);
            //树形章节输出
            this.Document.Variables.SetValue("olTree", buildOutlineHtml(outlines, 0, 0, ""));
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
                    string fileHy = Server.MapPath(videos[0].As_FileName);
                    if (!System.IO.File.Exists(fileHy))
                    {
                        string ext = System.IO.Path.GetExtension(fileHy).ToLower();
                        if (ext == ".mp4") videos[0].As_FileName = Path.ChangeExtension(videos[0].As_FileName, ".flv");
                        if (ext == ".flv") videos[0].As_FileName = Path.ChangeExtension(videos[0].As_FileName, ".mp4");
                    }
                    this.Document.Variables.SetValue("video", videos[0]);
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
            //视频的学习进度记录
            LogForStudentStudy studyLog = Business.Do<IStudent>().LogForStudySingle(this.Account.Ac_ID, id);
            if (studyLog != null)
            {
                double historyPlay = (double)studyLog.Lss_PlayTime / 1000;
                this.Document.Variables.SetValue("historyPlay", historyPlay);
                this.Document.Variables.SetValue("studyLog", studyLog);
            }
            //当前章节是否有试题
            if (isStudy)
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
        /// <summary>
        /// 生成章节的多级结构html
        /// </summary>
        /// <param name="outline"></param>
        /// <param name="pid"></param>
        /// <param name="level"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private string buildOutlineHtml(Song.Entities.Outline[] outline, int pid, int level, string prefix)
        {
            int index = 1;
            string html = "";
            html += "<div class=\"outline" + (level > 0 ? " indent" : "") + "\" level=\"" + level + "\">";
            foreach (Song.Entities.Outline ol in outline)
            {
                if (ol.Ol_PID == pid)
                {
                    html += "<div class=\"olitem\" olid=\"" + ol.Ol_ID + "\">";
                    html += prefix + index.ToString() + ".";
                    if (isStudy)
                    {
                        html += "<a href=\"CourseStudy.ashx?id=" + ol.Ol_ID + "\">" + ol.Ol_Name + "</a>";
                    }
                    else
                    {
                        html += "<span>" + ol.Ol_Name + "</span>";
                    }
                    html += "</div>";
                    html += buildOutlineHtml(outline, ol.Ol_ID, ++level, prefix + index.ToString() + ".");
                    index++;
                }
            }
            html += "</div>";
            return html;
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