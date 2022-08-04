using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;
using System.Data;


namespace Song.ViewData.Methods
{
    /// <summary>
    /// 支付接口
    /// </summary>
    [HttpGet]
    public class Pay : ViewMethod, IViewAPI
    {
        #region 增删改查
        /// <summary>
        /// 支付接口实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Song.Entities.PayInterface ForID(int id)
        {
            return Business.Do<IPayInterface>().PaySingle(id);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [SuperAdmin]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public bool Add(Song.Entities.PayInterface entity)
        {
            try
            {
                PayInterface pi = Business.Do<IPayInterface>().PayIsExist(-1, entity);
                if (pi != null) throw new Exception("当前支付接口已经存在");
                Business.Do<IPayInterface>().PayAdd(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        [SuperAdmin]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public bool Modify(PayInterface entity)
        {
            PayInterface pi = Business.Do<IPayInterface>().PayIsExist(-1, entity);
            if (pi != null) throw new Exception("当前支付接口已经存在");

            Song.Entities.PayInterface old = Business.Do<IPayInterface>().PaySingle(entity.Pai_ID);
            if (old == null) throw new Exception("Not found entity for PayInterface！");
            old.Copy<Song.Entities.PayInterface>(entity);
            Business.Do<IPayInterface>().PaySave(old);
            return true;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [SuperAdmin]
        [HttpDelete, HttpGet(Ignore = true)]
        public int Delete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            string[] arr = id.Split(',');
            foreach (string s in arr)
            {
                int idval = 0;
                int.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    Business.Do<IPayInterface>().PayDelete(idval);
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
        /// 所有接口
        /// </summary>
        /// <param name="platform">接口平台，电脑为web，手机为mobi</param>
        /// <returns>专业列表</returns>
        [SuperAdmin]
        public Song.Entities.PayInterface[] List(string platform)
        {
            return Business.Do<IPayInterface>().PayAll(-1, platform, null);
        }
        /// <summary>
        /// 所有可用接口
        /// </summary>
        /// <param name="platform">接口平台，电脑为web，手机为mobi</param>
        /// <returns></returns>
        public Song.Entities.PayInterface[] ListEnable(string platform)
        {
            return Business.Do<IPayInterface>().PayAll(-1, platform, true);
        }
        /// <summary>
        /// 更改排序
        /// </summary>
        /// <param name="items">数组</param>
        /// <returns></returns>
        [HttpPost]
        [SuperAdmin]
        public bool ModifyTaxis(Song.Entities.PayInterface[] items)
        {
            try
            {
                Business.Do<IPayInterface>().UpdateTaxis(items);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 支付
        /// <summary>
        /// 生成资金流水的记录
        /// </summary>
        /// <param name="money">金额</param>
        /// <param name="payif">支付接口配置的实体对象</param>
        /// <returns>资金流水的记录项，Ma_IsSuccess默认为false</returns>
        [Student][HttpPost]
        public MoneyAccount MoneyIncome(double money,Song.Entities.PayInterface payif)
        {
            if (money <= 0) return null;

            Song.Entities.Accounts acc = this.User;
            //产生支付流水号
            MoneyAccount ma = new MoneyAccount();
            ma.Ma_Money = (decimal)money;
            ma.Ac_ID = acc.Ac_ID;
            ma.Ma_Source = payif.Pai_Pattern;
            ma.Pai_ID = payif.Pai_ID;
            ma.Ma_From = 3;
            ma.Ma_IsSuccess = false;
            ma = Business.Do<IAccounts>().MoneyIncome(ma);
            return ma;
        }
        #endregion
    }
}
