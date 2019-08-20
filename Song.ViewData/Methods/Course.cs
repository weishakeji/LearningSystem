using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Common;


namespace Song.ViewData.Methods
{
    public class Course
    {
        [HttpGet(IsAllow = false)]
        public Song.Entities.Course ForID(int id)
        {
            return Business.Do<ICourse>().CourseSingle(id);
        }
        /// <summary>
        /// 课程的状态
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <returns></returns>        
        public Dictionary<string,object> State(int couid,int olid)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
            //是否免费，或是限时免费
            if (course.Cou_IsLimitFree)
            {
                DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
                if (!(course.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                    course.Cou_IsLimitFree = false;
            }
            Song.Entities.Accounts acc = Extend.LoginState.Accounts.CurrentUser;
            //是否可以学习，是否购买
            bool isStudy, isBuy;
            if (acc != null)
            {
                isStudy = Business.Do<ICourse>().Study(course.Cou_ID, acc.Ac_ID);
                isBuy = course.Cou_IsFree || course.Cou_IsLimitFree ? true : Business.Do<ICourse>().IsBuy(course.Cou_ID, acc.Ac_ID);
            }
            else
            {
                isStudy = isBuy = false;
            }
            dic.Add("isStudy", isStudy);
            dic.Add("isBuy", isBuy);
            return dic;
        }
    }
}
