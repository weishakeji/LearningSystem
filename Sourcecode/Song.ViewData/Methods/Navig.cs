using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiSha.Core;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Web;
using System.IO;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 站点导航
    /// </summary>
    [HttpPut, HttpGet]
    public class Navig : ViewMethod, IViewAPI
    {
        //资源的虚拟路径和物理路径
        public static string VirPath = WeiSha.Core.Upload.Get["Org"].Virtual;
        public static string PhyPath = WeiSha.Core.Upload.Get["Org"].Physics;
        #region 导航菜单       
        /// <summary>
        /// 手机端导航
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="type">类型，主菜单main，底部菜单foot</param>
        /// <returns></returns>
        [Cache(AdminDisable = true)]
        public JArray Mobi(int orgid, string type)
        {
            if (orgid <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            List<Song.Entities.Navigation> navi = Business.Do<IStyle>().NaviAll(true, "mobi", type, orgid);
            foreach (Song.Entities.Navigation n in navi)
            {
                n.Nav_Logo = System.IO.File.Exists(PhyPath + n.Nav_Logo) ? VirPath + n.Nav_Logo : "";
            }
            return navi.Count > 0 ? _MenuNode(null, navi) : null;
        }
        /// <summary>
        /// 电脑端web页导航，树形数据
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="type">类型，主菜单main，底部菜单foot</param>
        /// <returns></returns>
        [Cache(AdminDisable = true)]
        public JArray Web(int orgid, string type)
        {
            if (orgid <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }

            List<Song.Entities.Navigation> navi = Business.Do<IStyle>().NaviAll(true, "web", type, orgid);
            foreach (Song.Entities.Navigation n in navi)
            {
                n.Nav_Logo = System.IO.File.Exists(PhyPath + n.Nav_Logo) ? VirPath + n.Nav_Logo : "";
            }
            return navi.Count > 0 ? _MenuNode(null, navi) : null;
        }
        /// <summary>
        /// 手机端导航,所有导航，
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="type">类型，主菜单main，底部菜单foot</param>
        /// <returns></returns>
        [Cache(AdminDisable = true)]
        public JArray MobiAll(int orgid, string type)
        {
            if (orgid <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            List<Song.Entities.Navigation> navi = Business.Do<IStyle>().NaviAll(null, "mobi", type, orgid);
            foreach (Song.Entities.Navigation n in navi)
            {
                n.Nav_Logo = System.IO.File.Exists(PhyPath + n.Nav_Logo) ? VirPath + n.Nav_Logo : "";
            }
            return navi.Count > 0 ? _MenuNode(null, navi) : null;
        }
        /// <summary>
        /// 电脑端web页导航，所有导航，树形数据
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="type">类型，主菜单main，底部菜单foot</param>
        /// <returns></returns>
        [Cache(AdminDisable = true)]
        public JArray WebAll(int orgid, string type)
        {
            if (orgid <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }

            List<Song.Entities.Navigation> navi = Business.Do<IStyle>().NaviAll(null, "web", type, orgid);
            foreach (Song.Entities.Navigation n in navi)
            {
                n.Nav_Logo = System.IO.File.Exists(PhyPath + n.Nav_Logo) ? VirPath + n.Nav_Logo : "";
            }
            return navi.Count > 0 ? _MenuNode(null, navi) : null;
        }
        /// <summary>
        /// 生成菜单子节点
        /// </summary>
        /// <param name="item">当前菜单项</param>
        /// <param name="items">所有菜单项</param>
        /// <returns></returns>
        private JArray _MenuNode(Song.Entities.Navigation item, List<Song.Entities.Navigation> items)
        {
            JArray jarr = new JArray();
            foreach (Song.Entities.Navigation m in items)
            {
                if (item == null)
                {
                    if (!string.IsNullOrWhiteSpace(m.Nav_PID)) continue;
                }
                else
                {
                    if (m.Nav_PID != item.Nav_UID) continue;
                }
                string j = m.ToJson("", "Nav_CrtTime");
                JObject jo = JObject.Parse(j);
                jo.Add("id", "node_" + m.Nav_ID.ToString());
                jo.Add("label", m.Nav_Name);
                //字体样式
                JObject jfont = new JObject();
                jfont.Add("bold", m.Nav_IsBold);
                jfont.Add("color", m.Nav_Color);
                jo.Add("font", jfont);
                
                //计算下级
                JArray charray = _MenuNode(m, items);
                if (charray.Count > 0)
                    jo.Add("children", charray);
                jarr.Add(jo);
            }
            return jarr;
        }
        #endregion

        #region 修改
        /// <summary>
        /// 批量修改或创建菜单项
        /// </summary>
        /// <param name="site"></param>
        /// <param name="type"></param>
        /// <param name="orgid"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public bool ModifyMenus(string site, string type, int orgid, string items)
        {
            List<Song.Entities.Navigation> mlist = new List<Entities.Navigation>();
            _MenuUpdate(items, "", mlist);
            Business.Do<IStyle>().UpdateNavigation(site, type, orgid, "", mlist.ToArray());
            return false;
        }
        private void _MenuUpdate(string tree, string pid, List<Song.Entities.Navigation> mlist)
        {
            try
            {
                if (string.IsNullOrEmpty(tree)) return;
                JArray jarr = JArray.Parse(tree);
                for (int i = 0; i < jarr.Count; i++)
                {
                    string childJson = string.Empty;
                    Song.Entities.Navigation m = _MenuParse((JObject)jarr[i], out childJson);
                    if (string.IsNullOrWhiteSpace(m.Nav_UID))
                        m.Nav_UID = WeiSha.Core.Request.UniqueID();
                    m.Nav_Tax = i;
                    m.Nav_PID = pid;
                    mlist.Add(m);
                    if (m.Nav_Child > 0)
                    {
                        _MenuUpdate(childJson, m.Nav_UID, mlist);
                    }
                }
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        private Song.Entities.Navigation _MenuParse(JObject jo, out string childJson)
        {
            childJson = string.Empty;
            Song.Entities.Navigation mm = new Entities.Navigation();
            Type target = mm.GetType();
            IEnumerable<JProperty> properties = jo.Properties();
            foreach (JProperty item in properties)
            {
                string key = item.Name;
                string val = item.Value.ToString();

                PropertyInfo targetPP = target.GetProperty(key);
                if (targetPP != null)
                {
                    object tm = string.IsNullOrEmpty(val) ? null : WeiSha.Core.DataConvert.ChangeType(val.Trim(), targetPP.PropertyType);
                    targetPP.SetValue(mm, tm, null);
                }
                if (key.Equals("children", StringComparison.InvariantCultureIgnoreCase))
                {
                    childJson = item.Value.ToString();
                    if (childJson != "[]")
                        mm.Nav_Child = 1;
                }
            }
            return mm;
        }

        /// <summary>
        /// 修改显示状态
        /// </summary>
        /// <param name="id">导航id</param>
        /// <param name="show">是否显示</param>
        /// <returns></returns>
        [HttpPost]
        public bool ModifyState(int id,bool show)
        {
            return Business.Do<IStyle>().NaviState(id, show);
        }
        /// <summary>
        /// 修改导航菜单的图片
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Upload(Config = "NavigPhoto")]
        public Navigation ModifyLogo(Navigation entity)
        {
            string filename = string.Empty;

            //只保存第一张图片
            foreach (string key in this.Files)
            {
                HttpPostedFileBase file = this.Files[key];
                filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                file.SaveAs(PhyPath + filename);
                break;
            }
            if (!string.IsNullOrWhiteSpace(entity.Nav_Logo))
            {
                //删除原图
                if (System.IO.File.Exists(PhyPath + entity.Nav_Logo))
                    System.IO.File.Delete(PhyPath + entity.Nav_Logo);
            }
            entity.Nav_Logo = VirPath + filename;
            Song.Entities.Navigation old = Business.Do<IStyle>().NaviSingle(entity.Nav_ID);
            if (old != null)
            {
                if (!string.IsNullOrWhiteSpace(old.Nav_Logo))
                {
                    //删除原图
                    if (System.IO.File.Exists(PhyPath + old.Nav_Logo))
                        System.IO.File.Delete(PhyPath + old.Nav_Logo);
                }
                old.Nav_Logo = filename;
                Business.Do<IStyle>().NaviSaveLogo(old, filename);
                old.Nav_Logo = VirPath + filename;
                return old;
            }
            return entity;
        }
        /// <summary>
        /// 清除导航菜单项的图片
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpDelete]
        public bool ClearLogo(Song.Entities.Navigation item)
        {
            if (string.IsNullOrWhiteSpace(item.Nav_Logo)) return true;
            string img = item.Nav_Logo, filehy = string.Empty;
            if (img.IndexOf("/") > -1)
            {
                filehy = WeiSha.Core.Server.MapPath(item.Nav_Logo);
            }
            else
            {
                filehy = PhyPath + img;
            }
            try
            {
                if (System.IO.File.Exists(filehy))
                    System.IO.File.Delete(filehy);
                //删除缩略图，如果有
                string smallfile = WeiSha.Core.Images.Name.ToSmall(filehy);
                if (System.IO.File.Exists(smallfile))
                    System.IO.File.Delete(smallfile);
                //保存实体
                item.Nav_Logo = "";
                Song.Entities.Navigation old = Business.Do<IStyle>().NaviSingle(item.Nav_ID);
                if (old == null) return true;
                old.Nav_Logo = "";
                Business.Do<IStyle>().NaviSaveLogo(old, "");               
                return true;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        [HttpDelete]
        public bool Delete(int id)
        {

            try
            {
                Business.Do<IStyle>().NaviDelete(id);
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
