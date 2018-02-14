using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using WeiSha.Common;
using Song.Entities;
using WeiSha.Data;
using Song.ServiceInterfaces;

namespace Song.ServiceImpls
{
    /// <summary>
    /// 权限管理
    /// </summary>
    public class PurviewCom :IPurview
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Add(Purview entity)
        {
            Gateway.Default.Save<Purview>(entity);
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="memberId">成员id，即权限赋予对象的id</param>
        /// <param name="mmids">管理菜单的id</param>
        /// <param name="type">成员类型</param>
        public void AddBatch(int memberId, string mmids, string type)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    type = type.ToLower();
                    WhereClip wc = Purview._.Pur_Type == type;
                    //删除所有
                    switch (type)
                    {
                        case "posi":
                            tran.Delete<Purview>(wc && Purview._.Posi_Id == memberId);
                            break;
                        case "group":
                            tran.Delete<Purview>(wc && Purview._.EGrp_Id == memberId);
                            break;
                        case "depart":
                            tran.Delete<Purview>(wc && Purview._.Dep_Id == memberId);
                            break;
                        case "organ":
                            tran.Delete<Purview>(wc && Purview._.Org_ID == memberId);
                            break;
                        case "orglevel":
                            tran.Delete<Purview>(wc && Purview._.Olv_ID == memberId);
                            break;
                    }
                    foreach (string node in mmids.Split(','))
                    {
                        if (node.Trim() == "") continue;
                        if (node.IndexOf("|") < 0) continue;
                        int id = Convert.ToInt32(node.Substring(0, node.IndexOf("|")));
                        string state = node.Substring(node.IndexOf("|") + 1);
                        Song.Entities.Purview p = new Song.Entities.Purview();
                        p.Pur_Type = type;
                        p.Pur_State = state;
                        p.MM_Id = Convert.ToInt32(id);
                        switch (type)
                        {
                            case "posi":
                                p.Posi_Id = memberId;
                                break;
                            case "group":
                                p.EGrp_Id = memberId;
                                break;
                            case "depart":
                                p.Dep_Id = memberId;
                                break;
                            case "organ":
                                p.Org_ID = memberId;
                                break;
                            case "orglevel":
                                p.Olv_ID = memberId;
                                break;
                        }
                        tran.Save<Purview>(p);
                    }
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
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Save(Purview entity)
        {
            Gateway.Default.Save<Purview>(entity);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Delete(Purview entity)
        {
            Gateway.Default.Delete<Purview>(entity);
        }
        /// <summary>
        /// 根据分类、对象id删除
        /// </summary>
        /// <param name="memberId">角色或院系、组的id</param>
        /// <param name="type">用于区分不同权限分配，"Posi"角色(岗位)、"Group"组、"Depart"院系</param>
        public void Delete(int memberId, string type)
        {
            type = type.ToLower();
            WhereClip wc = Purview._.Pur_Type == type;
            switch (type)
            {
                case "posi":
                    Gateway.Default.Delete<Purview>(wc && Purview._.Posi_Id == memberId);
                    break;
                case "group":
                    Gateway.Default.Delete<Purview>(wc && Purview._.EGrp_Id == memberId);
                    break;
                case "depart":
                    Gateway.Default.Delete<Purview>(wc && Purview._.Dep_Id == memberId);
                    break;
                case "organ":
                    Gateway.Default.Delete<Purview>(wc && Purview._.Org_ID == memberId);
                    break;
                case "orglevel":
                    Gateway.Default.Delete<Purview>(wc && Purview._.Olv_ID == memberId);
                    break;
            }
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void Delete(int identify)
        {
            Gateway.Default.Delete<Purview>(Purview._.Pur_Id == identify);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public Purview GetSingle(int identify)
        {
            return Gateway.Default.From<Purview>().Where(Purview._.Pur_Id == identify).ToFirst<Purview>();
        }
        /// <summary>
        /// 获取单一实体对象，按权限主题的id
        /// </summary>
        /// <param name="MMId">功能菜单的id</param>
        /// <param name="type">用于区分不同权限分配，"Posi"角色(岗位)、"Group"组、"Depart"院系</param>
        /// <returns></returns>
        public Purview GetSingle4Member(int MMId, string type)
        {
            type = type.ToLower();
            WhereClip wc = Purview._.Pur_Type == type;
            return Gateway.Default.From<Purview>().Where(wc && Purview._.MM_Id == MMId).ToFirst<Purview>(); ;
        }
        /// <summary>
        /// 获取所有对象，按权限主题的id
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="type">用于区分不同权限分配，"Posi"角色(岗位)、"Group"组、"Depart"院系</param>
        /// <returns></returns>
        public Purview[] GetAll(int memberId, string type)
        {
            type = type.ToLower();
            Song.Entities.Purview[] pur = null;
            WhereClip wc = Purview._.Pur_Type == type;
            switch (type)
            {
                case "posi":
                    pur = Gateway.Default.From<Purview>().Where(wc && Purview._.Posi_Id == memberId).ToArray<Purview>();
                    break;
                case "group":
                    pur = Gateway.Default.From<Purview>().Where(wc && Purview._.EGrp_Id == memberId).ToArray<Purview>();
                    break;
                case "depart":
                    pur = Gateway.Default.From<Purview>().Where(wc && Purview._.Dep_Id == memberId).ToArray<Purview>();
                    break;
                case "organ":
                    pur = Gateway.Default.From<Purview>().Where(wc && Purview._.Org_ID == memberId).ToArray<Purview>();
                    break;
                case "orglevel":
                    pur = Gateway.Default.From<Purview>().Where(wc && Purview._.Olv_ID == memberId).ToArray<Purview>();
                    if (pur.Length < 1)
                    {
                        pur = Gateway.Default.From<Purview>().Where(Purview._.Pur_Type == "organ" && Purview._.Org_ID == -1).ToArray<Purview>();
                    }
                    break;
            }
            return pur;
        }
        /// <summary>
        /// 获取某个员工所拥用的全部操作权限，包括所在组、所属角色、所在院系的所有权限
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public ManageMenu[] GetAll4Emplyee(int empId)
        {
            //以下代码，先取员工所在组的权限；
            //员工所属角色权限
            //员工所在院系的权限
            string sql = @"select * from 
                (select [mm].* from [ManageMenu] as mm inner join
                (SELECT [pu].* from [EmpGroup] as em inner join [Purview] as pu on ([em].EGrp_Id=[pu].EGrp_Id)
                where [pu].EGrp_Id in
                (Select [ea].EGrp_Id from [EmpAccount] as e inner join [EmpAcc_Group] as ea 
                on ([e].Acc_Id=[ea].Acc_Id)
                where [ea].Acc_Id=" + empId + @")  and [em].EGrp_IsUse=true
                ) as p 
                on ([mm].MM_Id=[p].MM_Id)

                UNION

                SELECT [mm].*
                FROM [ManageMenu] AS mm INNER JOIN 
                (SELECT [pu].* from [Position] as po inner join [Purview] as pu on ([po].Posi_Id=[pu].Posi_Id)
                where [pu].Posi_id = (Select [Posi_Id] from [EmpAccount] where [Acc_Id]=" + empId + @")  and [po].Posi_IsUse=true)  AS p ON ([mm].MM_Id=[p].MM_Id)

                UNION 

                SELECT [mm].*
                FROM [ManageMenu] AS mm INNER JOIN (SELECT [pu].* from [Depart] as d inner join [Purview] as pu on ([d].Dep_Id=[pu].Dep_Id)
                where [pu].Dep_Id =
                (Select [Dep_Id] from [EmpAccount] where [Acc_Id]=" + empId + @")  and [d].Dep_IsUse=true)  AS p ON ([mm].MM_Id=[p].MM_Id)
                ) as tm where MM_IsUse=true";
            //如果不是access，就是sqlserver
            if (WeiSha.Common.Server.DatabaseType != "access")
            {
                sql = sql.Replace("true","1");
            }
            return Gateway.Default.FromSql(sql).ToArray<ManageMenu>();
        }


        /// <summary>
        /// 某个机构的权限
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public ManageMenu[] GetAll4Org(int orgid)
        {
            //取当前机构等级
            object objid = Gateway.Default.Max<Organization>(Organization._.Olv_ID, 
                Organization._.Org_ID == orgid && Organization._.Org_IsUse==true && Organization._.Org_IsPass==true);
            int olv = objid is int ? (int)objid : 0;
            if (olv == 0) return null;
            //判断当前机构等级是否可用
            objid = Gateway.Default.Max<OrganLevel>(OrganLevel._.Olv_ID, OrganLevel._.Olv_IsUse == true && OrganLevel._.Olv_ID == olv);
            olv = objid is int ? (int)objid : 0;
            if (olv == 0) return null;
            //当前机构是否有权限
            int num = Gateway.Default.Count<Purview>(Purview._.Pur_Type == "orglevel" && Purview._.Olv_ID == olv);
            string sql = "";
            if (num < 1)
            {
                //如果当前机构等级没有设置权限，则返回基础权限
                return GetOrganPurview();
            }
            else
            {
                //获取当前机构等级的权限，与基础专权为交集
                sql = @"select m2.* from
                    (select [mm].* from [ManageMenu] as mm inner join [Purview] as pur on mm.mm_id=pur.mm_id
                    where pur.org_id={orgid}
                    UNION ALL
                    select [mm].* from [ManageMenu] as mm inner join [Purview] as pur on mm.mm_id=pur.mm_id
                    where pur.olv_id={olvid} ) as m2  where m2.mm_isuse=true order by m2.mm_tax asc";
                sql = sql.Replace("{orgid}", orgid.ToString());
                sql = sql.Replace("{olvid}", olv.ToString());
            }
            //如果不是access，就是sqlserver
            if (WeiSha.Common.Server.DatabaseType != "Access")
                sql = sql.Replace("true", "1");
            return Gateway.Default.FromSql(sql).ToArray<ManageMenu>();
        }
        /// <summary>
        /// 获取机构的某一个根菜单项的权限
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="marker">例如教师管理teacher,学生管理student,机构管理organAdmin</param>
        /// <returns></returns>
        public ManageMenu[] GetAll4Org(int orgid, string marker)
        {
            if (string.IsNullOrWhiteSpace(marker)) return this.GetAll4Org(orgid);
            //取当前机构等级
            object objid = Gateway.Default.Max<Organization>(Organization._.Olv_ID,
                Organization._.Org_ID == orgid && Organization._.Org_IsUse == true && Organization._.Org_IsPass == true);
            int olv = objid is int ? (int)objid : 0;
            if (olv == 0) return null;
            //判断当前机构等级是否可用
            objid = Gateway.Default.Max<OrganLevel>(OrganLevel._.Olv_ID, OrganLevel._.Olv_IsUse == true && OrganLevel._.Olv_ID == olv);
            olv = objid is int ? (int)objid : 0;
            if (olv == 0) return null;
            //当前机构是否有权限
            int num = Gateway.Default.Count<Purview>(Purview._.Pur_Type == "orglevel" && Purview._.Olv_ID == olv);
            string sql = "";
            if (num < 1)
            {
                //如果当前机构等级没有设置权限，则返回基础权限
                return GetOrganPurview(marker);
            }
            else
            {
                //根菜单项
                ManageMenu root = Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_Marker == marker && ManageMenu._.MM_PatId == 0).ToFirst<ManageMenu>();
                int rootid = root == null ? 0 : (int)root.MM_Root;
                //获取当前机构等级的权限，与基础专权为交集
                sql = @"select m2.* from
                    (select [mm].* from [ManageMenu] as mm inner join [Purview] as pur on mm.mm_id=pur.mm_id
                    where pur.org_id={orgid}
                    UNION ALL
                    select [mm].* from [ManageMenu] as mm inner join [Purview] as pur on mm.mm_id=pur.mm_id
                    where pur.olv_id={olvid}) as m2
                   where m2.mm_isuse=true and m2.mm_root={root} order by m2.mm_tax asc";
                sql = sql.Replace("{orgid}", orgid.ToString());
                sql = sql.Replace("{olvid}", olv.ToString());
                sql = sql.Replace("{root}", rootid.ToString());
            }
            //如果不是access，就是sqlserver
            if (WeiSha.Common.Server.DatabaseType != "Access")
                sql = sql.Replace("true", "1");
            return Gateway.Default.FromSql(sql).ToArray<ManageMenu>();
        }
        /// <summary>
        /// 获取机构的基础权限，如果不设置机构所在等级的所权，则获取此权限
        /// </summary>
        /// <returns></returns>
        public ManageMenu[] GetOrganPurview()
        {
            //获取当前机构的权限
            //先取所有机构权限，再取等级权限，二者为并集；
            string sql = @"select * from 
                         (select [mm].* from [ManageMenu] as mm inner join [Purview] as pur on mm.mm_id=pur.mm_id
                         where pur.pur_type='organ' and mm.mm_isuse=1) as mm
                         order by mm_tax asc";
            //如果不是access，就是sqlserver
            if (WeiSha.Common.Server.DatabaseType != "access")
                sql = sql.Replace("true", "1");
            return Gateway.Default.FromSql(sql).ToArray<ManageMenu>();
        }
        /// <summary>
        /// 获取机构的某一个根菜单项的权限
        /// </summary>
        /// <param name="marker">例如教师管理teacher,学生管理student,机构管理organAdmin</param>
        /// <returns></returns>
        public ManageMenu[] GetOrganPurview(string marker)
        {
            //根菜单项
            ManageMenu root = Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_Marker == marker && ManageMenu._.MM_PatId == 0).ToFirst<ManageMenu>();
            int rootid = root == null ? 0 : (int)root.MM_Root;
            //获取当前机构的权限
            //先取所有机构权限，再取等级权限，二者为并集；
            string sql = @"select * from 
                         (select [mm].* from [ManageMenu] as mm inner join [Purview] as pur on mm.mm_id=pur.mm_id
                         where pur.pur_type='organ' and mm.mm_isuse=1 ) as mm where mm.mm_root={root}
                         order by mm_tax asc";
            sql = sql.Replace("{root}", rootid.ToString());
            //如果不是access，就是sqlserver
            if (WeiSha.Common.Server.DatabaseType != "access")
                sql = sql.Replace("true", "1");
            return Gateway.Default.FromSql(sql).ToArray<ManageMenu>();
        }
    }
}
