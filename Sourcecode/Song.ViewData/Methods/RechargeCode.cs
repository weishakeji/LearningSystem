using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
//using System.Web.Mvc;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;


namespace Song.ViewData.Methods
{
    /// <summary>
    /// 充值码
    /// </summary>
    [HttpPut, HttpGet]
    public class RechargeCode : ViewMethod, IViewAPI
    {
        #region 卡片设置项
        /// <summary>
        /// 充值卡的最小长度
        /// </summary>
        /// <returns></returns>
        public int MinLength()
        {
            return Business.Do<IRecharge>().MinLength();
        }
        /// <summary>
        /// 根据ID获取充值卡
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Song.Entities.RechargeSet SetForID(int id)
        {
            return Business.Do<IRecharge>().RechargeSetSingle(id);
        }
        /// <summary>
        /// 判断学习卡名称是否重复
        /// </summary>
        /// <param name="name">学习卡名称</param>
        /// <param name="id">学习卡id</param>
        /// <returns></returns>
        public bool SetExist(string name, int id)
        {
            return Business.Do<IRecharge>().RechargeSetIsExist(name, id);
        }
        /// <summary>
        /// 修改学习卡设置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public bool SetModify(Song.Entities.RechargeSet entity)
        {
            int min_len = entity.Rs_Count.ToString().Length + 1 + this.MinLength();
            if (entity.Rs_CodeLength < min_len) throw new Exception("充值卡卡号长度不得小于" + min_len);

            Song.Entities.RechargeSet old = Business.Do<IRecharge>().RechargeSetSingle(entity.Rs_ID);
            if (old == null) throw new Exception("Not found entity");
            old.Copy<Song.Entities.RechargeSet>(entity);
            Business.Do<IRecharge>().RechargeSetSave(old);
            return true;
        }
        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id">id，可以是多个，用逗号分隔</param>
        /// <returns>返回删除的个数</returns>
        [HttpPost(Ignore = true), HttpDelete, Admin]
        public int SetDelete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            string[] arr = id.Split(',');
            foreach (string s in arr)
            {
                int idval = 0;
                int.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    Business.Do<IRecharge>().RechargeSetDelete(idval);
                    i++;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return i;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public bool SetAdd(Song.Entities.RechargeSet entity)
        {
            int min_len = entity.Rs_Count.ToString().Length + 1 + this.MinLength();
            if (entity.Rs_CodeLength < min_len) throw new Exception("充值卡卡号长度不得小于" + min_len);

            try
            {            
                Business.Do<IRecharge>().RechargeSetAdd(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="search">检索字符，按课程名称</param>
        /// <param name="size">每页几条</param>
        /// <param name="index">第几页</param>
        /// <returns></returns>
        public ListResult SetPager(int orgid, string search, int size, int index)
        {
            int count = 0;         
            Song.Entities.RechargeSet[] eas = null;
            eas = Business.Do<IRecharge>().RechargeSetPager(orgid, null, search, size, index, out count);          
            ListResult result = new ListResult(eas);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 充值设置项的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns>total总数，used使用,disable禁用的</returns>
        public JObject SetDataInfo(int id)
        {
            Song.Entities.RechargeSet set = Business.Do<IRecharge>().RechargeSetSingle(id);
            //卡总数
            int count = set.Rs_Count;
            int disable = Business.Do<IRecharge>().RechargeCodeOfCount(-1, id, false, null);
            JObject jo = new JObject();
            jo.Add("total", count);
            jo.Add("used", set.Rs_UsedCount);      
            jo.Add("disable", disable);
            return jo;
        }
        #endregion

        #region 充值相关
        /// <summary>
        /// 充值码卡号列表，分页获取
        /// </summary>
        /// <param name="rsid">充值码设置项的id</param>
        /// <param name="isused">是否使用过，false为全部，true为使用过的</param>
        /// <param name="isdisable">是否禁用过，false为全部，true为启用的</param>
        /// <param name="code"></param>
        /// <param name="account">学员账号</param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ListResult CodePager(int rsid, bool isused, bool isdisable,string code, string account, int index, int size)
        {
            //总记录数
            int count = 0;
            Song.Entities.RechargeCode[] eas = null;
            bool? isUsed = isused ? (bool?)true : null;       
            bool? isEnable = !isdisable ? null : (bool?)false;
            eas = Business.Do<IRecharge>().RechargeCodePager(-1, rsid, code, account, isEnable, isUsed, size, index, out count);
            Song.ViewData.ListResult result = new ListResult(eas);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 某个设置项下的所有充值码
        /// </summary>
        /// <param name="rsid">充值码设置项的id</param>
        /// <returns></returns>
        public Song.Entities.RechargeCode[] Codes(int rsid)
        {
            Song.Entities.RechargeCode[] cards = Business.Do<IRecharge>().RechargeCodeCount(-1, rsid, null, null, -1);
            return cards;
        }
        
        /// <summary>
        /// 更改充值码启用状态
        /// </summary>
        /// <param name="code"></param>
        /// <param name="pw"></param>
        /// <param name="isenable"></param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public bool CodeChangeEnable(string code, string pw, bool isenable)
        {
            Song.Entities.RechargeCode card = Business.Do<IRecharge>().RechargeCodeSingle(code, pw);
            if(card==null) throw new Exception("未查询到充值码，卡号或密码错误");
            try
            {
                card.Rc_IsEnable = isenable;
                Business.Do<IRecharge>().RechargeCodeSave(card);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 导出

        private static string outputPath = "RechargeCodeToExcel";
        /// <summary>
        /// 生成导出的Excel文件
        /// </summary>
        /// <param name="id">充值码设置项id</param>
        /// <returns>name:充值码主题,file:文件名,url:下载地址,date:创建时间</returns>
        public JObject ExcelOutput(int id)
        {
            //导出文件的位置
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);

            Song.Entities.RechargeSet set = Business.Do<IRecharge>().RechargeSetSingle(id);
            DateTime date = DateTime.Now;         
            //创建文件
            string filename = string.Format("充值码[{0}]（{1}）.xls", set.Rs_ID, DateTime.Now.ToString("yyyy-MM-dd hh-mm"));
            string filePath = rootpath + filename;
            filePath = Business.Do<IRecharge>().RechargeCode4Excel(filePath, -1, id);
            JObject jo = new JObject();
            jo.Add("name", set.Rs_Theme);
            jo.Add("file", filename);
            jo.Add("url", WeiSha.Core.Upload.Get["Temp"].Virtual + outputPath + "/" + filename);
            jo.Add("date", date);
            return jo;
        }
        /// <summary>
        /// 删除Excel文件
        /// </summary>
        /// <param name="filename">文件名，带后缀名，不带路径</param>
        /// <returns></returns>
        [HttpDelete]
        [HttpGet(Ignore=true)]
        public bool ExcelDelete(string filename)
        {
            return Song.ViewData.Helper.Excel.DeleteFile(filename, outputPath, "Temp");
        }
        /// <summary>
        /// 已经生成的Excel文件
        /// </summary>
        /// <param name="id">充值码设置项id</param>
        /// <returns>name:充值码主题,file:文件名,url:下载地址,date:创建时间</returns>
        public JArray ExcelFiles(int id)
        {
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath + "\\";
            JArray jarr = new JArray();
            if (!System.IO.Directory.Exists(rootpath))return jarr;
            
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(rootpath);
            FileInfo[] files = dir.GetFiles("*.xls").OrderByDescending(f => f.CreationTime).ToArray();
            foreach (System.IO.FileInfo f in files)
            {
                //if (f.Name.IndexOf("-") < 0) continue;
                //if (f.Name.IndexOf("-") == f.Name.LastIndexOf("-")) continue;
                //if (f.Name.Substring(f.Name.IndexOf("-"), f.Name.LastIndexOf("-")) != id.ToString()) continue;
                Regex regex = new Regex(@"\[(\d+)\]");
                Match match = regex.Match(f.Name);
                if (!match.Success) continue;
                if (match.Groups[1].Value != id.ToString()) continue;

                JObject jo = new JObject();
                jo.Add("file", f.Name);
                jo.Add("url", WeiSha.Core.Upload.Get["Temp"].Virtual + outputPath + "/" + f.Name);
                jo.Add("date", f.CreationTime);
                jo.Add("size", f.Length);
                jarr.Add(jo);
            }
            return jarr;
        }
        #endregion

       
    }
}
