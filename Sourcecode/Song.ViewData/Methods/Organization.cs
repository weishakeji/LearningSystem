using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiSha.Core;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using System.IO;
using Ett = Song.Entities;
using Newtonsoft.Json.Linq;
using System.Web;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 机构管理
    /// </summary>
    [HttpPut, HttpGet]
    public class Organization : ViewMethod, IViewAPI
    {
        //资源的虚拟路径和物理路径
        public static string VirPath = WeiSha.Core.Upload.Get["Org"].Virtual;
        public static string PhyPath = WeiSha.Core.Upload.Get["Org"].Physics;

        #region 获取机构信息
        /// <summary>
        /// 机构的统计数据，例如学员数，等
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public JObject Statistics(int orgid)
        {
            if (orgid <= 0)
            {
                Ett.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            JObject jo = new JObject();
            //学员数
            JObject jacc = new JObject();
            jacc.Add("total", Business.Do<IAccounts>().AccountsOfCount(orgid, null, -1));     //学员总数
            jacc.Add("usecount", Business.Do<IAccounts>().AccountsOfCount(orgid, true, -1));     //启用的学员数
            jacc.Add("man", Business.Do<IAccounts>().AccountsOfCount(orgid, true, 1));            //男性人数    
            jacc.Add("woman", Business.Do<IAccounts>().AccountsOfCount(orgid, true, 2));            //女性人数
            jacc.Add("online", Song.ViewData.LoginAccount.list.Count);                       //在线人数           
            jacc.Add("recharge", Business.Do<IAccounts>().MoneyForAccount(orgid, 2, 3));         //付费学员数，即充过值的人数            
            jacc.Add("pay", Business.Do<IAccounts>().MoneyForAccount(orgid, 1, 4));         //消费学员数，即购买过课程的人数
            jacc.Add("course", Business.Do<IStudent>().ForCourseCount(orgid, false));             //正在学习的人数,即选修过课程的人数
            jacc.Add("test", Business.Do<IStudent>().ForTestCount(orgid));              //参与过模拟测试的人数
            jacc.Add("exercise", Business.Do<IStudent>().ForExerciseCount(orgid));      //参与过试题练习的人数
            jacc.Add("study", Business.Do<IStudent>().ForStudyCount(orgid));            //参与过视频学习的人数
            jo.Add("account", jacc);

            //课程
            JObject jcoud = new JObject();
            jcoud.Add("total", Business.Do<ICourse>().CourseOfCount(orgid, -1, -1, null, null));   //课程总数
            jcoud.Add("usecount", Business.Do<ITeacher>().TeacherOfCount(orgid, true));   //可用课程数
            jcoud.Add("free", Business.Do<ICourse>().CourseOfCount(orgid, -1, -1, null, true));   //免费的课程数  
            jcoud.Add("buycount", Business.Do<IStudent>().ForCourseCount(orgid, true));     //课程的选修人次
            jo.Add("course", jcoud);

            //教师
            JObject jteach = new JObject();
            jteach.Add("total", Business.Do<ITeacher>().TeacherOfCount(orgid, null));   //教师总数
            jteach.Add("usecount", Business.Do<ITeacher>().TeacherOfCount(orgid, true));   //启用的教师数
            jo.Add("teacher", jteach);

            //资源
            JObject jres = new JObject();
            jres.Add("video", Business.Do<IAccessory>().OfCount(orgid, string.Empty, "CourseVideo"));   //视频数
            jres.Add("document", Business.Do<IAccessory>().OfCount(orgid, string.Empty, "Course"));   //资料数
            jres.Add("testpaper", Business.Do<ITestPaper>().PaperOfCount(orgid, -1, -1, -1, null));       //试卷数         
            jres.Add("question", Business.Do<IQuestions>().QuesOfCount(orgid, -1, -1, -1, -1, -1, null));      //试题数
            jres.Add("subject", Business.Do<ISubject>().SubjectOfCount(orgid, -1, null, true));         //专业数
            jo.Add("resource", jres);
            return jo;
        }
        /// <summary>
        /// 通过机构id获取机构信息
        /// </summary>
        /// <param name="id">机构id</param>
        /// <returns>机构实体</returns>
        public Ett.Organization ForID(int id)
        {
            return _trans(Business.Do<IOrganization>().OrganSingle(id));
        }
        /// <summary>
        /// 获取所有可用的机构
        /// </summary>
        /// <returns></returns>
        public List<Ett.Organization> AllUse()
        {
            List<Ett.Organization> orgs = Business.Do<IOrganization>().OrganAll(true, -1, null);
            for (int i = 0; i < orgs.Count; i++)
            {
                orgs[i] = _trans(orgs[i]);
            }
            return orgs;
        }
        /// <summary>
        /// 获取所有机构
        /// </summary>
        /// <param name="use">是否启用</param>
        /// <param name="lv">机构等级id</param>
        /// <param name="name">按机构名称检索</param>
        /// <returns></returns>
        public List<Ett.Organization> All(bool? use, int lv, string name)
        {
            List<Ett.Organization> orgs = Business.Do<IOrganization>().OrganAll(use, lv, name);
            for (int i = 0; i < orgs.Count; i++)
            {
                orgs[i] = _trans(orgs[i]);
            }
            return orgs;
        }
        /// <summary>
        /// 当前机构是否重名
        /// </summary>
        /// <param name="name">机构的名称</param>
        /// <param name="id">机构的id</param>   
        /// <returns></returns>
        public bool ExistName(string name, int id)
        {
            return Business.Do<IOrganization>().ExistName(name, id);
        }
        /// <summary>
        /// 机构平台名称是否重复
        /// </summary>
        /// <param name="name">机构的平台名称</param>
        /// <param name="id">机构的id</param>
        /// <returns></returns>
        public bool ExistPlatform(string name, int id)
        {
            return Business.Do<IOrganization>().ExistPlatform(name, id);
        }
        /// <summary>
        /// 机构的二级域否重复
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id">机构的id</param>
        /// <returns></returns>
        public bool ExistDomain(string name, int id)
        {
            return Business.Do<IOrganization>().ExistDomain(name, id);
        }
        /// <summary>
        /// 二级域名是否允许使用，判断是否存在于保留域名中
        /// </summary>
        /// <param name="name">二级域名</param>
        /// <returns>允许使用该域，即在限制域名中不存在，返回true,</returns>
        public bool DomainAllow(string name)
        {
            return !Business.Do<ILimitDomain>().DomainIsExist(name, -1);
        }
        /// <summary>
        /// 分页获取机构信息
        /// </summary>
        /// <param name="lvid"></param>
        /// <param name="search"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ListResult Pager(int lvid, string search, int index, int size)
        {
            int sum = 0;
            Ett.Organization[] orgs = Business.Do<IOrganization>().OrganPager(null, lvid, search, size, index, out sum);
            for (int i = 0; i < orgs.Length; i++)
            {
                orgs[i] = _trans(orgs[i]);
            }
            Song.ViewData.ListResult result = new ListResult(orgs);
            result.Index = index;
            result.Size = size;
            result.Total = sum;
            return result;
        }
        /// <summary>
        /// 当前机构
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Ett.Organization Current()
        {
            return _trans(Business.Do<IOrganization>().OrganCurrent());
        }
        #endregion

        #region 编辑机构信息
        /// <summary>
        /// 新增机构
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [SuperAdmin]
        [HttpPost]
        public bool Add(Ett.Organization entity)
        {
            Business.Do<IOrganization>().OrganAdd(entity);
            return true;
        }
        /// <summary>
        /// 根据id删除账号，可以有多个id，用逗号分隔
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost(Ignore = true), HttpDelete, SuperAdmin]
        public int Delete(string id)
        {
            Ett.Organization org = Business.Do<IOrganization>().OrganCurrent();
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
                    if (org != null && org.Org_ID == idval) throw new Exception("不可删除自身所有机构");
                    Business.Do<IOrganization>().OrganDelete(idval);
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
        /// 修改机构信息
        /// </summary>
        /// <param name="entity">机构对象</param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet(Ignore = true)]
        [Admin]
        [HtmlClear(Not = "entity")]
        [Upload(Extension = "jpg,png,gif", MaxSize = 1024, CannotEmpty = false)]
        public bool Modify(Ett.Organization entity)
        {
            Ett.Organization old = Business.Do<IOrganization>().OrganSingle(entity.Org_ID);
            if (old == null) throw new Exception("Not found entity for Organization");
            //机构的logo图片上传
            string filename = string.Empty;
            foreach (string key in this.Files)
            {
                HttpPostedFileBase file = this.Files[key];
                filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                file.SaveAs(PhyPath + filename);
                break;
            }
            //如果有上传的图片，且之前也有图片,则删除原图片
            if ((!string.IsNullOrWhiteSpace(filename) || string.IsNullOrWhiteSpace(entity.Org_Logo)) && !string.IsNullOrWhiteSpace(old.Org_Logo))
                WeiSha.Core.Upload.Get["Org"].DeleteFile(old.Org_Logo);

            old.Copy<Ett.Organization>(entity, "Org_Config");
            if (!string.IsNullOrWhiteSpace(filename)) old.Org_Logo = filename;

          
            Business.Do<IOrganization>().OrganSave(old);
            return true;
        }
        /// <summary>
        /// 设置默认
        /// </summary>
        /// <param name="id">机构的id</param>
        /// <returns></returns>
        [SuperAdmin]
        [HttpPost]
        public bool SetDefault(int id)
        {
            Business.Do<IOrganization>().OrganSetDefault(id);
            return true;
        }
        #endregion

        #region 公章管理
        /// <summary>
        /// 机构公章信息
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns>path:公章图片路径;positon:位置</returns>
        public Dictionary<string, string> Stamp(int orgid)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            Ett.Organization org = Business.Do<IOrganization>().OrganSingle(orgid);
            if (org == null) throw new Exception("Not found entity for Organization");

            //公章
            WeiSha.Core.CustomConfig config = CustomConfig.Load(org.Org_Config);
            //公章显示位置
            string positon = config["StampPosition"].Value.String;
            if (string.IsNullOrEmpty(positon)) positon = "right-bottom";
            dic.Add("positon", positon);
            //公章图像信息
            string stamp = config["Stamp"].Value.String;
            string filepath = PhyPath + stamp;
            dic.Add("path", !File.Exists(filepath) ? "" : VirPath + stamp);
            return dic;
        }
        /// <summary>
        /// 修改公章信息
        /// </summary>
        /// <param name="stamp"></param>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        [HttpPost, Admin]
        [Upload(Extension = "png,gif", MaxSize = 1024, CannotEmpty = false)]
        public bool StampUpdate(JObject stamp,int orgid)
        {
            Ett.Organization org = Business.Do<IOrganization>().OrganSingle(orgid);
            if (org == null) throw new Exception("Not found entity for Organization");

            WeiSha.Core.CustomConfig config = CustomConfig.Load(org.Org_Config);
            //机构的logo图片上传
            string filename = string.Empty;
            foreach (string key in this.Files)
            {
                HttpPostedFileBase file = this.Files[key];
                filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                file.SaveAs(PhyPath + filename);
                break;
            }
            //如果有上传的图片，且之前也有图片,则删除原图片
            //之前的公章图像信息
            string history_img = config["Stamp"].Value.String;
            string upload_img = stamp["path"] ==null ? "" : stamp["path"].ToString();
            if ((!string.IsNullOrWhiteSpace(filename) || string.IsNullOrWhiteSpace(upload_img)) && !string.IsNullOrWhiteSpace(history_img))
                WeiSha.Core.Upload.Get["Org"].DeleteFile(config["Stamp"].Text);

            if (!string.IsNullOrWhiteSpace(filename)) config["Stamp"].Text = filename;
            config["StampPosition"].Text = stamp["positon"] == null ? "" : stamp["positon"].ToString();
            try
            {  
                org.Org_Config = config.XmlString;
                Business.Do<IOrganization>().OrganSave(org);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 配置管理
        /// <summary>
        /// 当前机构的配置项
        /// </summary>
        /// <param name="orgid">机构id，如果小于等于0，则为当前机构</param>
        /// <returns></returns>
        public JObject Config(int orgid)
        {
            Song.Entities.Organization org;
            if (orgid <= 0) org = Business.Do<IOrganization>().OrganCurrent();
            else
                org = Business.Do<IOrganization>().OrganSingle(orgid);
            WeiSha.Core.CustomConfig config = CustomConfig.Load(org.Org_Config);
            JObject jo = new JObject();
            foreach (WeiSha.Core.CustomConfigItem item in config)
            {
                if (item.Text == "True" || item.Text == "False")
                    jo.Add(item.Key, Convert.ToBoolean(item.Text.ToLower()));
                else
                    jo.Add(item.Key, item.Text);
            }
            return jo;
        }
        /// <summary>
        /// 更改配置项
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        [HttpPost, Admin]
        public bool ConfigUpdate(int orgid, JObject config)
        {
            Song.Entities.Organization org = null;
            if (orgid > 0) org = Business.Do<IOrganization>().OrganSingle(orgid);
            if(org==null) throw new Exception("Not found entity for Organization");
            try
            {
                WeiSha.Core.CustomConfig history = CustomConfig.Load(org.Org_Config);
                IEnumerable<JProperty> properties = config.Properties();
                foreach (JProperty item in properties)
                {
                    string key = item.Name;
                    string val = item.Value.ToString();
                    history[key].Text = val;
                }
                org.Org_Config = history.XmlString;
                Business.Do<IOrganization>().OrganSave(org);
                return true;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 处理机构对外展示的信息
        /// </summary>
        /// <param name="org"></param>
        /// <returns></returns>
        private Ett.Organization _trans(Ett.Organization org)
        {
            if (org == null) return org;
            org = (Ett.Organization)org.Clone();
            org.Org_Logo = System.IO.File.Exists(PhyPath + org.Org_Logo) ? VirPath + org.Org_Logo : "";
            return org;
        }
        #endregion

        #region 机构等级
        /// <summary>
        /// 通过机构id获取机构等级
        /// </summary>
        /// <param name="id">机构id</param>
        /// <returns>机构实体</returns>
        public Ett.OrganLevel LevelForID(int id)
        {
            return Business.Do<IOrganization>().LevelSingle(id);
        }
        /// <summary>
        /// 获取所有机构等级
        /// </summary>
        /// <param name="search">按等级名称检索</param>
        /// <param name="use">是否启用的</param>
        /// <returns></returns>
        public Ett.OrganLevel[] LevelAll(string search, bool? use)
        {
            return Business.Do<IOrganization>().LevelAll(search, use);          
        }
        /// <summary>
        /// 批量删除机构等级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public int LevelDelete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            string[] arr = id.Split(',');
            int[] idarr = new int[arr.Length];
            for(int j = 0; j < arr.Length; j++)
            {
                int idval = 0;
                int.TryParse(arr[j], out idval);
                idarr[j] = idval;
                if (idval > 0)
                {
                    int count = Business.Do<IOrganization>().LevelOrganCount(idval);
                    if (count > 0)
                    {
                        throw new Exception("机构等级下有机构，不可以删除");
                    }
                }
            }
            for (int j = 0; j < idarr.Length; j++)
            {
                if (idarr[j] > 0)
                {
                    try
                    {
                        Business.Do<IOrganization>().LevelDelete(idarr[j]);
                        i++;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return i;
        }
        /// <summary>
        /// 修改机构等级的信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [SuperAdmin]
        [HttpPost]
        public bool LevelModify(OrganLevel entity)
        {
            Ett.OrganLevel old = Business.Do<IOrganization>().LevelSingle(entity.Olv_ID);
            if (old == null) throw new Exception("Not found entity for OrganLevel");
            //账号，密码，登录状态值，不更改
            old.Copy<Ett.OrganLevel>(entity);
            Business.Do<IOrganization>().LevelSave(old);
            return true;
        }
        /// <summary>
        /// 新增机构等级
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [SuperAdmin]
        [HttpPost]
        public bool LevelAdd(OrganLevel entity)
        {
            Business.Do<IOrganization>().LevelAdd(entity);
            return true;
        }
        /// <summary>
        /// 机构等级名称是否存在
        /// </summary>
        /// <param name="name">机构等级名称</param>
        /// <param name="id">机构等级id，如果对象已经存在，则不断判断自身</param>
        /// <returns></returns>
        [HttpGet]
        public bool LevelNameExist(string name, int id)
        {
            return Business.Do<IOrganization>().LevelNameExist(name, id);
        }
        /// <summary>
        /// 机构等级的tag标识是否重名
        /// </summary>
        /// <param name="tag">机构等级的tag标识</param>
        /// <param name="id">机构等级的id</param>   
        /// <returns></returns>
        [HttpGet]
        public bool LevelTagExist(string tag, int id)
        {
            return Business.Do<IOrganization>().LevelTagExist(tag, id);
        }
        /// <summary>
        /// 设置默认等级
        /// </summary>
        /// <param name="id">机构等级的id</param>
        /// <returns></returns>
        [SuperAdmin]
        [HttpPost]
        public bool LevelSetDefault(int id)
        {
            Business.Do<IOrganization>().LevelSetDefault(id);
            return true;
        }
        /// <summary>
        /// 机构等级下的机构数
        /// </summary>
        /// <param name="id">机构等级的id</param>
        /// <returns></returns>
        [HttpGet]
        public int LevelOrgcount(int id)
        {
            return Business.Do<IOrganization>().LevelOrganCount(id);
        }
        /// <summary>
        /// 更改排序
        /// </summary>
        /// <param name="items">数组</param>
        /// <returns></returns>
        [HttpPost]
        [SuperAdmin]
        public bool LevelModifyTaxis(Ett.OrganLevel[] items)
        {
            try
            {
                Business.Do<IOrganization>().LevelUpdateTaxis(items);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
