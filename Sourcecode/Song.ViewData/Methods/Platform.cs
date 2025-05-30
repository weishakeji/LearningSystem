﻿using System;
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
        /// 公司产品版本
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Cache(Expires = 999)]
        public System.Data.DataTable EditionsChinese()
        {
            return WeiSha.Core.Request.EditionsChinese();
        }
        /// <summary>
        /// 公司产品版本
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Cache(Expires = 999)]
        public System.Data.DataTable Editions()
        {
            return WeiSha.Core.Request.Editions();
        }
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
            string dllfile = System.AppDomain.CurrentDomain.BaseDirectory + "\\bin\\Song.WebSite.dll";
            Assembly assembly = Assembly.LoadFrom(dllfile);
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
                //版本状态
                if (obj is AssemblyConfigurationAttribute)
                    jo.Add("stage", ((AssemblyConfigurationAttribute)obj).Configuration);
                //版权所有
                if (obj is AssemblyCopyrightAttribute)
                    jo.Add("copyright", ((AssemblyCopyrightAttribute)obj).Copyright);
                //公司名称（开发团队）
                if (obj is AssemblyCompanyAttribute)
                    jo.Add("company", ((AssemblyCompanyAttribute)obj).Company);
            }
            //发布时间
            DateTime lasttime = System.IO.File.GetLastWriteTime(dllfile);
            jo.Add("release", lasttime.ToString("yyyy-MM-dd HH:mm:ss"));
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
        #region 数据库相关
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
        /// 数据库类型，例如Sqlserver或PostgreSql
        /// </summary>
        /// <returns></returns>
        public string DataBaseType()
        {
            return Business.Do<ISystemPara>().DataBaseType();
        }
        /// <summary>
        /// 数据库的名称
        /// </summary>
        /// <returns></returns>
        public string DataBaseName()
        {
            return Business.Do<ISystemPara>().DataBaseName();
        }
        /// <summary>
        /// 数据库版本
        /// </summary>
        /// <returns></returns>
        public string DbVersion()
        {
            return Business.Do<ISystemPara>().DbVersion();
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
        #endregion
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
        /// 获取地址的gps坐标,已经弃用，可以用前端Js实现
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [HttpGet]
        public Dictionary<string, decimal> PositionGPS(string address)
        {
            WeiSha.Core.Param.Method.Position posi = WeiSha.Core.Request.Position(address);
            Dictionary<string, decimal> dic = new Dictionary<string, decimal>();
            dic.Add("lng", posi.Longitude);
            dic.Add("lat", posi.Latitude);
            return dic;
        }
        /// <summary>
        /// 通过IP地址计算位置
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public WeiSha.Core.Param.Method.Position PositionIP(string ip)
        {
            return new WeiSha.Core.Param.Method.Position(ip);
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
            jo.Add("course", Business.Do<ICourse>().CourseOfCount(orgid, -1, -1, null, null));
            //专业数
            jo.Add("subject", Business.Do<ISubject>().SubjectOfCount(orgid, -1, null, true));
            //试题数
            jo.Add("question", Business.Do<IQuestions>().QuesOfCount(orgid, -1, -1,-1,-1,-1, null));
            //学员数
            jo.Add("account", Business.Do<IAccounts>().AccountsOfCount(orgid, null, -1));
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
        /// 通过键值获取参数值
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>参数值</returns>
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
        /// 通过键值获取参数对象
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>参数对象</returns>
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
        /// <param name="entity">参数对象，如果Sys_Id大于零则按ID修改，否则按Sys_Key修改</param>
        /// <returns></returns>
        [HttpPost]
        [SuperAdmin]
        public SystemPara ParamModify(SystemPara entity)
        {
            Song.Entities.SystemPara old = null;
            if (entity.Sys_Id > 0) old = Business.Do<ISystemPara>().GetSingle(entity.Sys_Id);
            if (old == null && !string.IsNullOrWhiteSpace(entity.Sys_Key)) old = Business.Do<ISystemPara>().GetSingle(entity.Sys_Key);
            if (old != null)
            {
                old.Copy<Song.Entities.SystemPara>(entity);
                Business.Do<ISystemPara>().Save(old);
            }
            else
            {
                Business.Do<ISystemPara>().Add(entity);
            }
            return entity;
        }
        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [SuperAdmin]
        public SystemPara ParamAdd(SystemPara entity)
        {
            Business.Do<ISystemPara>().Add(entity);
            return entity;
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
            Business.Do<ISystemPara>().Delete(key);
            return true;
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
        [Upload]
        [Admin, Teacher]
        public JObject ExcelUpload()
        {
            //资源的虚拟路径和物理路径
            string pathKey = "Temp";
            string phyPath = WeiSha.Core.Upload.Get[pathKey].Physics;
            //文件存放在服务器的名称；Excel的文件名，Excel的路径
            string filename = string.Empty, excelname = string.Empty, excelurl = string.Empty, excelpath = string.Empty;

            foreach (string key in this.Files)
            {
                HttpPostedFileBase file = this.Files[key];
                //上传后保存的文件名
                filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                file.SaveAs(phyPath + filename);
                //如果是压缩包，则解压
                if (".zip".Equals(Path.GetExtension(filename), StringComparison.CurrentCultureIgnoreCase))
                {
                    //解压文件
                    WeiSha.Core.Compress.UnZipFile(phyPath + filename, true);
                    string undir = Path.Combine(phyPath, Path.GetFileNameWithoutExtension(filename));
                    string[] files = Directory.GetFiles(undir, "*.xls");
                    if (files.Length > 0) excelname = Path.GetFileName(files[0]);
                    else throw new Exception("没有Excel文档");
                    excelurl = WeiSha.Core.Upload.Get[pathKey].Virtual + Path.GetFileNameWithoutExtension(filename) + "/" + excelname;
                    excelpath = Path.Combine(phyPath, Path.GetFileNameWithoutExtension(filename), excelname);
                }
                else
                {
                    excelname = filename;
                    excelurl = WeiSha.Core.Upload.Get[pathKey].Virtual + filename;
                    excelpath = Path.Combine(phyPath, excelname);
                }
                break;
            }
            //工作簿
            JArray table = ViewData.Helper.Excel.Sheets(excelpath);
            JObject jo = new JObject();
            jo.Add("path", excelpath);    //excel文件的物理路径
            jo.Add("url", excelurl);     //excel文件的虚拟路径
            jo.Add("file", excelname);  //excel文件名
            jo.Add("sheets", table);    //工作簿列表
            return jo;
        }
        /// <summary>
        /// 获取工作薄的列表，即第一行的标题
        /// </summary>
        /// <param name="xlsUrl"></param>
        /// <param name="sheetIndex"></param>
        /// <returns>name:工作簿名称;index:工作簿索引;count:记录数;columns:列名 </returns>
        [HttpGet]
        public JObject ExcelSheetColumn(string xlsUrl, int sheetIndex)
        {
            string excel = WeiSha.Core.Server.MapPath(xlsUrl);
            return ViewData.Helper.Excel.Columns(excel, sheetIndex);
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
        /// <summary>
        /// 平台所有数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,int> DataDetails()
        {
            return WeiSha.Data.Gateway.TablesCount;
        }
        /// <summary>
        /// 数据记录的总数
        /// </summary>
        /// <returns></returns>
        public int DataTotal()
        {
            return WeiSha.Data.Gateway.Default.Total;
        }
        /// <summary>
        /// 数据库的初始时间
        /// </summary>
        /// <returns></returns>
        public DateTime DataBaseInitialDate()
        {
            DateTime? date= WeiSha.Core.Database.InitialDate;
            return (DateTime)date;
        }
    }
}
