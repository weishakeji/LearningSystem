using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;
using System.Xml;



namespace Song.ServiceImpls
{
    public class DepartCom :IDepart
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public int Add(Depart entity)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            //如果没有排序号，则自动计算
            if (entity.Dep_Tax < 1)
            {
                object obj = Gateway.Default.Max<Depart>(Depart._.Dep_Tax, Depart._.Org_ID == org.Org_ID && Depart._.Dep_PatId == entity.Dep_PatId);
                entity.Dep_Tax = obj is int ? (int)obj + 1 : 0;
            }
            //
            Gateway.Default.Save<Depart>(entity);
            entity = Gateway.Default.From<Depart>().OrderBy(Depart._.Dep_Id.Desc).ToFirst<Depart>();
            return entity.Dep_Id;
            
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Save(Depart entity)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Depart>(entity);
                    tran.Update<EmpAccount>(new Field[] { EmpAccount._.Dep_CnName }, new object[] { entity.Dep_CnName }, EmpAccount._.Dep_Id == entity.Dep_Id);
                    tran.Update<Subject>(new Field[] { Subject._.Dep_CnName }, new object[] { entity.Dep_CnName }, Subject._.Dep_Id == entity.Dep_Id);
                    tran.Update<Course>(new Field[] { Course._.Dep_CnName }, new object[] { entity.Dep_CnName }, Course._.Dep_Id == entity.Dep_Id);
                    //tran.Update<Teacher>(new Field[] { Teacher._.Dep_CnName }, new object[] { entity.Dep_CnName }, Teacher._.Dep_Id == entity.Dep_Id);
                    tran.Update<StudentSort>(new Field[] { StudentSort._.Dep_CnName }, new object[] { entity.Dep_CnName }, StudentSort._.Dep_Id == entity.Dep_Id);
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
        /// 修改院系排序
        /// </summary>
        /// <param name="xml"></param>
        public void SaveOrder(string xml)
        {           
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    XmlDocument resXml = new XmlDocument();
                    resXml.XmlResolver = null; 
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
                        Song.Entities.Depart dep = this.GetSingle(id);
                        if (dep != null)
                        {
                            dep.Dep_PatId = pid;
                            dep.Dep_Tax = tax;
                            dep.Dep_State = state;
                            tran.Save<Depart>(dep);
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
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Delete(Depart entity)
        {
            Depart dep = Gateway.Default.From<Depart>().Where(Depart._.Dep_Id == entity.Dep_Id).ToFirst<Depart>();
            if (dep == null) return;
            this.Delete(dep.Dep_Id);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void Delete(int identify)
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
        }
        /// <summary>
        /// 删除，按院系名称
        /// </summary>
        /// <param name="name">院系名称</param>
        public void Delete(string name)
        {
            int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
            Depart dep = Gateway.Default.From<Depart>().Where(Depart._.Org_ID == orgid && Depart._.Dep_CnName == name).ToFirst<Depart>();
            if (dep == null) return;
            this.Delete(dep.Dep_Id);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public Depart GetSingle(int identify)
        {
            return Gateway.Default.From<Depart>().Where(Depart._.Dep_Id==identify).ToFirst<Depart>();
        }
        /// <summary>
        /// 获取单一实体对象，按院系名称
        /// </summary>
        /// <param name="name">院系名称</param>
        /// <returns></returns>
        public Depart GetSingle(string name)
        {
            int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
            return Gateway.Default.From<Depart>().Where(Depart._.Org_ID == orgid && Depart._.Dep_CnName == name).ToFirst<Depart>();
        }
        /// <summary>
        /// 获取当前对象的父级对象；
        /// </summary>
        /// <param name="identify">当前实体的主键</param>
        /// <returns></returns>
        public Depart GetParent(int identify)
        {
            Depart mm = this.GetSingle(identify);
            if (mm == null)
            {
                return null;
            }
            return this.GetSingle(mm.Dep_Id);
        }
        /// <summary>
        /// 获取当前对象的父级对象；
        /// </summary>
        /// <param name="name">当前院系的名称</param>
        /// <returns></returns>
        public Depart GetParent(string name)
        {
            Depart mm = this.GetSingle(name);
            if (mm == null)
            {
                return null;
            }
            WhereClip wc = new WhereClip();

            return this.GetSingle(mm.Dep_Id);
        }
        /// <summary>
        /// 获取对象；即所有院系；
        /// </summary>
        /// <returns></returns>
        public Depart[] GetAll(int orgid)
        {
            return Gateway.Default.From<Depart>().Where(Depart._.Org_ID==orgid).OrderBy(Depart._.Dep_Tax.Asc).ToArray<Depart>();
        }
        public Depart[] GetAll(int orgid, bool? isUse, bool? isShow)
        {
            WhereClip wc = new WhereClip();
            if (isUse != null) wc &= Depart._.Dep_IsUse == (bool)isUse;
            if (isShow != null) wc &= Depart._.Dep_IsShow == (bool)isShow;
            return Gateway.Default.From<Depart>()
                .Where(Depart._.Org_ID == orgid && wc)
                .OrderBy(Depart._.Dep_Tax.Asc).ToArray<Depart>();
        }

        /// <summary>
        /// 获取当前对象的子级对象；
        /// </summary>
        /// <param name="identify">当前实体的主键</param>
        /// <returns></returns>
        public Depart[] GetChilds(int identify)
        {
            //仅取当前对象的下一级对象；
            return Gateway.Default.From<Depart>().Where(Depart._.Dep_PatId == identify).OrderBy(Depart._.Dep_Tax.Desc).ToArray<Depart>();           
        }
        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns>重名返回true，否则返回false</returns>
        public bool IsExist(int orgid, Depart entity)
        {
            Depart mm = null;
            //如果是一个新对象
            if (entity.Dep_Id == 0)
            {
                mm = Gateway.Default.From<Depart>()
                    .Where(Depart._.Org_ID == orgid && Depart._.Dep_CnName == entity.Dep_CnName)
                    .ToFirst<Depart>();
            }
            else
            {
                //如果是一个已经存在的对象，则不匹配自己
                mm = Gateway.Default.From<Depart>()
                    .Where(Depart._.Org_ID == orgid && Depart._.Dep_CnName == entity.Dep_CnName && Depart._.Dep_Id != entity.Dep_Id)
                    .ToFirst<Depart>();
            }
            return mm != null;
        }
        /// <summary>
        /// 在当前对象的同级（兄弟中），该对象是否重名，
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <param name="isSibling">是否限制在当前层的判断；true，表示仅在当前层判断，false表示在所有对象中判断</param>
        /// <returns></returns>
        public bool IsExist(int orgid, Depart entity, bool isSibling)
        {
            //在当前兄弟中判断
            if (isSibling)
            {
                Depart mm = null;
                //如果是一个新对象
                if (entity.Dep_Id == 0)
                {
                    mm = Gateway.Default.From<Depart>()
                        .Where(Depart._.Org_ID == orgid && Depart._.Dep_CnName == entity.Dep_CnName && Depart._.Dep_PatId == entity.Dep_PatId)
                        .ToFirst<Depart>();
                }
                else
                {
                    //如果是一个已经存在的对象，则不匹配自己
                    mm = Gateway.Default.From<Depart>()
                        .Where(Depart._.Org_ID == orgid && Depart._.Dep_CnName == entity.Dep_CnName && Depart._.Dep_Id != entity.Dep_CnName && Depart._.Dep_PatId == entity.Dep_PatId)
                        .ToFirst<Depart>();
                }
                return mm != null;
            }
            //在所有对象有判断
            return IsExist(orgid,entity);
        }
        /// <summary>
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
            Depart ws = Gateway.Default.From<Depart>().Where(Depart._.Dep_Id == currentId).ToFirst<Depart>();
            ws.Dep_PatId = parentId;
            Gateway.Default.Save<Depart>(ws);
            return true;
        }
        public bool RemoveUp(int id)
        {
            //当前对象
            Depart current = Gateway.Default.From<Depart>().Where(Depart._.Dep_Id == id).ToFirst<Depart>();
            //当前对象排序号
            int orderValue = (int)current.Dep_Tax;
            //上一个对象，即兄长对象；
            Depart up = Gateway.Default.From<Depart>()
                .Where(Depart._.Dep_Tax < orderValue && Depart._.Org_ID == current.Org_ID && Depart._.Dep_PatId == current.Dep_PatId)
                .OrderBy(Depart._.Dep_Tax.Desc).ToFirst<Depart>();
            if (up == null) return false;
            //交换排序号
            current.Dep_Tax = up.Dep_Tax;
            up.Dep_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Depart>(current);
                    tran.Save<Depart>(up);
                    tran.Commit();
                    return true;
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

        public bool RemoveDown(int id)
        {
            //当前对象
            Depart current = Gateway.Default.From<Depart>().Where(Depart._.Dep_Id == id).ToFirst<Depart>();
            //当前对象排序号
            int orderValue = (int)current.Dep_Tax;
            //下一个对象，即弟弟对象；
            Depart down = Gateway.Default.From<Depart>()
                .Where(Depart._.Dep_Tax > orderValue && Depart._.Org_ID == current.Org_ID && Depart._.Dep_PatId == current.Dep_PatId)
                .OrderBy(Depart._.Dep_Tax.Asc).ToFirst<Depart>();
            if (down == null) return false;
            //交换排序号
            current.Dep_Tax = down.Dep_Tax;
            down.Dep_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Depart>(current);
                    tran.Save<Depart>(down);
                    tran.Commit();
                    return true;
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
        #region 私有方法
        /// <summary>
        /// 私有对象，用于删除对象的子级，以及相关信息
        /// </summary>
        /// <param name="id"></param>
        private void _Delete(int id,DbTrans tran)
        {
            Depart[] ws = Gateway.Default.From<Depart>().Where(Depart._.Dep_PatId == id).ToArray<Depart>();
            if (ws != null)
            {
                foreach (Depart w in ws)
                {
                    //删除子级
                    _Delete(w.Dep_Id, tran);
                }
            }
            //删除自身
            tran.Delete<Depart>(Depart._.Dep_Id == id);
            tran.Delete<Purview>(Purview._.Dep_Id == id);
            tran.Update<EmpAccount>(new Field[] { EmpAccount._.Dep_CnName }, new object[] { "" }, EmpAccount._.Dep_Id == id);
        }
        #endregion
    }
}
