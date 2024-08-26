using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using WeiSha.Core;
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
            entity.MM_Tax = this.GetMaxTaxis("0") + 1;
            entity.MM_PatId = "0";
            Gateway.Default.Save<ManageMenu>(entity);
            return id;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public int RootSave(ManageMenu entity)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<ManageMenu>(entity);
                    //tran.Update<ManageMenu>(new Field[] { ManageMenu._.MM_IsUse, ManageMenu._.MM_IsShow },
                    //    new object[] { entity.MM_IsUse, entity.MM_IsShow },
                    //    ManageMenu._.MM_Root == entity.MM_Id);
                    tran.Commit();
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            return entity.MM_Id;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public int RootDelete(ManageMenu entity)
        {
            return RootDelete(entity.MM_UID);
        }
        public int RootDelete(string uid)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    this._Delete(uid, tran);
                    tran.Commit();
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            return 0;
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
            return Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_PatId == "0" && ManageMenu._.MM_Func == func).OrderBy(ManageMenu._.MM_Tax.Asc).ToArray<ManageMenu>();
        }
        public ManageMenu[] GetRoot(bool? isShow)
        {
            if (isShow == null)
            {
                return this.GetRoot("func");
            }
            return Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_PatId == "0" && ManageMenu._.MM_IsShow == isShow).OrderBy(ManageMenu._.MM_Tax.Asc).ToArray<ManageMenu>();
        }
        public ManageMenu[] GetRoot(string func, bool? isShow)
        {
            if (isShow == null)
            {
                return this.GetRoot(func);
            }
            return Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_PatId == "0" && ManageMenu._.MM_IsShow == isShow && ManageMenu._.MM_Func == func).OrderBy(ManageMenu._.MM_Tax.Asc).ToArray<ManageMenu>();
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
            //if (entity.MM_Id <= 0) entity.MM_Id = WeiSha.Core.Request.SnowID();
            //如果没有排序号，则自动计算
            if (entity.MM_Tax < 1)
            {
                object obj = Gateway.Default.Max<ManageMenu>(ManageMenu._.MM_Tax, ManageMenu._.MM_PatId == entity.MM_PatId);
                entity.MM_Tax = obj is int ? (int)obj + 1 : 0;
            }
            if (string.IsNullOrWhiteSpace(entity.MM_UID))
                entity.MM_UID = WeiSha.Core.Request.UniqueID();
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
                    this._Delete(dep.MM_UID, tran);
                    tran.Commit();
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }

        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void Delete(int identify)
        {

            ManageMenu dep = Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_Id == identify).ToFirst<ManageMenu>();
            if (dep == null) return;
            if (dep.MM_IsFixed)
            {
                string msg = string.Format("菜单项：{0}，不允许被删除", dep.MM_Name);
                throw new Exception(msg);
            }
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    this._Delete(dep.MM_UID, tran);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
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
                    this._Delete(dep.MM_UID, tran);
                    tran.Commit();
                }
                catch (Exception ex) { tran.Rollback(); throw ex; }
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
        /// 获取单一实体对象
        /// </summary>
        /// <param name="uid">菜单的全局一标识</param>
        /// <returns></returns>
        public ManageMenu GetSingle(string uid)
        {
            return Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_UID == uid).ToFirst<ManageMenu>();
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
        public int GetMaxTaxis(string parentId)
        {
            //添加对象，并设置排序号
            object obj = Gateway.Default.Max<ManageMenu>(ManageMenu._.MM_Tax, ManageMenu._.MM_PatId == parentId);
            return obj is int ? (int)obj : 0;
        }        
        /// <summary>
        /// 获取对象；即所有栏目；
        /// </summary>
        /// <returns></returns>
        public List<ManageMenu> GetAll()
        {
            return Gateway.Default.From<ManageMenu>().ToList<ManageMenu>();
        }
        /// <summary>
        /// 获取对象；即所有可用栏目；
        /// </summary>
        /// <returns></returns>
        public List<ManageMenu> GetAll(bool? isUse, bool? isShow)
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
            return Gateway.Default.From<ManageMenu>().Where(wc).OrderBy(ManageMenu._.MM_Tax.Asc).ToList<ManageMenu>();
        }
        /// <summary>
        /// 获取所有对象，功能菜单或系统菜菜
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="isShow"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<ManageMenu> GetAll(bool? isUse, bool? isShow, string type)
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
            return Gateway.Default.From<ManageMenu>().Where(wc).OrderBy(ManageMenu._.MM_Tax.Asc).ToList<ManageMenu>();
        }
        //public ManageMenu[] GetAll(int rootid, bool? isUse, bool? isShow, string type)
        //{
        //    if (rootid < 1) return GetAll(isUse, isShow, type);
        //    WhereClip wc = new WhereClip();
        //    if (rootid > 0) wc.And(ManageMenu._.MM_Root == rootid);
        //    if (isUse != null) wc.And(ManageMenu._.MM_IsUse == isUse);
        //    if (isShow != null) wc.And(ManageMenu._.MM_IsShow == isShow);
        //    if (type == "sys")
        //    {
        //        wc.And(ManageMenu._.MM_Func == type);
        //    }
        //    else
        //    {
        //        wc.And(ManageMenu._.MM_Func == "func");
        //    }
        //    return Gateway.Default.From<ManageMenu>().Where(wc).OrderBy(ManageMenu._.MM_Tax.Asc).ToArray<ManageMenu>();
        //}
        /// <summary>
        /// 获取某一个根菜单下的所有分级
        /// </summary>
        /// <param name="uid">根节点id的uid</param>
        /// <param name="isUse"></param>
        /// <param name="isShow"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<ManageMenu> GetFunctionMenu(string uid, bool? isUse, bool? isShow)
        {
            List<ManageMenu> list = new List<ManageMenu>();
            WhereClip wc = ManageMenu._.MM_Func == "func";          
            if (isUse != null) wc.And(ManageMenu._.MM_IsUse == isUse);
            if (isShow != null) wc.And(ManageMenu._.MM_IsShow == isShow);
            _GetFunctionMenu(uid, wc, list);
            return list;
        }
        private List<ManageMenu> _GetFunctionMenu(string uid, WhereClip wc, List<ManageMenu> list)
        {
            if(list==null)list= new List<ManageMenu>();
            WhereClip where = new WhereClip();
            if (!string.IsNullOrWhiteSpace(uid)) where.And(ManageMenu._.MM_PatId == uid);
            where.And(wc);
            ManageMenu[] mms = Gateway.Default.From<ManageMenu>().Where(where).OrderBy(ManageMenu._.MM_Tax.Asc).ToArray<ManageMenu>();
            foreach(ManageMenu m in mms)
            {
                list.Add(m);
                _GetFunctionMenu(m.MM_UID, wc, list);
            }
            return list;
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
        }

        /// <summary>
        /// 更改根菜单顺序
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool UpdateTaxis(ManageMenu[] items)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (ManageMenu item in items)
                    {
                        tran.Update<ManageMenu>(
                            new Field[] { ManageMenu._.MM_Tax },
                            new object[] { item.MM_Tax },
                            ManageMenu._.MM_Id == item.MM_Id);
                    }
                    tran.Commit();
                    return true;
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 更新菜单树
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool UpdateSystemTree(ManageMenu[] items)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Delete<ManageMenu>(ManageMenu._.MM_Func=="sys");
                    foreach (ManageMenu item in items)
                    {
                        item.MM_Func = "sys";
                        if (string.IsNullOrWhiteSpace(item.MM_Name))
                        {
                            item.MM_Name = "null";
                        }
                        tran.Save<ManageMenu>(item);
                    }
                    tran.Commit();
                    return true;
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 更新功能菜单树
        /// </summary>
        /// <param name="uid">根菜单uid</param>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool UpdateFunctionTree(string uid, ManageMenu[] items)
        {
            List<ManageMenu> mms = this.GetFunctionMenu(uid, null, null);
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (ManageMenu m in mms)
                    {
                        tran.Delete<ManageMenu>(ManageMenu._.MM_Id == m.MM_Id);
                    }
                    foreach (ManageMenu item in items)
                    {
                        item.MM_Func = "func";
                        if (string.IsNullOrWhiteSpace(item.MM_Name))
                            item.MM_Name = "null";
                        if (string.IsNullOrWhiteSpace(item.MM_UID))
                            item.MM_UID = WeiSha.Core.Request.UniqueID();
                        tran.Save<ManageMenu>(item);
                    }
                    tran.Commit();
                    return true;
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }               
            }
        }
        #region 私有方法

        /// <summary>
        /// 私有对象，用于删除对象的子级，以及相关信息
        /// </summary>
        /// <param name="uid"></param>
        private void _Delete(string uid)
        {
            _Delete(uid, null);
        }
        private void _Delete(string uid, DbTrans tran)
        {
            ManageMenu[] ws = tran.From<ManageMenu>().Where(ManageMenu._.MM_PatId == uid).ToArray<ManageMenu>();
            //删除子级
            foreach (ManageMenu w in ws)
            {
                _Delete(w.MM_UID, tran);
            }
            //删除菜单与权限的关联
            tran.Delete<Purview>(Purview._.MM_UID == uid);
            //删除自身
            tran.Delete<ManageMenu>(ManageMenu._.MM_UID == uid);
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
