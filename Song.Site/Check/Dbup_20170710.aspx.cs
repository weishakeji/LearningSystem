using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using System.Data;


namespace Song.Site.Check
{
    public partial class Dbup_20170710 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                int size = WeiSha.Common.Request.Form["size"].Int32 ?? 10;  //每页多少条
                int index = WeiSha.Common.Request.Form["index"].Int32 ?? 1;  //第几页
                ////取账户个数，如果个数大于1，则不再执行升级
                //int acc_conut = Business.Do<IAccounts>().AccountsOfCount(-1, null);
                //if (acc_conut > 0)
                //{
                //    Response.Write("{'success':'-1'}");
                //    Response.End();
                //    return;
                //}
                try
                {
                    //复制教师信息
                    if (index == 1)
                    {
                        Song.Entities.Teacher[] teachers = Business.Do<ITeacher>().TeacherCount(0, null, 0);
                        foreach (Song.Entities.Teacher t in teachers) copyTeacher(t);
                    }
                    //总共多少个学员记录
                    int sumcount = 0;
                    object count = Business.Do<ISystemPara>().ScalarSql("SELECT COUNT(*) as 'count'  FROM [Student]");
                    int.TryParse(count.ToString(), out sumcount);
                    //开始处理学员
                    string sql = @"select * from (
                                select *,row_number() over(order by st_id asc) rn  
                                from Student) a
                                where rn> {1}*({0}-1) and rn<= {0}*{1}";
                    sql = string.Format(sql, index, size);
                    sql = sql.Replace("\n", "").Replace("\r", "");
                    DataTable dt = Business.Do<ISystemPara>().ForSql(sql);
                    foreach (DataRow dr in dt.Rows)
                    {
                        copyStudent(dr);
                    }
                    string json = "{'success':'1','size':'" + size + "','index':'" + index + "','sumcount':'" + sumcount + "'}";
                    Response.Write(json);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    msg = msg.Replace("'", "  ").Replace("\n", "").Replace("\r", "");
                    
                    string json = "{'success':'0','msg':'" + msg + "'}";
                    Response.Write(json);
                }
                Response.End();
            }
        }
        /// <summary>
        /// 复制教师信息
        /// </summary>
        private void copyTeacher(Song.Entities.Teacher th)
        {
            bool isExist = false;
            Song.Entities.Accounts acc = Business.Do<IAccounts>().IsAccountsExist(th.Th_AccName);
            isExist = acc != null;
            //如果在整个系统都不存在
            if (!isExist)
            {
                acc = new Song.Entities.Accounts();
                acc.Ac_AccName = th.Th_AccName;
                acc.Ac_UID = WeiSha.Common.Request.UniqueID();
            }
            else
            {
                //如果存在，但在当前机构不存在
                acc = Business.Do<IAccounts>().IsAccountsExist(th.Org_ID, th.Th_AccName);
                isExist = acc != null;
                if (!isExist)
                {
                    Song.Entities.Organization org = Business.Do<IOrganization>().OrganSingle(th.Org_ID);
                    if (org == null) org = Business.Do<IOrganization>().OrganDefault();
                    acc = new Song.Entities.Accounts();
                    acc.Ac_AccName = th.Th_AccName + "-" + org.Org_TwoDomain;
                    acc.Ac_UID = WeiSha.Common.Request.UniqueID();
                }
            }
            acc.Ac_IsTeacher = true;
            acc.Ac_Pw = th.Th_Pw;
            acc.Ac_Qus = th.Th_Qus;
            acc.Ac_Ans = th.Th_Anwser;
            acc.Ac_Name = th.Th_Name;
            acc.Ac_Pinyin = th.Th_Pinyin;
            acc.Ac_Age = th.Th_Age;
            acc.Ac_Birthday = th.Th_Birthday;
            acc.Ac_IDCardNumber = th.Th_IDCardNumber;
            acc.Ac_Sex = th.Th_Sex;
            acc.Ac_Tel = th.Th_Phone;
            acc.Ac_IsOpenTel = th.Th_IsOpenPhone;
            acc.Ac_MobiTel1 = acc.Ac_MobiTel2 = th.Th_PhoneMobi;
            acc.Ac_Email = th.Th_Email;
            acc.Ac_Qq = th.Th_Qq;
            acc.Ac_Weixin = th.Th_Weixin;
            acc.Ac_RegTime = th.Th_RegTime;
            acc.Ac_IsUse = th.Th_IsUse;
            acc.Ac_IsPass = th.Th_IsPass;
            acc.Org_ID = th.Org_ID;
            //移动照片
            if (!string.IsNullOrWhiteSpace(th.Th_Photo))
            {
                string photo = Upload.Get["Teacher"].Physics + th.Th_Photo;
                System.IO.FileInfo fi = new System.IO.FileInfo(photo);
                if (fi.Exists)
                {
                    fi.CopyTo(Upload.Get["Accounts"].Physics + th.Th_Photo, true);
                }
                acc.Ac_Photo = th.Th_Photo;
            }
            if (!isExist) Business.Do<IAccounts>().AccountsAdd(acc);
            else
                Business.Do<IAccounts>().AccountsSave(acc);
            th.Ac_ID = acc.Ac_ID;
            th.Ac_UID = acc.Ac_UID;
            Business.Do<ITeacher>().TeacherSave(th);
        }
        /// <summary>
        /// 复制学员信息
        /// </summary>
        /// <param name="st"></param>
        private void copyStudent(DataRow st)
        {
            bool isExist = false;
            Song.Entities.Accounts acc = Business.Do<IAccounts>().IsAccountsExist(st["St_AccName"].ToString());
            isExist = acc != null;
            //如果在整个系统都不存在
            if (!isExist)
            {
                acc = new Song.Entities.Accounts();
                acc.Ac_AccName = st["St_AccName"].ToString();
                acc.Ac_UID = WeiSha.Common.Request.UniqueID();
            }
            else
            {
                //如果存在，但在当前机构不存在
                acc = Business.Do<IAccounts>().IsAccountsExist(this._convert_int(st["Org_ID"].ToString(), 0), st["St_AccName"].ToString());
                isExist = acc != null;
                if (!isExist)
                {
                    Song.Entities.Organization org = Business.Do<IOrganization>().OrganSingle(this._convert_int(st["Org_ID"].ToString(),0));
                    if (org == null) org = Business.Do<IOrganization>().OrganDefault();
                    acc = new Song.Entities.Accounts();
                    acc.Ac_AccName = st["St_AccName"].ToString() + "-" + org.Org_TwoDomain;
                    acc.Ac_UID = WeiSha.Common.Request.UniqueID();
                }
            }
            acc.Ac_Pw = st["St_Pw"].ToString();
            acc.Ac_Qus = st["St_Qus"].ToString();
            acc.Ac_Ans = st["St_Anwser"].ToString();
            acc.Ac_Name = st["St_Name"].ToString();
            acc.Ac_Pinyin = st["St_Pinyin"].ToString();
            acc.Ac_Age = _convert_int(st["St_Age"].ToString(),0);
            acc.Ac_Birthday = _convert_datetime(st["St_Birthday"].ToString(), DateTime.Now);
            acc.Ac_IDCardNumber = st["St_IDCardNumber"].ToString();
            acc.Ac_Sex = _convert_int(st["St_Sex"].ToString(),0);
            acc.Ac_Tel = st["St_Phone"].ToString();
            acc.Ac_IsOpenTel = _convert_bool(st["St_IsOpenPhone"].ToString(),false);
            acc.Ac_MobiTel1 = st["St_PhoneMobi"].ToString();
            acc.Ac_IsOpenMobile = _convert_bool(st["St_IsOpenMobi"].ToString(), false);
            acc.Ac_Email = st["St_Email"].ToString();
            acc.Ac_Qq = st["St_Qq"].ToString();
            acc.Ac_Weixin = st["St_Weixin"].ToString();
            acc.Ac_RegTime = _convert_datetime(st["St_RegTime"].ToString(), DateTime.Now);
            acc.Ac_IsUse = _convert_bool(st["St_IsUse"].ToString(), true);
            acc.Ac_IsPass = _convert_bool(st["St_IsPass"].ToString(),true);
            acc.Org_ID = _convert_int(st["Org_ID"].ToString(),0);
            acc.Ac_Signature = st["St_Signature"].ToString();
            acc.Ac_Photo = st["St_Photo"].ToString();
            acc.Ac_Money = _convert_decimal(st["St_Money"].ToString(), 0);
            acc.Ac_Coupon = 0;
            acc.Ac_Point = 0;
            acc.Ac_LastTime = _convert_datetime(st["St_LastTime"].ToString(), DateTime.Now);
            acc.Ac_LastIP = st["St_LastIP"].ToString();           
            acc.Ac_Email = st["St_Email"].ToString();
            acc.Ac_CheckUID = st["St_CheckUID"].ToString();
            acc.Ac_Zip = st["St_Zip"].ToString();
            acc.Ac_LinkMan = st["St_LinkMan"].ToString();
            acc.Ac_LinkManPhone = st["St_LinkManPhone"].ToString();
            acc.Ac_Intro = st["St_Intro"].ToString();
            acc.Ac_Major = st["St_Major"].ToString();
            acc.Ac_Education = st["St_Education"].ToString();
            acc.Ac_Native = st["St_Native"].ToString();
            acc.Ac_Nation = st["St_Nation"].ToString();
            acc.Ac_CodeNumber = st["St_CodeNumber"].ToString();
            acc.Ac_Address = st["St_Address"].ToString();
            acc.Ac_AddrContact = st["St_AddrContact"].ToString();

            acc.Dep_Id = _convert_int(st["Dep_Id"].ToString(),0);
            acc.Sts_ID = _convert_int(st["Sts_ID"].ToString(),0);
            acc.Sts_Name = st["Sts_Name"].ToString();
            acc.Ac_CurrCourse = _convert_int(st["St_CurrCourse"].ToString(),0);
            //
            if (!isExist) Business.Do<IAccounts>().AccountsAdd(acc);
            else
                Business.Do<IAccounts>().AccountsSave(acc);
            //修正相关表
            string tables = @"Forum,TestResults,Student_Ques,Student_Notes,Student_Course,Student_Collect,RechargeCode,MoneyAccount,MessageBoard,Message,LogForStudentStudy,LogForStudentOnline,ExamResults";
            string update = "update {0} set Ac_ID={2} where Ac_ID={1}";
            int stid=_convert_int(st["St_ID"].ToString(),0);
            foreach (string t in tables.Split(','))
            {
                if (string.IsNullOrWhiteSpace(t)) continue;
                Business.Do<ISystemPara>().ExecuteSql(string.Format(update, t, stid, acc.Ac_ID));
            }
        }

        /// <summary>
        /// 字符串转布尔值
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool _convert_bool(string s, bool def)
        {
            bool b = def;
            bool.TryParse(s, out b);
            return b;           
        }
        private int _convert_int(string s, int def)
        {
            int b = def;
            int.TryParse(s, out b);
            return b;
        }
        private DateTime _convert_datetime(string s, DateTime def)
        {
            DateTime b = def;
            DateTime.TryParse(s, out b);
            return b;
        }
        private Decimal _convert_decimal(string s, Decimal def)
        {
            Decimal b = def;
            Decimal.TryParse(s, out b);
            return b;
        }
    }
}