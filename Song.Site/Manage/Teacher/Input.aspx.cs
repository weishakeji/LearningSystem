using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;

using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
using System.Reflection;
using System.Collections.Generic;

namespace Song.Site.Manage.Teacher
{
    public partial class Input : Extend.CustomPage
    {
        
        //所有组
        Song.Entities.TeacherSort[] sorts = null;
        Song.Entities.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        

        protected void ExcelInput1_OnInput(object sender, EventArgs e)
        {
            //工作簿中的数据
            DataTable dt = ExcelInput1.SheetDataTable;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    //throw new Exception();
                    //将数据逐行导入数据库
                    _inputData(dt.Rows[i]);
                }
                catch
                {
                    //如果出错，将错误行返回给控件
                    ExcelInput1.AddError(dt.Rows[i]);
                }
            }
        }

        #region 导入数据
       
        /// <summary>
        /// 将某一行数据加入到数据库
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="dl"></param>
        private void _inputData(DataRow dr)
        {
            //取所有分类
            if (org == null) org = Business.Do<IOrganization>().OrganCurrent();           
            if (this.sorts == null)
                this.sorts = Business.Do<ITeacher>().SortCount(org.Org_ID, null, 0);
            Song.Entities.Accounts acc = null;
            Song.Entities.Teacher teacher = null;
            bool isExistAcc= false;   //是否存在该账号
            bool isExistTh = false;   //是否存在该教师
            foreach (KeyValuePair<String, String> rel in ExcelInput1.DataRelation)
            {
                //Excel的列的值
                string column = dr[rel.Key].ToString();
                //数据库字段的名称
                string field = rel.Value;
                if (field == "Th_PhoneMobi")
                {
                    acc = Business.Do<IAccounts>().AccountsSingle(column, -1);                    
                    if (acc != null)                    
                        teacher = Business.Do<IAccounts>().GetTeacher(acc.Ac_ID, null);
                    isExistAcc = acc != null;
                    isExistTh = teacher != null;
                    continue;
                }
            }
            if (acc == null) acc = new Entities.Accounts();
            if (teacher == null) teacher = new Entities.Teacher();
            foreach (KeyValuePair<String, String> rel in ExcelInput1.DataRelation)
            {
                //Excel的列的值
                string column = dr[rel.Key].ToString();
                //数据库字段的名称
                string field = rel.Value;
                if (field == "Th_Sex")
                {
                    teacher.Th_Sex = (short)(column == "男" ? 1 : 2);
                    continue;
                }
                PropertyInfo[] properties = teacher.GetType().GetProperties();
                for (int j = 0; j < properties.Length; j++)
                {
                    PropertyInfo pi = properties[j];
                    if (field == pi.Name && !string.IsNullOrEmpty(column))
                    {
                        pi.SetValue(teacher, Convert.ChangeType(column,pi.PropertyType), null);                        
                    }
                }               
            }
            //设置分组
            if (!string.IsNullOrWhiteSpace(teacher.Ths_Name)) teacher.Ths_ID = _getDepartId(sorts, teacher.Ths_Name);
            if (!string.IsNullOrWhiteSpace(teacher.Th_Pw)) teacher.Th_Pw = teacher.Th_Pw.Trim();
            acc.Org_ID = teacher.Org_ID = org.Org_ID;
            acc.Ac_Name = teacher.Th_Name;
            teacher.Org_Name = org.Org_Name;
            teacher.Th_AccName = teacher.Th_PhoneMobi;
            acc.Ac_IsPass = teacher.Th_IsPass = true;
            teacher.Th_IsShow = true;
            acc.Ac_IsUse = teacher.Th_IsUse = true;           
            //如果账号不存在
            if (!isExistAcc)
            {
                acc.Ac_AccName = acc.Ac_MobiTel1 = acc.Ac_MobiTel2 = teacher.Th_PhoneMobi;  //账号手机号
                acc.Ac_Pw = new WeiSha.Common.Param.Method.ConvertToAnyValue(teacher.Th_Pw).MD5;    //密码                
                acc.Ac_Sex = teacher.Th_Sex;        //性别
                acc.Ac_Birthday = teacher.Th_Birthday;
                acc.Ac_Qq = teacher.Th_Qq;
                acc.Ac_Email = teacher.Th_Email;
                acc.Ac_IDCardNumber = teacher.Th_IDCardNumber;  //身份证    
                acc.Ac_IsTeacher = true;        //该账号有教师身份
                //保存
                teacher.Ac_ID = Business.Do<IAccounts>().AccountsAdd(acc);
                Business.Do<ITeacher>().TeacherSave(teacher);
            }
            else
            {
                acc.Ac_IsTeacher = true;
                teacher.Ac_ID = acc.Ac_ID;                
                //如果账号存在,但教师不存在
                if (!isExistTh)
                {
                    Business.Do<ITeacher>().TeacherAdd(teacher);
                }
                else
                {
                    teacher.Th_Pw = new WeiSha.Common.Param.Method.ConvertToAnyValue(teacher.Th_Pw).MD5;    //密码
                    Business.Do<ITeacher>().TeacherSave(teacher);
                }
            }
        }
        /// <summary>
        /// 获取分组id
        /// </summary>
        /// <param name="sorts"></param>
        /// <param name="departName"></param>
        /// <returns></returns>
        private int _getDepartId(Song.Entities.TeacherSort[] sorts, string sortName)
        {
            try
            {
                int sortId = 0;
                foreach (Song.Entities.TeacherSort s in sorts)
                {
                    if (sortName.Trim() == s.Ths_Name)
                    {
                        sortId = s.Ths_ID;
                        break;
                    }
                }
                if (sortId == 0 && sortName.Trim() != "")
                {
                    int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
                    Song.Entities.TeacherSort nwsort = new Song.Entities.TeacherSort();
                    nwsort.Ths_Name = sortName;
                    nwsort.Ths_IsUse = true;
                    nwsort.Org_ID = orgid;
                    Business.Do<ITeacher>().SortAdd(nwsort);
                    sortId = nwsort.Ths_ID;
                    this.sorts = this.sorts = Business.Do<ITeacher>().SortCount(orgid, null, 0);
                }
                return sortId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
        #endregion
      
    }
}
