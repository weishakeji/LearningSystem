using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;

namespace Song.Site
{
    /// <summary>
    /// 视频播放地址，由此转码
    /// </summary>
    public partial class VideoUrl : System.Web.UI.Page
    {
        //附件的as_uid，如果用参数传递了，就不再取文件名了
        string uid = WeiSha.Common.Request.QueryString["uid"].String;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(uid)) uid = WeiSha.Common.Request.Page.Name;
            //提示消息
            string msg = string.Empty;
            //是否要播放
            bool isPlay = judge(uid, out msg);
            if (!isPlay)
            {
                Response.Write(msg);
                Response.End();
            }
            else
            {
                //***************如果可以播放
                Song.Entities.Accessory acc = Business.Do<IAccessory>().GetSingle(uid);
                if (acc == null)
                {
                    Response.Write("视频不存在！");
                    Response.End();
                }
                else
                {
                    //视频文件
                    string video = string.Empty;
                    //如果是系统内部文件
                    if (!acc.As_IsOuter)
                    {

                        video = Upload.Get[acc.As_Type].Physics + acc.As_FileName;
                        //如果不是手机端
                        if (!WeiSha.Common.Browser.IsMobile)
                        {
                            //如果不存在，则后缀名改成mp4的试试
                            if (!System.IO.File.Exists(video))
                                video = reExstname(video, "mp4");
                        }
                        else
                        {
                            video = reExstname(video, "mp4");
                        }
                    }
                    else
                    {
                        video = acc.As_FileName;    //如果是外部视频链接；
                    }
                    HttpContext context = System.Web.HttpContext.Current;
                    context.Response.Clear();
                    context.Response.ClearContent();
                    context.Response.ClearHeaders();
                    context.Response.AddHeader("Content-Disposition", "attachment;filename=" + acc.As_FileName);
                    System.IO.FileInfo fi = new System.IO.FileInfo(video);
                    context.Response.AddHeader("Content-Length", fi.Length.ToString());
                    context.Response.AddHeader("Content-Transfer-Encoding", "binary");
                    context.Response.ContentType = "application/octet-stream";
                    context.Response.ContentEncoding = System.Text.Encoding.Default;
                    context.Response.WriteFile(video);
                    context.Response.Flush();
                    context.Response.End(); 
                }
               
            }

        }
        /// <summary>
        /// 输出是否可以播放
        /// </summary>
        /// <param name="uid">文件名即Uid,是附件的as_uid</param>
        /// <param name="msg">输出提示</param>
        /// <returns>是否要播放</returns>
        private bool judge(string uid,out string msg)
        {
            msg = "";
            ////如果没有来源页信息
            //if (this.Request.UrlReferrer == null)
            //{
            //    msg = "请勿在本站以外访问！";
            //    return false;
            //}
            ////如果来源页与当前请求不在一个主域
            //if (Request.UrlReferrer.Host != Request.Url.Host)
            //{
            //    msg = "请勿在本站以外访问！";
            //    return false;
            //} 
            //如果学员没有登录
            if (!Extend.LoginState.Accounts.IsLogin)
            {
                msg = "学员未登录，视频不允许播放！";
                return false;
            }
            //首先获取章节
            Song.Entities.Outline outline = Business.Do<IOutline>().OutlineSingle(uid);
            if (outline == null || outline.Ol_IsUse == false)
            {
                msg = "当前视频所在章节不存在或已经被禁用";
                return false;
            }
            //判断是否与当前章节在同一环境下
            if (WeiSha.Common.Request.Cookies["olid"].String != outline.Ol_ID.ToString())
            {
                msg = "请勿在本站以外访问！";
                return false;
            }
            //获取课程
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(outline.Cou_ID);
            if (course == null || course.Cou_IsUse == false)
            {
                msg = "当前视频所在课程不存在或已经被禁用";
                return false;
            }
            //判断是否选修
            Song.Entities.Accounts st = Extend.LoginState.Accounts.CurrentUser;
            bool isBuy = Business.Do<ICourse>().StudyIsCourse(st.Ac_ID, course.Cou_ID);    //是否购买 
            bool istry = Business.Do<ICourse>().IsTryout(course.Cou_ID, st.Ac_ID);  //是否在试用中
            if (!isBuy && !istry)
            {
                msg = "当前学员未选修视频所在的课程";
                return false;
            }
            return true;
        }
        /// <summary>
        /// 更改文件的扩展名
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="exis"></param>
        /// <returns></returns>
        private string reExstname(string filename, string exis)
        {
            if (filename.IndexOf(".") > -1)
                filename = filename.Substring(0, filename.LastIndexOf("."));
            filename += "." + exis;
            return filename;
        }
    }
}