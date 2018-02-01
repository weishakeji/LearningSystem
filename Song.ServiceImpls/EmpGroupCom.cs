using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;



namespace Song.ServiceImpls
{
    public class EmpGroupCom :IEmpGroup
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Add(EmpGroup entity)
        {
            //添加对象，并设置排序号
            object obj = Gateway.Default.Max<EmpGroup>(EmpGroup._.EGrp_Tax, EmpGroup._.EGrp_Tax > -1 && EmpGroup._.Org_ID == entity.Org_ID);
            entity.EGrp_Tax = obj is int ? (int)obj+1 : 1;
            Gateway.Default.Save<EmpGroup>(entity);
        }
        /// <summary>
        /// 增加员工与组的关联
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="grpId"></param>
        public void AddRelation(int empId, int grpId)
        {
            Song.Entities.EmpAcc_Group eg = new EmpAcc_Group();
            eg.Acc_Id = empId;
            eg.EGrp_Id = grpId;
            Gateway.Default.Save<EmpAcc_Group>(eg);
        }
        /// <summary>
        /// 根据员工Id,删除关联
        /// </summary>
        /// <param name="empId"></param>
        public void DelRelation4Emplyee(int empId)
        {
            Gateway.Default.Delete<EmpAcc_Group>(EmpAcc_Group._.Acc_Id == empId);
        }
        /// <summary>
        /// 根据组Id,删除关联
        /// </summary>
        /// <param name="grpId"></param>
        public void DelRelation4Group(int grpId)
        {
            Gateway.Default.Delete<EmpAcc_Group>(EmpAcc_Group._.EGrp_Id == grpId);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Save(EmpGroup entity)
        {
            Gateway.Default.Save<EmpGroup>(entity);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Delete(EmpGroup entity)
        {
            if (entity == null) return;
            //如果是系统组，则不允许删除
            if (entity.EGrp_IsSystem) return;

            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Delete<EmpAcc_Group>(EmpAcc_Group._.EGrp_Id == entity.EGrp_Id);
                    tran.Delete<Purview>(Purview._.EGrp_Id == entity.EGrp_Id);
                    tran.Delete<EmpGroup>(entity);
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
            EmpGroup entity = this.GetSingle(identify);
            this.Delete(entity);                  
        }        
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public EmpGroup GetSingle(int identify)
        {
            EmpGroup group= Gateway.Default.From<EmpGroup>().Where(EmpGroup._.EGrp_Id==identify).ToFirst<EmpGroup>();
            return group;
        }
        /// <summary>
        /// 获取对象；即所有组；
        /// </summary>
        /// <returns></returns>
        public EmpGroup[] GetAll(int orgid)
        {
            return Gateway.Default.From<EmpGroup>().Where(EmpGroup._.Org_ID==orgid).OrderBy(EmpGroup._.EGrp_Tax.Asc).ToArray<EmpGroup>();
        }
        public EmpGroup[] GetAll(int orgid, bool? isUse)
        {
            if (isUse == null)
            {
                return this.GetAll(orgid);
            }
            return Gateway.Default.From<EmpGroup>()
                .Where(EmpGroup._.Org_ID == orgid && EmpGroup._.EGrp_IsUse == isUse)
                .OrderBy(EmpGroup._.EGrp_Tax.Asc).ToArray<EmpGroup>();
        }
        /// <summary>
        /// 获取某员工所属的所有组；
        /// </summary>
        /// <param name="EmpAccountId">员工id</param>
        /// <returns></returns>
        public EmpGroup[] GetAll4Emp(int EmpAccountId)
        {
            return Gateway.Default.From<EmpGroup>().InnerJoin<EmpAcc_Group>(EmpGroup._.EGrp_Id == EmpAcc_Group._.EGrp_Id)
                .Where(EmpAcc_Group._.Acc_Id == EmpAccountId)
                .OrderBy(EmpGroup._.EGrp_Tax.Asc).ToArray<EmpGroup>();
        }
        /// <summary>
        /// 获取某个组的所有员工
        /// </summary>
        /// <param name="grpId">组id</param>
        /// <returns></returns>
        public EmpAccount[] GetAll4Group(int grpId)
        {
            return Gateway.Default.From<EmpAccount>().InnerJoin<EmpAcc_Group>(EmpAcc_Group._.Acc_Id == EmpAccount._.Acc_Id)
                .Where(EmpAcc_Group._.EGrp_Id == grpId)
                .OrderBy(EmpAccount._.Acc_RegTime.Asc).ToArray<EmpAccount>();
         }
          /// <summary>
          /// 获取某个组的所有在职员工
          /// </summary>
          /// <param name="grpId"></param>
          /// <param name="use"></param>
          /// <returns></returns>
        public EmpAccount[] GetAll4Group(int grpId, bool use)
        {
            return Gateway.Default.From<EmpAccount>().InnerJoin<EmpAcc_Group>(EmpAcc_Group._.Acc_Id == EmpAccount._.Acc_Id)
                .Where(EmpAcc_Group._.EGrp_Id == grpId && EmpAccount._.Acc_IsUse == use)
                .OrderBy(EmpAccount._.Acc_RegTime.Asc).ToArray<EmpAccount>();
        }
        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns>重名返回true，否则返回false</returns>
        public bool IsExist(EmpGroup entity)
        {
            EmpGroup mm = null;
            //如果是一个新对象
            if (entity.EGrp_Id == 0)
            {
                mm = Gateway.Default.From<EmpGroup>().Where(EmpGroup._.EGrp_Name == entity.EGrp_Name).ToFirst<EmpGroup>();
            }
            else
            {
                //如果是一个已经存在的对象，则不匹配自己
                mm = Gateway.Default.From<EmpGroup>().Where(EmpGroup._.EGrp_Name == entity.EGrp_Name && EmpGroup._.EGrp_Id != entity.EGrp_Id).ToFirst<EmpGroup>();
            }
            return mm != null;
        }
        /// <summary>
        /// 将当前栏目向上移动；仅在当前对象的同层移动，即同一父节点下的对象之间移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool RemoveUp(int orgid,int id)
        {
            //当前对象
            EmpGroup current = Gateway.Default.From<EmpGroup>().Where(EmpGroup._.EGrp_Id == id).ToFirst<EmpGroup>();
            //当前对象排序号
            int orderValue = (int)current.EGrp_Tax; ;
            //上一个对象，即兄长对象；
            EmpGroup up = Gateway.Default.From<EmpGroup>()
                .Where(EmpGroup._.Org_ID == orgid && EmpGroup._.EGrp_Tax < orderValue)
                .OrderBy(EmpGroup._.EGrp_Tax.Desc).ToFirst<EmpGroup>();
            if (up == null) return false;
            //交换排序号
            current.EGrp_Tax = up.EGrp_Tax;
            up.EGrp_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<EmpGroup>(current);
                    tran.Save<EmpGroup>(up);
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
        public bool RemoveDown(int orgid,int id)
        {
            //当前对象
            EmpGroup current = Gateway.Default.From<EmpGroup>().Where(EmpGroup._.EGrp_Id == id).ToFirst<EmpGroup>();
               //当前对象排序号
            int orderValue = (int)current.EGrp_Tax;
            //下一个对象，即弟弟对象；
            EmpGroup down = Gateway.Default.From<EmpGroup>()
                .Where(EmpGroup._.Org_ID == orgid && EmpGroup._.EGrp_Tax > orderValue)
                .OrderBy(EmpGroup._.EGrp_Tax.Asc).ToFirst<EmpGroup>();
            if (down == null) return false;
            //交换排序号
            current.EGrp_Tax = down.EGrp_Tax;
            down.EGrp_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<EmpGroup>(current);
                    tran.Save<EmpGroup>(down);
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
        
    }
}
