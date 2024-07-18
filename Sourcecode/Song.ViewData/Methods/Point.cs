using Newtonsoft.Json.Linq;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using System;
using WeiSha.Core;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 学员积分管理
    /// </summary>
    [HttpPut, HttpGet]
    public class Point : ViewMethod, IViewAPI
    {
        #region 设置项

        /// <summary>
        /// 设置积分相关项
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        public bool ParamSetup(Letter letter)
        {
            try
            {
                //初次注册，送积分
                Business.Do<ISystemPara>().Save("RegFirst", letter["RegFirst"].String);
                //登录积分，每天最多多少分
                Business.Do<ISystemPara>().Save("LoginPoint", letter["LoginPoint"].String);
                Business.Do<ISystemPara>().Save("LoginPointMax", letter["LoginPointMax"].String);
                //分享积分，每天最多多少分
                Business.Do<ISystemPara>().Save("SharePoint", letter["SharePoint"].String);
                Business.Do<ISystemPara>().Save("SharePointMax", letter["SharePointMax"].String);
                //注册积分，每天最多多少分
                Business.Do<ISystemPara>().Save("RegPoint", letter["RegPoint"].String);
                Business.Do<ISystemPara>().Save("RegPointMax", letter["RegPointMax"].String);
                //积分与卡券的兑换
                Business.Do<ISystemPara>().Save("PointConvert", letter["PointConvert"].String);
                //刷新参数
                Business.Do<ISystemPara>().Refresh();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 积分相关设置项
        /// </summary>
        /// <returns>
        /// RegFirst:初次注册，送积分
        /// LoginPoint:登录积分
        /// LoginPointMax:每天最多多少分
        /// SharePoint:分享得积分
        /// SharePointMax：每天最多多少分
        /// RegPoint：注册积分
        /// RegPointMax：每天最多多少分
        /// PointConvert：积分兑换卡券的比率
        /// </returns>
        public JObject Param()
        {
            JObject dic = new JObject();
            //初次注册，送积分
            dic.Add("RegFirst", Business.Do<ISystemPara>()["RegFirst"].String);
            //登录积分，每天最多多少分
            dic.Add("LoginPoint", Business.Do<ISystemPara>()["LoginPoint"].String);
            dic.Add("LoginPointMax", Business.Do<ISystemPara>()["LoginPointMax"].String);
            //分享积分，每天最多多少分
            dic.Add("SharePoint", Business.Do<ISystemPara>()["SharePoint"].String);
            dic.Add("SharePointMax", Business.Do<ISystemPara>()["SharePointMax"].String);
            //注册积分，每天最多多少分
            dic.Add("RegPoint", Business.Do<ISystemPara>()["RegPoint"].String);
            dic.Add("RegPointMax", Business.Do<ISystemPara>()["RegPointMax"].String);
            //积分与卡券的兑换
            dic.Add("PointConvert", Business.Do<ISystemPara>()["PointConvert"].String);
            return dic;
        }

        #endregion 设置项

        /// <summary>
        /// 积分兑换卡券
        /// </summary>
        /// <param name="coupon">要兑换的卡券数量</param>
        /// <returns></returns>
        [Student]
        public CouponAccount Exchange(int coupon)
        {
            Song.Entities.Accounts acc = this.User;
            try
            {
                CouponAccount ca = Business.Do<IAccounts>().CouponExchange(acc, coupon);
                //刷新登录状态的学员信息
                LoginAccount.Status.Fresh(acc);
                return ca;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 分页获取学员的积分详情
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
            Song.Entities.PointAccount[] details = Business.Do<IAccounts>()
                .PointPager(-1, acid, type, search, start, end, size, index, out count);
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
        public Song.Entities.PointAccount ForID(int id)
        {
            return Business.Do<IAccounts>().PointSingle(id);
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
                Business.Do<IAccounts>().PointDelete(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 管理员对积分的增减

        /// <summary>
        /// 由管理给学员增加积分
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="point">要操作的积分数</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public bool Raise(int acid, int point, string remark)
        {
            return _admin_operate(acid, 2, point, remark);
        }

        /// <summary>
        /// 由管理员减去学员积分
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="point">要操作的积分数</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public bool Subtract(int acid, int point, string remark)
        {
            return _admin_operate(acid, 1, point, remark);
        }

        /// <summary>
        /// 管理员操作增减积分
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="type">增加积分为1，减去为1</param>
        /// <param name="point">要操作的积分数</param>
        /// <param name="remark"></param>
        /// <returns></returns>
        private bool _admin_operate(int acid, int type, int point, string remark)
        {
            if (point <= 0) throw new Exception("point必须大于零");
            Song.Entities.Accounts st = Business.Do<IAccounts>().AccountsSingle(acid);
            if (st == null) throw new Exception("学员账号不存在");

            Song.Entities.EmpAccount emp = this.Admin;
            //
            Song.Entities.PointAccount pa = new PointAccount();
            pa.Pa_Value = point;
            if (type == 2)
                pa.Pa_TotalAmount = st.Ac_Point + point;
            if (type == 1)
                pa.Pa_TotalAmount = st.Ac_Point - point;
            pa.Pa_Remark = remark;
            pa.Ac_ID = st.Ac_ID;
            pa.Pa_Source = "管理员操作";

            //充值方式，管理员充值
            pa.Pa_From = 1;

            string mobi = !string.IsNullOrWhiteSpace(emp.Acc_MobileTel) && emp.Acc_AccName != emp.Acc_MobileTel ? emp.Acc_MobileTel : "";
            try
            {
                //如果是充值
                if (type == 2)
                {
                    pa.Pa_Info = string.Format("管理员{0}（{1},{2}）向您充值{3}个积分", emp.Acc_Name, emp.Acc_AccName, mobi, point);
                    Business.Do<IAccounts>().PointAdd(pa);
                }
                //如果是转出
                if (type == 1)
                {
                    pa.Pa_Info = string.Format("管理员{0}（{1},{2}）扣除您{3}个积分", emp.Acc_Name, emp.Acc_AccName, mobi, point);
                    Business.Do<IAccounts>().PointPay(pa);
                }
                st.Ac_Point = pa.Pa_Total;
                //刷新登录状态的学员信息
                LoginAccount.Status.Fresh(st);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion 管理员对积分的增减

        #region 积分赚取

        /// <summary>
        /// 学员登录时赚取积分
        /// </summary>
        /// <param name="source">来源，例如：电脑网页、手机网页、微信小程序、</param>
        /// <param name="info">登录信息，这里指登录方式，例如：账号密码登录、微信登录、QQ登录</param>
        /// <param name="remark">备注：一些说明</param>
        /// <returns></returns>
        [HttpPost]
        public int AddForLogin(string source, string info, string remark)
        {
            Song.Entities.Accounts st = this.User;
            if (st == null) return 1;
            Business.Do<IAccounts>().PointAdd4Login(st.Ac_ID, source, info, remark);
            //刷新登录状态的学员信息
            LoginAccount.Status.Fresh(st);
            return 1;
        }

        #endregion 积分赚取
    }
}