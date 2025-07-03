using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using Song.ViewData.Attri;
using WeiSha.Core;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 岗位（或叫角色）管理
    /// </summary>
    [HttpPut, HttpGet,HttpPost]
    public class Position : ViewMethod,IViewAPI
    {
        /// <summary>
        /// 当前登录账号所在的机构的所有岗位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Song.Entities.Position[] All()
        {
            Song.Entities.Organization org = LoginAdmin.Status.Organ(this.Letter);
            if (org == null) return null;
            return  Business.Do<IPosition>().GetAll(org.Org_ID);
        }
        /// <summary>
        /// 机构的所有岗位
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        [HttpGet]
        public Song.Entities.Position[] All4Organ(int orgid)
        {           
            return Business.Do<IPosition>().GetAll(orgid);
        }
        /// <summary>
        /// 所有启用的岗位
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Song.Entities.Position[] EnableAll()
        {
            Song.Entities.Organization org = LoginAdmin.Status.Organ(this.Letter);
            return Business.Do<IPosition>().GetAll(org.Org_ID, true);
        }
        /// <summary>
        /// 机构的所有启用的岗位
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        [HttpGet]
        public Song.Entities.Position[] Enable4Organ(int orgid)
        {
            return Business.Do<IPosition>().GetAll(orgid, true);
        }
        /// <summary>
        /// 获取岗位信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public Song.Entities.Position ForID(int id)
        {
            return Business.Do<IPosition>().GetSingle(id);
        }
        /// <summary>
        /// 修改岗位
        /// </summary>
        /// <param name="posi"></param>
        /// <returns></returns>
        [Admin]
        public bool Modify(Song.Entities.Position posi)
        {
            Song.Entities.Position old = Business.Do<IPosition>().GetSingle(posi.Posi_Id);
            if (old == null) throw new Exception("对象不存在！");

            old.Copy<Song.Entities.Position>(posi);
            Business.Do<IPosition>().Save(old);
            return true;
        }
        /// <summary>
        /// 删除岗位信息
        /// </summary>
        /// <param name="id">岗位id，可以是多个，用逗号分隔</param>
        /// <returns>返回删除的个数</returns>
        [HttpPost(Ignore =true),HttpDelete,Admin]
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
                    Business.Do<IPosition>().Delete(idval);
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
        /// 岗位名称是否已经存在
        /// </summary>
        /// <param name="name">岗位名称</param>
        /// <param name="id">岗位id</param>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        public bool Exist(string name,int id,int orgid)
        {
            return Business.Do<IPosition>().IsExist(name,id,orgid);
        }
        /// <summary>
        /// 添加岗位
        /// </summary>
        /// <param name="posi"></param>
        /// <returns></returns>
        [Admin]
        public bool Add(Song.Entities.Position posi)
        {
            try
            {
                posi.Posi_IsAdmin = false;
                //posi.Org_ID = 0;
                Business.Do<IPosition>().Add(posi);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 添加管理岗位
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="name">管理岗位的名称</param>
        /// <returns></returns>
        [SuperAdmin,HttpPost]
        public bool AddAdminPosi(int orgid, string name)
        {
            try
            {
                Song.Entities.Position posi = new Entities.Position();
                posi.Posi_IsAdmin = true;
                posi.Posi_IsUse = true;
                posi.Posi_Name = string.IsNullOrWhiteSpace(name) ? "管理员" : name;
                posi.Org_ID = orgid;
                Business.Do<IPosition>().Add(posi);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 更改职务的排序
        /// </summary>
        /// <param name="items">职务信息数组</param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public bool UpdateTaxis(Song.Entities.Position[] items)
        {
            try
            {
                Business.Do<IPosition>().UpdateTaxis(items);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 当前岗位下的所有员工
        /// </summary>
        /// <param name="id">岗位id</param>
        /// <returns></returns>
        [HttpGet]
        public Song.Entities.EmpAccount[] Emplyees(int id)
        {
            return Business.Do<IPosition>().GetAllEmplyee(id);
        }
        /// <summary>
        /// 保存员工与岗位的关联
        /// </summary>
        /// <param name="datas">员工的id(Acc_Id)</param>
        /// <param name="posid"></param>
        /// <returns>返回员工与岗位的关联个数</returns>
        [HttpPost,Admin]
        public int UpdateEmp4Posi(int[] datas,int posid)
        {
            if (datas == null) return -1;
            //当前岗位
            Song.Entities.Position posi = Business.Do<IPosition>().GetSingle(posid);
            if (posi == null) throw new Exception("Not found entity for Position！");
            if (posi.Posi_IsAdmin && datas.Length < 1)
                throw new Exception("当前岗位为机构的管理岗，必须有至少一名管理员");
            //删除所有员工与岗位的关联
            Business.Do<IPosition>().DeleteRelation4Emp(posid);
            if (datas.Length < 1) return 0;
            //重建关联 
            foreach(int accid in datas)           
                Business.Do<IEmployee>().Update(accid, new WeiSha.Data.Field[] { EmpAccount._.Posi_Id, EmpAccount._.Posi_Name },
                      new object[] { posid, posi.Posi_Name });          
            return datas.Length;
        }       
    }
}
