using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;



namespace Song.ServiceImpls
{
    public class AddressListCom : IAddressList
    {
        /// <summary>
        /// 清除所有信息
        /// </summary>
        public void Clear()
        {
            Gateway.Default.Delete<AddressList>(AddressList._.Adl_Id > 0);
            Gateway.Default.Delete<AddressSort>(AddressSort._.Ads_Id > 0);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void AddressAdd(AddressList entity)
        {
            //当前通讯录信息，归属于某个用户，记录其id
            entity.Acc_Id = Extend.LoginState.Admin.CurrentUser.Acc_Id;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            Gateway.Default.Save<AddressList>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void AddressSave(AddressList entity)
        {
            Gateway.Default.Save<AddressList>(entity);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void AddressDelete(AddressList entity)
        {
            Gateway.Default.Delete<AddressList>(AddressList._.Adl_Id == entity.Adl_Id);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void AddressDelete(int identify)
        {
            Gateway.Default.Delete<AddressList>(AddressList._.Adl_Id == identify);
        }
        /// <summary>
        /// 删除所有
        /// </summary>
        public void AddressDeleteAll()
        {
            Gateway.Default.Delete<AddressList>(AddressList._.Adl_Id > 0);
        }
        /// <summary>
        /// 删除，按人员名称
        /// </summary>
        /// <param name="name">人员名称</param>
        public void AddressDelete(string name)
        {
            Gateway.Default.Delete<AddressList>(AddressList._.Adl_Name == name);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public AddressList AddressSingle(int identify)
        {
            return Gateway.Default.From<AddressList>().Where(AddressList._.Adl_Id == identify).ToFirst<AddressList>();
        }
        public AddressList AddressSingle(string mobiTel)
        {
            return Gateway.Default.From<AddressList>().Where(AddressList._.Adl_MobileTel == mobiTel.Trim()).ToFirst<AddressList>();
        }
        /// <summary>
        /// 获取某个院系的所有人员；
        /// </summary>
        /// <param name="isShow">是否显示</param>
        /// <returns></returns>
        public List<AddressList> AddressAll()
        {
            return Gateway.Default.From<AddressList>().ToList<AddressList>();
        }
        public List<AddressList> AddressAll(int? sortId)
        {
            WhereClip wc = new WhereClip();
            if (sortId != null && sortId > 0) wc.And(AddressList._.Ads_Id == (int)sortId);
            return Gateway.Default.From<AddressList>().Where(wc).ToList<AddressList>();
        }
        /// <summary>
        /// 分页获取所有的人员；
        /// </summary>
        /// <param name="size">每页显示几条记录</param>
        /// <param name="index">当前第几页</param>
        /// <param name="countSum">记录总数</param>
        /// <returns></returns>
        public List<AddressList> AddressPager(int accId, int size, int index, out int countSum)
        {
            WhereClip wc = AddressList._.Acc_Id == accId;
            countSum = Gateway.Default.Count<AddressList>(wc);
            return Gateway.Default.From<AddressList>().Where(wc).OrderBy(AddressList._.Adl_Id.Desc).ToList<AddressList>(size, (index - 1) * size);
     
        }
        /// <summary>
        /// 分页获取所有的人员；
        /// </summary>
        /// <param name="sortId">分类id</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<AddressList> AddressPager(int accId, string typeName, string personName, int size, int index, out int countSum)
        {

            WhereClip wc = AddressList._.Acc_Id == accId;
            if (typeName.Trim()!="")
            {
                wc.And(AddressList._.Ads_Name == typeName);
            }
            if (personName.Trim() != "" && personName != null)
            {
                wc.And(AddressList._.Adl_Name.Like("%" + personName + "%"));
            }
            countSum = Gateway.Default.Count<AddressList>(wc);
            return Gateway.Default.From<AddressList>().Where(wc).OrderBy(AddressList._.Adl_Name.Asc).ToList<AddressList>(size, (index - 1) * size);
     
        }
        #region 通讯录分类
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public int SortAdd(AddressSort entity)
        {
            //添加对象，并设置排序号
            object obj = Gateway.Default.Max<AddressSort>( AddressSort._.Ads_Tax,AddressSort._.Ads_Id > -1);
            int tax = 0;
            if (obj is int) tax = (int)obj;
            entity.Ads_Tax = tax + 1;
            return Gateway.Default.Save<AddressSort>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void SortSave(AddressSort entity)
        {
            using (DbTrans trans = Gateway.Default.BeginTrans())
            {
                try
                {
                    trans.Save<AddressSort>(entity);
                    trans.Update<AddressList>(new Field[] { AddressList._.Ads_Name }, new object[] { entity.Ads_Name }, AddressList._.Ads_Id == entity.Ads_Id);
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
                finally
                {
                    trans.Close();
                }
            } 
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void SortDelete(AddressSort entity)
        {
            this.SortDelete(entity.Ads_Id);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void SortDelete(int identify)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    object obj = Gateway.Default.Count<AddressList>(AddressList._.Ads_Id == identify);
                    if ((int)obj > 0) throw new Exception("当前分类含有下属信息，请勿删除");
                    //
                    tran.Delete<AddressSort>(AddressSort._.Ads_Id);
                    tran.Delete<AddressList>(AddressList._.Ads_Id);
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
        /// 清除所有分类，包括通讯录信息
        /// </summary>
        public void SortDeleteAll()
        {
            Gateway.Default.Delete<AddressList>(AddressList._.Adl_Id > 0);
            Gateway.Default.Delete<AddressSort>(AddressSort._.Ads_Id > 0);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public AddressSort SortSingle(int identify)
        {
            return Gateway.Default.From<AddressSort>().Where(AddressSort._.Ads_Id == identify).ToFirst<AddressSort>();
        }
        /// <summary>
        /// 获取某个院系的所有人员；
        /// </summary>
        /// <param name="isUse">是否使用</param>
        /// <returns></returns>
        public List<AddressSort> SortAll(bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (isUse != null) wc.And(AddressSort._.Ads_IsUse == (bool)isUse);
            return Gateway.Default.From<AddressSort>().Where(wc).ToList<AddressSort>();
        }
        /// <summary>
        /// 分页获取；
        /// </summary>
        /// <param name="accId">所属人员的id</param>
        /// <param name="size">每页显示几条记录</param>
        /// <param name="index">当前第几页</param>
        /// <param name="countSum">记录总数</param>
        /// <returns></returns>
        public List<AddressSort> SortPager(int accId, int size, int index, out int countSum)
        {
            WhereClip wc = AddressSort._.Acc_Id == accId;
            countSum = Gateway.Default.Count<AddressSort>(wc);
            return Gateway.Default.From<AddressSort>().Where(wc).OrderBy(AddressSort._.Ads_Tax.Desc).ToList<AddressSort>(size, (index - 1) * size);
        }
        /// <summary>
        /// 分页获取所有的人员；
        /// </summary>
        /// <param name="accId">所属人员的id</param>
        /// <param name="sortName">分类名称</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<AddressSort> SortPager(int accId, string sortName, int size, int index, out int countSum)
        {
            WhereClip wc = AddressSort._.Acc_Id == accId;
            if (sortName != string.Empty) wc.And(AddressSort._.Ads_Name.Like("%" + sortName + "%"));
            countSum = Gateway.Default.Count<AddressSort>(wc);
            return Gateway.Default.From<AddressSort>().Where(wc).OrderBy(AddressSort._.Ads_Tax.Desc).ToList<AddressSort>(size, (index - 1) * size);
        }
        /// <suPsary>
        /// 将当前栏目向上移动；仅在当前对象的同层移动，即同一父节点下的对象之间移动；
        /// </suPsary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool SortRemoveUp(int id)
        {
            //当前对象
            AddressSort current = Gateway.Default.From<AddressSort>().Where(AddressSort._.Ads_Id == id).ToFirst<AddressSort>();
            //当前对象排序号
            int orderValue = (int)current.Ads_Tax;
            //上一个对象，即兄长对象；
            AddressSort up = Gateway.Default.From<AddressSort>().Where(AddressSort._.Ads_Tax > orderValue).OrderBy(AddressSort._.Ads_Tax.Asc).ToFirst<AddressSort>();
            if (up == null)
            {
                //如果兄长对象不存在，则表示当前节点在兄弟中是老大；即是最顶点；
                return false;
            }
            //交换排序号
            current.Ads_Tax = up.Ads_Tax;
            up.Ads_Tax = orderValue;
            using (DbTrans trans = Gateway.Default.BeginTrans())
            {
                try
                {
                    trans.Save<AddressSort>(current);
                    trans.Save<AddressSort>(up);
                    trans.Commit();
                    return true;
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
                finally
                {
                    trans.Close();
                }
            }             
            
        }
        /// <suPsary>
        /// 将当前栏目向下移动；仅在当前对象的同层移动，即同一父节点下的对象之间移动；
        /// </suPsary>
        /// <param name="id"></param>
        /// <returns>如果已经处于最底端，则返回false；移动成功，返回true</returns>
        public bool SortRemoveDown(int id)
        {
            //当前对象
            AddressSort current = Gateway.Default.From<AddressSort>().Where(AddressSort._.Ads_Id == id).ToFirst<AddressSort>();
            //当前对象排序号
            int orderValue = (int)current.Ads_Tax;
            //下一个对象，即弟弟对象；
            AddressSort down = Gateway.Default.From<AddressSort>().Where(AddressSort._.Ads_Tax < orderValue).OrderBy(AddressSort._.Ads_Tax.Desc).ToFirst<AddressSort>();
            if (down == null)
            {
                //如果弟对象不存在，则表示当前节点在兄弟中是老幺；即是最底端；
                return false;
            }
            //交换排序号
            current.Ads_Tax = down.Ads_Tax;
            down.Ads_Tax = orderValue;
            using (DbTrans trans = Gateway.Default.BeginTrans())
            {
                try
                {
                    trans.Save<AddressSort>(current);
                    trans.Save<AddressSort>(down);
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
                finally
                {
                    trans.Close();
                }
            }
            return true;
        }
        #endregion
    }
}
