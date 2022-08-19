using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using WeiSha.Core;
using Song.Entities;
using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;

namespace Song.ServiceImpls
{
    public class PayInterfaceCom :IPayInterface
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void PayAdd(PayInterface entity)
        {           
            if (entity.Org_ID < 1)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganRoot();
                if (org != null) entity.Org_ID = org.Org_ID;              
            }
            //添加对象，并设置排序号
            object obj = Gateway.Default.Max<PayInterface>(PayInterface._.Pai_Tax, PayInterface._.Pai_Tax > -1 && PayInterface._.Org_ID == entity.Org_ID);
            entity.Pai_Tax = obj is int ? (int)obj + 1 : 1;
            Gateway.Default.Save<PayInterface>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void PaySave(PayInterface entity)
        {
            if (entity.Org_ID < 1)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganRoot();
                if (org != null) entity.Org_ID = org.Org_ID;
            }
            Gateway.Default.Save<PayInterface>(entity);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void PayDelete(PayInterface entity)
        {
            Gateway.Default.Delete<PayInterface>(entity);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void PayDelete(int identify)
        {
            Gateway.Default.Delete<PayInterface>(PayInterface._.Pai_ID == identify);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public PayInterface PaySingle(int identify)
        {
            return Gateway.Default.From<PayInterface>().Where(PayInterface._.Pai_ID == identify).ToFirst<PayInterface>();
        }
        /// <summary>
        /// 获取所有；
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="platform">接口平台，电脑为web，手机为mobi</param>
        /// <param name="isEnable">是否允许</param>
        /// <returns></returns>
        public PayInterface[] PayAll(int orgid, string platform, bool? isEnable)
        {
             WhereClip wc = new WhereClip();
             if (orgid > -1) wc.And(PayInterface._.Org_ID == orgid);
             if (!string.IsNullOrWhiteSpace(platform)) wc &= PayInterface._.Pai_Platform == platform.ToLower();
             if (isEnable != null) wc.And(PayInterface._.Pai_IsEnable == (bool)isEnable);
             return Gateway.Default.From<PayInterface>().Where(wc).OrderBy(PayInterface._.Pai_Tax.Asc).ToArray<PayInterface>();
        }
        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns></returns>
        public PayInterface PayIsExist(int orgid, PayInterface entity)
        {
            PayInterface mm = null;
            WhereClip wc = new WhereClip();
            if (orgid > -1) wc.And(PayInterface._.Org_ID == orgid);
            mm = Gateway.Default.From<PayInterface>()
                .Where(wc && PayInterface._.Pai_Name == entity.Pai_Name && PayInterface._.Pai_ID != entity.Pai_ID)
                .ToFirst<PayInterface>();
            return mm;
        }
        /// <summary>
        /// 更改排序
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool UpdateTaxis(PayInterface[] items)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (PayInterface item in items)
                    {
                        tran.Update<PayInterface>(
                            new Field[] { PayInterface._.Pai_Tax },
                            new object[] { item.Pai_Tax },
                            PayInterface._.Pai_ID == item.Pai_ID);
                    }
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
        /// <summary>
        /// 计算某一个支付接口的收入
        /// </summary>
        /// <param name="paid">支付接口的id</param>
        /// <returns></returns>
        public decimal Summary(int paid)
        {
            object o = Gateway.Default.Sum<MoneyAccount>(MoneyAccount._.Ma_Money, MoneyAccount._.Pai_ID == paid && MoneyAccount._.Ma_IsSuccess == true);
            if (o == null) return 0;
            return (decimal)o;
        }
    }
}
