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
    public class TaskCom : ITask
    {
        public int Add(Task entity)
        {
            //添加对象，并设置排序号
            object obj = Gateway.Default.Max<Task>(Task._.Task_Tax, Task._.Task_Tax > -1);
            int tax = 0;
            if (obj is int)
            {
                tax = (int)obj;
            }
            entity.Task_Tax = tax + 1;
            entity.Task_State = "2";
            entity = setEmployeeValue(entity);
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            return Gateway.Default.Save<Task>(entity);
        }

        public void Save(Task entity)
        {
            entity = setEmployeeValue(entity);
            //还未关闭的
            if (entity.Task_IsUse)
            {
                if (entity.Task_IsComplete)
                {
                    entity.Task_State = "1";
                    entity.Task_CompletePer = 100;
                }
                else
                {
                    //预期未安成，计划时间超过现在时间
                    if (entity.Task_EndTime < DateTime.Now)
                    {
                        entity.Task_State = "3";
                    }
                    else
                    {
                        //还在计划时间内
                        entity.Task_State = "2";
                    }
                }
            }
            else
            {
                //已经关闭
                entity.Task_State = "5";
            }
            Gateway.Default.Save<Task>(entity);
        }
        /// <summary>
        /// 设置任务中的各种值，如员工名称、院系名称等
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Song.Entities.Task setEmployeeValue(Task entity)
        {
            //设置承接任务的员工名称
            Song.Entities.EmpAccount ea = Gateway.Default.From<EmpAccount>().Where(EmpAccount._.Acc_Id == entity.Task_WorkerId).ToFirst<EmpAccount>();
            if (ea != null) entity.Task_WorkerName = ea.Acc_Name;
            return entity;
        }
        public void Delete(Task entity)
        {
            Gateway.Default.Delete<Task>(entity);
        }

        public void Delete(int identify)
        {
            Gateway.Default.Delete<Task>(Task._.Task_Id==identify);
        }

        public Task GetSingle(int identify)
        {
            return Gateway.Default.From<Task>().Where(Task._.Task_Id == identify).ToFirst<Task>();
        }

        public bool RemoveUp(int id)
        {
            Song.Entities.Task current = this.GetSingle(id);
            //当前对象排序号
            int orderValue = (int)current.Task_Tax; 
            //上一个对象，即兄长对象；存在当前优先级
            Song.Entities.Task up = Gateway.Default.From<Task>().Where(Task._.Task_Tax > orderValue && Task._.Task_Level == current.Task_Level).OrderBy(Task._.Task_Tax.Asc).ToFirst<Task>();
            if (up == null)
            {
                //如果兄长对象不存在，则表示当前节点在兄弟中是老大；即是最顶点；
                return false;
            }
            //交换排序号
            current.Task_Tax = up.Task_Tax;
            up.Task_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Task>(current);
                    tran.Save<Task>(up);
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
            Song.Entities.Task current = this.GetSingle(id);
            //当前对象排序号
            int orderValue = (int)current.Task_Tax; 
            //下一个对象，即弟弟对象；
            Song.Entities.Task down = Gateway.Default.From<Task>().Where(Task._.Task_Tax <orderValue && Task._.Task_Level == current.Task_Level).OrderBy(Task._.Task_Tax.Desc).ToFirst<Task>();
            if (down == null)
            {
                //如果弟对象不存在，则表示当前节点在兄弟中是老幺；即是最底端；
                return false;
            }
            //交换排序号
            current.Task_Tax = down.Task_Tax;
            down.Task_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Task>(current);
                    tran.Save<Task>(down);
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

        public Task[] GetPager(int level,int size, int index, out int countSum)
        {
            WhereClip wc = Task._.Task_Level == level;
            countSum = Gateway.Default.Count<Task>(wc);
            return Gateway.Default.From<Task>().Where(wc).OrderBy(Task._.Task_Tax.Desc).ToArray<Task>(size, (index - 1) * size);
        }
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="accId">员工</param>
        /// <param name="isGoback">是否是退回的任务</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="state">任务的状态，1完成，2未完成，3逾期未完成，4正在处理，5关闭</param>
        /// <param name="level">任务的优先级</param>
        /// <param name="searStr">检索字符</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Task[] GetMyPager(int accId, bool isGoback, DateTime start, DateTime end, string state, int level, string searStr, int size, int index, out int countSum)
        {
            WhereClip wc = Task._.Acc_Id == accId;
            wc.And(Task._.Task_IsGoback == isGoback);
            if (level > -1)
            {
                wc.And(Task._.Task_Level == level);
            }
            if (searStr != "" && searStr != String.Empty)
            {
                wc.And(Task._.Task_Name.Like("%" + searStr.Trim() + "%"));
            }
            switch (Convert.ToInt16(state))
            {
                case 1:
                    //如果为完成
                    wc.And(Task._.Task_IsComplete == true);
                    wc.And(Task._.Task_IsUse == true);
                    break;
                case 2:
                    //如果为未完成
                    wc.And(Task._.Task_IsComplete == false);
                    wc.And(Task._.Task_IsUse == true);
                    break;
                case 3:
                    //如果为逾期未完成
                    wc.And(Task._.Task_IsComplete == false);
                    wc.And(Task._.Task_EndTime < DateTime.Now);
                    wc.And(Task._.Task_IsUse == true);
                    break;
                case 4:
                    //如果为正在处理
                    wc.And(Task._.Task_IsComplete == false);
                    wc.And(Task._.Task_EndTime > DateTime.Now && Task._.Task_StartTime > DateTime.Now);
                    wc.And(Task._.Task_IsUse == true);
                    break;
                case 5:
                    //任务关闭
                    wc.And(Task._.Task_IsUse == false);
                    break;
                default:
                    break;
            }
            //wc.And(Task._.Task_State == state);
            wc.And(Task._.Task_EndTime > start && Task._.Task_EndTime <= end);    
            countSum = Gateway.Default.Count<Task>(wc);
            return Gateway.Default.From<Task>().Where(wc).OrderBy(Task._.Task_Tax.Desc).ToArray<Task>(size, (index - 1) * size);
        }
        /// <summary>
        /// 自己承接的任务
        /// </summary>
        /// <param name="accId"></param>
        /// <param name="isGoback"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="state"></param>
        /// <param name="level"></param>
        /// <param name="searStr"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Task[] GetWorkerPager(int accId, bool isGoback, DateTime start, DateTime end, string state, int level, string searStr, int size, int index, out int countSum)
        {
            WhereClip wc = Task._.Task_WorkerId == accId;
            wc.And(Task._.Task_IsGoback == isGoback);
            if (level > -1)
            {
                wc.And(Task._.Task_Level == level);
            }
            if (searStr != "" && searStr != String.Empty)
            {
                wc.And(Task._.Task_Name.Like("%" + searStr.Trim() + "%"));
            }
            switch (Convert.ToInt16(state))
            {
                case 1:
                    //如果为完成
                    wc.And(Task._.Task_IsComplete == true);
                    wc.And(Task._.Task_IsUse == true);
                    break;
                case 2:
                    //如果为未完成
                    wc.And(Task._.Task_IsComplete == false);
                    wc.And(Task._.Task_IsUse == true);
                    break;
                case 3:
                    //如果为逾期未完成
                    wc.And(Task._.Task_IsComplete == false);
                    wc.And(Task._.Task_EndTime < DateTime.Now);
                    wc.And(Task._.Task_IsUse == true);
                    break;
                case 4:
                    //如果为正在处理
                    wc.And(Task._.Task_IsComplete == false);
                    wc.And(Task._.Task_EndTime > DateTime.Now && Task._.Task_StartTime > DateTime.Now);
                    wc.And(Task._.Task_IsUse == true);
                    break;
                case 5:
                    //任务关闭
                    wc.And(Task._.Task_IsUse == false);
                    break;
                default:
                    break;
            }
            //wc.And(Task._.Task_State == state);
            wc.And(Task._.Task_EndTime > start && Task._.Task_EndTime <= end);
            countSum = Gateway.Default.Count<Task>(wc);
            return Gateway.Default.From<Task>().Where(wc).OrderBy(Task._.Task_Tax.Desc).ToArray<Task>(size, (index - 1) * size);
        }

    }
}
