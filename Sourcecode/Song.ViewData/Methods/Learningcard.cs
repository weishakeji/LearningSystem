using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
//using System.Web.Mvc;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;


namespace Song.ViewData.Methods
{
    /// <summary>
    /// 学习卡管理
    /// </summary>
    [HttpPut, HttpGet]
    public class Learningcard : ViewMethod, IViewAPI
    {
        #region 卡片信息
        /// <summary>
        /// 学员拥有的卡片数量
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <returns></returns>
        public JObject AccountOfCount(int acid)
        {
            if (acid <= 0)
            {
                Song.Entities.Accounts acc = this.User;
                if (acc != null) acid = acc.Ac_ID;
            }
            JObject jo = new JObject();
            //学员拥有几个学习卡
            int cardcount = Business.Do<ILearningCard>().AccountCardOfCount(acid);
            //使用了几张
            int usecount = cardcount - Business.Do<ILearningCard>().AccountCardOfCount(acid, 0);            

            jo.Add("count", cardcount);
            jo.Add("usecount", usecount);
          
            return jo;
        }
        /// <summary>
        /// 学员拥有的学习卡
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <returns></returns>
        public Song.Entities.LearningCard[] ForAccount(int acid)
        {
            if (acid <= 0)
            {
                Song.Entities.Accounts acc = this.User;
                if (acc != null) acid = acc.Ac_ID;
            }
            //学员的学习卡信息
            return Business.Do<ILearningCard>().AccountCards(acid);
        }
        /// <summary>
        /// 使用学习卡
        /// </summary>
        /// <param name="code">卡号-密码</param>
        /// <returns>关联的课程列表</returns>
        [Student]
        public JArray UseCode(string code)
        {
            JArray jarr = new JArray();
            //开始验证
            Song.Entities.LearningCard card = Business.Do<ILearningCard>().CardCheck(code);
            Business.Do<ILearningCard>().CardUse(card, this.User);
            //输出关联的课程
            List<Song.Entities.Course> courses = Business.Do<ILearningCard>().CoursesForCard(card.Lc_Code, card.Lc_Pw);
            for (int i = 0; i < courses.Count; i++)
            {
                Song.Entities.Course c = courses[i];
                jarr.Add(c.Cou_Name);
            }
            return jarr;
        }
        /// <summary>
        /// 使用学习卡，指定学习卡实体和学员账号实体
        /// </summary>
        /// <param name="card">学习卡的数据实体</param>
        /// <param name="account">账号的数据实体</param>
        /// <returns></returns>
        [HttpPost,HttpGet(Ignore =true) ]
        [Admin]
        public bool UseCode(LearningCard card, Accounts account)
        {
            Business.Do<ILearningCard>().CardUse(card, account);
            return true;
        }
        /// <summary>
        /// 收下学习卡（可以暂时收入学习卡，之后再使用）
        /// </summary>
        /// <param name="code">卡号-密码</param>
        /// <returns></returns>
        [Student]
        public bool AcceptCode(string code)
        {
            try
            {
                Song.Entities.LearningCard card = Business.Do<ILearningCard>().CardCheck(code);
                if (card != null)
                {
                    Song.Entities.Accounts acc = this.User;
                    if (card.Ac_ID == acc.Ac_ID) throw new Exception("您已经拥有过该学习卡");
                    Business.Do<ILearningCard>().CardReceive(card, this.User);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
        #endregion

        #region 卡片设置项
        /// <summary>
        /// 学习卡的最小长度
        /// </summary>
        /// <returns></returns>
        public int MinLength()
        {
            return Business.Do<ILearningCard>().MinLength();
        }
        /// <summary>
        /// 根据ID获取学习卡设置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Song.Entities.LearningCardSet SetForID(int id)
        {
            return Business.Do<ILearningCard>().SetSingle(id);
        }
        /// <summary>
        /// 判断学习卡名称是否重复
        /// </summary>
        /// <param name="name">学习卡名称</param>
        /// <param name="id">学习卡id</param>
        /// <returns></returns>
        public bool SetExist(string name,int id)
        {
            return Business.Do<ILearningCard>().SetIsExist(name, id);
        }
        /// <summary>
        /// 修改学习卡设置
        /// </summary>
        /// <param name="entity">习卡设置项（或叫主题）的实体对象</param>
        /// <param name="scope">更改范围，1为更改使用的，已经使用的不改；2为更改全部，默认是1</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public bool SetModify(Song.Entities.LearningCardSet entity,int scope)
        {
            int min_len = entity.Lcs_Count.ToString().Length + 1 + this.MinLength();
            if (entity.Lcs_CodeLength < min_len) throw new Exception("学习码长度不得小于" + min_len);

            Song.Entities.LearningCardSet old = Business.Do<ILearningCard>().SetSingle(entity.Lcs_ID);
            if (old == null) throw new Exception("Not found entity");
            old.Copy<Song.Entities.LearningCardSet>(entity);
            Business.Do<ILearningCard>().SetSave(old, scope);
            return true;
        }
        /// <summary>
        /// 删除学习卡的设置项
        /// </summary>
        /// <param name="id">id，可以是多个，用逗号分隔</param>
        /// <returns>返回删除的个数</returns>
        [HttpPost(Ignore = true), HttpDelete, Admin]
        public int SetDelete(string id)
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
                    Business.Do<ILearningCard>().SetDelete(idval);
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
        /// 添加学习卡的设置项
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public bool SetAdd(Song.Entities.LearningCardSet entity)
        {
            int min_len = entity.Lcs_Count.ToString().Length + 1 + this.MinLength();
            if (entity.Lcs_CodeLength < min_len) throw new Exception("学习码长度不得小于" + min_len);
            try
            {
                Business.Do<ILearningCard>().SetAdd(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 分页获取学习卡的设置项
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="search">检索字符，按课程名称</param>
        /// <param name="size">每页几条</param>
        /// <param name="index">第几页</param>
        /// <returns></returns>
        public ListResult SetPager(int orgid, string search, int size, int index)
        {
            int count = 0;         
            Song.Entities.LearningCardSet[] eas = null;
            eas = Business.Do<ILearningCard>().SetPager(orgid, null, search, size, index, out count);          
            ListResult result = new ListResult(eas);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 学习卡设置项所关联的课程
        /// </summary>
        /// <param name="id">学习卡设置项的id</param>
        /// <returns></returns>
        public List<Song.Entities.Course> SetCourses(int id)
        {
            Song.Entities.LearningCardSet set = Business.Do<ILearningCard>().SetSingle(id);
            if (set == null) throw new Exception("设置项不存在");
            List<Song.Entities.Course> courses = Business.Do<ILearningCard>().CoursesGet(set);
            if (courses == null) return null;
            for (int i = 0; i < courses.Count; i++)
            {
                courses[i] = _tran(courses[i]);
            }
            return courses;
        }
        /// <summary>
        /// 学习卡设置项的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JObject SetDataInfo(int id)
        {
            Song.Entities.LearningCardSet set = Business.Do<ILearningCard>().SetSingle(id);
            //学习卡总数
            int count = set.Lcs_Count;
            int used = Business.Do<ILearningCard>().CardOfCount(-1, id, null, true, null);
            int rollback = Business.Do<ILearningCard>().CardOfCount(-1, id, null, null, true);
            int disable = Business.Do<ILearningCard>().CardOfCount(-1, id, false, null, null);
            JObject jo = new JObject();
            jo.Add("total", count);
            jo.Add("used", used);
            jo.Add("rollbak", rollback);
            jo.Add("disable", disable);
            jo.Add("usable", count - used - disable);
            return jo;
        }
        #endregion

        #region 学习卡查询 回滚
        /// <summary>
        /// 学习卡的卡号，分页获取
        /// </summary>
        /// <param name="lsid">学习卡设置项的id</param>
        /// <param name="isused">true选择使用后的学习卡，为false选择所有</param>
        /// <param name="isback">true为回滚后的学习卡，false为所有</param>
        /// <param name="isdisable">true禁用的学习卡,false为所有</param>
        /// <param name="card">供检索的学习卡卡号</param>
        /// <param name="account">学员账号</param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ListResult CardPager(int lsid, bool isused, bool isback, bool isdisable, string card, string account, int index, int size)
        {
            //总记录数
            int count = 0;
            Song.Entities.LearningCard[] eas = null;
            bool? isUsed = isused ? (bool?)true : null;
            bool? isBack = isback ? (bool?)true : null;
            bool? isEnable = !isdisable ? null : (bool?)false;
            eas = Business.Do<ILearningCard>().CardPager(-1, lsid, card, account, isEnable, isUsed, isBack, size, index, out count);
            Song.ViewData.ListResult result = new ListResult(eas);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 某个设置项下的所有学习卡
        /// </summary>
        /// <param name="lsid">学习卡设置项(或叫主题）的id</param>
        /// <param name="enable">是否启用,null取所有</param>
        /// <param name="used">是否是使用过的,null取所有</param>
        /// <returns></returns>
        public List<Song.Entities.LearningCard> Cards(int lsid, bool? enable, bool? used)
        {
            //当前学习卡的编码
            List<Song.Entities.LearningCard> cards = Business.Do<ILearningCard>().CardCount(-1, lsid, enable, used, -1);
            return cards;
        }
        /// <summary>
        /// 学习卡回滚
        /// </summary>
        /// <param name="code">卡号</param>
        /// <param name="pw">密码</param>
        /// <param name="clear">是否清理学习记录</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public LearningCard CardRollback(string code,string pw,bool clear)
        {
            Song.Entities.LearningCard card = Business.Do<ILearningCard>().CardSingle(code, pw);
            //return card;
            if (card == null) throw new Exception("未查询到学习卡，卡号或密码错误");
            try
            {
                Business.Do<ILearningCard>().CardRollback(card, clear);
                return card;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 更改学习卡启用状态
        /// </summary>
        /// <param name="code"></param>
        /// <param name="pw"></param>
        /// <param name="isenable"></param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public bool CardChangeEnable(string code, string pw, bool isenable)
        {
            Song.Entities.LearningCard card = Business.Do<ILearningCard>().CardSingle(code, pw);
            if(card==null) throw new Exception("未查询到学习卡，卡号或密码错误");
            try
            {
                card.Lc_IsEnable = isenable;
                Business.Do<ILearningCard>().CardSave(card);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 学员的学习卡
        /// <summary>
        /// 学员的学习卡
        /// </summary>
        /// <param name="acid">学员账号id</param>
        /// <param name="isused">是否使用的学习卡，true取使用的，false取所有</param>
        /// <param name="isback">是否回滚的</param>
        /// <param name="isdisable">是否禁用的</param>
        /// <param name="code">学习卡卡号</param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ListResult AccountCards(int acid, bool isused, bool isback, bool isdisable, string code, int index, int size)
        {
            //总记录数
            int count = 0;
            Song.Entities.LearningCard[] entities = null;
            bool? isUsed = isused ? (bool?)true : null;
            bool? isBack = isback ? (bool?)true : null;
            bool? isEnable = !isdisable ? null : (bool?)false;
            entities = Business.Do<ILearningCard>().AccountCards(acid, isUsed, isBack, isEnable, code, index, size, out count);
            Song.ViewData.ListResult result = new ListResult(entities);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        #endregion

        #region 学习成果
        /// <summary>
        /// 学习卡关联课程的的学习成果
        /// </summary>
        /// </summary>
        /// <param name="lcsid">学习卡设置项的id</param>
        /// <param name="name">按学员姓名检索</param>
        /// <param name="acc">学员账号</param>
        /// <param name="phone">按学员手机号检索</param>
        /// <param name="gender">学员性别</param>
        /// <param name="couname">按课程名称查询</param>
        /// <param name="size">每页多少条</param>
        /// <param name="index">第几页</param>      
        /// <returns>Student_Course、Course、Accounts三个表的数据合集</returns>
        public ListResult Outcomes(long lcsid, string name, string acc, string phone, int gender, string couname, int size, int index)
        {
            int sum;
            DataTable dt = Business.Do<IStudent>().Outcomes4LearningCard(lcsid, name, acc, phone, gender, couname, size, index, out sum);
            Song.ViewData.ListResult result = new ListResult(dt);
            result.Index = index;
            result.Size = size;
            result.Total = sum;
            return result;
        }
        /// <summary>
        /// 批量计算学习关联课程的综合成绩，只有学员参与学习了才会有成绩
        /// </summary>
        /// <param name="lcsid">学习卡设置的id</param>
        /// <returns></returns>
        public bool ResultScoreCalc(int lcsid)
        {
            return Business.Do<ICourse>().ResultScoreCalc4LearningCard(lcsid);
        }
        private static string outputPath_ResultScore = "Learningcard_ResultScoreToExcel";
        /// <summary>
        /// 学习卡关联课程的学习成果导出Excel文件
        /// </summary>
        /// <param name="lcsid">学习卡设项卡的id</param>
        /// <returns></returns>
        [HttpPost]
        [Admin, Teacher]
        public JObject ResultScoreOutputExcel(int lcsid)
        {
            //导出文件的位置
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath_ResultScore + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);
            LearningCardSet set = Business.Do<ILearningCard>().SetSingle(lcsid);
            if (set == null) throw new Exception("学习卡不存在");

            DateTime date = DateTime.Now;
            string filename = string.Format("{0}.{1}.({2}).xls", lcsid, set.Lcs_Theme, date.ToString("yyyy-MM-dd hh-mm-ss"));
            if (File.Exists(rootpath + filename))
            {
                throw new Exception("当前文档已经存在，请删除或稍后再操作");
            }
            Business.Do<ILearningCard>().ResultScoreToExcel(rootpath + filename, lcsid);
            JObject jo = new JObject();
            jo.Add("file", filename);
            jo.Add("url", string.Format("{0}/{1}", WeiSha.Core.Upload.Get["Temp"].Virtual + outputPath_ResultScore, filename));
            jo.Add("date", date);
            return jo;
        }
        /// <summary>
        /// 删除学习卡关联课程的学习成果导出的Excel文件
        /// </summary>
        /// <param name="lcsid">学习卡设项卡的id</param>
        /// <param name="filename">文件名，带后缀名，不带路径</param>
        /// <returns></returns>
        [HttpDelete]
        public bool ResultScoreFileDelete(int lcsid, string filename)
        {
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath_ResultScore + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);
            string filePath = rootpath + lcsid + "." + filename;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 学习卡关联课程的学习成果的导出，已经生成的Excel文件
        /// </summary>
        /// <param name="lcsid">学习卡设项卡的id</param>
        /// <returns>file:文件名,url:下载地址,date:创建时间</returns>
        public JArray ResultScoreFiles(int lcsid)
        {
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath_ResultScore + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);
            JArray jarr = new JArray();
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(rootpath);
            FileInfo[] files = dir.GetFiles("*.xls").OrderByDescending(f => f.CreationTime).ToArray();
            foreach (System.IO.FileInfo f in files)
            {
                if (f.Name.IndexOf(".") < 0) continue;
                string prefix = f.Name.Substring(0, f.Name.IndexOf("."));
                if (prefix.Length < 1) continue;
                if (prefix != lcsid.ToString()) continue;

                JObject jo = new JObject();
                string name = f.Name.Substring(f.Name.IndexOf(".") + 1);
                jo.Add("file", name);
                jo.Add("url", string.Format("{0}/{1}", WeiSha.Core.Upload.Get["Temp"].Virtual + outputPath_ResultScore, f.Name));
                jo.Add("date", f.CreationTime);
                jo.Add("size", f.Length);
                jarr.Add(jo);
            }
            return jarr;
        }
        #endregion

        #region 导出学习卡
        private static string _output_item = "LearningCard";
        /// <summary>
        /// 生成导出的Excel文件
        /// </summary>
        /// <param name="id">学习卡设置项id</param>
        /// <returns>name:学习卡主题,file:文件名,url:下载地址,date:创建时间</returns>
        [HttpPost][Admin]
        public JObject ExcelOutput(int id)
        {
            Song.Entities.LearningCardSet set = Business.Do<ILearningCard>().SetSingle(id);
            DateTime date = DateTime.Now;
            string filename = string.Format("{0}_{1}.xls", set.Lcs_ID.ToString(), date.ToString("yyyy-MM-dd hh-mm-ss"));
            string filePath = WeiSha.Core.Upload.Get[_output_item].Physics + filename;
            filePath = Business.Do<ILearningCard>().Card4Excel(filePath, set.Org_ID, id);
            JObject jo = new JObject();
            jo.Add("name", set.Lcs_Theme);
            jo.Add("file", filename);
            jo.Add("url", WeiSha.Core.Upload.Get[_output_item].Virtual + filename);
            jo.Add("date", date);
            return jo;
        }
        /// <summary>
        /// 删除Excel文件
        /// </summary>
        /// <param name="filename">文件名，带后缀名，不带路径</param>
        /// <returns></returns>
        [HttpDelete]
        [Admin]
        public bool ExcelDelete(string filename)
        {
            string filePath = WeiSha.Core.Upload.Get[_output_item].Physics + filename;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 删除某个学习卡的所有导出文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Admin]
        public int ExcelDeleteAll(int id)
        {        
            string path = WeiSha.Core.Upload.Get[_output_item].Physics;
            if (!System.IO.Directory.Exists(path)) return 0;

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
            int i = 0;
            foreach (System.IO.FileInfo f in dir.GetFiles("*.xls"))
            {
                if (f.Name.IndexOf("_") < 0) continue;
                string pre = f.Name.Substring(0, f.Name.IndexOf("_"));
                if (id.ToString().Equals(pre, StringComparison.OrdinalIgnoreCase))
                {
                    f.Delete();
                    i++;
                }
            }          
            return i;
        }
        /// <summary>
        /// 已经生成的Excel文件
        /// </summary>
        /// <param name="id">学习卡设置项id</param>
        /// <returns>name:学习卡主题,file:文件名,url:下载地址,date:创建时间</returns>
        public JArray ExcelFiles(int id)
        {
            Song.Entities.LearningCardSet set = Business.Do<ILearningCard>().SetSingle(id);
            List<System.IO.FileInfo> list = new List<System.IO.FileInfo>();
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(WeiSha.Core.Upload.Get[_output_item].Physics);
            FileInfo[] files = dir.GetFiles("*.xls").OrderByDescending(f => f.CreationTime).ToArray();
            foreach (System.IO.FileInfo f in files)
            {
                if (f.Name.IndexOf("_") < 0) continue;
                string pre = f.Name.Substring(0, f.Name.IndexOf("_"));
                if (id.ToString().Equals(pre, StringComparison.OrdinalIgnoreCase))              
                    list.Add(f);
            }
            JArray jarr = new JArray();
            IEnumerable<System.IO.FileInfo> query = from items in list orderby items.CreationTime descending select items;
            string url = WeiSha.Core.Upload.Get[_output_item].Virtual;
            foreach (var f in query)
            {
                System.IO.FileInfo file = (System.IO.FileInfo)f;
                JObject jo = new JObject();
                jo.Add("name", set.Lcs_Theme);
                jo.Add("file", file.Name);
                jo.Add("url", url + file.Name);
                jo.Add("date", file.CreationTime);
                jo.Add("size", file.Length);
                jarr.Add(jo);
            }
            return jarr;
        }
        #endregion

        #region 私有方法，处理对象的关联信息
        /// <summary>
        /// 处理课程信息，图片转为全路径，并生成clone对象
        /// </summary>
        /// <param name="cour">课程对象的clone</param>
        /// <returns></returns>
        private Song.Entities.Course _tran(Song.Entities.Course cour)
        {
            if (cour == null) return cour;
            string pathKey = "Course";
            string virPath = WeiSha.Core.Upload.Get[pathKey].Virtual;
            string phyPath = WeiSha.Core.Upload.Get[pathKey].Physics;

            Song.Entities.Course curr = cour.Clone<Song.Entities.Course>();
            if (System.IO.File.Exists(phyPath + curr.Cou_Logo))
                curr.Cou_Logo = virPath + curr.Cou_Logo;
            else
                curr.Cou_Logo = "";

            if (System.IO.File.Exists(phyPath + curr.Cou_LogoSmall))
                curr.Cou_LogoSmall = virPath + curr.Cou_LogoSmall;
            else
                curr.Cou_LogoSmall = "";
            return curr;
        }
        #endregion
    }
}
