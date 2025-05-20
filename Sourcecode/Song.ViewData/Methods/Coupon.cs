using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;
using Newtonsoft.Json.Linq;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 卡券管理
    /// </summary>
    [HttpPut, HttpGet]
    public class Coupon : ViewMethod, IViewAPI
    {
        #region 卡券增减
        /// <summary>
        /// 用充值卡充值卡券
        /// </summary>
        /// <param name="code">卡号+密码，破折号不能少</param>
        /// <returns>充值产生的流水记录</returns>
        [Student,HttpPost]
        public CouponAccount Recharge(string code)
        {
            Song.Entities.RechargeCode card = Business.Do<IRecharge>().CouponCheckCode(code);
            Song.Entities.Accounts acc = this.User;
            if (acc != null)
            {
                card.Ac_ID = acc.Ac_ID;
                card.Ac_AccName = acc.Ac_AccName;
            }
            CouponAccount ca= Business.Do<IRecharge>().CouponUseCode(card);
            acc.Ac_Coupon = ca.Ca_Total;
            //刷新登录状态的学员信息
            LoginAccount.Status.Fresh(acc);
            return ca;
        }
        /// <summary>
        /// 由管理给学员增加卡券
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="coupon">要操作的卡券数</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public bool Raise(int acid, int coupon, string remark)
        {
            return _admin_operate(acid, 2, coupon, remark);
        }
        /// <summary>
        /// 由管理员减去学员卡券
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="coupon">要操作的卡券数</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public bool Subtract(int acid, int coupon, string remark)
        {
            return _admin_operate(acid, 1, coupon, remark);
        }
        /// <summary>
        /// 管理员操作增减卡券
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="type">增加卡券为1，减去为1</param>
        /// <param name="coupon">要操作的卡券数</param>
        /// <param name="remark"></param>
        /// <returns></returns>
        private bool _admin_operate(int acid, int type, int coupon, string remark)
        {
            if (coupon <= 0) throw new Exception("coupon必须大于零");
            Song.Entities.Accounts st = Business.Do<IAccounts>().AccountsSingle(acid);
            if (st == null) throw new Exception("学员账号不存在");

            Song.Entities.EmpAccount emp = this.Admin;
            //
            Song.Entities.CouponAccount ca = new CouponAccount();
            ca.Ca_Value = coupon;
            if (type == 2)
                ca.Ca_Total = st.Ac_Coupon + coupon;
            if (type == 1)
                ca.Ca_Total = st.Ac_Coupon - coupon;
            ca.Ca_Remark = remark;
            ca.Ac_ID = st.Ac_ID;
            ca.Ca_Source = "管理员操作";
            //充值方式，管理员充值
            ca.Ca_From = 1;

            string mobi = !string.IsNullOrWhiteSpace(emp.Acc_MobileTel) && emp.Acc_AccName != emp.Acc_MobileTel ? emp.Acc_MobileTel : "";
            try
            {
                //如果是充值
                if (type == 2)
                {
                    ca.Ca_Info = string.Format("管理员{0}（{1}{2}）向您充值{3}个卡券", emp.Acc_Name, emp.Acc_AccName, mobi, coupon);
                    Business.Do<IAccounts>().CouponAdd(ca);
                }
                //如果是转出
                if (type == 1)
                {
                    ca.Ca_Info = string.Format("管理员{0}（{1}{2}）扣除您{3}个卡券", emp.Acc_Name, emp.Acc_AccName, mobi, coupon);
                    Business.Do<IAccounts>().CouponPay(ca);
                }
                st.Ac_Coupon = ca.Ca_Total;
                //刷新登录状态的学员信息
                LoginAccount.Status.Fresh(st);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        /// <summary>
        /// 分页获取学员的卡券详情
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="start">查询区间的起始时间</param>
        /// <param name="end">查询区间的结尾时间</param>
        /// <param name="type">类型，支出为1，转入2</param>
        /// <param name="search">按内容检索</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [Student]
        public ListResult PagerForAccount(int acid, DateTime? start, DateTime? end, int type, string search, int size, int index)
        {
            if (acid <= 0)
            {
                Song.Entities.Accounts acc = this.User;
                if (acc != null)
                    acid = acc.Ac_ID;
            }
            int count = 0;
            Song.Entities.CouponAccount[] details = Business.Do<IAccounts>()
                .CouponPager(-1, acid, type, start, end, search, size, index, out count);
            ListResult result = new ListResult(details);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 获取积分流水项的详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Song.Entities.CouponAccount ForID(int id)
        {
            return Business.Do<IAccounts>().CouponSingle(id);
        }
        /// <summary>
        /// 删除单条信息
        /// </summary>
        /// <param name="id">流水的id</param>
        /// <returns></returns>
        [Student]
        [HttpDelete]
        [HttpGet(Ignore = true)]
        public bool DeleteSingle(int id)
        {
            try
            {
                Business.Do<IAccounts>().CouponDelete(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
