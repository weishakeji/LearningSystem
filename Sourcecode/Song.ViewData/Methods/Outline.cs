using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;
using System.Data;
using System.IO;
using pili_sdk;
using Newtonsoft.Json;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 章节信息
    /// </summary>
    [HttpGet, HttpPut]
    public class Outline : ViewMethod, IViewAPI
    {
        #region 增删改查
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Admin,Teacher]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public bool Add(Song.Entities.Outline entity)
        {
            try
            {
                Business.Do<IOutline>().OutlineAdd(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改章节
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Admin,Teacher]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public Song.Entities.Outline Modify(Song.Entities.Outline entity)
        {
            Song.Entities.Outline old = Business.Do<IOutline>().OutlineSingle(entity.Ol_ID);
            if (old == null) throw new Exception("Not found entity for Outline！");

            old.Copy<Song.Entities.Outline>(entity);
            Business.Do<IOutline>().OutlineSave(old);
            return old;
        }
        /// <summary>
        /// 更改章节的排序
        /// </summary>
        /// <param name="list">章节列表，对像只有Ol_ID、Ol_PID、Ol_Tax、Ol_Level</param>
        /// <returns></returns>
        [HttpPost]
        [Admin, Teacher]
        public bool ModifyTaxis(Song.Entities.Outline[] list)
        {
            try
            {
                Business.Do<IOutline>().UpdateTaxis(list);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除课程章节
        /// </summary>
        /// <param name="id">账户id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin,Teacher]
        [HttpDelete]
        public int Delete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            string[] arr = id.Split(',');
            foreach (string s in arr)
            {
                long idval = 0;
                long.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    Business.Do<IOutline>().OutlineDelete(idval);
                    i++;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return i;
        }
        /// <summary>
        /// 获取章节
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Cache(Expires = 20, AdminDisable = true)]
        public Song.Entities.Outline ForID(long id)
        {
            return Business.Do<IOutline>().OutlineSingle(id);
        }
        /// <summary>
        /// 章节列表
        /// </summary>
        /// <param name="couid">所属课程的id</param>
        /// <param name="pid">上级id</param>
        /// <returns></returns>
        //[HttpGet(Ignore = true)]
        [Cache(Expires = 20)]
        public List<Song.Entities.Outline> List(long couid, long pid)
        {
            return Business.Do<IOutline>().OutlineCount(couid, pid, true, 0);
        }
        /// <summary>
        /// 刷新课程下的章节缓存
        /// </summary>
        /// <param name="couid"></param>
        /// <returns></returns>
        [HttpPut,HttpPost]
        public bool FreshCache(long couid)
        {
            try
            {
                Business.Do<IOutline>().BuildCache(couid);
                return true;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// 当前课程所包含的所有下级章节数量（不包括自身）
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <param name="use">是否包括启用的课程,null取所有，true取启用的，false取未启用的</param>
        /// <returns></returns>
        public int CountOfChildren(long couid, long olid, bool? use)
        {
            return Business.Do<IOutline>().OutlineOfCount(couid, olid, use, true);
        }
        /// <summary>
        /// 树形章节列表
        /// </summary>
        /// <param name="couid">所属课程的id</param>
        /// <returns></returns>
        [HttpGet,HttpPost,HttpPut]
        [Cache(Expires=120, AdminDisable = true)]
        public DataTable TreeList(long couid)
        {
            // 当前课程的所有章节
            List<Song.Entities.Outline> outlines = Business.Do<IOutline>().OutlineAll(couid, true, true, null);
            if (outlines.Count > 0)
            {
                //foreach (Song.Entities.Outline ol in outlines) ol.Ol_Intro = string.Empty;
                //树形章节输出
                DataTable dt = Business.Do<IOutline>().OutlineTree(outlines.ToArray<Song.Entities.Outline>());
                return dt;
            }
            return null;
        }
        /// <summary>
        /// 课程章节，树形数据
        /// </summary>
        /// <param name="couid">所属课程的id</param>
        /// <param name="isuse">是否启用</param>
        /// <returns></returns>
        [Cache(AdminDisable =true)]
        public JArray Tree(long couid, bool? isuse)
        {
            if (couid <= 0) return null;
            List<Song.Entities.Outline> list = Business.Do<IOutline>().OutlineAll(couid, isuse, null, null);           
            return list.Count > 0 ? _outlineNode(null, list) : null;
        }
        /// <summary>
        /// 生成菜单子节点
        /// </summary>
        /// <param name="item">当前菜单项</param>
        /// <param name="items">所有菜单项</param>
        /// <returns></returns>
        private JArray _outlineNode(Song.Entities.Outline item, List<Song.Entities.Outline> items)
        {
            JArray jarr = new JArray();
            foreach (Song.Entities.Outline m in items)
            {
                if (item == null)
                {
                    if (m.Ol_PID != 0) continue;
                }
                else
                {
                    if (m.Ol_PID != item.Ol_ID) continue;
                }
                string j = m.ToJson("", "Ol_LiveTime,Ol_ModifyTime,Ol_Intro,Ol_Courseware");
                JObject jo = JObject.Parse(j);               
                //计算下级
                JArray charray = _outlineNode(m, items);
                if (charray.Count > 0) jo.Add("children", charray);
                jarr.Add(jo);
            }
            return jarr;
        }
        /// <summary>
        /// 视频事件
        /// </summary>
        /// <param name="olid"></param>
        /// <returns></returns>
        [Cache(Expires = 20, AdminDisable = true)]
        public OutlineEvent[] VideoEvents(long olid)
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
        #region 章节的状态
        /// <summary>
        /// 章节相对学员的状态，例如是否可以学习，学习进度等
        /// </summary>
        /// <param name="olid">章节id</param>
        /// <param name="acid"></param>
        /// <returns></returns>      
        [Study]
        public JObject State(long olid, int acid)
        {
            JObject dic = new JObject();
            dic.Add("isLogin", true);
            //Song.Entities.Accounts acc = this.User;
            //dic.Add("isLogin", acc != null);    //学员是否登录
            ////
            //Song.Entities.Outline outline = Business.Do<IOutline>().OutlineSingle(olid);
            //if (outline == null) throw new Exception("章节不存在");
            //dic.Add("Name", outline.Ol_Name);
            //Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(outline.Cou_ID);
            //if (course == null) throw new Exception("课程不存在");
            //dic.Add("Course", course.Cou_Name);
            //Song.Entities.Organization orgin;
            ////是否限制在桌面应用中打开
            //dic.Add("DeskAllow", this.getDeskallow(course, outline, out orgin));
            ////是否在切换浏览器时继续播放
            //dic.Add("SwitchPlay", this.getSwitchPlay(course, acc, orgin));
            ////是否免费，或是限时免费
            //if (course.Cou_IsLimitFree)
            //{
            //    DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
            //    if (!(course.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
            //        course.Cou_IsLimitFree = false;
            //}
            ////是否可以学习，是否购买
            //bool isStudy = false, isBuy = false, canStudy = false;
            //if (acc != null)
            //{
            //    isStudy = Business.Do<ICourse>().AllowStudy(course, acc);
            //    isBuy = course.Cou_IsFree || course.Cou_IsLimitFree ? true : Business.Do<ICourse>().IsBuy(course.Cou_ID, acc.Ac_ID);

            //}
            //学习记录
            Song.Entities.LogForStudentStudy studyLog = Business.Do<IStudent>().LogForStudySingle(acid, olid);
            dic.Add("StudyTime", studyLog != null ? studyLog.Lss_StudyTime : 0);
            dic.Add("PlayTime", studyLog != null ? studyLog.Lss_PlayTime : 0);
            dic.Add("Complete", studyLog != null ? studyLog.Lss_Complete : 0);

            //dic.Add("isStudy", isStudy);
            //dic.Add("isBuy", isBuy);
            ////是否可以学习,如果是免费或已经选修便可以学习，否则当前课程允许试用且当前章节是免费的，也可以学习
            //canStudy = isStudy && outline.Ol_IsUse && outline.Ol_IsFinish;
            dic.Add("canStudy", true);
            return dic;
        }
        /// <summary>
        /// 章节的相关信息，例如是否有视频，是否有附件
        /// </summary>
        /// <param name="olid"></param>
        /// <returns></returns>
        [Cache]
        public JObject Info(long olid)
        {
            JObject dic = new JObject();
            if (olid <= 0) return dic;
            Song.Entities.Outline outline = Business.Do<IOutline>().OutlineSingle(olid);
            if (outline == null) throw new Exception("章节不存在");
            dic.Add("name", outline.Ol_Name);
            dic.Add("couid", outline.Cou_ID);
            dic.Add("id", outline.Ol_ID);          
            //是否有知识库
            int knlCount = Business.Do<IKnowledge>().KnowledgeOfCount(-1, outline.Cou_ID, 0, true);
            dic.Add("isKnl", knlCount > 0);
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
                    videoUrl = WeiSha.Core.Upload.Get[videos[0].As_Type].Virtual + videoUrl;
                    string ext = System.IO.Path.GetExtension(videoUrl).ToLower();
                    if (ext == ".flv") videoUrl = Path.ChangeExtension(videoUrl, ".mp4");
                }
                dic.Add("urlVideo", videoUrl);
                outline.Ol_IsLive = false;
            }
            //直播  
            bool isLive = outline.Ol_IsLive, isLiving = false;
            if (outline.Ol_IsLive)
            {
                string urlVideo = string.Empty;
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
                //直播播放地址
                if (!dic.ContainsKey("urlVideo"))
                    dic.Add("urlVideo", urlVideo);
                //直播开始或结束
                dic.Add("LiveStart", DateTime.Now > outline.Ol_LiveTime);
                dic.Add("LiveOver", outline.Ol_LiveTime.AddMinutes(outline.Ol_LiveSpan) < DateTime.Now);
            }
            dic.Add("isLive", outline.Ol_IsLive);   //是否为直播章节
            dic.Add("isLiving", isLiving);  //是否在直播中
            dic.Add("existVideo", existVideo);

            //是否有课程内容
            bool isContext = !string.IsNullOrWhiteSpace(outline.Ol_Intro);
            dic.Add("isContext", isContext);
            //是否有试题
            bool isQues = Business.Do<IOutline>().OutlineIsQues(outline.Ol_ID, true);
            dic.Add("isQues", isQues);
            //是否有附件
            int accessCount = Business.Do<IAccessory>().OfCount(-1, outline.Ol_UID, "Course");
            dic.Add("isAccess", accessCount > 0);
            //啥都没有（视频，内容，附件，试题，都没有）
            bool isNull = !(existVideo || isLive || isContext || isQues || isQues || accessCount > 0);
            dic.Add("isNull", isNull);
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
            WeiSha.Core.CustomConfig config = CustomConfig.Load(organ.Org_Config);
            //是否限制在桌面应用中学习
            bool studyFordesk = config["StudyForDeskapp"].Value.Boolean ?? false;   //课程学习需要在桌面应用打开
            bool freeFordesk = config["FreeForDeskapp"].Value.Boolean ?? false;     //免费课程和试用章节除外
            if (!WeiSha.Core.Browser.IsDestopApp)
            {
                if (!freeFordesk)
                {
                    return studyFordesk && !WeiSha.Core.Browser.IsDestopApp;
                }
                else
                {
                    if (course.Cou_IsFree || course.Cou_IsLimitFree) return false;
                    if (ol.Ol_IsFree) return false;
                }
            }
            return true && !WeiSha.Core.Browser.IsDestopApp;
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
            WeiSha.Core.CustomConfig config = CustomConfig.Load(organ.Org_Config);
            bool isstop = config["IsSwitchPlay"].Value.Boolean ?? false;
            if (isstop) return true;
            //如果机构设置中为false，继续判断学员组的设置
            Song.Entities.StudentSort sort = Business.Do<IStudent>().SortSingle(acc.Sts_ID);
            if (sort == null || !sort.Sts_IsUse) return isstop;
            return sort.Sts_SwitchPlay;
        }
        #endregion
    }
}
