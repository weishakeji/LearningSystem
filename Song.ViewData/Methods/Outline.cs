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


namespace Song.ViewData.Methods
{
    public class Outline
    {
        /// <summary>
        /// 获取章节
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        [HttpGet(IsAllow = false)]
        public Song.Entities.Outline[] List(int couid, int pid)
        {
            return Business.Do<IOutline>().OutlineCount(couid, pid, true, 0);
        }

        /// <summary>
        /// 树形章节列表
        /// </summary>
        /// <param name="couid">所属课程的id</param>
        /// <returns></returns>
        public DataTable Tree(int couid)
        {
            // 当前课程的所有章节            
            Song.Entities.Outline[] outlines = Business.Do<IOutline>().OutlineAll(couid, true);
            foreach (Song.Entities.Outline ol in outlines)
            {
                ol.Ol_Intro = string.Empty;
            }
            //树形章节输出
            DataTable dt = Business.Do<IOutline>().OutlineTree(outlines);
            return dt;
        }
        /// <summary>
        /// 章节附件
        /// </summary>
        /// <param name="oluid">章节的uid</param>
        /// <returns></returns>
        public List<Song.Entities.Accessory> Accessory(string uid)
        {
            //先判断是否购买课程
            Song.Entities.Accounts acc = Extend.LoginState.Accounts.CurrentUser;
            if (acc == null) return new List<Accessory>();
            Song.Entities.Outline outline = Business.Do<IOutline>().OutlineSingle(uid);
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(outline.Cou_ID);
            //是否免费，或是限时免费
            if (course.Cou_IsLimitFree)
            {
                DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
                if (!(course.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                    course.Cou_IsLimitFree = false;
            }
            bool isBuy = course.Cou_IsFree || course.Cou_IsLimitFree ? true : Business.Do<ICourse>().IsBuy(course.Cou_ID, acc.Ac_ID);
            if (!isBuy) return new List<Accessory>();
            //获取附件
            List<Song.Entities.Accessory> access = Business.Do<IAccessory>().GetAll(uid, "Course");
            foreach (Accessory ac in access)
                ac.As_FileName = Upload.Get["Course"].Virtual + ac.As_FileName;
            return access;
        }
        /// <summary>
        /// 章节的状态
        /// </summary>
        /// <param name="olid"></param>
        /// <returns></returns>        
        public Dictionary<string, object> State(int olid)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();   
            Song.Entities.Outline outline = Business.Do<IOutline>().OutlineSingle(olid);
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(outline.Cou_ID);
             
            //是否免费，或是限时免费
            if (course.Cou_IsLimitFree)
            {
                DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
                if (!(course.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                    course.Cou_IsLimitFree = false;
            }
            Song.Entities.Accounts acc = Extend.LoginState.Accounts.CurrentUser;
            dic.Add("isLogin", acc != null);    //学员是否登录
            //是否可以学习，是否购买
            bool isStudy = false, isBuy = false, canStudy = false;
            if (acc != null)
            {
                isStudy = Business.Do<ICourse>().Study(course.Cou_ID, acc.Ac_ID);
                isBuy = course.Cou_IsFree || course.Cou_IsLimitFree ? true : Business.Do<ICourse>().IsBuy(course.Cou_ID, acc.Ac_ID);
            }
            dic.Add("isStudy", isStudy);
            dic.Add("isBuy", isBuy);
            //是否可以学习,如果是免费或已经选修便可以学习，否则当前课程允许试用且当前章节是免费的，也可以学习
            canStudy = isBuy || (isStudy && outline.Ol_IsUse && outline.Ol_IsFinish && course.Cou_IsTry && outline.Ol_IsFree);
            dic.Add("canStudy", canStudy);
            //是否有知识库
            int knlCount = Business.Do<IKnowledge>().KnowledgeOfCount(-1, course.Cou_ID, -1, true);
            dic.Add("isKnl", knlCount > 0);

            //是否有视频，是否为外链视频
            List<Song.Entities.Accessory> videos = Business.Do<IAccessory>().GetAll(outline.Ol_UID, "CourseVideo");
            bool existVideo = videos.Count > 0;
            dic.Add("existVideo", existVideo);
            dic.Add("outerVideo", existVideo && videos[0].As_IsOuter);
            string videoUrl = existVideo ? videos[0].As_FileName : string.Empty; //视频地址
            //如果是内部链接
            if (existVideo && !videos[0].As_IsOuter)
            {
                videoUrl = Upload.Get[videos[0].As_Type].Virtual + videoUrl;
                string fileHy = WeiSha.Common.Server.MapPath(videoUrl);
                if (!System.IO.File.Exists(fileHy))
                {
                    string ext = System.IO.Path.GetExtension(fileHy).ToLower();
                    if (ext == ".mp4") videoUrl = Path.ChangeExtension(videoUrl, ".flv");
                    if (ext == ".flv") videoUrl = Path.ChangeExtension(videoUrl, ".mp4");
                }
            }
            dic.Add("urlVideo", videoUrl);
            //是否有课程内容
            bool isContext=!string.IsNullOrWhiteSpace(outline.Ol_Intro);
            dic.Add("isContext", isContext);
            //是否有试题
            bool isQues=Business.Do<IOutline>().OutlineIsQues(outline.Ol_ID, true);
            dic.Add("isQues", isQues);
            //是否有附件
            int accessCount = Business.Do<IAccessory>().OfCount(outline.Ol_UID, "Course");
            dic.Add("isAccess", accessCount > 0);
            //啥都没有（视频，内容，附件，试题，都没有）
            dic.Add("isNull", !(existVideo || isContext || isQues || isQues || accessCount > 0));
            return dic;
        }
    }
}
