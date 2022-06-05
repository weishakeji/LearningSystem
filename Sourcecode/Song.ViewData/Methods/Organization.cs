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
    [HttpGet]
    public class Organization : ViewMethod, IViewAPI
    {
        //资源的虚拟路径和物理路径
        public static string VirPath = WeiSha.Core.Upload.Get["Org"].Virtual;
        public static string PhyPath = WeiSha.Core.Upload.Get["Org"].Physics;

        #region 获取机构信息
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
        public Ett.Organization[] AllUse()
        {
            Ett.Organization[] orgs = Business.Do<IOrganization>().OrganAll(true, -1, null);
            for (int i = 0; i < orgs.Length; i++)
            {
                orgs[i] = _trans(orgs[i]);
            }
            return orgs;
        }
        /// <summary>
        /// 获取所有机构
        /// </summary>
        /// <param name="lv">机构等级id</param>
        /// <returns></returns>
        public Ett.Organization[] All(int lv)
        {
            Ett.Organization[] orgs = Business.Do<IOrganization>().OrganAll(null, lv, null);
            for (int i = 0; i < orgs.Length; i++)
            {
                orgs[i] = _trans(orgs[i]);
            }
            return orgs;
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
        /// <returns></returns>
        [SuperAdmin]
        public Ett.OrganLevel[] LevelAll()
        {
            return Business.Do<IOrganization>().LevelAll(null);          
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
