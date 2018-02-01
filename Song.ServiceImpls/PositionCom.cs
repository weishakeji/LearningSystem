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
    public class PositionCom :IPosition
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Add(Position entity)
        {
            //添加对象，并设置排序号
            object obj = Gateway.Default.Max<Position>(Position._.Posi_Tax, Position._.Posi_Tax > -1 && Position._.Org_ID == entity.Org_ID);
            entity.Posi_Tax = obj is int ? (int)obj + 1 : 1;

            Gateway.Default.Save<Position>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Save(Position entity)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Position>(entity);
                    tran.Update<EmpAccount>(new Field[] { EmpAccount._.Posi_Name },
                        new object[] { entity.Posi_Name }, EmpAccount._.Posi_Id == entity.Posi_Id);
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
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Delete(Position entity)
        {
            if (entity.Posi_IsAdmin) return;
            //删除权限关联
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Delete<Purview>(Purview._.Posi_Id == entity.Posi_Id);
                    //修改员工信息中的岗位名称
                    tran.Update<EmpAccount>(new Field[] { EmpAccount._.Posi_Name }, new object[] { "" }, EmpAccount._.Posi_Id == entity.Posi_Id);
                    tran.Delete<Position>(entity);
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
            Song.Entities.Position entity = this.GetSingle(identify);
            this.Delete(entity);            
        }
        /// <summary>
        /// 删除，按职位名称
        /// </summary>
        /// <param name="name">职位名称</param>
        public void Delete(int orgid, string name)
        {
            Song.Entities.Position entity = this.GetSingle(orgid,name);
            this.Delete(entity); 
        }
        /// <summary>
        /// 删除与员工之间的关联
        /// </summary>
        /// <param name="identify"></param>
        public void DeleteRelation4Emp(int identify)
        {
            Gateway.Default.Update<EmpAccount>(new Field[] { EmpAccount._.Posi_Id,EmpAccount._.Posi_Name}, new object[] { -1,"" }, EmpAccount._.Posi_Id == identify);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public Position GetSingle(int identify)
        {
            return Gateway.Default.From<Position>().Where(Position._.Posi_Id==identify).ToFirst<Position>();
        }
        /// <summary>
        /// 获取单一实体对象，按职位名称
        /// </summary>
        /// <param name="name">职位名称</param>
        /// <returns></returns>
        public Position GetSingle(int orgid, string name)
        {

            return Gateway.Default.From<Position>().Where(Organization._.Org_ID == orgid && Position._.Posi_Name == name).ToFirst<Position>();
        }
        /// <summary>
        /// 获取超级管理员角色
        /// </summary>
        /// <returns></returns>
        public Position GetSuper()
        {
            Song.Entities.Organization org = Gateway.Default.From<Organization>().Where(Organization._.Org_IsRoot == true).ToFirst<Organization>();
            if (org == null) return null;
            return Gateway.Default.From<Position>().Where(Position._.Org_ID == org.Org_ID && Position._.Posi_IsAdmin == true).ToFirst<Position>();
        }
        /// <summary>
        /// 获取对象；即所有职位；
        /// </summary>
        /// <returns></returns>
        public Position[] GetAll(int orgid)
        {
            return Gateway.Default.From<Position>().Where(Position._.Org_ID == orgid).OrderBy(Position._.Posi_Tax.Asc).ToArray<Position>();
        }
        public Position[] GetAll(int orgid,bool? isUse)
        {
            if (isUse == null)
            {
                return this.GetAll(orgid);
            }
            return Gateway.Default.From<Position>()
                .Where(Position._.Org_ID == orgid && Position._.Posi_IsUse == isUse)
                .OrderBy(Position._.Posi_Tax.Asc).ToArray<Position>();
        }
        /// <summary>
        /// 获取当前角色的所有员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EmpAccount[] GetAllEmplyee(int id)
        {
            return Gateway.Default.From<EmpAccount>().Where(EmpAccount._.Posi_Id == id).ToArray<EmpAccount>();
        }
        /// <summary>
        /// 获取当前角色的所有在职员工
        /// </summary>
        /// <param name="posid"></param>
        /// <param name="use">是否在职</param>
        /// <returns></returns>
        public EmpAccount[] GetAllEmplyee(int posid, bool use)
        {
            return Gateway.Default.From<EmpAccount>().Where(EmpAccount._.Posi_Id == posid && EmpAccount._.Acc_IsUse == use).ToArray<EmpAccount>();
        }

        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns>重名返回true，否则返回false</returns>
        public bool IsExist(int orgid,Position entity)
        {
            Position mm = null;
            //如果是一个新对象
            if (entity.Posi_Id == 0)
            {
                mm = Gateway.Default.From<Position>()
                    .Where(Position._.Org_ID == orgid && Position._.Posi_Name == entity.Posi_Name)
                    .ToFirst<Position>();
            }
            else
            {
                //如果是一个已经存在的对象，则不匹配自己
                mm = Gateway.Default.From<Position>()
                    .Where(Position._.Org_ID == orgid && Position._.Posi_Name == entity.Posi_Name && Position._.Posi_Id != entity.Posi_Id)
                    .ToFirst<Position>();
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
            Position current = Gateway.Default.From<Position>().Where(Position._.Posi_Id == id).ToFirst<Position>();
            //当前对象排序号
            int orderValue = (int)current.Posi_Tax; ;
            //上一个对象，即兄长对象；
            Position up = Gateway.Default.From<Position>().Where(Position._.Org_ID == orgid && Position._.Posi_Tax < orderValue)
                .OrderBy(Position._.Posi_Tax.Desc).ToFirst<Position>();
            if (up == null) return false;
            //交换排序号
            current.Posi_Tax = up.Posi_Tax;
            up.Posi_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Position>(current);
                    tran.Save<Position>(up);
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
            Position current = Gateway.Default.From<Position>().Where(Position._.Posi_Id == id).ToFirst<Position>();
               //当前对象排序号
            int orderValue = (int)current.Posi_Tax;
            //下一个对象，即弟弟对象；
            Position down = Gateway.Default.From<Position>().Where(Position._.Org_ID == orgid && Position._.Posi_Tax > orderValue)
                .OrderBy(Position._.Posi_Tax.Asc).ToFirst<Position>();
            if (down == null) return false;
            //交换排序号
            current.Posi_Tax = down.Posi_Tax;
            down.Posi_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Position>(current);
                    tran.Save<Position>(down);
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



        public Position GetAdmin(int orgid)
        {
            Song.Entities.Organization org = Gateway.Default.From<Organization>().Where(Organization._.Org_ID == orgid).ToFirst<Organization>();
            if (org == null) throw new WeiSha.Common.ExceptionForWarning("ID为" + orgid + "的机构不存在！");
            Position pos = Gateway.Default.From<Position>().Where(Position._.Posi_IsAdmin == true && Position._.Org_ID == orgid).ToFirst<Position>();
            if (pos == null)
            {
                pos = new Position();
                pos.Posi_IsAdmin = true;
                pos.Posi_IsUse = true;
                pos.Posi_Name = "管理员";
                pos.Org_Name = org.Org_Name;
                pos.Org_ID = orgid;
                Gateway.Default.Save<Position>(pos);
            }
            pos = Gateway.Default.From<Position>().Where(Position._.Posi_IsAdmin == true && Position._.Org_ID == orgid).ToFirst<Position>();
            return pos;
        }
    }
}
