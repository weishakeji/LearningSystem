using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using System.Web.Mvc;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;


namespace Song.ViewData.Methods
{
    /// <summary>
    /// 课程管理
    /// </summary>
    [HttpGet,HttpPut]
    public class Course : ViewMethod, IViewAPI
    {
        //资源的虚拟路径和物理路径
        public static string PathKey = "Course";
        public static string VirPath = WeiSha.Core.Upload.Get[PathKey].Virtual;
        public static string PhyPath = WeiSha.Core.Upload.Get[PathKey].Physics;

        #region 课程的获取、增删改查        
        /// <summary>
        /// 课程是否已经存在，用课程名称判断，只判断当前专业下的课程是否重名
        /// </summary>
        /// <param name="name">课程名称</param>
        /// <param name="orgid"></param>
        /// <param name="sbjid">专业id</param>  
        /// <returns></returns>
        public bool NameExist(string name, int orgid, long sbjid)
        {
            Song.Entities.Course cur = Business.Do<ICourse>().CourseIsExist(orgid, sbjid, -1, name);
            return cur != null;
        }
        /// <summary>
        /// 获取或增加课程的浏览数
        /// </summary>
        /// <param name="couid">课程ID</param>
        /// <param name="num">要增总的浏览数，不可以为负数或零</param>
        /// <returns></returns>
        [HttpPut]
        public int ViewNum(long couid, int num)
        {
            return Business.Do<ICourse>().CourseViewNum(couid, num);
        }
        /// <summary>
        /// 根据课程ID获取课程信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Cache(Expires = 60, AdminDisable = true)]
        public Song.Entities.Course ForID(long id)
        {
            Song.Entities.Course cur = Business.Do<ICourse>().CourseSingle(id);
            return _tran(cur);
        }
        /// <summary>
        /// 根据课程UID获取课程信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [Cache(Expires = 60, AdminDisable = true)]
        public Song.Entities.Course ForUID(string uid)
        {
            Song.Entities.Course cur = Business.Do<ICourse>().CourseSingle(uid);
            return _tran(cur);
        }

        ///<summary>
        /// 创建课程
        /// </summary>
        /// <param name="name">课程名称</param>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">所属专业的id</param>
        /// <param name="thid">教师id</param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpPost, HttpGet(Ignore = true)]
        [Upload(Extension = "jpg,png,gif", MaxSize = 1024, CannotEmpty = false)]
        public Song.Entities.Course Add(string name, int orgid, long sbjid, int thid)
        {
            try
            {
                Song.Entities.Course entity = new Entities.Course();
                entity.Cou_Name = name;
                entity.Org_ID = orgid;
                if (thid > 0)
                {
                    entity.Th_ID = thid;
                    Song.Entities.Teacher teacher = this.Teacher;
                    if (!(teacher != null && teacher.Th_ID == thid))

                        teacher = Business.Do<ITeacher>().TeacherSingle(thid);
                    entity.Th_Name = teacher.Th_Name;
                    if (teacher != null) entity.Th_Name = teacher.Th_Name;
                }
                entity.Sbj_ID = sbjid;
                //接收上传的图片
                entity = this._upload_photo(this.Files, entity);

                Business.Do<ICourse>().CourseAdd(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改课程信息
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpPost, HttpGet(Ignore = true)]
        [Upload(Extension = "jpg,png,gif", MaxSize = 1024, CannotEmpty = false)]
        [HtmlClear(Not = "course")]
        public Song.Entities.Course Modify(JObject course)
        {
            try
            {
                long couid = 0;
                long.TryParse(course["Cou_ID"].ToString(), out couid);
                Song.Entities.Course old = Business.Do<ICourse>().CourseSingle(couid);
                if (old == null) throw new Exception("Not found entity for Course！");
                //
                string cou_login = course["Cou_Logo"] != null ? course["Cou_Logo"].ToString() : string.Empty;
                if (this.Files.Count <= 0 && string.IsNullOrWhiteSpace(cou_login) && !string.IsNullOrWhiteSpace(old.Cou_Logo))
                    WeiSha.Core.Upload.Get["Course"].DeleteFile(old.Cou_Logo);
                //接收上传的图片
                if (this.Files.Count > 0)
                {
                    if (!string.IsNullOrWhiteSpace(old.Cou_Logo))
                        WeiSha.Core.Upload.Get["Course"].DeleteFile(old.Cou_Logo);
                    old = this._upload_photo(this.Files, old);                   
                }
                //某些字段将不同步修改                   
                string nomidfy = "Cou_CrtTime,Cou_StudentSum,Cou_UID,Th_ID,Th_Name,Org_ID,Org_Name,Cou_Logo,Cou_LogoSmall";
                ////如果名称为空，则不修改
                //if (string.IsNullOrWhiteSpace(course.Cou_Name))
                //    nomidfy = "Cou_Name," + nomidfy;
                old.Copy<Song.Entities.Course>(course, nomidfy);
                Business.Do<ICourse>().CourseSave(old);
                return old;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        ///  修改课程信息
        /// </summary>
        /// <param name="course">course的Json对象</param>
        /// <returns></returns>
        [Admin,Teacher]
        [HttpPost, HttpGet(Ignore = true)]
        [Upload(Extension = "jpg,png,gif", MaxSize = 1024, CannotEmpty = false)]
        [HtmlClear(Not = "course")]
        public Song.Entities.Course ModifyJson(JObject course)
        {
            //由JObject对象转为 Song.Entities.Course对象
            Song.Entities.Course cour = new Entities.Course();
            cour.Copy<Song.Entities.Course>(course);          
            //获取历史记录
            Song.Entities.Course old = Business.Do<ICourse>().CourseSingle(cour.Cou_ID);
            if (old == null) throw new Exception("Not found entity for Course！");
            //将新的值传给历史记录
            old.Copy<Song.Entities.Course>(course);
            Business.Do<ICourse>().CourseSave(old);
            return old;
        }
        private Song.Entities.Course _upload_photo(HttpFileCollectionBase files,Song.Entities.Course course)
        {
            string filename = string.Empty, smallfile = string.Empty;
            //只保存第一张图片
            foreach (string key in this.Files)
            {
                HttpPostedFileBase file = this.Files[key];
                filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                file.SaveAs(PhyPath + filename);
                //生成缩略图
                smallfile = WeiSha.Core.Images.Name.ToSmall(filename);
                WeiSha.Core.Images.FileTo.Thumbnail(PhyPath + filename, PhyPath + smallfile, 320, 180, 0);
                break;
            }
            course.Cou_Logo = filename;
            course.Cou_LogoSmall = smallfile;
            return course;
        }
        /// <summary>
        /// 修改课程的状态
        /// </summary>
        /// <param name="id">课程的id</param>
        /// <param name="use">是否启用</param>
        /// <param name="rec">是否推荐</param>
        /// <param name="edit">是否允许编辑</param>
        /// <returns></returns>
        [HttpPost]
        [Admin, Teacher]
        public int ModifyState(string id, bool? use, bool? rec, bool? edit)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            if (use == null && rec == null) return i;
            string[] arr = id.Split(',');
            foreach (string s in arr)
            {
                long idval = 0;
                long.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    if (use != null && rec != null)
                    {
                        Business.Do<ICourse>().CourseUpdate(idval,
                       new WeiSha.Data.Field[] {
                        Song.Entities.Course._.Cou_IsUse,Song.Entities.Course._.Cou_IsRec,Song.Entities.Course._.Cou_Allowedit },
                       new object[] { (bool)use, (bool)rec, (bool)edit });
                    }
                    else
                    {
                        if (use != null) Business.Do<ICourse>().CourseUpdate(idval, new WeiSha.Data.Field[] { Song.Entities.Course._.Cou_IsUse }, new object[] { (bool)use });
                        else Business.Do<ICourse>().CourseUpdate(idval, new WeiSha.Data.Field[] { Song.Entities.Course._.Cou_IsRec }, new object[] { (bool)rec });
                    }
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
        /// 修改课程的名称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="couid"></param>
        /// <returns></returns>
        [HttpPost]
        [Admin, Teacher]
        public bool ModifyName(string name, long couid)
        {
            //只有教师登录，管理员没有登录
            if (this.Teacher != null && this.Admin == null)
            {
                Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
                if (course != null)
                {
                    if (course.Th_ID != this.Teacher.Th_ID)
                    {
                        throw new Exception("当前登录的教师无权操作该课程");
                    }
                }
            }
            //
            try
            {
                Business.Do<ICourse>().CourseUpdate(couid,
                    new WeiSha.Data.Field[] {
                        Song.Entities.Course._.Cou_Name},
                    new object[] { name });
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除课程
        /// </summary>
        /// <param name="id">课程id，可以是多个，用逗号分隔</param>
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
                    Business.Do<ICourse>().CourseDelete(idval);
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
        /// 课程的数据信息
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <returns>
        /// "outline":章节数,MostHot
        /// "question":试题数,
        /// "video":视频数,
        /// "student":当前课程的学习人数,
        /// "view":浏览数,
        /// "live":是否为直播课程
        /// </returns>
        [Cache]
        public JObject Datainfo(long couid)
        {
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
            if (course == null) throw new Exception("Not found entity for Course！");
            //章节数
            int outline = Business.Do<IOutline>().OutlineOfCount(couid, -1, true);
            //试题数
            int qus = Business.Do<IQuestions>().QuesOfCount(-1, -1, couid, -1, 0, -1, true);
            //知识点
            int knl = Business.Do<IKnowledge>().KnowledgeOfCount(-1, couid, -1, true);
            //课程通知
            int guide = Business.Do<IGuide>().GuideOfCount(-1, couid, null, true);
            //视频数
            int video = Business.Do<IOutline>().OutlineOfCount(couid, -1, true, true, true, null);
            //学习人数
            int student = Business.Do<ICourse>().CourseStudentSum(couid, true);
            //试卷数
            int testpaper = Business.Do<ITestPaper>().PaperOfCount(-1, -1, couid, -1, true);
            //结课考试
            Song.Entities.TestPaper final = Business.Do<ITestPaper>().FinalPaper(couid, null);
            if (final != null) testpaper--;

            JObject jo = new JObject();
            jo.Add("outline", outline);
            jo.Add("question", qus);
            jo.Add("knowledge", knl);
            jo.Add("guide", guide);
            jo.Add("testpaper", testpaper);
            jo.Add("testfinal", final != null && final.Tp_IsUse ? 1 : 0);
            jo.Add("video", video);
            jo.Add("student", student);
            jo.Add("view", course.Cou_ViewNum);     //课程浏览数
            jo.Add("live", course.Cou_ExistLive);       //是否为直播课
            return jo;
        }
        #endregion

        #region 课程价格   

        /// <summary>
        /// 获取课程价格信息(仅限起用的价项，用于前端展现）
        /// </summary>
        /// <param name="uid">课程的uid，注意不是id</param>
        /// <returns></returns>
        [HttpGet, HttpPut]
        [Cache(Expires = 60)]
        public Song.Entities.CoursePrice[] Prices(string uid)
        {
            if (string.IsNullOrWhiteSpace(uid)) return null;
            Song.Entities.CoursePrice[] prices = Business.Do<ICourse>().PriceCount(0, uid, true, 0);
            return prices;
        }
        /// <summary>
        /// 获取课程价格的所有信息（用于后台管理）
        /// </summary>
        /// <param name="uid">课程的uid，注意不是id</param>
        /// <returns></returns>
        [HttpGet, HttpPut]
        [Admin, Teacher]
        public Song.Entities.CoursePrice[] PriceItems(string uid)
        {
            if (string.IsNullOrWhiteSpace(uid)) return null;           
            Song.Entities.CoursePrice[] prices = Business.Do<ICourse>().PriceCount(0, uid, null, 0);
            return prices;
        }
        /// <summary>
        /// 更改课程价格的排序
        /// </summary>
        /// <param name="items">课程价格的设置项</param>
        /// <returns></returns>
        [HttpPost]
        [Admin,Teacher]
        public bool PriceUpdateTaxis(Song.Entities.CoursePrice[] items)
        {
            try
            {
                Business.Do<ICourse>().PriceUpdateTaxis(items);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 添加课程价格的设置项
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpPost]
        public bool PriceAdd(Song.Entities.CoursePrice entity)
        {
            try
            {
                Business.Do<ICourse>().PriceAdd(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改课程价格的设置项
        /// </summary>
        /// <param name="entity">价格设置项</param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpPost]
        public bool PriceModify(CoursePrice entity)
        {
            Song.Entities.CoursePrice old = Business.Do<ICourse>().PriceSingle(entity.CP_ID);
            if (old == null) throw new Exception("Not found entity for CoursePrice！");

            old.Copy<Song.Entities.CoursePrice>(entity);
            Business.Do<ICourse>().PriceSave(old);
            return true;
        }
        /// <summary>
        /// 删除课程价格设置项
        /// </summary>
        /// <param name="id">价格项的id</param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpDelete]
        public int PriceDelete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            string[] arr = id.Split(',');
            foreach (string s in arr)
            {
                int idval = 0;
                int.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    Business.Do<ICourse>().PriceDelete(idval);
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
        /// 获取当前课程的收益
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        public decimal Income(long couid)
        {
            return Business.Do<ICourse>().Income(couid);
        }
        #endregion

        #region 课程列表
        /// <summary>
        /// 分页获取课程,用于后端，包括被禁用的课程
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjids">章节id，可以为多个，以逗号分隔</param>
        /// <param name="thid">教师id</param>
        /// <param name="search">检索字符，按课程名称</param>
        /// <param name="order">排序方式,排序方式，def:默认，先推荐，然后按访问量倒序;flux：按访问量倒序;tax：按自定义排序要求;new:按创建时间，最新发布在前面;rec:按推荐，先推荐，然后按tax排序</param>
        /// <param name="size">每页几条</param>
        /// <param name="index">第几页</param>
        /// <returns></returns>
        public ListResult Pager(int orgid, string sbjids, int thid, string search, string order,int size, int index)
        {
            size = size <= 0 ? int.MaxValue : size;
            int count = 0;
            List<Song.Entities.Course> eas = null;
            eas = Business.Do<ICourse>().CoursePager(orgid, sbjids,thid, null, search, order, size, index, out count);
            for (int i = 0; i < eas.Count; i++)          
                eas[i] = _tran(eas[i]);
            //ListResult result = new ListResult(eas.ToArray<Song.Entities.Course>());
            ListResult result = new ListResult(eas);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 分页获取课程,用于前端，只限允许的课程
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjids">章节id，可以为多个，以逗号分隔；如果为空或为0，取推荐课程</param>
        /// <param name="search">检索字符，按课程名称</param>
        /// <param name="order">排序方式,排序方式，def:默认，先推荐，然后按访问量倒序;flux：按访问量倒序;tax：按自定义排序要求;new:按创建时间，最新发布在前面;rec:按推荐，先推荐，然后按tax排序</param>
        /// <param name="size">每页几条</param>
        /// <param name="index">第几页</param>
        /// <returns></returns>
        [Cache]
        public ListResult ShowPager(int orgid, string sbjids,  string search, string order, int size, int index)
        {
            int count = 0;
            List<Song.Entities.Course> eas = null;
            if (sbjids == "0") sbjids = "";
            if (string.IsNullOrWhiteSpace(order))
            {
                order = "new";
                if (string.IsNullOrWhiteSpace(sbjids) || sbjids == "0")
                    order = "rec";
            }
            eas = Business.Do<ICourse>().CoursePager(orgid, sbjids, 0, true, search, order, size, index, out count);
            for (int i = 0; i < eas.Count; i++)
            {
                Song.Entities.Course c = _tran(eas[i]);
                c.Cou_Intro = c.Cou_Target = c.Cou_Content = "";
                if (!string.IsNullOrWhiteSpace(c.Cou_Name))
                    c.Cou_Name = c.Cou_Name.Replace("\"", "&quot;");
                eas[i] = c;
            }
            ListResult result = new ListResult(eas);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 根据专业id，获取指定个数的课程
        /// </summary>
        /// <param name="sbjid">专业id</param>
        /// <param name="orgid">机构id</param>
        /// <param name="search">检索</param>
        /// <param name="order">排序方式,排序方式，def:默认，先推荐，然后按访问量倒序;flux：按访问量倒序;tax：按自定义排序要求;new:按创建时间，最新发布在前面;rec:按推荐，先推荐，然后按tax排序</param>
        /// <param name="count">取多少条记录</param>
        /// <returns></returns>
        public List<Song.Entities.Course> ShowCount(long sbjid,int orgid, string search, string order, int count)
        {
            List<Song.Entities.Course> eas = null;
            if (string.IsNullOrWhiteSpace(order))
                order = "rec";
            eas = Business.Do<ICourse>().CourseCount(orgid, sbjid, search, true, order, count);
            for (int i = 0; i < eas.Count; i++)
            {
                Song.Entities.Course c = _tran(eas[i]);
                c.Cou_Intro = c.Cou_Target = c.Cou_Content = "";
                if(!string.IsNullOrWhiteSpace(c.Cou_Name))
                    c.Cou_Name = c.Cou_Name.Replace("\"", "&quot;");
                eas[i] = c;
            }
            return eas;
        }
        /// <summary>
        ///  购买的课程
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="search">按课程检索</param>
        /// <param name="enable">是否使用中的记录,null取所有</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult Purchased(int acid, string search, bool? enable, int size, int index)
        {
            int count = 0;
            List<Song.Entities.Course> courses = Business.Do<ICourse>().CourseForStudent(acid, search, 1, enable, false, size, index, out count);
            for (int i = 0; i < courses.Count; i++)
            {
                Song.Entities.Course c = _tran(courses[i]);
                c.Cou_Intro = c.Cou_Target = c.Cou_Content = "";
                if (!string.IsNullOrWhiteSpace(c.Cou_Name))
                    c.Cou_Name = c.Cou_Name.Replace("\"", "&quot;");
                courses[i] = c;
            }
            ListResult result = new ListResult(courses);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 过期的课程
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="search">按课程检索</param>
        /// <param name="enable">是否使用中的记录,null取所有</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult Overdue(int acid, string search, bool? enable, int size, int index)
        {
            int count = 0;
            List<Song.Entities.Course> courses = Business.Do<ICourse>().CourseForStudent(acid, search, 2, enable, false, size, index, out count);
            for (int i = 0; i < courses.Count; i++)
            {
                Song.Entities.Course c = _tran(courses[i]);
                c.Cou_Intro = c.Cou_Target = c.Cou_Content = "";
                if (!string.IsNullOrWhiteSpace(c.Cou_Name))
                    c.Cou_Name = c.Cou_Name.Replace("\"", "&quot;");
                courses[i] = c;
            }
            ListResult result = new ListResult(courses);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;                 
        }
        /// <summary>
        /// 快要过期的课程
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="day">剩余几天内的</param>
        /// <returns></returns>
        public JArray OverdueSoon(int acid, int day)
        {
            List<Song.Entities.Student_Course> list = Business.Do<ICourse>().OverdueSoon(acid, day);
            if (list == null || list.Count < 1) { return null; }
            JArray jarr = new JArray();
            foreach (Student_Course sc in list)
            {
                Song.Entities.Course cou = Business.Do<ICourse>().CourseSingle(sc.Cou_ID);
                if (cou == null) continue;
                cou.Cou_Intro = cou.Cou_Target = string.Empty;
                JObject jo = cou.ToJObject();
                jo.Add("end",sc.Stc_EndTime.ToJsString());
                jo.Add("enddate", sc.Stc_EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                jo.Add("study",sc.Stc_StudyScore);
                jo.Add("exam", sc.Stc_ExamScore);
                jo.Add("ques", sc.Stc_QuesScore);
                jarr.Add(jo);
            }
            return jarr;
        }
        /// <summary>
        /// 试用的课程
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="search">按课程检索</param>
        /// <param name="enable">是否使用中的记录,null取所有</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult Ontrial(int acid, string search, bool? enable, int size, int index)
        {
            int count = 0;
            List<Song.Entities.Course> courses = Business.Do<ICourse>().CourseForStudent(acid, search, -1, enable, true, size, index, out count);
            for (int i = 0; i < courses.Count; i++)
            {
                Song.Entities.Course c = _tran(courses[i]);
                c.Cou_Intro = c.Cou_Target = c.Cou_Content = "";
                if (!string.IsNullOrWhiteSpace(c.Cou_Name))
                    c.Cou_Name = c.Cou_Name.Replace("\"", "&quot;");
                courses[i] = c;
            }
            ListResult result = new ListResult(courses);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 学员的所有课程
        /// </summary>
        /// <param name="acid"></param>
        /// <param name="search"></param>
        /// <param name="enable">是否使用中的记录,null取所有</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult ForStudent(int acid, string search, bool? enable, int size, int index)
        {
            int count = 0;
            List<Song.Entities.Course> courses = Business.Do<ICourse>().CourseForStudent(acid, search, 0, enable, false, size, index, out count);
            for (int i = 0; i < courses.Count; i++)
            {
                Song.Entities.Course c = _tran(courses[i]);
                c.Cou_Intro = c.Cou_Target = c.Cou_Content = "";
                if (!string.IsNullOrWhiteSpace(c.Cou_Name))
                    c.Cou_Name = c.Cou_Name.Replace("\"", "&quot;");
                courses[i] = c;
            }
            ListResult result = new ListResult(courses);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 判断课程是否存在于学员组的关联
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stsid">学员组id</param>
        /// <returns></returns>
        public bool ExistStudentSort(long couid,long stsid)
        {
            return Business.Do<IStudent>().SortExistCourse(couid, stsid);
        }
        #endregion

        #region 课程统计
        /// <summary>
        /// 热门课程
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="count">取多少条记录</param>
        /// <returns></returns>
        public DataTable MostHot(int orgid, long sbjid, int count)
        {
            DataSet ds = Business.Do<ICourse>().CourseHot(orgid, sbjid, count);
            if (ds == null || ds.Tables.Count < 1) return null;
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                dr["Cou_Logo"] = System.IO.File.Exists(PhyPath + dr["Cou_Logo"].ToString()) ? VirPath + dr["Cou_Logo"].ToString() : "";
                dr["Cou_LogoSmall"] = System.IO.File.Exists(PhyPath + dr["Cou_LogoSmall"].ToString()) ? VirPath + dr["Cou_LogoSmall"].ToString() : "";
            }
            return ds.Tables[0];
        }
        #endregion

        #region 学习记录
        /// <summary>
        /// 记录当前学员的视频学习进度
        /// </summary>
        /// <param name="couid">课程ID</param>
        /// <param name="olid">章节ID</param>
        /// <param name="playTime">观看进度，单位：毫秒</param>
        /// <param name="studyTime">学习时间，单位：秒</param>
        /// <param name="totalTime">视频总时长，单位：秒</param>
        /// <returns></returns>
        [Study]
        [HttpPost]
        public double StudyLog(long couid, long olid, int playTime, int studyTime, int totalTime)
        {
            //当前学员
            Song.Entities.Accounts student = LoginAccount.Status.User();
            if (student == null) return -1;
            //计算完成度的百分比
            double per = (double)studyTime * 1000 / (double)totalTime;
            per = Math.Floor(per * 10000) / 100;
            //new System.Threading.Tasks.Task(() =>
            //{
                Business.Do<IStudent>().LogForStudyUpdate(couid, olid, student, playTime, studyTime, totalTime);
            //}).Start();           
            return per;
        }
        /// <summary>
        /// 分页获取视频学习记录
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult StudyLogPager(int orgid, long couid, int size,int index)
        {
            int total = 0;
            LogForStudentStudy[] list= Business.Do<IStudent>().LogForStudyPager(orgid, couid,-1,-1,null,size,index,out total);
            Song.ViewData.ListResult result = new ListResult(list);
            result.Index = index;
            result.Size = size;
            result.Total = total;
            return result;
        }
        /// <summary>
        /// 学员学习某个课程的视频的完成信息，包括课程详细信息与完成信息
        /// </summary>
        /// <param name="stid">学员账号id</param>
        /// <param name="couid">课程id</param>
        /// <returns>课程信息，额外增加属性："lastTime"最后记录时间,"studyTime"累计学习时长,"complete"课程完成度
        /// </returns>
        [Cache(Expires = 10)]
        public DataTable LogForVideo(long couid, int stid)
        {
            return Business.Do<IStudent>().StudentStudyCourseLog(stid, couid);
        }
        /// <summary>
        /// 学员学习某个课程所有章节的视频的完成信息，包括章节详细信息与完成信息
        /// </summary>
        /// <param name="stid">学员账号id</param>
        /// <param name="couid">课程账号id</param>
        /// <returns>
        /// 课程章节列表，包括章节信息详情，且数据行包括额外字段
        /// "lastTime":最后记录时间,
        /// "studyTime":累计学习时长（单位秒）,
        /// "totalTime":视频总时间（单位毫秒）,
        /// "playTime":视频播放进度（单位毫秒）,
        /// "complete":完成度（百分比）
        /// </returns>
        public DataTable LogForOutlineVideo(int stid, long couid)
        {
            DataTable dt= Business.Do<IStudent>().StudentStudyOutlineLog(couid, stid);
            return dt;
        }
        /// <summary>
        /// 保存视频学习进度到课程购买记录
        /// </summary>
        /// <param name="acid"></param>
        /// <param name="couid"></param>
        /// <param name="rate">进度百分比</param>
        /// <returns></returns>
        [Student,Admin]
        [HttpPost]
        public double LogForVideoRecord(int acid, long couid, double rate)
        {
            Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsSingle(acid);
            if (acc == null) return rate;

            Student_Course sc = Business.Do<ICourse>().StudentCourse(acid, couid);
            //将学员组关联的课程，创建到Student_Course表（学员与课程的关联）
            if (sc == null) sc = Business.Do<IStudent>().SortCourseToStudent(acc, couid);
            if (sc == null) return rate;
            if (sc.Stc_StudyScore != rate)
            {
                sc.Stc_StudyScore = rate;
                Business.Do<ICourse>().StudentScoreSave(sc, rate, -1, -1);
                return rate;
            }
            return rate;
        }
        /// <summary>
        /// 学习记录修改为完成
        /// </summary>
        /// <param name="stid">学员Id</param>
        /// <param name="olid">章节id</param>
        /// <returns></returns>
        [Admin,SuperAdmin]
        public int LogUpdateOutlineVideo(int stid, long olid)
        {
            try
            {
                Business.Do<IStudent>().CheatOutlineLog(stid, olid);
                return 1;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 所有选修该课程的人数（包括过期的）
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        [HttpGet, HttpPut]
        [Cache(Expires = 1)]
        public int StudentSum(long couid)
        {
            return Business.Do<ICourse>().CourseStudentSum(couid, true);
        }
        /// <summary>
        /// 正在学习该课程的人数（过期的不算）
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        [HttpGet, HttpPut]
        [Cache(Expires = 1)]
        public int StudySum(long couid)
        {
            return Business.Do<ICourse>().CourseStudentSum(couid, false);
        }        
        /// <summary>
        /// 分页获取当前课程的学员（即学习该课程的学员），并计算出完成度
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="acc">学员账号或姓名</param>
        /// <param name="name">学员的姓名</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns>结果中的complete字段为学员在当前课程的学习完成度</returns>
        [Admin,Teacher]
        public ListResult Students(long couid, string acc, string name, int size, int index)
        {
            int total = 0;
            DataTable dt = Business.Do<ICourse>().StudentPager(couid, acc, name, size, index, out total);
            //处理返回结果
            string virPath = WeiSha.Core.Upload.Get["Accounts"].Virtual;
            string phyPath = WeiSha.Core.Upload.Get["Accounts"].Physics;
            foreach (DataRow dr in dt.Rows)
            {
                dr["Ac_Pw"] = "";
                dr["Ac_CheckUID"] = "";
                dr["Ac_Ans"] = "";
                dr["Ac_Photo"] = System.IO.File.Exists(phyPath + dr["Ac_Photo"]) ? virPath + dr["Ac_Photo"] : "";
            }
            ListResult result = new ListResult(dt);
            result.Index = index;
            result.Size = size;
            result.Total = total;
            return result;
        }
        /// <summary>
        /// 学员在某个课程下的考试成绩
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="acid"></param>
        /// <returns>{"score":成绩得分，没有成绩为-1,"state":nopass不及格，normal及格，fine优秀}</returns>
        public JObject StudentForCourseExam(long couid, int acid)
        {
            JObject jo = new JObject();
            if (couid <= 0 || acid <= 0)
            {
                jo.Add("score", -1);
                jo.Add("state", "none");
                return jo;
            }
            ExamResults result = Business.Do<IExamination>().StudentForCourseExam(couid, acid);
            if (result == null)
            {
                jo.Add("score", -1);
                jo.Add("state", "none");
            }
            else
            {
                jo.Add("score", result.Exr_ScoreFinal);
                Song.Entities.Examination exam = Business.Do<IExamination>().ExamSingle(result.Exam_ID);
                //考试不及极
                if (result.Exr_ScoreFinal < exam.Exam_PassScore)
                    jo.Add("state", "nopass");
                else if (result.Exr_ScoreFinal < exam.Exam_Total * 0.8)
                    jo.Add("state", "normal");
                else if (result.Exr_ScoreFinal >= exam.Exam_Total * 0.8)
                    jo.Add("state", "fine");
            }
            return jo;
        }         
        #endregion

        #region 课程购买(或叫选修)
        /// <summary>
        /// 购买课程
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="cpid">课程价格项的id</param>
        /// <returns></returns>
        [Student]
        public Song.Entities.Student_Course Buy(long couid, int cpid)
        { 
            //价格项
            Song.Entities.CoursePrice price = Business.Do<ICourse>().PriceSingle(cpid);
            if (price == null) throw new Exception("没有选中价格");
            //当前登录学员
            Song.Entities.Accounts st = this.User;
            //余额是否充足
            decimal money = st.Ac_Money;    //资金余额
            int coupon = st.Ac_Coupon;      //卡券余额
            int mprice = price.CP_Price;    //价格，所需现金
            int cprice = price.CP_Coupon;   //价格，可以用来抵扣的卡券
            bool tm = money >= mprice || money >= (mprice - (coupon > cprice ? cprice : coupon));
            if (!tm)
            {
                throw new Exception("资金或卡券不足");
            }
            //购买课程
            try
            {
                Song.Entities.Student_Course sc = Business.Do<ICourse>().Buy(st.Ac_ID, couid, price);
                //刷新登录状态的学员信息
                LoginAccount.Fresh(st.Ac_ID);
                return sc;
            }
            catch (Exception ex)
            {
                throw ex;
            }         
        }
        /// <summary>
        /// 禁用或启用学员的购买课程记录
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="couid">课程id</param>
        /// <param name="enable">启用或禁用</param>
        /// <returns></returns>
        [Admin,Teacher][HttpPost]
        public bool PurchaseEnable(int stid, long couid, bool enable)
        {
            Song.Entities.Student_Course sc = Business.Do<ICourse>().StudentCourse(stid, couid);
            if (sc == null) throw new Exception("购买记录不存在");
            sc.Stc_IsEnable = enable;
            Business.Do<ICourse>().StudentCourseUpdate(sc);
            return true;
        }
        /// <summary>
        /// 删除课程购买记录
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couid"></param>
        /// <returns></returns>
        [Admin]
        [HttpDelete]
        public bool PurchaseDelete(int stid, long couid)
        {
            Business.Do<ICourse>().DeleteCourseBuy(stid, couid);  
            return true;
        }
        /// <summary>
        /// 增加学员购买课程的时间
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="couid">课程id</param>
        /// <param name="number">要增加的时间数</param>
        /// <param name="unit">要增加的时间单位；
        /// 天:天,日,day,d ;
        /// 周:周,星期,week,w ;
        /// 月:月,month,m ; 
        /// 年:年,yeer,y</param>
        /// <returns></returns>
        [Admin][HttpPost]
        public bool PurchaseAddTime(int stid, long couid, int number, string unit)
        {
            number = number > 0 ? number : 0;
            if (number <= 0) throw new Exception("当前学员没有选修该课程");

            Student_Course sc = Business.Do<ICourse>().StudentCourse(stid, couid);
            if (sc == null) throw new Exception("当前学员没有选修该课程");

            unit = unit.ToLower();
            switch (unit)
            {
                case "天":
                case "日":
                case "day":
                case "d":
                    sc.Stc_EndTime = sc.Stc_EndTime.AddDays(number);
                    break;
                case "周":
                case "星期":
                case "week":
                case "w":
                    sc.Stc_EndTime = sc.Stc_EndTime.AddDays(number * 7);
                    break;
                case "月":
                case "month":
                case "m":
                    sc.Stc_EndTime = sc.Stc_EndTime.AddMonths(number);
                    break;
                case "年":
                case "yeer":
                case "y":
                    sc.Stc_EndTime = sc.Stc_EndTime.AddYears(number);
                    break;
            }
            try
            {
                sc = Business.Do<ICourse>().StudentCourseUpdate(sc);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 免费学习课程
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        [Student]
        public Song.Entities.Student_Course FreeBuy(long couid)
        {
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
            if (course == null) throw new Exception("课程不存在");
            course = _tran(course);
            //如果不可以免费
            if (!course.Cou_IsFree)
                throw new Exception("当课程不免费");
            try
            {
                //当前登录学员
                Song.Entities.Accounts st = this.User;
                Song.Entities.Student_Course sc = Business.Do<ICourse>().FreeStudy(st.Ac_ID, couid);
                return sc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 试学某个课程
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        [Student]
        public Song.Entities.Student_Course Try(long couid)
        {
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
            if (course == null) throw new Exception("课程不存在");
            course = _tran(course);
            //如果不可以试用
            if (!course.Cou_IsFree && !course.Cou_IsTry)
                throw new Exception("当课程不可以试用");
            try
            {
                //当前登录学员
                Song.Entities.Accounts st = this.User;
                Song.Entities.Student_Course sc = Business.Do<ICourse>().Tryout(st.Ac_ID, couid);
                return sc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 学生课程的记录项
        /// </summary>
        /// <param name="stid">学员账号Id</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        public Student_Course Purchaselog(int stid, long couid)
        {
            Student_Course sc = Business.Do<ICourse>().StudentCourse(stid, couid);
            if (sc == null) sc = Business.Do<IStudent>().SortCourseToStudent(stid, couid);
            if (sc != null)
            {
                sc.Stc_StudyScore = sc.Stc_StudyScore >= 100 ? 100 : sc.Stc_StudyScore;
            }
            return sc;
        }
        /// <summary>
        /// 当前登录学员，是否在学习这门课程
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        //[Student]
        public bool Studied(long couid)
        {
            Song.Entities.Accounts acc = LoginAccount.Status.User(this.Letter);
            if (acc == null) return false;
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
            if (course == null || !course.Cou_IsUse) return false;
            //是否免费，或是限时免费
            if (course.Cou_IsLimitFree)
            {
                DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
                if (!(course.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                    course.Cou_IsLimitFree = false;
            }
            if (course.Cou_IsFree || course.Cou_IsLimitFree || course.Cou_IsTry) return true;

            //是否存在于学员组所关联的课程
            bool isExistSort = Business.Do<IStudent>().SortExistCourse(couid, acc.Sts_ID);
            if (isExistSort) return true;

            //是否购买过该课程
            bool isBuy = Business.Do<ICourse>().IsBuy(couid, acc.Ac_ID);
            if (isBuy) return true;

            return false;
        }
        ///// <summary>
        ///// 当前登录学员，是否可以学习该课程（学员可能未购买，但课程可以试用）
        ///// </summary>
        ///// <param name="couid">课程id</param>
        ///// <returns></returns>
        ////[Student]
        //public bool StudyAllow(long couid)
        //{
        //    Song.Entities.Accounts acc = LoginAccount.Status.User(this.Letter);
        //    if (acc == null) return false;

        //    Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
        //    if (course == null || !course.Cou_IsUse) return false;
        //    //是否免费，或是限时免费
        //    if (course.Cou_IsLimitFree)
        //    {
        //        DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
        //        if (!(course.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
        //            course.Cou_IsLimitFree = false;
        //    }
        //    if (course.Cou_IsFree || course.Cou_IsLimitFree || course.Cou_IsTry) return true;

        //    //是否存在于学员组所关联的课程
        //    bool isExistSort = Business.Do<IStudent>().SortExistCourse(couid, acc.Sts_ID);
        //    if (isExistSort) return true;

        //    //是否购买过该课程
        //    bool isBuy = Business.Do<ICourse>().IsBuy(couid, acc.Ac_ID);
        //    if (isBuy) return true;            

        //    return false;
        //}
        /// <summary>
        /// 学员是否可以学习该课程（学员可能未购买，但课程可以试用）
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="acid">学员id</param>
        /// <returns></returns>
        public bool Allowlearn(long couid,int acid)
        {
            return Business.Do<ICourse>().AllowStudy(couid, acid); 
        }
        /// <summary>
        /// 学员是否拥有这个课程，包括购买或学员组关联，试用的不算
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="acid"></param>
        /// <returns></returns>
        public bool Owned(long couid, int acid)
        {
            //学员是否存在或通过审核
            Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsSingle(acid);
            if (acc == null || !acc.Ac_IsUse || !acc.Ac_IsPass) return false;

            //是否购买过该课程
            bool isBuy = Business.Do<ICourse>().IsBuy(couid, acid);
            if (isBuy) return true;
          
            //是否存在于学员组所关联的课程
            bool isExistSort = Business.Do<IStudent>().SortExistCourse(couid, acc.Sts_ID);
            if (isExistSort) return true;

            return false;
        }
        #endregion

        #region 导出课程的学习记录
        private static string outputPath = "StudentsLogToExcel";
        /// <summary>
        /// 学员学习记录生成excel
        /// </summary>
        /// <param name="couid">课程的id</param> 
        /// <returns></returns>
        [HttpPost]
        public JObject StudentsLogOutputExcel(long couid)
        {
            //导出文件的位置
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);

            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
            if (course == null) throw new Exception("当前课程不存在");
            return _StudentsLogOutputExcel(course, rootpath);
        }
        public static JObject _StudentsLogOutputExcel(Song.Entities.Course course, string path)
        {
            DateTime date = DateTime.Now;
            string filename = string.Format("{0}.{1}.({2}).xls", course.Cou_ID, WeiSha.Core.Upload.NameFilter(course.Cou_Name), date.ToString("yyyy-MM-dd hh-mm-ss"));
            string filePath = path + filename;
            try
            {
                filePath = Business.Do<ICourse>().StudentToExcel(filePath, course);
            }catch(Exception ex)
            {
                WeiSha.Core.Log.Error("StudentsLogOutputExcel", ex);
            }
            JObject jo = new JObject();
            jo.Add("file", filename);
            jo.Add("url", string.Format("{0}/{1}", WeiSha.Core.Upload.Get["Temp"].Virtual + outputPath, filename));
            jo.Add("date", date);
            return jo;
        }
        /// <summary>
        /// 生成当前机构下的所有课程的学习记录excel文档
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        public bool StudentsLogBatExcel(int orgid)
        {
            //判断之前的生成是否完成
            JObject jo = StudentsLogBatExcelProgress(orgid);
            if (jo != null)
            {
                int total, complete;
                int.TryParse(jo["total"].ToString(), out total);
                int.TryParse(jo["complete"].ToString(), out complete);
                if (total != 0 || complete != 0)
                {
                    if (total > complete) return false;
                }
            }
            //开始生成
            //导出文件的位置
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath + "\\";
            if (System.IO.Directory.Exists(rootpath))
            {
                foreach (string file in System.IO.Directory.GetFiles(rootpath))
                {
                    try
                    {
                        System.IO.File.Delete(file);
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                System.IO.Directory.CreateDirectory(rootpath);
            }
            //
            List<Song.Entities.Course> courses = Business.Do<ICourse>().CourseAll(orgid, -1, -1, null);
            Thread thread = new Thread(new ParameterizedThreadStart(_studentsLogBatExcel_Mwthod));
            thread.Start(courses);

            return true;
        }
        private static object _lock_StudentsLogBatExcelProgress = new object();
        /// <summary>
        /// 获取批量
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public JObject StudentsLogBatExcelProgress(int orgid)
        {
            lock (_lock_StudentsLogBatExcelProgress)
            {
                //导出文件的位置
                string file = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath + "\\" + "progress.json";
                if (System.IO.File.Exists(file))
                {
                    string text = System.IO.File.ReadAllText(file);
                    JObject jo = (JObject)JsonConvert.DeserializeObject(text);
                    return jo;
                }
                return null;
            }    
        }
        //用于批量生成学员学习记录的类（用于多线程）
        private static void _studentsLogBatExcel_Mwthod(object courses)
        {
            //导出文件的位置
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath + "\\";
            List<Song.Entities.Course> list = courses as List<Song.Entities.Course>;
            //记录执行状态
            JObject jo = new JObject();
            jo.Add("total", list.Count);
            int complete = 0;
            DateTime start = DateTime.Now;
            jo.Add("complete", complete);
            jo.Add("starttime", start.ToString());
            jo.Add("couid", string.Empty);
            jo.Add("course", string.Empty);
            jo.Add("timespan", 0);
            jo.Add("zipurl", WeiSha.Core.Upload.Get["Temp"].Virtual + outputPath + "/" + outputPath + ".zip");
            jo.Add("zipfile", outputPath + ".zip");
            foreach (Song.Entities.Course c in list)
            {
                jo["complete"] = ++complete;
                jo["couid"] = c.Cou_ID;
                jo["course"] = c.Cou_Name;
                jo["timespan"] = (DateTime.Now - start).ToString(@"hh\:mm\:ss");
                using (StreamWriter sw = new StreamWriter(rootpath + "progress.json", false, System.Text.Encoding.UTF8))
                {
                    sw.Write(jo.ToString());
                }
                _StudentsLogOutputExcel(c, rootpath);
            }
            WeiSha.Core.Compress.ZipDirectory(rootpath,"*.xls");
        }
        /// <summary>
        /// 删除Excel文件
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="filename">文件名，带后缀名，不带路径</param>
        /// <returns></returns>
        [HttpDelete]
        public bool StudentsLogOutputDelete(long couid,string filename)
        {
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);
            string filePath = rootpath + couid + "." + filename;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 已经生成的Excel文件
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <returns>file:文件名,url:下载地址,date:创建时间</returns>
        public JArray StudentsLogOutputFiles(long couid)
        {
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);
            JArray jarr = new JArray();
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(rootpath);
            foreach (System.IO.FileInfo f in dir.GetFiles("*.xls"))
            {
                if (f.Name.IndexOf(".") < 0) continue;
                string prefix = f.Name.Substring(0, f.Name.IndexOf("."));
                if (prefix.Length < 1) continue;
                if (prefix != couid.ToString()) continue;

                JObject jo = new JObject();
                string name = f.Name.Substring(f.Name.IndexOf(".") + 1);
                jo.Add("file", name);
                jo.Add("url", string.Format("{0}/{1}", WeiSha.Core.Upload.Get["Temp"].Virtual + outputPath, f.Name));
                jo.Add("date", f.CreationTime);
                jo.Add("size", f.Length);
                jarr.Add(jo);
            }
            return jarr;
        }
        #endregion

        #region 私有方法，处理对象的关联信息
        /// <summary>
        /// 处理课程信息，图片转为全路径，并生成clone对象
        /// </summary>
        /// <param name="course">课程对象的clone</param>
        /// <returns></returns>
        public static Song.Entities.Course _tran(Song.Entities.Course course)
        {
            if (course == null) return course;
            course.Cou_Logo = System.IO.File.Exists(PhyPath + course.Cou_Logo) ? VirPath + course.Cou_Logo : "";
            //如果缩略图不为空
            if (!string.IsNullOrWhiteSpace(course.Cou_LogoSmall))
            {
                course.Cou_LogoSmall = System.IO.File.Exists(PhyPath + course.Cou_LogoSmall) ? VirPath + course.Cou_LogoSmall : "";
            }else if (!string.IsNullOrWhiteSpace(course.Cou_Logo))
            {
                course.Cou_LogoSmall = WeiSha.Core.Images.Name.ToSmall(course.Cou_Logo);
            }
            //是否免费，或是限时免费
            if (course.Cou_IsLimitFree)
            {
                DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
                if (!(course.Cou_FreeStart <= DateTime.Now && freeEnd > DateTime.Now))
                    course.Cou_IsLimitFree = false;
            }
            return course;
        }
        #endregion
    }
    /// <summary>
    /// 用于批量生成学员学习记录的类（用于多线程）
    /// </summary>
    class studentsLogBatExcel
    {
        private List<Song.Entities.Course> courses;
        private string path = string.Empty;
        public studentsLogBatExcel(List<Song.Entities.Course> courses, string path)
        {
            this.courses = courses;
            this.path = path;
        }
        public void _start()
        {
            foreach (Song.Entities.Course course in courses)
            {
                Course._StudentsLogOutputExcel(course, this.path);
            }                  
        }
    }
}
