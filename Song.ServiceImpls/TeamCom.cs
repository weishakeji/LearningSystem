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
    public class TeamCom : ITeam
    {

        #region ITeam 成员

        public int TeamAdd(Team entity)
        {
            entity.Team_CrtTime = DateTime.Now;
            //如果没有排序号，则自动计算
            if (entity.Team_Tax < 1)
            {
                object obj = Gateway.Default.Max<Team>( Team._.Team_Tax,Team._.Team_ID > -1);
                entity.Team_Tax = obj is int ? (int)obj + 1 : 0;
            }
            Song.Entities.Depart dep = Gateway.Default.From<Depart>().Where(Depart._.Dep_Id == entity.Dep_ID).ToFirst<Depart>();
            if (dep != null) entity.Dep_Name = dep.Dep_CnName;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            return Gateway.Default.Save<Team>(entity);
        }

        public void TeamSave(Team entity)
        {
            //
            Song.Entities.Depart dep = Gateway.Default.From<Depart>().Where(Depart._.Dep_Id == entity.Dep_ID).ToFirst<Depart>();
            if (dep != null) entity.Dep_Name = dep.Dep_CnName;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Team>(entity);
                    tran.Update<EmpAccount>(new Field[] { EmpAccount._.Team_Name }, new object[] { entity.Team_Name }, EmpAccount._.Team_ID == entity.Team_ID);
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

        public void TeamDelete(int identify)
        {
            Gateway.Default.Delete<Team>(Team._.Team_ID == identify);
        }

        public Team TeamSingle(int identify)
        {
            return Gateway.Default.From<Team>().Where(Team._.Team_ID == identify).ToFirst<Team>();
        }

        public Team[] GetTeam(bool? isUse, int count)
        {
            WhereClip wc = Team._.Team_ID > -1;
            if (isUse != null) wc.And(Team._.Team_IsUse == (bool)isUse);
            count = count > 0 ? count : int.MaxValue;
            return Gateway.Default.From<Team>().Where(wc).OrderBy(Team._.Team_Tax.Asc).ToArray<Team>(count);
        }
        public Team[] GetTeam(bool? isUse, int? depid, int count)
        {
            WhereClip wc = Team._.Team_ID > -1;
            if (isUse != null) wc.And(Team._.Team_IsUse == (bool)isUse);
            if (depid != null) wc.And(Team._.Dep_ID == (int)depid);
            count = count > 0 ? count : int.MaxValue;
            return Gateway.Default.From<Team>().Where(wc).OrderBy(Team._.Team_Tax.Asc).ToArray<Team>(count);
        }
        public Team[] GetTeamPager(bool? isUse, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = Team._.Team_ID > -1;
            if (isUse != null) wc.And(Team._.Team_IsUse == isUse);
            if (searTxt != string.Empty) wc.And(Team._.Team_Name.Like("%" + searTxt + "%"));
            countSum = Gateway.Default.Count<Team>(wc);
            return Gateway.Default.From<Team>().Where(wc).OrderBy(Team._.Team_Tax.Asc).ToArray<Team>(size, (index - 1) * size);
        }

        public Team[] GetTeamPager(int depid, bool? isUse, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = Team._.Team_ID > -1;
            if (depid > 0) wc.And(Team._.Dep_ID == depid);
            if (isUse != null) wc.And(Team._.Team_IsUse == isUse);
            if (searTxt != string.Empty) wc.And(Team._.Team_Name.Like("%" + searTxt + "%"));
            countSum = Gateway.Default.Count<Team>(wc);
            return Gateway.Default.From<Team>().Where(wc).OrderBy(Team._.Team_Tax.Asc).ToArray<Team>(size, (index - 1) * size);
        }

        public bool RemoveUp(int id)
        {
            //当前对象
            Team current = Gateway.Default.From<Team>().Where(Team._.Team_ID == id).ToFirst<Team>();
            //当前对象排序号
            int orderValue = (int)current.Team_Tax;
            //上一个对象，即兄长对象；
            Team up = Gateway.Default.From<Team>().Where(Team._.Team_Tax > orderValue).OrderBy(Team._.Team_Tax.Asc).ToFirst<Team>();
            if (up == null)
            {
                //如果兄长对象不存在，则表示当前节点在兄弟中是老大；即是最顶点；
                return false;
            }
            //交换排序号
            current.Team_Tax = up.Team_Tax;
            up.Team_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Team>(current);
                    tran.Save<Team>(up);
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
            Team current = Gateway.Default.From<Team>().Where(Team._.Team_ID == id).ToFirst<Team>();
            //当前对象排序号
            int orderValue = (int)current.Team_Tax;
            //下一个对象，即弟弟对象；
            Team down = Gateway.Default.From<Team>().Where(Team._.Team_Tax < orderValue).OrderBy(Team._.Team_Tax.Desc).ToFirst<Team>();
            if (down == null)
            {
                //如果弟对象不存在，则表示当前节点在兄弟中是老幺；即是最底端；
                return false;
            }
            //交换排序号
            current.Team_Tax = down.Team_Tax;
            down.Team_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Team>(current);
                    tran.Save<Team>(down);
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
        #endregion
    }
}
