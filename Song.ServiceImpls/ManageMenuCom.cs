using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;
using System.Reflection;
using System.Xml;

namespace Song.ServiceImpls
{
    public class ManageMenuCom : IManageMenu
    {
        #region 菜单树的管理
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public int RootAdd(ManageMenu entity)
        {
            int id = this.Add(entity);
            if (id > 0)
            {
                entity = this.GetSingle(id);
                entity.MM_Root = id;
            }
            else
            {
                entity = Gateway.Default.From<ManageMenu>().OrderBy(ManageMenu._.MM_Id.Desc).ToFirst<ManageMenu>();
                entity.MM_Root = entity.MM_Id;
                id = entity.MM_Id;
            }
            entity.MM_Tax = this.GetMaxTaxis(0) + 1;
            entity.MM_PatId = entity.MM_PatId;
            Gateway.Default.Save<ManageMenu>(entity);
            return id;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public int RootSave(ManageMenu entity)
        {
            int id = entity.MM_Id;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<ManageMenu>(entity);
                    tran.Update<ManageMenu>(new Field[] { ManageMenu._.MM_IsUse, ManageMenu._.MM_IsShow }, new object[] { entity.MM_IsUse, entity.MM_IsShow }, ManageMenu._.MM_Root == entity.MM_Id);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;

                }
                finally
                {
                    tran.Close();
                }
            }

            return id;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public int RootDelete(ManageMenu entity)
        {
            return RootDelete(entity.MM_Id);
        }
        public int RootDelete(int identify)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    this._Delete(identify, tran);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
                finally
                {
                    tran.Close();
                }
            }
            return identify;
        }
        /// <summary>
        /// 获取对象；即所有栏目；
        /// </summary>
        /// <returns></returns>
        public ManageMenu[] GetRoot(int identify)
        {
            return Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_Root == identify).OrderBy(ManageMenu._.MM_Tax.Asc).ToArray<ManageMenu>();
        }
        public ManageMenu[] GetRoot(string func)
        {
            return Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_PatId == 0 && ManageMenu._.MM_Func == func).OrderBy(ManageMenu._.MM_Tax.Asc).ToArray<ManageMenu>();
        }
        public ManageMenu[] GetRoot(bool? isShow)
        {
            if (isShow == null)
            {
                return this.GetRoot("func");
            }
            return Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_PatId == 0 && ManageMenu._.MM_IsShow == isShow).OrderBy(ManageMenu._.MM_Tax.Asc).ToArray<ManageMenu>();
        }
        public ManageMenu[] GetRoot(string func, bool? isShow)
        {
            if (isShow == null)
            {
                return this.GetRoot(func);
            }
            return Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_PatId == 0 && ManageMenu._.MM_IsShow == isShow && ManageMenu._.MM_Func == func).OrderBy(ManageMenu._.MM_Tax.Asc).ToArray<ManageMenu>();
        }
        public ManageMenu[] GetRoot(int identify, bool? isShow)
        {
            if (isShow == null)
            {
                return this.GetRoot(identify);
            }
            return Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_Root == identify && ManageMenu._.MM_IsShow == isShow).OrderBy(ManageMenu._.MM_Tax.Asc).ToArray<ManageMenu>();
        }
        /// <summary>
        /// 获取树对象,即所有栏目；
        /// </summary>
        /// <param name="func"></param>
        /// <param name="isShow"></param>
        /// <returns></returns>
        public ManageMenu[] GetTree(string func, bool? isShow)
        {
            if (isShow == null)
            {
                return Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_Func == func).OrderBy(ManageMenu._.MM_Tax.Asc).ToArray<ManageMenu>();
            }
            return Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_Func == func && ManageMenu._.MM_IsShow == isShow).OrderBy(ManageMenu._.MM_Tax.Asc).ToArray<ManageMenu>();
        }
        public ManageMenu[] GetTree(string func, bool? isShow, bool? isUse)
        {
            if (isUse == null)
            {
                return this.GetTree(func, isShow);
            }
            if (isShow == null)
            {
                return Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_Func == func && ManageMenu._.MM_IsUse == isUse).OrderBy(ManageMenu._.MM_Tax.Asc).ToArray<ManageMenu>();
            }
            return Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_Func == func && ManageMenu._.MM_IsShow == isShow && ManageMenu._.MM_IsUse == isUse).OrderBy(ManageMenu._.MM_Tax.Asc).ToArray<ManageMenu>();
        }
        #endregion
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public int Add(ManageMenu entity)
        {
            //如果没有排序号，则自动计算
            if (entity.MM_Tax < 1)
            {
                object obj = Gateway.Default.Max<ManageMenu>(ManageMenu._.MM_Tax, ManageMenu._.MM_PatId == entity.MM_PatId);
                entity.MM_Tax = obj is int ? (int)obj + 1 : 0;
            }
            Gateway.Default.Save<ManageMenu>(entity);
            entity = Gateway.Default.From<ManageMenu>().OrderBy(ManageMenu._.MM_Id.Desc).ToFirst<ManageMenu>();
            return entity.MM_Id;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Save(ManageMenu entity)
        {
            //entity.MM_IsShow
            Gateway.Default.Save<ManageMenu>(entity);
        }
        /// <summary>
        /// 修改排序
        /// </summary>
        /// <param name="xml"></param>
        public void SaveOrder(string xml)
        {
            XmlDocument resXml = new XmlDocument();
            resXml.XmlResolver = null; 
            using (DbTrans tran = Gateway.Default.BeginTrans())
                try
                {
                    resXml.LoadXml(xml, false);
                    XmlNodeList nodeList = resXml.SelectSingleNode("nodes").ChildNodes;
                    //取rootid
                    XmlNode nodes = resXml.SelectSingleNode("nodes");
                    XmlElement xenodes = (XmlElement)nodes;
                    //遍历所有子节点 
                    foreach (XmlNode xn in nodeList)
                    {
                        XmlElement xe = (XmlElement)xn;
                        int id = Convert.ToInt32(xe.Attributes["id"].Value);
                        int pid = Convert.ToInt32(xe.Attributes["pid"].Value);
                        int tax = Convert.ToInt32(xe.Attributes["tax"].Value);
                        bool state = Convert.ToBoolean(xe.Attributes["state"].Value);
                        Song.Entities.ManageMenu mm = this.GetSingle(id);
                        if (mm != null)
                        {
                            mm.MM_PatId = pid;
                            mm.MM_Tax = tax;
                            mm.MM_State = state;
                            tran.Save<ManageMenu>(mm);
                        }
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
                finally
                {
                    tran.Close();
                }
        }
        public void Move(ManageMenu entity, int rootid)
        {
            if (entity.MM_Root == rootid)
            {
                Gateway.Default.Save<ManageMenu>(entity);
            }
            if (entity.MM_Root != rootid)
            {
                entity.MM_PatId = rootid;
                entity.MM_Tax = 0;
                this._move(entity, rootid);
            }
        }
        private void _move(ManageMenu entity, int rootid)
        {
            entity.MM_Root = rootid;
            Gateway.Default.Save<ManageMenu>(entity);
            //
            ManageMenu[] mm = this.GetChilds(entity.MM_Id);
            foreach (ManageMenu m in mm) _move(m, rootid);
        }
        public void Copy(ManageMenu entity, int rootid)
        {
            if (entity.MM_Root == rootid)
            {
                Gateway.Default.Save<ManageMenu>(entity);
            }
            if (entity.MM_Root != rootid)
            {
                entity.MM_PatId = rootid;
                _copty(entity, rootid, -1);
            }
        }
        /// <summary>
        /// 复制一个新增节点
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private ManageMenu _copySingle(ManageMenu entity)
        {
            ManageMenu mm = new ManageMenu();
            Type info = mm.GetType();
            //获取对象的属性列表
            PropertyInfo[] properties = info.GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo pi = properties[i];
                string pname = pi.Name;
                if (pi.Name.IndexOf("_") > -1)
                    pname = pname.Substring(pname.IndexOf("_") + 1);
                if (pname.Trim().ToLower() == "id") continue;
                //当前属性的值
                object obj = info.GetProperty(pi.Name).GetValue(entity, null);
                info.GetProperty(pi.Name).SetValue(mm, obj, null);
            }
            return mm;
        }
        private void _copty(ManageMenu entity, int rootid, int pid)
        {
            ManageMenu tm = _copySingle(entity);
            tm.MM_Root = rootid;
            if (pid > 0) tm.MM_PatId = pid;
            Gateway.Default.Save<ManageMenu>(tm);
            //
            ManageMenu[] mm = this.GetChilds(entity.MM_Id);
            foreach (ManageMenu m in mm) _copty(m, rootid, tm.MM_Id);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Delete(ManageMenu entity)
        {
            ManageMenu dep = Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_Id == entity.MM_Id).ToFirst<ManageMenu>();
            if (dep == null) return;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    this._Delete(dep.MM_Id, tran);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;

                }
                finally
                {
                    tran.Close();
                }
            }

        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void Delete(int identify)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
                try
                {
                    this._Delete(identify, tran);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;

                }
                finally
                {
                    tran.Close();
                }
        }
        /// <summary>
        /// 删除，按栏目名称
        /// </summary>
        /// <param name="name">栏目名称</param>
        public void Delete(string name)
        {
            ManageMenu dep = Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_Name == name).ToFirst<ManageMenu>();
            if (dep == null) return;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    this._Delete(dep.MM_Id, tran);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;

                }
                finally
                {
                    tran.Close();
                }
            }
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public ManageMenu GetSingle(int identify)
        {
            return Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_Id == identify).ToFirst<ManageMenu>();
        }
        /// <summary>
        /// 获取单一实体对象，按栏目名称
        /// </summary>
        /// <param name="name">栏目名称</param>
        /// <returns></returns>
        public ManageMenu GetSingle(string name)
        {
            return Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_Name == name).ToFirst<ManageMenu>();
        }
        /// <summary>
        /// 通过标识获取根节点
        /// </summary>
        /// <param name="marker"></param>
        /// <returns></returns>
        public ManageMenu GetRootMarker(string marker)
        {
            WhereClip wc = ManageMenu._.MM_Marker == marker && ManageMenu._.MM_PatId == 0;
            return Gateway.Default.From<ManageMenu>().Where(wc).OrderBy(ManageMenu._.MM_Tax.Asc).ToFirst<ManageMenu>();
        }
        /// <summary>
        /// 获取同一父级下的最大排序号；
        /// </summary>
        /// <param name="parentId">父Id</param>
        /// <returns></returns>
        public int GetMaxTaxis(int parentId)
        {
            //添加对象，并设置排序号
            object obj = Gateway.Default.Max<ManageMenu>(ManageMenu._.MM_Tax, ManageMenu._.MM_PatId == parentId);
            int tax = 0;
            if (obj is int)
            {
                tax = (int)obj;
            }
            return tax;
        }
        /// <summary>
        /// 获取当前对象的父级对象；
        /// </summary>
        /// <param name="identify">当前实体的主键</param>
        /// <returns></returns>
        public ManageMenu GetParent(int identify)
        {
            ManageMenu mm = this.GetSingle(identify);
            if (mm == null)
            {
                return null;
            }
            return this.GetSingle(mm.MM_Id);
        }
        /// <summary>
        /// 获取当前对象的父级对象；
        /// </summary>
        /// <param name="name">当前栏目的名称</param>
        /// <returns></returns>
        public ManageMenu GetParent(string name)
        {
            ManageMenu mm = this.GetSingle(name);
            if (mm == null)
            {
                return null;
            }
            return this.GetSingle(mm.MM_Id);
        }
        /// <summary>
        /// 获取对象；即所有栏目；
        /// </summary>
        /// <returns></returns>
        public ManageMenu[] GetAll()
        {
            return Gateway.Default.From<ManageMenu>().ToArray<ManageMenu>();
        }
        /// <summary>
        /// 获取对象；即所有可用栏目；
        /// </summary>
        /// <returns></returns>
        public ManageMenu[] GetAll(bool? isUse, bool? isShow)
        {
            WhereClip wc = ManageMenu._.MM_Name != "";
            if (isUse != null)
            {
                wc.And(ManageMenu._.MM_IsUse == isUse);
            }
            if (isShow != null)
            {
                wc.And(ManageMenu._.MM_IsShow == isShow);
            }
            return Gateway.Default.From<ManageMenu>().Where(wc).OrderBy(ManageMenu._.MM_Tax.Asc).ToArray<ManageMenu>();
        }
        /// <summary>
        /// 获取所有对象，功能菜单或系统菜菜
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="isShow"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ManageMenu[] GetAll(bool? isUse, bool? isShow, string type)
        {
            WhereClip wc = new WhereClip();
            if (isUse != null) wc.And(ManageMenu._.MM_IsUse == isUse);
            if (isShow != null) wc.And(ManageMenu._.MM_IsShow == isShow);
            if (type == "sys")
            {
                wc.And(ManageMenu._.MM_Func == type);
            }
            else
            {
                wc.And(ManageMenu._.MM_Func == "func");
            }
            return Gateway.Default.From<ManageMenu>().Where(wc).OrderBy(ManageMenu._.MM_Tax.Asc).ToArray<ManageMenu>();
        }
        public ManageMenu[] GetAll(int rootid, bool? isUse, bool? isShow, string type)
        {
            if (rootid < 1) return GetAll(isUse, isShow, type);
            WhereClip wc = new WhereClip();
            if (rootid > 0) wc.And(ManageMenu._.MM_Root == rootid);
            if (isUse != null) wc.And(ManageMenu._.MM_IsUse == isUse);
            if (isShow != null) wc.And(ManageMenu._.MM_IsShow == isShow);
            if (type == "sys")
            {
                wc.And(ManageMenu._.MM_Func == type);
            }
            else
            {
                wc.And(ManageMenu._.MM_Func == "func");
            }
            return Gateway.Default.From<ManageMenu>().Where(wc).OrderBy(ManageMenu._.MM_Tax.Asc).ToArray<ManageMenu>();
        }
        /// <summary>
        /// 获取当前对象的子级对象；
        /// </summary>
        /// <param name="identify">当前实体的主键</param>
        /// <returns></returns>
        public ManageMenu[] GetChilds(int identify)
        {
            //仅取当前对象的下一级对象；
            return Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_PatId == identify).OrderBy(ManageMenu._.MM_Tax.Asc).ToArray<ManageMenu>();
        }
        public ManageMenu[] GetChilds(int identify, bool? isUse, bool? isShow)
        {
            WhereClip wc = ManageMenu._.MM_PatId == identify;
            if (isUse != null) wc.And(ManageMenu._.MM_IsUse == isUse);
            if (isShow != null) wc.And(ManageMenu._.MM_IsShow == isShow);
            return Gateway.Default.From<ManageMenu>().Where(wc).OrderBy(ManageMenu._.MM_Tax.Asc).ToArray<ManageMenu>();

        }
        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns>重名返回true，否则返回false</returns>
        public bool IsExist(ManageMenu entity)
        {
            ManageMenu mm = null;
            //如果是一个新对象
            if (entity.MM_Id == 0)
            {
                mm = Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_Name == entity.MM_Name).ToFirst<ManageMenu>();
            }
            else
            {
                //如果是一个已经存在的对象，则不匹配自己
                mm = Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_Name == entity.MM_Name && ManageMenu._.MM_Id != entity.MM_Id).ToFirst<ManageMenu>();
            }
            return mm != null;
        }
        /// <summary>
        /// 在当前对象的同级（兄弟中），该对象是否重名，
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <param name="isSibling">是否限制在当前层的判断；true，表示仅在当前层判断，false表示在所有对象中判断</param>
        /// <returns></returns>
        public bool IsExist(ManageMenu entity, bool isSibling)
        {
            //在当前兄弟中判断
            if (isSibling)
            {
                ManageMenu mm = null;
                //如果是一个新对象
                if (entity.MM_Id == 0)
                {
                    mm = Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_Name == entity.MM_Name && ManageMenu._.MM_PatId == entity.MM_PatId).ToFirst<ManageMenu>();
                }
                else
                {
                    //如果是一个已经存在的对象，则不匹配自己
                    mm = Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_Name == entity.MM_Name && ManageMenu._.MM_Id != entity.MM_Id && ManageMenu._.MM_PatId == entity.MM_PatId).ToFirst<ManageMenu>();
                }
                return mm != null;
            }
            //在所有对象有判断
            return IsExist(entity);
        }/// <summary>
        /// 移动对象到其它节点下；
        /// </summary>
        /// <param name="currentId">当前对象id</param>
        /// <param name="parentId">要移动到某个节点下的id，即父节点id,必须大于-1</param>
        /// <returns>移动成功返回true，否则返回false</returns>
        public bool Remove(int currentId, int parentId)
        {
            if (parentId < 0)
            {
                return false;
            }
            ManageMenu ws = Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_Id == currentId).ToFirst<ManageMenu>();
            ws.MM_PatId = parentId;
            Gateway.Default.Save<ManageMenu>(ws);
            return true;
        }
        /// <summary>
        /// 将当前栏目向上移动；仅在当前对象的同层移动，即同一父节点下的对象之间移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool RemoveUp(int id)
        {
            //当前对象
            ManageMenu current = Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_Id == id).ToFirst<ManageMenu>();
            //当前对象父节点id;
            int parentId = (int)current.MM_PatId;
            //当前对象排序号
            int orderValue = (int)current.MM_Tax;
            //上一个对象，即兄长对象；
            ManageMenu up = Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_PatId == parentId && ManageMenu._.MM_Tax < orderValue).OrderBy(ManageMenu._.MM_Tax.Desc).ToFirst<ManageMenu>();
            if (up == null)
            {
                //如果兄长对象不存在，则表示当前节点在兄弟中是老大；即是最顶点；
                return false;
            }
            //交换排序号
            current.MM_Tax = up.MM_Tax;
            up.MM_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<ManageMenu>(current);
                    tran.Save<ManageMenu>(up);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;

                }
                finally
                {
                    tran.Close();
                }
            }
            return true;
        }
        /// <summary>
        /// 将当前栏目向下移动；仅在当前对象的同层移动，即同一父节点下的对象之间移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于最底端，则返回false；移动成功，返回true</returns>
        public bool RemoveDown(int id)
        {
            //当前对象
            ManageMenu current = Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_Id == id).ToFirst<ManageMenu>();
            //当前对象父节点id;
            int parentId = (int)current.MM_PatId;
            //当前对象排序号
            int orderValue = (int)current.MM_Tax;
            //下一个对象，即弟弟对象；
            ManageMenu down = Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_PatId == parentId && ManageMenu._.MM_Tax > orderValue).OrderBy(ManageMenu._.MM_Tax.Asc).ToFirst<ManageMenu>();
            if (down == null)
            {
                //如果弟对象不存在，则表示当前节点在兄弟中是老幺；即是最底端；
                return false;
            }
            //交换排序号
            current.MM_Tax = down.MM_Tax;
            down.MM_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<ManageMenu>(current);
                    tran.Save<ManageMenu>(down);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;

                }
                finally
                {
                    tran.Close();
                }
            }
            return true;
        }
        public DataTable GetFuncPoint(int identify)
        {
            DataSet ds = Gateway.Default.From<ManageMenu_Point>().ToDataSet();
            //所有对象的dataTable
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        #region 私有方法

        /// <summary>
        /// 私有对象，用于删除对象的子级，以及相关信息
        /// </summary>
        /// <param name="id"></param>
        private void _Delete(int id)
        {
            _Delete(id, null);
        }
        private void _Delete(int id, DbTrans tran)
        {
            ManageMenu[] ws = tran.From<ManageMenu>().Where(ManageMenu._.MM_PatId == id).ToArray<ManageMenu>();
            //删除子级
            foreach (ManageMenu w in ws)
            {
                _Delete(w.MM_Id, tran);
            }
            //删除菜单与权限的关联
            tran.Delete<Purview>(Purview._.MM_Id == id);
            //删除自身
            tran.Delete<ManageMenu>(ManageMenu._.MM_Id == id);
        }
        private void _Save(ManageMenu entity, int rootid)
        {
            entity.MM_Root = rootid;
            Gateway.Default.Save<ManageMenu>(entity);
            //获取下级比菜单项列表
            ManageMenu[] ws = Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_PatId == entity.MM_Id).ToArray<ManageMenu>();
            foreach (ManageMenu w in ws)
            {
                _Save(w, rootid);
            }

        }
        #endregion

    }
}
