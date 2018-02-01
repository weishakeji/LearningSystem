using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;

namespace Song.Site.SOAP
{
    /// <summary>
    /// Mobile 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class Mobile : System.Web.Services.WebService
    {
        #region 通记录功能院系
        /// <summary>
        /// 通过手机号判断当前员工是否在职
        /// </summary>
        /// <param name="phone">员工的手机号，从手机中自动获取</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        [WebMethod]
        public bool IsOnJob(string phone,string code)
        {
            if (!isVerify(code)) return false;
            return Business.Do<IEmployee>().IsOnJob(phone);
        }
        ///// <summary>
        ///// 通过员工手机号，获取员工信息
        ///// </summary>
        ///// <param name="phoneNumber">员工的手机号，从手机中自动获取</param>
        ///// <param name="code">验证码</param>
        ///// <returns></returns>
        //[WebMethod]
        //public Song.Entities.EmpAccount Employee(string phoneNumber, string code)
        //{
        //    if (!isVerify(code)) return null;
        //    return Business.Do<IEmployee>().GetSingleByPhone(phoneNumber);
        //}
        /// <summary>
        /// 获取所有在职员工
        /// </summary>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        [WebMethod]
        public Song.Entities.EmpAccount[] Employees(string code)
        {
            if (!isVerify(code)) return null;
            int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
            Song.Entities.EmpAccount[] eas = null;
            eas = Business.Do<IEmployee>().GetAll(orgid,-1,true, "");
            foreach (Song.Entities.EmpAccount ea in eas)
            {
                if (!ea.Acc_IsOpenTel)
                    ea.Acc_Tel = "";
                if (!ea.Acc_IsOpenMobile)
                    ea.Acc_MobileTel = "";
                ea.Acc_Pw = "";
            }
            return eas;
        }
        /// <summary>
        /// 获取企业信息
        /// </summary>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        [WebMethod]
        public Song.Entities.Organization OrgInfo(string code)
        {
            if (!isVerify(code)) return null;
            return Business.Do<IOrganization>().OrganDefault();
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 验证是否服务端允许手机客户端访问，是否需要验证码，验证码是否正确
        /// </summary>
        /// <param name="verifyCode"></param>
        /// <returns>不通过，返回false</returns>
        private bool isVerify(string verifyCode)
        {
            //是否允许客户端访问
            bool isAllow = Business.Do<ISystemPara>()["IsAllowMobile"].Boolean ?? true;
            if (!isAllow) return false;
            //是否需要验证码
            bool isVerify = Business.Do<ISystemPara>()["IsAllowMobileVerifyCode"].Boolean ?? true;
            if (!isVerify) return true;     //如果不需要验证，则返回true
            //验证码是否正确
            string code = Business.Do<ISystemPara>()["MobileVerifyCode"].String;
            if (code == verifyCode.Trim()) return true;
            return false;
        }
        #endregion
    }
}
