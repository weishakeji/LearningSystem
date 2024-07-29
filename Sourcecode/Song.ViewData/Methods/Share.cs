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
using pili_sdk;
using Newtonsoft.Json.Linq;

namespace Song.ViewData.Methods
{

    /// <summary>
    /// 分享相关的数据汇总
    /// </summary> 
    [HttpPut, HttpGet]
    public class Share : ViewMethod, IViewAPI
    {
        //资源的虚拟路径和物理路径
        public static string PathKey = "Accounts";
        public static string VirPath = WeiSha.Core.Upload.Get[PathKey].Virtual;
        public static string PhyPath = WeiSha.Core.Upload.Get[PathKey].Physics;
        /// <summary>
        /// 好朋友（即直接分享注册的账户）
        /// </summary>
        /// <param name="acid">当前学员id</param>
        /// <returns></returns>
        public int Friends(int acid)
        {           
            int friends = Business.Do<IAccounts>().SubordinatesCount(acid, false);
            return friends;
        }
        /// <summary>
        /// 所有朋友，（即享注注册的账户再次分享所得）
        /// </summary>
        /// <param name="acid">当前学员id</param>
        /// <returns></returns>
        public int FriendAll(int acid)
        {
            return Business.Do<IAccounts>().SubordinatesCount(acid, true);            
        }
        /// <summary>
        /// 分页获取下级账号
        /// </summary>
        /// <param name="acid">自身账号id</param>
        /// <param name="acc">按账号查询</param>
        /// <param name="name">按名称查询</param>
        /// <param name="phone">按手机号查询</param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ListResult FriendPager(int acid, string acc, string name, string phone, int index, int size)
        {
            int count = 0;
            Song.Entities.Accounts[] entities = Business.Do<IAccounts>().AccountsPager(-1, -1, acid, null, acc, name, phone,null,-1,null, size, index, out count);
            foreach(Song.Entities.Accounts st in entities)
            {
                st.Ac_Pw = string.Empty;
                st.Ac_CheckUID = string.Empty;
                st.Ac_Ans = string.Empty;
                st.Ac_Photo = System.IO.File.Exists(PhyPath + st.Ac_Photo) ? VirPath + st.Ac_Photo : "";
                if (!st.Ac_IsOpenMobile) st.Ac_MobiTel1 = st.Ac_MobiTel2 = string.Empty;
            }
            Song.ViewData.ListResult result = new ListResult(entities);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 分享得来的积分，分享访问+分享注册
        /// </summary>
        /// <param name="acid">当前学员id</param>
        /// <returns></returns>
        public int EarnPoint(int acid)
        {
            int sumPoint = Business.Do<IAccounts>().PointClac(acid, 2, null, null);
            sumPoint += Business.Do<IAccounts>().PointClac(acid, 3, null, null);
            return sumPoint;
        }
        /// <summary>
        /// 分享得来的现金
        /// </summary>
        /// <param name="acid"></param>
        /// <returns></returns>
        public int EarnMoney(int acid)
        {
            return Business.Do<IAccounts>().MoneyClac(acid, 5, null, null);
        }
        /// <summary>
        /// 分享得来的卡券
        /// </summary>
        /// <param name="acid"></param>
        /// <returns></returns>
        public int EarnCoupon(int acid)
        {
            return Business.Do<IAccounts>().CouponClac(acid, 5, null, null);
        }
        /// <summary>
        /// 积分与卡券的兑换
        /// </summary>
        /// <returns></returns>
        public JObject Param()
        { 
            JObject jo = new JObject();
            //分享积分，每天最多多少分
            jo.Add("SharePoint", Business.Do<ISystemPara>()["SharePoint"].String);
            jo.Add("SharePointMax", Business.Do<ISystemPara>()["SharePointMax"].String);
            //注册积分，每天最多多少分
            jo.Add("RegPoint", Business.Do<ISystemPara>()["RegPoint"].String);
            jo.Add("RegPointMax", Business.Do<ISystemPara>()["RegPointMax"].String);
            return jo;
        }
    }   
}
