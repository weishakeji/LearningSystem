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
using Newtonsoft.Json.Linq;
using System.IO;

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

        #region 学习成绩
        /// <summary>
        /// 批量计算某个学员所有课程学习的综合成绩
        /// </summary>
        /// <param name="acid">学员账号id</param>
        /// <returns></returns>
        public bool ResultScoreCalc(int acid)
        {
            return Business.Do<ICourse>().ResultScoreCalc4Student(acid);
        }
        private static string outputPath_ResultScore = "ResultScoreToExcel";
        /// <summary>
        /// 学员学习记录生成excel
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <returns></returns>
        [HttpPost]
        [Admin, Teacher]
        public JObject ResultScoreOutputExcel(int acid)
        {
            //导出文件的位置
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath_ResultScore + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);
            Accounts acc = Business.Do<IAccounts>().AccountsSingle(acid);
            if (acc == null) throw new Exception("当前学员不存在");

            DateTime date = DateTime.Now;
            string filename = string.Format("{0}.{1}.({2}).xls", acid, acc.Ac_Name, date.ToString("yyyy-MM-dd hh-mm-ss"));
            if (File.Exists(rootpath + filename))
            {
                throw new Exception("当前文档已经存在，请删除或稍后再操作");
            }
            Business.Do<IStudent>().ResultScoreToExcel(rootpath + filename, acid);
            JObject jo = new JObject();
            jo.Add("file", filename);
            jo.Add("url", string.Format("{0}/{1}", WeiSha.Core.Upload.Get["Temp"].Virtual + outputPath_ResultScore, filename));
            jo.Add("date", date);
            return jo;
        }
        /// <summary>
        /// 删除学习成果导出的Excel文件
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="filename">文件名，带后缀名，不带路径</param>
        /// <returns></returns>
        [HttpDelete]
        public bool ResultScoreFileDelete(int acid, string filename)
        {
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath_ResultScore + "\\";
            if (!System.IO.Directory.Exists(rootpath)) return false;
            string filePath = rootpath + acid + "." + filename;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 学员学习成果的导出，已经生成的Excel文件
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <returns>file:文件名,url:下载地址,date:创建时间</returns>
        public JArray ResultScoreFiles(int acid)
        {
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath_ResultScore + "\\";
            JArray jarr = new JArray();
            if (!System.IO.Directory.Exists(rootpath))return jarr;
            
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(rootpath);
            FileInfo[] files = dir.GetFiles("*.xls").OrderByDescending(f => f.CreationTime).ToArray();
            foreach (System.IO.FileInfo f in files)
            {
                if (f.Name.IndexOf(".") < 0) continue;
                string prefix = f.Name.Substring(0, f.Name.IndexOf("."));
                if (prefix.Length < 1) continue;
                if (prefix != acid.ToString()) continue;

                JObject jo = new JObject();
                string name = f.Name.Substring(f.Name.IndexOf(".") + 1);
                jo.Add("file", name);
                jo.Add("url", string.Format("{0}/{1}", WeiSha.Core.Upload.Get["Temp"].Virtual + outputPath_ResultScore, f.Name));
                jo.Add("date", f.CreationTime);
                jo.Add("size", f.Length);
                jarr.Add(jo);
            }
            return jarr;
        }
        #endregion
    }
}
