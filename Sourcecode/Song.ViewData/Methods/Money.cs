using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiSha.Core;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using Newtonsoft.Json.Linq;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 资金流水
    /// </summary>
    [HttpGet]
    public class Money : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 根据ID查询资金流水
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>    
        public Song.Entities.MoneyAccount ForID(int id)
        {
            return Business.Do<IAccounts>().MoneySingle(id);
        }
        /// <summary>
        /// 计算资金总额
        /// </summary>
        /// <param name="acid">学员账号id</param>
        /// <param name="type">1支出，2收入（包括充值、分润等）</param>
        /// <param name="from">类型，来源，1为管理员操作，2为充值码充值；3这在线支付；4购买课程,5分润</param>
        /// <returns></returns>
        public decimal MoneySum(int acid, int type, int from)
        {
            decimal sum = Business.Do<IAccounts>().MoneySum(acid, type, from);
            return sum;
        }
        /// <summary>
        /// 增加或减去资金
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="money"></param>
        /// <param name="type"></param>
        /// <param name="remark"></param>
        /// <returns>当前学员的余额</returns>
        [HttpPost]
        [SuperAdmin]
        public decimal AddOrSubtract(int acid,double money,int type,string remark)
        {
            Song.Entities.Accounts st = Business.Do<IAccounts>().AccountsSingle(acid);
            if (st == null) throw new Exception("当前信息不存在！");
            //操作对象
            Song.Entities.MoneyAccount ma = new MoneyAccount();
            ma.Ma_Money = (decimal)money;
            ma.Ma_Total = st.Ac_Money; //当前资金总数
            ma.Ma_Remark = remark;
            ma.Ac_ID = acid;
            ma.Ma_Source = "系统管理员操作";
            //充值方式，管理员充值
            ma.Ma_From = 1;
            ma.Ma_IsSuccess = true;     //充值结果为“成功”
                                        //操作者
            Song.Entities.EmpAccount emp = LoginAdmin.Status.User(this.Letter);

            try
            {
                string mobi = !string.IsNullOrWhiteSpace(emp.Acc_MobileTel) && emp.Acc_AccName != emp.Acc_MobileTel ? emp.Acc_MobileTel : "";
                //如果是充值
                if (type == 2)
                {
                    ma.Ma_Info = string.Format("系统管理员{0}（{1}{2}）向您充值{3}元", emp.Acc_Name, emp.Acc_AccName, mobi, money);
                    Business.Do<IAccounts>().MoneyIncome(ma);
                }
                //如果是转出
                if (type == 1)
                {
                    ma.Ma_Info = string.Format("系统管理员{0}（{1}{2}）扣除您{3}元", emp.Acc_Name, emp.Acc_AccName, mobi, money);
                    Business.Do<IAccounts>().MoneyPay(ma);
                }
                //刷新登录状态的学员信息
                LoginAccount.Fresh(st);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ma.Ma_Total;
        }
        /// <summary>
        /// 删除资金流水
        /// </summary>
        /// <param name="id">可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete,HttpGet(Ignore =true)]
        public int Delete(string id)
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
                    Business.Do<IAccounts>().MoneyDelete(idval);
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
        /// 学员账户删除资金流水
        /// </summary>
        /// <param name="id">可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Student]
        [HttpDelete]
        public bool DeleteForAccount(int id)
        {
            try
            {
                Business.Do<IAccounts>().MoneyDelete(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 确认订单。
        /// 充值成功时系统默认会确认订单。偶尔网络原因导致扣款成功，但钱数没有增加时，用这个方法
        /// </summary>
        /// <param name="serial">流水号</param>
        /// <returns></returns>
        [HttpPost]
        [SuperAdmin]
        public bool ConfirmSerial(string serial)
        {
            try
            {
                Business.Do<IAccounts>().MoneyConfirm(serial);
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
        /// <param name="type">类型，1支出，2充值</param>
        /// <param name="from">来源，1管理员操作，3在线支付，4购买课程</param>
        /// <param name="account">学员账号</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="serial">流水号</param>
        /// <param name="moneymin">查询金额区间，起始金额</param>
        /// <param name="moneymax">查询金额区间，最大金额</param>
        /// <param name="state">状态，-1为所有，1为成功，2为失败</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult Pager(int type, int from, string account, DateTime? start, DateTime? end, string serial, int moneymin, int moneymax, int state, int size, int index)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            int count = 0;
            Song.Entities.MoneyAccount[] eas = null;
            eas = Business.Do<IAccounts>().MoneyPager(-1, type, from, account, (DateTime?)start, (DateTime?)end, moneymin, moneymax, serial, state, size, index, out count);
            ListResult result = new ListResult(eas);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="acid">学员账号id</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="type">类型，1支出，2充值</param>
        /// <param name="from">来源，1管理员操作，3在线支付，4购买课程</param>
        /// <param name="search"></param>
        /// <param name="state">状态，-1为所有，1为成功，2为失败</param> 
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult PagerForAccount(int acid, DateTime? start, DateTime? end, int type, int from, string search, int state, int size, int index)
        {
            if (acid <= 0)
            {
                Song.Entities.Accounts acc = this.User;
                if (acc != null)
                    acid = acc.Ac_ID;
            }
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            int count = 0;
            Song.Entities.MoneyAccount[] eas = null;
            eas = Business.Do<IAccounts>().MoneyPager(-1, acid, type, start, end, from, search, state, size, index, out count);
            ListResult result = new ListResult(eas);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        #region 导出
        private static string outputPath = "MoneyOutputToExcel";
        /// <summary>
        /// 生成excel
        /// </summary>
        /// <param name="type">类型，1支出，2充值</param>
        /// <param name="from">来源，1管理员操作，3在线支付，4购买课程<</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        public JObject ExcelOutput(int type, int from, DateTime? start, DateTime? end)
        {
            DateTime dts = start == null ? DateTime.MinValue : (DateTime)start;
            DateTime dte = end == null ? DateTime.MaxValue : (DateTime)end;
            //导出文件的位置
            string rootpath = Upload.Get["Temp"].Physics + outputPath + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);

            DateTime date = DateTime.Now;          
            string filename = string.Format("{0} to {1}.({2}).xls", dts.ToString("yyyy-MM-dd"), dte.ToString("yyyy-MM-dd"), date.ToString("yyyy-MM-dd hh-mm-ss"));
            string filePath = rootpath + filename;
            filePath = Business.Do<IAccounts>().MoneyRecords4Excel(filePath, type, from, start, end);
            JObject jo = new JObject();
            jo.Add("file", filename);
            jo.Add("url", Upload.Get["Temp"].Virtual + outputPath + "/" + filename);
            jo.Add("date", date);
            return jo;
        }
        /// <summary>
        /// 删除Excel文件
        /// </summary>
        /// <param name="filename">文件名，带后缀名，不带路径</param>
        /// <returns></returns>
        public bool ExcelDelete(string filename)
        {
            string rootpath = Upload.Get["Temp"].Physics + outputPath + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);
            string filePath = rootpath + filename;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 已经生成的Excel文件
        /// </summary>
        /// <returns>file:文件名,url:下载地址,date:创建时间</returns>
        public JArray ExcelFiles()
        {
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);
            JArray jarr = new JArray();
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(rootpath);
            foreach (System.IO.FileInfo f in dir.GetFiles("*.xls"))
            {
                JObject jo = new JObject();
                jo.Add("file", f.Name);
                jo.Add("url", Upload.Get["Temp"].Virtual + outputPath + "/" + f.Name);
                jo.Add("date", f.CreationTime);
                jo.Add("size", f.Length);
                jarr.Add(jo);
            }            
            return jarr;
        }
        #endregion
    }
}
