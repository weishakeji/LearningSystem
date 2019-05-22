using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Extend;


namespace Song.Site.Ajax
{
    /// <summary>
    /// BuySubmit 的摘要说明
    /// </summary>
    public class CourseBuySubmit : IHttpHandler
    {
        protected HttpContext Context { get; private set; }
        //验证码
        private string veriCode = WeiSha.Common.Request.Form["veriCode"].MD5;
        //价格项的id，课程id
        private int cpid = WeiSha.Common.Request.Form["cpid"].Int32 ?? 0;
        private int couid = WeiSha.Common.Request.Form["couid"].Int32 ?? 0;
        //返回的url地址
        private string return_url = WeiSha.Common.Request.Form["return_url"].String;
        //是否免费购买
        private bool isFree = (WeiSha.Common.Request.Form["isfree"].Int16 ?? 0) == 0 ? true : false;
        //是否试用课程
        private bool isTry = (WeiSha.Common.Request.Form["istry"].Int16 ?? 0) == 0 ? false : true;

        public void ProcessRequest(HttpContext context)
        {
            this.Context = context;
            Context.Response.ContentType = "text/plain";
            //如果未登录
            if (!Extend.LoginState.Accounts.IsLogin)
            {
                Context.Response.Write(getBackJson(1, null, null));
                return;
            }
            //当前学员
            Song.Entities.Accounts st = Extend.LoginState.Accounts.CurrentUser;
            //当前课程
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
            if (course == null)
            {
                Context.Response.Write(getBackJson(4, null, null));
                return;
            }            
            //生成学员与课程的关联
            Song.Entities.Student_Course sc = new Entities.Student_Course();
            //是否试用
            if (isTry) Try(course, st);
            //如果免费购买
            if (!isTry && isFree) FreeBuy(course, st);
            //如果不是免费课程，花钱买课程
            if (!isTry && !isFree) Buy(course, st);            
        }
        /// <summary>
        /// 试用课程
        /// </summary>
        private void Try(Song.Entities.Course course, Song.Entities.Accounts st)
        {
            //如果不可以试用
            if (!course.Cou_IsFree && !course.Cou_IsTry)
            {
                //当课程不可以试用，直接退出
                Context.Response.Write(getBackJson(7, null, null));
                return;
            }
            else
            {
                try
                {
                    Song.Entities.Student_Course sc = Business.Do<ICourse>().Tryout(st.Ac_ID, couid);
                    Extend.LoginState.Accounts.Course(course);
                    Context.Response.Write(getBackJson(0, sc, null));
                    return;
                }
                catch(Exception ex)
                {
                    Context.Response.Write(getBackJson(6, null, ex));
                    return;
                }
            }
        }
        /// <summary>
        /// 免费购买
        /// </summary>
        /// <param name="course"></param>
        /// <param name="st"></param>
        private void FreeBuy(Song.Entities.Course course, Song.Entities.Accounts st)
        {
            //如果不是免费课程
            if (!course.Cou_IsFree)
            {
                //当课程不是免费的，直接退出
                Context.Response.Write(getBackJson(7, null, null));
                return;
            }
            else
            {                
                try
                {
                    //生成学员与课程的关联
                    Song.Entities.Student_Course sc = Business.Do<ICourse>().FreeStudy(st.Ac_ID, couid);
                    Extend.LoginState.Accounts.Course(course);
                    Context.Response.Write(getBackJson(0, sc, null));
                    return;
                }
                catch(Exception ex)
                {
                    Context.Response.Write(getBackJson(6, null, ex));
                    return;
                }
            }
        }
        /// <summary>
        /// 购买
        /// </summary>
        /// <param name="course"></param>
        /// <param name="st"></param>
        private void Buy(Song.Entities.Course course, Song.Entities.Accounts st)
        {
            ////取图片验证码
            //string imgCode = WeiSha.Common.Request.Cookies["buycode"].ParaValue;
            ////验证码不正确
            //if (veriCode != imgCode)
            //{
            //    Context.Response.Write(getBackJson(2, null, null));
            //    return;
            //}
            //价格项
            Song.Entities.CoursePrice price = Business.Do<ICourse>().PriceSingle(cpid);
            if (price == null)
            {
                Context.Response.Write(getBackJson(3, null,null));
                return;
            }
            //余额是否充足
            decimal money = st.Ac_Money;    //资金余额
            int coupon = st.Ac_Coupon;      //卡券余额
            int mprice = price.CP_Price;    //价格，所需现金
            int cprice = price.CP_Coupon;   //价格，可以用来抵扣的卡券
            bool tm = money >= mprice || money >= (mprice - (coupon > cprice ? cprice : coupon));
            if (!tm)
            {
                Context.Response.Write(getBackJson(5, null, null));
                return;
            }
            //WeiSha.Common.Log.Write
            //开始实现购买
            try
            {
                Song.Entities.Student_Course sc = Business.Do<ICourse>().Buy(st.Ac_ID, couid, price);
                Extend.LoginState.Accounts.Course(course);
                Context.Response.Write(getBackJson(0, sc, null));
                return;
            }
            catch (Exception ex)
            {
                Context.Response.Write(getBackJson(6, null, ex));
                return;
            }
        }       
        /// <summary>
        /// 返回状态结果
        /// </summary>
        /// <param name="status"></param>
        /// <param name="sc"></param>
        /// <param name="ex">如果报错，记录错误信息</param>
        /// <returns></returns>
        private string getBackJson(int status, Song.Entities.Student_Course sc, Exception ex)
        {
            string json = "{";
            json += "'status':'" + status + "',";
            json += "'couid':'" + couid + "',";
            //当前学员的剩余钱数
            Song.Entities.Accounts st = Extend.LoginState.Accounts.CurrentUser;
            json += "'money':'" + (st != null ? st.Ac_Money : -1) + "',";
            //返回不为空
            if (sc != null)
            {
                json += "'startTime':'" + WeiSha.Common.Server.getTime(sc.Stc_StartTime) + "',";
                json += "'endTime':'" + WeiSha.Common.Server.getTime(sc.Stc_EndTime) + "',";
            }
            if (ex != null)
            {
                //写入错误日志，并返回所在路径
                string path = WeiSha.Common.Log.Error("Course", ex);
                json += "'logfile':'" + path + "',";
                json += "'errinfo':'" + ex.Message + "',";
            }
            json += "'return_url':'" + return_url + "'";
            return json + "}";
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}