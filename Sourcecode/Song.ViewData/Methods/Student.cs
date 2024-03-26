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
    /// 学员管理,主要是学习情况等
    /// </summary>
    [HttpPut, HttpGet]
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
            string uppath = WeiSha.Core.Upload.Get["Accounts"].Virtual;
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

        #region 学员组与课程
        /// <summary>
        /// 学员组下的课程数量
        /// </summary>
        /// <param name="sortid">学员组的id</param>
        /// <returns></returns>
        public int SortCoursOfNumber(long sortid)
        {
            return Business.Do<IStudent>().SortCourseCount(sortid);
        }
         
        /// <summary>
        /// 学员组关联的课程
        /// </summary>
        /// <param name="sortid">学员组id</param>
        /// <param name="name">按课程名称检索</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult SortCoursePager(long sortid, string name, int size, int index)
        {
            int count = 0;
            List<Song.Entities.Course> list = Business.Do<IStudent>().SortCoursePager(sortid, name, size, index, out count);
            for (int i = 0; i < list.Count; i++)
            {
                Song.Entities.Course c = Methods.Course._tran(list[i]);
                c.Cou_Intro = c.Cou_Target = c.Cou_Content = "";
                if (!string.IsNullOrWhiteSpace(c.Cou_Name))
                    c.Cou_Name = c.Cou_Name.Replace("\"", "&quot;");
                list[i] = c;
            }
            ListResult result = new ListResult(list);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 增加学员组与课程的关联
        /// </summary>
        /// <param name="sortid">学员组id</param>
        /// <param name="couid">课程id,多个id用逗号分隔</param>
        /// <returns></returns>
        [HttpPost]
        [Admin,Teacher]
        public int SortCourseAdd(long sortid, string couid)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(couid)) return i;
            string[] arr = couid.Split(',');
            foreach (string s in arr)
            {
                long idval = 0;
                long.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    int n= Business.Do<IStudent>().SortCourseAdd(sortid, idval);
                    if (n > 0) i++;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }            
            return i;
        }
        /// <summary>
        /// 移除学员组与课程的关联
        /// </summary>
        /// <param name="sortid">学员组id</param>
        /// <param name="couid">课程id,多个id用逗号分隔</param>
        /// <returns></returns>
        [HttpDelete]
        [Admin, Teacher]
        public int SortCourseRemove(long sortid, string couid)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(couid)) return i;
            string[] arr = couid.Split(',');
            foreach (string s in arr)
            {
                long idval = 0;
                long.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    Business.Do<IStudent>().SortCourseDelete(sortid, idval);
                    i++;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return i;
        }
        #endregion

        #region 统计信息

        /// <summary>
        /// 学员活跃度
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="stsid">学员组id</param>
        /// <param name="acc">账号</param>
        /// <param name="name">姓名</param>
        /// <param name="mobi">手机号</param>
        /// <param name="idcard">身份证号</param>
        /// <param name="code">学号</param>
        /// <param name="orderby">排序字段；
        /// logincount:登录次数
        /// logintime：最后登录时间
        /// coursecount：课程购买数
        /// rechargecount：充值次数
        /// lastrecharge:最后充值时间
        /// laststudy：最后视频学习时间
        /// lastexrcise：最后试题练习时间
        /// lasttest：最后测试时间
        /// lastexam：最后考试时间
        /// </param>
        /// <param name="orderpattr">排序方式，asc或desc</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [Cache]
        public ListResult Activation(int orgid, long stsid, string acc, string name, string mobi, string idcard, string code,
           string orderby, string orderpattr,
           int size, int index)
        {
            int total = 0;
            DataTable dt = Business.Do<IStudent>().Activation(orgid, stsid, acc, name, mobi, idcard, code, orderby, orderpattr, size, index, out total);
            ListResult result = new ListResult(dt);
            result.Index = index;
            result.Size = size;
            result.Total = total;
            return result;
        }

        #endregion
    }
}
