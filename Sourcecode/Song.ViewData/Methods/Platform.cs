using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
        [Cache(AdminDisable = true, Expires = 120)]
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
        public JObject Version() => Business.Do<ISystemPara>().VersionDetail();
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
            jo.Add("course", Business.Do<ICourse>().CourseOfCount(orgid, -1, -1, null, null, null));
            //专业数
            jo.Add("subject", Business.Do<ISubject>().SubjectOfCount(orgid, -1, null, true));
            //试题数
            jo.Add("question", Business.Do<IQuestions>().Total(orgid, -1, -1, -1, -1, -1, null, false));
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

        #region 其它
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
        #endregion

        #region 生成验证码
        /// <summary>
        /// 生成图片验证码
        /// </summary>
        /// <param name="leng">验证码长度</param>
        /// <param name="acc">账号</param>
        /// <returns>返回类为json类型，value:加密后的校检码，验证时提交到服务端；base64：图片的base64编码</returns>
        [HttpPost]
        public Dictionary<string, string> CheckCodeImg(int leng, string acc)
        {
            return CodeImg(leng, 1, acc);
        }
        /// <summary>
        /// 生成图片验证码
        /// </summary>
        /// <param name="leng">长度</param>
        /// <param name="type">类型:0为数字与大小写字母，1为纯数字，2为纯小字母，3为纯大写字母，4为大小写字母，5数字加小写，6数字加大写</param>
        /// <param name="acc"></param>
        /// <returns></returns>
        [HttpPost]
        public Dictionary<string, string> CodeImg(int leng, int type, string acc)
        {
            if (leng <= 0) throw new Exception("长度不得小于等于零");
            //设定生成几位随机数
            string tmp = RndNum(leng, type);
            string val = ViewData.Helper.ConvertToAnyValue.Create(acc + tmp).MD5;
            //生成图片
            System.Drawing.Bitmap image = CreateImage(tmp);
            string base64 = WeiSha.Core.Images.ImageTo.ToBase64(image);
            //
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("value", val);
            dic.Add("base64", "data:image/Gif;base64," + base64);
            return dic;
        }
        /// <summary>
        /// 生成随机字符串，可以选择长度与类型
        /// </summary>
        /// <param name="VcodeNum">随机字符串的长度</param>
        /// <param name="type">生成的随机数类型，0为数字与大小写字母，1为纯数字，2为纯小字母，3为纯大写字母，4为大小写字母，5数字加小写，6数字加大写，</param>
        /// <returns></returns>
        private string RndNum(int VcodeNum, int type)
        {
            string Vchar;
            //数字串
            string num = "0,1,2,3,4,5,6,7,8,9,";
            //小写字母
            string lower = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
            //大写字母
            string upper = lower.ToUpper();
            switch (type)
            {
                case 0:
                    Vchar = num + lower + upper;
                    break;
                case 1:
                    Vchar = num;
                    break;
                case 2:
                    Vchar = lower;
                    break;
                case 3:
                    Vchar = upper;
                    break;
                case 4:
                    Vchar = upper + lower;
                    break;
                case 5:
                    Vchar = num + lower;
                    break;
                case 6:
                    Vchar = num + upper;
                    break;
                default:
                    Vchar = num + lower + upper;
                    break;
            }
            Vchar = Vchar.Substring(0, Vchar.Length - 1);
            string[] VcArray = Vchar.Split(new Char[] { ',' });
            string VNum = "";
            Random rand = new Random();
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                VNum += VcArray[rand.Next(VcArray.Length)];
            }
            return VNum;
        }
        /// <summary>
        /// 生成验证码的图片，将字符串填充到图片，并输出到数据流
        /// </summary>
        /// <param name="checkCode">需要生成图片的字符串</param>
        private System.Drawing.Bitmap CreateImage(string checkCode)
        {
            int iwidth = (int)(checkCode.Length * 13);
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(iwidth, 18);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            //定义颜色
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            Random rand = new Random();
            //随机输出噪点

            for (int i = 0; i < 20; i++)
            {
                int x = rand.Next(image.Width);
                int y = rand.Next(image.Height);
                g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 1, 1);
            }

            //输出不同字体和颜色的验证码字符
            for (int i = 0; i < checkCode.Length; i++)
            {
                int cindex = rand.Next(c.Length);
                int findex = rand.Next(font.Length);

                System.Drawing.Font f = new System.Drawing.Font(font[findex], 12, System.Drawing.FontStyle.Bold);
                Brush b = new System.Drawing.SolidBrush(c[cindex]);
                //字符上下位置不同
                int ii = 0;
                if ((i + 1) % 2 == 0)
                {
                    ii = 1;
                }
                g.DrawString(checkCode.Substring(i, 1), f, b, 1 + (i * 13), ii);
            }
            //画一个边框
            //g.DrawRectangle(new Pen(Color.Black,0),0,0,image.Width-1,image.Height-1);

            //输出到浏览器
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            g.Dispose();
            //image.Dispose();
            return image;
        }
        #endregion

        #region 获取地理信息

        /// <summary>
        /// 通过经纬度，获取地理信息
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns>Province省份，City城市，District县区，Street街道</returns>
        public JObject Position(string lng, string lat)
        {
            //解析地址
            WeiSha.Core.Param.Method.Position posi = WeiSha.Core.Request.Position(lng, lat);
            JObject jo = new JObject();
            jo.Add("Province", posi.Province);
            jo.Add("City", posi.City);
            jo.Add("District", posi.District);
            jo.Add("Street", posi.Street);
            return jo;
        }
        #endregion
    }
}
