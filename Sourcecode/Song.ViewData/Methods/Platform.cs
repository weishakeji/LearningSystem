using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 平台参数
    /// </summary>
    [HttpPut, HttpGet,HttpPost]
    public class Platform : ViewMethod, IViewAPI
    {
        #region 平台信息
        /// <summary>
        /// 平台信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Cache(AdminDisable = true)]
        public JObject PlatInfo()
        {
            string title = Business.Do<ISystemPara>().GetValue("SystemName");
            string intro = Business.Do<ISystemPara>().GetValue("PlatInfo_intro");
            JObject jo = new JObject();
            jo.Add("title", title);
            jo.Add("intro", intro);
            return jo;
        }
        /// <summary>
        /// 保存平台信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="intro"></param>
        /// <returns></returns>
        [SuperAdmin]
        [HttpPost]
        public bool PlatInfoUpdate(string title, string intro)
        {
            try
            {
                Business.Do<ISystemPara>().Save("SystemName", title);
                Business.Do<ISystemPara>().Save("PlatInfo_intro", intro);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 用户注册协议
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Cache(Expires = 60 * 60 * 24)]
        public string RegisterAgreement()
        {
            return Business.Do<IAccounts>().RegAgreement();
        }
        /// <summary>
        /// 授权的商业版本的详细信息
        /// </summary>
        /// <returns></returns>
        public WeiSha.Core.License Edition()
        {
            WeiSha.Core.License lic = WeiSha.Core.License.Value;
            return lic;
        }
        /// <summary>
        /// 版本信息，来自Song.WebSite.dll文件的编译信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Cache(Expires = int.MaxValue)]
        public JObject Version()
        {
            string dllPath = System.AppDomain.CurrentDomain.BaseDirectory;
            Assembly assembly = Assembly.LoadFrom(dllPath + "\\bin\\Song.WebSite.dll");
            JObject jo = new JObject();
            //内部版本号    
            Version version = assembly.GetName().Version;
            jo.Add("version", version.ToString());
            //内版本号的主版本号
            jo.Add("major", version.Major.ToString());
            jo.Add("minor", version.Minor.ToString());
            //获取动态库的属性
            object[] attributes = assembly.GetCustomAttributes(false);
            foreach (object obj in attributes)
            {
                //产品名称
                if (obj is AssemblyProductAttribute)
                    jo.Add("product", ((AssemblyProductAttribute)obj).Product);
                //产品介绍
                if (obj is AssemblyDescriptionAttribute)
                    jo.Add("desc", ((AssemblyDescriptionAttribute)obj).Description);
                //发布日期
                if (obj is AssemblyConfigurationAttribute)
                    jo.Add("release", ((AssemblyConfigurationAttribute)obj).Configuration);
                //版权所有
                if (obj is AssemblyCopyrightAttribute)
                    jo.Add("copyright", ((AssemblyCopyrightAttribute)obj).Copyright);
                //公司名称（开发团队）
                if (obj is AssemblyCompanyAttribute)
                    jo.Add("company", ((AssemblyCompanyAttribute)obj).Company);
            }
            return jo;
        }
        /// <summary>
        /// 主域，来自db.config中的设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Domain()
        {
            return WeiSha.Core.Server.MainName;
        }
        /// <summary>
        /// 网站的端口号
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string ServerPort()
        {
            return WeiSha.Core.Server.Port;
        }
        /// <summary>
        /// 数据库是否链接正常
        /// </summary>
        /// <returns></returns>
        public bool DbConnection()
        {
            bool isCorrect = Business.Do<ISystemPara>().DatabaseLinkTest();
            return isCorrect;
        }

        /// <summary>
        /// 数据库版本
        /// </summary>
        /// <returns></returns>
        public string DbVersion()
        {
            object version = Business.Do<ISystemPara>().ScalarSql("select @@version");
            if (version == null) return string.Empty;
            string str = version.ToString();
            str = str.Replace("\n", "").Replace("\t", "").Replace("\r", "");
            return str;
        }
        /// <summary>
        /// 服务器端的时间，用于考试等场景，保持所有学员跨时区时间同步
        /// </summary>
        /// <returns>时间戳，客户端通过eval('new ' + eval('/Date({$servertime})/').source)转为本地时间</returns>
        [HttpGet, HttpPost]
        public long ServerTime()
        {
            return WeiSha.Core.Server.getTime();
        }
        /// <summary>
        /// 数据库字段与表是否完成
        /// </summary>
        /// <returns>string, string[],前者为表名，后者为字段</returns>
        public JArray DbCheck()
        {
            bool isCorrect = Business.Do<ISystemPara>().DatabaseLinkTest();
            if (!isCorrect)
                throw new Exception("数据库链接不正常！");
            JArray jarr = new JArray();
            Dictionary<string, string[]> dic = Business.Do<ISystemPara>().DatabaseCompleteTest();
            foreach (KeyValuePair<string, string[]> item in dic)
            {
                JObject jo = new JObject();
                JArray jval = new JArray();
                foreach (string s in item.Value) jval.Add(s);
                jo.Add(item.Key, jval);
                jarr.Add(jo);
            }
            return jarr;
        }
        /// <summary>
        /// 是否支持多机构
        /// </summary>
        /// <returns>0为启用多机构，1为单机构</returns>
        [HttpGet]
        public int MultiOrgan()
        {
            //是否启用多机构，默认启用
            return Business.Do<ISystemPara>()["MultiOrgan"].Int32 ?? 0;
        }
        /// <summary>
        ///  设置是否支持多机构
        /// </summary>
        /// <param name="multi">0为启用多机构，1为单机构</param>
        /// <returns></returns>
        [SuperAdmin]
        [HttpPost]
        public bool MultiOrganUpdate(int multi)
        {
            try
            {
                //是否启用多机构
                Business.Do<ISystemPara>().Save("MultiOrgan", multi.ToString(), false);
                //刷新全局参数
                Business.Do<ISystemPara>().Refresh();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 字体图标
        /// </summary>
        /// <returns>iconfont图标库的编码，不带说明</returns>
        [HttpGet]
        [Cache(Expires = 60 * 24 * 30)]
        public string[] IconFonts()
        {
            string file = "/Utilities/Fonts/index.html";
            file = WeiSha.Core.Server.MapPath(file);
            string html = System.IO.File.ReadAllText(file, Encoding.UTF8);
            //
            List<string> list = new List<string>();
            string pattern = @"<div.*?>\\(.*?)<\/div>";
            foreach (Match match in Regex.Matches(html, pattern))
            {
                list.Add(match.Groups[1].Value);
            }
            return list.ToArray();
        }
        /// <summary>
        /// 字体图标
        /// </summary>
        /// <returns>iconfont图标库的编码,带说明</returns>
        [HttpGet]
        [Cache(Expires = 60 * 24 * 30)]
        public JArray IconJson()
        {
            string file = "/Utilities/Fonts/index.html";
            file = WeiSha.Core.Server.MapPath(file);
            string html = System.IO.File.ReadAllText(file, Encoding.UTF8);
            //
            JArray arr = new JArray();
            string pattern = @"<li>(.*?)<\/li>";
            foreach (Match match in Regex.Matches(html, pattern,RegexOptions.Singleline))
            {
                string li = match.Groups[1].Value;
                Match code = Regex.Match(li, @"<div.*?>\\(.*?)<\/div>", RegexOptions.Singleline);
                Match name = Regex.Match(li, @"<div class=""name"">(.*?)<\/div>", RegexOptions.Singleline);
                JObject jo = new JObject();
                if (code.Success)
                {
                    jo.Add(code.Groups[1].Value,
                        name.Success ? name.Groups[1].Value : "");
                }
                arr.Add(jo);               
            }
            return arr;
        }
        /// <summary>
        /// 获取地址的gps坐标
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [HttpGet]
        public Dictionary<string, string> PositionGPS(string address)
        {
            WeiSha.Core.Param.Method.Position posi = WeiSha.Core.Request.Position(address);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("lng", posi.Longitude);
            dic.Add("lat", posi.Latitude);
            return dic;
        }
        /// <summary>
        /// 上传文件的路径
        /// </summary>
        /// <param name="key">来自web.config中Upload的key值</param>
        /// <returns></returns>
        [HttpGet]
        public JObject UploadPath(string key)
        {
            string virPath = WeiSha.Core.Upload.Get[key].Virtual;
            string phyPath = WeiSha.Core.Upload.Get[key].Physics;
            JObject jo = new JObject();
            jo.Add("virtual", virPath);
            jo.Add("physics", phyPath);
            return jo;

        }
        /// <summary>
        /// 平台数据,包括课程数、专业数、试题数、视频数，学员数，教师数等
        /// </summary>
        /// <param name="orgid">机构id,小于等于零取所有机构</param>
        /// <returns></returns>
        [HttpGet]
        public JObject Datas(int orgid)
        {
            JObject jo = new JObject();
            //课程数
            jo.Add("course", Business.Do<ICourse>().CourseOfCount(orgid, -1, -1, null));
            //专业数
            jo.Add("subject", Business.Do<ISubject>().SubjectOfCount(orgid, -1, null, true));
            //试题数
            jo.Add("question", Business.Do<IQuestions>().QuesOfCount(orgid, -1, -1,-1,-1, null));
            //学员数
            jo.Add("account", Business.Do<IAccounts>().AccountsOfCount(orgid, null));
            //教师数
            jo.Add("teacher", Business.Do<ITeacher>().TeacherOfCount(orgid, null));
            //视频数
            jo.Add("video", Business.Do<IAccessory>().OfCount(orgid, string.Empty, "CourseVideo"));
            //试卷数
            jo.Add("testpaper", Business.Do<ITestPaper>().PaperOfCount(orgid, -1, -1, -1, null));
            //资料数
            jo.Add("document", Business.Do<IAccessory>().OfCount(orgid, string.Empty, "Course"));

            return jo;
        }
        #endregion

        #region 系统参数管理
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Parameter(string key)
        {
            return Business.Do<ISystemPara>().GetValue(key);
        }
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public SystemPara ParamForID(int id)
        {
            return Business.Do<ISystemPara>().GetSingle(id);
        }
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public SystemPara ParamForKey(string key)
        {
            return Business.Do<ISystemPara>().GetSingle(key);
        }
        /// <summary>
        /// 修改参数信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        [Admin]
        [HtmlClear(Not = "val")]
        public bool ParamUpdate(string key, string val)
        {
            try
            {
                Business.Do<ISystemPara>().Save(key, val);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改系统参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public bool ParamModify(SystemPara entity)
        {
            Song.Entities.SystemPara old = Business.Do<ISystemPara>().GetSingle(entity.Sys_Id);
            if (old == null) throw new Exception("Not found entity for SystemPara！");

            old.Copy<Song.Entities.SystemPara>(entity);
            Business.Do<ISystemPara>().Save(old);
            return true;
        }
        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public bool ParamAdd(SystemPara entity)
        {
            try
            {
                Business.Do<ISystemPara>().Add(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpDelete]
        [SuperAdmin]
        public bool ParamDelete(string key)
        {
            try
            {
                Business.Do<ISystemPara>().Delete(key);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 所有的系统参数
        /// </summary>
        /// <returns></returns>
        [SuperAdmin]
        [HttpGet, HttpPost]
        public List<SystemPara> Parameters()
        {
            return Business.Do<ISystemPara>().GetAll();
        }
        #endregion

        #region Excel导入

        /// <summary>
        /// 上传excel文件
        /// </summary>
        /// <returns>工作簿列表、文件名（服务器端）</returns>
        [HttpPost]
        [Upload(Extension = "xls,xlsx", MaxSize = int.MaxValue, CannotEmpty = true)]
        [Admin, Teacher]
        public JObject ExcelUpload()
        {
            //资源的虚拟路径和物理路径
            string pathKey = "Temp";
            string phyPath = WeiSha.Core.Upload.Get[pathKey].Physics;
            //文件存放在服务器的名称，仅名称
            string filename = string.Empty;
            try
            {
                foreach (string key in this.Files)
                {
                    HttpPostedFileBase file = this.Files[key];
                    filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                    file.SaveAs(phyPath + filename);
                    break;
                }
                //工作簿
                JArray table = ViewData.Helper.Excel.Sheets(phyPath + filename);
                JObject jo = new JObject();
                jo.Add("file", filename);
                jo.Add("sheets", table);
                return jo;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取工作薄的列表，即第一行的标题
        /// </summary>
        /// <param name="xlsFile"></param>
        /// <param name="sheetIndex"></param>
        /// <returns>name:工作簿名称;index:工作簿索引;count:记录数;columns:列名 </returns>
        [HttpGet]
        public JObject ExcelSheetColumn(string xlsFile, int sheetIndex)
        {
            string phyPath = WeiSha.Core.Upload.Get["Temp"].Physics;
            return ViewData.Helper.Excel.Columns(phyPath + xlsFile, sheetIndex);
        }
        /// <summary>
        /// 获取列名与字段名的对应关系的设置
        /// </summary>
        /// <param name="file">配置文件的路径</param>
        [HttpGet]
        public DataTable ExcelConfig(string file)
        {
            return ViewData.Helper.Excel.Config(file);
        }
        #endregion

        
        
        /// <summary>
        /// 解析身份证号
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        [HttpGet]
        public IDCardNumber IDCardNumber(string card)
        {
            return WeiSha.Core.IDCardNumber.Get(card);
        }
    }
}
