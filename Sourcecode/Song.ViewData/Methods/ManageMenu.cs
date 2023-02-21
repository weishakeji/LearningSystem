#define RELEASE
#define DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 系统管理菜单的管理，包括后台管理、学员、教师等的菜单都在这里管理
    /// </summary>
    [HttpPut, HttpGet]
    public class ManageMenu : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 根菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet, SuperAdmin]
        public Song.Entities.ManageMenu[] Root()
        {
            return  Business.Do<IManageMenu>().GetRoot("func");
        }
        /// <summary>
        /// 所有启用的根菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Song.Entities.ManageMenu[] EnableRoot()
        {
            return Business.Do<IManageMenu>().GetRoot("func", true);
        }
        /// <summary>
        /// 更改根菜单的排序
        /// </summary>
        /// <param name="items">根菜单的数组</param>
        /// <returns></returns>
        [HttpPost]
        [SuperAdmin]
        public bool ModifyTaxis(Song.Entities.ManageMenu[] items)
        {
            try
            {
                Business.Do<IManageMenu>().UpdateTaxis(items);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取功能菜单的对象信息
        /// </summary>
        /// <param name="id">菜单id</param>
        /// <returns></returns>
        [HttpGet]
        public Song.Entities.ManageMenu ForID(int id)
        {
            return Business.Do<IManageMenu>().GetSingle(id);
        }
        /// <summary>
        /// 通过uid获取菜单对象
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public Song.Entities.ManageMenu ForUID(string uid)
        {
            return Business.Do<IManageMenu>().GetSingle(uid);
        }
        /// <summary>
        /// 修改菜单对象
        /// </summary>
        /// <param name="mm">菜单对象</param>
        /// <returns></returns>
        [HttpPost, SuperAdmin]
        public bool Modify(Song.Entities.ManageMenu mm)
        {
            Song.Entities.ManageMenu old = Business.Do<IManageMenu>().GetSingle(mm.MM_Id);
            if (old == null) throw new Exception("对象不存在！");           
            old.Copy<Song.Entities.ManageMenu>(mm);
            Business.Do<IManageMenu>().Save(old);
            return true;
        }
        /// <summary>
        /// 修改根菜单
        /// </summary>
        /// <param name="mm"></param>
        /// <returns></returns>
        [HttpPost, SuperAdmin]
        public bool ModifyFuncRoot(Song.Entities.ManageMenu mm)
        {
            Song.Entities.ManageMenu old = Business.Do<IManageMenu>().GetSingle(mm.MM_Id);
            if (old == null) throw new Exception("对象不存在！");
            old.Copy<Song.Entities.ManageMenu>(mm, "MM_Marker");
            Business.Do<IManageMenu>().RootSave(old);
            return true;
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id">可以是多个，用逗号分隔</param>
        /// <returns>返回删除的个数</returns>
        [HttpDelete,SuperAdmin]
        public int Delete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            string[] arr = id.Split(',');
            foreach(string s in arr)
            {
                int idval = 0;
                int.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    Business.Do<IManageMenu>().Delete(idval);
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
        /// 添加功能菜单的根菜单
        /// </summary>
        /// <param name="mm">菜单对象</param>
        /// <returns></returns>
        [HttpPost,SuperAdmin]
        public bool AddFuncRoot(Song.Entities.ManageMenu mm)
        {
            try
            {
                mm.MM_PatId = "0";
                mm.MM_Func = "func";
                Business.Do<IManageMenu>().RootAdd(mm);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 将功能菜单，从一个根菜单移到另一个根菜单
        /// </summary>
        /// <param name="cuid">当前菜单的uid</param>
        /// <param name="puid">要移动到的菜单的uid</param>
        /// <returns></returns>
        [SuperAdmin]
        [HttpPost]
        public bool FuncMoveRoot(string cuid,string puid)
        {
            Song.Entities.ManageMenu mm = Business.Do<IManageMenu>().GetSingle(cuid);
            if (mm == null) return false;
            mm.MM_PatId = puid;
            try
            {
                Business.Do<IManageMenu>().Save(mm);
                return true;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 更改菜单项的完成度，用于开发过程中方便查看工作进度，实际应用中用不到
        /// </summary>
        /// <param name="uid">菜单项的uid</param>
        /// <param name="complete">完成度</param>
        /// <returns></returns>
        [SuperAdmin]
        [HttpPost]
        public bool UpdateComplete(string uid, int complete)
        {
            Song.Entities.ManageMenu mm = Business.Do<IManageMenu>().GetSingle(uid);
            if (mm == null) return false;
            mm.MM_Complete = complete;
            try
            {
                Business.Do<IManageMenu>().Save(mm);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取功能菜单的树
        /// </summary>
        /// <param name="uid">功能菜单根节点的uid</param>
        /// <returns></returns>
        [HttpGet]
        [SuperAdmin]
        public JArray FuncMenu(string uid)
        {
            List<Song.Entities.ManageMenu> mm = Business.Do<IManageMenu>().GetFunctionMenu(uid, null, null);
            if (mm.Count > 0)
            {
                Song.Entities.ManageMenu root=Business.Do<IManageMenu>().GetSingle(uid);
                JArray ja = _MenuNode(root, mm);
                return ja;
            }
            return null;
        }
        /// <summary>
        /// 供机构等级选择权限的菜单
        /// </summary>    
        /// <returns></returns>
        [HttpGet]
        [SuperAdmin]
        public JArray OrganPurviewSelect()
        {
            List<Song.Entities.ManageMenu> mm = Business.Do<IManageMenu>().GetFunctionMenu("0", true, false);
            if (mm.Count > 0)
            {
                return _MenuNode(null, mm);
            }
            return null;
        }
        /// <summary>
        /// 记录机构等级的权限选中信息
        /// </summary>
        /// <param name="lvid"></param>
        /// <param name="mms"></param>
        /// <returns></returns>
        [HttpPost]
        [SuperAdmin]
        public bool OrganPurviewSelected(int lvid,string[] mms)
        {
            Business.Do<IPurview>().BatchAdd(lvid, mms, "orglevel");
            return true;
        }
        /// <summary>
        /// 机构等级的权限菜单项的Uid
        /// </summary>
        /// <param name="lvid">机构等级的id</param>
        /// <returns></returns>
        public JArray OrganPurviewUID(int lvid)
        {
            Purview[] pur = Business.Do<IPurview>().OrganLevelItems(lvid);
            JArray ja = new JArray();
            for(int i = 0; i < pur.Length; i++)
            {
                ja.Add(pur[i].MM_UID);
            }
            return ja;
        }
        /// <summary>
        /// 机构下某一类marker标识的菜单项
        /// </summary>
        /// <param name="marker"></param>
        /// <returns></returns>
        [Cache(AdminDisable = true, Expires = 1440)]
        public JArray OrganMarkerMenus(string marker)
        {
            //JArray ja = new JArray();
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            List<Song.Entities.ManageMenu> mm = Business.Do<IPurview>().GetOrganPurview(org, marker);
            return mm.Count > 0 ? _MenuNode(null, mm) : null;
        }
        /// <summary>
        /// 系统菜单，即超级管理左上角菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SuperAdmin]
        public JArray SystemMenu()
        {
            Song.Entities.ManageMenu[] mm = Business.Do<IManageMenu>().GetAll(null, null, "sys");
            return mm.Length > 0 ? _MenuNode(null, mm.ToList<Song.Entities.ManageMenu>()) : null;
        }
        /// <summary>
        /// 显示系统菜单项
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Cache]
        public JArray SystemMenuShow()
        {
            Song.Entities.ManageMenu[] mm = Business.Do<IManageMenu>().GetAll(true, true, "sys");
            return mm.Length > 0 ? _MenuNode(null, mm.ToList<Song.Entities.ManageMenu>()) : null;
        }
        /// <summary>
        /// 生成菜单子节点
        /// </summary>
        /// <param name="item">当前菜单项</param>
        /// <param name="items">所有菜单项</param>
        /// <returns></returns>
        private JArray _MenuNode(Song.Entities.ManageMenu item, List<Song.Entities.ManageMenu> items)
        {
            JArray jarr = new JArray();
            bool islocal = WeiSha.Core.Server.IsLocalIP;
            foreach (Song.Entities.ManageMenu m in items)
            {

                if (item == null)
                {
                    if (m.MM_PatId != "0") continue;
                }
                else
                {
                    if (!m.MM_PatId.Equals(item.MM_UID, StringComparison.OrdinalIgnoreCase)) continue;
                }
                //如果不是本机id，则显示项单项完成（因为完成度只是为了在开发时记录一下完成状态）
                if (!islocal) m.MM_Complete = 100;

                string j = m.ToJson();

                JObject jo = JObject.Parse(j);
                jo.Add("id", "node_" + m.MM_UID.ToString());
                jo.Add("label", m.MM_Name);
                jo.Add("ico", string.IsNullOrWhiteSpace(m.MM_IcoS) ? "" : m.MM_IcoS);
                //字体样式
                JObject jfont = new JObject();
                jfont.Add("bold", m.MM_IsBold);
                jfont.Add("italic", m.MM_IsItalic);
                jfont.Add("color", m.MM_Color);
                jo.Add("font", jfont);
                //计算下级
                jo.Add("children", _MenuNode(m, items));
                jarr.Add(jo);
            }
            return jarr;
        }
        /// <summary>
        /// 更新系统菜单
        /// </summary>
        /// <param name="tree">来自客户端提交的json</param>
        /// <returns></returns>
        [HttpPost]
        [SuperAdmin]
        public bool SystemMenuUpdate(string tree)
        {
            List<Song.Entities.ManageMenu> mlist = new List<Entities.ManageMenu>();
            _MenuUpdate(tree, "0", mlist);
            Business.Do<IManageMenu>().UpdateSystemTree(mlist.ToArray());
            return true;
        }
        /// <summary>
        /// 更新功能菜单
        /// </summary>
        /// <param name="uid">功能菜单根菜单的uid</param>
        /// <param name="tree">来自客户端提交的json</param>
        /// <returns></returns>
        [HttpPost]
        [SuperAdmin]
        public bool FuncMenuUpdate(string uid, string tree)
        {
            List<Song.Entities.ManageMenu> mlist = new List<Entities.ManageMenu>();
            _MenuUpdate(tree, uid, mlist);
            Business.Do<IManageMenu>().UpdateFunctionTree(uid, mlist.ToArray());
            return true;
        }
        private void _MenuUpdate(string tree,string pid, List<Song.Entities.ManageMenu> mlist)
        {
            JArray jarr = JArray.Parse(tree);
            for (int i = 0; i < jarr.Count; i++)
            {
                string childJson = string.Empty;
                Song.Entities.ManageMenu m = _MenuParse((JObject)jarr[i], out childJson);
                if (string.IsNullOrWhiteSpace(m.MM_UID))
                    m.MM_UID = WeiSha.Core.Request.UniqueID();               
                m.MM_Tax = i;
                m.MM_PatId = pid;
                mlist.Add(m);
                if (m.MM_IsChilds)
                {
                    _MenuUpdate(childJson, m.MM_UID, mlist);
                }
            }
        }
        private Song.Entities.ManageMenu _MenuParse(JObject jo,out string childJson)
        {
            childJson = string.Empty;
            Song.Entities.ManageMenu mm = new Entities.ManageMenu();
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
                        mm.MM_IsChilds = true;                     
                }
            }
            return mm;
        }

        /// <summary>
        /// 当前管理员的菜单项
        /// </summary>
        /// <returns></returns>
        [Admin]
        [HttpGet]
        public JArray Menus()
        {
            Song.Entities.EmpAccount acc = LoginAdmin.Status.User(this.Letter);
            if (acc == null) throw new ExceptionForNoLogin();         
         
            if (LoginAdmin.Status.IsSuperAdmin(this.Letter))
            {
                List<Song.Entities.ManageMenu> mm = Business.Do<IManageMenu>().GetFunctionMenu("0", true, true);
                if (mm.Count > 0)
                {
                    return _MenuNode(null, mm);
                }
                return null;
            }
            else
            {
              
            }          
            return null;
        }
    }
}
