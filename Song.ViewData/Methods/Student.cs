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
using System.Data;


namespace Song.ViewData.Methods
{
    /// <summary>
    /// 学员管理,主要是学习情况等
    /// </summary>
    [HttpGet]
    public class Student : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 取所有学员
        /// </summary>
        /// <param name="sts">分组信息</param>
        /// <returns></returns>
        public List<Song.Entities.Accounts> All(string sts)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            List<Song.Entities.Accounts> accounts = null;
            //所有学员
            if (string.IsNullOrWhiteSpace(sts))
            {
                accounts = Business.Do<IAccounts>().AccountsCount(org.Org_ID, true, null, -1);
            }
            //分组学员
            if (!string.IsNullOrWhiteSpace(sts) && sts != "-1")
            {
                accounts = Business.Do<IAccounts>().AccountsCount(org.Org_ID, true, sts, -1);
            }
            //处理一些基础信息
            string uppath = Upload.Get["Accounts"].Virtual;
            foreach (Accounts acc in accounts)
            {
                acc.Ac_Pw = string.Empty;
                //个人照片
                if (!string.IsNullOrEmpty(acc.Ac_Photo) && acc.Ac_Photo.Trim() != "")
                {
                    acc.Ac_Photo = uppath + acc.Ac_Photo;
                }
                else
                {
                    acc.Ac_Photo = string.Empty;
                }
            }
            return accounts;
        }
        /// <summary>
        /// 学员所学课程的完成度
        /// </summary>
        /// <param name="stid">学员账号id</param>
        /// <param name="couid">课程id，可以为多个(逗号分隔），可以为空</param>
        /// <returns></returns>
        public DataTable CourseCompletion(int stid, string couid)
        {
            DataTable dt = null;
            if (!string.IsNullOrWhiteSpace(couid))
                dt = Business.Do<IStudent>().StudentStudyCourseLog(stid, couid);
            else
                dt = Business.Do<IStudent>().StudentStudyCourseLog(stid);
            return dt;
        }
    }
}
