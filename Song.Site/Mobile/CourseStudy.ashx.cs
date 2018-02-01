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
        protected int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            //如果没有登录则跳转
            if (this.Account == null)
            {
                if (WeiSha.Common.Browser.IsMobile)
                    context.Response.Redirect("/Mobile/Login.ashx");
                else
                    context.Response.Redirect("/student/index.ashx");
                return;
            }
            //当前章节
            Song.Entities.Outline ol = null;
            ol = id < 1 ? Business.Do<IOutline>().OutlineFirst(couid, true)
                       : ol = Business.Do<IOutline>().OutlineSingle(id);  
            this.Document.Variables.SetValue("outline", ol);
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
                }
                this.Document.Variables.SetValue("vpath", Upload.Get["CourseVideo"].Virtual);
                this.Document.Variables.SetValue("IsVideoNoload", config["IsVideoNoload"].Value.Boolean ?? false);
            }
            //章节事件
            OutlineEvent[] events = Business.Do<IOutline>().EventAll(-1, ol.Ol_ID, -1, true);
            this.Document.Variables.SetValue("events", events);
            this.Document.RegisterGlobalFunction(this.getEventQues);
            this.Document.RegisterGlobalFunction(this.getEventFeedback);
            this.Document.RegisterGlobalFunction(this.GetOrder);
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